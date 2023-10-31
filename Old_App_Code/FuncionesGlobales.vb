Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports IntranetSIAC.Cn_Mail
Imports IntranetSIAC.Cn_Soporte
Imports System.Data.DataColumn
Imports System.Web.UI.Page

Public Class FuncionesGlobales
    Inherits Page

#Region "Variables Privadas"
    Private _Alerta As Page = Page.LoadControl("~\Mensaje.aspx")
#End Region

#Region "Eventos"
    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        _Alerta = Page.LoadControl("~\UserControls\Alerta.ascx")
    End Sub
#End Region

    Public Enum Validar_Cadena
        Numeros_Enteros = 1
        Numeros_Decimales = 2
        Letras = 3
        LetrasYcaracteres = 4
        LetrasYnumeros = 5
        LetrasNumerosYCar = 6
        Porcentaje = 7
        DireccionIP = 8
    End Enum

    Public Shared Sub MsgBox(ByVal Mensaje As String, ByVal response As HttpResponse)
        response.Write(scripthandler("alert('" & Mensaje & "');"))
    End Sub

    Public Shared Function scripthandler(ByVal script As String) As String
        Return "<script type='text/javascript'>" & Chr(13) & _
               "<!--" & Chr(13) & _
               script & Chr(13) & _
               "-->" & Chr(13) & _
               "</script>"
    End Function

    Public Shared Sub MostrarAlertAjax(ByVal mensaje As String, ByVal control As Control, ByVal page As Page)
        Dim cstype As Type = page.[GetType]()
        Dim csname As String = "AlertaErrorScript"
        Dim cs As ClientScriptManager = page.ClientScript
        If Not cs.IsStartupScriptRegistered(cstype, csname) Then
            Dim alertaErrorScript As String = "alert('" & mensaje & "');"
            ScriptManager.RegisterClientScriptBlock(control, cstype, csname, alertaErrorScript, True)
        End If
    End Sub

    Public Shared Sub MostrarMensajeAjax(ByVal Mensaje As String)
        '_Alerta.
    End Sub

    Public Shared Function fn_Valida_Cadena(ByVal cadena As String, ByVal tipo As Validar_Cadena) As Boolean
        Dim Ii As Integer
        Dim Car As String
        Dim Numeros As String = "0123456789"
        Dim Numeros_Dec As String = "0123456789."
        Dim Letras As String = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ "
        Dim LetrasYcar As String = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ.,-() @"
        Dim LetrasYnumeros As String = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ0123456789 "
        Dim LetrasNumerosYCar As String = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ0123456789 .,-_()@*"
        Select Case tipo
            Case 1 ' Solo Numeros
                fn_Valida_Cadena = True
                For Ii = 1 To cadena.Length
                    Car = Mid(cadena, Ii, 1)
                    If InStr(Numeros, Car) = 0 Then
                        fn_Valida_Cadena = False
                        Exit Function
                    End If
                Next Ii
            Case 2, 7, 8 ' Numeros Decimales
                fn_Valida_Cadena = True
                For Ii = 1 To cadena.Length
                    Car = Mid(cadena, Ii, 1)
                    If InStr(Numeros_Dec, Car) = 0 Then
                        fn_Valida_Cadena = False
                        Exit Function
                    End If
                Next Ii
            Case 3 ' Solo Letras
                fn_Valida_Cadena = True
                For Ii = 1 To cadena.Length
                    Car = Mid(cadena, Ii, 1)
                    Car = Car.ToUpper
                    If InStr(Letras, Car) = 0 Then
                        fn_Valida_Cadena = False
                        Exit Function
                    End If
                Next Ii
            Case 4 ' Letras y Caracteres
                fn_Valida_Cadena = True
                For Ii = 1 To cadena.Length
                    Car = Mid(cadena, Ii, 1)
                    Car = Car.ToUpper
                    If InStr(LetrasYcar, Car) = 0 Then
                        fn_Valida_Cadena = False
                        Exit Function
                    End If
                Next Ii
            Case 5 ' Letras y numeros
                fn_Valida_Cadena = True
                For Ii = 1 To cadena.Length
                    Car = Mid(cadena, Ii, 1)
                    Car = Car.ToUpper
                    If InStr(LetrasYnumeros, Car) = 0 Then
                        fn_Valida_Cadena = False
                        Exit Function
                    End If
                Next Ii
            Case 6
                fn_Valida_Cadena = True
                For Ii = 1 To cadena.Length
                    Car = Mid(cadena, Ii, 1)
                    Car = Car.ToUpper
                    If InStr(LetrasNumerosYCar, Car) = 0 Then
                        fn_Valida_Cadena = False
                        Exit Function
                    End If
                Next Ii
        End Select
    End Function

    Public Shared Sub TrataEx(ByVal Ex As Exception)
        If TypeOf (Ex) Is System.Data.SqlClient.SqlException Then
            Dim SqlEx As System.Data.SqlClient.SqlException = CType(Ex, System.Data.SqlClient.SqlException)
            fn_GuardaError(SqlEx.Procedure, SqlEx.Number, SqlEx.Message)
        Else
            fn_GuardaError(Ex.StackTrace, 0, Ex.Message)
        End If
    End Sub

    Public Shared Function fn_GuardaError(ByVal donde As String, ByVal numero_error As String, ByVal descripcion As String) As Boolean
        On Error GoTo kk
        Dim Resu As Integer
        Dim CnN1 As New SqlConnection
        Dim CmD1 As New SqlCommand
        Dim Consulta As String
        fn_GuardaError = False

        'CnN1 = Cn_Datos.Crea_ConexionSTD
        'Consulta = "insert into usr_Errores(Id_Sucursal, Fecha, Id_Empleado, Clave_Modulo, Version, Estacion, EstacionIP, EstacionMAC, Donde, Numero_Error, Descripcion)" _
        '    & " values(" & Id_Sucursal & " ,getdate()," & Id_Usuario & " , '33','','','','','" & Right(Replace(donde, "'", ""), 50) & "','" & numero_error & "','" & Left(Replace(descripcion, "'", ""), 250) & "')"
        'CmD1 = Cn_Datos.Crea_Comando(Consulta, Data.CommandType.Text, CnN1)
        'Resu = Cn_Datos.EjecutarNonQuery(CmD1)
        'If Resu > 0 Then
        '    fn_GuardaError = True
        'Else
        '    fn_GuardaError = False
        'End If
        'CmD1.Dispose()
        'CnN1.Dispose()

        CnN1 = Cn_Datos.Crea_ConexionSTD
        CmD1 = Cn_Datos.Crea_Comando("Usr_Errores_Create", CommandType.StoredProcedure, CnN1)

        Cn_Datos.Crea_Parametro(CmD1, "@Id_Sucursal", SqlDbType.Int, HttpContext.Current.Session("SucursalID"))
        Cn_Datos.Crea_Parametro(CmD1, "@Id_Empleado", SqlDbType.Int, HttpContext.Current.Session("UsuarioID"))
        Cn_Datos.Crea_Parametro(CmD1, "@Clave_Modulo", SqlDbType.VarChar, "33")
        Cn_Datos.Crea_Parametro(CmD1, "@Version", SqlDbType.VarChar, "")
        Cn_Datos.Crea_Parametro(CmD1, "@Estacion", SqlDbType.VarChar, "")
        Cn_Datos.Crea_Parametro(CmD1, "@EstacionIP", SqlDbType.VarChar, "")
        Cn_Datos.Crea_Parametro(CmD1, "@EstacionMAC", SqlDbType.VarChar, "")
        Cn_Datos.Crea_Parametro(CmD1, "@Donde", SqlDbType.VarChar, donde)
        Cn_Datos.Crea_Parametro(CmD1, "@Numero_Error", SqlDbType.VarChar, numero_error)
        Cn_Datos.Crea_Parametro(CmD1, "@Descripcion", SqlDbType.VarChar, descripcion)


        Resu = Cn_Datos.EjecutarScalar(CmD1)
        If Resu > 0 Then
            fn_GuardaError = True
        Else
            fn_GuardaError = False
        End If


        Dim Texto_Mail As String = "                  Módulo: " & "INTRANET SIAC" & Chr(13) _
                                 & "         Usuario Firmado: " & HttpContext.Current.Session("SucursalN") & Chr(13) _
                                 & "         Usuario Firmado: " & HttpContext.Current.Session("UsuarioNombre") & Chr(13) _
                                 & "                  Equipo: " & "" & Chr(13) _
                                 & "                   Donde: " & donde & Chr(13) _
                                 & "             Descripcion: " & descripcion & Chr(13) & Chr(13) _
                                 & "Agente de Servicios SIAC."
        Cn_Mail.fn_Enviar_MailFallas("Reporte de Errores SIAC.", Texto_Mail, "", HttpContext.Current.Session("SucursalID"))

        Exit Function
