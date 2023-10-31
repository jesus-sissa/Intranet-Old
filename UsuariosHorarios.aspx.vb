Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.Cn_Login
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data

Partial Public Class UsuariosHorarios
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub
      
        Dim dt_Usuarios As DataTable
        dt_Usuarios = fn_EnviarMensajes_LlenarListaU(Session("SucursalID"), Session("UsuarioID"))

        fn_LlenarDDL(ddl_Usuarios, dt_Usuarios, "Nombre", "Id_Empleado", "0")

        Call CrearTablaHorarios()
    End Sub

    Sub CrearTablaHorarios()
        Dim dias() As String = Split("Domingo,Lunes,Martes,Miercoles,Jueves,Viernes,Sábado", ",")
        Dim dt As New DataTable

        dt.Columns.Add("Id_Dia")
        dt.Columns.Add("Dia")

        Dim dr As DataRow = dt.NewRow

        For x As Integer = 0 To 6
            dt.Rows.Add(x, dias(x))
        Next

        gv_Horarios.DataSource = dt
        gv_Horarios.DataBind()

        Dim diasL() As String = Split("D,L,M,M,J,V,S", ",")
        Dim boton As Button
        For y As Integer = 0 To 6
            boton = CType(gv_Horarios.Rows(y).Cells(25).FindControl("btn_Dia"), Button)
            boton.Text = diasL(y)
        Next
    End Sub

    Sub DesmarcarTodo()
        Dim chk As CheckBox

        For x As Integer = 0 To 6
            For y As Integer = 1 To 24
                chk = CType(gv_Horarios.Rows(x).Cells(y).FindControl("chk_" & (y - 1).ToString), CheckBox)
                chk.Checked = False
            Next
        Next
    End Sub

    Protected Sub gv_Horarios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Horarios.RowCommand
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim chk As CheckBox
        Dim Cant_Checados As Integer = 0
        Dim ChecarTodos As Boolean = False

        If index >= 0 Then
            For x As Integer = 0 To 23
                chk = CType(gv_Horarios.Rows(index).Cells(x).FindControl("chk_" & x.ToString), CheckBox)
                If chk.Checked Then
                    Cant_Checados += 1
                End If
            Next

            If Cant_Checados = 24 Then
                ChecarTodos = False
            Else
                ChecarTodos = True
            End If

            For x As Integer = 0 To 23
                chk = CType(gv_Horarios.Rows(index).Cells(x).FindControl("chk_" & x.ToString), CheckBox)
                chk.Checked = ChecarTodos
            Next
        Else
            Dim indexCol As Integer = (Convert.ToInt16(e.CommandName) + 1)  'Este es el índice de la columna
            Dim indexChk As Integer = Convert.ToInt16(e.CommandName)        'Este es el número de checkbox

            For x As Integer = 0 To 6
                chk = CType(gv_Horarios.Rows(x).Cells(indexCol).FindControl("chk_" & indexChk.ToString), CheckBox)
                If chk.Checked Then
                    Cant_Checados += 1
                End If
            Next

            If Cant_Checados = 7 Then
                ChecarTodos = False
            Else
                ChecarTodos = True
            End If

            For x As Integer = 0 To 6
                chk = CType(gv_Horarios.Rows(x).Cells(indexCol).FindControl("chk_" & indexChk.ToString), CheckBox)
                chk.Checked = ChecarTodos
            Next
        End If

    End Sub

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Guardar.Click
        If ddl_Usuarios.SelectedValue = 0 Then
            fn_Alerta("Seleccione el Usuario.")
            Exit Sub
        End If

        'Guardar los Horarios de Acceso
        Dim cant As Integer = 0
        Dim Horarios() As String = ValidarHorarios(cant)

        If Horarios(0) Is Nothing Then
            fn_Alerta("Marque los horarios para el usuario seleccionado")
            Exit Sub
        End If

        'Borrarle Todos los Horarios
        Cn_Login.fn_Log_Create(Session("UsuarioID"), Session("ModuloClave"), "GUARDAR HORARIOS DE ACCESO PARA: " & ddl_Usuarios.SelectedItem.Text, Session("EstacioN"), Session("EstacionIP"), Session("EstacionMac"), Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

        If Not fn_UsuariosHoras_Agregar(Session("SucursalID"), Session("UsuarioID"), ddl_Usuarios.SelectedValue, Horarios) Then
            fn_Alerta("Ha ocurrido un error al intentar Guardar los Horarios.")
            Exit Sub
        Else
            fn_Alerta("Los Horarios se han guardado correctamente.")
        End If
    End Sub

    Function ValidarHorarios(ByRef cant As Integer) As String()
        Dim chk As CheckBox
        Dim arrHrs(0) As String

        For x As Integer = 0 To 6
            Dim Dia As Integer = x + 1
            For y As Integer = 1 To 24
                Dim Hora As Integer = y - 1
                chk = CType(gv_Horarios.Rows(x).Cells(y).FindControl("chk_" & (y - 1).ToString), CheckBox)
                If chk.Checked Then
                    ReDim Preserve arrHrs(cant)
                    arrHrs(cant) = Dia.ToString & "," & Hora.ToString
                    cant += 1
                End If
            Next
        Next
        Return arrHrs
    End Function

    Protected Sub ddl_Usuarios_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Usuarios.SelectedIndexChanged
        DesmarcarTodo()
        If ddl_Usuarios.SelectedValue = 0 Then Exit Sub

        Dim dt As DataTable = Cn_Soporte.fn_UsuariosHoras_Consultar(Session("SucursalID"), Session("UsuarioID"), ddl_Usuarios.SelectedValue)

        If dt IsNot Nothing Then
            Dim chk As CheckBox
            For Each row As DataRow In dt.Rows
                Dim x As Integer = row("Dia")
                Dim y As Integer = row("Hora")
                chk = CType(gv_Horarios.Rows(x - 1).Cells(y).FindControl("chk_" & (y).ToString), CheckBox)
                chk.Checked = True
            Next
        Else
            Exit Sub
        End If
    End Sub

End Class