Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.FuncionesGlobales
Imports System.IO

Partial Public Class ConsultaChecador
    Inherits BasePage
    Dim dtHorasC As DataTable = Nothing


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        '-------------------------------------------
        Dim dt_Empleados As DataTable = fn_HorasChecadas_ObtenerEmpleados(Session("SucursalID"), Session("DepartamentoID"), Session("UsuarioID"))
        fn_LlenarDDL_VariosCampos(ddl_Empleado, dt_Empleados, "CveNombre", "Id_Empleado")
        Call MuestraHorasChecadasVacio()

        Dim Columnas As Byte
        Dim Gv_Nombres() As GridView = {gv_horasChec}
        For j As Byte = 0 To Gv_Nombres.Length - 1
            Columnas = Gv_Nombres(j).Columns.Count

            For i As Byte = 0 To Columnas - 1
                Gv_Nombres(j).Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
            Next
        Next
    End Sub

    Sub MuestraHorasChecadasVacio()
        gv_horasChec.DataSource = fn_CreaGridVacio("Clave,Nombre,Departamento,Puesto,Fecha,Hora")
        gv_horasChec.DataBind()
    End Sub

    Sub LlenarHorasChecadas()
        dtHorasC = fn_HorasChecadas_LlenarLista(Session("DepartamentoID"), ddl_Empleado.SelectedValue, CDate(tbx_FechaIni.Text), CDate(tbx_FechaFin.Text))
        If dtHorasC IsNot Nothing Then
            If dtHorasC.Rows.Count > 0 Then
                gv_horasChec.DataSource = dtHorasC
                gv_horasChec.DataBind()
            Else
                fn_Alerta("No se encontro información en los rangos de fecha seleccionada.")
                Call MuestraHorasChecadasVacio()
            End If
        End If
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Mostrar.Click
        dtHorasC = Nothing

        If tbx_FechaIni.Text = "" Then
              fn_Alerta("Seleccione la Fecha Inicio.")
            Exit Sub
        End If

        If tbx_FechaFin.Text = "" Then
             fn_Alerta("Seleccione la Fecha Fin.")
            Exit Sub
        End If

        If CDate(tbx_FechaFin.Text) < CDate(tbx_FechaIni.Text) Then
                  fn_Alerta("La Fecha Fin no puede ser menor que la Fecha Inicio.")

            Exit Sub
        End If

        If DateDiff(DateInterval.Day, CDate(tbx_FechaIni.Text), CDate(tbx_FechaFin.Text)) > 31 Then
            fn_Alerta("El intervalo entre las fechas debe ser de 31 dias como máximo.")
            Exit Sub
        End If

        If ddl_Empleado.SelectedIndex = 0 And Not chk_Empleados.Checked Then
               fn_Alerta("Seleccione el Empleado.")

            Exit Sub
        End If

        gv_horasChec.PageIndex = 0
        Call LlenarHorasChecadas()
    End Sub

    Protected Sub chk_Empleados_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chk_Empleados.CheckedChanged
        MuestraHorasChecadasVacio()
        ddl_Empleado.Enabled = Not chk_Empleados.Checked
        ddl_Empleado.SelectedValue = 0
        ddl_Empleado.SelectedItem.Text = "Todos"
    End Sub

    Protected Sub gv_horasChec_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_horasChec.PageIndexChanging
        gv_horasChec.SelectedIndex = -1
        gv_horasChec.PageIndex = e.NewPageIndex
        dtHorasC = fn_HorasChecadas_LlenarLista(Session("DepartamentoID"), ddl_Empleado.SelectedValue, CDate(tbx_FechaIni.Text), CDate(tbx_FechaFin.Text))
        gv_horasChec.DataSource = dtHorasC
        gv_horasChec.DataBind()
    End Sub

    Protected Sub btn_Exportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Exportar.Click
        If gv_horasChec.Rows.Count = 1 Then
            fn_Alerta("No existe información para exportar.")
            Exit Sub
        End If
        Dim gv_Nuevo As New GridView
        gv_Nuevo.DataSource = fn_HorasChecadas_LlenarLista(Session("DepartamentoID"), ddl_Empleado.SelectedValue, CDate(tbx_FechaIni.Text), CDate(tbx_FechaFin.Text))
        gv_Nuevo.DataBind()
        gv_Nuevo.AllowPaging = False
        gv_Nuevo.EnableViewState = False
         
        For k As Byte = 0 To gv_horasChec.Columns.Count - 1
            gv_Nuevo.HeaderRow.Cells(k).Style.Add("background-color", "#BEBEBE")
        Next

        If Not fn_Exportar_Excel(gv_Nuevo, Page.Title, "Desde: " & tbx_FechaIni.Text & " Hasta: " & tbx_FechaFin.Text, "Filtrado Por : " & ddl_Empleado.SelectedItem.Text) Then
            fn_Alerta("Ha ocurrido un Error al generar el archivo en excel.")
        End If
    End Sub

    Protected Sub tbx_FechaIni_TextChanged(sender As Object, e As EventArgs) Handles tbx_FechaIni.TextChanged
        Call MuestraHorasChecadasVacio()
    End Sub

    Protected Sub tbx_FechaFin_TextChanged(sender As Object, e As EventArgs) Handles tbx_FechaFin.TextChanged
        Call MuestraHorasChecadasVacio()
    End Sub
End Class