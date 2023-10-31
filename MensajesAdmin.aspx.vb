
Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.FuncionesGlobales
Imports System.Diagnostics
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.HttpServerUtility

Partial Class MensajesAdmin
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Session("TipoMensaje") = Request.QueryString("TipoM").ToString

        'Dim Mensaje As String = Request.Form("Mensaje").ToString
        'Dim err As String = Server.GetLastError.Message

        'If Mensaje.Length < 10 Then
        '    MostrarAlertAjax("Escriba un mensaje para el administrador", Page, Page)
        '    'Response.Write("Escriba un mensaje para el administrador.")
        '    'Exit Sub
        '    Response.Redirect("~/GeneralErrorPage.htm")
        'End If


        'If Session("TipoMensaje") = 1 Then
        '    Dim Detalles As String = "Mensaje de Error del INTRANET " _
        '                            & Mensaje _
        '                            & "; Usuario Registró : " & Session("UsuarioNombre")

        '    fn_AlertasGeneradas_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), "11", Detalles)
        'End If

        'Dim DetalleMail As String = fn_DetalleMail(Session("DeptoNombre"), "INTRANET SIAC", Mensaje.ToUpper, Session("UsuarioNombre"))

        'If Not fn_EnviarCorreos("11", DetalleMail, Session("SucursalID"), "ADMINISTRADOR DE MENSAJES") Then
        '    MostrarAlertAjax("Ha ocurrido un error al intentar enviar los Correos.", sender, Page)
        '    Exit Sub
        'End If

        'Response.Redirect("~/Default.aspx")

    End Sub

    Public Shared Function fn_DetalleHTML(ByVal Departamento As String, ByVal Modulo As String, ByVal Mensaje As String, ByVal UsuarioRegistro As String, ByVal SucursalN As String) As String

        'Dim Detalle As String = "            ADMINISTRADOR DE MENSAJES " & Chr(13) & Chr(13) _
        '                      & "                Sucursal: " & SucursalN & Chr(13) _
        '                      & "            Departamento: " & Departamento & Chr(13) _
        '                      & "                  Módulo: " & Modulo & Chr(13) _
        '                      & "                 Mensaje: " & Mensaje & Chr(13) _
        '                      & "         Usuario Firmado: " & UsuarioRegistro & Chr(13) _
        '                      & "                   Fecha: " & Now.Date & Chr(13) _
        '                      & "                    Hora: " & Now.ToShortTimeString & Chr(13) & Chr(13) _
        '                      & "Agente de Servicios SIAC."

        Dim Pie As String = "Agente de Servicios SIAC " & Now.Year.ToString
        Dim DetalleHTML As String = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                                 & "<tr><td colspan='4' align='center'> ADMINISTRADOR DE MENSAJES </td></tr>" _
                                 & "<tr><td colspan='4'><hr /></td></tr>" _
                                 & "<tr><td align='right'><label><b>Sucursal:</b></label></td><td> " & SucursalN & " </td><td></td><td></td></tr>" _
                                 & "<tr><td align='right'><label><b>Departamento:</b></label></td><td>" & Departamento & "</td><td></td><td></td></tr>" _
                                 & "<tr><td align='right'><label><b>Módulo:</b></label></td><td>" & Modulo & "</td><td></td><td></td></tr>" _
                                 & "<tr><td align='right'><label><b>Mensaje:</b></label></td><td>" & Mensaje & "</td><td></td><td></td></tr>" _
                                 & "<tr><td align='right'><label><b>Usuario Firmado:</b></label></td><td>" & UsuarioRegistro & "</td><td></td><td></td></tr>" _
                                 & "<tr><td align='right'><label><b>Fecha:</b></label></td><td>" & Now.Date & "</td><td></td><td></td></tr>" _
                                 & "<tr><td align='right'><label><b>Hora:</b></label></td><td>" & Now.ToShortTimeString & "</td><td></td><td></td></tr>" _
                                 & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>" & Pie & "</td></tr></table><br/><br/></body></html>"

        Return DetalleHTML

    End Function

    Protected Sub btn_EnviarMensaje_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_EnviarMensaje.Click
        If txt_Mensaje.Value.Length < 10 Then
            MostrarAlertAjax("Escriba un mensaje completo para el administrador", btn_EnviarMensaje, Page)
            Exit Sub
        Else
            If Session("TipoMensaje") = 1 Then
                Dim Detalles As String = "Mensaje de Error del INTRANET " _
                                        & txt_Mensaje.Value _
                                        & "; Sucursal Registró : " & Session("SucursalN") _
                                        & "; Usuario Registró : " & Session("UsuarioNombre")

                fn_AlertasGeneradas_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), "11", Detalles)
            End If

            Dim DetalleHTML As String = fn_DetalleHTML(Session("DeptoNombre"), "INTRANET SIAC", txt_Mensaje.Value.ToUpper, Session("UsuarioNombre"), Session("SucursalN"))

            If Not fn_EnviarCorreos("11", DetalleHTML, Session("SucursalID"), "ADMINISTRADOR DE MENSAJES") Then
                MostrarAlertAjax("Ha ocurrido un error al intentar enviar los Correos.", sender, Page)
                Exit Sub
            End If
        End If

        Response.Redirect("~/Login.aspx")

    End Sub
End Class
