Partial Public Class Alerta
    Inherits System.Web.UI.UserControl

    Public Sub Alerta(ByVal Mensaje As String, Tema As String)
        Select Case Tema
            Case "DORADO"
                div_tituloAlerta.Style.Add("background-color", "#867044")
            Case "AZUL"
                div_tituloAlerta.Style.Add("background-color", "#104e8b")
            Case "GUINDA"
                div_tituloAlerta.Style.Add("background-color", "#6f3232")
            Case "VERDE"
                div_tituloAlerta.Style.Add("background-color", "#268968")
            Case Else
                div_tituloAlerta.Style.Add("background-color", "#000000")
        End Select

        lbl_Mensaje.Text = Mensaje
        pnl_Alerta_ModalPopupExtender.Show()
    End Sub

    Protected Sub btn_Cerrar_Click(sender As Object, e As ImageClickEventArgs) Handles btn_Cerrar.Click
        pnl_Alerta_ModalPopupExtender.Hide()
    End Sub
End Class
