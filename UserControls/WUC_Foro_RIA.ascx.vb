Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data

Partial Class WUC_Foro_RIA
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Page.Title = "Foro de Reportes de Incidentes/Accidentes"
        MuestraGridsVacios()
        LlenarGridRIA()

    End Sub

    Sub MuestraGridsVacios()

        'gv_RIA.SelectedRowStyle.BackColor = Drawing.Color.White
        'gv_RIA.SelectedRowStyle.ForeColor = Drawing.Color.Black
        'gv_RIA.SelectedRowStyle.Font.Bold = False
        gv_RIA.DataSource = fn_CreaGridVacio("Id_RIA,Numero,Sucursal,Fecha,Hora,Tipo,Entidad,Status")
        gv_RIA.DataBind()

        'gv_Detalle.SelectedRowStyle.BackColor = Drawing.Color.White
        'gv_Detalle.SelectedRowStyle.ForeColor = Drawing.Color.Black
        'gv_Detalle.SelectedRowStyle.Font.Bold = False
        'gv_Detalle.DataSource = fn_CreaGridVacio("Id_RIAD,Id_RIA,Fecha,Tipo,Entidad,Descripcion")
        'gv_Detalle.DataBind()

        'gv_Imagenes.DataSource = Nothing
        'gv_Imagenes.DataBind()

        'gv_Usuarios.DataSource = fn_CreaGridVacio("Id_Entidad,Nombre")
        'gv_Usuarios.DataBind()

        'lbl_UsuarioA.Enabled = False
        'ddl_UsuarioA.Enabled = False
        'btn_Asignar.Enabled = False
    End Sub

    Sub LlenarGridRIA()
        'gv_RIA.SelectedRowStyle.BackColor = Drawing.Color.White
        'gv_RIA.SelectedRowStyle.ForeColor = Drawing.Color.Black
        'gv_RIA.SelectedRowStyle.Font.Bold = False


        Dim dt As DataTable = fn_SeguimientoRIA_LlenarLista(Session("SucursalID"), Session("UsuarioID"))

        If dt IsNot Nothing Then
            gv_RIA.DataSource = dt
            gv_RIA.DataBind()
        Else
            gv_RIA.DataSource = fn_CreaGridVacio("Id_RIA,Numero,Sucursal,Fecha,Hora,Tipo,Entidad,Status")
            gv_RIA.DataBind()
        End If

    End Sub

    Protected Sub gv_RIA_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_RIA.SelectedIndexChanged
        If gv_RIA.SelectedDataKey.Values("Status") = "" Then
            'gv_RIA.SelectedRowStyle.ForeColor = Drawing.Color.Black
            'gv_RIA.SelectedRowStyle.Font.Bold = False
            'gv_RIA.SelectedRowStyle.BackColor = System.Drawing.Color.White
            Exit Sub
        End If
        'gv_RIA.SelectedRowStyle.BackColor = System.Drawing.Color.FromName("#9aabcc")
        'gv_RIA.SelectedRowStyle.ForeColor = Drawing.Color.White
        'gv_RIA.SelectedRowStyle.Font.Bold = True

        Dim dt As DataTable = fn_ConsultaRIA_LlenarDetalle(gv_RIA.SelectedDataKey.Values("Id_RIA"), Session("SucursalID"), Session("UsuarioID"))
        For Each Seg As DataRow In dt.Rows

            Dim lbl_Encabezado As New System.Web.UI.WebControls.Label()
            lbl_Encabezado.Width = 823
            lbl_Encabezado.Height = 20
            lbl_Encabezado.BorderStyle = BorderStyle.Solid
            lbl_Encabezado.BorderWidth = 1
            lbl_Encabezado.BorderColor = Drawing.Color.Gray
            lbl_Encabezado.BackColor = System.Drawing.Color.FromName("#9aabcc")
            lbl_Encabezado.Text = Seg.Item("Fecha") & " - " & Seg.Item("Entidad")
            lbl_Encabezado.Font.Name = "Verdana"
            lbl_Encabezado.Font.Size = 8
            lbl_Encabezado.Font.Bold = True
            PlaceHolder1.Controls.Add(lbl_Encabezado)

            'PlaceHolder1.Controls.Add(New LiteralControl("<br />"))
            'PlaceHolder1.Controls.Add(New LiteralControl("<br />"))

            Dim tbx_Seguimiento As New System.Web.UI.WebControls.TextBox()
            tbx_Seguimiento.TextMode = TextBoxMode.MultiLine
            tbx_Seguimiento.Style("overflow") = "hidden"
            tbx_Seguimiento.Font.Name = "Verdana"
            tbx_Seguimiento.Width = 819
            tbx_Seguimiento.Text = Seg.Item("Descripcion") & vbNewLine
            tbx_Seguimiento.Font.Size = 8
            PlaceHolder1.Controls.Add(tbx_Seguimiento)

            'Dim lbl_Seguimiento As New System.Web.UI.WebControls.Label()
            'lbl_Seguimiento.Width = 823
            ''lbl_Seguimiento.Height = 100
            ''lbl_Seguimiento.BorderWidth = 1
            ''lbl_Seguimiento.BorderStyle = BorderStyle.Dotted
            ''lbl_Seguimiento.BorderColor = Drawing.Color.Gray
            ''lbl_Seguimiento.BackColor = Drawing.Color.Cornsilk
            'lbl_Seguimiento.Text = Seg.Item("Descripcion")
            'lbl_Seguimiento.Font.Size = 8
            'PlaceHolder1.Controls.Add(lbl_Seguimiento)

            PlaceHolder1.Controls.Add(New LiteralControl("<hr />"))
            PlaceHolder1.Controls.Add(New LiteralControl("<br />"))
            'PlaceHolder1.Controls.Add(New LiteralControl("<br />"))

            'Dim espacio As LiteralControl = New System.Web.UI.LiteralControl("<br />")
            'PlaceHolder1.Controls.Add(espacio)


        Next

    End Sub
End Class
