
Imports System.Data

Partial Class MostrarImagenes
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Clear()

        Dim Id_RIAI As Integer = Request("Id")

        Dim dt As DataTable = Cn_Soporte.fn_IncidentesAccidentes_AmpliarImagenes(Id_RIAI, Session("SucursalID"), Session("UsuarioID"))

        For x As Integer = 0 To dt.Rows.Count - 1
            ' Dim arr As Byte() = dt.Rows(x).Item("Imagen")
            Context.Response.BinaryWrite(DirectCast(dt.Rows(x).Item("Imagen"), Byte()))
        Next

        Response.ContentType = "image/jpeg"

    End Sub
End Class
