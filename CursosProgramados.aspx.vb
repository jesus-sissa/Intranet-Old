Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.Cn_Login
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data

Partial Class CursosProgramados
    Inherits BasePage
    Dim DeptoN As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Dim dt_Cursos As DataTable = fn_CursosProgramados_ObtenerCursos(Session("SucursalID"), Session("DepartamentoID"), Session("UsuarioID"))
        If dt_Cursos IsNot Nothing Then
            gv_Cursos.DataSource = dt_Cursos
            gv_Cursos.DataBind()
        Else
            gv_Cursos.DataSource = fn_CreaGridVacio("Id_Programacion,Clave,Curso,Fecha,HoraInicio,Id_Curso,Instructor,Sitio,Duracion,AsistMin,AsistMax,Status,HoraFin")
            gv_Cursos.DataBind()
        End If

        Dim dt_Puestos As DataTable
        If Session("Dpto_Reclutamiento") = Session("DepartamentoId") Then
            dt_Puestos = fn_CursosProgramados_ObtenerEmpleados(0, Session("SucursalID"), Session("UsuarioID"))
            fn_LlenarDDL(ddl_Empleados, dt_Puestos, "CveNombre", "Id_Empleado", "")
        Else
            dt_Puestos = fn_CursosProgramados_ObtenerEmpleados(Session("DepartamentoID"), Session("SucursalID"), Session("UsuarioID"))
            fn_LlenarDDL(ddl_Empleados, dt_Puestos, "CveNombre", "Id_Empleado", "")
        End If

        fn_Log_Create(Session("UsuarioID"), Session("ModuloClave"), "ABRIR PAGINA: CURSOS PROGRAMADOS", Session("EstacioN"), Session("EstacionIP"), "", Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

        gv_EmpleadosAgregados.DataSource = fn_CreaGridVacio("Id_Empleado,Clave,Nombre,Puesto,FechaReg")
        gv_EmpleadosAgregados.DataBind()

        gv_Temas.DataSource = fn_CreaGridVacio("Clave,Descripcion,Horas")
        gv_Temas.DataBind()

        Dim Columnas As Byte
        Dim Gv_Nombres() As GridView = {gv_Cursos, gv_EmpleadosAgregados, gv_Temas}

        For j As Byte = 0 To Gv_Nombres.Length - 1
            Columnas = Gv_Nombres(j).Columns.Count

            For i As Byte = 0 To Columnas - 1
                Gv_Nombres(j).Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
            Next
        Next
    End Sub

    Protected Sub btn_Agregar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Agregar.Click
        If gv_Cursos.SelectedIndex = -1 Then
            fn_Alerta("Seleccione un Curso.")
            Exit Sub
        End If

        If ddl_Empleados.SelectedValue = "" Then
            fn_Alerta("Seleccione un Empleado.")
            Exit Sub
        End If

        For Each fila As GridViewRow In gv_EmpleadosAgregados.Rows
            If gv_EmpleadosAgregados.DataKeys(fila.RowIndex).Values("Id_Empleado") = ddl_Empleados.SelectedValue Then
                fn_Alerta("Empleado seleccionado ya existe en la lista.")
                Exit Sub
            End If
        Next

        If fn_CursosProgramados_Guardar(gv_Cursos.SelectedDataKey.Values("Id_Programacion"), ddl_Empleados.SelectedValue, Session("UsuarioID"), Session("EstacioN"), Session("SucursalID"), Session("UsuarioID")) Then
            fn_Alerta("Los datos han sido guardados correctamente.")
              Else
            fn_Alerta("Ha ocurrido un error al intentar guardar los datos.")
            Exit Sub
        End If

        'Enviar Alerta
        Dim Detalles As String = "Curso : " & gv_Cursos.SelectedRow.Cells(2).Text _
                                & "; Empleado : " & ddl_Empleados.SelectedItem.Text _
                                & "; Usuario Registró : " & Session("UsuarioNombre") _
                                & "; Sucursal : " & Session("SucursalN")

        fn_AlertasGeneradas_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), "10", Detalles)

        Dim Clave As String = Left(ddl_Empleados.SelectedItem.Text, 4)
        Dim Nombre As String = Right(ddl_Empleados.SelectedItem.Text, (Len(ddl_Empleados.SelectedItem.Text) - 5))

        Dim DetalleHTML As String = fn_DetalleHTML(Session("DeptoNombre"), gv_Cursos.SelectedRow.Cells(2).Text, gv_Cursos.SelectedRow.Cells(3).Text, Clave, Nombre, Session("UsuarioNombre"), gv_Cursos.SelectedRow.Cells(4).Text, gv_Cursos.SelectedRow.Cells(5).Text, Session("SucursalN"))

        If Not fn_EnviarCorreos("10", DetalleHTML, Session("SucursalID"), "ALTA DE EMPLEADO EN CURSO") Then
            fn_Alerta("Ha ocurrido un error al intentar enviar los Correos.")
            Exit Sub
        End If

        ddl_Empleados.SelectedIndex = 0
        gv_EmpleadosAgregados.DataSource = fn_CursosProgramados_ObtenerEmpleadosProgramados(gv_Cursos.SelectedDataKey.Values("Id_Programacion"), Session("SucursalID"), Session("UsuarioID"))
        gv_EmpleadosAgregados.DataBind()

        If gv_EmpleadosAgregados.Rows.Count = gv_Cursos.SelectedDataKey.Values("AsistMax") Then
            ddl_Empleados.Enabled = False
            btn_Agregar.Enabled = False
        Else
            ddl_Empleados.Enabled = True
            btn_Agregar.Enabled = True
        End If
    End Sub

    Protected Sub gv_Cursos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Cursos.SelectedIndexChanged
        If gv_Cursos.Rows(0).Cells(2).Text = "&nbsp;" Then Exit Sub

        Dim resultado As Integer
        Integer.TryParse(gv_Cursos.SelectedDataKey.Values("Id_Programacion"), resultado)

        If resultado = 0 Then
            gv_Cursos.SelectedIndex = -1
            Exit Sub
        End If

        Dim dt_EmpleadosProgramados As DataTable = fn_CursosProgramados_ObtenerEmpleadosProgramados(gv_Cursos.SelectedDataKey.Values("Id_Programacion"), Session("SucursalID"), Session("UsuarioID"))
        gv_EmpleadosAgregados.DataSource = dt_EmpleadosProgramados
        gv_EmpleadosAgregados.DataBind()

        If (gv_EmpleadosAgregados.Rows.Count = gv_Cursos.SelectedDataKey.Values("AsistMax")) Or gv_Cursos.SelectedRow.Cells(12).Text = "CERRADO" Then
            ddl_Empleados.Enabled = False
            btn_Agregar.Enabled = False
        Else
            ddl_Empleados.Enabled = True
            btn_Agregar.Enabled = True
        End If

        tbx_Instructor.Text = gv_Cursos.SelectedDataKey.Values("Instructor")
        tbx_Sitio.Text = gv_Cursos.SelectedDataKey.Values("Sitio")
        tbx_Duracion.Text = gv_Cursos.SelectedDataKey.Values("Duracion")

        tbx_HoraInicio.Text = gv_Cursos.SelectedDataKey.Values("HoraInicio")
        tbx_HoraFin.Text = gv_Cursos.SelectedDataKey.Values("HoraFin")

        gv_Temas.DataSource = fn_CursosProgramados_ObtenerTemas(gv_Cursos.SelectedDataKey.Values("Id_Programacion"), Session("SucursalID"), Session("UsuarioID"))
        gv_Temas.DataBind()

    End Sub

    Protected Sub gv_EmpleadosAgregados_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_EmpleadosAgregados.RowDeleting

        If Not fn_CursosProgramados_Borrar(gv_Cursos.SelectedDataKey.Values("Id_Programacion"), gv_EmpleadosAgregados.DataKeys(e.RowIndex).Values("Id_Empleado"), Session("SucursalID"), Session("UsuarioID")) Then
            fn_Alerta("Ha ocurrido un error al intentar eliminar el Empleado.")
            Exit Sub
        Else
            fn_Alerta("Se ha eliminado correctamente el Empleado.")
        End If

        gv_EmpleadosAgregados.DataSource = fn_CursosProgramados_ObtenerEmpleadosProgramados(gv_Cursos.SelectedDataKey.Values("Id_Programacion"), Session("SucursalID"), Session("UsuarioID"))
        gv_EmpleadosAgregados.DataBind()

        If gv_EmpleadosAgregados.Rows.Count = gv_Cursos.SelectedDataKey.Values("AsistMax") Then
            ddl_Empleados.Enabled = False
            btn_Agregar.Enabled = False
        Else
            ddl_Empleados.Enabled = True
            btn_Agregar.Enabled = True
        End If

    End Sub

    Protected Sub gv_Temas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Temas.PageIndexChanging
        gv_Temas.PageIndex = e.NewPageIndex
        gv_Temas.DataSource = fn_CursosProgramados_ObtenerTemas(gv_Cursos.SelectedDataKey.Values("Id_Programacion"), Session("SucursalID"), Session("UsuarioID"))
        gv_Temas.DataBind()
    End Sub

    Protected Sub gv_Cursos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Cursos.PageIndexChanging

        tbx_Instructor.Text = String.Empty
        tbx_Sitio.Text = String.Empty
        tbx_Duracion.Text = String.Empty

          gv_Temas.DataSource = Nothing
        gv_Temas.DataSource = fn_CreaGridVacio("Clave,Descripcion,Horas")
        gv_Temas.DataBind()
       
        gv_Cursos.PageIndex = e.NewPageIndex
        gv_Cursos.DataSource = fn_CursosProgramados_ObtenerCursos(Session("SucursalID"), Session("DepartamentoID"), Session("UsuarioID"))
        gv_Cursos.DataBind()
    End Sub

    Public Shared Function fn_DetalleHTML(ByVal Departamento As String, ByVal ClaveCurso As String, ByVal Curso As String, ByVal ClaveEmpleado As String, ByVal Nombre As String, ByVal UsuarioRegistro As String, ByVal Fecha As String, ByVal Hora As String, ByVal SucursalN As String) As String

        Dim Pie As String = "Agente de Servicios SIAC " & Now.Year.ToString
        Dim DetalleHTML As String = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                                & "<tr><td colspan='4' align='center'> EMPLEADO AGREGADO A CURSO </td></tr>" _
                                & "<tr><td colspan='4'><hr /></td></tr>" _
                                & "<tr><td align='right'><label><b>Departamento:</b></label></td><td> " & Departamento & " </td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Clave Curso:</b></label></td><td>" & ClaveCurso & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Curso:</b></label></td><td>" & Curso & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Fecha Curso:</b></label></td><td>" & Fecha & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Hora Curso:</b></label></td><td>" & Hora & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Clave Empleado:</b></label></td><td>" & ClaveEmpleado & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Nombre Empleado:</b></label></td><td>" & Nombre & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Sucursal Registró:</b></label></td><td>" & SucursalN & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Registró:</b></label></td><td>" & UsuarioRegistro & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Fecha Registro:</b></label></td><td>" & Now.Date & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Hora Registro:</b></label></td><td>" & Now.ToShortTimeString & "</td><td></td><td></td></tr>" _
                                & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>" & Pie & "</td></tr></table><br/><br/></body></html>"
        Return DetalleHTML
    End Function

End Class
