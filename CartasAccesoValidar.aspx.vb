Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data
Imports IntranetSIAC.Cn_Mail
Imports System.Web.Security
Imports IntranetSIAC.Cn_Login

Partial Class CartasAccesoValidar
    Inherits BasePage

    Dim Firma As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub
       
        Call MuestraGridsVacios()
        Call LlenarGridCartasAcceso()
        pnl_Firma.Enabled = False

        Dim Columnas As Byte
        Dim Gv_Nombres() As GridView = {gv_CartasAcceso, gv_Detalle}

        For j As Byte = 0 To Gv_Nombres.Length - 1
            Columnas = Gv_Nombres(j).Columns.Count

            For i As Byte = 0 To Columnas - 1
                Gv_Nombres(j).Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
            Next
        Next
    End Sub

    Sub MuestraGridsVacios()
        gv_CartasAcceso.SelectedIndex = -1
        gv_CartasAcceso.DataSource = fn_CreaGridVacio("Id_Carta,FechaRegistro,UsuarioRegistro,FechaInicio,FechaFin,Status,Tipo,Observaciones")
        gv_CartasAcceso.DataBind()

        gv_Detalle.DataSource = fn_CreaGridVacio("Id_Carta,Clave,Nombre,Empresa")
        gv_Detalle.DataBind()

        pnl_Firma.Enabled = False
        tbx_Observaciones.Text = ""
        lbl_EncabezadoComentarios.Enabled = False
    End Sub

    Sub LlenarGridCartasAcceso()

        Dim dt As DataTable = fn_CartasAccesoValidar_LlenarLista(Session("SucursalID"), Session("UsuarioID"))

        If dt IsNot Nothing Then
            gv_CartasAcceso.DataSource = dt
            gv_CartasAcceso.DataBind()
        Else
            gv_CartasAcceso.DataSource = fn_CreaGridVacio("Id_Carta,FechaRegistro,UsuarioRegistro,FechaInicio,FechaFin,Status,Tipo,Observaciones")
            gv_CartasAcceso.DataBind()
        End If

    End Sub

    Sub LlenarGridDetalle()
        Dim dt As DataTable = fn_CartasAccesoValidar_LlenarDetalle(Session("SucursalID"), Session("UsuarioID"), gv_CartasAcceso.SelectedDataKey.Values("Id_Carta"))
        If dt IsNot Nothing Then
            gv_Detalle.DataSource = dt
            Tabla("Detalle") = dt
            gv_Detalle.DataBind()
            tbx_Observaciones.Text = gv_CartasAcceso.SelectedDataKey.Values("Observaciones")
        Else
            gv_Detalle.DataSource = fn_CreaGridVacio("Id_Carta,Clave,Nombre,Empresa")
            gv_Detalle.DataBind()
            tbx_Observaciones.Text = ""
        End If
    End Sub

    Protected Sub gv_CartasAcceso_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_CartasAcceso.PageIndexChanging
        MuestraGridsVacios()
        gv_CartasAcceso.PageIndex = e.NewPageIndex
        gv_CartasAcceso.DataSource = fn_CartasAccesoValidar_LlenarLista(Session("SucursalID"), Session("UsuarioID"))
        gv_CartasAcceso.DataBind()
    End Sub

    Protected Sub gv_CartasAcceso_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_CartasAcceso.SelectedIndexChanged
        Call LlenarGridDetalle()
        pnl_Firma.Enabled = True
        lbl_EncabezadoComentarios.Enabled = True
        tbx_Comentarios.Text = ""
    End Sub

    Protected Sub btn_Validar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Validar.Click

        If tbx_Comentarios.Text.Trim = "" Then
            fn_Alerta("Capture los Comentarios de Validación.")
            tbx_Comentarios.Focus()
            Exit Sub
        End If

        Call ValidarFirma()

        If Firma Then

            If Not fn_CartasAccesoValidar_Validar(gv_CartasAcceso.SelectedDataKey.Values("Id_Carta"), Session("UsuarioID"), Session("EstacioN"), tbx_Comentarios.Text, Session("SucursalID"), Session("UsuarioID")) Then
                    fn_Alerta("Ha ocurrido un error al intentar guardar los datos.")
                Exit Sub
            Else
                'Despues de Validar, enviar las alertas a destinatarios----
                'Se consulta los datos del RIA para obtener el Número de RIA

                Dim Dr_InfoUsuarioAutoriza As DataRow = fn_CartasAccesoConsulta_UsuarioAutoriza(CInt(Session("UsuarioID")))
                Dim dt_detalle As DataTable = Tabla("Detalle")

                If Dr_InfoUsuarioAutoriza IsNot Nothing Then
                    ' Dim Descripcion As String = "AUTORIZACION DE MEMO DE ACCESO"

                    Dim Detalle As String = "AUTORIZACION DE MEMO DE ACCESO PARA: " & gv_Detalle.Rows.Count & " USUARIOS" & Chr(13) & Chr(13) _
                                            & "Agente de Servicios SIAC " & Now.Year.ToString

                    Dim DetalleHTML As String = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                                                & "<tr><td colspan='4' align='center'>AUTORIZACION DE MEMO DE ACCESO</td></tr>" _
                                                & "<tr><td colspan='4'><br><hr /></td></tr>" _
                                                & "<tr><td align='right'><label><b>Asunto:</b></label></td><td>" & tbx_Observaciones.Text.ToUpper & "</td><td></td><td></td></tr>" _
                                                & "<tr><td align='right'><label><b>Autorizado por:</b></label></td><td>" & Dr_InfoUsuarioAutoriza("Clave") & " - " & Dr_InfoUsuarioAutoriza("Nombre") & "</td></tr>" _
                                                & "<tr><td align='right'><label><b>Fecha de Autorización:</b></label></td><td>" & Now.ToShortDateString & "  " & Now.ToShortTimeString & "</td></tr>" _
                                                & "<tr><td align='right'><label><b>Observaciones de Autorización::</b></label></td><td>" & tbx_Comentarios.Text.ToUpper & "</td></tr>" _
                                                & "<tr><td colspan='4' align='center'><br>" & FuncionesGlobales.fn_DatatableToHTML(dt_detalle, "USUARIOS AUTORIZADOS", 1, 0) _
                                                & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='4' align='center'>Agente de Servicios SIAC " & Now.Date.Year.ToString & "</td></tr></table><br/><br/></body></html>"

                    'Aquí se guarda la Alerta y se envian los correos
                    If fn_AlertasGeneradas_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), "40", Detalle) Then
                        'Obtener los Destinos
                        Dim Dt_Destinos As DataTable = fn_AlertasGeneradas_ObtenerMails("40")
                        If Dt_Destinos IsNot Nothing Then
                            For Each renglon As DataRow In Dt_Destinos.Rows
                                Cn_Mail.fn_Enviar_MailHTML(renglon("Mail"), "AUTORIZACION DE MEMO DE ACCESO", DetalleHTML, "", Session("SucursalID"))
                            Next
                        End If
                    End If

                End If
                'fn_Alerta("Se Autorizó correctamente la Carta de Acceso.")
            End If

            Call CerrarFirma()

               Call MuestraGridsVacios()
            Call LlenarGridCartasAcceso()
        Else
            tbx_Contrasena.Focus()
        End If

    End Sub

    Sub ValidarFirma()

        Firma = False

        Dim Contra As String = tbx_Contrasena.Text

        If Contra = "" Then
                fn_Alerta("Indique la Contraseña.")
            tbx_Contrasena.Focus()
            Exit Sub
        End If
        If Len(Contra) < 4 Then
                  fn_Alerta("Contraseña Incorrecta.")
            tbx_Contrasena.Focus()
            Exit Sub
        End If
        If Not FuncionesGlobales.fn_Valida_Cadena(Contra, FuncionesGlobales.Validar_Cadena.LetrasYnumeros) Then
            'En caso de que el nombre no sea valido
               fn_Alerta("Indique una Contraseña válida.")
            tbx_Contrasena.Focus()
            Exit Sub
        End If

        Try
            Dim tbl As DataTable = Usuarios_Login(Session("UsuarioID"))

            If tbl.Rows.Count > 0 Then

                Dim PasswordUsr As String = FormsAuthentication.HashPasswordForStoringInConfigFile(Contra, "SHA1")
                Dim PasswordDb As String = tbl.Rows(0).Item("Password")
                Dim Usuario As String = tbl.Rows(0).Item("Nombre")
                Dim Tipo As Integer = tbl.Rows(0).Item("Tipo")

                If PasswordUsr = PasswordDb Then
                       Firma = True
                Else
                       fn_Alerta("Contraseña incorrecta.")
                    Exit Sub
                End If

                If tbl.Rows(0)("Dias_Expira") < 1 Then
                         fn_Alerta("La Contraseña está expirada.")
                    Exit Sub
                End If

                If tbl.Rows(0)("Status") <> "A" Then
                        fn_Alerta("Usuario Bloqueado. Imposible Entrar al SIAC Intranet. Consulte al Administrador.")
                    Exit Sub
                End If

            Else
                     fn_Alerta("Usuario no encontrado.")
                Exit Sub
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub

    Sub CerrarFirma()
        tbx_Contrasena.Text = ""
        tbx_Comentarios.Text = ""
        btn_Validar.Enabled = False
    End Sub

End Class
