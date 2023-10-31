
Imports SISSAIntranet.Cn_Soporte
Imports SISSAIntranet.FuncionesGlobales
Imports System.Data

Partial Class wuc_Actualizaciones
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub
        Page.Title = "ACTUALIZACIONES"
        LlenarGridActualizaciones()
    End Sub

    Sub LlenarGridActualizaciones()
        gv_Actualizaciones.SelectedRowStyle.BackColor = Drawing.Color.White
        gv_Actualizaciones.SelectedRowStyle.ForeColor = Drawing.Color.Black
        gv_Actualizaciones.SelectedRowStyle.Font.Bold = False

        Dim dt As DataTable = fn_Actualizaciones_LlenarLista(Session("ModuloClave"), Session("SucursalID"), Session("UsuarioID"))

        If dt IsNot Nothing Then
            gv_Actualizaciones.DataSource = dt
            gv_Actualizaciones.DataBind()
        Else
            MuestraGridsVacios()
        End If
    End Sub

    Sub MuestraGridsVacios()

        gv_Actualizaciones.SelectedRowStyle.BackColor = Drawing.Color.White
        gv_Actualizaciones.SelectedRowStyle.ForeColor = Drawing.Color.Black
        gv_Actualizaciones.SelectedRowStyle.Font.Bold = False
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
        If gv_Actualizaciones.SelectedDataKey.Values("Status") = "" Then
            gv_Actualizaciones.SelectedRowStyle.ForeColor = Drawing.Color.Black
            gv_Actualizaciones.SelectedRowStyle.Font.Bold = False
            gv_Actualizaciones.SelectedRowStyle.BackColor = System.Drawing.Color.White
            Exit Sub
        End If
        tbx_Descripcion.Text = gv_Actualizaciones.SelectedDataKey.Values("Descripcion")
        gv_Actualizaciones.SelectedRowStyle.BackColor = System.Drawing.Color.FromName("#C0A062")
        gv_Actualizaciones.SelectedRowStyle.ForeColor = Drawing.Color.White
        gv_Actualizaciones.SelectedRowStyle.Font.Bold = True

        lbl_Descripcion.Enabled = True
        tbx_Descripcion.Enabled = True
    End Sub

End Class
