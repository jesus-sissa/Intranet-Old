<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="wuc_PermisosSolicitud.ascx.vb"
    Inherits="SISSAIntranet.wuc_PermisosSolicitud" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../Estilos/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="Div_Principal">
        <br />
        <div id="div1">
            <asp:GridView ID="gv_Empleados" runat="server" AutoGenerateColumns="False" CssClass="gridSISSA"
                DataKeyNames="Id_Empleado" Width="99%">
                <RowStyle BackColor="White" ForeColor="#333333" />
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select"
                                ImageUrl="~/Imagenes/1rightarrow.png" Text="Seleccionar" />
                        </ItemTemplate>
                        <ItemStyle Width="15px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave">
                        <ItemStyle Width="70px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                </Columns>
                <FooterStyle BackColor="White" ForeColor="Black" />
                <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#C0A062" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
            </asp:GridView>
        </div>
        <br />
        <asp:UpdatePanel ID="udp_Fecha" runat="server">
                <ContentTemplate>
        <table class="tablaSISSA1">
            <tr>
                <td style="width: 120px; text-align: right">
                    <asp:Label ID="lbl_Fecha" runat="server" Text="Fecha"></asp:Label>
                </td>
                <td class="celdaMargenDer10">
                    <asp:TextBox ID="tbx_Fecha" runat="server" CssClass="CalendarTextbox" AutoPostBack="true"
                        Style="vertical-align: text-bottom" MaxLength="1"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbx_Fecha"
                        FilterType="Custom, Numbers" ValidChars="/">
                    </ajax:FilteredTextBoxExtender>
                    <ajax:CalendarExtender runat="server" ID="CalendarExtender1" TargetControlID="tbx_Fecha"
                        CssClass="calendarSISSA" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                    </ajax:CalendarExtender>
                </td>
            </tr>
        </table>
        </ContentTemplate>
            </asp:UpdatePanel>
        <br />
        <asp:UpdatePanel ID="udp_DatosPermiso" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table class="tablaSISSA1">
                    <tr style="text-align: right;">
                        <td style="width: 120px;">
                        </td>
                        <td align="center">
                            <asp:Label ID="lbl_JornadaActual" runat="server" Text="Actual" Font-Bold="true"></asp:Label>
                        </td>
                        <td colspan="4" align="center">
                            <asp:Label ID="lbl_JornadaNueva" runat="server" Text="Nueva" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lbl_Jornada1" runat="server" Text="Jornada 1"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:TextBox ID="tbx_Jornada1" runat="server" CssClass="CalendarTextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:Label ID="Label3" runat="server" Text="De"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:DropDownList ID="ddl_Jornada1De" runat="server" Width="70px" CssClass="DropDownList18"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:Label ID="Label8" runat="server" Text="A"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:DropDownList ID="ddl_Jornada1A" runat="server" Width="70px" CssClass="DropDownList18"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lbl_Jornada2" runat="server" Text="Jornada 2"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:TextBox ID="tbx_Jornada2" runat="server" CssClass="CalendarTextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:Label ID="Label1" runat="server" Text="De"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:DropDownList ID="ddl_Jornada2De" runat="server" Width="70px" CssClass="DropDownList18"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:Label ID="Label2" runat="server" Text="A"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:DropDownList ID="ddl_Jornada2A" runat="server" Width="70px" CssClass="DropDownList18"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <table class="tablaSISSA1">
            <tr>
                <td style="width: 120px; text-align: right" valign="top">
                    <asp:Label ID="lbl_Motivos" runat="server" Text="Motivos"></asp:Label>
                </td>
                <td class="celdaMargenDer10">
                    <asp:TextBox ID="tbx_Motivos" runat="server" Style="width: 400px; height: 100px"
                        CssClass="tbx_Mayusculas" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" />
                </td>
            </tr>
        </table>
    </div>
</body>
</html>
