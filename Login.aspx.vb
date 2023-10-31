Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.Cn_Login
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data
Imports System.Text
Imports System.Web.Security
Imports System.Configuration
Imports System.Net.HttpListenerRequest

Partial Class Login
    Inherits BasePage
    Dim Veces As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If IsPostBack Then Exit Sub
        Session("ModuloVersion") = "1.0.0.0"
        Session("ModuloClave") = "33"

        Session("EstacioN") = ""
        Session("EstacionIP") = ""
        Session("EstacionMac") = ""
        Session("Fecha") = Now.Date.ToShortDateString
        Session("Hora") = Now.ToString("HH:mm:ss")
        Session("Mensajes") = ""
        Session("InsumosAgregados") = ""
        Session("Sitio") = ""

        Dim dt_Sucursales As New DataTable
        dt_Sucursales.Columns.Add("value")
        dt_Sucursales.Columns.Add("display")
        dt_Sucursales.Rows.Add("0", "Seleccione...")

        'se conecta a la central para cargar las sucursales propias
        Session("ConexionCentral") = ConfigurationManager.ConnectionStrings("ConexionCentral").ConnectionString

        'Traemos las cadenas de conex de las cursales Propias
        Dim dt_SucursalesPro As DataTable = cn.fn_Consulta_sucursalesPropias

        If dt_SucursalesPro IsNot Nothing Then
            For Each fila As DataRow In dt_SucursalesPro.Rows
                Dim Valor As String = fila("Servidor") & "," & fila("Base Datos") & "," & fila("Usuario") & "," & fila("Clave") & "," & fila("Sucursal Id") & "," & fila("Clave SP")
                Dim Visualiza As String = fila("Nombre")
                dt_Sucursales.Rows.Add(Valor, Visualiza)
            Next
            ddl_Sucursal.DataSource = dt_Sucursales
            ddl_Sucursal.DataBind()
        End If

    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Aceptar.Click
        Call Validar_Usuario()
    End Sub

    Private Function Validar_Usuario() As Boolean

        Dim UsuarioID As String = Request.Form("usuarioID").Trim
        Dim Contraseña As String = Request.Form("password").Trim

        If ddl_Sucursal.SelectedValue = "0" Then
            fn_Alerta("Indique un sitio a conectarse")
            Return False
        End If

        If UsuarioID.Trim = "" Then
            fn_Alerta("Capture un Id de usuario.")
            Return False
        End If

        If Not IsNumeric(UsuarioID) Then
            fn_Alerta("Solo deben ser numeros en ID Usuario")
            Return False
        End If

        If Contraseña.Trim = "" Then
            fn_Alerta("Ingrese su Contraseña.")
            Return False
        End If

        If Len(Contraseña) < 4 Then
            fn_Alerta("Contraseña Incorrecta.")
            Return False
        End If

        'If Validar_Captcha() = False Then
        '    fn_Alerta("NO ENVIADO, Válida que eres humano.")
        '    Return False
        'End If

        Dim cadConex() As String = Split(ddl_Sucursal.SelectedValue, ",")
        cadConex(0) = BasePage.fn_Decode(cadConex(0))
        cadConex(1) = BasePage.fn_Decode(cadConex(1))
        cadConex(2) = BasePage.fn_Decode(cadConex(2))
        cadConex(3) = BasePage.fn_Decode(cadConex(3))

        'Ojo:Esta conexionLocal es la que se maneja en el cn_datos
        Session("Sitio") = "Data Source=" & cadConex(0) & "; Initial Catalog=" & cadConex(1) & ";User ID=" & cadConex(2) & ";Password=" & cadConex(3) & ";"

        '»»»»»»»»»» VALIDACION DE USUARIO«««««««««

        Dim dt_UsuariosLogin As DataTable = Usuarios_Login(CInt(UsuarioID))

        If dt_UsuariosLogin Is Nothing Then
            fn_Alerta("Ocurrió un error al consultar datos del usuario.")
            Return False
        End If

        If dt_UsuariosLogin.Rows.Count = 0 Then
            fn_Alerta("Usuario no encontrado.")
            Return False
        End If

        '1.- Validar Status del Usuario

        If dt_UsuariosLogin.Rows(0)("Status") <> "A" Then
            dt_UsuariosLogin.Dispose()
            fn_Alerta("Usuario Bloqueado. Imposible entrar al Sistema. Consulte al Administrador.")
            Return False
        End If

        '2.- Validar la Contraseña capturada
        Dim ContraHash As String = fn_EncryptToSHA1(Contraseña)
        If dt_UsuariosLogin.Rows(0)("Password") <> ContraHash Then
            fn_Alerta("Usuario ó Contraseña incorrecta.")
            Return False
        End If

        '3.- Verificar fecha Expira de contraseña
        If dt_UsuariosLogin.Rows(0)("Dias_Expira") < 1 Then

            'con esta linea no redirige cuando pones iconos en frm_login o en la master
            'pero sí entra si cancelas cambio de contraseña
            FormsAuthentication.SetAuthCookie(UsuarioID, False) 'NUEVO 16/01/2017

            Session("UsuarioID") = UsuarioID
            Session("RequiereCambioContraseña") = "SI"
            dt_UsuariosLogin.Dispose()
            'Abrir form Cambiar Contraseña
            Response.Redirect("LoginContra.aspx", False)
            Return False
        End If

        '4.- Si la contraseña es correcta y NO expirada
        Try
            Dim HOST_Nombre As String = System.Net.Dns.GetHostEntry(Request.ServerVariables("REMOTE_HOST")).HostName
            Session("EstacioN") = HOST_Nombre
        Catch ex As Exception
            Session("EstacioN") = "NOMBRE DE HOST NO RESUELTO"
            fn_Alerta("Ocurrio el siguiente error: " & ex.Message)
        End Try

        Try
            Dim HOST_IP = System.Net.Dns.GetHostEntry(Request.ServerVariables("REMOTE_ADDR")).AddressList
            For index As Integer = 0 To HOST_IP.Length - 1
                If HOST_IP(index).ToString.Length > 7 And HOST_IP(index).ToString.Length < 16 Then
                    Session("EstacionIP") = HOST_IP(index).ToString
                    Exit For
                End If
            Next index
        Catch ex As Exception
            Session("EstacionIP") = "IP NO RESUELTO"
            fn_Alerta("Ocurrio el siguiente error: " & ex.Message)
        End Try

        Session("Dpto_Reclutamiento") = dt_UsuariosLogin.Rows(0).Item("Dpto_Reclutamiento")
        Session("UsuarioID") = CInt(UsuarioID)
        Session("SucursalID") = dt_UsuariosLogin.Rows(0).Item("Id_Sucursal")
        Session("EmpresaID") = dt_UsuariosLogin.Rows(0).Item("Id_Empresa")
        Session("SucursalN") = dt_UsuariosLogin.Rows(0).Item("Sucursal")
        Session("ModuloClave") = "33"
        Session("ModuloVersion") = "1.0.0.0"
        Session("DepartamentoID") = dt_UsuariosLogin.Rows(0).Item("Id_Departamento")
        Session("PuestoID") = dt_UsuariosLogin.Rows(0).Item("Id_Puesto")
        Session("UsuarioNombre") = dt_UsuariosLogin.Rows(0).Item("Nombre")
        Session("DeptoNombre") = dt_UsuariosLogin.Rows(0).Item("Depto")
        Session("MonedaId") = dt_UsuariosLogin.Rows(0).Item("Moneda_Local")
        Session("TurnoId") = fn_ObtenTurno(Session("SucursalID"))
        Session("NombreEmpresa") = fn_ObtenerDatosEmpresa(Session("EmpresaID"))
        Session("usuarioNivel") = dt_UsuariosLogin.Rows(0).Item("Tipo")

        Session("LoginID") = Login_Insertar(Session("UsuarioID"), Session("SucursalID"), Session("ModuloClave"), Session("EstacioN"), Session("EstacionIP"), "", Session("ModuloVersion"))

        If Session("LoginID") > 0 Then
            fn_Log_Create(Session("UsuarioID"), Session("ModuloClave"), "INICIO DE SESION", Session("EstacioN"), Session("EstacionIP"), "", Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))
        End If

        '--------------------------------------------------------------------------------
        'Aquí se obtienen los mensajes a mostrar para este módulo
        Dim mensajes As String = ""

        Dim dt_Mensajes As DataTable = fn_Login_ObtenerMensajes(Session("ModuloClave"), Session("SucursalID"), Session("UsuarioID"))
        If dt_Mensajes IsNot Nothing Then
            If dt_Mensajes.Rows.Count > 0 Then
                For Each row As DataRow In dt_Mensajes.Rows
                    If mensajes = "" Then
                        mensajes = row("Asunto") & "/" & row("Mensaje")
                    Else
                        mensajes = mensajes & "\" & row("Asunto") & "," & row("Mensaje")
                    End If
                Next
            Else
                'Si no hay ningún mensaje
                Session("Mensajes") = ""
            End If
            Session("Mensajes") = mensajes
        End If
        '-----------------------------------------------------------------------------------------

        FormsAuthentication.RedirectFromLoginPage(CInt(UsuarioID), False)
        Return True
    End Function


