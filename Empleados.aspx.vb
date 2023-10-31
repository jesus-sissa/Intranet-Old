Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.Cn_Login
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data

Partial Public Class Empleados
    Inherits BasePage

    Dim Vistas As Integer = 0
    Dim Firma As Boolean = False
    Dim Veces As Integer = 0
    Dim Tipo As Char
    Dim RutaF As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If IsPostBack Then Exit Sub


        fn_Log_Create(Session("UsuarioID"), Session("ModuloClave"), "ABRIR PAGINA: CONSULTA DE EMPLEADOS", Session("EstacioN"), Session("EstacionIP"), "", Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

        Dim dt As DataTable = fn_Default_GetEmpleados(Session("SucursalID"), Session("UsuarioID"), "N")

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            dgv_Empleados.DataSource = dt
        Else
            dgv_Empleados.DataSource = fn_CreaGridVacio("Id_Empleado,Clave,Nombre,Departamento,Puesto")
        End If
        dgv_Empleados.DataBind()

        '-------Cuenta columnas y alinea contenido y encabezado
        Dim Columnas As Byte
        Dim Gv_Nombres() As GridView = {dgv_Empleados, dgv_Cursos, dgv_Familiares, dgv_Empleos, dgv_Referencias, dgv_Señas}

        For j As Byte = 0 To Gv_Nombres.Length - 1
            Columnas = Gv_Nombres(j).Columns.Count

            For i As Byte = 0 To Columnas - 1
                Gv_Nombres(j).Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
            Next
        Next

    End Sub

    Sub MuestraGridVacios()

        gv_DocumentosRequeridos.DataSource = fn_CreaGridVacio("Id_DoctoR,DoctoRecivido,Descripcion")
        gv_DocumentosRequeridos.DataBind()

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
        tbc_Datos.Visible = True '26/11/2012
        Call LimpiaDetalle()
        Call LimpiarDatos()

        Dim resultado As Integer
        Try
            Integer.TryParse(dgv_Empleados.SelectedDataKey.Values("Id_Empleado"), resultado)

            If resultado = 0 Then
                dgv_Empleados.SelectedIndex = -1
                tbc_Datos.Visible = False
                Exit Sub
            End If
        Catch
            dgv_Empleados.SelectedIndex = -1
            Exit Sub
        End Try
        Session("EmpleadoID") = resultado
        tbc_Datos.ActiveTabIndex = 0
        pnl_EmpleosDetalle.Visible = True
        Call LlenarDG()
        Call LlenarMasDatos()

        ICompleto.ImageUrl = "~/Mostar_Fotos.aspx?Id=" & dgv_Empleados.SelectedDataKey.Value & "&Foto='Cuerpo_Completo'&Red=True"
        IFrente.ImageUrl = "~/Mostar_Fotos.aspx?Id=" & dgv_Empleados.SelectedDataKey.Value & "&Foto='Frente'"
        IDerecho.ImageUrl = "~/Mostar_Fotos.aspx?Id=" & dgv_Empleados.SelectedDataKey.Value & "&Foto='Perfil_Derecho'"
        IIzquierdo.ImageUrl = "~/Mostar_Fotos.aspx?Id=" & dgv_Empleados.SelectedDataKey.Value & "&Foto='Perfil_Izquierdo'"
        IFirma.ImageUrl = "~/Mostar_Fotos.aspx?Id=" & dgv_Empleados.SelectedDataKey.Value & "&Foto='Firma'"
    End Sub

    Sub LlenarDG()
        'Aqui se obtienen los DATOS GENERALES del Empleado seleccionado
        Dim Dr_Datos As DataRow = fn_Empleados_ObtenValores(Session("EmpleadoID"))

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
            tbx_FechaIngreso.Text = CDate(Dr_Datos("FechaIngreso")).ToShortDateString
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

        If Dr_DatosEscolares IsNot Nothing Then
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
        If dt_Cursos IsNot Nothing AndAlso dt_Cursos.Rows.Count > 0 Then
            dgv_Cursos.DataSource = dt_Cursos 'fn_CursosRecibidos_ObtenValores(Session("EmpleadoID"))
        Else
            dgv_Cursos.DataSource = fn_CreaGridVacio("Curso,Finalizado,FechaInicio,FechaFin,Instructor,TipoDocumento,Comentarios")
        End If
        dgv_Cursos.DataBind()

        'Aqui se llena el listview de DATOS FAMILIARES con los datos del empleado seleccionado
        Dim dt_Fam As DataTable = fn_DatosFamiliares_ObtenValores(Session("EmpleadoID"), Session("SucursalID"), Session("UsuarioID"))
        If dt_Fam IsNot Nothing AndAlso dt_Fam.Rows.Count > 0 Then
            dgv_Familiares.DataSource = dt_Fam
        Else
            dgv_Familiares.DataSource = fn_CreaGridVacio("Nombre,Parentesco,FechaNacimiento,Direccion,Ciudad,Telefono,Vive,MismoDomicilio")
        End If
        dgv_Familiares.DataBind()

        'Aquí se llena el listview de EMPLEOS con los datos del empleado seleccionado
        Dim dt_Empleos As DataTable = fn_Empleos_ObtenValores(Session("EmpleadoID"), Session("SucursalID"), Session("UsuarioID"))
        If dt_Empleos IsNot Nothing AndAlso dt_Empleos.Rows.Count > 0 Then
            dgv_Empleos.DataSource = dt_Empleos
        Else
            dgv_Empleos.DataSource = fn_CreaGridVacio("NombreEmpresa,Calle,EntreCalle1,EntreCalle2,NumeroExt,NumeroInt,Colonia,Ciudad,CodigoPostal,Latitud,Longitud,FechaIngreso,FechaBaja,Puesto,NombreJefe,PuestoJefe,Telefono,SueldoIni,SueldoFin,MotivoBaja,Otro,EmpresaSeg,PorteArmas")
        End If
        dgv_Empleos.DataBind()

        'Aquí se llena el listview de REFERENCIAS con los datos del empleado seleccionado
        Dim dt_Ref As DataTable = fn_Referencias_ObtenValores(Session("EmpleadoID"), Session("SucursalID"), Session("UsuarioID"))
        If dt_Ref IsNot Nothing AndAlso dt_Ref.Rows.Count > 0 Then
            dgv_Referencias.DataSource = dt_Ref
        Else
            dgv_Referencias.DataSource = fn_CreaGridVacio("Descripcion,Nombre,Sexo,Ocupacion,Domicilio,EntreCalle1,EntreCalle2,NumeroExt,NumeroInt,Colonia,Ciudad,CodigoPostal,Telefono,Status")
        End If
        dgv_Referencias.DataBind()

        'Aquí se obtienen los DATOS VARIOS del empleado seleccionado
        'Los datos de ingresos
        Dim Dr_DatosIngresos As DataRow = fn_EmpleadosIngresos_ObtenValores(Session("EmpleadoID"), Session("SucursalID"), Session("UsuarioID"))
        If Dr_DatosIngresos IsNot Nothing Then
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
        If dt_Senas IsNot Nothing AndAlso dt_Senas.Rows.Count > 0 Then
            dgv_Señas.DataSource = dt_Senas
        Else
            dgv_Señas.DataSource = fn_CreaGridVacio("Descripcion,Forma,Ubicacion,Comentarios,Cantidad")
        End If
        dgv_Señas.DataBind()

        'Aquí se obtienen los datos de la PAPELERIA RECIBIDA del Empleado seleccionado
        Dim DT_Papeleria As DataTable = fn_DocumentosRequeridos_ObtenValores(Session("EmpleadoID"))

        'Aquí se chequean los Tipos de Documentos Requeridos
        'si se encuentra el registro correspondiente para ese documento se marca

        If DT_Papeleria IsNot Nothing AndAlso DT_Papeleria.Rows.Count > 0 Then
            gv_DocumentosRequeridos.DataSource = DT_Papeleria
            gv_DocumentosRequeridos.DataBind()

            For Each rowPrivilegio As GridViewRow In gv_DocumentosRequeridos.Rows

                Dim DoctoEntregado As String = gv_DocumentosRequeridos.DataKeys(rowPrivilegio.RowIndex).Values("DoctoRecivido")
                Dim cb As CheckBox = CType(rowPrivilegio.FindControl("chk_DoctoRequerido"), CheckBox)
                If DoctoEntregado = "SI" Then
                    cb.Checked = True
                End If
                cb.Enabled = False
            Next
        Else
            gv_DocumentosRequeridos.DataSource = fn_CreaGridVacio("Id_DoctoR,DoctoRecivido,Descripcion")
            gv_DocumentosRequeridos.DataBind()
        End If
    End Sub

    Protected Sub dgv_Empleos_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles dgv_Empleos.SelectedIndexChanging
        Call LimpiaDetalle()
    End Sub

    Protected Sub dgv_Empleos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgv_Empleos.SelectedIndexChanged
        If dgv_Empleos.Rows(0).Cells(1).Text = "&nbsp;" Then Exit Sub

        Call LimpiaDetalle()
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

    Private Sub LimpiaDetalle()
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

    Private Sub LimpiarDatos()
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

        'Fotos
        ICompleto.ImageUrl = Nothing
        IFrente.ImageUrl = Nothing
        IDerecho.ImageUrl = Nothing
        IIzquierdo.ImageUrl = Nothing
        IFirma.ImageUrl = Nothing
    End Sub

    Protected Sub dgv_Empleados_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles dgv_Empleados.PageIndexChanging

        Call LimpiarDatos()
        Call LimpiaDetalle()
        tbc_Datos.Visible = False

        Session("EmpleadoID") = 0
        dgv_Empleados.SelectedIndex = -1
        dgv_Empleados.PageIndex = e.NewPageIndex
        dgv_Empleados.DataSource = fn_Default_GetEmpleados(Session("SucursalID"), Session("UsuarioID"), "N")
        dgv_Empleados.DataBind()
    End Sub
End Class
