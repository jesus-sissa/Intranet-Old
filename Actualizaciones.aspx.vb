Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data

Partial Class Actualizaciones
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        LlenarGridActualizaciones()
        Dim Columnas As Byte
        Dim Gv_Nombres() As GridView = {gv_Actualizaciones}

        For j As Byte = 0 To Gv_Nombres.Length - 1
            Columnas = Gv_Nombres(j).Columns.Count

            For i As Byte = 0 To Columnas - 1
                Gv_Nombres(j).Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
            Next
        Next
    End Sub

    Sub LlenarGridActualizaciones()
        Dim dt As DataTable = fn_Actualizaciones_LlenarLista(Session("ModuloClave"), Session("SucursalID"), Session("UsuarioID"))

        If dt IsNot Nothing Then
            gv_Actualizaciones.DataSource = dt
            gv_Actualizaciones.DataBind()
        Else
            Call MuestraGridsVacios()
        End If
    End Sub

    Sub MuestraGridsVacios()
        gv_Actualizaciones.DataSource = fn_CreaGridVacio("Id_Actualizacion,Fecha,Modulo,Menu,Opcion,Descripcion,Status")
        gv_Actualizaciones.DataBind()

    End Sub

    Protected Sub gv_Actualizaciones_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Actualizaciones.PageIndexChanging
        MuestraGridsVacios()
        tbx_Descripcion.Text = ""

        gv_Actualizaciones.PageIndex = e.NewPageIndex
        gv_Actualizaciones.DataSource = fn_Actualizaciones_LlenarLista(Session("ModuloClave"), Session("SucursalID"), Session("UsuarioID"))
        gv_Actualizaciones.DataBind()
    End Sub

    Protected Sub gv_Actualizaciones_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Actualizaciones.SelectedIndexChanged
        tbx_Descripcion.Text = gv_Actualizaciones.SelectedDataKey.Values("Descripcion")
        lbl_Descripcion.Enabled = True
        tbx_Descripcion.Enabled = True
    End Sub

End Class
