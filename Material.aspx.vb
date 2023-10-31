Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data
Imports IntranetSIAC.Cn_Mail
Imports System.Threading



Public Class Material
    Inherits BasePage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Columnas As Byte
        Dim Gv_Nombres() As GridView = {gv_Solicitud}

        For j As Byte = 0 To Gv_Nombres.Length - 1
            Columnas = Gv_Nombres(j).Columns.Count

            For i As Byte = 0 To Columnas - 1
                Gv_Nombres(j).Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
            Next
        Next
    End Sub

#Region "Solicitud de Material"

    Private Sub MuestraGridMaterilVacio()
        gv_Material.SelectedIndex = -1
        gv_Material.DataSource = fn_CreaGridVacio("Id_Inventario,Clave,Descripcion,Cantidad")
        gv_Material.DataBind()
    End Sub
    Private Sub MuestraGridDetalleVacio()
        gv_Material.SelectedIndex = -1
        gv_Material.DataSource = fn_CreaGridVacio("Id_Inventario,Clave,Descripcion,CantidadSolicitada,CantidadValidada,CantidadSurtida")
        gv_Material.DataBind()

    End Sub
    Private Sub LlenarGridConsumibles()
        gv_Material.SelectedIndex = -1
        Dim dt As DataTable = fn_Insumos_ObtenerMaterial()
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            gv_Material.DataSource = dt
            gv_Material.DataBind()
        Else
            Call MuestraGridMaterilVacio()
        End If
    End Sub
    Private Sub MuestraGridSolicitudVacio()
        gv_Solicitud.SelectedIndex = -1
        gv_Solicitud.DataSource = fn_CreaGridVacio("Id_Inventario,Clave,Descripcion,Cantidad,Cantidad_Ant")
        gv_Solicitud.DataBind()
        btn_Guardar.Enabled = False
    End Sub

    Private Sub CancelarEdicionGridSolicitud()
        For Each Row As GridViewRow In gv_Solicitud.Rows
            If Row.Cells(6).FindControl("tbx_CantidadSolicitada") IsNot Nothing Then
                gv_Solicitud.EditIndex = -1
                gv_Solicitud.DataSource = fn_AgregarFila("Id_Inventario,Clave,Descripcion,Cantidad,Cantidad_Ant", InsumosAgregados_Solicitud(False, Row.RowIndex))
                gv_Solicitud.DataBind()
                Exit For
            End If
        Next
    End Sub

    Private Function InsumosAgregados_Solicitud(ByVal Editado As Boolean, Optional ByVal Fila_Edit As Integer = -1, Optional ByVal Fila_Eliminar As Integer = -1) As String
        'Editado = True significa que se deberá de tomar en cuenta el valor del textbox
        'Editado = False significa que se deberá de tomar en cuenta el valor del label
        Dim tbx_Cantidad As TextBox
        Dim lbl_Cantidad As Label
        Dim InsumosAgregar As String = ""

        For Each Row As GridViewRow In gv_Solicitud.Rows
            If Editado Then
                If Fila_Edit = Row.RowIndex Then
                    tbx_Cantidad = Row.Cells(4).FindControl("tbx_CantidadSolicitada")
                    If Val(tbx_Cantidad.Text) = 0 Then
                        fn_Alerta("Debe de Indicar una cantidad para el Material o en su defecto eliminarlo.")
                        Return ""
                    End If
                    InsumosAgregar &= gv_Solicitud.DataKeys(Row.RowIndex).Value & "," &
                                  gv_Solicitud.DataKeys(Row.RowIndex).Values("Clave") & "," &
                                  Server.HtmlDecode(gv_Solicitud.DataKeys(Row.RowIndex).Values("Descripcion")) & "," &
                                  tbx_Cantidad.Text & "," & tbx_Cantidad.Text & ";"
                Else
                    InsumosAgregar &= gv_Solicitud.DataKeys(Row.RowIndex).Value & "," &
                                  gv_Solicitud.DataKeys(Row.RowIndex).Values("Clave") & "," &
                                  Server.HtmlDecode(gv_Solicitud.DataKeys(Row.RowIndex).Values("Descripcion")) & "," &
                                  gv_Solicitud.DataKeys(Row.RowIndex).Values("Cantidad_Ant") & "," &
                                  gv_Solicitud.DataKeys(Row.RowIndex).Values("Cantidad_Ant") & ";"
                End If
            Else
                If Fila_Eliminar = Row.RowIndex Then
                    'Cuando se quiere eliminar una fila
                    Continue For
                ElseIf Fila_Edit = Row.RowIndex Then
                    'Cuando se quiere cancelar la edición pero dejar el valor anterior
                    InsumosAgregar &= gv_Solicitud.DataKeys(Row.RowIndex).Value & "," &
                                      gv_Solicitud.DataKeys(Row.RowIndex).Values("Clave") & "," &
                                      Server.HtmlDecode(gv_Solicitud.DataKeys(Row.RowIndex).Values("Descripcion")) & "," &
                                      gv_Solicitud.DataKeys(Row.RowIndex).Values("Cantidad_Ant") & "," &
                                      gv_Solicitud.DataKeys(Row.RowIndex).Values("Cantidad_Ant") & ";"
                Else
                    'Cuando se agrega normalmente
                    lbl_Cantidad = Row.Cells(4).FindControl("lbl_CantidadSolicitada")
                    InsumosAgregar &= gv_Solicitud.DataKeys(Row.RowIndex).Value & "," &
                                      gv_Solicitud.DataKeys(Row.RowIndex).Values("Clave") & "," &
                                      Server.HtmlDecode(gv_Solicitud.DataKeys(Row.RowIndex).Values("Descripcion")) & "," &
                                      lbl_Cantidad.Text & "," &
                                      gv_Solicitud.DataKeys(Row.RowIndex).Values("Cantidad_Ant") & ";"
                End If
            End If
        Next
        If InsumosAgregar <> "" Then InsumosAgregar = InsumosAgregar.Substring(0, (InsumosAgregar.Length - 1))
        Return InsumosAgregar
    End Function


    Protected Sub btn_Agregar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Agregar.Click

        ' Se valida si el grid de SubClases tiene elementos
        If gv_Material.DataKeys(0).Values("Id_Inventario").ToString = "" Then Exit Sub

        Dim Cantidad As TextBox
        Dim Con As GridViewRow
        Dim Sol As GridViewRow
        For Each Con In gv_Material.Rows
            For Each Sol In gv_Solicitud.Rows
                'Revisar los que se esten capturando con lo que se tiene ya registrado en la solicitud
                Cantidad = Con.Cells(3).FindControl("tbx_Cantidad")
                If Val(Cantidad.Text) <> 0 Then
                    btn_Guardar.Enabled = True
                    If gv_Material.DataKeys(Con.RowIndex).Value.ToString = gv_Solicitud.DataKeys(Sol.RowIndex).Value.ToString Then
                        fn_Alerta("Existen elementos seleccionados que ya existen en la Solicitud. Favor de verificar.")
                        Con.Cells(3).Focus()
                        Exit Sub
                    End If
                End If
            Next
        Next

        Call CancelarEdicionGridSolicitud()

        Dim InsumosAgregar As String = ""
        Dim Cantidad_Sol As Label
        For Each Sol In gv_Solicitud.Rows
            'Omitir la primer fila que se tiene vacía
            If gv_Solicitud.DataKeys(0).Value = "" Then Continue For

            Cantidad_Sol = Sol.Cells(3).FindControl("lbl_CantidadSolicitada")
            InsumosAgregar &= gv_Solicitud.DataKeys(Sol.RowIndex).Value.ToString & "," & gv_Solicitud.DataKeys(Sol.RowIndex).Values("Clave") _
                              & "," & Server.HtmlDecode(gv_Solicitud.DataKeys(Sol.RowIndex).Values("Descripcion")) & "," & Cantidad_Sol.Text _
                              & "," & Cantidad_Sol.Text & ";"
        Next
        For Each Con In gv_Material.Rows
            'Revisar los que se esten capturando con lo que se tiene ya registrado en la solicitud
            Cantidad = Con.Cells(3).FindControl("tbx_Cantidad")

            If Val(Cantidad.Text) = 0 Then Continue For

            InsumosAgregar &= gv_Material.DataKeys(Con.RowIndex).Value.ToString & "," & gv_Material.DataKeys(Con.RowIndex).Values("Clave") _
                              & "," & Server.HtmlDecode(gv_Material.Rows(Con.RowIndex).Cells(2).Text) & "," & Cantidad.Text _
                              & "," & Cantidad.Text & ";"

        Next

        If InsumosAgregar = "" Then
            Call MuestraGridDetalleVacio()
        Else
            InsumosAgregar = InsumosAgregar.Substring(0, (InsumosAgregar.Length - 1))
            gv_Solicitud.DataSource = fn_AgregarFila("Id_Inventario,Clave,Descripcion,Cantidad,Cantidad_Ant", InsumosAgregar)
            gv_Solicitud.DataBind()
        End If

        Call LlenarGridConsumibles()
        btn_Agregar.Enabled = True
        btn_Guardar.Visible = True


    End Sub
    Protected Sub btn_Guardar_Click(sender As Object, e As EventArgs) Handles btn_Guardar.Click


        Try

            If gv_Solicitud.DataKeys(0).Value.ToString = "" Then
                fn_Alerta("No se ha agregado ningún Material.")
                Exit Sub
            End If
            Call CancelarEdicionGridSolicitud()
            Dim InsumosAgregar As String = ""
            Dim Cantidad_Sol As Label

            For Each Sol In gv_Solicitud.Rows
                'Omitir la primer fila que se tiene vacía
                If gv_Solicitud.DataKeys(0).Value = "" Then Continue For

                Cantidad_Sol = Sol.Cells(3).FindControl("lbl_CantidadSolicitada")
                InsumosAgregar &= gv_Solicitud.DataKeys(Sol.RowIndex).Value.ToString & "," & gv_Solicitud.DataKeys(Sol.RowIndex).Values("Clave") _
                                  & "," & Server.HtmlDecode(gv_Solicitud.DataKeys(Sol.RowIndex).Values("Descripcion")) & "," & Cantidad_Sol.Text & ";"
            Next
            InsumosAgregar = InsumosAgregar.Substring(0, (InsumosAgregar.Length - 1))

            Dim SolicitudId As Integer = fn_Insumos_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), InsumosAgregar, txt_Observaciones.Text.ToUpper, 3)

            If SolicitudId = 0 Then
                fn_Alerta("Ha ocurrido un error al intentar guardar los datos.")

                Exit Sub
            End If

            Dim dr_Solicitud As DataRow = fn_Insumos_LeerDatosSolicitud(Session("SucursalID"), Session("UsuarioID"), SolicitudId)

            Dim NumSolicitud As Integer = 0
            If dr_Solicitud IsNot Nothing Then NumSolicitud = dr_Solicitud("Numero")

            Dim dt_Detalle As DataTable = fn_Insumos_LeerDetalleSolicitud(Session("SucursalID"), Session("UsuarioID"), SolicitudId)

            'Aquí se inserta la Alerta de Solicitud de Insumos

            Dim Detalles As String = "Número de Solicitud: " & NumSolicitud & Chr(13) _
                                    & "   Usuario Solicita: " & Session("UsuarioNombre") & Chr(13) _
                                    & "           Sucursal: " & Session("SucursalN") & Chr(13) _
                                    & "       Departamento: " & Session("DeptoNombre") & Chr(13) _
                                    & "    Fecha Solicitud: " & Now.ToShortDateString & " - " & Now.ToShortTimeString & Chr(13) _
                                    & "      Observaciones: " & txt_Observaciones.Text.ToUpper

            Dim DetalleHTML As String
            If dt_Detalle IsNot Nothing Then
                'DetalleHTML = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                '              & "<tr><td colspan='4' align='center'>SOLICITUD DE INSUMOS</td></tr>" _
                '              & "<tr><td colspan='4'><br><hr /></td></tr>" _
                '              & "<tr><td align='right'><label><b>Número de Solicitud:</b></label></td><td>" & NumSolicitud & "</td><td></td><td></td></tr>" _
                '              & "<tr><td align='right'><label><b>Usuario Solicita:</b></label></td><td>" & Session("UsuarioNombre") & "</td></tr>" _
                '              & "<tr><td align='right'><label><b>Sucursal:</b></label></td><td>" & Session("SucursalN") & "</td></tr>" _
                '              & "<tr><td align='right'><label><b>Departamento:</b></label></td><td>" & Session("DeptoNombre") & "</td></tr>" _
                '              & "<tr><td align='right'><label><b>Fecha Solicitud:</b></label></td><td>" & Now.ToShortDateString & " - " & Now.ToShortTimeString & "<br></td></tr>" _
                '              & "<tr><td align='right'><label><b>Observaciones:</b></label></td><td>" & txt_Observaciones.Text.ToUpper & "<br></td></tr>" _
                '              & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>Agente de Servicios SIAC</td></tr></table><br/><br/></body></html>" _
                '              & fn_DatatableToHTML(dt_Detalle, "DETALLE", 1, 3)
                DetalleHTML = "<html><style>body{font-family:'Lucida Console';}</style><body>" _
             & "<table style='border-radius: 25px; border: solid 0px ;  width:100%;'><tr><td colspan='4' align='center'><b style='color: #D68910; font-size: 25px;'>Boletín Informativo</b></td></tr>" _
             & "<tr><td colspan='4' align='center'> <b style='color: #D68910; font-size: 25px;'>SOLICITUD DE MATERIALES</b></td></tr>" _
             & "<tr><td colspan='4'><br><hr /></td></tr>" _
             & "<tr><td align='right'><label><b>Número de Solicitud:</b></label></td><td>" & NumSolicitud & "</td><td></td><td></td></tr>" _
             & "<tr><td align='right'><label><b>Usuario Solicita:</b></label></td><td>" & Session("UsuarioNombre") & "</td></tr>" _
             & "<tr><td align='right'><label><b>Sucursal:</b></label></td><td>" & Session("SucursalN") & "</td></tr>" _
             & "<tr><td align='right'><label><b>Departamento:</b></label></td><td>" & Session("DeptoNombre") & "</td></tr>" _
             & "<tr><td align='right'><label><b>Fecha Solicitud:</b></label></td><td>" & Now.ToShortDateString & " - " & Now.ToShortTimeString & "<br></td></tr>" _
             & "<tr><td align='right'><label><b>Observaciones:</b></label></td><td>" & txt_Observaciones.Text.ToUpper & "<br></td></tr>" _
             & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'> <b style='color: #D68910; font-size: 26PX;'>Agente de Servicios SIAC</b></td></tr></table><br/><br/>" _
             & "</body></html>"



            Else
                DetalleHTML = "<html><body  style='font-family: Lucida Console, Courier New, monospace;'><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                              & "<tr><td colspan='4' align='center'>SOLICITUD DE MATERIALES</td></tr>" _
                              & "<tr><td colspan='4'><br><hr /></td></tr>" _
                              & "<tr><td align='right'><label><b>Número de Solicitud:</b></label></td><td>" & NumSolicitud & "</td><td></td><td></td></tr>" _
                              & "<tr><td align='right'><label><b>Usuario Solicita:</b></label></td><td>" & Session("UsuarioNombre") & "</td></tr>" _
                              & "<tr><td align='right'><label><b>Sucursal:</b></label></td><td>" & Session("SucursalN") & "</td></tr>" _
                              & "<tr><td align='right'><label><b>Departamento:</b></label></td><td>" & Session("DeptoNombre") & "</td></tr>" _
                              & "<tr><td align='right'><label><b>Fecha Solicitud:</b></label></td><td>" & Now.ToShortDateString & " - " & Now.ToShortTimeString & "<br></td></tr>" _
                              & "<tr><td align='right'><label><b>Observaciones:</b></label></td><td>" & txt_Observaciones.Text.ToUpper & "<br></td></tr>" _
                              & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>Agente de Servicios SIAC</td></tr></table><br/><br/></body></html>"
            End If

            'Aquí se guarda la Alerta y se envian los correos
            Call ObtenerDestinos(Detalles, DetalleHTML, "SOLICITUD DE MATERIALES")
            fn_Alerta("Solictud enviada.")


            'Call MuestraGridDetalleVacio()
            txt_Observaciones.Text = String.Empty
            'Call MuestraGridDetalleVacio()MuestraGridSolicitudVacio()
            MuestraGridSolicitudVacio()

        Catch ex As Exception
            fn_Alerta("Ocurrio un error intete de nuevo por favor")
        End Try



    End Sub

    Private Function ObtenerDestinos(ByVal Detalles As String, ByVal DetalleHTML As String, ByVal Cabezera As String) As Boolean
        If fn_AlertasGeneradas_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), "55", Detalles) Then
            'Obtener los Destinos
            Dim Dt_Destinos As DataTable = fn_AlertasGeneradas_ObtenerMails("56")
            If Dt_Destinos IsNot Nothing Then
                For Each renglon As DataRow In Dt_Destinos.Rows
                    If fn_ValidarMail(renglon("Mail")) Then Cn_Mail.fn_Enviar_MailHTML(renglon("Mail"), Cabezera, DetalleHTML, "", Session("SucursalID"))
                Next
            End If
        End If
    End Function

    Private Sub gv_Solicitud_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Solicitud.RowDeleting
        If gv_Solicitud.DataKeys(0).Values("Id_Inventario").ToString = "" Then Exit Sub

        Call CancelarEdicionGridSolicitud()

        Dim InsumosAgregar As String = InsumosAgregados_Solicitud(False, -1, e.RowIndex)
        If InsumosAgregar <> "" Then
            gv_Solicitud.EditIndex = -1
            gv_Solicitud.DataSource = fn_AgregarFila("Id_Inventario,Clave,Descripcion,Cantidad,Cantidad_Ant", InsumosAgregar)
            gv_Solicitud.DataBind()
        Else
            Call MuestraGridSolicitudVacio()
            btn_Guardar.Enabled = False
        End If
    End Sub

    Protected Sub gv_Solicitud_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_Solicitud.RowEditing
        If gv_Solicitud.DataKeys(0).Values("Id_Inventario").ToString = "" Then Exit Sub
        Call CancelarEdicionGridSolicitud()

        gv_Solicitud.EditIndex = e.NewEditIndex
        gv_Solicitud.DataSource = fn_AgregarFila("Id_Inventario,Clave,Descripcion,Cantidad,Cantidad_Ant", InsumosAgregados_Solicitud(False))
        gv_Solicitud.DataBind()
    End Sub

    Protected Sub gv_Solicitud_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gv_Solicitud.RowCancelingEdit
        Call CancelarEdicionGridSolicitud()
    End Sub

    Protected Sub gv_Solicitud_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gv_Solicitud.RowUpdating
        Dim InsumosAgregar As String = InsumosAgregados_Solicitud(True, e.RowIndex)
        If InsumosAgregar <> "" Then
            gv_Solicitud.EditIndex = -1
            gv_Solicitud.DataSource = fn_AgregarFila("Id_Inventario,Clave,Descripcion,Cantidad,Cantidad_Ant", InsumosAgregar)
            gv_Solicitud.DataBind()
        Else
            e.Cancel = True
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        LlenarGridConsumibles()
        btn_Agregar.Visible = True
        btn_Guardar.Visible = False

    End Sub
    'Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    'End Sub
#End Region
End Class