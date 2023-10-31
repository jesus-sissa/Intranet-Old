Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.Cn_Login
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data

Partial Public Class ProspectosValida
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If IsPostBack Then Exit Sub

        fn_Log_Create(Session("UsuarioID"), Session("ModuloClave"), "ABRIR PAGINA: EVALUAR PROSPECTOS", Session("EstacioN"), Session("EstacionIP"), "", Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

        Dim dt As DataTable = fn_Prospectos_GetProspectos(Session("UsuarioID"))

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            dgv_Prospectos.DataSource = dt
            dgv_Prospectos.DataBind()
        Else
            dgv_Prospectos.DataSource = fn_CreaGridVacio("Id_EmpleadoP,FechaRegistro,Nombre,Departamento,Puesto")
            dgv_Prospectos.DataBind()
            pnl_DatosTodosP.Visible = False
        End If

        Dim Columnas As Byte
        Dim Gv_Nombres() As GridView = {dgv_Prospectos, dgv_Entrevistas}

        For j As Byte = 0 To Gv_Nombres.Length - 1
            Columnas = Gv_Nombres(j).Columns.Count

            For i As Byte = 0 To Columnas - 1
                Gv_Nombres(j).Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
            Next
        Next
    End Sub

    Protected Sub dgv_Prospectos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Prospectos.SelectedIndexChanged

        Dim resultado As Integer
        Try
            Integer.TryParse(dgv_Prospectos.DataKeys(dgv_Prospectos.SelectedIndex).Value.ToString, resultado)

            If resultado = 0 Then
                dgv_Prospectos.SelectedIndex = -1
                tbc_DatosProspecto.Visible = False
                Exit Sub
            End If
        Catch
            dgv_Prospectos.SelectedIndex = -1
            Exit Sub
        End Try
        '-------------
        tbc_DatosProspecto.Visible = True
        Call LimpiarDG()
        Call LimpiarEntrevista()
        Call LimpiarDatosFirma()
        '----------------------
        Session("EmpleadoID") = CInt(resultado)
        pnl_DatosTodosP.Visible = True
        pnl_Firma.Visible = True
        tbc_DatosProspecto.ActiveTabIndex = 0
        Call LlenarDG()
        Call LlenarListaEntrevista()
    End Sub

    Private Sub LimpiarEntrevista()
        dgv_Entrevistas.SelectedIndex = -1
        dgv_Entrevistas.DataSource = fn_CreaGridVacio("Fecha,Entrevisto,Apto,Comentarios")
        dgv_Entrevistas.DataBind()
    End Sub

    Private Sub LimpiarDG()
        tbx_NombreCompletoP.Text = String.Empty
        tbx_NombresP.Text = String.Empty
        tbx_ApellidoPaternoP.Text = String.Empty
        tbx_ApellidoMaternoP.Text = String.Empty
        tbx_DepartamentoP.Text = String.Empty
        tbx_PuestoP.Text = String.Empty
        tbx_CalleP.Text = String.Empty
        tbx_EntreCalle1P.Text = String.Empty
        tbx_EntreCalle2P.Text = String.Empty
        tbx_NumExteriorP.Text = String.Empty
        tbx_NumInteriorP.Text = String.Empty
        tbx_ColoniaP.Text = String.Empty
        tbx_CiudadP.Text = String.Empty
        tbx_ZonaP.Text = String.Empty
        tbx_CPP.Text = String.Empty
        rdb_MasculinoP.Checked = False
        rdb_FemeninoP.Checked = False
        rdb_SiConFamP.Checked = False
        rdb_NoConFamP.Checked = False
        tbx_EstadoCivilP.Text = String.Empty
        tbx_ModoNacP.Text = String.Empty
        tbx_LugarNacP.Text = String.Empty
        tbx_PaisNacP.Text = String.Empty
        tbx_FechaNacP.Text = String.Empty
        tbx_FechaNaturalizacionP.Text = String.Empty
        tbx_CantidadHijosP.Text = String.Empty
        tbx_EdadP.Text = String.Empty
        tbx_MailP.Text = String.Empty
        tbx_TelefonoP.Text = String.Empty
        tbx_TelefonoMovilP.Text = String.Empty
        tbx_ElectorP.Text = String.Empty
        tbx_RFCP.Text = String.Empty
        tbx_CURPP.Text = String.Empty
        tbx_IMSSP.Text = String.Empty
        tbx_UMFP.Text = String.Empty
        tbx_NumCartillaP.Text = String.Empty
        tbx_NumPasaporteP.Text = String.Empty
        tbx_TipoLicenciaP.Text = String.Empty
        tbx_FechaExpiraP.Text = String.Empty
        rdb_SiCatFirmasP.Checked = False
        rdb_NoCatFirmasP.Checked = False
        rdb_SiCertifP.Checked = False
        rdb_NoCertifP.Checked = False
        tbx_EmpleadoRefP.Text = String.Empty
        rdb_SiJefeP.Checked = False
        rdb_NoJefeP.Checked = False
    End Sub

    Sub LlenarListaEntrevista()
        Dim dt As DataTable = fn_Prospectos_GetEntrevistas(Session("EmpleadoID"), Session("SucursalID"), Session("UsuarioID"))
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            dgv_Entrevistas.DataSource = dt
        Else
            dgv_Entrevistas.DataSource = fn_CreaGridVacio("Fecha,Entrevisto,Apto,Comentarios")
        End If
        dgv_Entrevistas.DataBind()
    End Sub

    Sub LlenarDG()
        'Aqui se obtienen los DATOS GENERALES del Empleado seleccionado
        Dim Dr_Datos As DataRow = fn_Prospectos_ObtenDatosGenerales(Session("SucursalID"), Session("EmpleadoID"), Session("UsuarioID"))

        If Dr_Datos IsNot Nothing Then
            tbx_NombreCompletoP.Text = Dr_Datos("NombreCompleto")
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
            tbx_ApellidoPaternoP.Text = Dr_Datos("APaterno")
            tbx_ApellidoMaternoP.Text = Dr_Datos("AMaterno")
            tbx_NombresP.Text = Dr_Datos("Nombres")
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
            fn_Alerta("Seleccione Apto o No Apto para Validar.")
            tbx_Contrasena.Focus()
            Exit Sub
        End If

        If rdb_AptoP.Checked Then
            Apto = "S"
        Else
            Apto = "N"
        End If

        Call Validar()

        If Validar() Then
            Dim fecha As Date = Date.Parse(tbx_FechaEntrevista.Text) '.ToShortDateString

            If fn_Prospectos_GuardarEntrevista(Session("EmpleadoID"), fecha, Session("UsuarioID"), Session("EstacioN"), Apto, tbx_Comentarios.Text, Session("SucursalID"), Session("UsuarioID")) Then
                fn_Alerta("Los datos han sido guardados correctamente.")
            Else
                fn_Alerta("Ocurrio un error al guardar los datos.")
            End If

            Call LimpiarDatosFirma()
            Call LlenarListaEntrevista()
        End If
    End Sub

    Private Function Validar() As Boolean
        Dim Contra As String = tbx_Contrasena.Text

        If Contra = "" Then
            fn_Alerta("Indique la Contraseña.")
            tbx_Contrasena.Focus()
            Return False
        End If
        If Len(Contra) < 4 Then
            fn_Alerta("Contraseña Incorrecta.")
            tbx_Contrasena.Focus()
            Return False
        End If
        If Not FuncionesGlobales.fn_Valida_Cadena(Contra, FuncionesGlobales.Validar_Cadena.LetrasYnumeros) Then
            'En caso de que el nombre no sea valido
            fn_Alerta("Indique una Contraseña válida.")
            tbx_Contrasena.Focus()
            Return False
        End If

        Try
            Dim tbl As DataTable = Usuarios_Login(Session("UsuarioID"))

            If tbl IsNot Nothing AndAlso tbl.Rows.Count > 0 Then
                Dim PasswordUsr As String = FormsAuthentication.HashPasswordForStoringInConfigFile(Contra, "SHA1")
                Dim PasswordDb As String = tbl.Rows(0).Item("Password")
                Dim Usuario As String = tbl.Rows(0).Item("Nombre")
                Dim Tipo As Integer = tbl.Rows(0).Item("Tipo")

                If tbl.Rows(0)("Dias_Expira") < 1 Then
                    fn_Alerta("La Contraseña está expirada.")
                    Return False
                End If

                If PasswordUsr = PasswordDb Then
                         'MsgBox("Solo los usuarios tipo 2 pueden entrar a esta aplicación", Response)
                    Return True
                Else
                    fn_Alerta("Usuario o Contraseña incorrecta.")
                    tbx_Contrasena.Focus()
                    Return False
                End If
            Else
                fn_Alerta("Usuario o Contraseña incorrecta.")
                tbx_Contrasena.Focus()
                Return False
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
            Return False
        End Try
    End Function

    Sub LimpiarDatosFirma()
        tbx_FechaEntrevista.Text = ""
        tbx_Contrasena.Text = ""
        rdb_AptoP.Checked = False
        rdb_NoAptoP.Checked = False
        tbx_Comentarios.Text = ""
        pnl_Firma.Visible = False
    End Sub

    Protected Sub dgv_Prospectos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgv_Prospectos.PageIndexChanging
        Call LimpiarDG()
        Call LimpiarEntrevista()
        dgv_Prospectos.PageIndex = e.NewPageIndex
        dgv_Prospectos.SelectedIndex = -1
        Session("EmpleadoID") = 0
        dgv_Prospectos.DataSource = fn_Prospectos_GetProspectos(Session("UsuarioID"))
        dgv_Prospectos.DataBind()
    End Sub

End Class
