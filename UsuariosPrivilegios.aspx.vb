Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.Cn_Login
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data

Partial Public Class UsuariosPrivilegios
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub
       
        Dim dt_Usuarios As DataTable
        Dim dt_Modulos As DataTable

        dt_Usuarios = fn_EnviarMensajes_LlenarListaU(Session("SucursalID"), Session("UsuarioID"))
        dt_Modulos = fn_UsuariosPrivilegios_LlenarListaModulos(Session("SucursalID"), Session("UsuarioID"))

        fn_LlenarDDL(ddl_Usuarios, dt_Usuarios, "Nombre", "Id_Empleado", "0")
        fn_LlenarDDL(ddl_Modulos, dt_Modulos, "Modulo", "Clave_Modulo", "0")

        Call Mostrar_GridsVacios()

        Dim Columnas As Byte
        Dim Gv_Nombres() As GridView = {gv_Controles, gv_Menus, gv_Opciones}
        For j As Byte = 0 To Gv_Nombres.Length - 1
            Columnas = Gv_Nombres(j).Columns.Count

            For i As Byte = 0 To Columnas - 1
                Gv_Nombres(j).Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
            Next
        Next
    End Sub

    Sub Mostrar_GridsVacios()
        Call MostrarMenusVacio()
        Call MostrarOpcionesVacio()
        Call MostrarControlesVacio()
    End Sub

    Sub MostrarMenusVacio()
        gv_Menus.DataSource = fn_CreaGridVacio("Id_Menu,Descripcion,Status,Orden")
        gv_Menus.DataBind()
        gv_Menus.SelectedIndex = -1
    End Sub

    Sub MostrarOpcionesVacio()
        gv_Opciones.DataSource = fn_CreaGridVacio("Id_Opcion,Descripcion,Status")
        gv_Opciones.DataBind()
        gv_Opciones.SelectedIndex = -1
    End Sub

    Sub MostrarControlesVacio()
        gv_Controles.DataSource = fn_CreaGridVacio("Id_Control,Descripcion,Status")
        gv_Controles.DataBind()
        gv_Controles.SelectedIndex = -1
    End Sub

    Sub LlenarMenus()
        Dim dt_Menus As DataTable = fn_UsuariosPrivilegios_ObtenerMenus(Session("SucursalID"), Session("UsuarioID"), ddl_Modulos.SelectedValue)

        If dt_Menus IsNot Nothing AndAlso dt_Menus.Rows.Count > 0 Then
            gv_Menus.DataSource = dt_Menus
            gv_Menus.DataBind()
            gv_Menus.SelectedIndex = -1

        End If
    End Sub

    Sub LlenarOpciones()
        Dim dt_Opciones As DataTable = fn_UsuariosPrivilegios_ObtenerOpciones(Session("SucursalID"), Session("UsuarioID"), Session("MenuID"))

        If dt_Opciones IsNot Nothing AndAlso dt_Opciones.Rows.Count > 0 Then
            gv_Opciones.DataSource = dt_Opciones
            gv_Opciones.DataBind()
            gv_Opciones.SelectedIndex = -1
        End If
    End Sub

    Sub LlenarControles()
        Dim dt_Controles As DataTable = fn_UsuarioPrivilegios_ObtenerControles(Session("SucursalID"), Session("UsuarioID"), Session("OpcionID"))

        If dt_Controles IsNot Nothing AndAlso dt_Controles.Rows.Count > 0 Then
            gv_Controles.DataSource = fn_MostrarSiempre(dt_Controles)
            gv_Controles.DataBind()
        End If
    End Sub

    Protected Sub chkSeleccionaTodo_CheckedChanged(sender As Object, e As EventArgs)
        Dim ChkBoxHeader As CheckBox = DirectCast(gv_Opciones.HeaderRow.FindControl("chkSeleccionaTodo"), CheckBox)
        For Each row As GridViewRow In gv_Opciones.Rows
            Dim ChkBoxRows As CheckBox = DirectCast(row.FindControl("chkOpc"), CheckBox)
            If ChkBoxHeader.Checked = True Then
                ChkBoxRows.Checked = True
            Else
                ChkBoxRows.Checked = False
            End If
        Next
    End Sub

    Protected Sub chkSeleccionaTodoControl_CheckedChanged(sender As Object, e As EventArgs)
        Dim ChkBoxHeader As CheckBox = DirectCast(gv_Controles.HeaderRow.FindControl("chkSeleccionaTodoControl"), CheckBox)
        For Each row As GridViewRow In gv_Controles.Rows
            Dim ChkBoxRows As CheckBox = DirectCast(row.FindControl("chkControl"), CheckBox)
            If ChkBoxHeader.Checked = True Then
                ChkBoxRows.Checked = True
            Else
                ChkBoxRows.Checked = False
            End If
        Next
    End Sub

    Sub MarcarPrivilegiosOpciones()
        If ddl_Usuarios.SelectedValue = 0 Then Exit Sub

        Dim chk As CheckBox

        Dim DT As DataTable = Cn_Soporte.fn_UsuariosPrivilegios_PrivilegiosOpciones(Session("SucursalID"), Session("UsuarioID"), ddl_Usuarios.SelectedValue, ddl_Modulos.SelectedValue)
        If DT IsNot Nothing Then
            For Ilocal = 0 To DT.Rows.Count - 1
                For Jlocal = 0 To gv_Opciones.Rows.Count - 1
                    If Integer.Parse(gv_Opciones.DataKeys(Jlocal).Value) = Integer.Parse(DT.Rows(Ilocal)("Id_Opcion")) Then
                        chk = CType(gv_Opciones.Rows(Jlocal).Cells(0).FindControl("chkOpc"), CheckBox)
                        chk.Checked = True
                    End If
                Next Jlocal
            Next Ilocal
        End If
    End Sub

    Sub MarcarPrivilegiosControles()
        If ddl_Usuarios.SelectedValue = 0 Then Exit Sub

        Dim chk As CheckBox

        Dim DT As DataTable = Cn_Soporte.fn_UsuariosPrivilegios_PrivilegiosControles(Session("SucursalID"), Session("UsuarioID"), ddl_Usuarios.SelectedValue, ddl_Modulos.SelectedValue)
        If DT IsNot Nothing Then
            For Ilocal = 0 To DT.Rows.Count - 1
                For Jlocal = 0 To gv_Controles.Rows.Count - 1
                    If Integer.Parse(gv_Controles.DataKeys(Jlocal).Value) = Integer.Parse(DT.Rows(Ilocal)("Id_Control")) Then
                        chk = CType(gv_Controles.Rows(Jlocal).Cells(0).FindControl("chkControl"), CheckBox)
                        chk.Checked = True
                    End If
                Next Jlocal
            Next Ilocal
        End If
    End Sub

    Function ValidarPrivilegiosOpciones() As Integer()
        Dim arrOpciones(0) As Integer
        Dim cant As Integer = 0

        Dim chk As CheckBox

        For Each row As GridViewRow In gv_Opciones.Rows
            chk = CType(row.Cells(0).FindControl("chkOpc"), CheckBox)
            If chk.Checked Then
                ReDim Preserve arrOpciones(cant)
                arrOpciones(cant) = gv_Opciones.DataKeys(row.RowIndex).Values("Id_Opcion")
                cant += 1
            End If
        Next
        Return arrOpciones
    End Function

    Function ValidarPrivilegiosControles() As Integer()
        Dim arrControles(0) As Integer
        Dim cant As Integer = 0

        Dim chk As CheckBox

        For Each row As GridViewRow In gv_Controles.Rows
            chk = CType(row.Cells(0).FindControl("chkControl"), CheckBox)
            If chk.Checked Then
                ReDim Preserve arrControles(cant)
                arrControles(cant) = gv_Controles.DataKeys(row.RowIndex).Values("Id_Control")
                cant += 1
            End If
        Next
        Return arrControles
    End Function

    Protected Sub ddl_Usuarios_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Usuarios.SelectedIndexChanged
        Call MostrarMenusVacio()
        Call MostrarOpcionesVacio()
        Call MostrarControlesVacio()

        ddl_Modulos.SelectedValue = 0
    End Sub

    Protected Sub ddl_Modulos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Modulos.SelectedIndexChanged
        Call Mostrar_GridsVacios()

        If ddl_Modulos.SelectedValue > 0 Then
            Call LlenarMenus()
        End If
    End Sub

    Protected Sub gv_Menus_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Menus.SelectedIndexChanged

        If gv_Menus.SelectedRow.Cells(1).Text = "&nbsp;" Then Exit Sub

        Session("MenuID") = gv_Menus.SelectedDataKey("Id_Menu")
        Call MostrarOpcionesVacio()
        Call MostrarControlesVacio()

        Call LlenarOpciones()
        Call MarcarPrivilegiosOpciones()

    End Sub

    Protected Sub gv_Opciones_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Opciones.SelectedIndexChanged
        If gv_Opciones.SelectedRow.Cells(2).Text = "&nbsp;" Then Exit Sub

        Session("OpcionID") = gv_Opciones.SelectedDataKey("Id_Opcion")
        Call MostrarControlesVacio()

        Call LlenarControles()
        Call MarcarPrivilegiosControles()
    End Sub

    Protected Sub btn_GuardarOpciones_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_GuardarOpciones.Click

        If ddl_Usuarios.SelectedValue = 0 Then
            fn_Alerta("Debe seleccionar por lo menos un Usuario.")
            ddl_Usuarios.Focus()
            Exit Sub
        End If

        If ddl_Modulos.SelectedValue = 0 Then
            fn_Alerta("Debe seleccionar por lo menos un Modulo.")
            ddl_Modulos.Focus()
            Exit Sub
        End If

        If gv_Menus.Rows(0).Cells(1).Text = "&nbsp;" Then
            fn_Alerta("Debe seleccionar por lo menos un Modulo.")
            Exit Sub
        End If

        If gv_Opciones.Rows(0).Cells(2).Text = "&nbsp;" Then
            fn_Alerta("Seleccione un menu para elegir las opciones a agregar.")
            Exit Sub
        End If

        Dim selectOpcion As Boolean = False
        For Each rowPrivilegio As GridViewRow In gv_Opciones.Rows
            Dim cb As CheckBox = CType(rowPrivilegio.FindControl("chkOpc"), CheckBox)
            If cb.Checked Then
                selectOpcion = True
                Exit For
            End If
        Next
        If selectOpcion = False Then
            fn_Alerta("Seleccione por lo menos una opcion de la lista.")
            Exit Sub
        End If

        'Primero borrar los privilegios del Usuario seleccionado para el Modulo y Menu seleccionados
        Cn_Soporte.fn_UsuariosPrivilegios_Eliminar(Session("SucursalID"), Session("UsuarioID"), ddl_Usuarios.SelectedValue, gv_Menus.SelectedDataKey("Id_Menu"))

        Dim Opciones() As Integer = ValidarPrivilegiosOpciones()

        'Guardar Privilegios en Opciones en Usr_Permisos
        If Not Cn_Soporte.fn_UsuariosPrivilegios_AgregarOpciones(Session("SucursalID"), Session("UsuarioID"), ddl_Usuarios.SelectedValue, Opciones) Then
            fn_Alerta("Ha ocurrido un error al intentar Guardar los Privilegios en Opciones.")
        Else
            fn_Alerta("Se han Guardado correctamente los Privilegios.")
        End If

        'Cn_Login.fn_Log_Create("GUARDAR PRIVILEGIOS-OPCIONES PARA: " & ddl_Usuarios.SelectedItem.Text)

    End Sub

    Protected Sub btn_GuardarControles_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_GuardarControles.Click
        If gv_Controles.Rows.Count = 0 Then Exit Sub
        If Val(gv_Controles.DataKeys(0).Value) = 0 Then Exit Sub

        'Primero borrar los privilegios del Usuario seleccionado para el Modulo y Menu seleccionados
        Cn_Soporte.fn_UsuariosPrivilegios_EliminarControles(Session("SucursalID"), Session("UsuarioID"), ddl_Usuarios.SelectedValue, gv_Opciones.SelectedDataKey("Id_Opcion"))

        Dim Controles() As Integer = ValidarPrivilegiosControles()

        'Guardar Privilegios en Controles
        If Not Cn_Soporte.fn_UsuariosPrivilegios_AgregarControles(Session("SucursalID"), Session("UsuarioID"), ddl_Usuarios.SelectedValue, Controles) Then
            fn_Alerta("Ha ocurrido un error al intentar Guardar los Privilegios en Controles.")
        Else
            fn_Alerta("Se han Guardado correctamente los Privilegios.")
        End If

        'Cn_Login.fn_Log_Create("GUARDAR PRIVILEGIOS-CONTROLES PARA: " & lsv_Usuarios.SelectedItems(0).SubItems(2).Text)
    End Sub
End Class