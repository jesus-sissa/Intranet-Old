<%@ Control Language="VB" AutoEventWireup="false"
    Inherits="SISSAIntranet.UserControls_wuc_Insumos" Codebehind="wuc_Insumos.ascx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../Estilos/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="Div_Principal">
        <%--<br />
        <div id="divEnca" title="prueba">
            <strong>Insumos</strong>
        </div>--%>
        <br />
        <ajax:TabContainer ID="tc_Insumos" runat="server" Width="100%" 
            ActiveTabIndex="0">
            <ajax:TabPanel ID="tab_Solicitud" runat="server" HeaderText="Solicitud de Insumos">
                <HeaderTemplate>
                    Solicitud de Insumos
                </HeaderTemplate>
                <ContentTemplate>
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <a href="#Solicitud">Ir al Final</a>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <fieldset>
                        
                        <br />
                        <asp:UpdatePanel runat="server" ID="udp_Clases">
                            <ContentTemplate>
                                <asp:RadioButton ID="rdb_Accesorios" Text="Accesorios" runat="server" />
                                <asp:RadioButton ID="rdb_Consumibles" Text="Consumibles" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <asp:UpdatePanel runat="server" ID="udp_SubClases">
                            <ContentTemplate>
                                <asp:GridView runat="server" ID="gv_Consumibles" AutoGenerateColumns="False" Style="color: Black;
                                    font-family: Verdana; font-size: x-small" DataKeyNames="Id_SubClase" Width="100%">
                                    <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Id_SubClase" Visible="False" DataField="Id_SubClase" />
                                        <asp:BoundField DataField="Clave" HeaderText="Clave" Visible="False">
                                            <ItemStyle Width="30px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                        <asp:BoundField DataField="Elementos" HeaderText="SubClases" Visible="False" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" Visible="False" />
                                        <asp:TemplateField HeaderText="Cantidad">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="tbx_Cantidad" Width="50px" Height="15px" MaxLength="3"></asp:TextBox><ajax:FilteredTextBoxExtender
                                                    runat="server" ID="ftb_Cantidad" TargetControlID="tbx_Cantidad" FilterType="Numbers">
                                                </ajax:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle CssClass="SelectedRowSISSA" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <table width="100%">
                            <tr align="right">
                                <td>
                                    <asp:Button ID="btn_Agregar" runat="server" Text="Agregar" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                     <p><a name="Solicitud"></a></p>
                    
                    <fieldset style="width: 60%;">
                        <p style="text-align: center; width: 100%">
                            <strong>Insumos Agregados</strong></p>
                        <asp:UpdatePanel ID="udp_Solicitud" runat="server">
                            <ContentTemplate>
                                <asp:GridView runat="server" ID="gv_Solicitud" AutoGenerateColumns="False" Style="color: Black;
                                    font-family: Verdana; font-size: x-small" Width="100%" DataKeyNames="Id_Subclase">
                                    <Columns>
                                        <asp:BoundField HeaderText="Id_SubClase" DataField="Id_SubClase" Visible="False">
                                            <ItemStyle Width="50px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Clave" DataField="Clave" ReadOnly="true" Visible="False">
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Descripción" DataField="Descripcion" HtmlEncode="false"
                                            ReadOnly="true" />
                                        <asp:TemplateField HeaderText="Cantidad">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="tbx_CantidadSolicitada" runat="server" Text='<%# Bind("Cantidad") %>'
                                                    Width="100px"></asp:TextBox><ajax:FilteredTextBoxExtender runat="server" ID="ftb_CantSol"
                                                        TargetControlID="tbx_CantidadSolicitada" FilterType="Numbers">
                                                    </ajax:FilteredTextBoxExtender>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_CantidadSolicitada" runat="server" Text='<%# Bind("Cantidad") %>'></asp:Label></ItemTemplate>
                                            <ItemStyle Width="70px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <table width="100%">
                            <tr align="right">
                                <td>
                                    <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                    
                    <a href="#top">Ir al Principio</a>
                </ContentTemplate>
            </ajax:TabPanel>
            <ajax:TabPanel ID="tab_Consulta" runat="server" HeaderText="Consulta">
                <ContentTemplate>
                    <br />
                    <table class="tablaSISSA1">
                        <tr>
                            <td style="width: 120px; text-align: right">
                                <asp:Label ID="lbl_Fecha_Inicio" runat="server" Text="Fecha Inicio"></asp:Label>
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:TextBox ID="tbx_FechaIni" runat="server" CssClass="CalendarTextbox"
                                    AutoPostBack="True" MaxLength="1"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbx_FechaIni"
                                    FilterType="Custom, Numbers" ValidChars="/" Enabled="True">
                                </ajax:FilteredTextBoxExtender>
                                <ajax:CalendarExtender runat="server" ID="CalendarExtender1" TargetControlID="tbx_FechaIni"
                                    CssClass="calendarSISSA" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy"
                                    Enabled="True">
                                </ajax:CalendarExtender>
                            </td>
                            <td align="right" style="width: 70px">
                                <asp:Label ID="Lbl_FechaFin" runat="server" Text="Fecha Fin"></asp:Label>
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:TextBox ID="tbx_FechaFin" runat="server" CssClass="CalendarTextbox"
                                    AutoPostBack="True" MaxLength="1"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbx_FechaFin"
                                    FilterType="Custom, Numbers" ValidChars="/" Enabled="True">
                                </ajax:FilteredTextBoxExtender>
                                <ajax:CalendarExtender runat="server" ID="CalendarExtender2" TargetControlID="tbx_FechaFin"
                                    CssClass="calendarSISSA" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy"
                                    Enabled="True">
                                </ajax:CalendarExtender>
                            </td>
                            <td style="width: 30px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lbl_Usuario" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td class="celdaMargenDer10" colspan="3">
                                <asp:DropDownList ID="ddl_Status" runat="server" CssClass="DropDownList18"
                                    AutoPostBack="True" Width="270px" Enabled="False">
                                    <asp:ListItem Value="0" Text="Seleccione..."></asp:ListItem>
                                    <asp:ListItem Value="A" Text="PENDIENTE"></asp:ListItem>
                                    <asp:ListItem Value="V" Text="VALIDADA"></asp:ListItem>
                                    <asp:ListItem Value="S" Text="SURTIDA"></asp:ListItem>
                                    <asp:ListItem Value="C" Text="CANCELADA"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_Status" runat="server" Text="Todos" AutoPostBack="True"
                                    Width="70px" Checked="True" />
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:Button ID="btn_Mostrar" runat="server" Text="Mostrar" Width="100px" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:GridView ID="gv_Solicitudes" runat="server" DataKeyNames="Id_Solicitud" AllowPaging="True"
                        Width="100%" Style="color: Black; font-family: Verdana; font-size: x-small" AutoGenerateColumns="False">
                        <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select"
                                        ImageUrl="~/Imagenes/1rightarrow.png" /></ItemTemplate>
                                <ItemStyle Width="15px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Id_Solicitud" HeaderText="Id_Solicitud" Visible="False" />
                            <asp:BoundField DataField="Numero" HeaderText="Numero" />
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                            <asp:BoundField DataField="Hora" HeaderText="Hora" />
                            <asp:BoundField DataField="UsuarioSolicita" HeaderText="Usuario Solicita" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                        </Columns>
                        <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle CssClass="SelectedRowSISSA" />
                    </asp:GridView>
                    <br />
                    <asp:GridView runat="server" ID="gv_Detalle" AutoGenerateColumns="False" Style="color: Black;
                        font-family: Verdana; font-size: x-small" Width="100%" DataKeyNames="Id_Subclase">
                        <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                        <Columns>
                            <asp:BoundField HeaderText="Id_Subclase" DataField="Id_Subclase" Visible="False">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Clave" DataField="Clave" ReadOnly="True" Visible="False">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Descripción" DataField="Descripcion" HtmlEncode="False"
                                ReadOnly="True" />
                            <asp:TemplateField HeaderText="Cantidad Solicitada">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_CantidadSolicitada" runat="server" Text='<%# Bind("CantidadSolicitada") %>'></asp:Label></ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="tbx_CantidadSolicitada" runat="server" Text='<%# Bind("Cantidad") %>'
                                        Width="100px"></asp:TextBox><ajax:FilteredTextBoxExtender runat="server" ID="ftb_CantSol"
                                            TargetControlID="tbx_CantidadSolicitada" FilterType="Numbers">
                                        </ajax:FilteredTextBoxExtender>
                                </EditItemTemplate>
                                <ItemStyle Width="110px" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="CantidadValidada" HeaderText="CantidadValidada">
                                <ItemStyle Width="70px" HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CantidadSurtida" HeaderText="CantidadSurtida">
                                <ItemStyle HorizontalAlign="Right" Width="70px" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </ajax:TabPanel>
            <%--<ajax:TabPanel ID="tab_Listado" runat="server" HeaderText="Listado">
                <ContentTemplate>
                    <asp:GridView ID="gv_Insumos" runat="server" DataKeyNames="Id_Insumo,Clave,Status"
                        Width="100%" Style="color: Black; font-family: Verdana; font-size: x-small" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="Id_Insumo" HeaderText="Id_Insumo" Visible="False" />
                            <asp:BoundField DataField="Clave" HeaderText="Clave" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                            <asp:BoundField DataField="Stock" HeaderText="Stock" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                        </Columns>
                        <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                        <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle CssClass="SelectedRowSISSA" />
                    </asp:GridView>
                </ContentTemplate>
            </ajax:TabPanel>--%>
        </ajax:TabContainer>
    </div>

</body>
</html>
