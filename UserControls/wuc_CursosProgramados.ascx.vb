
Imports SISSAIntranet.Cn_Soporte
Imports SISSAIntranet.Cn_Login
Imports SISSAIntranet.FuncionesGlobales
Imports System.Data

Partial Class wuc_CursosProgramados
    Inherits System.Web.UI.UserControl

    Dim DeptoN As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Page.Title = "CURSOS PROGRAMADOS"

        '-----------------------------------------
        'Aquí se valida que el Usuario firmado tenga realmente Privilegios para acceder a esta página
        'o se esta introduciendo la página directo en la barra de direcciones

        Dim arr As String() = Split(Session("CadenaPriv"), ";")

        For x As Integer = 0 To arr.Length - 1
            If arr(x) = "CURSOS PROGRAMADOS" Then
                Exit For
            ElseIf x = (arr.Length - 1) Then
                MostrarAlertAjax("NO TIENE PRIVILEGIOS PARA ACCEDER A ESTA OPCION", Me, Page)
                Response.Redirect("frm_login.aspx")
                Exit Sub
            End If
        Next

        '-------------------------------------------

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

        gv_EmpleadosAgregados.EmptyDataText = "<div class='myGrid' style='width:100%'><table cellpadding='0' cellspacing='0' summary='' width='100%' ><thead><tr><th scope='col'></th><th scope='col'>Clave</th><th scope='col'>Nombre</th><th scope='col'>Puesto</th><th scope='col'>Fecha Registro</th></tr></thead><tbody><tr><td width='50'></td><td colspan='5'><strong>No se ha seleccionado un Curso.</strong></td></tr></tbody></table></div>"
        gv_EmpleadosAgregados.DataSource = fn_CursosProgramados_ObtenerEmpleadosProgramados(0, Session("SucursalID"), Session("UsuarioID"))
        gv_EmpleadosAgregados.DataBind()

        gv_Temas.EmptyDataText = "<div class='myGrid' style='width:100%'><table cellpadding='0' cellspacing='0' summary='' width='100%' ><thead><tr><th scope='col'>Clave</th><th scope='col'>Tema</th><th scope='col'>Duración</th></tr></thead><tbody><tr><td width='50'></td><td colspan='5'><strong>No se ha seleccionado un Curso.</strong></td></tr></tbody></table></div>"
        gv_Temas.DataSource = fn_CursosProgramados_ObtenerTemas(0, Session("SucursalID"), Session("UsuarioID"))
        gv_Temas.DataBind()

    End Sub

    Protected Sub btn_Agregar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Agregar.Click

        If gv_Cursos.SelectedIndex = -1 Then
            MostrarAlertAjax("Seleccione un Curso.", btn_Agregar, Page)
            Exit Sub
        End If

        If ddl_Empleados.SelectedValue = "" Then
            MostrarAlertAjax("Seleccione un Empleado.", btn_Agregar, Page)
            Exit Sub
        End If

        For Each fila As GridViewRow In gv_EmpleadosAgregados.Rows
            If gv_EmpleadosAgregados.DataKeys(fila.RowIndex).Values("Id_Empleado") = ddl_Empleados.SelectedValue Then
                MostrarAlertAjax("Empleado seleccionado ya existe en la lista.", btn_Agregar, Page)
                Exit Sub
            End If
        Next

        If fn_CursosProgramados_Guardar(gv_Cursos.SelectedDataKey.Values("Id_Programacion"), ddl_Empleados.SelectedValue, Session("UsuarioID"), Session("EstacioN"), Session("SucursalID"), Session("UsuarioID")) Then
            'MostrarAlertAjax("Los datos han sido guardados correctamente.", btn_Agregar, Page)
        Else
            MostrarAlertAjax("Ha ocurrido un error al intentar guardar los datos.", btn_Agregar, Page)
            Exit Sub
        End If

        'Enviar Alerta
        Dim Detalles As String = "Curso : " & gv_Cursos.SelectedRow.Cells(2).Text _
                                & "; Empleado : " & ddl_Empleados.SelectedItem.Text _
                                & "; Usuario Registró : " & Session("UsuarioNombre")

        fn_AlertasGeneradas_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), "10", Detalles)

        Dim Clave As String = Left(ddl_Empleados.SelectedItem.Text, 4)
        Dim Nombre As String = Right(ddl_Empleados.SelectedItem.Text, (Len(ddl_Empleados.SelectedItem.Text) - 5))

        Dim DetalleMail As String = fn_DetalleMail(Session("DeptoNombre"), gv_Cursos.SelectedRow.Cells(2).Text, gv_Cursos.SelectedRow.Cells(3).Text, Clave, Nombre, Session("UsuarioNombre"), gv_Cursos.SelectedRow.Cells(4).Text, gv_Cursos.SelectedRow.Cells(5).Text)

        If Not fn_EnviarCorreos("10", DetalleMail, Session("SucursalID"), "ALTA DE EMPLEADO EN CURSO") Then
            MostrarAlertAjax("Ha ocurrido un error al intentar enviar los Correos.", btn_Agregar, Page)
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

        Dim resultado As Integer
        Integer.TryParse(gv_Cursos.SelectedDataKey.Values("Id_Programacion"), resultado)

        If resultado = 0 Then
            gv_Cursos.SelectedRowStyle.ForeColor = Drawing.Color.Black
            gv_Cursos.SelectedRowStyle.Font.Bold = False
            gv_Cursos.SelectedRowStyle.BackColor = System.Drawing.Color.White
            gv_Cursos.SelectedIndex = -1
            Exit Sub
        End If

        gv_EmpleadosAgregados.EmptyDataText = "<div class='myGrid' style='width:100%'><table cellpadding='0' cellspacing='0' summary='' width='100%' ><thead><tr><th scope='col'></th><th scope='col'>Clave</th><th scope='col'>Nombre</th><th scope='col'>Puesto</th><th scope='col'>Fecha Registro</th></tr></thead><tbody><tr><td width='50'></td><td id='gv_Text' colspan='5'><strong>No se han agregado Empleados a este Curso.</strong></td></tr></tbody></table></div>"
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

        gv_Temas.EmptyDataText = "<div class='myGrid' style='width:100%'><table cellpadding='0' cellspacing='0' summary='' width='100%' ><thead><tr><th scope='col'>Clave</th><th scope='col'>Tema</th><th scope='col'>Duración</th></tr></thead><tbody><tr><td width='50'></td><td colspan='5'><strong>No se han agregado Temas a este Curso.</strong></td></tr></tbody></table></div>"
        gv_Temas.DataSource = fn_CursosProgramados_ObtenerTemas(gv_Cursos.SelectedDataKey.Values("Id_Programacion"), Session("SucursalID"), Session("UsuarioID"))
        gv_Temas.DataBind()

    End Sub

    Protected Sub gv_EmpleadosAgregados_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_EmpleadosAgregados.RowDeleting

        If Not fn_CursosProgramados_Borrar(gv_Cursos.SelectedDataKey.Values("Id_Programacion"), gv_EmpleadosAgregados.DataKeys(e.RowIndex).Values("Id_Empleado"), Session("SucursalID"), Session("UsuarioID")) Then
            MostrarAlertAjax("Ha ocurrido un error al intentar eliminar el Empleado.", gv_EmpleadosAgregados, Page)
            Exit Sub
        Else
            'MostrarAlertAjax("Se ha eliminado correctamente el Empleado.", gv_EmpleadosAgregados, Page)
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

    Protected Sub gv_Cursos_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Cursos.RowCreated
        ' En este Sub se agregan a las filas de dgv_Empleados los atributos "onmouseover" y "onmouseout"
        ' para que cuando el puntero del mouse este sobre una fila, se apliquen las propiedades declaradas (backgoundColor)

        ' only apply changes if its DataRow
        If e.Row.RowType = DataControlRowType.DataRow Then

            ' when mouse is over the row, save original color to new attribute, and change it to highlight yellow color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#C0A062'")  '#D0A540'")

            ' when mouse leaves the row, change the bg color to its original value    
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")
        End If
    End Sub

    Protected Sub gv_Temas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Temas.PageIndexChanging
        gv_Temas.PageIndex = e.NewPageIndex
        gv_Temas.DataSource = fn_CursosProgramados_ObtenerTemas(gv_Cursos.SelectedDataKey.Values("Id_Programacion"), Session("SucursalID"), Session("UsuarioID"))
        gv_Temas.DataBind()
    End Sub

    Protected Sub gv_Cursos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Cursos.PageIndexChanging

        tbx_Instructor.Text = ""
        tbx_Sitio.Text = ""
        tbx_Duracion.Text = ""

        gv_Temas.EmptyDataText = "<div class='myGrid' style='width:100%'><table cellpadding='0' cellspacing='0' summary='' width='100%' ><thead><tr><th scope='col'>Clave</th><th scope='col'>Tema</th><th scope='col'>Duración</th></tr></thead><tbody><tr><td width='50'></td><td colspan='5'><strong>No se ha seleccionado un Curso.</strong></td></tr></tbody></table></div>"
        gv_Temas.DataSource = Nothing
        gv_Temas.DataBind()
        gv_Cursos.SelectedRowStyle.BackColor = Drawing.Color.White
        gv_Cursos.SelectedRowStyle.ForeColor = Drawing.Color.Black
        gv_Cursos.SelectedRowStyle.Font.Bold = False

        gv_Cursos.PageIndex = e.NewPageIndex
        gv_Cursos.DataSource = fn_CursosProgramados_ObtenerCursos(Session("SucursalID"), Session("DepartamentoID"), Session("UsuarioID"))
        gv_Cursos.DataBind()
    End Sub

    Public Shared Function fn_DetalleMail(ByVal Departamento As String, ByVal ClaveCurso As String, ByVal Curso As String, ByVal ClaveEmpleado As String, ByVal Nombre As String, ByVal UsuarioRegistro As String, ByVal Fecha As String, ByVal Hora As String) As String

        Dim Detalle As String = "               EMPLEADO AGREGADO A CURSO " & Chr(13) & Chr(13) _
                              & "                Departamento: " & Departamento & Chr(13) _
                              & "                 Clave Curso: " & ClaveCurso & Chr(13) _
                              & "                       Curso: " & Curso & Chr(13) _
                              & "                 Fecha Curso: " & Fecha & Chr(13) _
                              & "                  Hora Curso: " & Hora & Chr(13) _
                              & "              Clave Empleado: " & ClaveEmpleado & Chr(13) _
                              & "             Nombre Empleado: " & Nombre & Chr(13) _
                              & "                    Registró: " & UsuarioRegistro & Chr(13) _
                              & "              Fecha Registro: " & Now.Date & Chr(13) _
                              & "               Hora Registro: " & Now.ToShortTimeString & Chr(13) & Chr(13) _
                              & "Agente de Servicios SIAC."
        Return Detalle
    End Function

End Class