kk:
        fn_GuardaError = False
    End Function

    Public Shared Function fn_Fecha102(ByVal fe As String) As String
        If fe.Length <> 10 Then
            fn_Fecha102 = fe
        Else
            fn_Fecha102 = Right(fe, 4) + "." + Mid(fe, 4, 2) + "." + Left(fe, 2)
        End If
    End Function

    Public Shared Sub fn_LlenarDDL(ByVal ddl As DropDownList, ByVal tabla As DataTable, ByVal texto As String, ByVal valor As String, ByVal valorinicial As String)

        Dim ItemCero As ListItem = New ListItem("Seleccione ...", valorinicial)
        ddl.DataSource = tabla
        Dim cant As Integer = tabla.Rows.Count
        ddl.DataTextField = texto
        ddl.DataValueField = valor
        ddl.DataBind()
        ddl.Items.Insert(0, ItemCero)
        ddl.SelectedIndex = 0

    End Sub

    Public Shared Sub fn_LlenarDDL_VariosCampos(ByVal ddl As DropDownList, ByVal tabla As DataTable, ByVal campoText As String, ByVal campoValue As String)
        Dim dr As DataRow = tabla.NewRow
        'Dim a As String = tabla.Rows(0)("Descripcion")
        For Each DataColumn In tabla.Columns
            If DataColumn.ColumnName = campoText Then
                dr(DataColumn.ColumnName) = "Seleccione..."
            Else
                dr(DataColumn.ColumnName) = "0"
            End If
        Next

        tabla.Rows.InsertAt(dr, 0)
        ddl.DataSource = tabla
        ddl.DataBind()
    End Sub

    Public Shared Sub LlenarMinutos(ByVal ddl As DropDownList, ByVal Minutos As Integer)
        Dim t As DateTime = #12:00:00 AM#
        Dim cont As Integer = 1

        ddl.Items.Insert(0, "Seleccione..")
        While t <= #11:59:59 PM#
            ddl.Items.Insert(cont, t.ToString("HH:mm"))
            t = t.AddMinutes(Minutos)
            cont += 1
        End While

    End Sub

    Public Shared Function fn_ConvertirFormato_hhmm(ByVal Hora As String) As String

        Dim HoraF As String
        Dim hA As String()
        Dim hS As String
        Dim mN As Integer
        Dim mS As String

        hA = Split(Hora, ".")
        If Len(hA(0)) < 2 Then
            hS = "0" & hA(0)
        Else
            hS = hA(0)
        End If

        mN = CInt(hA(1)) * 0.6
        If mN = 0 Then
            mS = "00"
        Else
            mS = mN.ToString
        End If

        HoraF = hS & ":" & mS

        Return HoraF

    End Function

    Public Shared Function fn_EnviarCorreos(ByVal ClaveAlerta As Integer, ByVal DetalleHTML As String, ByVal SucursalId As Integer, ByVal Asunto As String) As Boolean
        Dim tbl As DataTable = fn_AlertasGeneradas_ObtenerMails(ClaveAlerta)

        If Not tbl Is Nothing Then
            For Each renglon As DataRow In tbl.Rows
                If fn_ValidarMail(renglon("Mail")) Then
                    fn_Enviar_MailHTML(renglon("Mail"), Asunto, DetalleHTML, "", SucursalId)
                End If
            Next
        End If

        Return True
    End Function

    Public Shared Function fn_CreaGridVacio(ByVal campos As String) As DataTable
        Dim dt As New DataTable
        Dim column As New DataColumn

        Dim arr() As String = Split(campos, ",")
        For x As Integer = 0 To arr.Length - 1
            dt.Columns.Add(New DataColumn(arr(x), GetType(String)))
        Next

        Dim Dr As DataRow = dt.NewRow
        For x As Integer = 0 To arr.Length - 1
            Dr(arr(x)) = ""
        Next

        dt.Rows.Add(Dr)

        Return dt
    End Function

    Public Shared Function fn_AgregarFila(ByVal Columns As String, ByVal Filas As String) As DataTable
        Dim dt As New DataTable
        Dim column As New DataColumn

        Dim Columnas() As String = Split(Columns, ",")
        Dim Usuarios() As String = Split(Filas, ";")
        Dim arr() As String

        For x As Integer = 0 To Columnas.Length - 1
            dt.Columns.Add(Columnas(x))
        Next

        Dim Dr As DataRow
        For u As Integer = 0 To Usuarios.Length - 1
            Dr = dt.NewRow
            arr = Split(Usuarios(u), ",")
            For c As Integer = 0 To Columnas.Length - 1
                Dr(c) = arr(c)
            Next
            dt.Rows.Add(Dr)
        Next

        Return dt
    End Function

    Public Shared Function fn_AgregarFilaGrid(ByVal campos As String, ByVal ElementosAgregados As String) As DataTable

        Dim dt As New DataTable
        Dim column As New DataColumn

        Dim Columnas() As String = Split(campos, ",")
        Dim Usuarios() As String = Split(ElementosAgregados, "/")
        Dim arr() As String

        For x As Integer = 0 To Columnas.Length - 1
            dt.Columns.Add(Columnas(x))
        Next

        Dim Dr As DataRow
        For u As Integer = 0 To Usuarios.Length - 1
            Dr = dt.NewRow
            arr = Split(Usuarios(u), ";")
            For c As Integer = 0 To Columnas.Length - 1
                Dr(c) = arr(c)
            Next
            dt.Rows.Add(Dr)
        Next

        Return dt

    End Function

    Public Shared Function fn_EliminarFila(ByVal Campos As String, ByVal UsuariosAgregados As String, ByVal Columnas As Integer, ByVal UEliminar As String) As DataTable
        ' No esta en uso
        Dim dt As New DataTable
        Dim UsuariosActualizados As String = ""

        Dim Usuarios() As String = Split(UsuariosAgregados, ";")

        For x As Integer = 0 To Usuarios.Length - 1
            Dim div() As String = Split(Usuarios(x), ",")
            If div(0) <> UEliminar Then
                If UsuariosActualizados = "" Then
                    UsuariosActualizados = Usuarios(x)
                Else
                    UsuariosActualizados = UsuariosActualizados & ";" & Usuarios(x)
                End If
            End If
        Next

        dt = fn_AgregarFila(Campos, UsuariosActualizados)

        Return dt

    End Function

    Public Shared Function fn_Valida_Contra(ByVal cadena As String) As Boolean
        Dim Ii As Integer
        Dim Car As String
        Dim Numeros As String = "0123456789"
        Dim Mayus As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim Minus As String = "abcdefghijklmnopqrstuvwxyz"
        Dim Cant_Numeros As Integer = 0
        Dim Cant_Mayus As Integer = 0
        Dim Cant_Minus As Integer = 0

        fn_Valida_Contra = False
        If cadena.Length < 8 Then
            Exit Function
        End If
        For Ii = 1 To cadena.Length
            Car = Mid(cadena, Ii, 1)
            If InStr(Numeros, Car) > 0 Then
                Cant_Numeros = Cant_Numeros + 1
            End If
            If InStr(Mayus, Car) > 0 Then
                Cant_Mayus = Cant_Mayus + 1
            End If
            If InStr(Minus, Car) > 0 Then
                Cant_Minus = Cant_Minus + 1
            End If
        Next Ii
        If Cant_Mayus > 0 And Cant_Minus > 0 And Cant_Numeros > 3 And (Cant_Mayus + Cant_Minus) > 3 Then
            fn_Valida_Contra = True
        Else
            fn_Valida_Contra = False
        End If

    End Function

    Public Shared Function fn_ValidarMail(ByVal sMail As String) As Boolean
        Return System.Text.RegularExpressions.Regex.IsMatch(sMail, "^([\w-]+\.)*?[\w-]+@[\w-]+\.([\w-]+\.)*?[\w]+$")
    End Function

    Public Shared Function fn_DatatableToHTML(ByVal dt As DataTable, ByVal Titulo As String, ByVal Cols_Omitir_Izq As Integer, ByVal Cols_Omitir_Der As Integer) As String
        '"Prueba de Correo HTML.<Br><Table><Tr><Td>Celda 1</Td><Td>Celda 2</Td></Tr><Tr><Td>Celda 1</Td><Td>Celda 2</Td></Tr></Table>"
        Dim Cadena As String = ""
        Dim Fila As Integer = 0
        Dim Columna As Integer = 0
        'Titulo
        Cadena = "<Table style='border:solid 1px black; border-collapse:collapse' width='100%'>"
        Cadena &= "<CAPTION style='border:solid 1px black'><b>"
        Cadena &= Titulo
        Cadena &= "</b></CAPTION>"
        'Encabezados
        Cadena &= "<thead>"
        Cadena &= "<tr>"
        Dim indice As Integer = 0
        For Each cl As DataColumn In dt.Columns
            If indice >= Cols_Omitir_Izq Then
                If indice > (dt.Columns.Count - 1 - Cols_Omitir_Der) Then Exit For
                Cadena &= "<th style='border:solid 1px black'>"
                Cadena &= cl.Caption
                Cadena &= "</th>"
            End If
            indice += 1
        Next
        Cadena &= "</tr>"
        Cadena &= "<thead>"
        'Filas
        For Fila = 0 To dt.Rows.Count - 1
            Cadena &= "<Tr>"
            For Columna = 0 + Cols_Omitir_Izq To dt.Columns.Count - 1 - Cols_Omitir_Der
                Cadena &= "<Td style='border:solid 1px black'>"
                Cadena &= dt.Rows(Fila)(Columna).ToString
                Cadena &= "</Td>"
            Next
            Cadena &= "</Tr>"
        Next Fila
        Cadena &= "</Table>"
        Return Cadena
    End Function

    Public Shared Function fn_GetComputerName() As String
        fn_GetComputerName = System.Net.Dns.GetHostName
    End Function

    Public Shared Function fn_GetComputerIP() As String
        fn_GetComputerIP = ""
        Try
            Dim hostName As String = Net.Dns.GetHostName()
            Dim host As Net.IPHostEntry = Net.Dns.GetHostEntry(hostName)
            Dim IP As Net.IPAddress() = host.AddressList

            Dim index As Integer

            For index = 0 To IP.Length - 1
                If IP(index).ToString.Length > 7 And IP(index).ToString.Length < 16 Then
                    fn_GetComputerIP = IP(index).ToString
                    Exit For
                End If
            Next index
            Return fn_GetComputerIP
        Catch
            fn_GetComputerIP = ""
        End Try
    End Function
End Class
