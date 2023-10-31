﻿
Imports SISSAIntranet.Cn_Soporte
Imports SISSAIntranet.FuncionesGlobales
Imports System.Data
Imports SISSAIntranet.Cn_Mail
Imports System.Web.UI.WebControls.Unit

Partial Class wuc_EnviarMensajes
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Page.Title = "ENVIAR MENSAJES"

        Dim arr As String() = Split(Session("CadenaPriv"), ";")

        '-----------------------------------------
        'Aquí se valida que el Usuario firmado tenga realmente Privilegios para acceder a esta página
        'o se esta introduciendo la página directo en la barra de direcciones

        For x As Integer = 0 To arr.Length - 1
            If arr(x) = "ENVIAR MENSAJES" Then
                Exit For
            ElseIf x = (arr.Length - 1) Then
                MostrarAlertAjax("NO TIENE PRIVILEGIOS PARA ACCEDER A ESTA OPCION", Me, Page)
                'Session.Clear()
                'Session.Abandon()
                Response.Redirect("frm_login.aspx")
                Exit Sub
            End If
        Next

        '-------------------------------------------

        Dim dt_Modulos As DataTable
        Dim dt_Usuarios As DataTable

        dt_Modulos = fn_EnviarMensajes_LlenarLista(Session("SucursalID"), Session("UsuarioID"))

        dt_Usuarios = fn_EnviarMensajes_LlenarListaU(Session("SucursalID"), Session("UsuarioID"))

        gv_Modulos.DataSource = dt_Modulos
        gv_Modulos.DataBind()

        fn_LlenarDDL(ddl_Usuario, dt_Usuarios, "Nombre", "Id_Empleado", "0")

        gv_Usuarios.DataSource = fn_CreaGridVacio("Id_Empleado,Nombre")
        gv_Usuarios.DataBind()

        Session("UsuariosAgregados") = ""

    End Sub

    Protected Sub btn_Enviar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Enviar.Click

        If tbc_Destinos.ActiveTab.ID Is "tab_Modulos" Then

            Dim cant As Integer = 0
            Dim Modulos() As String = ValidarModulos(cant)

            If cant = 0 Then
                MostrarAlertAjax("Debe seleccionar al menos un Módulo.", btn_Enviar, Page)
                Exit Sub
            End If

            If Not ValidarTextos() Then Exit Sub

            If Not fn_EnviarMensajes_Enviar(Modulos, tbx_Asunto.Text.ToUpper, tbx_Mensaje.Text.ToUpper, Session("UsuarioID"), Session("EstacioN"), Session("ModuloClave"), Session("SucursalID")) Then
                MostrarAlertAjax("Ha ocurrido un error al intentar enviar el Mensaje.", btn_Enviar, Page)
            Else
                MostrarAlertAjax("El Mensaje se ha enviado correctamente a todos los Destinatarios.", btn_Enviar, Page)
                tbx_Asunto.Text = ""
                tbx_Mensaje.Text = ""
                DesmarcarGrid()
            End If

        Else
            Dim Usuarios() As Integer = ValidarUsuarios()

            If Session("UsuariosAgregados") = "" Then
                MostrarAlertAjax("Debe seleccionar al menos un Usuario.", btn_Enviar, Page)
                Exit Sub
            End If

            If Not ValidarTextos() Then Exit Sub

            If Not fn_EnviarMensajesU_Enviar(Usuarios, tbx_Asunto.Text.ToUpper, tbx_Mensaje.Text.ToUpper, Session("UsuarioID"), Session("EstacioN"), Session("ModuloClave"), Session("SucursalID")) Then
                MostrarAlertAjax("Ha ocurrido un error al intentar enviar el Mensaje.", btn_Enviar, Page)
            Else
                MostrarAlertAjax("El Mensaje se ha enviado correctamente a todos los Destinatarios.", btn_Enviar, Page)

                Session("UsuariosAgregados") = ""
                gv_Usuarios.DataSource = fn_CreaGridVacio("Id_Empleado,Nombre")
                gv_Usuarios.DataBind()

                ddl_Usuario.SelectedValue = 0
            End If

            Dim dt As DataTable = fn_EnviarMensajesU_ObtenerMails(Session("SucursalID"), Session("UsuarioID"))

            If Not dt Is Nothing Then
                For Each u As Integer In Usuarios
                    For Each renglon As DataRow In dt.Rows
                        If u = renglon("Id_Empleado") Then
                            fn_Enviar_Mail(renglon("Mail"), tbx_Asunto.Text.ToUpper, tbx_Mensaje.Text.ToUpper, Session("SucursalID"))
                            Exit For
                        End If
                    Next
                Next
            End If

            tbx_Asunto.Text = ""
            tbx_Mensaje.Text = ""

        End If

    End Sub

    Function ValidarTextos() As Boolean
        If tbx_Asunto.Text = "" Then
            MostrarAlertAjax("Debe Escribir un Asunto.", btn_Enviar, Page)
            tbx_Asunto.Focus()
            Return False
            Exit Function
        End If

        If tbx_Mensaje.Text = "" Then
            MostrarAlertAjax("Debe Escribir un Mensaje.", btn_Enviar, Page)
            tbx_Mensaje.Focus()
            Return False
            Exit Function
        End If
        Return True
    End Function

    Function ValidarModulos(ByRef cant As Integer) As String()
        Dim row As GridViewRow
        Dim ischecked As Boolean = False
        Dim arrMod(0) As String
        For i As Integer = 0 To gv_Modulos.Rows.Count - 1
            row = gv_Modulos.Rows(i)
            ischecked = DirectCast(row.FindControl("cbx_Modulo"), CheckBox).Checked
            If ischecked Then
                ReDim Preserve arrMod(cant)
                arrMod(cant) = gv_Modulos.DataKeys(i).Values("Clave")
                cant += 1
            End If
        Next
        Return arrMod
    End Function

    Sub DesmarcarGrid()
        For Each row As GridViewRow In gv_Modulos.Rows
            DirectCast(row.FindControl("cbx_Modulo"), CheckBox).Checked = False
        Next
    End Sub

    Function ValidarUsuarios() As Integer()
        Dim arrUsu(0) As Integer
        Dim cant As Integer = 0

        For Each row As GridViewRow In gv_Usuarios.Rows
            ReDim Preserve arrUsu(cant)
            arrUsu(cant) = gv_Usuarios.DataKeys(row.RowIndex).Values("Id_Empleado")
            cant += 1
        Next
        Return arrUsu
    End Function

    Protected Sub gv_Modulos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Modulos.PageIndexChanging
        gv_Modulos.PageIndex = e.NewPageIndex

        gv_Modulos.DataSource = fn_EnviarMensajes_LlenarLista(Session("SucursalID"), Session("UsuarioID"))
        gv_Modulos.DataBind()
    End Sub

    Protected Sub tbc_Destinos_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbc_Destinos.ActiveTabChanged
        If tbc_Destinos.ActiveTabIndex = 0 Then
            tbc_Destinos.Height = Pixel(550)
        Else
            tbc_Destinos.Height = Pixel(800)
        End If
    End Sub

    Protected Sub btn_Agregar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Agregar.Click
        If ddl_Usuario.SelectedValue = "0" Then
            MostrarAlertAjax("Seleccione un Usuario para agregar a la lista.", ddl_Usuario, Page)
            ddl_Usuario.Focus()
            Exit Sub
        End If

        For Each fila As GridViewRow In gv_Usuarios.Rows
            If gv_Usuarios.DataKeys(fila.RowIndex).Values("Id_Empleado") = ddl_Usuario.SelectedValue Then
                MostrarAlertAjax("Elemento seleccionado ya existe en la lista.", ddl_Usuario, Page)
                Exit Sub
            End If
        Next

        If Session("UsuariosAgregados") = "" Then
            Session("UsuariosAgregados") = ddl_Usuario.SelectedValue & "," & ddl_Usuario.SelectedItem.Text
        Else
            Session("UsuariosAgregados") = Session("UsuariosAgregados") & ";" & ddl_Usuario.SelectedValue & "," & ddl_Usuario.SelectedItem.Text
        End If

        gv_Usuarios.DataSource = fn_AgregarFila("Id_Empleado,Nombre", Session("UsuariosAgregados"))
        gv_Usuarios.DataBind()


    End Sub

    Protected Sub gv_Usuarios_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Usuarios.RowDeleting
        If gv_Usuarios.DataKeys(e.RowIndex).Value = "" Then Exit Sub

        ActualizarUsuarios(gv_Usuarios.DataKeys(e.RowIndex).Value)

        If Session("UsuariosAgregados") = "" Then
            gv_Usuarios.DataSource = fn_CreaGridVacio("Id_Empleado,Nombre")
            gv_Usuarios.DataBind()
        Else
            gv_Usuarios.DataSource = fn_AgregarFila("Id_Empleado,Nombre", Session("UsuariosAgregados"))
            gv_Usuarios.DataBind()
        End If
    End Sub

    Sub ActualizarUsuarios(ByVal UEliminar As String)
        Dim UsuariosActualizados As String = ""
        Dim Usuarios() As String = Split(Session("UsuariosAgregados"), ";")

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
        Session("UsuariosAgregados") = UsuariosActualizados
    End Sub

End Class
