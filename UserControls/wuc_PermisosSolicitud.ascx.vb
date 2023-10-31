Imports SISSAIntranet.Cn_Soporte
Imports SISSAIntranet.Cn_Login
Imports SISSAIntranet.FuncionesGlobales
Imports System.Data
Imports SISSAIntranet.BasePage

Partial Public Class wuc_PermisosSolicitud
    Inherits System.Web.UI.UserControl
    'Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Page.Title = "PERMISOS"


        '-----------------------------------------
        'Aquí se valida que el Usuario firmado tenga realmente Privilegios para acceder a esta página
        'o se esta introduciendo la página directo en la barra de direcciones

        Dim arr As String() = Split(Session("CadenaPriv"), ";")

        For x As Integer = 0 To arr.Length - 1
            If arr(x) = "REGISTRO DE PERMISOS" Then
                Exit For
            ElseIf x = (arr.Length - 1) Then
                MostrarAlertAjax("NO TIENE PRIVILEGIOS PARA ACCEDER A ESTA OPCION", Me, Page)
                Response.Redirect("frm_login.aspx")
                Exit Sub
            End If
        Next

        '-------------------------------------------

        fn_Log_Create(Session("UsuarioID"), Session("ModuloClave"), "ABRIR PAGINA: REGISTRO DE PERMISOS", Session("EstacioN"), Session("EstacionIP"), "", Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

        Call LlenarEmpleados()

        Call LlenarDDLsManual()
    End Sub

    Sub MuestraGridsVacios()
        gv_Empleados.DataSource = fn_CreaGridVacio("Id_Empleado,Clave,Nombre")
        gv_Empleados.DataBind()
        gv_Empleados.SelectedIndex = -1
    End Sub

    Sub LlenarEmpleados()
        Dim dt As DataTable

        If Session("Dpto_Reclutamiento") = Session("DepartamentoId") Then
            dt = fn_Default_GetEmpleados(Session("SucursalID"), 0, "N", Session("UsuarioID"))
        Else
            dt = fn_Default_GetEmpleados(Session("SucursalID"), Session("DepartamentoID"), "N", Session("UsuarioID"))
        End If

        If dt IsNot Nothing Then
            If dt.Rows.Count > 0 Then
                gv_Empleados.DataSource = dt
                gv_Empleados.DataBind()
            Else
                gv_Empleados.DataSource = fn_CreaGridVacio("Id_Empleado,Clave,Nombre")
                gv_Empleados.DataBind()
            End If
        End If
    End Sub

    Sub LlenarDDLsManual()
        LlenarMinutos(ddl_Jornada1De, 30)
        ddl_Jornada1De.SelectedValue = 0

        LlenarMinutos(ddl_Jornada1A, 30)
        ddl_Jornada1A.SelectedValue = 0

        LlenarMinutos(ddl_Jornada2De, 30)
        ddl_Jornada2De.SelectedValue = 0

        LlenarMinutos(ddl_Jornada2A, 30)
        ddl_Jornada2A.SelectedValue = 0
    End Sub

    Sub LimpiarObjetos()
        tbx_Fecha.Text = ""
        tbx_Jornada1.Text = ""
        tbx_Jornada2.Text = ""
        ddl_Jornada1De.SelectedIndex = 0
        ddl_Jornada1A.SelectedIndex = 0
        ddl_Jornada2De.SelectedIndex = 0
        ddl_Jornada2A.SelectedIndex = 0
        tbx_Motivos.Text = ""
        udp_DatosPermiso.Update()
    End Sub

    Private Sub gv_Empleados_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Empleados.RowCreated
        ' En este Sub se agregan a las filas de dgv_Empleados los atributos "onmouseover" y "onmouseout"
        ' para que cuando el puntero del mouse este sobre una fila, se apliquen las propiedades declaradas (backgoundColor)

        ' only apply changes if its DataRow
        If e.Row.RowType = DataControlRowType.DataRow Then

            ' when mouse is over the row, save original color to new attribute, and change it to highlight yellow color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#C0A062'")

            ' when mouse leaves the row, change the bg color to its original value    
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")

        End If
    End Sub

    Protected Sub gv_Empleados_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Empleados.SelectedIndexChanged
        If Val(gv_Empleados.SelectedDataKey.Values("Id_Empleado")) = 0 Then
            gv_Empleados.SelectedIndex = -1
            Exit Sub
        Else
            Call LimpiarObjetos()
        End If
    End Sub

    Protected Sub tbx_Fecha_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbx_Fecha.TextChanged
        If Len(tbx_Fecha.Text) > 1 Then
            If tbx_Fecha.Text >= Now.Date Then
                Dim dr As DataRow = fn_PermisosRegistro_ObtenerJornada(gv_Empleados.SelectedValue, tbx_Fecha.Text, Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"))
                If dr IsNot Nothing Then
                    tbx_Jornada1.Text = dr.Item("Jornada1")
                    tbx_Jornada2.Text = dr.Item("Jornada2")
                    udp_DatosPermiso.Update()
                End If
            Else
                MostrarAlertAjax("La Fecha del Permiso debe ser el día de hoy o uno posterior.", tbx_Fecha, Page)
                'fn_Alerta("La Fecha del Permiso debe ser el día de hoy o uno posterior.")
                LimpiarObjetos()
                tbx_Fecha.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Guardar.Click

    End Sub
End Class