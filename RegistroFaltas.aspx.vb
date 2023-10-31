Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.Cn_Login
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data
Imports System.Web.UI.Page

Partial Public Class RegistroFaltas
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub
       
        Dim dt_Empleados As DataTable = fn_Faltas_ObtenerEmpleados(Session("SucursalID"), Session("DepartamentoID"), Session("UsuarioID"))
        fn_LlenarDDL_VariosCampos(ddl_Empleado, dt_Empleados, "CveNombre", "Id_Empleado")
        Call MuestraJornadasVacio()

        For i As Byte = 0 To 4
            gv_Jornadas.Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
        Next

    End Sub

    Sub MuestraJornadasVacio()
        gv_Jornadas.DataSource = fn_CreaGridVacio("Id_Jornada,Fecha,Dia,Jornada1,Jornada2,Turno,TipoFalta")
        gv_Jornadas.DataBind()
    End Sub

    Protected Sub tbx_Clave_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbx_Clave.TextChanged
        MuestraJornadasVacio()
        ddl_Empleado.SelectedValue = 0
        If tbx_Clave.Text.Length < 4 Then Exit Sub
        For elemento As Integer = 0 To ddl_Empleado.Items.Count - 1
            If Microsoft.VisualBasic.Left(ddl_Empleado.Items(elemento).Text, 4) = tbx_Clave.Text Then
                ddl_Empleado.SelectedIndex = elemento
            End If
        Next
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Mostrar.Click
        If ddl_Empleado.SelectedIndex = 0 Then
              fn_Alerta("Seleccione el Empleado.")
            ddl_Empleado.Focus()
            Exit Sub
        End If

        gv_Jornadas.PageIndex = 0
        Call LlenarJornadas()
    End Sub

    Sub LlenarJornadas()
        Dim chk As CheckBox
        Dim dt As DataTable = fn_Jornadas_ConsultaJornadas(Session("SucursalID"), Session("UsuarioID"), ddl_Empleado.SelectedValue)

        If dt IsNot Nothing Then
            If dt.Rows.Count > 0 Then
                gv_Jornadas.DataSource = dt
                gv_Jornadas.DataBind()

                For i As Integer = 0 To (gv_Jornadas.Rows.Count - 1)
                    If gv_Jornadas.DataKeys(i).Values("TipoFalta") > 0 Then
                        Select Case gv_Jornadas.DataKeys(i).Values("TipoFalta")
                            Case 1
                                chk = gv_Jornadas.Rows(i).FindControl("chk_Falta")
                                chk.Checked = True
                                gv_Jornadas.Rows(i).ForeColor = Drawing.Color.Red
                                gv_Jornadas.Rows(i).Font.Bold = True
                            Case 2
                                chk = gv_Jornadas.Rows(i).FindControl("chk_Descanso")
                                chk.Checked = True
                                gv_Jornadas.Rows(i).ForeColor = Drawing.Color.DeepSkyBlue
                                gv_Jornadas.Rows(i).Font.Bold = True
                            Case 3
                                chk = gv_Jornadas.Rows(i).FindControl("chk_Vacaciones")
                                chk.Checked = True
                                gv_Jornadas.Rows(i).ForeColor = Drawing.Color.LightSeaGreen
                                gv_Jornadas.Rows(i).Font.Bold = True
                            Case 4
                                chk = gv_Jornadas.Rows(i).FindControl("chk_Incapacidad")
                                chk.Checked = True
                                gv_Jornadas.Rows(i).ForeColor = Drawing.Color.SaddleBrown
                                gv_Jornadas.Rows(i).Font.Bold = True
                        End Select
                    End If
                Next

            Else
                Call MuestraJornadasVacio()
            End If
        End If
    End Sub

    Protected Sub ValidaChecks(ByVal sender As Object, ByVal e As EventArgs)
        Dim checkbox As CheckBox = DirectCast(sender, CheckBox)
        Dim row As GridViewRow = DirectCast(checkbox.NamingContainer, GridViewRow)
        If Not checkbox.Checked Then
            row.ForeColor = Drawing.Color.Black
            row.Font.Bold = False
            Exit Sub
        End If
        Select Case checkbox.ID
            Case "chk_Falta"
                Dim chD As CheckBox = row.FindControl("chk_Descanso")
                chD.Checked = False
                Dim chV As CheckBox = row.FindControl("chk_Vacaciones")
                chV.Checked = False
                Dim chI As CheckBox = row.FindControl("chk_Incapacidad")
                chI.Checked = False
                row.ForeColor = Drawing.Color.Red
                row.Font.Bold = True
            Case "chk_Descanso"
                Dim chF As CheckBox = row.FindControl("chk_Falta")
                chF.Checked = False
                Dim chV As CheckBox = row.FindControl("chk_Vacaciones")
                chV.Checked = False
                Dim chI As CheckBox = row.FindControl("chk_Incapacidad")
                chI.Checked = False
                row.ForeColor = Drawing.Color.DeepSkyBlue
                row.Font.Bold = True
            Case "chk_Vacaciones"
                Dim chF As CheckBox = row.FindControl("chk_Falta")
                chF.Checked = False
                Dim chD As CheckBox = row.FindControl("chk_Descanso")
                chD.Checked = False
                Dim chI As CheckBox = row.FindControl("chk_Incapacidad")
                chI.Checked = False
                row.ForeColor = Drawing.Color.LightSeaGreen
                row.Font.Bold = True
            Case "chk_Incapacidad"
                Dim chF As CheckBox = row.FindControl("chk_Falta")
                chF.Checked = False
                Dim chD As CheckBox = row.FindControl("chk_Descanso")
                chD.Checked = False
                Dim chV As CheckBox = row.FindControl("chk_Vacaciones")
                chV.Checked = False
                row.ForeColor = Drawing.Color.SaddleBrown
                row.Font.Bold = True
        End Select
    End Sub

    Protected Sub ddl_Empleado_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Empleado.SelectedIndexChanged
        Call MuestraJornadasVacio()
        If ddl_Empleado.SelectedValue = 0 Then
            tbx_Clave.Text = ""
            Exit Sub
        End If
        tbx_Clave.Text = Microsoft.VisualBasic.Left(ddl_Empleado.SelectedItem.Text, 4)
    End Sub

    Protected Sub gv_Jornadas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Jornadas.PageIndexChanging
        gv_Jornadas.PageIndex = e.NewPageIndex
        LlenarJornadas()
    End Sub

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Guardar.Click
        Dim cant As Integer = 0
        If gv_Jornadas.DataKeys(0).Values("Id_Jornada").ToString = "" Then
              fn_Alerta("No hay Jornadas en la lista.")
            Exit Sub
        End If

        Dim Faltas() As String = ValidarFaltas(cant)

        If cant = 0 Then
              fn_Alerta("No se ha marcado ningún registro.")
            Exit Sub
        End If
        If Not fn_Faltas_Guardar(Session("SucursalID"), Session("UsuarioID"), "", Faltas) Then
              fn_Alerta("Ha ocurrido un error al intentar eliminar el Día.")
        End If
    End Sub

    Function ValidarFaltas(ByRef cant As Integer) As String()
        Dim row As GridViewRow
        Dim ischecked As Boolean = False
        Dim arrFaltas(0) As String
        For i As Integer = 0 To gv_Jornadas.Rows.Count - 1
            row = gv_Jornadas.Rows(i)

            If DirectCast(row.FindControl("chk_Falta"), CheckBox).Checked Then
                ReDim Preserve arrFaltas(cant)
                arrFaltas(cant) = gv_Jornadas.DataKeys(i).Values("Id_Jornada") & "," & "1"
                cant += 1
            ElseIf DirectCast(row.FindControl("chk_Descanso"), CheckBox).Checked Then
                ReDim Preserve arrFaltas(cant)
                arrFaltas(cant) = gv_Jornadas.DataKeys(i).Values("Id_Jornada") & "," & "2"
                cant += 1
            ElseIf DirectCast(row.FindControl("chk_Vacaciones"), CheckBox).Checked Then
                ReDim Preserve arrFaltas(cant)
                arrFaltas(cant) = gv_Jornadas.DataKeys(i).Values("Id_Jornada") & "," & "3"
                cant += 1
            ElseIf DirectCast(row.FindControl("chk_Incapacidad"), CheckBox).Checked Then
                ReDim Preserve arrFaltas(cant)
                arrFaltas(cant) = gv_Jornadas.DataKeys(i).Values("Id_Jornada") & "," & "4"
                cant += 1
            End If
        Next
        Return arrFaltas
    End Function

End Class
