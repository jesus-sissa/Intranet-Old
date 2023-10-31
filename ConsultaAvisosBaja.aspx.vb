
Public Class ConsultaAvisosBaja
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If IsPostBack Then Exit Sub
      
        Cn_Login.fn_Log_Create(Session("UsuarioID"), Session("ModuloClave"), "ABRIR PAGINA: CONSULTA AVISOS DE BAJA", Session("EstacioN"), Session("EstacionIP"), "", Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

        Call MostrarGridVacio()
    End Sub

    Private Sub MostrarGridVacio()
        gv_Empleados.DataSource = FuncionesGlobales.fn_CreaGridVacio("Id_BajaAviso,Observaciones,FechaRegistro,UsuarioRegistro,FechaBaja,Clave,EmpleadoBaja,Departamento,Puesto")
        gv_Empleados.DataBind()
        tbx_Observaciones.Text = String.Empty
    End Sub

    Protected Sub btn_Mostrar_Click(sender As Object, e As EventArgs) Handles btn_Mostrar.Click

        If tbx_FechaIni.Text.Trim = "" Then
            fn_Alerta("Seleccione la Fecha Inicio.")
            Exit Sub
        End If

        If tbx_FechaFin.Text.Trim = "" Then
            fn_Alerta("Seleccione la Fecha Fin.")
            Exit Sub
        End If

        If CDate(tbx_FechaFin.Text) < CDate(tbx_FechaIni.Text) Then
            fn_Alerta("La Fecha Fin no puede ser menor que la Fecha Inicio.")
            Exit Sub
        End If

        Dim dt_Avisos As DataTable = Cn_Soporte.fn_Empleados_GetAvisoBaja(Session("SucursalID"), Session("UsuarioID"), CDate(tbx_FechaIni.Text), CDate(tbx_FechaFin.Text))

        If dt_Avisos Is Nothing Then
            fn_Alerta("Ocurrió un error al consultar los avisos de baja.")
            Exit Sub
        End If

        If dt_Avisos.Rows.Count > 0 Then
            gv_Empleados.DataSource = dt_Avisos
            gv_Empleados.DataBind()
        Else
            fn_Alerta("No se encontraron datos en ese rango de fechas.")
            Call MostrarGridVacio()
        End If

    End Sub

    Protected Sub gv_Empleados_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_Empleados.PageIndexChanging
        tbx_Observaciones.Text = String.Empty
        gv_Empleados.SelectedIndex = -1
        gv_Empleados.PageIndex = e.NewPageIndex
        gv_Empleados.DataSource = Cn_Soporte.fn_Empleados_GetAvisoBaja(Session("SucursalID"), Session("UsuarioID"), CDate(tbx_FechaIni.Text), CDate(tbx_FechaFin.Text))
        gv_Empleados.DataBind()
    End Sub

    Protected Sub gv_Empleados_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Empleados.SelectedIndexChanged
        If gv_Empleados.Rows(0).Cells(2).Text = "&nbsp;" Then Exit Sub

        tbx_Observaciones.Text = gv_Empleados.SelectedDataKey("Observaciones")
    End Sub

    Protected Sub tbx_FechaIni_TextChanged(sender As Object, e As EventArgs) Handles tbx_FechaIni.TextChanged
        Call MostrarGridVacio()
    End Sub

    Protected Sub tbx_FechaFin_TextChanged(sender As Object, e As EventArgs) Handles tbx_FechaFin.TextChanged
        Call MostrarGridVacio()
    End Sub
End Class