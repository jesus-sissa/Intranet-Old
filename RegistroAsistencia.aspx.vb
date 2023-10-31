Imports IntranetSIAC.FuncionesGlobales
Imports System.Data
Imports System.Web.UI.Page
Imports IntranetSIAC.Cn_Soporte

Partial Class RegistroAsistencia
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub
    
        Dim dt_Empleados As DataTable = fn_Faltas_ObtenerEmpleados(Session("SucursalID"), Session("DepartamentoID"), Session("UsuarioID"))
        fn_LlenarDDL_VariosCampos(ddl_Empleado, dt_Empleados, "CveNombre", "Id_Empleado")

        Session("Id_Jornada") = 0
        Session("Cadena") = ""

        btn_OK.OnClientClick = String.Format("fnClickOK('{0}','{1}')", btn_OK.UniqueID, "")
        btn_Cancelar.OnClientClick = String.Format("fnClickOK('{0}','{1}')", btn_Cancelar.UniqueID, "")

        hf_Respuesta.Value = 0

        Dim Columnas As Byte
        Dim Gv_Nombres() As GridView = {gv_Jornadas}

        For j As Byte = 0 To Gv_Nombres.Length - 1
            Columnas = Gv_Nombres(j).Columns.Count

            For i As Byte = 0 To Columnas - 1
                Gv_Nombres(j).Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
            Next
        Next
    End Sub

    Sub MuestraJornadasVacio()
        gv_Jornadas.DataSource = fn_CreaGridVacio("Id_Jornada,Fecha,Dia,Jornada1,Jornada2,Turno")
        gv_Jornadas.DataBind()
        gv_Jornadas.SelectedIndex = -1
    End Sub

    Private Sub Buscar()
        If tbx_FechaAsistencia.Text = "" Or ddl_Empleado.SelectedValue = 0 Then Exit Sub

        'Validar que la fecha de la asistencia no sea mayor a un día anterior a la actual y tampoco ser mayor a la fecha actual.
        Select Case DateDiff(DateInterval.Day, Now.Date, CDate(tbx_FechaAsistencia.Text))
            Case Is > 0
                btn_OK.Enabled = False
                btn_Cancelar.Text = "Cerrar"
                hf_Respuesta.Value = 2
                 fn_Alerta("La Fecha para la Asistencia no puede ser mayor a la Fecha Actual.")
                tbx_FechaAsistencia.Focus()
                Exit Sub
            Case Is < -1
                btn_OK.Enabled = False
                btn_Cancelar.Text = "Cerrar"
                hf_Respuesta.Value = 2
                 fn_Alerta("La Fecha para la Asistencia no debe ser menor a un día anterior a la Fecha Actual.")
                tbx_FechaAsistencia.Focus()
                Exit Sub
        End Select

        'Traer la Información del Guardia
        Dim Dt_Guardia As DataTable = fn_RegistroAsistencia_Buscar(tbx_Clave.Text, CDate(tbx_FechaAsistencia.Text), Session("SucursalID"), Session("UsuarioID"))
        If Dt_Guardia IsNot Nothing AndAlso Dt_Guardia.Rows.Count > 0 Then
            btn_OK.Enabled = True
            btn_Cancelar.Text = "Cancelar"
            hf_Respuesta.Value = 1
            If Dt_Guardia.Rows(0)("Asistio") = "S" Then
                
                lbl_Mensaje.Text = "El Empleado ya cuenta con un registro de Asistencia, desea Modificar la Información?"
                mpe_Ejemplo.Show()
            ElseIf Dt_Guardia.Rows(0)("Asistio") = "N" Then
                lbl_Mensaje.Text = "El Empleado ya cuenta con un registro de Falta, desea Modificar la Información?"
                mpe_Ejemplo.Show()
            Else
                pnl_TiposAsistencia.Enabled = True
            End If

            If Dt_Guardia.Rows(0)("Id_EmpleadoJornada") = 0 Then
                  fn_Alerta("El Empleado no tiene una Jornada Laboral para este día.")
                Exit Sub
            End If

            'Si viene distinto a un Tipo de Asistencia = 0(Asistencia) o 1(Falta) se podrá asignar una falta.
            rdb_Falta.Enabled = Dt_Guardia.Rows(0)("Tipo_Falta") = 0 OrElse Dt_Guardia.Rows(0)("Tipo_Falta") = 1

        Else
            lbl_Mensaje.Text = "El Empleado no tiene Jornada Laboral para este día."
            btn_OK.Enabled = False
            btn_Cancelar.Text = "Cerrar"
            hf_Respuesta.Value = 2
            mpe_Ejemplo.Show()
        End If
    End Sub

    Protected Sub tbx_Clave_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbx_Clave.TextChanged
        ddl_Empleado.SelectedValue = 0
        If tbx_Clave.Text.Length < 4 Then Exit Sub
        For elemento As Integer = 0 To ddl_Empleado.Items.Count - 1
            If Microsoft.VisualBasic.Left(ddl_Empleado.Items(elemento).Text, 4) = tbx_Clave.Text Then
                ddl_Empleado.SelectedIndex = elemento
            End If
        Next
    End Sub

    Protected Sub rdb_Asistencia_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdb_Asistencia.CheckedChanged
        tbx_Retardo.Enabled = Not rdb_Asistencia.Checked
        tbx_HorasExtra.Enabled = Not rdb_Asistencia.Checked
        btn_Guardar.Enabled = True
        Call MuestraJornadasVacio()
    End Sub

    Protected Sub rdb_Retardo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdb_Retardo.CheckedChanged
        tbx_Retardo.Enabled = rdb_Retardo.Checked
        tbx_HorasExtra.Enabled = Not rdb_Retardo.Checked
        btn_Guardar.Enabled = True
        Call MuestraJornadasVacio()
    End Sub

    Protected Sub rdb_HorasExtra_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdb_HorasExtra.CheckedChanged
        tbx_HorasExtra.Enabled = rdb_HorasExtra.Checked
        tbx_Retardo.Enabled = Not rdb_HorasExtra.Checked
        btn_Guardar.Enabled = True
        Call MuestraJornadasVacio()
    End Sub

    Protected Sub rdb_Falta_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdb_Falta.CheckedChanged
        tbx_Retardo.Enabled = Not rdb_Falta.Checked
        tbx_HorasExtra.Enabled = Not rdb_Falta.Checked
        btn_Guardar.Enabled = True
        Call MuestraJornadasVacio()
    End Sub

    Protected Sub rdb_RecuperarFalta_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdb_RecuperarFalta.CheckedChanged
        tbx_Retardo.Enabled = Not rdb_RecuperarFalta.Checked
        tbx_HorasExtra.Enabled = Not rdb_RecuperarFalta.Checked
        btn_Guardar.Enabled = True
        Call LlenarJornadas()
    End Sub

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Guardar.Click
        Dim TipoFalta As Integer = 0
        Dim TipoRegistro As String = ""

        If tbx_FechaAsistencia.Text = "" Then
             fn_Alerta("Seleccione la Fecha de la Asistencia.")
            tbx_FechaAsistencia.Focus()
            Exit Sub
        End If
        If ddl_Empleado.SelectedValue = 0 Then
             fn_Alerta("Seleccione el Empleado.")
            ddl_Empleado.Focus()
            Exit Sub
        End If

        If rdb_Retardo.Checked AndAlso (tbx_Retardo.Text = "" OrElse tbx_Retardo.Text = 0) Then
               fn_Alerta("Capture los Minutos de Retraso.")
            tbx_Retardo.Focus()
            Exit Sub
        End If
        If rdb_HorasExtra.Checked AndAlso Val(tbx_HorasExtra.Text) = 0 Then
            fn_Alerta("Capture la cantidad de Horas Extras trabajadas.")
            tbx_HorasExtra.Focus()
            Exit Sub
        End If

        If rdb_RecuperarFalta.Checked AndAlso Session("Id_Jornada") = 0 Then
             fn_Alerta("Seleccione el día que se Recuperará la Falta.")
            Exit Sub
        End If

        If rdb_Asistencia.Checked OrElse rdb_HorasExtra.Checked OrElse rdb_Retardo.Checked OrElse rdb_RecuperarFalta.Checked Then
            TipoFalta = 0 'Asistencia
            TipoRegistro = "Asistencia"
        ElseIf rdb_Falta.Checked Then
            TipoFalta = 1 'Falta
            TipoRegistro = "Falta"
        End If

        'Traer la Información del Guardia
        Dim Dt_Guardia As DataTable = fn_RegistroAsistencia_Buscar(tbx_Clave.Text, CDate(tbx_FechaAsistencia.Text), Session("SucursalID"), Session("UsuarioID"))

        If Not fn_RegistroAsistencia_Guardar(Dt_Guardia, TipoFalta, Val(tbx_HorasExtra.Text), tbx_Observaciones.Text, Val(tbx_Retardo.Text), Session("Id_Jornada"), Session("SucursalID"), Session("UsuarioID"), Session("EstacioN")) Then
              fn_Alerta("Ha ocurrido un error al intentar registrar la Asistencia.")
            Exit Sub
        Else
            hf_Respuesta.Value = 0
            lbl_Mensaje.Text = "Se ha completado el Registro de " & TipoRegistro & " correctamente."
            mpe_Ejemplo.Show()
        End If

        tbx_FechaAsistencia.Text = ""
        tbx_Clave.Text = ""
        ddl_Empleado.SelectedValue = 0
        Call ActualizaPanelAsistencia()
        Call MuestraJornadasVacio()
    End Sub

    Sub LlenarJornadas()
        Dim dt As DataTable = fn_RegistrarAsistencia_LlenarListaFaltas(Session("SucursalID"), Session("UsuarioID"), ddl_Empleado.SelectedValue)
        If dt IsNot Nothing Then
            If dt.Rows.Count > 0 Then
                gv_Jornadas.DataSource = dt
                gv_Jornadas.DataBind()
            Else
                'JornadasVacio()
            End If
        End If
    End Sub

    Sub ActualizaPanelAsistencia()
        rdb_Asistencia.Checked = False
        rdb_Falta.Checked = False
        rdb_HorasExtra.Checked = False
        rdb_Retardo.Checked = False
        rdb_RecuperarFalta.Checked = False

        tbx_HorasExtra.Text = ""
        tbx_Retardo.Text = ""
        tbx_Observaciones.Text = ""

        tbx_HorasExtra.Enabled = False
        tbx_Retardo.Enabled = False
        btn_Guardar.Enabled = False

        pnl_TiposAsistencia.Enabled = False
    End Sub

    Protected Sub ddl_Empleado_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Empleado.SelectedIndexChanged
        Call ActualizaPanelAsistencia()

        If ddl_Empleado.SelectedValue = 0 Then
            tbx_Clave.Text = ""
        Else
            tbx_Clave.Text = Microsoft.VisualBasic.Left(ddl_Empleado.SelectedItem.Text, 4)
            Buscar()
        End If

        Call MuestraJornadasVacio()
    End Sub

    Protected Sub gv_Jornadas_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Jornadas.SelectedIndexChanged
        If Val(gv_Jornadas.SelectedDataKey.Values("Id_Jornada")) = 0 Then
            Session("Id_Jornada") = 0
            Exit Sub
        Else
            Session("Id_Jornada") = gv_Jornadas.SelectedDataKey.Values("Id_Jornada")
        End If
    End Sub

    Protected Sub tbx_FechaAsistencia_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbx_FechaAsistencia.TextChanged
        Call ActualizaPanelAsistencia()
        Call Buscar()
        Call MuestraJornadasVacio()
    End Sub

    Protected Sub btn_OK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_OK.Click
        If hf_Respuesta.Value = 2 Then
            tbx_FechaAsistencia.Focus()
            Exit Sub
        End If

        pnl_TiposAsistencia.Enabled = True
    End Sub

    Protected Sub btn_Cancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Cancelar.Click
        If hf_Respuesta.Value = 2 Then
            tbx_FechaAsistencia.Text = ""
            tbx_FechaAsistencia.Focus()
            Exit Sub
        End If
        pnl_TiposAsistencia.Enabled = False
    End Sub

End Class
