
Imports SISSAIntranet.Cn_Soporte
Imports SISSAIntranet.Cn_Login
Imports System.Data
Imports SISSAIntranet.FuncionesGlobales

Partial Public Class UserControls_wuc_NuevoIngreso
    Inherits System.Web.UI.UserControl

    Dim Vistas As Integer = 0
    Dim Firma As Boolean = False
    Dim Veces As Integer = 0
    Dim Tipo As Char
    Dim RutaF As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack Then Exit Sub

        Page.Title = "VALIDAR NUEVO INGRESO"

        Dim arr As String() = Split(Session("CadenaPriv"), ";")

        '-----------------------------------------
        'Aquí se valida que el Usuario firmado tenga realmente Privilegios para acceder a esta página
        'o se esta introduciendo la página directo en la barra de direcciones

        For x As Integer = 0 To arr.Length - 1
            If arr(x) = "VALIDAR NUEVO INGRESO" Then
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


        'Titulo.InnerHtml = "Validar Nuevo Ingreso"
        fn_Log_Create(Session("UsuarioID"), Session("ModuloClave"), "ABRIR PAGINA: VALIDAR NUEVO INGRESO", Session("EstacioN"), Session("EstacionIP"), "", Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

        Dim dt As DataTable

        If Session("Dpto_Reclutamiento") = Session("DepartamentoId") Then
            dt = fn_Default_GetEmpleados(Session("SucursalID"), 0, "S", Session("UsuarioID"))
        Else
            dt = fn_Default_GetEmpleados(Session("SucursalID"), Session("DepartamentoID"), "S", Session("UsuarioID"))
        End If

        If dt IsNot Nothing Then
            If dt.Rows.Count > 0 Then
                dgv_Empleados.DataSource = dt
                dgv_Empleados.DataBind()
            Else
                dgv_Empleados.DataSource = fn_CreaGridVacio("Id_Empleado,Clave,Nombre,Departamento,Puesto")
                dgv_Empleados.DataBind()
            End If
        End If

        MuestraGridVacios()
    End Sub

    Sub MuestraGridVacios()
        dgv_Cursos.DataSource = fn_CreaGridVacio("Curso,Finalizado,FechaInicio,FechaFin,Instructor,TipoDocumento,Comentarios")
        dgv_Cursos.DataBind()

        dgv_Familiares.DataSource = fn_CreaGridVacio("Nombre,Parentesco,FechaNacimiento,Direccion,Ciudad,Telefono,Vive,MismoDomicilio")
        dgv_Familiares.DataBind()

        dgv_Empleos.DataSource = fn_CreaGridVacio("NombreEmpresa,Calle,EntreCalle1,EntreCalle2,NumeroExt,NumeroInt,Colonia,Ciudad,CodigoPostal,Latitud,Longitud,FechaIngreso,FechaBaja,Puesto,NombreJefe,PuestoJefe,Telefono,SueldoIni,SueldoFin,MotivoBaja,Otro,EmpresaSeg,PorteArmas")
        dgv_Empleos.DataBind()

        dgv_Referencias.DataSource = fn_CreaGridVacio("Descripcion,Nombre,Sexo,Ocupacion,Domicilio,EntreCalle1,EntreCalle2,NumeroExt,NumeroInt,Colonia,Ciudad,CodigoPostal,Telefono,Status")
        dgv_Referencias.DataBind()

        dgv_Señas.DataSource = fn_CreaGridVacio("Descripcion,Forma,Ubicacion,Comentarios,Cantidad")
        dgv_Señas.DataBind()
    End Sub

    Protected Sub dgv_Empleados_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Empleados.SelectedIndexChanged
        Dim resultado As Integer
        Integer.TryParse(dgv_Empleados.SelectedDataKey.Values("Id_Empleado"), resultado)

        'Establecemos el grid de Empleos No Seleccionado
        dgv_Empleos.SelectedIndex = -1

        If resultado = 0 Then
            dgv_Empleados.SelectedIndex = -1
            Exit Sub
        End If

        Session("EmpleadoID") = resultado
        tbx_Comentarios.Text = ""
        tbx_Contrasena.Text = ""
        tbc_Datos.ActiveTabIndex = 0
        LimpiaDetalle()
        LimpiarDatos()
        pnl_EmpleosDetalle.Visible = True
        LlenarDG()
        LlenarMasDatos()

        btn_Aceptar.Enabled = True

        ICompleto.ImageUrl = "~/Mostar_Fotos.aspx?Id=" & dgv_Empleados.SelectedDataKey.Value & "&Foto='Cuerpo_Completo'"
        IFrente.ImageUrl = "~/Mostar_Fotos.aspx?Id=" & dgv_Empleados.SelectedDataKey.Value & "&Foto='Frente'"
        IDerecho.ImageUrl = "~/Mostar_Fotos.aspx?Id=" & dgv_Empleados.SelectedDataKey.Value & "&Foto='Perfil_Derecho'"
        IIzquierdo.ImageUrl = "~/Mostar_Fotos.aspx?Id=" & dgv_Empleados.SelectedDataKey.Value & "&Foto='Perfil_Izquierdo'"
        IFirma.ImageUrl = "~/Mostar_Fotos.aspx?Id=" & dgv_Empleados.SelectedDataKey.Value & "&Foto='Firma'"

        'Session("cont") = 0

    End Sub

    Sub LlenarDG()

        'Aqui se obtienen los DATOS GENERALES del Empleado seleccionado
        Dim Dr_Datos As DataRow = fn_Empleados_ObtenValores(Session("SucursalID"), Session("EmpleadoID"), Session("UsuarioID"))

        If Not Dr_Datos Is Nothing Then
            tbx_NombreCompleto.Text = Dr_Datos("NombreCompleto")
            tbx_Clave.Text = Dr_Datos("Clave")
            tbx_Departamento.Text = Dr_Datos("Departamento")
            tbx_Puesto.Text = Dr_Datos("Puesto")
            tbx_EstadoCivil.Text = Dr_Datos("EstadoCivil")
            tbx_Ciudad.Text = Dr_Datos("Ciudad")
            tbx_Zona.Text = Dr_Datos("Zona")

            tbx_Mail.Text = Dr_Datos("Mail")
            If Dr_Datos("Sexo") = "M" Then
                rdb_Masculino.Checked = True
                rdb_Femenino.Checked = False
            Else
                rdb_Masculino.Checked = False
                rdb_Femenino.Checked = True
            End If
            If Dr_Datos("Catalogo") = "S" Then
                rdb_SiCatFirmas.Checked = True
                rdb_NoCatFirmas.Checked = False
            Else
                rdb_SiCatFirmas.Checked = False
                rdb_NoCatFirmas.Checked = True
            End If

            tbx_Calle.Text = Dr_Datos("Calle")
            tbx_NumExterior.Text = Dr_Datos("NumeroExterior")
            tbx_NumInterior.Text = Dr_Datos("NumeroInterior")
            tbx_Colonia.Text = Dr_Datos("Colonia")
            tbx_CP.Text = Dr_Datos("CP")
            tbx_Telefono.Text = Dr_Datos("Telefono")
            tbx_TelefonoMovil.Text = Dr_Datos("TelefonoMovil")
            tbx_FechaNac.Text = CDate(Dr_Datos("FechaNac")).ToShortDateString
            tbx_LugarNac.Text = Dr_Datos("LugarNac")
            tbx_FechaIngreso.Text = CDate(Dr_Datos("FechaIngreso")).ToShortDateString
            tbx_RFC.Text = Dr_Datos("RFC")
            tbx_CURP.Text = Dr_Datos("CURP")
            tbx_IMSS.Text = Dr_Datos("IMSS")
            tbx_Elector.Text = Dr_Datos("IFE")
            tbx_TipoLicencia.Text = Dr_Datos("TipoLicencia")
            tbx_NumCartilla.Text = Dr_Datos("NumCartilla")

            If Dr_Datos("Certificacion") = "S" Then
                rdb_SiCertif.Checked = True
                rdb_NoCertif.Checked = False
            Else
                rdb_SiCertif.Checked = False
                rdb_NoCertif.Checked = True
            End If
            If Dr_Datos("ViveConFam") = "S" Then
                rdb_SiConFam.Checked = True
                rdb_NoConFam.Checked = False
            Else
                rdb_SiConFam.Checked = False
                rdb_NoConFam.Checked = True
            End If
            tbx_CantidadHijos.Text = Dr_Datos("CantidadHijos")
            tbx_Edad.Text = Dr_Datos("Edad")
            tbx_ApellidoPaterno.Text = Dr_Datos("APaterno")
            tbx_ApellidoMaterno.Text = Dr_Datos("AMaterno")
            tbx_Nombres.Text = Dr_Datos("Nombres")
            tbx_EmpleadoRef.Text = Dr_Datos("EmpleadoRef")

            tbx_EntreCalle1.Text = Dr_Datos("EntreCalle1")
            tbx_EntreCalle2.Text = Dr_Datos("EntreCalle2")

            Select Case Dr_Datos("ModoNacionalidad")
                Case 1
                    tbx_ModoNac.Text = "NACIMIENTO"
                Case 2
                    tbx_ModoNac.Text = "NATURALIZADO"
                Case 3
                    tbx_ModoNac.Text = "EXTRANJERO"
            End Select

            tbx_PaisNac.Text = Dr_Datos("PaisNacimiento")
            tbx_FechaNaturalizacion.Text = CDate(Dr_Datos("FechaNaturalizacion")).ToShortDateString
            tbx_NumPasaporte.Text = Dr_Datos("Pasaporte")

            If Dr_Datos("JefeArea") = "S" Then
                rdb_SiJefe.Checked = True
                rdb_NoJefe.Checked = False
            Else
                rdb_SiJefe.Checked = False
                rdb_NoJefe.Checked = True
            End If

            'Aquí se llenan los Datos Varios
            tbx_Vicios.Text = Dr_Datos("Vicios")
            tbx_Idiomas.Text = Dr_Datos("Idiomas")
            tbx_ActividadesCulturales.Text = Dr_Datos("ActivDepCult")
            tbx_Habilidades.Text = Dr_Datos("Habilidades")
            tbx_UMF.Text = Dr_Datos("UMF")

            If Dr_Datos("CreditoINFONAVIT") = "S" Then
                rdb_SiINFONAVIT.Checked = True
                rdb_NoINFONAVIT.Checked = False
            Else
                rdb_SiINFONAVIT.Checked = False
                rdb_NoINFONAVIT.Checked = True
            End If

            tbx_FechaVenceCred.Text = Dr_Datos("FechaVenceCred")

            If Dr_Datos("SaleRuta") = "S" Then
                rdb_SiSaleRuta.Checked = True
                rdb_NoSaleRuta.Checked = False
            Else
                rdb_SiSaleRuta.Checked = False
                rdb_NoSaleRuta.Checked = True
            End If

            If Dr_Datos("VerificaDepositos") = "S" Then
                rdb_SiVerificaDepositos.Checked = True
                rdb_NoVerificaDepositos.Checked = False
            Else
                rdb_SiVerificaDepositos.Checked = False
                rdb_NoVerificaDepositos.Checked = True
            End If

        End If

    End Sub

    Sub LlenarMasDatos()

        'Aquí se obtienen los RASGOS del Empleado seleccionado
        Dim dr_Rasgos As DataRow = fn_Rasgos_ObtenValores(Session("EmpleadoID"), Session("SucursalID"), Session("UsuarioID"))

        If Not dr_Rasgos Is Nothing Then

            Select Case dr_Rasgos("Complexion")
                Case 1
                    tbx_Complexion.Text = "DELGADA"
                Case 2
                    tbx_Complexion.Text = "REGULAR"
                Case 3
                    tbx_Complexion.Text = "ROBUSTA"
                Case 4
                    tbx_Complexion.Text = "ATLETICA"
                Case 5
                    tbx_Complexion.Text = "OBESA"
            End Select
            Select Case dr_Rasgos("PielColor")
                Case 0
                    tbx_PielColor.Text = ""
                Case 1
                    tbx_PielColor.Text = "ALBINO"
                Case 2
                    tbx_PielColor.Text = "BLANCO"
                Case 3
                    tbx_PielColor.Text = "AMARILLO"
                Case 4
                    tbx_PielColor.Text = "MORENO CLARO"
                Case 5
                    tbx_PielColor.Text = "MORENO"
                Case 6
                    tbx_PielColor.Text = "MORENO OSCURO"
                Case 7
                    tbx_PielColor.Text = "NEGRO"
                Case 8
                    tbx_PielColor.Text = "OTRO"
            End Select
            Select Case dr_Rasgos("CaraForma")
                Case 0
                    tbx_Cara.Text = ""
                Case 1
                    tbx_Cara.Text = "ALARGADA"
                Case 2
                    tbx_Cara.Text = "CUADRADA"
                Case 3
                    tbx_Cara.Text = "OVALADA"
                Case 4
                    tbx_Cara.Text = "REDONDA"
            End Select
            Select Case dr_Rasgos("SangreTipo")
                Case 0
                    tbx_TipoSangre.Text = ""
                Case 1
                    tbx_TipoSangre.Text = "A"
                Case 2
                    tbx_TipoSangre.Text = "B"
                Case 3
                    tbx_TipoSangre.Text = "O"
                Case 4
                    tbx_TipoSangre.Text = "AB"
            End Select
            Select Case dr_Rasgos("SangreFactorRH")
                Case 0
                    tbx_FactorRH.Text = ""
                Case 1
                    tbx_FactorRH.Text = "POSITIVO"
                Case 2
                    tbx_FactorRH.Text = "NEGATIVO"
            End Select

            tbx_Peso.Text = dr_Rasgos("Peso")
            tbx_Estatura.Text = dr_Rasgos("Estatura")

            If dr_Rasgos("UsaAnteojos") = "0" Then
                tbx_UsaAnteojos.Text = ""
            ElseIf dr_Rasgos("UsaAnteojos") = "S" Then
                tbx_UsaAnteojos.Text = "SI"
            ElseIf dr_Rasgos("UsaAnteojos") = "N" Then
                tbx_UsaAnteojos.Text = "NO"
            End If

            Select Case dr_Rasgos("CabelloCantidad")
                Case 0
                    tbx_CabelloCantidad.Text = ""
                Case 1
                    tbx_CabelloCantidad.Text = "ABUNDANTE"
                Case 2
                    tbx_CabelloCantidad.Text = "ESCASO"
                Case 3
                    tbx_CabelloCantidad.Text = "REGULAR"
                Case 4
                    tbx_CabelloCantidad.Text = "SIN CABELLO"
            End Select
            Select Case dr_Rasgos("CabelloColor")
                Case 0
                    tbx_CabelloColor.Text = ""
                Case 1
                    tbx_CabelloColor.Text = "ALBINO"
                Case 2
                    tbx_CabelloColor.Text = "CANO TOTAL"
                Case 3
                    tbx_CabelloColor.Text = "CASTAÑO CLARO"
                Case 4
                    tbx_CabelloColor.Text = "CASTAÑO OSCURO"
                Case 5
                    tbx_CabelloColor.Text = "ENTRECANO"
                Case 6
                    tbx_CabelloColor.Text = "NEGRO"
                Case 7
                    tbx_CabelloColor.Text = "PELIRROJO"
                Case 8
                    tbx_CabelloColor.Text = "RUBIO"
            End Select
            Select Case dr_Rasgos("CabelloForma")
                Case 0
                    tbx_CabelloForma.Text = ""
                Case 1
                    tbx_CabelloForma.Text = "CRESPO"
                Case 2
                    tbx_CabelloForma.Text = "LACIO"
                Case 3
                    tbx_CabelloForma.Text = "ONDULADO"
                Case 4
                    tbx_CabelloForma.Text = "RIZADO"
            End Select
            Select Case dr_Rasgos("CabelloCalvicie")
                Case 0
                    tbx_CabelloCalvicie.Text = ""
                Case 1
                    tbx_CabelloCalvicie.Text = "FRONTAL"
                Case 2
                    tbx_CabelloCalvicie.Text = "TONSURAL"
                Case 3
                    tbx_CabelloCalvicie.Text = "FRONTOPARIETAL"
                Case 4
                    tbx_CabelloCalvicie.Text = "TOTAL"
            End Select
            Select Case dr_Rasgos("CabelloImplantacion")
                Case 0
                    tbx_CabelloImplantacion.Text = ""
                Case 1
                    tbx_CabelloImplantacion.Text = "CIRCULAR"
                Case 2
                    tbx_CabelloImplantacion.Text = "RECTANGULAR"
                Case 3
                    tbx_CabelloImplantacion.Text = "EN PUNTA"
            End Select
            Select Case dr_Rasgos("FrenteAltura")
                Case 0
                    tbx_FrenteAltura.Text = ""
                Case 1
                    tbx_FrenteAltura.Text = "GRANDE"
                Case 2
                    tbx_FrenteAltura.Text = "MEDIANA"
                Case 3
                    tbx_FrenteAltura.Text = "PEQUEÑA"
            End Select
            Select Case dr_Rasgos("FrenteInclinacion")
                Case 0
                    tbx_FrenteInclinacion.Text = ""
                Case 1
                    tbx_FrenteInclinacion.Text = "OBLICUA"
                Case 2
                    tbx_FrenteInclinacion.Text = "INTERMEDIA"
                Case 3
                    tbx_FrenteInclinacion.Text = "VERTICAL"
                Case 4
                    tbx_FrenteInclinacion.Text = "PROMINENTE"
            End Select
            Select Case dr_Rasgos("FrenteAncho")
                Case 0
                    tbx_FrenteAncho.Text = ""
                Case 1
                    tbx_FrenteAncho.Text = "GRANDE"
                Case 2
                    tbx_FrenteAncho.Text = "MEDIANA"
                Case 3
                    tbx_FrenteAncho.Text = "PEQUEÑA"
            End Select
            Select Case dr_Rasgos("CejasDireccion")
                Case 0
                    tbx_CejasDireccion.Text = ""
                Case 1
                    tbx_CejasDireccion.Text = "INTERNAS"
                Case 2
                    tbx_CejasDireccion.Text = "EXTERNAS"
                Case 3
                    tbx_CejasDireccion.Text = "HORIZONTAL"
            End Select
            Select Case dr_Rasgos("CejasImplantacion")
                Case 0
                    tbx_CejasImplantacion.Text = ""
                Case 1
                    tbx_CejasImplantacion.Text = "ALTAS"
                Case 2
                    tbx_CejasImplantacion.Text = "BAJAS"
                Case 3
                    tbx_CejasImplantacion.Text = "PROXIMAS"
                Case 4
                    tbx_CejasImplantacion.Text = "SEPARADAS"
            End Select
            Select Case dr_Rasgos("CejasForma")
                Case 0
                    tbx_CejasForma.Text = ""
                Case 1
                    tbx_CejasForma.Text = "ARQUEADAS"
                Case 2
                    tbx_CejasForma.Text = "ARQ SINUOSAS"
                Case 3
                    tbx_CejasForma.Text = "RECTILINEAS"
                Case 4
                    tbx_CejasForma.Text = "RECT SINUOSAS"
            End Select
            Select Case dr_Rasgos("CejasTamano")
                Case 0
                    tbx_CejasTamaño.Text = ""
                Case 1
                    tbx_CejasTamaño.Text = "GRUESAS"
                Case 2
                    tbx_CejasTamaño.Text = "DELGADAS"
                Case 3
                    tbx_CejasTamaño.Text = "CORTAS"
                Case 4
                    tbx_CejasTamaño.Text = "LARGAS"
            End Select
            Select Case dr_Rasgos("OjosColor")
                Case 0
                    tbx_OjosColor.Text = ""
                Case 1
                    tbx_OjosColor.Text = "AZUL"
                Case 2
                    tbx_OjosColor.Text = "CAFE CLARO"
                Case 3
                    tbx_OjosColor.Text = "CAFE OSCURO"
                Case 4
                    tbx_OjosColor.Text = "GRIS"
                Case 5
                    tbx_OjosColor.Text = "VERDE"
                Case 6
                    tbx_OjosColor.Text = "OTRO"
            End Select
            Select Case dr_Rasgos("OjosForma")
                Case 0
                    tbx_OjosForma.Text = ""
                Case 1
                    tbx_OjosForma.Text = "ALRGADOS"
                Case 2
                    tbx_OjosForma.Text = "S"
                Case 3
                    tbx_OjosForma.Text = "OVALES"
            End Select
            Select Case dr_Rasgos("OjosTamano")
                Case 0
                    tbx_OjosTamaño.Text = ""
                Case 1
                    tbx_OjosTamaño.Text = "GRANDES"
                Case 2
                    tbx_OjosTamaño.Text = "PEQUEÑOS"
                Case 3
                    tbx_OjosTamaño.Text = "REGULARES"
            End Select
            Select Case dr_Rasgos("NarizRaiz")
                Case 0
                    tbx_NarizRaiz.Text = ""
                Case 1
                    tbx_NarizRaiz.Text = "GRANDE"
                Case 2
                    tbx_NarizRaiz.Text = "MEDIANA"
                Case 3
                    tbx_NarizRaiz.Text = "PEQUEÑA"
            End Select
            Select Case dr_Rasgos("NarizDorso")
                Case 0
                    tbx_NarizDorso.Text = ""
                Case 1
                    tbx_NarizDorso.Text = "CONCAVO"
                Case 2
                    tbx_NarizDorso.Text = "CONVEXO"
                Case 3
                    tbx_NarizDorso.Text = "RECTO"
                Case 4
                    tbx_NarizDorso.Text = "SINUOSO"
            End Select
            Select Case dr_Rasgos("NarizAncho")
                Case 0
                    tbx_NarizAncho.Text = ""
                Case 1
                    tbx_NarizAncho.Text = "GRANDE"
                Case 2
                    tbx_NarizAncho.Text = "MEDIANA"
                Case 3
                    tbx_NarizAncho.Text = "PEQUEÑA"
            End Select
            Select Case dr_Rasgos("NarizBase")
                Case 0
                    tbx_NarizBase.Text = ""
                Case 1
                    tbx_NarizBase.Text = "ABATIDA"
                Case 2
                    tbx_NarizBase.Text = "HORIZONTAL"
                Case 3
                    tbx_NarizBase.Text = "LEVANTADA"
            End Select
            Select Case dr_Rasgos("NarizAltura")
                Case 0
                    tbx_NarizAltura.Text = ""
                Case 1
                    tbx_NarizAltura.Text = "GRANDE"
                Case 2
                    tbx_NarizAltura.Text = "MEDIANA"
                Case 3
                    tbx_NarizAltura.Text = "PEQUEÑA"
            End Select
            Select Case dr_Rasgos("BocaTamano")
                Case 0
                    tbx_BocaTamaño.Text = ""
                Case 1
                    tbx_BocaTamaño.Text = "GRANDE"
                Case 2
                    tbx_BocaTamaño.Text = "MEDIANA"
                Case 3
                    tbx_BocaTamaño.Text = "PEQUEÑA"
            End Select
            Select Case dr_Rasgos("BocaComisuras")
                Case 0
                    tbx_BocaComisuras.Text = ""
                Case 1
                    tbx_BocaComisuras.Text = "ABATIDAS"
                Case 2
                    tbx_BocaComisuras.Text = "ELEVADAS"
                Case 3
                    tbx_BocaComisuras.Text = "SIMETRICAS"
                Case 4
                    tbx_BocaComisuras.Text = "ASIMETRICAS"
            End Select
            Select Case dr_Rasgos("LabiosEspesor")
                Case 0
                    tbx_LabiosEspesor.Text = ""
                Case 1
                    tbx_LabiosEspesor.Text = "DELGADOS"
                Case 2
                    tbx_LabiosEspesor.Text = "MEDIANOS"
                Case 3
                    tbx_LabiosEspesor.Text = "GRUESOS"
                Case 4
                    tbx_LabiosEspesor.Text = "MORRUDOS"
            End Select
            Select Case dr_Rasgos("LabiosAlturaNasolabial")
                Case 0
                    tbx_LabiosAltura.Text = ""
                Case 1
                    tbx_LabiosAltura.Text = "GRANDE"
                Case 2
                    tbx_LabiosAltura.Text = "MEDIANA"
                Case 3
                    tbx_LabiosAltura.Text = "PEQUEÑA"
            End Select
            Select Case dr_Rasgos("LabiosProminencia")
                Case 0
                    tbx_LabiosProminencia.Text = ""
                Case 1
                    tbx_LabiosProminencia.Text = "LABIO INFERIOR"
                Case 2
                    tbx_LabiosProminencia.Text = "LABIO SUPERIOR"
                Case 3
                    tbx_LabiosProminencia.Text = "NINGUNO"
            End Select
            Select Case dr_Rasgos("MentonTipo")
                Case 0
                    tbx_MentonTipo.Text = ""
                Case 1
                    tbx_MentonTipo.Text = "BILOVADO"
                Case 2
                    tbx_MentonTipo.Text = "FOSETA"
                Case 3
                    tbx_MentonTipo.Text = "BORLA"
                Case 4
                    tbx_MentonTipo.Text = "NINGUNA"
            End Select
            Select Case dr_Rasgos("MentonForma")
                Case 0
                    tbx_MentonForma.Text = ""
                Case 1
                    tbx_MentonForma.Text = "OVAL"
                Case 2
                    tbx_MentonForma.Text = "CUADRADO"
                Case 3
                    tbx_MentonForma.Text = "EN PUNTA"
            End Select
            Select Case dr_Rasgos("MentonInclinacion")
                Case 0
                    tbx_MentonInclinacion.Text = ""
                Case 1
                    tbx_MentonInclinacion.Text = "HUYENTE"
                Case 2
                    tbx_MentonInclinacion.Text = "PROMINENTE"
                Case 3
                    tbx_MentonInclinacion.Text = "VERTICAL"
            End Select
            Select Case dr_Rasgos("OrejaForma")
                Case 0
                    tbx_OrejaForma.Text = ""
                Case 1
                    tbx_OrejaForma.Text = "CUADRADA"
                Case 2
                    tbx_OrejaForma.Text = "OVALADA"
                Case 3
                    tbx_OrejaForma.Text = "REDONDA"
                Case 4
                    tbx_OrejaForma.Text = "TRIANGULAR"
            End Select
            Select Case dr_Rasgos("OrejaOriginal")
                Case 0
                    tbx_OrejaOriginal.Text = ""
                Case 1
                    tbx_OrejaOriginal.Text = "GRANDE"
                Case 2
                    tbx_OrejaOriginal.Text = "MEDIANO"
                Case 3
                    tbx_OrejaOriginal.Text = "PEQUEÑO"
            End Select
            Select Case dr_Rasgos("HelixSuperior")
                Case 0
                    tbx_HelixSuperior.Text = ""
                Case 1
                    tbx_HelixSuperior.Text = "GRANDE"
                Case 2
                    tbx_HelixSuperior.Text = "MEDIANA"
                Case 3
                    tbx_HelixSuperior.Text = "PEQUEÑA"
            End Select
            Select Case dr_Rasgos("HelixPosterior")
                Case 0
                    tbx_HelixPosterior.Text = ""
                Case 1
                    tbx_HelixPosterior.Text = "GRANDE"
                Case 2
                    tbx_HelixPosterior.Text = "MEDIANA"
                Case 3
                    tbx_HelixPosterior.Text = "PEQUEÑA"
            End Select
            Select Case dr_Rasgos("HelixAdherencia")
                Case 0
                    tbx_HelixAdherencia.Text = ""
                Case 1
                    tbx_HelixAdherencia.Text = "UNIDO"
                Case 2
                    tbx_HelixAdherencia.Text = "SEPARADO"
                Case 3
                    tbx_HelixAdherencia.Text = "MUY SEPARADO"
            End Select
            Select Case dr_Rasgos("HelixContorno")
                Case 0
                    tbx_HelixContorno.Text = ""
                Case 1
                    tbx_HelixContorno.Text = "DESCENDENTE"
                Case 2
                    tbx_HelixContorno.Text = "EN ESCUADRA"
                Case 3
                    tbx_HelixContorno.Text = "EN GOLFO"
                Case 4
                    tbx_HelixContorno.Text = "INTERMEDIO"
            End Select
            Select Case dr_Rasgos("LobuloAdherencia")
                Case 0
                    tbx_LobuloAdherencia.Text = ""
                Case 1
                    tbx_LobuloAdherencia.Text = "UNIDO"
                Case 2
                    tbx_LobuloAdherencia.Text = "SEPARADO"
                Case 3
                    tbx_LobuloAdherencia.Text = "MUY SEPARADO"
            End Select
            Select Case dr_Rasgos("LobuloDimension")
                Case 0
                    tbx_LobuloDimension.Text = ""
                Case 1
                    tbx_LobuloDimension.Text = "GRANDE"
                Case 2
                    tbx_LobuloDimension.Text = "MEDIANO"
                Case 3
                    tbx_LobuloDimension.Text = "PEQUEÑO"
            End Select
            Select Case dr_Rasgos("LobuloParticularidad")
                Case 0
                    tbx_LobuloParticularidad.Text = ""
                Case 1
                    tbx_LobuloParticularidad.Text = "PERFORADO"
                Case 2
                    tbx_LobuloParticularidad.Text = "FOSETA"
                Case 3
                    tbx_LobuloParticularidad.Text = "ISLOTE"
            End Select

        End If


        ' Aqui se obtienen los DATOS ESCOLARES del Empleado seleccionado
        Dim Dr_DatosEscolares As DataRow = fn_EmpleadosEscolares_ObtenValores(Session("EmpleadoID"), Session("SucursalID"), Session("UsuarioID"))

        If Not Dr_DatosEscolares Is Nothing Then
            tbx_UltimosEstudios.Text = Dr_DatosEscolares("GradoEscolar")
            tbx_Documentacion.Text = Dr_DatosEscolares("TipoDoctoEscolar")
            tbx_NombreEscuela.Text = Dr_DatosEscolares("NombreEscuela")
            tbx_Carrera.Text = Dr_DatosEscolares("Carrera")
            tbx_Especialidad.Text = Dr_DatosEscolares("Especialidad")
            tbx_AInicio.Text = Dr_DatosEscolares("FechaInicio")
            tbx_ATermino.Text = Dr_DatosEscolares("FechaFin")
            tbx_Folio.Text = Dr_DatosEscolares("Folio")
            tbx_Promedio.Text = Dr_DatosEscolares("Promedio")
            tbx_CedulaProfesional.Text = Dr_DatosEscolares("CedulaProfesional")
        End If


        'Aqui se llena el listview de CURSOS RECIBIDOS con los datos del empleado seleccionado
        Dim dt_Cursos As DataTable = fn_CursosRecibidos_ObtenValores(Session("EmpleadoID"), Session("SucursalID"), Session("UsuarioID"))
        If dt_Cursos.Rows.Count > 0 Then
            dgv_Cursos.DataSource = dt_Cursos 'fn_CursosRecibidos_ObtenValores(Session("EmpleadoID"))
        Else
            dgv_Cursos.DataSource = fn_CreaGridVacio("Curso,Finalizado,FechaInicio,FechaFin,Instructor,TipoDocumento,Comentarios")
        End If
        dgv_Cursos.DataBind()



        'Aqui se llena el listview de DATOS FAMILIARES con los datos del empleado seleccionado
        Dim dt_Fam As DataTable = fn_DatosFamiliares_ObtenValores(Session("EmpleadoID"), Session("SucursalID"), Session("UsuarioID"))
        If dt_Fam.Rows.Count > 0 Then
            dgv_Familiares.DataSource = dt_Fam
        Else
            dgv_Familiares.DataSource = fn_CreaGridVacio("Nombre,Parentesco,FechaNacimiento,Direccion,Ciudad,Telefono,Vive,MismoDomicilio")
        End If
        dgv_Familiares.DataBind()



        'Aquí se llena el listview de EMPLEOS con los datos del empleado seleccionado
        Dim dt_Empleos As DataTable = fn_Empleos_ObtenValores(Session("EmpleadoID"), Session("SucursalID"), Session("UsuarioID"))
        If dt_Empleos.Rows.Count > 0 Then
            dgv_Empleos.DataSource = dt_Empleos
        Else
            dgv_Empleos.DataSource = fn_CreaGridVacio("NombreEmpresa,Calle,EntreCalle1,EntreCalle2,NumeroExt,NumeroInt,Colonia,Ciudad,CodigoPostal,Latitud,Longitud,FechaIngreso,FechaBaja,Puesto,NombreJefe,PuestoJefe,Telefono,SueldoIni,SueldoFin,MotivoBaja,Otro,EmpresaSeg,PorteArmas")
        End If
        dgv_Empleos.DataBind()



        'Aquí se llena el listview de REFERENCIAS con los datos del empleado seleccionado
        Dim dt_Ref As DataTable = fn_Referencias_ObtenValores(Session("EmpleadoID"), Session("SucursalID"), Session("UsuarioID"))
        If dt_Ref.Rows.Count > 0 Then
            dgv_Referencias.DataSource = dt_Ref
        Else
            dgv_Referencias.DataSource = fn_CreaGridVacio("Descripcion,Nombre,Sexo,Ocupacion,Domicilio,EntreCalle1,EntreCalle2,NumeroExt,NumeroInt,Colonia,Ciudad,CodigoPostal,Telefono,Status")
        End If
        dgv_Referencias.DataBind()



        'Aquí se obtienen los DATOS VARIOS del empleado seleccionado
        'Los datos de ingresos
        Dim Dr_DatosIngresos As DataRow = fn_EmpleadosIngresos_ObtenValores(Session("EmpleadoID"), Session("SucursalID"), Session("UsuarioID"))

        If Not Dr_DatosIngresos Is Nothing Then

            tbx_IngresoFamiliar.Text = Dr_DatosIngresos("Ingreso_Mensual")
            tbx_IngresoAdicional.Text = Dr_DatosIngresos("Ingreso_Adicional")
            tbx_DescripcionAdicional.Text = Dr_DatosIngresos("Descripcion_Adicional")
            tbx_GastoFamiliar.Text = Dr_DatosIngresos("Gasto_Mensual")
            tbx_TipoVivienda.Text = Dr_DatosIngresos("TipoVivienda")
            tbx_PagoMensual.Text = Dr_DatosIngresos("Pago_Mensual")
            tbx_ValorVivienda.Text = Dr_DatosIngresos("Valor_Vivienda")
            If Dr_DatosIngresos("Vehiculo_Propio") = "S" Then
                rdb_SiVehiculo.Checked = True
                rdb_NoVehiculo.Checked = False
            Else
                rdb_SiVehiculo.Checked = False
                rdb_NoVehiculo.Checked = True
            End If
            tbx_Modelo.Text = Dr_DatosIngresos("Modelo_Vehiculo")
            tbx_ValorVehiculo.Text = Dr_DatosIngresos("Valor_Vehiculo")

        End If

        'Se llena el listview de Señas Particulares
        Dim dt_Senas As DataTable = fn_SeñasParticulares_ObtenValores(Session("EmpleadoID"), Session("SucursalID"), Session("UsuarioID"))
        If dt_Senas.Rows.Count > 0 Then
            dgv_Señas.DataSource = dt_Senas
        Else
            dgv_Señas.DataSource = fn_CreaGridVacio("Descripcion,Forma,Ubicacion,Comentarios,Cantidad")
        End If
        dgv_Señas.DataBind()



        'Aquí se obtienen los datos de la PAPELERIA RECIBIDA del Empleado seleccionado
        Dim DT_Papeleria As DataTable = fn_PapeleriaRecibida_ObtenValores(Session("EmpleadoID"), Session("SucursalID"), Session("UsuarioID"))

        'Aquí se chequean los Tipos de Documentos Requeridos
        'si se encuentra el registro correspondiente para ese documento

        Dim DocumentoRecibido As Integer = 0
        For Each dr_Papeleria As DataRow In DT_Papeleria.Rows
            DocumentoRecibido = dr_Papeleria("Id_DoctoR")
            'Select Case dr_Papeleria("Id_DoctoR")
            Select Case DocumentoRecibido
                Case 1
                    chb_ActaNac.Checked = True
                Case 2
                    chb_ActaMatrimonio.Checked = True
                Case 3
                    chb_IMSS.Checked = True
                Case 4
                    chb_CompDom.Checked = True
                Case 5
                    chb_IFE.Checked = True
                Case 6
                    chb_CompEstudios.Checked = True
                Case 7
                    chb_Cartilla.Checked = True
                Case 8
                    chb_Recomendacion.Checked = True
                Case 9
                    chb_NoAntecedentes.Checked = True
                Case 10
                    chb_Fotografias.Checked = True
                Case 11
                    chb_CURP.Checked = True
                Case 12
                    chb_Huellas.Checked = True
            End Select

        Next

        '------------------------------------------------------------------
        'Aquí estoy intentando cargar las fotos del Empleado seleccionado
        'pero como que no se deja
        '------------------------------------------------------------------

        'Dim dr As DataRow = fn_Empleados_LeerImagenes(1107) 'Session("EmpleadoID"))
        'If dr IsNot Nothing Then
        '    'If IsPostBack Then Exit Sub
        '    '    'Frente
        '    If Not IsDBNull(dr("Frente")) Then
        '        'Dim RutaF As String = "c:\SIAC\Modulo_Reclutamiento_WEB\Imagenes\" & (Session("EmpleadoID").ToString) & "Frente.jpg"
        '        RutaF = "c:\SIAC\Modulo_Reclutamiento_WEB\Imagenes\" & "1107" & "Frente.jpg"
        '        If File.Exists(RutaF) Then File.Delete(RutaF)

        '        Dim fs As New FileStream(RutaF, FileMode.CreateNew, FileAccess.Write)
        '        Dim bw As New BinaryWriter(fs)
        '        bw.Write(dr("Frente"), 0, dr("Frente").Length)
        '        fs.Flush()
        '        'fs.Close()
        '        Response.ContentType = "image/jpg"
        '        'Context.Response.BinaryWrite(dr("Frente"))
        '        'Context.Response.BinaryWrite(DirectCast(dr("Frente"), Byte()))
        '        'pnlFoto.Visible = True
        '        'IFrente.ImageUrl = RutaF
        '        Response.Write("<script>iDerecho.src =" & RutaF & ";</script>")
        '    Else
        '        'img_Frente = Nothing
        '    End If
        ''Perfil(derecho)
        'If Not IsDBNull(dr("Perfil_Derecho")) Then
        '    If File.Exists("c:\SIAC\Modulo_Reclutamiento_WEB\Imagenes\Derecho.jpg") Then File.Delete("c:\SIAC\Modulo_Reclutamiento_WEB\Imagenes\Derecho.jpg")
        '    Dim fs As New FileStream("c:\SIAC\Modulo_Reclutamiento_WEB\Imagenes\Derecho.jpg", FileMode.CreateNew, FileAccess.Write)
        '    Dim bw As New BinaryWriter(fs)
        '    bw.Write(dr("Perfil_Derecho"), 0, dr("Perfil_Derecho").Length)
        '    fs.Flush()
        '    fs.Close()
        '    Dim RutaF As String = "c:\SIAC\Modulo_Reclutamiento_WEB\Imagenes\" & (Session("EmpleadoID").ToString) & "Frente.jpg"
        '    Response.ContentType = "image/jpg"
        '    Context.Response.Write("<img src=""" & RutaF & """")
        'Else
        '    'pct_Derecho.Tag = 0
        'End If
        ''Perfil Izquierdo
        'If Not IsDBNull(dr("Perfil_Izquierdo")) Then
        '    If File.Exists("c:\SIAC\Modulo_Reclutamiento_WEB\Imagenes\Izquierdo.jpg") Then File.Delete("c:\SIAC\Modulo_Reclutamiento_WEB\Imagenes\Izquierdo.jpg")
        '    Dim fs As New FileStream("c:\SIAC\Modulo_Reclutamiento_WEB\Imagenes\Izquierdo.jpg", FileMode.CreateNew, FileAccess.Write)
        '    Dim bw As New BinaryWriter(fs)
        '    bw.Write(dr("Perfil_Izquierdo"), 0, dr("Perfil_Izquierdo").Length)
        '    fs.Flush()
        '    fs.Close()
        'Else
        '    'pct_Derecho.Tag = 0
        'End If
        ''Cuerpo Completo
        'If Not IsDBNull(dr("Cuerpo_Completo")) Then
        '    If File.Exists("c:\SIAC\Modulo_Reclutamiento_WEB\Imagenes\Completo.jpg") Then File.Delete("c:\SIAC\Modulo_Reclutamiento_WEB\Imagenes\Completo.jpg")
        '    Dim fs As New FileStream("c:\SIAC\Modulo_Reclutamiento_WEB\Imagenes\Completo.jpg", FileMode.CreateNew, FileAccess.Write)
        '    Dim bw As New BinaryWriter(fs)
        '    bw.Write(dr("Cuerpo_Completo"), 0, dr("Cuerpo_Completo").Length)
        '    fs.Flush()
        '    fs.Close()
        'Else
        '    'pct_Derecho.Tag = 0
        'End If
        'End If

    End Sub

    Protected Sub dgv_Empleos_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles dgv_Empleos.SelectedIndexChanging
        LimpiaDetalle()
    End Sub

    Protected Sub dgv_Empleos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Empleos.SelectedIndexChanged
        'If IsPostBack Then Exit Sub
        If dgv_Empleos.Rows(0).Cells(1).Text = "&nbsp;" Then Exit Sub

        LimpiaDetalle()

        dgv_Empleos.SelectedRowStyle.ForeColor = Drawing.Color.White
        dgv_Empleos.SelectedRowStyle.Font.Bold = True
        dgv_Empleos.SelectedRowStyle.BackColor = System.Drawing.Color.FromName("#C0A062")

        tbx_NombreEmpresaE.Text = dgv_Empleos.SelectedRow.Cells(1).Text
        tbx_CalleE.Text = dgv_Empleos.SelectedRow.Cells(2).Text
        If dgv_Empleos.SelectedRow.Cells(3).Text = "&nbsp;" Then
            tbx_EntreCalle1E.Text = ""
        Else
            tbx_EntreCalle1E.Text = dgv_Empleos.SelectedRow.Cells(3).Text
        End If
        If dgv_Empleos.SelectedRow.Cells(4).Text = "&nbsp;" Then
            tbx_EntreCalle2E.Text = ""
        Else
            tbx_EntreCalle2E.Text = dgv_Empleos.SelectedRow.Cells(4).Text
        End If
        tbx_NumeroExteriorE.Text = dgv_Empleos.SelectedRow.Cells(5).Text
        If dgv_Empleos.SelectedRow.Cells(6).Text = "&nbsp;" Then
            tbx_NumeroInteriorE.Text = ""
        Else
            tbx_NumeroInteriorE.Text = dgv_Empleos.SelectedRow.Cells(6).Text
        End If
        If dgv_Empleos.SelectedRow.Cells(7).Text = "&nbsp;" Then
            tbx_ColoniaE.Text = ""
        Else
            'Utilicé esta función para que apareciera la "ñ"
            tbx_ColoniaE.Text = HttpUtility.HtmlDecode(dgv_Empleos.SelectedRow.Cells(7).Text.ToString)
        End If
        tbx_CiudadE.Text = dgv_Empleos.SelectedRow.Cells(8).Text
        tbx_CPE.Text = dgv_Empleos.SelectedRow.Cells(9).Text
        tbx_TelefonoE.Text = dgv_Empleos.SelectedRow.Cells(17).Text
        tbx_FechaIngresoE.Text = dgv_Empleos.SelectedRow.Cells(12).Text
        tbx_FechaBajaE.Text = dgv_Empleos.SelectedRow.Cells(13).Text
        If dgv_Empleos.SelectedRow.Cells(22).Text = "SI" Then
            rdb_SiSeguridadE.Checked = True
            rdb_NoSeguridadE.Checked = False
        Else
            rdb_SiSeguridadE.Checked = False
            rdb_NoSeguridadE.Checked = True
        End If
        If dgv_Empleos.SelectedRow.Cells(23).Text = "SI" Then
            rdb_SiPorteArmas.Checked = True
            rdb_NoPorteArmas.Checked = False
        Else
            rdb_SiPorteArmas.Checked = False
            rdb_NoPorteArmas.Checked = True
        End If

        tbx_PuestoE.Text = dgv_Empleos.SelectedRow.Cells(14).Text
        tbx_NombreJefeE.Text = dgv_Empleos.SelectedRow.Cells(15).Text
        tbx_PuestoJefeE.Text = dgv_Empleos.SelectedRow.Cells(16).Text
        tbx_SueldoIE.Text = dgv_Empleos.SelectedRow.Cells(18).Text
        tbx_SueldoFE.Text = dgv_Empleos.SelectedRow.Cells(19).Text
        tbx_MotivoSeparacionE.Text = dgv_Empleos.SelectedRow.Cells(20).Text
        If dgv_Empleos.SelectedRow.Cells(21).Text = "&nbsp;" Then
            tbx_OtroMotivoE.Text = ""
        Else
            tbx_OtroMotivoE.Text = dgv_Empleos.SelectedRow.Cells(21).Text
        End If
        pnl_EmpleosDetalle.Visible = True

    End Sub

    Sub LimpiaDetalle()

        tbx_NombreEmpresaE.Text = ""
        tbx_CalleE.Text = ""
        tbx_EntreCalle1E.Text = ""
        tbx_EntreCalle2.Text = ""
        tbx_NumeroExteriorE.Text = ("")
        tbx_NumeroInteriorE.Text = ""
        tbx_ColoniaE.Text = ""
        tbx_CiudadE.Text = ""
        tbx_CPE.Text = ""
        tbx_TelefonoE.Text = ""
        tbx_FechaIngresoE.Text = ""
        tbx_FechaBajaE.Text = ""
        rdb_SiSeguridadE.Checked = False
        rdb_NoSeguridadE.Checked = False
        rdb_SiPorteArmas.Checked = False
        rdb_NoPorteArmas.Checked = False
        tbx_PuestoE.Text = ""
        tbx_NombreJefeE.Text = ""
        tbx_PuestoJefeE.Text = ""
        tbx_SueldoIE.Text = ""
        tbx_SueldoFE.Text = ""
        tbx_MotivoSeparacionE.Text = ""
        tbx_OtroMotivoE.Text = ""

    End Sub

    Sub LimpiarDatos()

        'Datos Generales
        tbx_NombreCompleto.Text = ""
        tbx_Clave.Text = ""
        tbx_Departamento.Text = ""
        tbx_Puesto.Text = ""
        tbx_EstadoCivil.Text = ""
        tbx_Ciudad.Text = ""
        tbx_Zona.Text = ""
        tbx_Mail.Text = ""
        rdb_Masculino.Checked = False
        rdb_Femenino.Checked = False
        rdb_SiCatFirmas.Checked = False
        rdb_NoCatFirmas.Checked = False
        tbx_Calle.Text = ""
        tbx_NumExterior.Text = ""
        tbx_NumInterior.Text = ""
        tbx_Colonia.Text = ""
        tbx_CP.Text = ""
        tbx_Telefono.Text = ""
        tbx_TelefonoMovil.Text = ""
        tbx_FechaNac.Text = ""
        tbx_LugarNac.Text = ""
        tbx_RFC.Text = ""
        tbx_CURP.Text = ""
        tbx_IMSS.Text = ""
        tbx_Elector.Text = ""
        tbx_TipoLicencia.Text = ""
        tbx_NumCartilla.Text = ""
        rdb_SiCertif.Checked = False
        rdb_NoCertif.Checked = False
        rdb_SiConFam.Checked = False
        rdb_NoConFam.Checked = False
        tbx_CantidadHijos.Text = ""
        tbx_Edad.Text = ""
        tbx_ApellidoPaterno.Text = ""
        tbx_ApellidoMaterno.Text = ""
        tbx_Nombres.Text = ""
        tbx_EmpleadoRef.Text = ""
        tbx_EntreCalle1.Text = ""
        tbx_EntreCalle2.Text = ""
        tbx_ModoNac.Text = ""
        tbx_PaisNac.Text = ""
        tbx_FechaNaturalizacion.Text = ""
        tbx_NumPasaporte.Text = ""
        rdb_SiJefe.Checked = False
        rdb_NoJefe.Checked = False
        tbx_UMF.Text = ""

        rdb_SiINFONAVIT.Checked = False
        rdb_NoINFONAVIT.Checked = False
        tbx_FechaVenceCred.Text = ""
        rdb_SiSaleRuta.Checked = False
        rdb_NoSaleRuta.Checked = False
        rdb_SiVerificaDepositos.Checked = False
        rdb_NoVerificaDepositos.Checked = False

        tbx_FechaIngreso.Text = ""

        'Rasgos
        tbx_Complexion.Text = ""
        tbx_PielColor.Text = ""
        tbx_Cara.Text = ""
        tbx_TipoSangre.Text = ""
        tbx_FactorRH.Text = ""
        tbx_Peso.Text = ""
        tbx_Estatura.Text = ""
        tbx_UsaAnteojos.Text = ""
        tbx_CabelloCantidad.Text = ""
        tbx_CabelloColor.Text = ""
        tbx_CabelloForma.Text = ""
        tbx_CabelloCalvicie.Text = ""
        tbx_CabelloImplantacion.Text = ""
        tbx_FrenteAltura.Text = ""
        tbx_FrenteInclinacion.Text = ""
        tbx_FrenteAncho.Text = ""
        tbx_CejasDireccion.Text = ""
        tbx_CejasImplantacion.Text = ""
        tbx_CejasForma.Text = ""
        tbx_CejasTamaño.Text = ""
        tbx_OjosColor.Text = ""
        tbx_OjosForma.Text = ""
        tbx_OjosTamaño.Text = ""
        tbx_NarizRaiz.Text = ""
        tbx_NarizDorso.Text = ""
        tbx_NarizAncho.Text = ""
        tbx_NarizBase.Text = ""
        tbx_NarizAltura.Text = ""
        tbx_BocaTamaño.Text = ""
        tbx_BocaComisuras.Text = ""
        tbx_LabiosEspesor.Text = ""
        tbx_LabiosAltura.Text = ""
        tbx_LabiosProminencia.Text = ""
        tbx_MentonTipo.Text = ""
        tbx_MentonForma.Text = ""
        tbx_MentonInclinacion.Text = ""
        tbx_OrejaForma.Text = ""
        tbx_OrejaOriginal.Text = ""
        tbx_HelixSuperior.Text = ""
        tbx_HelixPosterior.Text = ""
        tbx_HelixAdherencia.Text = ""
        tbx_HelixContorno.Text = ""
        tbx_LobuloAdherencia.Text = ""
        tbx_LobuloDimension.Text = ""
        tbx_LobuloParticularidad.Text = ""

        'Datos Escolares
        dgv_Cursos.DataSource = Nothing
        dgv_Cursos.DataBind()
        tbx_UltimosEstudios.Text = ""
        tbx_Documentacion.Text = ""
        tbx_NombreEscuela.Text = ""
        tbx_Carrera.Text = ""
        tbx_Especialidad.Text = ""
        tbx_AInicio.Text = ""
        tbx_ATermino.Text = ""
        tbx_Folio.Text = ""
        tbx_Promedio.Text = ""
        tbx_CedulaProfesional.Text = ""

        'Datos Familiares
        dgv_Familiares.DataSource = Nothing
        dgv_Familiares.DataBind()

        'Empleos
        dgv_Empleos.DataSource = Nothing
        dgv_Empleos.DataBind()

        'Referencias
        dgv_Referencias.DataSource = Nothing
        dgv_Referencias.DataBind()

        'Datos Varios
        tbx_IngresoFamiliar.Text = ""
        tbx_IngresoAdicional.Text = ""
        tbx_DescripcionAdicional.Text = ""
        tbx_GastoFamiliar.Text = ""
        tbx_TipoVivienda.Text = ""
        tbx_PagoMensual.Text = ""
        tbx_ValorVivienda.Text = ""
        rdb_SiVehiculo.Checked = False
        rdb_NoVehiculo.Checked = False
        tbx_Modelo.Text = ""
        tbx_ValorVehiculo.Text = ""

        tbx_Vicios.Text = ""
        tbx_Idiomas.Text = ""
        tbx_ActividadesCulturales.Text = ""
        tbx_Habilidades.Text = ""
        dgv_Señas.DataSource = Nothing
        dgv_Señas.DataBind()

        'Papelería Recibida
        chb_ActaNac.Checked = False
        chb_ActaMatrimonio.Checked = False
        chb_IMSS.Checked = False
        chb_CompDom.Checked = False
        chb_IFE.Checked = False
        chb_CompEstudios.Checked = False
        chb_Cartilla.Checked = False
        chb_Recomendacion.Checked = False
        chb_NoAntecedentes.Checked = False
        chb_Fotografias.Checked = False
        chb_CURP.Checked = False
        chb_Huellas.Checked = False

        'Fotos
        ICompleto.ImageUrl = Nothing
        IFrente.ImageUrl = Nothing
        IDerecho.ImageUrl = Nothing
        IIzquierdo.ImageUrl = Nothing
        IFirma.ImageUrl = Nothing

    End Sub

    Protected Sub dgv_Empleados_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgv_Empleados.PageIndexChanging
        LimpiarDatos()
        LimpiaDetalle()
        dgv_Empleados.SelectedRowStyle.BackColor = Drawing.Color.White
        dgv_Empleados.SelectedRowStyle.ForeColor = Drawing.Color.Black
        dgv_Empleados.SelectedRowStyle.Font.Bold = False

        dgv_Empleados.PageIndex = e.NewPageIndex
        If Session("Dpto_Reclutamiento") = Session("DepartamentoId") Then
            dgv_Empleados.DataSource = fn_Default_GetEmpleados(Session("SucursalID"), 0, "S", Session("UsuarioID"))
        Else
            dgv_Empleados.DataSource = fn_Default_GetEmpleados(Session("SucursalID"), Session("DepartamentoID"), "S", Session("UsuarioID"))
        End If
        dgv_Empleados.DataBind()
    End Sub

    Protected Sub btn_Aceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Aceptar.Click

        Validar()

        If Firma Then

            If Not fn_Empleados_ActualizarNuevoIngreso(Session("EmpleadoID"), Session("UsuarioID"), Session("EstacioN"), tbx_Comentarios.Text, Session("SucursalID"), Session("UsuarioID")) Then
                MostrarAlertAjax("Ha ocurrido un error al intentar guardar los datos.", btn_Aceptar, Page)
                Exit Sub
            Else
                MostrarAlertAjax("Los datos han sido guardados correctamente.", btn_Aceptar, Page)
            End If

            'Aquí se inserta la Alerta de Validación de Nuevo Ingreso

            Dim Detalles As String = "           Nombre: " & tbx_Clave.Text & " - " & tbx_NombreCompleto.Text & Chr(13) _
                                    & "    Departamento: " & tbx_Departamento.Text & Chr(13) _
                                    & "          Puesto: " & tbx_Puesto.Text & Chr(13) _
                                    & "   Fecha Ingreso: " & tbx_FechaIngreso.Text & Chr(13) _
                                    & "Fecha Validación: " & Now.ToShortDateString & " - " & Now.ToShortTimeString & Chr(13) _
                                    & "    Validado por: " & Session("UsuarioNombre")
            '& "Agente de Servicios SIAC " & Today.Year.ToString

            Dim DetalleHTML As String = "<html><body><table style='border: solid 1px'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                                        & "<tr><td colspan='4' align='center'>VALIDACION DE EMPLEADO DE NUEVO INGRESO</td></tr>" _
                                        & "<tr><td colspan='4'><br><hr /></td></tr>" _
                                        & "<tr><td align='right'><label><b>Nombre:</b></label></td><td>" & tbx_Clave.Text & " - " & tbx_NombreCompleto.Text & "</td><td></td><td></td></tr>" _
                                        & "<tr><td align='right'><label><b>Departamento:</b></label></td><td>" & tbx_Departamento.Text & "</td></tr>" _
                                        & "<tr><td align='right'><label><b>Puesto:</b></label></td><td>" & tbx_Puesto.Text & "</td></tr>" _
                                        & "<tr><td align='right'><label><b>Fecha Ingreso:</b></label></td><td>" & tbx_FechaIngreso.Text & "</td></tr>" _
                                        & "<tr><td align='right'><label><b>Fecha Validación:</b></label></td><td>" & Now.ToShortDateString & " - " & Now.ToShortTimeString & "<br></td></tr>" _
                                        & "<tr><td align='right'><label><b>Validado por:</b></label></td><td>" & Session("UsuarioNombre") & "</td></tr>" _
                                        & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>Pie</td></tr></table></body></html>"

            Dim Pie As String = "Agente de Servicios SIAC " & Today.Year.ToString
            DetalleHTML = Replace(DetalleHTML, "Pie", Pie)

            'Aquí se guarda la Alerta y se envian los correos

            If fn_AlertasGeneradas_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), "21", Detalles) Then
                'Obtener los Destinos
                Dim Dt_Destinos As DataTable = fn_AlertasGeneradas_ObtenerMails("21")
                If Dt_Destinos IsNot Nothing Then
                    For Each renglon As DataRow In Dt_Destinos.Rows
                        Cn_Mail.fn_Enviar_MailHTML(renglon("Mail"), "VALIDACION DE NUEVO INGRESO", DetalleHTML, "", Session("SucursalID"))
                        'Cn_Mail.fn_Enviar_MailHTML("jose.nuncio@sissaseguridad.com", "VALIDACION DE NUEVO INGRESO", DetalleHTML, "", Session("SucursalID"))
                    Next
                End If
            End If

            CerrarFirma()
            LimpiarDatos()

            'Actualizar el GridView de Empleados al Validar el Nuevo Ingreso
            Dim dt As DataTable = fn_Default_GetEmpleados(Session("SucursalID"), Session("DepartamentoID"), "S", Session("UsuarioID"))

            If dt.Rows.Count > 0 Then
                dgv_Empleados.DataSource = dt
                dgv_Empleados.DataBind()
            Else
                dgv_Empleados.DataSource = fn_CreaGridVacio("Id_Empleado,Clave,Nombre,Departamento,Puesto")
                dgv_Empleados.DataBind()
            End If
        Else
            'dgv_Empleados.SelectedRowStyle.ForeColor = Drawing.Color.White
            'dgv_Empleados.SelectedRowStyle.Font.Bold = True
            'dgv_Empleados.SelectedRowStyle.BackColor = System.Drawing.Color.FromName("#C0A062")
            'tbx_Contrasena.Focus()
        End If

    End Sub

    Sub Validar()

        Firma = False

        If tbx_Comentarios.Text = "" Then
            MostrarAlertAjax("Capture los Comentarios.", btn_Aceptar, Page)
            tbx_Comentarios.Focus()
            Exit Sub
        ElseIf Len(tbx_Comentarios.Text) < 10 Then
            MostrarAlertAjax("Los Comentarios deben ser mas descriptivos.", btn_Aceptar, Page)
            tbx_Comentarios.Focus()
            Exit Sub
        End If

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
        If Not FuncionesGlobales.fn_Valida_Cadena(Contra, FuncionesGlobales.Validar_Cadena.LetrasNumerosYCar) Then
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

                If PasswordUsr = PasswordDb Then
                    'FormsAuthentication.RedirectFromLoginPage(Usuario, False)
                    'MsgBox("Solo los usuarios tipo 2 pueden entrar a esta aplicación", Response)
                    Firma = True
                Else
                    'MsgBox("Usuario o Contraseña incorrecta", Response)
                    MostrarAlertAjax("Usuario o Contraseña incorrecta.", btn_Aceptar, Page)
                    Exit Sub
                End If

                If tbl.Rows(0)("Dias_Expira") < 1 Then
                    'MsgBox("La Contraseña está expirada.", Response)
                    MostrarAlertAjax("La Contraseña está expirada.", btn_Aceptar, Page)
                    Exit Sub
                End If

                If tbl.Rows(0)("Status") <> "A" Then
                    'MsgBox("Usuario Bloqueado. Imposible Entrar al SIAC Intranet. Consulte al Administrador.", Response)
                    MostrarAlertAjax("Usuario Bloqueado. Imposible Entrar al SIAC Intranet. Consulte al Administrador.", btn_Aceptar, Page)
                    Exit Sub
                End If

            Else
                MostrarAlertAjax("Usuario no encontrado.", btn_Aceptar, Page)
                Exit Sub
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub

    Sub CerrarFirma()

        tbx_Contrasena.Text = ""
        tbx_Comentarios.Text = ""
        btn_Aceptar.Enabled = False

    End Sub

    Protected Sub dgv_Empleados_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgv_Empleados.RowCreated

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

End Class


