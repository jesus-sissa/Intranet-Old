Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.FuncionesGlobales

Partial Public Class CartaAccesoGenerar
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Dim dt_Empleados As DataTable = fn_Faltas_ObtenerEmpleados(Session("SucursalID"), Session("DepartamentoID"), Session("UsuarioID"))
        fn_LlenarDDL_VariosCampos(ddl_Empleado, dt_Empleados, "Nombre", "Id_Empleado")

        gv_Empleados.DataSource = fn_CreaGridVacio("Id_Empleado,Nombre")
        gv_Empleados.DataBind()

        dt_Empleados = fn_GenerarCartasAcceso_LlenarComboEmpleados(Session("SucursalID"), Session("UsuarioID"))
        fn_LlenarDDL_VariosCampos(ddl_Autoriza, dt_Empleados, "Nombre", "Id_Empleado")

        dt_Empleados = fn_GenerarCartasAcceso_LlenarComboEmpleados(Session("SucursalID"), Session("UsuarioID"))
        fn_LlenarDDL_VariosCampos(ddl_Dirigida, dt_Empleados, "Nombre", "Id_Empleado")

        Session("UsuariosAgregadosCarta") = ""

    End Sub

    Protected Sub rdb_Empleado_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdb_Empleado.CheckedChanged
        tbx_Asunto.Text = ""
        lbl_Empleado.Enabled = True
        ddl_Empleado.Enabled = True
        lbl_Nombre.Enabled = False
        tbx_Nombre.Text = ""
        tbx_Nombre.Enabled = False
        tbx_Nombre.ReadOnly = True
        lbl_Empresa.Enabled = False
        tbx_Empresa.Text = Session("NombreEmpresa")
        tbx_Empresa.Enabled = False
        tbx_Empresa.ReadOnly = True
        btn_Agregar.Enabled = True
        rdb_NuevoIngreso.Enabled = True
        rdb_FaltaRetardo.Enabled = True
        rdb_Externo.Enabled = False
        rdb_Otro.Enabled = True

        rdb_NuevoIngreso.Checked = False
        rdb_FaltaRetardo.Checked = False
        rdb_Externo.Checked = False
        rdb_Otro.Checked = False

        ddl_Empleado.Focus()
    End Sub

    Protected Sub rdb_Visitante_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdb_Visitante.CheckedChanged
        tbx_Asunto.Text = ""

        lbl_Empleado.Enabled = False
        ddl_Empleado.Enabled = False
        ddl_Empleado.SelectedValue = 0
        lbl_Nombre.Enabled = True
        tbx_Nombre.Enabled = True
        tbx_Nombre.ReadOnly = False
        lbl_Empresa.Enabled = True
        tbx_Empresa.Text = ""
        tbx_Empresa.Enabled = True
        tbx_Empresa.ReadOnly = False
        btn_Agregar.Enabled = True
        rdb_NuevoIngreso.Enabled = False
        rdb_FaltaRetardo.Enabled = False
        rdb_Externo.Enabled = True
        rdb_Otro.Enabled = False

        rdb_NuevoIngreso.Checked = False
        rdb_FaltaRetardo.Checked = False
        rdb_Externo.Checked = False
        rdb_Otro.Checked = False

        tbx_Nombre.Focus()
    End Sub

    Protected Sub btn_Agregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Agregar.Click
        Dim PersonaAcreditada As String = ""

        If rdb_Empleado.Checked Then
            If ddl_Empleado.SelectedValue = "0" Then
                   fn_Alerta("Seleccione un Usuario para agregar a la lista.")
                ddl_Empleado.Focus()
                Exit Sub
            End If
            PersonaAcreditada = ddl_Empleado.SelectedValue & "," & ddl_Empleado.SelectedItem.Text

            For Each fila As GridViewRow In gv_Empleados.Rows
                If gv_Empleados.DataKeys(fila.RowIndex).Values("Id_Empleado") = ddl_Empleado.SelectedValue Then
                       fn_Alerta("Elemento seleccionado ya existe en la lista.")
                    Exit Sub
                End If
            Next
        Else
            If tbx_Nombre.Text = "" Then
                   fn_Alerta("Capture el Nombre.")
                tbx_Nombre.Focus()
                Exit Sub
            End If
            If tbx_Empresa.Text = "" Then
                     fn_Alerta("Capture el nombre de la Empresa.")
                tbx_Empresa.Focus()
                Exit Sub
            End If
            PersonaAcreditada = "0," & Trim(tbx_Nombre.Text).ToUpper
        End If

        If Session("UsuariosAgregadosCarta") = "" Then
              Session("UsuariosAgregadosCarta") = PersonaAcreditada
        Else
               Session("UsuariosAgregadosCarta") = Session("UsuariosAgregadosCarta") & ";" & PersonaAcreditada
        End If

        gv_Empleados.DataSource = fn_AgregarFila("Id_Empleado,Nombre", Session("UsuariosAgregadosCarta"))
        gv_Empleados.DataBind()

        If rdb_Empleado.Checked Then
            ddl_Empleado.SelectedValue = 0
            ddl_Empleado.Focus()
            rdb_Visitante.Enabled = False
        Else
            tbx_Nombre.Text = ""
            tbx_Nombre.Focus()
            tbx_Empresa.Enabled = False
            rdb_Empleado.Enabled = False
        End If
    End Sub

    Protected Sub gv_Empleados_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Empleados.RowDeleting
        If gv_Empleados.DataKeys(e.RowIndex).Value = "" Then Exit Sub

        If rdb_Empleado.Checked Then
            ActualizarUsuarios(gv_Empleados.DataKeys(e.RowIndex).Value)
        Else
            ActualizarVisitantes(e.RowIndex)
        End If

        If Session("UsuariosAgregadosCarta") = "" Then
            gv_Empleados.DataSource = fn_CreaGridVacio("Id_Empleado,Nombre")
            gv_Empleados.DataBind()
            rdb_Empleado.Enabled = True
            rdb_Visitante.Enabled = True
        Else
            gv_Empleados.DataSource = fn_AgregarFila("Id_Empleado,Nombre", Session("UsuariosAgregadosCarta"))
            gv_Empleados.DataBind()
        End If
    End Sub

    Sub ActualizarVisitantes(ByVal VEliminar As Integer)
        Dim VisitantesActualizados As String = ""
        Dim Visitantes() As String = Split(Session("UsuariosAgregadosCarta"), ";")

        For x As Integer = 0 To Visitantes.Length - 1
             If x <> VEliminar Then
                If VisitantesActualizados = "" Then
                    VisitantesActualizados = Visitantes(x)
                Else
                    VisitantesActualizados = VisitantesActualizados & ";" & Visitantes(x)
                End If
            End If
        Next
        Session("UsuariosAgregadosCarta") = VisitantesActualizados
    End Sub

    Sub ActualizarUsuarios(ByVal UEliminar As String)
        Dim UsuariosActualizados As String = ""
        Dim Usuarios() As String = Split(Session("UsuariosAgregadosCarta"), ";")

        For x As Integer = 0 To Usuarios.Length - 1
            Dim div() As String = Split(Usuarios(x), ",")
            If div(0) <> UEliminar Then
                If UsuariosActualizados = "" Then
                    UsuariosActualizados = Usuarios(x)
                Else
                    UsuariosActualizados = UsuariosActualizados & ";" & Usuarios(x)
                End If
            End If
        Next
        Session("UsuariosAgregadosCarta") = UsuariosActualizados
    End Sub

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Guardar.Click
        Dim Tipo As Integer = 0

        If Session("UsuariosAgregadosCarta") = "" Then
               fn_Alerta("Debe agregar al menos un Empleado/Visitante.")
            Exit Sub
        End If
        If tbx_FechaInicio.Text = "" Then
               fn_Alerta("Seleccione la Fecha de Inicio.")
            tbx_FechaInicio.Focus()
            Exit Sub
        ElseIf tbx_FechaInicio.Text < DateTime.Now.Date Then
              fn_Alerta("La Fecha de Inicio debe ser mayor o igual al día de hoy.")
            tbx_FechaInicio.Focus()
            Exit Sub
        End If
        If Not rdb_NuevoIngreso.Checked And Not rdb_FaltaRetardo.Checked And Not rdb_Externo.Checked And Not rdb_Otro.Checked Then
              fn_Alerta("Seleccione el Tipo de Carta.")
            Exit Sub
        End If
        If tbx_Asunto.Text = "" Then
                fn_Alerta("Capture el Asunto.")
            tbx_Asunto.Focus()
            Exit Sub
        End If
        If ddl_Autoriza.SelectedIndex = 0 Then
             fn_Alerta("Seleccione quién Autoriza la Carta.")
            ddl_Autoriza.Focus()
            Exit Sub
        End If
        If ddl_Dirigida.SelectedIndex = 0 Then
             fn_Alerta("Seleccione a quién va Dirigida la Carta.")
            ddl_Dirigida.Focus()
            Exit Sub
        End If

        If rdb_Empleado.Checked Then
            Tipo = 1
        Else
            Tipo = 2
        End If

        If Not fn_GenerarCartasAcceso_Nuevo(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), Session("UsuariosAgregadosCarta"), tbx_Asunto.Text.ToUpper, ddl_Autoriza.SelectedValue, tbx_FechaInicio.Text, tbx_FechaInicio.Text, Tipo, ddl_Dirigida.SelectedValue, tbx_Empresa.Text) Then
                  fn_Alerta("Ha ocurrido un error al intentar Generar la Carta de Acceso.")
        Else
            Call LimpiarPantalla()
            gv_Empleados.DataSource = fn_CreaGridVacio("Id_Empleado,Nombre")
            gv_Empleados.DataBind()
            Session("UsuariosAgregadosCarta") = ""
            fn_Alerta("La carta de Acceso se ha genreado correctamente.")
        End If

    End Sub

    Sub LimpiarPantalla()
        rdb_Empleado.Enabled = True
        rdb_Empleado.Checked = False
        rdb_Visitante.Enabled = True
        rdb_Visitante.Checked = False

        ddl_Empleado.Enabled = False
        ddl_Empleado.SelectedValue = 0
        tbx_Nombre.Enabled = False
        tbx_Nombre.Text = ""
        tbx_Empresa.Enabled = False
        tbx_Empresa.Text = ""
        btn_Agregar.Enabled = False
        tbx_FechaInicio.Text = ""

        rdb_NuevoIngreso.Enabled = True
        rdb_NuevoIngreso.Checked = False
        rdb_FaltaRetardo.Enabled = True
        rdb_FaltaRetardo.Checked = False
        rdb_Externo.Enabled = True
        rdb_Externo.Checked = False
        rdb_Otro.Enabled = True
        rdb_Otro.Checked = False

        tbx_Asunto.Text = ""
        ddl_Autoriza.SelectedValue = 0
        ddl_Dirigida.SelectedValue = 0
    End Sub

    Protected Sub rdb_NuevoIngreso_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdb_NuevoIngreso.CheckedChanged
        tbx_Asunto.Text = "POR MEDIO DEL PRESENTE SOLICITO QUE SE LE FACILITE EL ACCESO A LAS INSTALACIONES DE VALORES A LA(S) SIGUIENTE(S) PERSONA(S) DE NUEVO INGRESO. ASÍ MISMO AGRADECERÉ SE REALICEN LOS PROCEDIMIENTOS INDICADOS PARA EL INGRESO, E INFORMEN A LA BREVEDAD POSIBLE CUALQUIER ANOMALIA."
    End Sub

    Protected Sub rdb_FaltaRetardo_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdb_FaltaRetardo.CheckedChanged
        tbx_Asunto.Text = "POR MEDIO DEL PRESENTE SOLICITO QUE SE LE FACILITE EL ACCESO A LAS INSTALACIONES DE VALORES A LA(S) SIGUIENTE(S) PERSONA(S), YA QUE FALTO EL DIA -DD- DE -MMMMMMMM- DEL PRESENTE AÑO POR MOTIVOS PERSONALES."
    End Sub

    Protected Sub rdb_Externo_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdb_Externo.CheckedChanged
        tbx_Asunto.Text = "POR MEDIO DEL PRESENTE SOLICITO QUE SE LE FACILITE EL ACCESO A LAS INSTALACIONES DE VALORES A LA(S) SIGUIENTE(S) PERSONA(S). ASÍ MISMO AGRADECERÉ SE REALICEN LOS PROCEDIMIENTOS INDICADOS PARA EL INGRESO, E INFORMEN A LA BREVEDAD POSIBLE CUALQUIER ANOMALIA."
    End Sub

    Protected Sub rdb_Otro_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdb_Otro.CheckedChanged
        tbx_Asunto.Text = ""
    End Sub

End Class