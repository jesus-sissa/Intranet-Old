<%@ Page Title="EVALUAR PROSPECTOS" Language="VB" AutoEventWireup="false"
    Inherits="IntranetSIAC.ProspectosValida" MasterPageFile="~/MP_Principal.master" Codebehind="ProspectosValida.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:GridView ID="dgv_Prospectos" runat="server"
         AutoGenerateColumns="False" CssClass="gv_general"
        DataKeyNames="Id_EmpleadoP" Width="100%" AllowPaging="True" PageSize="15" >
        
        <EmptyDataRowStyle BackColor="LightYellow" />
        <EmptyDataTemplate>
            No Existen Prospectos capturados
        </EmptyDataTemplate>
        <Columns>
            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/1rightarrow.png"
                ShowSelectButton="true">
                <ControlStyle Width="16px" />
            </asp:CommandField>
            <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha Registro" SortExpression="FechaRegistro" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
            <asp:BoundField DataField="Departamento" HeaderText="Departamento" SortExpression="Departamento" />
            <asp:BoundField DataField="Puesto" HeaderText="Puesto" SortExpression="Puesto" />
        </Columns>

        <RowStyle CssClass="rowHover" />
 <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
  <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
   <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
    </asp:GridView>
    <br />
    <asp:Panel runat="server" ID="pnl_DatosTodosP" Visible="true" Height="100%">
        <ajax:TabContainer ID="tbc_DatosProspecto" runat="server" ActiveTabIndex="0" Width="100%"
            Font-Size="Small" Height="675px" Visible="False">
            <ajax:TabPanel ID="tab_GeneralesP" runat="server" HeaderText="Datos Generales" Height="150px">
                <HeaderTemplate>
                    Datos Generales</HeaderTemplate>
                <ContentTemplate>
                    <asp:Panel ID="pnl_DatosG" runat="server" Height="676px">
                        <table class="tablaSISSA1">
                            <tr>
                                <td style="text-align: right; width: 110px">
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Nombre Completo
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="tbx_NombreCompletoP" runat="server" ReadOnly="True" CssClass="textbox1"
                                        Width="250px" />
                                </td>
                                <td align="right">
                                    Correo Electrónico
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_MailP" runat="server" ReadOnly="True" Width="250px" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Nombre(s)
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_NombresP" runat="server" ReadOnly="True" Width="210px" CssClass="textbox1" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right">
                                    Teléfono
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_TelefonoP" runat="server" ReadOnly="True" Width="210px" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Apellido Paterno
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_ApellidoPaternoP" runat="server" ReadOnly="True" CssClass="textbox1"
                                        Width="210px" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right">
                                    Teléfono Móvil
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_TelefonoMovilP" runat="server" ReadOnly="True" CssClass="textbox1"
                                        Width="210px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Apellido Materno
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_ApellidoMaternoP" runat="server" ReadOnly="True" CssClass="textbox1"
                                        Width="210px" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right">
                                    Clave Elector
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_ElectorP" runat="server" ReadOnly="True" Width="110px" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Departamento
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_DepartamentoP" runat="server" ReadOnly="True" CssClass="textbox1"
                                        Width="210px" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right">
                                    RFC
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_RFCP" runat="server" ReadOnly="True" Width="110px" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Puesto
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_PuestoP" runat="server" ReadOnly="True" Width="210px" CssClass="textbox1" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right">
                                    CURP
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_CURPP" runat="server" ReadOnly="True" Width="110px" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Calle
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_CalleP" runat="server" ReadOnly="True" Width="210px" CssClass="textbox1" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right">
                                    IMSS
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_IMSSP" runat="server" ReadOnly="True" Width="110px" CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Entre la Calle
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_EntreCalle1P" runat="server" ReadOnly="True" CssClass="textbox1"
                                        Width="210px" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right">
                                    UMF
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_UMFP" runat="server" ReadOnly="True" Width="110px" Style="text-align: right"
                                        CssClass="textbox1" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Y la Calle
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_EntreCalle2P" runat="server" ReadOnly="True" CssClass="textbox1"
                                        Width="209px" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right">
                                    Num. Cartilla
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_NumCartillaP" runat="server" ReadOnly="True" CssClass="textbox1"
                                        Width="110px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Número Exterior
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="tbx_NumExteriorP" runat="server" ReadOnly="True" Style="text-align: right"
                                        CssClass="textbox1" Width="50px" />
                                </td>
                                <td align="right">
                                    Número Interior
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_NumInteriorP" runat="server" ReadOnly="True" Style="text-align: right"
                                        CssClass="textbox1" Width="50px" />
                                </td>
                                <td align="right">
                                    Pasaporte
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_NumPasaporteP" runat="server" ReadOnly="True" CssClass="textbox1"
                                        Width="110px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Colonia
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_ColoniaP" runat="server" Width="210px" CssClass="textbox1" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right">
                                    Tipo Licencia
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_TipoLicenciaP" runat="server" ReadOnly="True" CssClass="textbox1"
                                        Width="210px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Ciudad
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_CiudadP" runat="server" ReadOnly="True" Width="210px" CssClass="textbox1" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right">
                                    Fecha Expira Licencia
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_FechaExpiraP" runat="server" ReadOnly="True" CssClass="textbox1"
                                        Width="110px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Zona
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_ZonaP" runat="server" ReadOnly="True" Width="210px" CssClass="textbox1" />
                                </td>
                                <td align="right">
                                    C.P.
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_CPP" runat="server" ReadOnly="True" Width="50px" Style="text-align: right"
                                        CssClass="textbox1" />
                                </td>
                                <td align="right">
                                    En Catálogo Firmas
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_SiCatFirmasP" runat="server" Text="Si" />
                                    &nbsp;&nbsp;<asp:RadioButton ID="rdb_NoCatFirmasP" runat="server" Text="No" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Género
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_MasculinoP" runat="server" Text="Masculino" />
                                    &nbsp;
                                    <asp:RadioButton ID="rdb_FemeninoP" runat="server" Text="Femenino" />
                                </td>
                                <td align="right">
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="right">
                                    Certficación Academia
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_SiCertifP" runat="server" Text="Si" />
                                    &nbsp;&nbsp;<asp:RadioButton ID="rdb_NoCertifP" runat="server" Text="No" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Vive con su Familia
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_SiConFamP" runat="server" Text="Si" />
                                    &nbsp;
                                    <asp:RadioButton ID="rdb_NoConFamP" runat="server" Text="No" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                </td>
                                <td align="right">
                                    Empleado Referencia
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_EmpleadoRefP" runat="server" ReadOnly="True" CssClass="textbox1"
                                        Width="250px" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Estado Civil
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="tbx_EstadoCivilP" runat="server" ReadOnly="True" Width="110px" CssClass="textbox1"></asp:TextBox>
                                </td>
                                <td align="right">
                                    Cantidad Hijos
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_CantidadHijosP" runat="server" ReadOnly="True" Style="text-align: right"
                                        CssClass="textbox1" Width="50px" />
                                </td>
                                <td align="right">
                                    Es Jefe de Area
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_SiJefeP" runat="server" Text="Si" />
                                    &nbsp;&nbsp;<asp:RadioButton ID="rdb_NoJefeP" runat="server" Text="No" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Modo Nacionalidad
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="tbx_ModoNacP" runat="server" ReadOnly="True" Width="110px" CssClass="textbox1"></asp:TextBox>
                                </td>
                                <td align="right">
                                    Edad
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_EdadP" runat="server" ReadOnly="True" Width="50px" Style="text-align: right"
                                        CssClass="textbox1" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Lugar Nacimiento
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_LugarNacP" runat="server" ReadOnly="True" Width="210px" CssClass="textbox1"></asp:TextBox>
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
                            <tr>
                                <td align="right">
                                    País Nacimiento
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_PaisNacP" runat="server" ReadOnly="True" Width="210px" CssClass="textbox1"></asp:TextBox>
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
                            <tr>
                                <td align="right">
                                    Fecha Nacimiento
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_FechaNacP" runat="server" ReadOnly="True" Width="110px" CssClass="textbox1"></asp:TextBox>
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
                            <tr>
                                <td align="right">
                                    Fecha Naturalización
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="tbx_FechaNaturalizacionP" runat="server" ReadOnly="True" CssClass="textbox1"
                                        Width="110px"></asp:TextBox>
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
                            <tr>
                                <td align="right">
                                    &nbsp;
                                </td>
                                <td colspan="2">
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
                    </asp:Panel>
                </ContentTemplate>
            </ajax:TabPanel>
            <ajax:TabPanel ID="tab_Entrevistas" runat="server" HeaderText="Entrevistas">
                <HeaderTemplate>
                    Entrevistas
                </HeaderTemplate>
                <ContentTemplate>
                    <br />
                    <asp:GridView ID="dgv_Entrevistas" runat="server"
                         AutoGenerateColumns="False" CssClass="gv_general"
                        Width="100%">
                       
                        <Columns>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha">
                                <ItemStyle Width="30px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Entrevisto" HeaderText="Entrevisto" SortExpression="Entrevisto" />
                            <asp:BoundField DataField="Apto" HeaderText="Apto" SortExpression="Apto" />
                            <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" SortExpression="Comentarios" />
                        </Columns>
                        <EmptyDataRowStyle BackColor="LightYellow" />
                        <EmptyDataTemplate>
                            No Existen Entrevistas capturadas
                        </EmptyDataTemplate>
                     
  <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
   <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />

                    </asp:GridView>
                    <br />
                    <br />
                    <asp:Panel ID="pnl_Firma" runat="server" ForeColor="Black" Visible="False" Width="542px">
                        &nbsp;
                        <table class="tablaSISSA1">
                            <tr>
                                <td style="text-align: right">
                                    Fecha
                                </td>
                                <td colspan="2" class="calendarioAjax">
                                    <asp:TextBox ID="tbx_FechaEntrevista" runat="server" AutoPostBack="True" CssClass="calendarioAjax"
                                        MaxLength="1"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="tbx_FechaEntrevista"
                                        FilterType="Custom, Numbers" ValidChars="/" Enabled="True">
                                    </ajax:FilteredTextBoxExtender>
                                    <ajax:CalendarExtender runat="server" ID="CalendarExtender3" TargetControlID="tbx_FechaEntrevista"
                                        CssClass="calendarioAjax" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy" Enabled="True">
                                    </ajax:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                </td>
                                <td colspan="2" rowspan="1" style="text-align: left;">
                                    <asp:RadioButton ID="rdb_AptoP" runat="server" GroupName="Radios" Text="Apto" />
                                    &nbsp;&nbsp;
                                    <asp:RadioButton ID="rdb_NoAptoP" runat="server" GroupName="Radios" Text="No Apto" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;" valign="top">
                                    Comentarios
                                </td>
                                <td colspan="2" rowspan="4" style="text-align: left;">
                                    <asp:TextBox ID="tbx_Comentarios" runat="server" Height="85px" TextMode="MultiLine"
                                        Width="400px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td rowspan="1" style="text-align: right;">
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    Contraseña
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="tbx_Contrasena" runat="server" Style="margin-left: 0px" TextMode="Password" CssClass="textbox1"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    &nbsp;
                                </td>
                                <td style="text-align: left;">
                                    <asp:Button ID="btn_Aceptar" runat="server" Text="Aceptar" Width="70px" CssClass="button" />
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </asp:Panel>
                </ContentTemplate>
            </ajax:TabPanel>
        </ajax:TabContainer>
        <br />
    </asp:Panel>

    <asp:ValidationSummary runat="server" ShowMessageBox="true" ShowSummary="false" ID="ValidationSummary1" />

</asp:Content>
