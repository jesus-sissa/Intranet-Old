
Imports SISSAIntranet.Cn_Soporte
Imports SISSAIntranet.Cn_Login
Imports SISSAIntranet.FuncionesGlobales
Imports System.Data
Imports System.Web.UI.Page

Partial Class wuc_Jornadas
    Inherits System.Web.UI.UserControl

    Dim Depto As Integer
    Dim Puesto As Integer
    Dim Empleado As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Page.Title = "JORNADAS"

        '-----------------------------------------
        'Aquí se valida que el Usuario firmado tenga realmente Privilegios para acceder a esta página
        'o se esta introduciendo la página directo en la barra de direcciones

        Dim arr As String() = Split(Session("CadenaPriv"), ";")

        For x As Integer = 0 To arr.Length - 1
            If arr(x) = "JORNADAS LABORALES" Then
                Exit For
            ElseIf x = (arr.Length - 1) Then
                MostrarAlertAjax("NO TIENE PRIVILEGIOS PARA ACCEDER A ESTA OPCION", Me, Page)
                Response.Redirect("frm_login.aspx")
                Exit Sub
            End If
        Next

        '-------------------------------------------

        'Se llena la lista desplegable de Puestos
        Dim dt_Puestos As DataTable = fn_Jornadas_ObtenerPuestos(Session("DepartamentoID"), Session("SucursalID"), Session("UsuarioID"))
        fn_LlenarDDL(ddl_Puesto, dt_Puestos, "Descripcion", "Id_Puesto", "0")

        MuestraGridsVacios()

        'Se llena la lista desplegable de Plantillas de Jornadas
        Dim dt_Plantillas As DataTable = fn_Jornadas_LlenarListaPlantillas(Session("SucursalID"), Session("DepartamentoID"))
        If dt_Plantillas.Rows.Count > 0 Then
            fn_LlenarDDL_VariosCampos(ddl_Jornada, dt_Plantillas, "Descripcion", "Id_Jornada")
        End If

        'Se llenan las listas desplegables de la sección de captura Manual de las Jornadas
        LlenarDDLsManual()

    End Sub

    Sub MuestraGridsVacios()
        gv_Empleados.DataSource = fn_CreaGridVacio("Id_Empleado,Clave,Nombre")
        gv_Empleados.DataBind()
        gv_Empleados.SelectedIndex = -1
        gv_Jornadas.DataSource = fn_CreaGridVacio("Id_Jornada,Fecha,Dia,Jornada1,Jornada2,Turno")
        gv_Jornadas.DataBind()
        gv_Plantillas.DataSource = fn_CreaGridVacio("Id_Jornada,Turno,Dia,PrimerJornada,SegundaJornada")
        gv_Plantillas.DataBind()
    End Sub

    Sub JornadasVacio()
        gv_Jornadas.DataSource = fn_CreaGridVacio("Id_Jornada,Fecha,Dia,Jornada1,Jornada2,Turno")
        gv_Jornadas.DataBind()
    End Sub

    Sub PlantillasVacio()
        gv_Plantillas.DataSource = fn_CreaGridVacio("Id_Jornada,Turno,Dia,PrimerJornada,SegundaJornada")
        gv_Plantillas.DataBind()
    End Sub

    Sub LlenarEmpleados()
        Dim dt_Empleados As DataTable = fn_Jornadas_ObtenerEmpleados(Session("SucursalID"), Session("DepartamentoID"), Puesto, Session("UsuarioID"))

        If dt_Empleados IsNot Nothing Then
            If dt_Empleados.Rows.Count > 0 Then
                gv_Empleados.DataSource = dt_Empleados
                gv_Empleados.DataBind()
            Else
                MuestraGridsVacios()
            End If
        End If
    End Sub

    Sub LlenarJornadas()
        btn_Eliminar.Enabled = False
        btn_EliminarRango.Enabled = False
        Dim dt As DataTable = fn_Jornadas_ConsultaJornadas(Session("SucursalID"), Session("UsuarioID"), gv_Empleados.SelectedValue)
        If dt IsNot Nothing Then
            If dt.Rows.Count > 0 Then
                gv_Jornadas.DataSource = dt
                gv_Jornadas.DataBind()
                btn_Eliminar.Enabled = True
                btn_EliminarRango.Enabled = True
            Else
                JornadasVacio()
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

    Protected Sub chk_Puesto_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Puesto.CheckedChanged
        MuestraGridsVacios()
        ddl_Puesto.Enabled = Not chk_Puesto.Checked
        ddl_Puesto.SelectedValue = 0
    End Sub

    Protected Sub ddl_Puesto_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Puesto.SelectedIndexChanged
        MuestraGridsVacios()
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Mostrar.Click
        If ddl_Puesto.SelectedIndex = 0 And Not chk_Puesto.Checked Then
            MostrarAlertAjax("Seleccione el Puesto.", btn_Mostrar, Page)
            ddl_Puesto.Focus()
            Exit Sub
        Else
            If chk_Puesto.Checked Then
                Puesto = 0
            Else
                Puesto = ddl_Puesto.SelectedValue
            End If
        End If

        LlenarEmpleados()
    End Sub

    Protected Sub gv_Empleados_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Empleados.SelectedIndexChanged
        If gv_Empleados.DataKeys(0).Values("Id_Empleado").ToString = "" Then
            gv_Empleados.SelectedIndex = -1
            JornadasVacio()
            Exit Sub
        End If
        gv_Jornadas.PageIndex = 0
        LlenarJornadas()
    End Sub

    Protected Sub gv_Empleados_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Empleados.PageIndexChanging
        gv_Empleados.PageIndex = e.NewPageIndex
        LlenarEmpleados()
    End Sub

    Protected Sub gv_Jornadas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Jornadas.PageIndexChanging
        gv_Jornadas.PageIndex = e.NewPageIndex
        LlenarJornadas()
    End Sub

    Protected Sub chk_NoAplica_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_NoAplica.CheckedChanged
        ddl_Jornada2De.Enabled = Not chk_NoAplica.Checked
        ddl_Jornada2A.Enabled = Not chk_NoAplica.Checked
    End Sub

    Protected Sub btn_Eliminar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Eliminar.Click
        If gv_Empleados.SelectedIndex = -1 Then
            MostrarAlertAjax("Seleccione un Empleado.", btn_EliminarRango, Page)
            Exit Sub
        End If
        'If gv_Jornadas.DataKeys(0).Values("Id_Jornada").ToString = "" Then
        '    MostrarAlertAjax("No hay Jornadas en la lista.", btn_Eliminar, Page)
        '    Exit Sub
        'End If

        Dim cant As Integer = 0
        Dim Dias() As String = ValidarDias(cant)
        If cant = 0 Then
            MostrarAlertAjax("Debe seleccionar al menos un Dia.", btn_Eliminar, Page)
            Exit Sub
        End If

        If Not fn_Jornadas_Eliminar(Session("SucursalID"), Session("UsuarioID"), Dias) Then
            MostrarAlertAjax("Ha ocurrido un error al intentar eliminar el Día.", btn_Eliminar, Page)
        End If
        LlenarJornadas()
    End Sub

    Function ValidarDias(ByRef cant As Integer) As String()
        Dim row As GridViewRow
        Dim ischecked As Boolean = False
        Dim arrMod(0) As String
        For i As Integer = 0 To gv_Jornadas.Rows.Count - 1
            row = gv_Jornadas.Rows(i)
            ischecked = DirectCast(row.FindControl("chk_Dia"), CheckBox).Checked
            If ischecked Then
                ReDim Preserve arrMod(cant)
                arrMod(cant) = gv_Jornadas.DataKeys(i).Values("Id_Jornada")
                cant += 1
            End If
        Next
        Return arrMod
    End Function

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Guardar.Click
        Dim J1 As String
        Dim J2 As String

        Dim cant As Integer = 0
        Dim Empleados() As String = ValidarEmpleados(cant)
        If Empleados Is Nothing Then
            MostrarAlertAjax("Debe seleccionar al menos un Empleado.", btn_Guardar, Page)
            Exit Sub
        End If

        If tbx_Desde.Text = "" Then
            MostrarAlertAjax("Seleccione la Fecha Inicio.", btn_Guardar, Page)
            tbx_Desde.Focus()
            Exit Sub
        End If

        If tbx_Hasta.Text = "" Then
            MostrarAlertAjax("Seleccione la Fecha Fin.", btn_Guardar, Page)
            tbx_Hasta.Focus()
            Exit Sub
        End If

        If CDate(tbx_Hasta.Text) < CDate(tbx_Desde.Text) Then
            MostrarAlertAjax("La Fecha Hasta no puede ser menor que la Fecha Desde.", btn_Guardar, Page)
            tbx_Hasta.Focus()
            Exit Sub
        End If

        If CDate(tbx_Hasta.Text) > DateAdd(DateInterval.Day, 30, Today.Date) Then
            MostrarAlertAjax("Sólo puede programar movimientos para los próximos 30 días.", btn_Guardar, Page)
            tbx_Hasta.Focus()
            Exit Sub
        End If

        If DateDiff(DateInterval.Day, CDate(tbx_Desde.Text), Today.Date) > 0 Then
            MostrarAlertAjax("No se puede modificar Jornadas Laborales hacia atrás.", btn_Guardar, Page)
            tbx_Desde.Focus()
            Exit Sub
        End If

        If tbc_Jornadas.ActiveTab.ID Is "tab_Manual" Then
            If ddl_Dia.SelectedValue = 0 Then
                MostrarAlertAjax("Seleccione el(los) Dia(s).", btn_Guardar, Page)
                ddl_Dia.Focus()
                Exit Sub
            End If

            If ddl_Turno.SelectedValue = 0 Then
                MostrarAlertAjax("Seleccione el Turno.", btn_Guardar, Page)
                ddl_Turno.Focus()
                Exit Sub
            End If
            If ddl_Jornada1De.SelectedIndex = 0 Or ddl_Jornada1A.SelectedIndex = 0 Then
                MostrarAlertAjax("Seleccione la Primera Parte su Jornada Laboral.", btn_Guardar, Page)
                ddl_Jornada1De.Focus()
                Exit Sub
                'ElseIf cmb_Jornada1.SelectedValue = cmb_Jornada1A.SelectedValue Then
                '    Me.Cursor = Cursors.Default
                '    MsgBox("El rango de la Primera Parte de la Jornada Laboral no es correcta.", MsgBoxStyle.Critical, frm_MENU.Text)
                '    cmb_Jornada1.Focus()
                '    Exit Sub
            End If
            J1 = ddl_Jornada1De.Text & "/" & ddl_Jornada1A.Text
            If ddl_Jornada2De.Enabled Then
                If ddl_Jornada2De.SelectedIndex = 0 Or ddl_Jornada2A.SelectedIndex = 0 Then
                    MostrarAlertAjax("Seleccione de la Segunda Parte su Jornada Laboral.", btn_Guardar, Page)
                    ddl_Jornada2De.Focus()
                    Exit Sub
                    'ElseIf cmb_Jornada2.SelectedValue = cmb_Jornada2A.SelectedValue Then
                    '    Me.Cursor = Cursors.Default
                    '    MsgBox("El rango de la Segunda Parte de la Jornada Laboral no es correcta.", MsgBoxStyle.Critical, frm_MENU.Text)
                    '    cmb_Jornada2.Focus()
                    '    Exit Sub
                End If
                J2 = ddl_Jornada2De.Text & "/" & ddl_Jornada2A.Text
            Else
                J2 = "00:00/00:00"
            End If

            'Validar si es posible la ejecución o no, dependiendo del dia seleccionado
            'y el dia de la semana segun la fecha seleccionada
            Dim DiaSemana As Integer
            Dim ArrDia() As String
            Dim Dia As String

            Select Case ddl_Dia.SelectedValue
                Case 8 'De L-V
                    Dia = "2,3,4,5,6"
                Case 9 'De L-S
                    Dia = "2,3,4,5,6,7"
                Case 10 'De L-D
                    Dia = "1,2,3,4,5,6,7"
                Case 11 'S y D
                    Dia = "1,7"
                Case Else
                    Dia = ddl_Dia.SelectedValue
            End Select
            ArrDia = Split(Dia, ",")

            Dim Encontrado As Boolean = False
            Dim CantidadDias As Integer = DateDiff(DateInterval.Day, CDate(tbx_Desde.Text), CDate(tbx_Hasta.Text))

            For Cantidad As Integer = 0 To CantidadDias
                DiaSemana = Microsoft.VisualBasic.DatePart(DateInterval.Weekday, DateAdd(DateInterval.Day, Cantidad, CDate(tbx_Desde.Text)), Microsoft.VisualBasic.FirstDayOfWeek.Sunday)
                For Index As Integer = 0 To (ArrDia.Length - 1)
                    If DiaSemana = ArrDia(Index) Then
                        Encontrado = True
                    End If
                Next
            Next Cantidad

            If Encontrado = False Then
                MostrarAlertAjax("El o los Días que seleccionó no pueden agregarse porque no son Días correspondientes del Rango de Fechas deseado.", btn_Guardar, Page)
                ddl_Dia.Focus()
                Exit Sub
            End If
        Else
            If ddl_Jornada.SelectedValue = "" OrElse ddl_Jornada.SelectedValue = 0 Then
                MostrarAlertAjax("Seleccione una Plantilla.", btn_Guardar, Page)
                ddl_Jornada.Focus()
                Exit Sub
            End If
            ddl_Turno.SelectedValue = 0
        End If

        'Validar si hay empleados con Jornadas asignadas y preguntar si se quiere reemplazar

        Dim HayJornadas As Boolean = fn_Jornadas_Iguales(Empleados, ddl_Turno.SelectedValue, CDate(tbx_Desde.Text), CDate(tbx_Hasta.Text), Session("SucursalID"), Session("UsuarioID"), "")
        If HayJornadas Then
            MostrarAlertAjax("Los Empleados seleccionados ya tienen Jornadas Laborales asignadas en el rango de Fechas seleccionado y serán reemplazadas por las nuevas.", btn_Guardar, Page)
        End If

        'Dim alerta As Page = Page.LoadControl("~\UserControls\Alerta.ascx")
        'Dim ale As Control = LoadControl("~\UserControls\Alerta.ascx")
        'Page.Controls.Add(ale)

        'alerta.Alerta("Los Empleados seleccionados ya tienen Jornadas Laborales asignadas en el rango de Fechas seleccionado y serán reemplazadas por las nuevas.")


        Select Case tbc_Jornadas.ActiveTab.ID
            Case "tab_Manual"
                If Not fn_Jornadas_GuardarManual(Empleados, CInt(ddl_Dia.SelectedValue), CDate(tbx_Desde.Text), CDate(tbx_Hasta.Text), J1, J2, ddl_Turno.SelectedValue, Session("SucursalID"), Session("UsuarioID"), "") Then
                    '            MostrarAlertAjax("Ocurrió un error al intentar guardar los datos.", btn_Guardar, Page)
                    Exit Sub
                End If
            Case "tab_Plantillas"
                If Not fn_Jornadas_GuardarXPlantilla(Empleados, CDate(tbx_Desde.Text), CDate(tbx_Hasta.Text), ddl_Jornada.SelectedValue, Session("SucursalID"), Session("UsuarioID"), "") Then
                    MostrarAlertAjax("Ocurrió un error al intentar guardar los datos.", btn_Guardar, Page)
                    Exit Sub
                End If
        End Select

        ActualizarPagina()

    End Sub

    Sub ActualizarPagina()
        JornadasVacio()
        PlantillasVacio()
        LlenarJornadas()
        tbx_Desde.Text = ""
        tbx_Hasta.Text = ""
        tbc_Jornadas.ActiveTabIndex = 0
        ddl_Jornada.SelectedValue = 0
        ddl_Dia.SelectedValue = 0
        ddl_Turno.SelectedValue = 0
        ddl_Jornada1De.SelectedIndex = 0
        ddl_Jornada1A.SelectedIndex = 0
        ddl_Jornada2De.SelectedIndex = 0
        ddl_Jornada2A.SelectedIndex = 0
        ddl_Jornada2De.Enabled = True
        ddl_Jornada2A.Enabled = True
        chk_NoAplica.Checked = False
    End Sub

    Function ValidarEmpleados(ByVal cant As Integer) As String()
        'Esta función da como resultado un arreglo con el ID de todo los Empleados que se hayan chequeado para asignarles Jornadas laborales
        Dim row As GridViewRow
        Dim ischecked As Boolean = False
        Dim arrEmpleados() As String
        For i As Integer = 0 To gv_Empleados.Rows.Count - 1
            row = gv_Empleados.Rows(i)
            ischecked = DirectCast(row.FindControl("chk_Empleado"), CheckBox).Checked
            If ischecked Then
                ReDim Preserve arrEmpleados(cant)
                arrEmpleados(cant) = gv_Empleados.DataKeys(i).Values("Id_Empleado")
                cant += 1
            End If
        Next
        Return arrEmpleados
    End Function

    Protected Sub btn_EliminarRango_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_EliminarRango.Click
        If gv_Empleados.SelectedIndex = -1 Then
            MostrarAlertAjax("Seleccione un Empleado.", btn_EliminarRango, Page)
            Exit Sub
        End If

        'If gv_Jornadas.DataKeys(0).Values("Id_Jornada").ToString = "" Then
        '    MostrarAlertAjax("No hay Jornadas en la lista.", btn_EliminarRango, Page)
        '    Exit Sub
        'End If

        If tbx_DesdeEliminar.Text = "" Then
            MostrarAlertAjax("Seleccione la Fecha Inicio.", btn_EliminarRango, Page)
            tbx_DesdeEliminar.Focus()
            Exit Sub
        End If

        If tbx_HastaEliminar.Text = "" Then
            MostrarAlertAjax("Seleccione la Fecha Fin.", btn_EliminarRango, Page)
            tbx_HastaEliminar.Focus()
            Exit Sub
        End If

        If CDate(tbx_HastaEliminar.Text) < CDate(tbx_DesdeEliminar.Text) Then
            MostrarAlertAjax("La Fecha Hasta no puede ser menor que la Fecha Desde.", btn_EliminarRango, Page)
            tbx_HastaEliminar.Focus()
            Exit Sub
        End If

        If Not fn_Jornadas_EliminarXRango(Session("SucursalID"), Session("UsuarioID"), gv_Empleados.SelectedDataKey.Values("Id_Empleado"), CDate(tbx_DesdeEliminar.Text), CDate(tbx_HastaEliminar.Text)) Then
            MostrarAlertAjax("Ha ocurrido un error al intentar eliminar el Día.", btn_EliminarRango, Page)
        End If
        tbx_DesdeEliminar.Text = ""
        tbx_HastaEliminar.Text = ""
        LlenarJornadas()
    End Sub

    Protected Sub gv_Plantillas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Plantillas.SelectedIndexChanged
        If gv_Plantillas.DataKeys(0).Values("Id_Jornada").ToString = "" Then
            gv_Plantillas.SelectedIndex = -1
            Exit Sub
        End If
    End Sub

    Protected Sub ddl_Jornada_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Jornada.SelectedIndexChanged
        Dim dt As DataTable = fn_Jornadas_PlantillasDetalle(Session("SucursalID"), Session("UsuarioID"), ddl_Jornada.SelectedValue)

        If dt IsNot Nothing Then
            If dt.Rows.Count > 0 Then
                gv_Plantillas.DataSource = dt
                gv_Plantillas.DataBind()
            Else
                PlantillasVacio()
            End If
        End If
    End Sub

End Class
