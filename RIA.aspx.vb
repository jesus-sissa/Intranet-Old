Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.FuncionesGlobales
Imports IntranetSIAC.Cn_Login
Imports IntranetSIAC.Cn_Mail
Imports System.Data

Partial Class RIA
    Inherits BasePage
    Dim Fecha_IA As String
    Dim FechaEstFin As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        fn_Log_Create(Session("UsuarioID"), Session("ModuloClave"), "ABRIR PAGINA: CAPTURA DE REPORTE DE INCIDENTES/ACCIDENTES", Session("EstacioN"), Session("EstacionIP"), "", Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

        Dim ds As Data.DataTable = fn_Default_GetUsuarios(Session("SucursalID"), Session("UsuarioID"))
        Dim row As Data.DataRow = ds.NewRow

        row(ddl_UsuarioSeg.DataTextField) = "Seleccione..."
        row(ddl_UsuarioSeg.DataValueField) = 0
        row("Clave_Empleado") = ""
        ds.Rows.InsertAt(row, 0)
        ddl_UsuarioSeg.DataSource = ds
        ddl_UsuarioSeg.DataBind()

        pnl_AgregarI.Enabled = False

        Dim xs As String

        For x As Integer = 23 To 0 Step -1
            If x < 10 Then
                xs = "0" & x.ToString
            Else
                xs = x.ToString
            End If
            ddl_HoraIA.Items.Insert(0, xs)
        Next

        For x As Integer = 59 To 0 Step -1
            If x < 10 Then
                xs = "0" & x.ToString
            Else
                xs = x.ToString
            End If
            ddl_MinIA.Items.Insert(0, xs)
        Next

        gv_Usuarios.DataSource = fn_CreaGridVacio("Id_Empleado,Nombre,Rol")
        gv_Usuarios.DataBind()

        ddl_UsuarioSeg.SelectedValue = Session("UsuarioID")
        Session("UsuariosAgregados") = ddl_UsuarioSeg.SelectedValue & "," & ddl_UsuarioSeg.SelectedItem.Text & "," & "SECUNDARIO"
        gv_Usuarios.DataSource = fn_AgregarFila("Id_Empleado,Nombre,Rol", Session("UsuariosAgregados"))
        gv_Usuarios.DataBind()
        ddl_UsuarioSeg.SelectedValue = 0

    End Sub

    Sub InicializarUsuariosSeguimiento()
        Session("UsuariosAgregados") = Session("UsuarioID") & "," & Session("UsuarioNombre")
        gv_Usuarios.DataSource = fn_AgregarFila("Id_Empleado,Nombre,Rol", Session("UsuariosAgregados"))
        gv_Usuarios.DataBind()
        ddl_UsuarioSeg.SelectedValue = 0
    End Sub

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Guardar.Click
        Dim Descripcion As String = ""
        Dim DescripcionMail As String = ""
        Dim Notas As String = ""


        If tbx_FechaIA.Text = "" Or (Val(Right(tbx_FechaIA.Text, 4))) < (Now.Year - 1) Then
            fn_Alerta("Indíque la Fecha del Incidente/Accidente.")
            tbx_FechaIA.Focus()
            Exit Sub
        End If

        Fecha_IA = fn_Fecha102(tbx_FechaIA.Text)

        If ddl_HoraIA.SelectedValue = "00" And ddl_MinIA.SelectedValue = "00" Then
            fn_Alerta("Seleccione la Hora de I/A.")
            ddl_HoraIA.Focus()
            Exit Sub
        End If

        Dim Hora As String = ddl_HoraIA.SelectedValue & ":" & ddl_MinIA.SelectedValue

        If ddl_Tipo.SelectedValue = "0" Then
            fn_Alerta("Seleccione el Tipo.")
            ddl_Tipo.Focus()
            Exit Sub
        End If

        If ddl_Entidad.SelectedValue = "0" Then
            fn_Alerta("Seleccione la Entidad.")
            ddl_Entidad.Focus()
            Exit Sub
        End If

        If tbx_Descripcion.Text = "" Then
            fn_Alerta("Capture la Descripción.")
            tbx_Descripcion.Focus()
            Exit Sub
        End If

        If gv_Usuarios.DataKeys(0).Values("Id_Empleado") = 0 Then
            fn_Alerta("Agregue un usuario a la lista")
            btn_Agregar.Focus()
            Exit Sub
        End If

        If tbx_Notas.Text <> "" Then
            Notas = "Notas: " & tbx_Notas.Text.ToUpper
            Descripcion = tbx_Descripcion.Text.ToUpper & ";  " & Chr(13) & Notas
        Else
            Descripcion = tbx_Descripcion.Text.ToUpper
        End If

        DescripcionMail = tbx_Descripcion.Text.ToUpper

        If tbx_FechaEstFin.Text = "" Then
            FechaEstFin = Today.Date
        ElseIf (CInt(Right(tbx_FechaEstFin.Text, 4))) < (Now.Year - 1) Then
            fn_Alerta("Indíque la Fecha Estimada de Finalización.")
            tbx_FechaEstFin.Focus()
            Exit Sub
        Else
            FechaEstFin = fn_Fecha102(tbx_FechaEstFin.Text)
        End If

        Dim Usuarios As String() = LlenarUsuarios()

        Dim Id_RIAD As Integer = 0

        ' Aquí se guarda el RIA, el RIAD y los Usuarios para Seguimiento
        Dim Id_RIA As Integer = fn_IncidentesAccidentes_Guardar(Session("SucursalID"), ddl_Tipo.SelectedValue, ddl_Entidad.SelectedValue, Fecha_IA, Hora, tbx_Descripcion.Text.ToUpper, Notas, Session("UsuarioID"), Session("EstacioN"), Session("UsuarioID"), FechaEstFin, ddl_Entidad.SelectedItem.Text, Descripcion, Id_RIAD, Usuarios)
        hfd_IDRIA.Value = Id_RIA
        hfd_IDRIAD.Value = Id_RIAD

        If Id_RIA = 0 Then
            fn_Alerta("Ha ocurrido un error al intentar guardar los datos.")
            Exit Sub
        End If

        'Se consulta los datos del RIA para obtener el Número de RIA
        Dim dr_Reporte As DataRow = fn_IncidentesAccidentes_ObtenerDatos(Id_RIA, Session("SucursalID"), Session("UsuarioID"))

        If dr_Reporte Is Nothing Then
            fn_Alerta("Ha ocurrido un error al intentar obtener los datos.")
            Exit Sub
        End If

        For Each Row As GridViewRow In gv_Usuarios.Rows

            Dim Pie As String = "Agente de Servicios SIAC " & Now.Year.ToString
            Dim DetalleHTML As String = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                                     & "<tr><td colspan='4' align='center'> REPORTE DE INCIDENTES/ACCIDENTES </td></tr>" _
                                     & "<tr><td colspan='4'><hr /></td></tr>" _
                                     & "<tr><td align='right'><label><b>Número de Reporte:</b></label></td><td> " & dr_Reporte("Numero_RIA") & " </td><td></td><td></td></tr>" _
                                     & "<tr><td align='right'><label><b>Tipo:</b></label></td><td>" & ddl_Tipo.SelectedItem.Text & "</td><td></td><td></td></tr>" _
                                     & "<tr><td align='right'><label><b>Entidad:</b></label></td><td>" & ddl_Entidad.SelectedItem.Text & "</td><td></td><td></td></tr>" _
                                     & "<tr><td align='right'><label><b>Usuario Registro:</b></label></td><td>" & Session("UsuarioNombre") & "</td><td></td><td></td></tr>" _
                                     & "<tr><td align='right'><label><b>Fecha RIA:</b></label></td><td>" & tbx_FechaIA.Text & "</td><td></td><td></td></tr>" _
                                     & "<tr><td align='right'><label><b>Hora RIA:</b></label></td><td>" & Hora & "</td><td></td><td></td></tr>" _
                                     & "<tr><td align='right'><label><b>Fecha Registro:</b></label></td><td>" & Now.Date & "</td><td></td><td></td></tr>" _
                                     & "<tr><td align='right'><label><b>Hora Registro:</b></label></td><td>" & Now.ToShortTimeString & "</td><td></td><td></td></tr>" _
                                     & "<tr><td align='right'><label><b>Descripción:</b></label></td><td>" & DescripcionMail & "</td><td></td><td></td></tr>" _
                                     & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>" & Pie & "</td></tr></table><br/><br/></body></html>"

            Dim dr As DataRow = fn_Empleados_ObtenValores(gv_Usuarios.DataKeys(Row.RowIndex).Values("Id_Empleado"))

            If dr IsNot Nothing Then
                If dr("Mail") <> "" Then
                    If Not fn_Enviar_MailHTML(dr("Mail"), "REPORTE DE INCIDENTES/ACCIDENTES", DetalleHTML, "", Session("SucursalID")) Then
                        fn_Alerta("Ha ocurrido un error al intentar enviar el Correo.")
                    End If
                End If
            End If
        Next

        pnl_CapturaDatos.Enabled = False
        pnl_AgregarI.Enabled = True

        lbl_NumRIA.Text = dr_Reporte("Numero_RIA")
        lbl_Numero.Visible = True
        lbl_NumRIA.Visible = True
        lbl_Imagen.Enabled = True
        FileUpload1.Enabled = True
        btn_Subir.Enabled = True
        btn_Nuevo.Enabled = True
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

    Function LlenarUsuarios() As String()
        Dim arrUsu(0) As String
        Dim cant As Integer = 0

        For Each row As GridViewRow In gv_Usuarios.Rows
            ReDim Preserve arrUsu(cant)
            arrUsu(cant) = gv_Usuarios.DataKeys(row.RowIndex).Values("Id_Empleado") & "," & row.Cells(3).Text
            cant += 1
        Next

        Return arrUsu
    End Function

    Sub LimpiarDatos()
        tbx_FechaIA.Text = ""
        ddl_HoraIA.SelectedIndex = 0
        ddl_MinIA.SelectedIndex = 0
        ddl_Tipo.SelectedValue = 0
        tbx_Descripcion.Text = ""
        tbx_Notas.Text = ""
        ddl_UsuarioSeg.SelectedValue = 0
        'ddl_UsuarioSeg.Enabled = False
        tbx_FechaEstFin.Text = ""
        ddl_Entidad.SelectedIndex = 0
    End Sub

    Protected Sub ddl_Tipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Tipo.SelectedIndexChanged
        Dim entidad As Integer = ddl_Tipo.SelectedValue

        Select Case entidad
            Case 1
                Dim ds As Data.DataTable = fn_IncidentesAccidentes_GetUnidades(Session("SucursalID"), Session("UsuarioID"))
                Dim row As Data.DataRow = ds.NewRow

                ddl_Entidad.DataTextField = "Descripcion"
                ddl_Entidad.DataValueField = "Id_Unidad"

                fn_LlenarDDL_VariosCampos(ddl_Entidad, ds, "Descripcion", "Id_Unidad")
            Case 2, 3, 4
                Dim ds As Data.DataTable = fn_IncidentesAccidentes_GetEmpleados(Session("SucursalID"), Session("UsuarioID"))
                Dim row As Data.DataRow = ds.NewRow

                ddl_Entidad.DataTextField = "Nombre"
                ddl_Entidad.DataValueField = "Id_Empleado"

                fn_LlenarDDL_VariosCampos(ddl_Entidad, ds, "Nombre", "Id_Empleado")

            Case 5
                Dim ds As Data.DataTable = fn_IncidentesAccidentes_GetClientes(Session("SucursalID"), Session("UsuarioID"))
                Dim row As Data.DataRow = ds.NewRow

                ddl_Entidad.DataTextField = "Nombre Comercial"
                ddl_Entidad.DataValueField = "Id_Cliente"

                fn_LlenarDDL_VariosCampos(ddl_Entidad, ds, "Nombre Comercial", "Id_Cliente")

            Case 6
                Dim ds As Data.DataTable = fn_IncidentesAccidentes_GetRutas(Session("SucursalID"), Session("UsuarioID"))
                Dim row As Data.DataRow = ds.NewRow

                ddl_Entidad.DataTextField = "Descripcion"
                ddl_Entidad.DataValueField = "Id_Ruta"

                fn_LlenarDDL_VariosCampos(ddl_Entidad, ds, "Descripcion", "Id_Ruta")
        End Select
    End Sub

    Protected Sub btn_Subir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Subir.Click
        If (FileUpload1.HasFile) Then

            ' Obtener el nombre del archivo a subir.
            Dim fileName As String = Server.HtmlEncode(FileUpload1.FileName)

            ' Obtener la extensión de el archivo cargado.
            Dim extension As String = System.IO.Path.GetExtension(fileName)
            extension = LCase(extension) 'convierte a minuscula
            If (extension = ".jpg") Then

                Dim imageSize As Byte() = New Byte(FileUpload1.PostedFile.ContentLength - 1) {}

                If imageSize.LongLength > 300000 Then
                    fn_Alerta("La Imagen debe ser menor de 300 kb.")
                    FileUpload1.Focus()
                    Exit Sub
                End If

                Dim Imagen As HttpPostedFile = FileUpload1.PostedFile

                Imagen.InputStream.Read(imageSize, 0, CInt(FileUpload1.PostedFile.ContentLength))

                If Not fn_IncidentesAccidentes_GuardarImagenes(Session("SucursalID"), Session("UsuarioID"), hfd_IDRIA.Value, imageSize, "", hfd_IDRIAD.Value) Then
                          fn_Alerta("Ha ocurrido un error al intentar guardar los datos.")
                    Exit Sub
                End If

            Else
                   fn_Alerta("El Archivo debe tener la extension (.jpg).")
                FileUpload1.Focus()
                Exit Sub
            End If

        Else
            fn_Alerta("Seleccione el archivo que se va a subir.")
            Exit Sub
        End If
        '-----------------------------
        Dim DT_Imagenes = fn_IncidentesAccidentes_LeerImagenes(hfd_IDRIA.Value, Session("SucursalID"), Session("UsuarioID"))
        pnl_CapturaDatos.Enabled = False
        If DT_Imagenes.Rows.Count = 0 Then Exit Sub

        '-------------prueba con imageButton----------
        Dim Id_RiaImagen As String = "", Id_RIA As String = hfd_IDRIA.Value

        For i As Byte = 0 To DT_Imagenes.Rows.Count - 1
            Dim BotonImagen As New System.Web.UI.WebControls.ImageButton

            Id_RiaImagen = DT_Imagenes.Rows(i).Item("Id_RIAI")
            BotonImagen.ImageUrl = "~/MostrarMiniImagen.aspx?Id=" & Id_RIA & "&Id_RIAI=" & Id_RiaImagen & ""
            BotonImagen.Width = 124
            BotonImagen.Height = 93
            '------------------------
            ' BotonImagen.PostBackUrl = "MostrarImagenes.aspx?ID= " + Id_RiaImagen + "" 'en la misma ventana' 
            BotonImagen.OnClientClick = "window.open('MostrarImagenes.aspx?id=" + Id_RiaImagen + "','Search','width=500,height=400,left=150,top=200,scrollbars=0,toolbar=no,status=1');return false;"

            PlaceHolderImagenesRIA.Controls.Add(BotonImagen)
            PlaceHolderImagenesRIA.Controls.Add(New LiteralControl("&nbsp"))
        Next
        '-------------esto es con Links a href----------
        'Dim Id_RiaImagen As String = ""
        'Dim contador As Byte = 1
        'Dim Url As String, nombreLiga As String
        'PlaceHolderImagenesRIA.Controls.Add(New LiteralControl("<table border='3px' width='140'><tr><td style='background-color:black; color:White;'>Imagenes Adjuntas</td></tr></table>"))

        'For Each Img_Ria As DataRow In DT_Imagenes.Rows
        '    Id_RiaImagen = Img_Ria.Item("Id_RIAI")
        '    nombreLiga = "Imagen_" & contador
        '    Url = "MostrarImagenes.aspx?ID= " + Id_RiaImagen + "" '
        '    PlaceHolderImagenesRIA.Controls.Add(New LiteralControl("<table border='3px' width='140'><tr><td><a href=""" & Url & """ onclick=""window.open(this.href,'ViewImage','width=700,height=500,menubar=no,location=no,resizable=no,maximized=no,scrollbars=no,status=no'); return false;"">" & nombreLiga & "</a></td></tr></table>"))
        '    'PlaceHolderImagenesRIA.Controls.Add(New LiteralControl("<font size=1><br /></font>")) 'salto de liena
        '    contador += 1
        'Next
        '------------------------------------------------
    End Sub

    Protected Sub btn_Nuevo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Nuevo.Click
        LimpiarDatos()
        lbl_NumRIA.Text = ""
        lbl_NumRIA.Text = "######"
        pnl_CapturaDatos.Enabled = True
        pnl_AgregarI.Enabled = False
    End Sub

    Protected Sub btn_Agregar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Agregar.Click
        Dim TipoUsuario As String = ""

        If ddl_UsuarioSeg.SelectedValue = "0" Then
            fn_Alerta("Seleccione un Usuario para agregar a la lista.")
            ddl_UsuarioSeg.Focus()
            Exit Sub
        End If

        If Not rdb_Principal.Checked And Not rdb_Secundario.Checked Then
            fn_Alerta("Seleccione el Tipo de Usuario.")
            Exit Sub
        ElseIf rdb_Principal.Checked Then
            TipoUsuario = "PRINCIPAL"
        Else
            TipoUsuario = "SECUNDARIO"
        End If

        For Each fila As GridViewRow In gv_Usuarios.Rows
            If gv_Usuarios.DataKeys(fila.RowIndex).Values("Id_Empleado") = ddl_UsuarioSeg.SelectedValue Then
                fn_Alerta("Elemento seleccionado ya existe en la lista.")
                Exit Sub
            End If
        Next

        If Session("UsuariosAgregados") = "" Then
            Session("UsuariosAgregados") = ddl_UsuarioSeg.SelectedValue & "," & ddl_UsuarioSeg.SelectedItem.Text & "," & TipoUsuario
        Else
            Session("UsuariosAgregados") = Session("UsuariosAgregados") & ";" & ddl_UsuarioSeg.SelectedValue & "," & ddl_UsuarioSeg.SelectedItem.Text & "," & TipoUsuario
        End If

        gv_Usuarios.DataSource = fn_AgregarFila("Id_Empleado,Nombre,Rol", Session("UsuariosAgregados"))
        gv_Usuarios.DataBind()

        rdb_Principal.Checked = False
        rdb_Secundario.Checked = False
    End Sub

    Protected Sub gv_Usuarios_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Usuarios.RowDeleting
        If gv_Usuarios.DataKeys(e.RowIndex).Value = "" Then Exit Sub

        If gv_Usuarios.DataKeys(e.RowIndex).Value = Session("UsuarioID") Then
            fn_Alerta("No es posible Eliminar de la lista este Usuario.")
            Exit Sub
        End If

        ActualizarUsuarios(gv_Usuarios.DataKeys(e.RowIndex).Value)

        If Session("UsuariosAgregados") = "" Then
            gv_Usuarios.DataSource = fn_CreaGridVacio("Id_Empleado,Nombre,Rol")
            gv_Usuarios.DataBind()
        Else
            gv_Usuarios.DataSource = fn_AgregarFila("Id_Empleado,Nombre,Rol", Session("UsuariosAgregados"))
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
