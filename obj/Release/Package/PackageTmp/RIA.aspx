<%@ Page Title="Captura de Reportes de Incidentes/Accidentes" Language="VB" MasterPageFile="~/MP_Principal.master"
    AutoEventWireup="false" Inherits="IntranetSIAC.RIA" CodeBehind="RIA.aspx.vb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="Div_Principal">
        <div>

            <asp:Panel runat="server" ID="pnl_CapturaDatos" Enabled="true">
                <table class="tablaSISSA1">
                    <tr>
                        <td style="width: 150px; height: 18px; text-align: right; vertical-align: bottom">
                            <asp:Label ID="lbl_Fecha_Inicio" runat="server" Text="Fecha I/A"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:TextBox ID="tbx_FechaIA" runat="server" CssClass="calendarioAjax" AutoPostBack="true"
                                ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario" MaxLength="1"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbx_FechaIA"
                                FilterType="Custom, Numbers" ValidChars="/">
                            </ajax:FilteredTextBoxExtender>
                            <ajax:CalendarExtender runat="server" ID="CalendarExtender1" TargetControlID="tbx_FechaIA"
                                CssClass="calendarioAjax" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                            </ajax:CalendarExtender>
                        </td>
                        <td style="text-align: right">
                            <asp:Label ID="lbl_Hora_Inicio" runat="server" Text="Hora I/A"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:DropDownList runat="server" ID="ddl_HoraIA" Width="50px" CssClass="DropDownList18">
                            </asp:DropDownList>
                            <label style="font-weight: bold">
                                :</label>
                            <asp:DropDownList runat="server" ID="ddl_MinIA" Width="50px" CssClass="DropDownList18">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 125px"></td>
                    </tr>
                    <tr>
                        <td style="width: 150px; text-align: right">
                            <asp:Label ID="Label1" runat="server" Text="Tipo"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10" colspan="5">
                            <asp:DropDownList ID="ddl_Tipo" runat="server" DataTextField="Tipo" CssClass="DropDownList18"
                                AutoPostBack="true" DataValueField="Clave_Tipo" Width="408px">
                                <asp:ListItem Value="0" Text="Seleccione..."></asp:ListItem>
                                <asp:ListItem Value="1" Text="INCIDENTE DE UNIDAD (VEHICULO)"></asp:ListItem>
                                <asp:ListItem Value="2" Text="INCIDENTE DE GUARDIA DE SEGURIDAD PATRIMONIAL"></asp:ListItem>
                                <asp:ListItem Value="3" Text="INCIDENTE EN AREA SEGURA"></asp:ListItem>
                                <asp:ListItem Value="4" Text="INCIDENTE EN AREA NO SEGURA"></asp:ListItem>
                                <asp:ListItem Value="5" Text="INCIDENTE CON CLIENTES"></asp:ListItem>
                                <asp:ListItem Value="6" Text="INCIDENTE EN RUTA"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px; text-align: right">
                            <asp:Label ID="Label4" runat="server" Text="Entidad"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10" colspan="5">
                            <asp:DropDownList ID="ddl_Entidad" runat="server" DataTextField="Nombre_Comercial"
                                CssClass="DropDownList18" DataValueField="Clave_Cliente" Width="408px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right" valign="top">Descripción
                        </td>
                        <td class="celdaMargenDer10" colspan="5">
                            <asp:TextBox ID="tbx_Descripcion" runat="server" Width="600px" Height="100px" TextMode="MultiLine"
                                CssClass="textbox1" MaxLength="1000" Style="text-transform: uppercase"></asp:TextBox>
                        </td>
                        <asp:RequiredFieldValidator ID="rfv_Descripcion" ControlToValidate="tbx_Descripcion"
                            ValidationGroup="Val1" runat="server" ErrorMessage="Capture la Descripción."
                            ForeColor="Red">*</asp:RequiredFieldValidator>
                    </tr>
                    <tr>
                        <td style="text-align: right" valign="top">Notas
                        </td>
                        <td class="celdaMargenDer10" colspan="5">
                            <asp:TextBox ID="tbx_Notas" runat="server" Width="600px" Height="85px" TextMode="MultiLine"
                                CssClass="textbox1" MaxLength="500" Style="text-transform: uppercase"></asp:TextBox>
                        </td>
                        <asp:RequiredFieldValidator ID="rfv_Notas" ControlToValidate="tbx_Notas" ValidationGroup="Val1"
                            runat="server" ErrorMessage="Capture las Notas." ForeColor="Red">*</asp:RequiredFieldValidator>
                    </tr>
                    <tr>
                        <td style="width: 150px; text-align: right">
                            <asp:Label ID="Label2" runat="server" Text="Usuarios para Seguimiento"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10" colspan="5" style="width: 408px">
                            <asp:DropDownList ID="ddl_UsuarioSeg" runat="server" DataTextField="Nombre" CssClass="DropDownList18"
                                DataValueField="Id_Empleado" Width="408px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px; text-align: right">
                            <asp:Label ID="lbl_TipoUsuario" runat="server" Text="Tipo Usuarios"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:RadioButton ID="rdb_Principal" runat="server" Text="Principal" GroupName="TipoU" />
                            &nbsp;&nbsp;<asp:RadioButton ID="rdb_Secundario" runat="server" Text="Secundario"
                                GroupName="TipoU" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btn_Agregar" Text="Agregar" Width="90px" CssClass="button" />
                        </td>
                        <td></td>
                        <td></td>
                        <td style="width: 110px"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="celdaMargenDer10" colspan="4">
                            <asp:GridView runat="server" ID="gv_Usuarios"
                                Width="408px" CssClass="gv_general"
                                Height="100%" AutoGenerateColumns="False"
                                DataKeyNames="Id_Empleado">

                                <Columns>
                                    <asp:BoundField DataField="Id_Empleado" HeaderText="Id_Empleado" Visible="False">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Imagenes/EliminarEmpleado.png"
                                        ShowDeleteButton="True">
                                        <ItemStyle Width="20px" />
                                    </asp:CommandField>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Rol" HeaderText="Tipo">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                </Columns>

                                <RowStyle CssClass="rowHover" />
                                <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                                <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px; text-align: right; vertical-align: baseline">
                            <asp:Label ID="Label3" runat="server" Text="Fecha Estimada Fin"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:TextBox ID="tbx_FechaEstFin" runat="server" CssClass="calendarioAjax"
                                AutoPostBack="true" MaxLength="1" ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbx_FechaEstFin"
                                FilterType="Custom, Numbers" ValidChars="/">
                            </ajax:FilteredTextBoxExtender>
                            <ajax:CalendarExtender runat="server" ID="CalendarExtender2" TargetControlID="tbx_FechaEstFin"
                                CssClass="calendarioAjax" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                            </ajax:CalendarExtender>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="celdaMargenDer10">
                            <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" Width="90px" CssClass="button" />
                        </td>
                    </tr>
                </table>


            </asp:Panel>


            <br />
            <%--<br />--%>
            <asp:Panel ID="pnl_AgregarI" runat="server" Enabled="false">
                <table class="tablaSISSA3">
                    <tr>
                        <td class="celdaMargenDer10" style="width: 150px; text-align: left"></td>
                        <td>
                            <asp:Label ID="lbl_Numero" runat="server" Style="font-size: small; font-weight: bold"
                                Text="El Número del Reporte es:"></asp:Label>
                        </td>
                        <td style="width: 150px; text-align: left">
                            <asp:Label ID="lbl_NumRIA" runat="server" Style="font-size: x-large; color: red; font-weight: bold">######</asp:Label>
                        </td>
                        <td style="width: 150px; text-align: left"></td>
                    </tr>
                </table>
                <br />
                <%--<br />--%>
                <table class="tablaSISSA3">
                    <tr>
                        <td style="width: 150px; text-align: left"></td>
                        <td class="celdaMargenDer10" style="width: 400px; text-align: left" colspan="3">
                            <asp:Label ID="lbl_Mensaje" runat="server" Style="font-size: x-small; font-weight: bold"
                                Text="Si lo desea puede agregar Imagenes a su Reporte."></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px; text-align: left"></td>
                        <td class="celdaMargenDer10" style="width: 400px; text-align: left" colspan="3">
                            <asp:Label ID="Label5" runat="server" Style="font-size: x-small; font-weight: bold"
                                Text="Las Imagenes deben ser menores de 300 kb"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 25px"></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <%--<tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>--%>
                    <tr>
                        <td style="width: 150px; text-align: right">
                            <asp:Label ID="lbl_Imagen" runat="server" Text="Agregar Imagen(es):"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10" colspan="2">
                            <asp:FileUpload ID="FileUpload1" runat="server" Width="500px"  />
                        </td>
                        <td>
                            <asp:Button ID="btn_Subir" Text="Guardar" Width="90px" runat="server" CssClass="button" ></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td>
                            <asp:HiddenField runat="server" ID="hfd_IDRIA" />
                        </td>
                        <td>
                            <asp:HiddenField runat="server" ID="hfd_IDRIAD" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td>
                            <asp:Button runat="server" ID="btn_Nuevo" Text="Nuevo RIA" Width="90px" CssClass="button" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:PlaceHolder ID="PlaceHolderImagenesRIA" runat="server"></asp:PlaceHolder>

        </div>


    </div>
</asp:Content>
