<%@ Control Language="VB" AutoEventWireup="false"
    Inherits="SISSAIntranet.UserControls_wuc_RIASeguimiento" Codebehind="wuc_RIASeguimiento.ascx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../Estilos/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>

    <script type="text/javascript" language="javascript">
        // Abrir en ventana sin barra de scroll

        function AbrirSinScroll(url) {
            web = url
            alto = 600
            ancho = 800
            izq = (screen.width - ancho) / 2
            arr = ((screen.height - alto) / 2) - 15
            popupWin = window.open(web, "_blank", "scroll='no',width=" + ancho + ",height=" + alto + ",top=" + arr + ",left=" + izq + ",titlebar='no',Toolbar='no',Status='yes',Location='no'")
        }
    </script>

    <div class="Div_Principal">
        <%--<br />
        <div id="divEnca" title="prueba">
            <strong>Seguimiento de Reportes de Incidentes/Accidentes</strong>
        </div>--%>
        <br />
        <div>
            <asp:GridView ID="gv_RIA" runat="server" AutoGenerateColumns="False" DataKeyNames="Id_RIA,Sucursal,Status"
                AllowPaging="True" Width="100%" Style="color: Black; font-family: Verdana; font-size: x-small">
                <Columns>
                    <asp:BoundField DataField="Id_RIA" HeaderText="RIAID" Visible="False">
                        <ItemStyle HorizontalAlign="Right" Wrap="True" />
                    </asp:BoundField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select"
                                ImageUrl="~/Imagenes/1rightarrow.png" Text="Seleccionar" />
                        </ItemTemplate>
                        <ItemStyle Width="15px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Numero" HeaderText="Num">
                        <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Sucursal" HeaderText="Sucursal">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Hora" HeaderText="Hora">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Tipo" HeaderText="Tipo">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="240px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Entidad" DataField="Entidad">
                        <ItemStyle Width="240px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Status">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" />
                    </asp:BoundField>
                </Columns>
                <SelectedRowStyle CssClass="SelectedRowSISSA" />
                <%--                BackColor="#C0A062" Font-Bold="True" ForeColor="White"--%>
                <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
            </asp:GridView>
            <br />
            <asp:GridView runat="server" ID="gv_Usuarios" Width="57%" CssClass="gridSISSA" AutoGenerateColumns="False"
                DataKeyNames="Id_Entidad">
                <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                <Columns>
                    <asp:BoundField DataField="Id_Entidad" HeaderText="Id_Entidad" Visible="False">
                        <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Nombre" HeaderText="Usuarios para Seguimiento" />
                </Columns>
                <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
            </asp:GridView>
            <br />
            <asp:PlaceHolder runat="server" ID="PlaceHolderSeguimiento"></asp:PlaceHolder>            
            <br />
            <table class="tablaSISSA1">
                <tr>
                    <td style="width: 100px; text-align: right">
                        <asp:Label ID="lbl_UsuarioA" runat="server" Text="Asignar a Usuario" Enabled="false"></asp:Label>
                    </td>
                    <td class="celdaMargenDer10" colspan="2">
                        <asp:DropDownList ID="ddl_UsuarioA" runat="server" DataTextField="Nombre" CssClass="DropDownList18"
                            DataValueField="Id_Empleado" Width="350px" Enabled="False">
                        </asp:DropDownList>
                    </td>
                    <%--                BackColor="#C0A062" Font-Bold="True" ForeColor="White"--%>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lbl_TipoUsuario" runat="server" Text="Tipo Usuarios" Enabled="False"></asp:Label>
                    </td>
                    <td class="celdaMargenDer10">
                        <asp:RadioButton ID="rdb_Principal" runat="server" Text="Principal" GroupName="TipoU"
                            Enabled="False" />
                        &nbsp;&nbsp;<asp:RadioButton ID="rdb_Secundario" runat="server" Text="Secundario"
                            GroupName="TipoU" Enabled="False" />
                    </td>
                    <td align="right">
                        <asp:Button ID="btn_Asignar" runat="server" Text="Asignar" Enabled="false" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div style="float: left; border-style: solid; border-color: Gray; border-width: thin;
            border-top: none">
            <div id="divEncaComentarios" title="prueba">
                <strong>Capture Comentarios</strong>
            </div>
            <%--                BackColor="#C0A062" Font-Bold="True" ForeColor="White"--%>
            <asp:UpdatePanel runat="server" ID="udp_Comentarios" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnl_Comentarios">
                        <table class="tablaSISSA1">
                            <tr>
                                <td style="width: 120px; height: 18px; text-align: right; vertical-align: bottom">
                                    <asp:Label ID="lbl_Fecha_Inicio" runat="server" Text="Fecha Seguimiento"></asp:Label>
                                </td>
                                <td class="celdaMargenDer10" style="width: 75px">
                                    <asp:TextBox ID="tbx_FechaSeguimiento" runat="server" CssClass="CalendarTextbox" 
                                        ToolTip="No es la fecha actual. Es la Fecha en la que ocurrió el evento que se está capturando."
                                        AutoPostBack="true" Style="vertical-align: bottom" MaxLength="1"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbx_FechaSeguimiento"
                                        FilterType="Custom, Numbers" ValidChars="/">
                                    </ajax:FilteredTextBoxExtender>
                                    <ajax:CalendarExtender runat="server" ID="CalendarExtender1" TargetControlID="tbx_FechaSeguimiento"
                                        CssClass="calendarSISSA" Format="dd/MM/yyyy" 
                                        TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                                    </ajax:CalendarExtender>
                                </td>
                                <td style="width: 15px; vertical-align: bottom">
                                </td>
                                <td style="text-align: right; width: 100px">
                                    <asp:Label ID="lbl_Hora_Seguimiento" runat="server" Text="Hora Seguimiento"></asp:Label>
                                </td>
                                <td class="celdaMargenDer10">
                                    <asp:DropDownList runat="server" ID="ddl_HoraSeguimiento" Width="45px" CssClass="DropDownList18"
                                        
                                        ToolTip="No es la Hora actual. Es la Hora en la que ocurrió el evento que se está capturando.">
                                    </asp:DropDownList>
                                    <label style="font-weight: bold">
                                        :</label>
                                    <asp:DropDownList runat="server" ID="ddl_MinSeguimiento" Width="45px" CssClass="DropDownList18"
                                        
                                        ToolTip="No es la Hora actual. Es la Hora en la que ocurrió el evento que se está capturando.">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px; text-align: right; vertical-align: top">
                                    <asp:Label ID="lbl_Descripcion" runat="server" Text="Descripción"></asp:Label>
                                </td>
                                <td class="celdaMargenDer10" colspan="4">
                                    <asp:TextBox ID="tbx_Descripcion" runat="server" Width="390px" Height="85px" TextMode="MultiLine"
                                        CssClass="textbox1" Style="text-transform: uppercase"></asp:TextBox>
                                </td>
                                <asp:RequiredFieldValidator ID="rfv_Descripcion" ControlToValidate="tbx_Descripcion"
                                    ValidationGroup="Val1" runat="server" ErrorMessage="Capture la Descripción."
                                    ForeColor="Red">*</asp:RequiredFieldValidator>
                            </tr>
                        </table>
                    </asp:Panel>
                    <%--<br />--%>
                    <asp:Panel ID="pnl_AgregarI" runat="server">
                        <table class="tablaSISSA3">
                            <tr>
                                <td style="width: 120px; text-align: left">
                                </td>
                                <td class="celdaMargenDer10" style="width: 400px; text-align: left" colspan="2">
                                    <asp:Label ID="lbl_Mensaje" runat="server" Style="font-size: x-small; font-weight: bold"
                                        Text="Si lo desea puede agregar una Imagen a su Reporte."></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px; text-align: left">
                                </td>
                                <td class="celdaMargenDer10" style="width: 380px; text-align: left" colspan="2">
                                    <asp:Label ID="Label5" runat="server" Style="font-size: x-small; font-weight: bold"
                                        Text="Las Imagenes deben ser menores de 300 kb"></asp:Label>
                                </td>
                            </tr>
                            <%--<tr>
                                <td style="height: 25px">
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>--%>
                            <tr>
                                <td style="width: 120px; text-align: right">
                                    <asp:Label ID="lbl_Imagen" runat="server" Text="Agregar Imagen"></asp:Label>
                                </td>
                                <td class="celdaMargenDer10" colspan="2">
                                    <asp:FileUpload ID="FileUpload1" runat="server" Width="410px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <%--<td class="celdaMargenDer10">
                                    <asp:Button ID="btn_Subir" Text="Guardar" Width="100px" runat="server"></asp:Button>
                                    <asp:HiddenField ID="hfd_IDRIA" runat="server" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_Finalizar" runat="server" Width="100px" Text="Finalizar" />
                                    <asp:HiddenField runat="server" ID="hfd_IDRIAD" />
                                </td>--%>
                            </tr>
                            <tr>
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
            </asp:UpdatePanel>
            <table>
                <tr>
                    <td style="width: 120px; text-align: right">
                    </td>
                    <td class="celdaMargenDer10">
                        <asp:Button ID="btn_Subir" Text="Guardar" Width="100px" runat="server" Enabled="False"></asp:Button>
                        <asp:HiddenField ID="hfd_IDRIA" runat="server" />
                    </td>
                    <td align="left">
                        <asp:Button ID="btn_Finalizar" runat="server" Width="100px" Text="Finalizar" Enabled="False"/>
                        <asp:HiddenField runat="server" ID="hfd_IDRIAD" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="float: right">
            <asp:GridView runat="server" ID="gv_Imagenes" AutoGenerateColumns="False" DataKeyNames="ID_RIAI"
                Style="margin-top: 32px" CssClass="gridSISSA" Width="20%">
                <Columns>
                    <asp:BoundField DataField="Id_RIAI" HeaderText="ID" Visible="False" />
                    <asp:BoundField DataField="Id_RIA" HeaderText="IDRIA" Visible="False" />
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                        <ItemStyle VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Imagen" FooterText="EJEMPLO">
                        <ItemTemplate>
                            <a href="javascript:;" onclick="AbrirSinScroll('MostrarImagenes.aspx?ID=<%# Eval("ID_RIAI")%>');">
                                <asp:Image ID="Image1" runat="server" Height="150px" Width="150px" ImageUrl='<%# "~/MostrarImagenes.ashx?ID=" & Eval("Id_RIAI") %>' />
                            </a>
                        </ItemTemplate>
                        <ItemStyle Height="150px" Width="150px" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
            </asp:GridView>
        </div>
    </div>
</body>
</html>
