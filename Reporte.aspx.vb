Partial Public Class Reporte
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Dim dt As New DataTable
        dt.Columns.Add("Remision")
        dt.Columns.Add("Caja")
        dt.Columns.Add("Cliente")
        dt.Columns.Add("Importe")
        dt.Columns.Add("Envases")

        lbl_NombreEmpresa.Text = Session("NombreEmpresa")
        lbl_Sucursal.Text = Session("SucursalN")

        Dim Remisiones() As String = Split(Session("RemisionesDespachadas"), "/")

        For x As Integer = 0 To Remisiones.Length - 1
            Dim div() As String = Split(Remisiones(x), ";")
            dt.Rows.Add(div(0), div(1), div(2), div(3), div(4))
        Next

        Dim datos() As String = Split(Session("DatosRuta"), ";")
        lbl_RutaD.Text = datos(0)
        lbl_UnidadD.Text = datos(1)
        lb_DespachaD.Text = datos(3)
        lbl_RecibeD.Text = datos(2)

        gv_Remisiones.DataSource = dt
        gv_Remisiones.DataBind()

        Dim cstype As Type = Page.[GetType]()
        ClientScript.RegisterStartupScript(cstype, "Reporte", "<script type=text/javascript>Imprimir()</script>")

        Session("RemisionesDespachadas") = ""
        Session("DatosRuta") = ""
    End Sub

    Private Sub btnImprimir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
        Dim cstype As Type = Page.[GetType]()
        ClientScript.RegisterStartupScript(cstype, "Reporte", "<script type=text/javascript>Imprimir()</script>")
    End Sub
End Class