#Region "Funciones Privadas CAPTCHA"

    Private Function Validar_Captcha() As Boolean

        Dim Response As String = Request("g-recaptcha-response") 'Getting Response String Appned to Post Method

        Dim Valid As Boolean = False
        'Request to Google Server

        Dim req As Net.HttpWebRequest = CType(Net.WebRequest.Create(" https://www.google.com/recaptcha/api/siteverify?secret=6LdtDQwUAAAAAFriH9JnNvzyW5OScGaN5MKDIuAz&response=" & Response), Net.HttpWebRequest)
        Try

            'Google recaptcha Responce
            Using wResponse As Net.WebResponse = req.GetResponse()

                Using readStream As New IO.StreamReader(wResponse.GetResponseStream())
                    Dim jsonResponse As String = readStream.ReadToEnd()

                    Dim js As New Script.Serialization.JavaScriptSerializer()
                    Dim data As MyObject = js.Deserialize(Of MyObject)(jsonResponse) ' Deserialize Json

                    Valid = Convert.ToBoolean(data.success)
                End Using
            End Using
            Return Valid
        Catch ex As Exception
            ' msgerror = ex.ToString
            Return False

        End Try

    End Function

    Public Class MyObject
        Public Property success() As String
            Get
                Return m_success
            End Get
            Set(value As String)
                m_success = value
            End Set
        End Property
        Private m_success As String
    End Class

#End Region

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        mpeEditOrder.Show()
    End Sub

    Protected Sub imgbtn_Salir_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtn_Salir.Click
        mpeEditOrder.Hide()
    End Sub
End Class
