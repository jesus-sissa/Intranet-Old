
Partial Public Class _Mensaje
    Inherits System.Web.UI.Page

    Private Enum TipoMensaje
        eerror = 1
        correcto = 2
        advertencia = 3
    End Enum

    Private Sub mostrarmensaje(ByVal mensaje As String, ByVal Tipo As TipoMensaje)
        Select Case CInt(Tipo)
            Case 1
                pnlMensaje.CssClass = "CajaDialogoError"
                mensajeheaderPanel.CssClass = "MensajeHeaderError"
            Case 2
                pnlMensaje.CssClass = "CajaDialogoCorrecto"
                mensajeheaderPanel.CssClass = "MensajeHeaderCorrecto"
            Case 3
                pnlMensaje.CssClass = "CajaDialogoAdvertencia"
                mensajeheaderPanel.CssClass = "MensajeHeaderAdvertencia"
        End Select
        mpeMensaje.Show()
        mensajeLabel.Text = mensaje
    End Sub

    'Protected Sub correctoButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles correctoButton.Click
    '    Call mostrarmensaje("Mensaje correcto", TipoMensaje.correcto)
    'End Sub

    'Protected Sub advertenciaButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles advertenciaButton.Click
    '    Call mostrarmensaje("Mensaje de advertencia", TipoMensaje.advertencia)
    'End Sub

    'Protected Sub errorButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles errorButton.Click
    '    Call mostrarmensaje("Mensaje de error", TipoMensaje.eerror)
    'End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call mostrarmensaje("Los Empleados seleccionados ya tienen Jornadas Laborales asignadas en el rango de Fechas seleccionado y serán reemplazadas por las nuevas.", TipoMensaje.advertencia)
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

    End Sub
End Class
