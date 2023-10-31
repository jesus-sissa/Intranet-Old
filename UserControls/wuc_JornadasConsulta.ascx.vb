Imports SISSAIntranet.Cn_Soporte
Imports SISSAIntranet.Cn_Login
Imports SISSAIntranet.FuncionesGlobales
Imports System.Data
Imports System.Web.UI.Page

Partial Class wuc_JornadasConsulta
    Inherits System.Web.UI.UserControl

    Dim EmpleadoID As Integer = 0
    Dim TipoFalta As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Page.Title = "CONSULTA DE JORNADAS"

        '-----------------------------------------
        'Aquí se valida que el Usuario firmado tenga realmente Privilegios para acceder a esta página
        'o se esta introduciendo la página directo en la barra de direcciones

        Dim arr As String() = Split(Session("CadenaPriv"), ";")

        For x As Integer = 0 To arr.Length - 1
            If arr(x) = "CONSULTA DE JORNADAS LABORALES" Then
                Exit For
            ElseIf x = (arr.Length - 1) Then
                MostrarAlertAjax("NO TIENE PRIVILEGIOS PARA ACCEDER A ESTA OPCION", Me, Page)
                Response.Redirect("frm_login.aspx")
                Exit Sub
            End If
        Next

        '-------------------------------------------
        Dim dt_Empleados As DataTable = fn_Faltas_ObtenerEmpleados(Session("SucursalID"), Session("DepartamentoID"), Session("UsuarioID"))
        fn_LlenarDDL_VariosCampos(ddl_Empleado, dt_Empleados, "CveNombre", "Id_Empleado")
        MuestraJornadasVacio()
    End Sub

    Sub MuestraJornadasVacio()
        gv_Jornadas.DataSource = fn_CreaGridVacio("Id_Jornada,Clave,Nombre,Fecha,Dia,Jornada1,Jornada2,Turno,Checada1,Checada2,Checada3,Checada4,Retardo,HorasExtra,Recupera,TipoFalta")
        gv_Jornadas.DataBind()
    End Sub

    Protected Sub chk_Empleados_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Empleados.CheckedChanged
        MuestraJornadasVacio()
        ddl_Empleado.Enabled = Not chk_Empleados.Checked
        ddl_Empleado.SelectedValue = 0
    End Sub

    Protected Sub chk_TipoFalta_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_TipoFalta.CheckedChanged
        MuestraJornadasVacio()
        ddl_TipoFalta.Enabled = Not chk_TipoFalta.Checked
        ddl_TipoFalta.SelectedValue = 0
    End Sub

    Protected Sub ddl_Empleado_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Empleado.SelectedIndexChanged
        MuestraJornadasVacio()
    End Sub

    Protected Sub ddl_TipoFalta_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_TipoFalta.SelectedIndexChanged
        MuestraJornadasVacio()
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Mostrar.Click
        If tbx_FechaIni.Text = "" Then
            MostrarAlertAjax("Seleccione la Fecha Inicio.", btn_Mostrar, Page)
            tbx_FechaIni.Focus()
            Exit Sub
        End If

        If tbx_FechaFin.Text = "" Then
            MostrarAlertAjax("Seleccione la Fecha Fin.", btn_Mostrar, Page)
            tbx_FechaFin.Focus()
            Exit Sub
        End If

        If CDate(tbx_FechaFin.Text) < CDate(tbx_FechaIni.Text) Then
            MostrarAlertAjax("La Fecha Fin no puede ser menor que la Fecha Inicio.", btn_Mostrar, Page)
            tbx_FechaFin.Focus()
            Exit Sub
        End If

        If ddl_Empleado.SelectedIndex = 0 And Not chk_Empleados.Checked Then
            MostrarAlertAjax("Seleccione el Empleado.", btn_Mostrar, Page)
            ddl_Empleado.Focus()
            Exit Sub
        Else
            If chk_Empleados.Checked Then
                EmpleadoID = 0
            Else
                EmpleadoID = ddl_Empleado.SelectedValue
            End If
        End If
        If ddl_TipoFalta.SelectedIndex = 0 And Not chk_TipoFalta.Checked Then
            MostrarAlertAjax("Seleccione el Tipo de Falta.", btn_Mostrar, Page)
            ddl_TipoFalta.Focus()
            Exit Sub
        Else
            If chk_TipoFalta.Checked Then
                TipoFalta = 0
            Else
                TipoFalta = ddl_TipoFalta.SelectedValue
            End If
        End If
        gv_Jornadas.PageIndex = 0
        LlenarJornadas()
        mpe_Consulta.Show()
    End Sub

    Sub LlenarJornadas()
        Dim i As Integer = 0
        Dim cant As Integer = 0
        Dim TipoFalta As Integer = 0

        Dim dt As DataTable = fn_JornadasConsulta_LlenarLista(Session("SucursalID"), Session("UsuarioID"), Session("DepartamentoID"), ddl_Empleado.SelectedValue, CDate(tbx_FechaIni.Text), CDate(tbx_FechaFin.Text), ddl_TipoFalta.SelectedValue)

        If dt IsNot Nothing Then
            If dt.Rows.Count > 0 Then
                gv_Jornadas.DataSource = dt
                gv_Jornadas.DataBind()

                For i = 0 To (gv_Jornadas.Rows.Count - 1)
                    TipoFalta = gv_Jornadas.DataKeys(i).Values("TipoFalta")
                    If TipoFalta >= 0 Then
                        Select Case TipoFalta
                            Case 0
                                gv_Jornadas.Rows(i).Cells(7).Text = "X"
                            Case 1
                                gv_Jornadas.Rows(i).Cells(8).Text = "X"
                                gv_Jornadas.Rows(i).ForeColor = Drawing.Color.Red
                            Case 2
                                gv_Jornadas.Rows(i).Cells(9).Text = "X"
                                gv_Jornadas.Rows(i).ForeColor = Drawing.Color.DeepSkyBlue
                            Case 3
                                gv_Jornadas.Rows(i).Cells(10).Text = "X"
                                gv_Jornadas.Rows(i).ForeColor = Drawing.Color.LightSeaGreen
                            Case 4
                                gv_Jornadas.Rows(i).Cells(11).Text = "X"
                                gv_Jornadas.Rows(i).ForeColor = Drawing.Color.SaddleBrown
                        End Select
                    End If
                Next

            Else
                MuestraJornadasVacio()
            End If
        End If
    End Sub

    Protected Sub gv_Jornadas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Jornadas.PageIndexChanging
        gv_Jornadas.PageIndex = e.NewPageIndex
        LlenarJornadas()
        mpe_Consulta.Show()
    End Sub

End Class
