Imports System.Drawing
Imports System.IO
Imports System.Drawing.Imaging

Partial Class Mostar_Fotos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Clear()

        Dim IdEmpleado As Integer = Request("Id")
        Dim Redimensionar As Boolean = False
        If Request("Red") IsNot Nothing Then
            Redimensionar = CBool(Request("Red"))
        End If
        Dim Foto As String = Replace(Request("Foto"), "'", "")

        Dim dr As System.Data.DataRow = Cn_Soporte.fn_Empleados_LeerImagenes(IdEmpleado, Session("SucursalID"), Session("UsuarioID"))
        If dr IsNot Nothing Then

            Dim arr As Byte() = dr(Foto)
            If Redimensionar Then
                Dim bmpOrigen As New Bitmap(New MemoryStream(arr))
                Dim bmpDestino As New Bitmap(CInt(bmpOrigen.Width * 0.5), CInt(bmpOrigen.Height * 0.5))
                Dim grDest As Graphics = Graphics.FromImage(bmpDestino)


                grDest.DrawImage(bmpOrigen, 0, 0, CInt(bmpOrigen.Width * 0.5) + 1, CInt(bmpOrigen.Height * 0.5) + 1)
                Dim ms As New MemoryStream()
                bmpDestino.Save(ms, ImageFormat.Jpeg)
                Response.ContentType = "image/jpeg"
                Response.BinaryWrite(ms.GetBuffer())
            Else
                Response.ContentType = "image/jpeg"
                Response.BinaryWrite(arr)
            End If
        End If
    End Sub
End Class
