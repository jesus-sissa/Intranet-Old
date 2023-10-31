Public Partial Class MostrarMiniImagen
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Clear()

        Dim Id_RIA As Integer = Request("Id") 'ES EL ID DEL RIA
        Dim Id_RIAI As String = Request("Id_RIAI")  'jala el id de la imagen
        Dim DT_Imagenes As System.Data.DataTable = Cn_Soporte.fn_IncidentesAccidentes_LeerImagenes(Id_RIA, Session("SucursalID"), Session("UsuarioID"))

        If DT_Imagenes IsNot Nothing Then

            Dim DR_Temporal() As System.Data.DataRow = DT_Imagenes.Select("Id_RIAI=" & Id_RIAI)
            Dim arr As Byte() = DR_Temporal(0)("Imagen") 'TOMAMOS LA IMAGEN DEL CAMPO
            Response.ContentType = "image/jpeg" 'LA DEVOLVEMOS
            Response.BinaryWrite(arr)

        End If

    End Sub

End Class