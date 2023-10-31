
Imports SISSAIntranet.Cn_Soporte
Imports SISSAIntranet.Cn_Login
Imports SISSAIntranet.FuncionesGlobales
Imports System.Data

Partial Public Class UserControls_wuc_Prospectos
    Inherits System.Web.UI.UserControl

    Dim EmpleadoID As Integer
    Dim Firma As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Page.Title = "EVALUAR PROSPECTOS"

            '-----------------------------------------
            'Aquí se valida que el Usuario firmado tenga realmente Privilegios para acceder a esta página
            'o se esta introduciendo la página directo en la barra de direcciones

            Dim arr As String() = Split(Session("CadenaPriv"), ";")

            For x As Integer = 0 To arr.Length - 1
                If arr(x) = "EVALUAR PROSPECTOS" Then
                    Exit For
                ElseIf x = (arr.Length - 1) Then
                    MostrarAlertAjax("NO TIENE PRIVILEGIOS PARA ACCEDER A ESTA OPCION", Me, Page)
                    Response.Redirect("frm_login.aspx")
                    Exit Sub
                End If
            Next

            '-------------------------------------------

            fn_Log_Create(Session("UsuarioID"), Session("ModuloClave"), "ABRIR PAGINA: EVALUAR PROSPECTOS", Session("EstacioN"), Session("EstacionIP"), "", Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

            Dim dt As DataTable = fn_Prospectos_GetProspectos(Session("DepartamentoID"), Session("SucursalID"), Session("UsuarioID"))

            If dt.Rows.Count > 0 Then
                dgv_Prospectos.DataSource = dt
                dgv_Prospectos.DataBind()
            Else
                dgv_Prospectos.DataSource = fn_CreaGridVacio("Id_EmpleadoP,FechaRegistro,Nombre,Departamento,Puesto")
                dgv_Prospectos.DataBind()
                pnl_DatosTodosP.Visible = False
            End If

        End If
    End Sub

    Protected Sub dgv_Prospectos_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgv_Prospectos.RowCreated
        ' En este Sub se agregan a las filas de dgv_Prospectos los atributos "onmouseover" y "onmouseout"
        ' para que cuando el puntero del mouse este sobre una fila, se apliquen las propiedades declaradas (backgoundColor)

        ' only apply changes if its DataRow
        If e.Row.RowType = DataControlRowType.DataRow Then

            ' when mouse is over the row, save original color to new attribute, and change it to highlight yellow color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#C0A062'")  '#D0A540'")

            ' when mouse leaves the row, change the bg color to its original value    
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")

        End If
    End Sub

    Protected Sub dgv_Prospectos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Prospectos.SelectedIndexChanged

        EmpleadoID = (dgv_Prospectos.DataKeys(dgv_Prospectos.SelectedIndex).Value.ToString)
        Session("EmpleadoID") = EmpleadoID
        LimpiarDatosFirma()
        pnl_DatosTodosP.Visible = True
        pnl_Firma.Visible = True
        tbc_DatosProspecto.ActiveTabIndex = 0
        LlenarDG()
        LlenarListaEntrevista()

    End Sub

    Sub LlenarListaEntrevista()
        Dim dt As DataTable = fn_Prospectos_GetEntrevistas(Session("EmpleadoID"), Session("SucursalID"), Session("UsuarioID"))
        If dt.Rows.Count > 0 Then
            dgv_Entrevistas.DataSource = dt
        Else
            dgv_Entrevistas.DataSource = fn_CreaGridVacio("Fecha,Entrevisto,Apto,Comentarios")
        End If
        'dgv_Entrevistas.DataSource = fn_Prospectos_GetEntrevistas(Session("EmpleadoID"))
        dgv_Entrevistas.DataBind()
    End Sub

    Sub LlenarDG()
        'Aqui se obtienen los DATOS GENERALES del Empleado seleccionado
        Dim Dr_Datos As DataRow = fn_Prospectos_ObtenDatosGenerales(Session("SucursalID"), EmpleadoID, Session("UsuarioID"))

        If Not Dr_Datos Is Nothing Then
            'tbx_Clave.Tag = Dr_Datos("Clave")
            tbx_NombreCompletoP.Text = Dr_Datos("NombreCompleto")
            'tbx_Clave.Text = Dr_Datos("Clave")
            tbx_DepartamentoP.Text = Dr_Datos("Departamento")
            tbx_PuestoP.Text = Dr_Datos("Puesto")
            tbx_EstadoCivilP.Text = Dr_Datos("EstadoCivil")
            tbx_CiudadP.Text = Dr_Datos("CiudadD")
            tbx_ZonaP.Text = Dr_Datos("ZonaD")

            tbx_MailP.Text = Dr_Datos("Mail")
            If Dr_Datos("Sexo") = "M" Then
                rdb_MasculinoP.Checked = True
                rdb_FemeninoP.Checked = False
            Else
                rdb_MasculinoP.Checked = False
                rdb_FemeninoP.Checked = True
            End If
            If Dr_Datos("Catalogo") = "S" Then
                rdb_SiCatFirmasP.Checked = True
                rdb_NoCatFirmasP.Checked = False
            Else
                rdb_SiCatFirmasP.Checked = False
                rdb_NoCatFirmasP.Checked = True
            End If

            'tbx_SueldoBase.Text = Dr_Datos("SueldoBase")
            tbx_CalleP.Text = Dr_Datos("Calle")
            tbx_NumExteriorP.Text = Dr_Datos("NumeroExterior")
            tbx_NumInteriorP.Text = Dr_Datos("NumeroInterior")
            tbx_ColoniaP.Text = Dr_Datos("Colonia")
            tbx_CPP.Text = Dr_Datos("CP")
            tbx_TelefonoP.Text = Dr_Datos("Telefono")
            tbx_TelefonoMovilP.Text = Dr_Datos("TelefonoMovil")
            tbx_FechaNacP.Text = CDate(Dr_Datos("FechaNac")).ToShortDateString
            tbx_LugarNacP.Text = Dr_Datos("LugarNac")
            tbx_RFCP.Text = Dr_Datos("RFC")
            tbx_CURPP.Text = Dr_Datos("CURP")
            tbx_IMSSP.Text = Dr_Datos("IMSS")
            tbx_ElectorP.Text = Dr_Datos("IFE")
            tbx_TipoLicenciaP.Text = Dr_Datos("TipoLicenciaD")
            'If Dr_Datos("Vence_Licencia") Is DBNull.Value Then
            '    tbx_FechaExpira.Text = #1/1/1900#
            'Else
            '    tbx_FechaExpira.Text = Dr_Datos("Vence_Licencia")
            'End If
            tbx_NumCartillaP.Text = Dr_Datos("NumCartilla")

            If Dr_Datos("Certificacion") = "S" Then
                rdb_SiCertifP.Checked = True
                rdb_NoCertifP.Checked = False
            Else
                rdb_SiCertifP.Checked = False
                rdb_NoCertifP.Checked = True
            End If
            If Dr_Datos("ViveConFam") = "S" Then
                rdb_SiConFamP.Checked = True
                rdb_NoConFamP.Checked = False
            Else
                rdb_SiConFamP.Checked = False
                rdb_NoConFamP.Checked = True
            End If
            tbx_CantidadHijosP.Text = Dr_Datos("CantidadHijos")
            tbx_EdadP.Text = Dr_Datos("Edad")
            'tbx_fec = Dr_Datos("FechaIngreso")
            'If Dr_Datos("FechaFin_Labores") <> "" Then
            '    tbx_Fecha_FinLabores.Text = Format(Dr_Datos("FechaFin_Labores"), "dd/MM/yyyy")
            'Else
            '    tbx_Fecha_FinLabores.Text = Dr_Datos("FechaFin_Labores")
            'End If
            tbx_ApellidoPaternoP.Text = Dr_Datos("APaterno")
            tbx_ApellidoMaternoP.Text = Dr_Datos("AMaterno")
            tbx_NombresP.Text = Dr_Datos("Nombres")
            'TBX_FechaVenceCredencial.TEXT = CDate(Dr_Datos("FechaVenceCred")).ToShortDateString
            tbx_EmpleadoRefP.Text = Dr_Datos("EmpleadoRefD")

            tbx_EntreCalle1P.Text = Dr_Datos("EntreCalle1")
            tbx_EntreCalle2P.Text = Dr_Datos("EntreCalle2")
            Dim modoNac As Integer = Dr_Datos("ModoNacionalidad")
            Select Case modoNac
                Case 1
                    tbx_ModoNacP.Text = "NACIMIENTO"
                Case 2
                    tbx_ModoNacP.Text = "NATURALIZADO"
                Case 3
                    tbx_ModoNacP.Text = "EXTRANJERO"
            End Select

            tbx_PaisNacP.Text = Dr_Datos("PaisNacimiento")
            tbx_FechaNaturalizacionP.Text = CDate(Dr_Datos("FechaNaturalizacion")).ToShortDateString
            tbx_NumPasaporteP.Text = Dr_Datos("Pasaporte")

            If Dr_Datos("JefeArea") = "S" Then
                rdb_SiJefeP.Checked = True
                rdb_NoJefeP.Checked = False
            Else
                rdb_SiJefeP.Checked = False
                rdb_NoJefeP.Checked = True
            End If

            tbx_UMFP.Text = Dr_Datos("UMF")

        End If
    End Sub

    Protected Sub btn_Aceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Aceptar.Click

        Dim Apto As String = ""

        If rdb_AptoP.Checked = False And rdb_NoAptoP.Checked = False Then
            MostrarAlertAjax("Seleccione Apto o No Apto para Validar.", btn_Aceptar, Page)
            tbx_Contrasena.Focus()
            Exit Sub
        End If

        If rdb_AptoP.Checked Then
            Apto = "S"
        Else
            Apto = "N"
        End If

        Validar()

        If Firma Then
            Dim fecha As Date = Date.Parse(tbx_FechaEntrevista.Text) '.ToShortDateString

            If fn_Prospectos_GuardarEntrevista(Session("EmpleadoID"), fecha, Session("UsuarioID"), Session("EstacioN"), Apto, tbx_Comentarios.Text, Session("SucursalID"), Session("UsuarioID")) Then
                MostrarAlertAjax("Los datos han sido guardados correctamente.", btn_Aceptar, Page)
            Else

            End If

            LimpiarDatosFirma()
            LlenarListaEntrevista()

        End If

    End Sub

    Sub Validar()

        Firma = False

        Dim Contra As String = tbx_Contrasena.Text

        If Contra = "" Then
            MostrarAlertAjax("Indique la Contraseña.", btn_Aceptar, Page)
            tbx_Contrasena.Focus()
            Exit Sub
        End If
        If Len(Contra) < 4 Then
            MostrarAlertAjax("Contraseña Incorrecta.", btn_Aceptar, Page)
            tbx_Contrasena.Focus()
            Exit Sub
        End If
        If Not FuncionesGlobales.fn_Valida_Cadena(Contra, FuncionesGlobales.Validar_Cadena.LetrasYnumeros) Then
            'En caso de que el nombre no sea valido
            MostrarAlertAjax("Indique una Contraseña válida.", btn_Aceptar, Page)
            tbx_Contrasena.Focus()
            Exit Sub
        End If

        Try
            Dim tbl As DataTable = Usuarios_Login(Session("UsuarioID"))

            If tbl.Rows.Count > 0 Then
                Dim PasswordUsr As String = FormsAuthentication.HashPasswordForStoringInConfigFile(Contra, "SHA1")
                Dim PasswordDb As String = tbl.Rows(0).Item("Password")
                Dim Usuario As String = tbl.Rows(0).Item("Nombre")
                Dim Tipo As Integer = tbl.Rows(0).Item("Tipo")

                If tbl.Rows(0)("Dias_Expira") < 1 Then
                    MostrarAlertAjax("La Contraseña está expirada.", btn_Aceptar, Page)
                    Exit Sub
                End If

                If PasswordUsr = PasswordDb Then
                    'FormsAuthentication.RedirectFromLoginPage(Usuario, False)
                    'MsgBox("Solo los usuarios tipo 2 pueden entrar a esta aplicación", Response)
                    Firma = True
                    Exit Sub
                Else
                    MostrarAlertAjax("Usuario o Contraseña incorrecta.", btn_Aceptar, Page)
                    tbx_Contrasena.Focus()
                    Exit Sub
                End If
            Else
                MostrarAlertAjax("Usuario o Contraseña incorrecta.", btn_Aceptar, Page)
                tbx_Contrasena.Focus()
                Exit Sub
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub

    Sub LimpiarDatosFirma()
        tbx_FechaEntrevista.Text = ""
        tbx_Contrasena.Text = ""
        rdb_AptoP.Checked = False
        rdb_NoAptoP.Checked = False
        tbx_Comentarios.Text = ""
        pnl_Firma.Visible = False

    End Sub

    Protected Sub dgv_Prospectos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgv_Prospectos.PageIndexChanging
        dgv_Prospectos.PageIndex = e.NewPageIndex
        dgv_Prospectos.DataSource = fn_Prospectos_GetProspectos(Session("DepartamentoID"), Session("SucursalID"), Session("UsuarioID"))
        dgv_Prospectos.DataBind()
    End Sub

    'Protected Sub btn_Calendario_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_Calendario.Click
    '    If cal_FechaEntrevista.Visible = False Then
    '        cal_FechaEntrevista.Visible = True
    '        Return
    '    End If

    '    If cal_FechaEntrevista.SelectedDate.Year < (Now.Year - 1) Then
    '        tbx_FechaEntrevista.Text = Now.Date
    '    Else
    '        tbx_FechaEntrevista.Text = cal_FechaEntrevista.SelectedDate.ToShortDateString
    '    End If
    '    cal_FechaEntrevista.Visible = False
    'End Sub

    'Protected Sub cal_FechaEntrevista_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cal_FechaEntrevista.SelectionChanged
    '    If cal_FechaEntrevista.Visible = False Then
    '        cal_FechaEntrevista.Visible = True
    '        Return
    '    End If
    '    tbx_FechaEntrevista.Text = cal_FechaEntrevista.SelectedDate.ToShortDateString
    '    cal_FechaEntrevista.Visible = False
    'End Sub
End Class
