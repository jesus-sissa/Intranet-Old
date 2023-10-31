<%@ Control Language="VB" AutoEventWireup="false" Inherits="SISSAIntranet.wuc_Empleados"
    CodeBehind="wuc_Empleados.ascx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="../Estilos/StyleSheet.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title></title>
    <style type="text/css">
        #btn_Cerrar
        {
            width: 71px;
            margin-left: 0px;
        }
        #form1
        {
            height: 819px;
            margin-bottom: 0px;
        }
        #iDerecho
        {
            height: 183px;
            width: 212px;
        }
    </style>
</head>

<body>
    <%--<br />
    <div id="divEnca" style="width:100%">
        <strong>Consulta de Empleados
        </strong>
    </div>--%>
    <div id="div1">
        <asp:GridView ID="dgv_Empleados" runat="server" AutoGenerateColumns="False" CssClass="gridSISSA"
            DataKeyNames="Id_Empleado" GridLines="Horizontal" Width="100%" AllowPaging="True">
            <RowStyle BackColor="White" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select"
                            ImageUrl="~/Imagenes/1rightarrow.png" Text="Seleccionar" />
                    </ItemTemplate>
                    <ItemStyle Width="15px" />
                </asp:TemplateField>
                <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                <asp:BoundField DataField="Departamento" HeaderText="Departamento" SortExpression="Departamento" />
                <asp:BoundField DataField="Puesto" HeaderText="Puesto" SortExpression="Puesto" />
            </Columns>
            <FooterStyle BackColor="White" ForeColor="Black" />
            <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#C0A062" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
        </asp:GridView>
    </div>
    <div id="div3">
        <br />
        <ajax:TabContainer runat="server" ID="tbc_Datos" ActiveTabIndex="0" Width="100%"
            Height="620px" Style="margin-top: 12px">
            <ajax:TabPanel ID="tab_Generales" runat="server" HeaderText="Datos Generales" Height="150px"
                Font-Size="XX-Small">
                <HeaderTemplate>
                    Datos Generales
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel ID="pnl_DatosG" runat="server" Height="656px">
                        <table class="tablaSISSA1">
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 140px">
                                    Clave
                                    <br />
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_Clave" runat="server" ReadOnly="True" Style="margin-left: 0px;
                                        text-align: right" Width="50px" CssClass="textbox1"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td style="text-align: right; width: 200px">
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Nombre Completo
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="tbx_NombreCompleto" runat="server" Width="250px" ReadOnly="True"
                                        CssClass="textbox1" />
                                </td>
                                <td style="text-align: right">
                                    Correo Electrónico
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_Mail" runat="server" Width="250px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Nombre(s)
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_Nombres" runat="server" ReadOnly="True" Width="210px" CssClass="textbox1" />
                                </td>
                                <td>
                                </td>
                                <td style="text-align: right">
                                    Teléfono
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_Telefono" runat="server" Width="110px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Apellido Paterno
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_ApellidoPaterno" runat="server" Width="210px" ReadOnly="True"
                                        CssClass="textbox1" />
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td style="text-align: right">
                                    Teléfono Móvil
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_TelefonoMovil" runat="server" Width="110px" ReadOnly="True"
                                        CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Apellido Materno
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_ApellidoMaterno" runat="server" Width="210px" ReadOnly="True"
                                        CssClass="textbox1" />
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td style="text-align: right">
                                    Clave Elector
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_Elector" runat="server" Width="210px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Departamento
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_Departamento" runat="server" Width="210px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td style="text-align: right">
                                    RFC
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_RFC" runat="server" Width="210px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Puesto
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_Puesto" runat="server" Width="210px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td style="text-align: right">
                                    CURP
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_CURP" runat="server" Width="210px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Calle
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_Calle" runat="server" Width="210px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td style="text-align: right">
                                    IMSS
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_IMSS" runat="server" Width="110px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Entre la Calle
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_EntreCalle1" runat="server" Width="210px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td style="text-align: right">
                                    UMF
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_UMF" runat="server" Width="110px" ReadOnly="True" Style="text-align: right"
                                        CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Y la Calle
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_EntreCalle2" runat="server" Width="209px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td style="text-align: right">
                                    Num. Cartilla
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_NumCartilla" runat="server" Width="110px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Número Exterior
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="tbx_NumExterior" runat="server" ReadOnly="True" Style="text-align: right"
                                        Width="50px" CssClass="textbox1" />
                                </td>
                                <td style="text-align: right">
                                    Número Int.
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_NumInterior" runat="server" ReadOnly="True" Style="text-align: right"
                                        Width="50px" CssClass="textbox1" />
                                </td>
                                <td style="text-align: right">
                                    Pasaporte
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_NumPasaporte" runat="server" ReadOnly="True" Width="110px" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Colonia
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_Colonia" runat="server" Width="210px" CssClass="textbox1" />
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td style="text-align: right">
                                    Tipo Licencia
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_TipoLicencia" runat="server" Width="210px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Ciudad
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_Ciudad" runat="server" Width="210px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td style="text-align: right">
                                    Fecha Expira Licencia
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_FechaExpira" runat="server" Width="110px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Zona
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_Zona" runat="server" Width="210px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                                <td style="text-align: right">
                                    C.P.
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_CP" runat="server" ReadOnly="True" Style="text-align: right"
                                        Width="50px" CssClass="textbox1" />
                                </td>
                                <td style="text-align: right">
                                    Crédito INFONAVIT
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_SiINFONAVIT" runat="server" Text="Si" />
                                    &nbsp;&nbsp;<asp:RadioButton ID="rdb_NoINFONAVIT" runat="server" Text="No" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Género
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_Masculino" runat="server" Text="Masculino" />
                                    &nbsp;&nbsp;<asp:RadioButton ID="rdb_Femenino" runat="server" Text="Femenino" />
                                </td>
                                <td style="text-align: right">
                                    &nbsp;&nbsp;
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td style="text-align: right">
                                    En Catálogo Firmas
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_SiCatFirmas" runat="server" Text="Si" />
                                    &nbsp;&nbsp;<asp:RadioButton ID="rdb_NoCatFirmas" runat="server" Text="No" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Vive con su Familia
                                </td>
                                <td colspan="1">
                                    <asp:RadioButton ID="rdb_SiConFam" runat="server" Text="Si" />
                                    &nbsp;&nbsp;<asp:RadioButton ID="rdb_NoConFam" runat="server" Text="No" />
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td>
                                </td>
                                <td style="text-align: right">
                                    Certificación Academia
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_SiCertif" runat="server" Text="Si" />
                                    &nbsp;&nbsp;<asp:RadioButton ID="rdb_NoCertif" runat="server" Text="No" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Estado Civil
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="tbx_EstadoCivil" runat="server" ReadOnly="True" Width="110px" CssClass="textbox1"></asp:TextBox>
                                </td>
                                <td style="text-align: right">
                                    Cantidad Hijos
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_CantidadHijos" runat="server" Width="50px" ReadOnly="True" Style="text-align: right"
                                        CssClass="textbox1" />
                                </td>
                                <td style="text-align: right">
                                    Fecha Ingreso
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_FechaIngreso" runat="server" Width="110px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Modo Nacionalidad
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="tbx_ModoNac" runat="server" ReadOnly="True" Width="110px" CssClass="textbox1"></asp:TextBox>
                                </td>
                                <td style="text-align: right">
                                    Edad
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_Edad" runat="server" Width="50px" ReadOnly="True" Style="text-align: right"
                                        CssClass="textbox1" />
                                </td>
                                <td style="text-align: right">
                                    Fecha Vence Credencial
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_FechaVenceCred" runat="server" Width="110px" ReadOnly="True"
                                        CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Lugar Nacimiento
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_LugarNac" runat="server" ReadOnly="True" Width="210px" CssClass="textbox1"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td style="text-align: right">
                                    Empleado Referencia
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_EmpleadoRef" runat="server" Width="250px" ReadOnly="True" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    País Nacimiento
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_PaisNac" runat="server" ReadOnly="True" Width="210px" CssClass="textbox1"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td style="text-align: right">
                                    Es Jefe de Area
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_SiJefe" runat="server" Text="Si" />&nbsp;&nbsp;<asp:RadioButton
                                        ID="rdb_NoJefe" runat="server" Text="No" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Fecha Nacimiento
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_FechaNac" runat="server" ReadOnly="True" Width="110px" CssClass="textbox1"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td style="text-align: right">
                                    Sale a Ruta
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_SiSaleRuta" runat="server" Text="Si" />&nbsp;&nbsp;<asp:RadioButton
                                        ID="rdb_NoSaleRuta" runat="server" Text="No" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; width: 150px">
                                    Fecha Naturalización
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_FechaNaturalizacion" runat="server" ReadOnly="True" Width="110px"
                                        CssClass="textbox1"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                                <td style="text-align: right">
                                    Verificará Depósitos
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_SiVerificaDepositos" runat="server" Text="Si" />&nbsp;&nbsp;<asp:RadioButton
                                        ID="rdb_NoVerificaDepositos" runat="server" Text="No" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </ajax:TabPanel>
            <ajax:TabPanel ID="tab_Rasgos" runat="server" HeaderText="Rasgos">
                <HeaderTemplate>
                    Rasgos
                </HeaderTemplate>
                <ContentTemplate>
                    <br />
                    <table class="tablaSISSA1" style="border: 1px solid #000000; width: auto;">
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                Complexión
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_Complexion" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Tipo Sangre
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_TipoSangre" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Usa Anteojos
                            </td>
                            <td style="width: 100px;">
                                <asp:TextBox ID="tbx_UsaAnteojos" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px" Width="50px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 138px;">
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                Color Piel
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_PielColor" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Factor RH
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_FactorRH" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Peso
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_Peso" runat="server" CssClass="textbox1" ReadOnly="True" Style="margin-left: 10px"
                                    Width="50px"></asp:TextBox>
                                &nbsp;kg
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                Cara
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_Cara" runat="server" CssClass="textbox1" ReadOnly="True" Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Estatura
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_Estatura" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px" Width="50px"></asp:TextBox>
                                &nbsp; m
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table style="border: 1px solid #000000; width: auto;" class="tablaSISSA1">
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                <b>Cabello</b>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                <b>Frente</b>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                <b>Cejas</b>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 138px;">
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                Cantidad
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_CabelloCantidad" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Altura
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_FrenteAltura" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Dirección
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_CejasDireccion" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                Color
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_CabelloColor" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Inclinación
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_FrenteInclinacion" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Implantación
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_CejasImplantacion" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                Forma
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_CabelloForma" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Ancho
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_FrenteAncho" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Forma
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_CejasForma" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                Calvicie
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_CabelloCalvicie" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Tamaño
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_CejasTamaño" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                Implantación
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_CabelloImplantacion" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table class="tablaSISSA1" style="border: 1px solid #000000; width: auto;">
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                <b>Ojos</b>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                <b>Nariz</b>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                <b>Boca</b>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                Color
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_OjosColor" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Raíz
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_NarizRaiz" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Tamaño
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_BocaTamaño" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                Forma
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_OjosForma" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Dorso
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_NarizDorso" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Comisuras
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_BocaComisuras" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                Tamaño
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_OjosTamaño" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Ancho
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_NarizAncho" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                <b>Labios</b>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Base
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_NarizBase" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="text-align: right;">
                                Espesor
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_LabiosEspesor" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Altura
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_NarizAltura" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Altura Naso-Labial
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_LabiosAltura" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="text-align: right;">
                                Prominencia
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_LabiosProminencia" runat="server" CssClass="textbox1" ReadOnly="True"
                                    Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table class="tablaSISSA1" style="border: 1px solid #000000; width: auto;">
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                <b>Mentón</b>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                <b>Oreja Derecha</b>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                <b>Hélix</b>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 20px;">
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                Tipo
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_MentonTipo" runat="server" CssClass="textbox1" Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Forma
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_OrejaForma" runat="server" CssClass="textbox1" Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Superior
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_HelixSuperior" runat="server" CssClass="textbox1" Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                Forma
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_MentonForma" runat="server" CssClass="textbox1" Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Original
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_OrejaOriginal" runat="server" CssClass="textbox1" Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Posterior
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_HelixPosterior" runat="server" CssClass="textbox1" Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">
                                Inclinación
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_MentonInclinacion" runat="server" CssClass="textbox1" Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="text-align: right;">
                                <b>Lóbulo</b>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Adherencia
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_HelixAdherencia" runat="server" CssClass="textbox1" Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="width: 100px; text-align: right;">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td style="text-align: right;">
                                Adherencia
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_LobuloAdherencia" runat="server" CssClass="textbox1" Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td style="width: 100px; text-align: right;">
                                Contorno
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_HelixContorno" runat="server" CssClass="textbox1" Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="text-align: right;">
                                Particularidad
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_LobuloParticularidad" runat="server" CssClass="textbox1" Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td style="text-align: right;">
                                Dimensión
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_LobuloDimension" runat="server" CssClass="textbox1" Style="margin-left: 10px"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <br />
                </ContentTemplate>
            </ajax:TabPanel>
            <ajax:TabPanel ID="tab_Escolares" runat="server" HeaderText="Datos Escolares">
                <HeaderTemplate>
                    Datos Escolares
                </HeaderTemplate>
                <ContentTemplate>
                    <table class="tablaSISSA1" style="width: auto;">
                        <tr>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp; &nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Ultimo Grado Estudios
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="tbx_UltimosEstudios" runat="server" Style="margin-left: 0px;" Width="210px"
                                    CssClass="textbox1" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Documentación Recibida
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="tbx_Documentacion" runat="server" Style="margin-left: 0px" Width="210px"
                                    CssClass="textbox1" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Nombre Completo Escuela
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="tbx_NombreEscuela" runat="server" Style="margin-left: 0px" Width="350px"
                                    CssClass="textbox1" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Carrera
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="tbx_Carrera" runat="server" Style="margin-left: 0px" Width="350px"
                                    CssClass="textbox1" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Cédula Profesional
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="tbx_CedulaProfesional" runat="server" Style="margin-left: 0px" Width="210px"
                                    CssClass="textbox1" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Especialidad
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="tbx_Especialidad" runat="server" Style="margin-left: 0px" Width="210px"
                                    CssClass="textbox1" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Año Inicio
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_AInicio" runat="server" ReadOnly="True" Style="margin-left: 0px;
                                    text-align: right" Width="90px" CssClass="textbox1"></asp:TextBox>
                            </td>
                            <td style="text-align: right">
                                Año Terminó
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_ATermino" runat="server" ReadOnly="True" Style="margin-left: 0px;
                                    text-align: right" Width="90px" CssClass="textbox1"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Folio
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_Folio" runat="server" ReadOnly="True" Style="margin-left: 0px;
                                    text-align: right" Width="90px" CssClass="textbox1"></asp:TextBox>
                            </td>
                            <td style="text-align: right">
                                Promedio
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_Promedio" runat="server" ReadOnly="True" Style="margin-left: 0px;
                                    text-align: right" Width="90px" CssClass="textbox1"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="dgv_Cursos" runat="server" AutoGenerateColumns="False" CssClass="gridSISSA">
                        <Columns>
                            <asp:BoundField DataField="Curso" HeaderText="Curso" SortExpression="Curso" />
                            <asp:BoundField DataField="Finalizado" HeaderText="Finalizado" SortExpression="Finalizado" />
                            <asp:BoundField DataField="FechaInicio" HeaderText="FechaInicio" SortExpression="FechaInicio" />
                            <asp:BoundField DataField="FechaFin" HeaderText="FechaFin" SortExpression="FechaFin" />
                            <asp:BoundField DataField="Instructor" HeaderText="Instructor" SortExpression="Instructor" />
                            <asp:BoundField DataField="TipoDocumento" HeaderText="TipoDocumento" SortExpression="TipoDocumento" />
                            <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" SortExpression="Comentarios" />
                        </Columns>
                        <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                        <SelectedRowStyle BackColor="#C0A062" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                </ContentTemplate>
            </ajax:TabPanel>
            <ajax:TabPanel ID="tab_Familiares" runat="server" HeaderText="Datos Familiares">
                <HeaderTemplate>
                    Datos Familiares
                </HeaderTemplate>
                <ContentTemplate>
                    <br />
                    <asp:GridView ID="dgv_Familiares" runat="server" AutoGenerateColumns="False" CssClass="gridSISSA"
                        Width="100%">
                        <Columns>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                            <asp:BoundField DataField="Parentesco" HeaderText="Parentesco" SortExpression="Parentesco" />
                            <asp:BoundField DataField="FechaNacimiento" HeaderText="FechaNacimiento" SortExpression="FechaNacimiento" />
                            <asp:BoundField DataField="Direccion" HeaderText="Direccion" SortExpression="Direccion" />
                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" SortExpression="Ciudad" />
                            <asp:BoundField DataField="Telefono" HeaderText="Telefono" SortExpression="Telefono" />
                            <asp:BoundField DataField="Vive" HeaderText="Vive" SortExpression="Vive" />
                            <asp:BoundField DataField="MismoDomicilio" HeaderText="MismoDomicilio" SortExpression="MismoDomicilio" />
                        </Columns>
                        <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                    </asp:GridView>
                </ContentTemplate>
            </ajax:TabPanel>
            <ajax:TabPanel ID="tab_Empleos" runat="server" HeaderText="Empleos">
                <HeaderTemplate>
                    Empleos
                </HeaderTemplate>
                <ContentTemplate>
                    <br />
                    <asp:GridView ID="dgv_Empleos" runat="server" AutoGenerateColumns="False" CssClass="gridSISSA"
                        Width="100%">
                        <Columns>
                            <asp:CommandField SelectImageUrl="~/Imagenes/handtoright.png" ButtonType="Image"
                                ShowSelectButton="True" />
                            <asp:BoundField DataField="NombreEmpresa" HeaderText="NombreEmpresa" SortExpression="NombreEmpresa"
                                HtmlEncode="False" />
                            <asp:BoundField DataField="Calle" HeaderText="Calle" SortExpression="Calle" HtmlEncode="False" />
                            <asp:BoundField DataField="EntreCalle1" HeaderText="EntreCalle1" SortExpression="EntreCalle1"
                                HtmlEncode="False" />
                            <asp:BoundField DataField="EntreCalle2" HeaderText="EntreCalle2" SortExpression="EntreCalle2"
                                HtmlEncode="False">
                                <FooterStyle CssClass="hiddenCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NumeroExt" HeaderText="NumeroExt" SortExpression="NumeroExt" />
                            <asp:BoundField DataField="NumeroInt" HeaderText="NumeroInt" SortExpression="NumeroInt" />
                            <asp:BoundField DataField="Colonia" HeaderText="Colonia" SortExpression="Colonia" />
                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" SortExpression="Ciudad" HtmlEncode="False" />
                            <asp:BoundField DataField="CodigoPostal" HeaderText="CodigoPostal" SortExpression="CodigoPostal">
                                <ControlStyle CssClass="hiddenCol" />
                                <FooterStyle CssClass="hiddenCol" />
                                <HeaderStyle CssClass="hiddenCol" />
                                <ItemStyle CssClass="hiddenCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Latitud" HeaderText="Latitud" SortExpression="Latitud">
                                <ControlStyle CssClass="hiddenCol" />
                                <FooterStyle CssClass="hiddenCol" />
                                <HeaderStyle CssClass="hiddenCol" />
                                <ItemStyle CssClass="hiddenCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Longitud" HeaderText="Longitud" SortExpression="Longitud">
                                <ControlStyle CssClass="hiddenCol" />
                                <FooterStyle CssClass="hiddenCol" />
                                <HeaderStyle CssClass="hiddenCol" />
                                <ItemStyle CssClass="hiddenCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaIngreso" HeaderText="FechaIngreso" SortExpression="FechaIngreso">
                                <ControlStyle CssClass="hiddenCol" />
                                <FooterStyle CssClass="hiddenCol" />
                                <HeaderStyle CssClass="hiddenCol" />
                                <ItemStyle CssClass="hiddenCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaBaja" HeaderText="FechaBaja" SortExpression="FechaBaja">
                                <ControlStyle CssClass="hiddenCol" />
                                <FooterStyle CssClass="hiddenCol" />
                                <HeaderStyle CssClass="hiddenCol" />
                                <ItemStyle CssClass="hiddenCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Puesto" HeaderText="Puesto" SortExpression="Puesto">
                                <ControlStyle CssClass="hiddenCol" />
                                <FooterStyle CssClass="hiddenCol" />
                                <HeaderStyle CssClass="hiddenCol" />
                                <ItemStyle CssClass="hiddenCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NombreJefe" HeaderText="NombreJefe" SortExpression="NombreJefe"
                                HtmlEncode="False">
                                <ControlStyle CssClass="hiddenCol" />
                                <FooterStyle CssClass="hiddenCol" />
                                <HeaderStyle CssClass="hiddenCol" />
                                <ItemStyle CssClass="hiddenCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PuestoJefe" HeaderText="PuestoJefe" SortExpression="PuestoJefe">
                                <ControlStyle CssClass="hiddenCol" />
                                <FooterStyle CssClass="hiddenCol" />
                                <HeaderStyle CssClass="hiddenCol" />
                                <ItemStyle CssClass="hiddenCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Telefono" HeaderText="Telefono" SortExpression="Telefono">
                                <ControlStyle CssClass="hiddenCol" />
                                <FooterStyle CssClass="hiddenCol" />
                                <HeaderStyle CssClass="hiddenCol" />
                                <ItemStyle CssClass="hiddenCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SueldoIni" HeaderText="SueldoIni" SortExpression="SueldoIni">
                                <ControlStyle CssClass="hiddenCol" />
                                <FooterStyle CssClass="hiddenCol" />
                                <HeaderStyle CssClass="hiddenCol" />
                                <ItemStyle CssClass="hiddenCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SueldoFin" HeaderText="SueldoFin" SortExpression="SueldoFin">
                                <ControlStyle CssClass="hiddenCol" />
                                <FooterStyle CssClass="hiddenCol" />
                                <HeaderStyle CssClass="hiddenCol" />
                                <ItemStyle CssClass="hiddenCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MotivoBaja" HeaderText="MotivoBaja" SortExpression="MotivoBaja"
                                HtmlEncode="False">
                                <ControlStyle CssClass="hiddenCol" />
                                <FooterStyle CssClass="hiddenCol" />
                                <HeaderStyle CssClass="hiddenCol" />
                                <ItemStyle CssClass="hiddenCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Otro" HeaderText="Otro" SortExpression="Otro">
                                <ControlStyle CssClass="hiddenCol" />
                                <FooterStyle CssClass="hiddenCol" />
                                <HeaderStyle CssClass="hiddenCol" />
                                <ItemStyle CssClass="hiddenCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="EmpresaSeg" HeaderText="EmpresaSeg" SortExpression="EmpresaSeg">
                                <ControlStyle CssClass="hiddenCol" />
                                <FooterStyle CssClass="hiddenCol" />
                                <HeaderStyle CssClass="hiddenCol" />
                                <ItemStyle CssClass="hiddenCol" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PorteArmas" HeaderText="PorteArmas" SortExpression="PorteArmas">
                                <ControlStyle CssClass="hiddenCol" />
                                <FooterStyle CssClass="hiddenCol" />
                                <HeaderStyle CssClass="hiddenCol" />
                                <ItemStyle CssClass="hiddenCol" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                    </asp:GridView>
                    <br />
                    <asp:Panel ID="pnl_EmpleosDetalle" runat="server">
                        <table class="tablaSISSA1" style="width: auto;">
                            <tr>
                                <td style="text-align: right;">
                                    Nombre Empresa
                                </td>
                                <td class="celdaMargenDer10" colspan="4">
                                    <asp:TextBox ID="tbx_NombreEmpresaE" runat="server" Width="350px" ReadOnly="True"
                                        CssClass="textbox1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Calle
                                </td>
                                <td class="celdaMargenDer10" colspan="4">
                                    <asp:TextBox ID="tbx_CalleE" runat="server" ReadOnly="True" Width="350px" CssClass="textbox1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Entre la Calle
                                </td>
                                <td class="celdaMargenDer10" colspan="4">
                                    <asp:TextBox ID="tbx_EntreCalle1E" runat="server" ReadOnly="True" Width="350px" CssClass="textbox1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    Y la Calle
                                </td>
                                <td class="celdaMargenDer10" colspan="4">
                                    <asp:TextBox ID="tbx_EntreCalle2E" runat="server" ReadOnly="True" Width="350px" CssClass="textbox1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Número Exterior
                                </td>
                                <td class="celdaMargenDer10">
                                    <asp:TextBox ID="tbx_NumeroExteriorE" runat="server" ReadOnly="True" Style="text-align: right"
                                        CssClass="textbox1" Width="50px"></asp:TextBox>
                                </td>
                                <td style="text-align: right">
                                    Número Interior
                                </td>
                                <td class="celdaMargenDer10">
                                    <asp:TextBox ID="tbx_NumeroInteriorE" runat="server" ReadOnly="True" Style="text-align: right"
                                        CssClass="textbox1" Width="50px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Colonia
                                </td>
                                <td class="celdaMargenDer10" colspan="4">
                                    <asp:TextBox ID="tbx_ColoniaE" runat="server" Width="350px" ReadOnly="True" CssClass="textbox1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Ciudad
                                </td>
                                <td class="celdaMargenDer10" colspan="4">
                                    <asp:TextBox ID="tbx_CiudadE" runat="server" Width="350px" ReadOnly="True" CssClass="textbox1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Código Postal
                                </td>
                                <td class="celdaMargenDer10">
                                    <asp:TextBox ID="tbx_CPE" runat="server" ReadOnly="True" Style="text-align: right"
                                        CssClass="textbox1" Width="50px"></asp:TextBox>
                                </td>
                                <td style="text-align: right">
                                    Teléfono
                                </td>
                                <td class="celdaMargenDer10">
                                    <asp:TextBox ID="tbx_TelefonoE" runat="server" ReadOnly="True" CssClass="textbox1"
                                        Width="80px"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Fecha Ingreso
                                </td>
                                <td class="celdaMargenDer10">
                                    <asp:TextBox ID="tbx_FechaIngresoE" runat="server" ReadOnly="True" CssClass="textbox1"
                                        Width="80px"></asp:TextBox>
                                </td>
                                <td style="text-align: right">
                                    Fecha Baja
                                </td>
                                <td class="celdaMargenDer10">
                                    <asp:TextBox ID="tbx_FechaBajaE" runat="server" ReadOnly="True" CssClass="textbox1"
                                        Width="80px"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Empresa Seguridad
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_SiSeguridadE" runat="server" Text="Si" />
                                    &nbsp;&nbsp;
                                    <asp:RadioButton ID="rdb_NoSeguridadE" runat="server" Text="No" />
                                </td>
                                <td style="text-align: right">
                                    Porte Armas
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_SiPorteArmas" runat="server" Text="Si" />
                                    &nbsp;&nbsp;
                                    <asp:RadioButton ID="rdb_NoPorteArmas" runat="server" Text="No" />
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Puesto
                                </td>
                                <td class="celdaMargenDer10" colspan="4">
                                    <asp:TextBox ID="tbx_PuestoE" runat="server" ReadOnly="True" Width="350px" CssClass="textbox1"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Nombre Jefe Inmediato
                                </td>
                                <td class="celdaMargenDer10" colspan="4">
                                    <asp:TextBox ID="tbx_NombreJefeE" runat="server" ReadOnly="True" Width="350px" CssClass="textbox1"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Puesto Jefe Inmediato
                                </td>
                                <td class="celdaMargenDer10" colspan="4">
                                    <asp:TextBox ID="tbx_PuestoJefeE" runat="server" Width="350px" ReadOnly="True" CssClass="textbox1"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Sueldo Inicial Mensual
                                </td>
                                <td class="celdaMargenDer10" style="width: 120px">
                                    <asp:TextBox ID="tbx_SueldoIE" runat="server" ReadOnly="True" Style="text-align: right"
                                        CssClass="textbox1" Width="80px"></asp:TextBox>
                                </td>
                                <td style="text-align: right; width: 133px">
                                    Sueldo Final Mensual
                                </td>
                                <td class="celdaMargenDer10">
                                    <asp:TextBox ID="tbx_SueldoFE" runat="server" ReadOnly="True" Style="text-align: right"
                                        CssClass="textbox1" Width="80px"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Motivo de Separación
                                </td>
                                <td class="celdaMargenDer10" colspan="4">
                                    <asp:TextBox ID="tbx_MotivoSeparacionE" runat="server" ReadOnly="True" Width="350px"
                                        CssClass="textbox1"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Otro Motivo
                                </td>
                                <td class="celdaMargenDer10" colspan="4">
                                    <asp:TextBox ID="tbx_OtroMotivoE" runat="server" ReadOnly="True" Width="350px" CssClass="textbox1"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                                <td>
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </ajax:TabPanel>
            <ajax:TabPanel ID="tab_Referencias" runat="server" HeaderText="Referencias">
                <ContentTemplate>
                    <br />
                    <asp:GridView ID="dgv_Referencias" runat="server" AutoGenerateColumns="False" CssClass="gridSISSA"
                        Width="100%">
                        <Columns>
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                            <asp:BoundField DataField="Sexo" HeaderText="Sexo" SortExpression="Sexo" />
                            <asp:BoundField DataField="Ocupacion" HeaderText="Ocupación" SortExpression="Ocupacion" />
                            <asp:BoundField DataField="Domicilio" HeaderText="Domicilio" SortExpression="Domicilio" />
                            <asp:BoundField DataField="EntreCalle1" HeaderText="EntreCalle1" SortExpression="EntreCalle1" />
                            <asp:BoundField DataField="EntreCalle2" HeaderText="EntreCalle2" SortExpression="EntreCalle2" />
                            <asp:BoundField DataField="NumeroExt" HeaderText="NumExt" SortExpression="NumeroExt" />
                            <asp:BoundField DataField="NumeroInt" HeaderText="NumInt" SortExpression="NumeroInt" />
                            <asp:BoundField DataField="Colonia" HeaderText="Colonia" SortExpression="Colonia" />
                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" SortExpression="Ciudad" />
                            <asp:BoundField DataField="CodigoPostal" HeaderText="CP" SortExpression="CodigoPostal" />
                            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" SortExpression="Telefono" />
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                        </Columns>
                        <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                    </asp:GridView>
                </ContentTemplate>
            </ajax:TabPanel>
            <ajax:TabPanel ID="tab_Varios" runat="server" HeaderText="Datos Varios">
                <HeaderTemplate>
                    Datos Varios
                </HeaderTemplate>
                <ContentTemplate>
                    <br />
                    <table class="tablaSISSA1" style="width: auto;">
                        <tr>
                            <td style="text-align: right">
                                Ingreso Familiar Mensual
                            </td>
                            <td style="width: 90px">
                                <asp:TextBox ID="tbx_IngresoFamiliar" runat="server" ReadOnly="True" Style="margin-left: 0px;
                                    text-align: right" Width="90px" CssClass="textbox1"></asp:TextBox>
                            </td>
                            <td style="text-align: right">
                                Gasto Familiar Mensual
                            </td>
                            <td style="width: 90px">
                                <asp:TextBox ID="tbx_GastoFamiliar" runat="server" ReadOnly="True" Style="margin-left: 0px;
                                    text-align: right" Width="90px" AutoCompleteType="Disabled" CssClass="textbox1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Ingreso Adicional
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_IngresoAdicional" runat="server" ReadOnly="True" Style="margin-left: 0px;
                                    text-align: right" Width="90px" CssClass="textbox1"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Descripción
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="tbx_DescripcionAdicional" runat="server" ReadOnly="True" Style="margin-left: 0px"
                                    Width="350px" CssClass="textbox1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Tipo Vivienda
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="tbx_TipoVivienda" runat="server" ReadOnly="True" Style="margin-left: 0px"
                                    Width="350px" CssClass="textbox1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Pago Mensual
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_PagoMensual" runat="server" ReadOnly="True" Style="margin-left: 0px;
                                    text-align: right" Width="90px" CssClass="textbox1"></asp:TextBox>
                            </td>
                            <td style="text-align: right">
                                Valor Vivienda
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_ValorVivienda" runat="server" ReadOnly="True" Style="margin-left: 0px;
                                    text-align: right" Width="90px" CssClass="textbox1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Vehículo Propio
                            </td>
                            <td>
                                <asp:RadioButton ID="rdb_SiVehiculo" runat="server" Text="Si" />&nbsp;&nbsp;
                                <asp:RadioButton ID="rdb_NoVehiculo" runat="server" Text="No" />
                            </td>
                            <td style="text-align: right">
                                Modelo(Año)
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_Modelo" runat="server" ReadOnly="True" Style="margin-left: 0px;
                                    text-align: right" Width="90px" CssClass="textbox1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td style="text-align: right">
                                Valor Vehículo
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_ValorVehiculo" runat="server" ReadOnly="True" Style="margin-left: 0px;
                                    text-align: right" Width="90px" CssClass="textbox1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Vicios
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="tbx_Vicios" runat="server" ReadOnly="True" Style="margin-left: 0px"
                                    Width="350px" CssClass="textbox1"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Idiomas/%Dominio
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="tbx_Idiomas" runat="server" ReadOnly="True" Style="margin-left: 0px"
                                    Width="350px" CssClass="textbox1"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Actividades Culturales/Deportivas
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="tbx_ActividadesCulturales" runat="server" ReadOnly="True" Style="margin-left: 0px"
                                    Width="350px" CssClass="textbox1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                Habilidades o Aptitudes/%Dominio
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="tbx_Habilidades" runat="server" ReadOnly="True" Style="margin-left: 0px"
                                    Width="350px" CssClass="textbox1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView ID="dgv_Señas" runat="server" AutoGenerateColumns="False" CssClass="gridSISSA"
                        Width="100%">
                        <Columns>
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" SortExpression="Descripcion" />
                            <asp:BoundField DataField="Forma" HeaderText="Forma" SortExpression="Forma" />
                            <asp:BoundField DataField="Ubicacion" HeaderText="Ubicación" SortExpression="Ubicacion" />
                            <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" SortExpression="Comentarios" />
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" />
                        </Columns>
                        <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                    </asp:GridView>
                </ContentTemplate>
            </ajax:TabPanel>
            <ajax:TabPanel ID="tab_Papeleria" runat="server" HeaderText="Papelería Recibida">
                <HeaderTemplate>
                    Papelería Recibida
                </HeaderTemplate>
                <ContentTemplate>
                    <br />
                    <table class="tablaSISSA1" style="width: auto;">
                        <tr>
                            <td style="text-align: right">
                                <asp:CheckBox ID="chb_ActaNac" runat="server" EnableTheming="True" TextAlign="Left" />
                            </td>
                            <td>
                                Acta de Nacimiento
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:CheckBox ID="chb_ActaMatrimonio" runat="server" TextAlign="Left" />
                            </td>
                            <td>
                                Acta de Matrimonio
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:CheckBox ID="chb_IMSS" runat="server" TextAlign="Left" />
                            </td>
                            <td>
                                Hoja del IMSS
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:CheckBox ID="chb_CompDom" runat="server" TextAlign="Left" />
                            </td>
                            <td>
                                Comprobante de Domicilio
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:CheckBox ID="chb_IFE" runat="server" TextAlign="Left" />
                            </td>
                            <td>
                                Credencial de Elector
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:CheckBox ID="chb_CompEstudios" runat="server" TextAlign="Left" />
                            </td>
                            <td>
                                Comprobante de Estudios
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:CheckBox ID="chb_Cartilla" runat="server" TextAlign="Left" />
                            </td>
                            <td>
                                Cartilla Militar
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:CheckBox ID="chb_Recomendacion" runat="server" TextAlign="Left" />
                            </td>
                            <td>
                                Carta de Recomendación
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:CheckBox ID="chb_NoAntecedentes" runat="server" TextAlign="Left" />
                            </td>
                            <td>
                                Carta de No Antecedentes Penales
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:CheckBox ID="chb_Fotografias" runat="server" TextAlign="Left" />
                            </td>
                            <td>
                                Fotografías
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:CheckBox ID="chb_CURP" runat="server" TextAlign="Left" />
                            </td>
                            <td>
                                CURP
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                                <asp:CheckBox ID="chb_Huellas" runat="server" TextAlign="Left" />
                            </td>
                            <td>
                                Huellas para expediente
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <br />
                </ContentTemplate>
            </ajax:TabPanel>
            <ajax:TabPanel ID="tab_Fotos" runat="server" HeaderText="Fotos">
                <ContentTemplate>
                    <br />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Image runat="server" ID="ICompleto" Height="400px" Width="200px" />
                    &nbsp;&nbsp;
                    <asp:Image runat="server" ID="IFrente" Height="200px" Width="200px" />
                    &nbsp;&nbsp;
                    <asp:Image runat="server" ID="IDerecho" Height="200px" Width="200px" />
                    &nbsp;&nbsp;
                    <asp:Image runat="server" ID="IIzquierdo" Height="200px" Width="200px" />
                    &nbsp;&nbsp;
                    <asp:Image runat="server" ID="IFirma" Height="200px" Width="200px" />
                    &nbsp;
                </ContentTemplate>
            </ajax:TabPanel>
        </ajax:TabContainer>
        <br />
    </div>
</body>
</html>
