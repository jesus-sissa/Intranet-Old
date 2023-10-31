Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.Cn_Login
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data

Partial Class Jornadas
    Inherits BasePage

    Dim Depto As Integer
    Dim Puesto As Integer
    Dim Empleado As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        'Se llena la lista desplegable de Puestos
        Dim dt_Puestos As DataTable = fn_Jornadas_ObtenerPuestos(Session("DepartamentoID"), Session("SucursalID"), Session("UsuarioID"))
        fn_LlenarDDL(ddl_Puesto, dt_Puestos, "Descripcion", "Id_Puesto", "0")

        Call MuestraGridsVacios()

        'Se llena la lista desplegable de Plantillas de Jornadas
        Dim dt_Plantillas As DataTable = fn_Jornadas_LlenarListaPlantillas(Session("SucursalID"), Session("DepartamentoID"))
        If dt_Plantillas.Rows.Count > 0 Then
            fn_LlenarDDL_VariosCampos(ddl_Jornada, dt_Plantillas, "Descripcion", "Id_Jornada")
        End If

        'Se llenan las listas desplegables de la sección de captura Manual de las Jornadas
        Call LlenarDDLsManual()

        Dim Columnas As Byte
        Dim Gv_Nombres() As GridView = {gv_Empleados, gv_Jornadas, gv_Plantillas}

        For j As Byte = 0 To Gv_Nombres.Length - 1
            Columnas = Gv_Nombres(j).Columns.Count

            For i As Byte = 0 To Columnas - 1
                Gv_Nombres(j).Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
            Next
        Next
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
                Call MuestraGridsVacios()
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
                Call JornadasVacio()
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
        Call MuestraGridsVacios()
        ddl_Puesto.Enabled = Not chk_Puesto.Checked
        ddl_Puesto.SelectedValue = 0
    End Sub

    Protected Sub ddl_Puesto_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Puesto.SelectedIndexChanged
        Call MuestraGridsVacios()
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Mostrar.Click
        If ddl_Puesto.SelectedIndex = 0 And Not chk_Puesto.Checked Then
              fn_Alerta("Seleccione el Puesto.")
            Exit Sub
        Else
            If chk_Puesto.Checked Then
                Puesto = 0
            Else
                Puesto = ddl_Puesto.SelectedValue
            End If
        End If

        Call LlenarEmpleados()
    End Sub

    Protected Sub gv_Empleados_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Empleados.SelectedIndexChanged
        If gv_Empleados.DataKeys(0).Values("Id_Empleado").ToString = "" Then
            gv_Empleados.SelectedIndex = -1
            Call JornadasVacio()
            Exit Sub
        End If
        gv_Jornadas.PageIndex = 0
        Call LlenarJornadas()
    End Sub

    Protected Sub gv_Empleados_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Empleados.PageIndexChanging
        gv_Empleados.PageIndex = e.NewPageIndex
        Call LlenarEmpleados()
    End Sub

    Protected Sub gv_Jornadas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Jornadas.PageIndexChanging
        gv_Jornadas.PageIndex = e.NewPageIndex
        Call LlenarJornadas()
    End Sub

    Protected Sub chk_NoAplica_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_NoAplica.CheckedChanged
        ddl_Jornada2De.Enabled = Not chk_NoAplica.Checked
        ddl_Jornada2A.Enabled = Not chk_NoAplica.Checked
    End Sub

    Protected Sub btn_Eliminar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Eliminar.Click

        Dim cant As Integer = 0
        Dim Dias() As String = ValidarDias(cant)
        If cant = 0 Then
               fn_Alerta("Debe seleccionar al menos un Dia.")
            Exit Sub
        End If

        If Not fn_Jornadas_Eliminar(Session("SucursalID"), Session("UsuarioID"), Dias) Then
                  fn_Alerta("Ha ocurrido un error al intentar eliminar el Día.")
        End If
        Call LlenarJornadas()
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
                fn_Alerta("Debe seleccionar al menos un Empleado.")
            Exit Sub
        End If

        If tbx_Desde.Text = "" Then
             fn_Alerta("Seleccione la Fecha Inicio.")
            tbx_Desde.Focus()
            Exit Sub
        End If

        If tbx_Hasta.Text = "" Then
                fn_Alerta("Seleccione la Fecha Fin.")
            tbx_Hasta.Focus()
            Exit Sub
        End If

        If CDate(tbx_Hasta.Text) < CDate(tbx_Desde.Text) Then
             fn_Alerta("La Fecha Hasta no puede ser menor que la Fecha Desde.")
            tbx_Hasta.Focus()
            Exit Sub
        End If

        If CDate(tbx_Hasta.Text) > DateAdd(DateInterval.Day, 30, Today.Date) Then
              fn_Alerta("Sólo puede programar movimientos para los próximos 30 días.")
            tbx_Hasta.Focus()
            Exit Sub
        End If

        If DateDiff(DateInterval.Day, CDate(tbx_Desde.Text), Today.Date) > 0 Then
             fn_Alerta("No se puede modificar Jornadas Laborales hacia atrás.")
            tbx_Desde.Focus()
            Exit Sub
        End If

        If tbc_Jornadas.ActiveTab.ID Is "tab_Manual" Then
            If ddl_Dia.SelectedValue = 0 Then
                     fn_Alerta("Seleccione el(los) Dia(s).")
                ddl_Dia.Focus()
                Exit Sub
            End If

            If ddl_Turno.SelectedValue = 0 Then
                    fn_Alerta("Seleccione el Turno.")
                ddl_Turno.Focus()
                Exit Sub
            End If
            If ddl_Jornada1De.SelectedIndex = 0 Or ddl_Jornada1A.SelectedIndex = 0 Then
                   fn_Alerta("Seleccione la Primera Parte su Jornada Laboral.")
                ddl_Jornada1De.Focus()
                Exit Sub
               
            End If
            J1 = ddl_Jornada1De.Text & "/" & ddl_Jornada1A.Text
            If ddl_Jornada2De.Enabled Then
                If ddl_Jornada2De.SelectedIndex = 0 Or ddl_Jornada2A.SelectedIndex = 0 Then
                          fn_Alerta("Seleccione de la Segunda Parte su Jornada Laboral.")
                    ddl_Jornada2De.Focus()
                    Exit Sub
                    
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
                    fn_Alerta("El o los Días que seleccionó no pueden agregarse porque no son Días correspondientes del Rango de Fechas deseado.")
                ddl_Dia.Focus()
                Exit Sub
            End If
        Else
            If ddl_Jornada.SelectedValue = "" OrElse ddl_Jornada.SelectedValue = 0 Then
                     fn_Alerta("Seleccione una Plantilla.")
                ddl_Jornada.Focus()
                Exit Sub
            End If
            ddl_Turno.SelectedValue = 0
        End If

        'Validar si hay empleados con Jornadas asignadas y preguntar si se quiere reemplazar

        Dim HayJornadas As Boolean = fn_Jornadas_Iguales(Empleados, ddl_Turno.SelectedValue, CDate(tbx_Desde.Text), CDate(tbx_Hasta.Text), Session("SucursalID"), Session("UsuarioID"), "")
        If HayJornadas Then
                   fn_Alerta("Los Empleados seleccionados ya tienen Jornadas Laborales asignadas en el rango de Fechas seleccionado y serán reemplazadas por las nuevas.")
        End If

        Select Case tbc_Jornadas.ActiveTab.ID
            Case "tab_Manual"
                If Not fn_Jornadas_GuardarManual(Empleados, CInt(ddl_Dia.SelectedValue), CDate(tbx_Desde.Text), CDate(tbx_Hasta.Text), J1, J2, ddl_Turno.SelectedValue, Session("SucursalID"), Session("UsuarioID"), "") Then
                         Exit Sub
                End If
            Case "tab_Plantillas"
                If Not fn_Jornadas_GuardarXPlantilla(Empleados, CDate(tbx_Desde.Text), CDate(tbx_Hasta.Text), ddl_Jornada.SelectedValue, Session("SucursalID"), Session("UsuarioID"), "") Then
                     fn_Alerta("Ocurrió un error al intentar guardar los datos.")
                    Exit Sub
                End If
        End Select

        Call ActualizarPagina()

    End Sub

    Sub ActualizarPagina()
        Call JornadasVacio()
        Call PlantillasVacio()
        Call LlenarJornadas()
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
                fn_Alerta("Seleccione un Empleado.")
            Exit Sub
        End If

        If tbx_DesdeEliminar.Text = "" Then
             fn_Alerta("Seleccione la Fecha Inicio.")
            Exit Sub
        End If

        If tbx_HastaEliminar.Text = "" Then
              fn_Alerta("Seleccione la Fecha Fin.")
            tbx_HastaEliminar.Focus()
            Exit Sub
        End If

        If CDate(tbx_HastaEliminar.Text) < CDate(tbx_DesdeEliminar.Text) Then
              fn_Alerta("La Fecha Hasta no puede ser menor que la Fecha Desde.")
            tbx_HastaEliminar.Focus()
            Exit Sub
        End If

        If Not fn_Jornadas_EliminarXRango(Session("SucursalID"), Session("UsuarioID"), gv_Empleados.SelectedDataKey.Values("Id_Empleado"), CDate(tbx_DesdeEliminar.Text), CDate(tbx_HastaEliminar.Text)) Then
                 fn_Alerta("Ha ocurrido un error al intentar eliminar el Día.")
        End If
        tbx_DesdeEliminar.Text = ""
        tbx_HastaEliminar.Text = ""
        Call LlenarJornadas()
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

    Protected Sub chkSeleccionaTodo_CheckedChanged(sender As Object, e As EventArgs)
        Dim ChkBoxHeader As CheckBox = DirectCast(gv_Jornadas.HeaderRow.FindControl("chkAll"), CheckBox)
        For Each row As GridViewRow In gv_Jornadas.Rows
            Dim ChkBoxRows As CheckBox = DirectCast(row.FindControl("chk_Dia"), CheckBox)
            If ChkBoxHeader.Checked = True Then
                ChkBoxRows.Checked = True
            Else
                ChkBoxRows.Checked = False
            End If
        Next
    End Sub
End Class
