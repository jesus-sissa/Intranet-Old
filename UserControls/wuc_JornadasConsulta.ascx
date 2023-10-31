<%@ Control Language="VB" AutoEventWireup="false" Inherits="SISSAIntranet.wuc_JornadasConsulta"
    CodeBehind="wuc_JornadasConsulta.ascx.vb" %>
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
        <div id="divEnca">
            <strong>Consulta de Jornadas</strong>
        </div>--%>
        <br />
        <div>
            <asp:UpdatePanel ID="udp_Filtros" runat="server">
                <ContentTemplate>
                    <table class="tablaSISSA1">
                        <tr>
                            <td style="width: 120px; text-align: right">
                                <asp:Label ID="lbl_Fecha_Inicio" runat="server" Text="Fecha Inicio"></asp:Label>
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:TextBox ID="tbx_FechaIni" runat="server" CssClass="CalendarTextbox"
                                    AutoPostBack="true" Style="vertical-align: text-bottom" MaxLength="1" ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbx_FechaIni"
                                    FilterType="Custom, Numbers" ValidChars="/">
                                </ajax:FilteredTextBoxExtender>
                                <ajax:CalendarExtender runat="server" ID="CalendarExtender1" TargetControlID="tbx_FechaIni"
                                    CssClass="calendarSISSA" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                                </ajax:CalendarExtender>
                            </td>
                            <td style="width: 30px;"></td>
                            <td style="width: 100px;"></td>
                            <td align="right" style="width: 70px">
                                <asp:Label ID="Lbl_FechaFin" runat="server" Text="Fecha Fin"></asp:Label>
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:TextBox ID="tbx_FechaFin" runat="server" CssClass="CalendarTextbox"
                                    AutoPostBack="true" MaxLength="1" ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbx_FechaFin"
                                    FilterType="Custom, Numbers" ValidChars="/">
                                </ajax:FilteredTextBoxExtender>
                                <ajax:CalendarExtender runat="server" ID="CalendarExtender2" TargetControlID="tbx_FechaFin"
                                    CssClass="calendarSISSA" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                                </ajax:CalendarExtender>
                            </td>
                            <td style="width: 30px"></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td style="width: 120px; text-align: right">
                                <asp:Label ID="Label1" runat="server" Text="Empleado"></asp:Label>
                            </td>
                            <td class="celdaMargenDer10" colspan="6" style="width: 406px">
                                <asp:DropDownList ID="ddl_Empleado" runat="server" DataTextField="CveNombre" CssClass="DropDownList18"
                                    AutoPostBack="true" DataValueField="Id_Empleado" Width="406px" Enabled="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_Empleados" runat="server" CssClass="Gera1" Text="Todos" AutoPostBack="true"
                                    Checked="False" />
                            </td>
                            <td align="left">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lbl_Tipo" runat="server" Text="Tipo Falta"></asp:Label>
                            </td>
                            <td class="celdaMargenDer10" colspan="6">
                                <asp:DropDownList ID="ddl_TipoFalta" runat="server" CssClass="DropDownList18"
                                    AutoPostBack="true" Width="406px" Enabled="False">
                                    <asp:ListItem Value="0" Text="Seleccione..."></asp:ListItem>
                                    <asp:ListItem Value="1" Text="FALTA"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="DESCANSO"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="VACACIONES"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="INCAPACIDAD"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_TipoFalta" runat="server" CssClass="Gera1" Text="Todos" AutoPostBack="True"
                                    Checked="True" />
                            </td>
                            <td>
                                <asp:Button ID="btn_Mostrar" runat="server" Text="Mostrar" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <br />
        <asp:UpdatePanel ID="udp_Lista" runat="server">
            <ContentTemplate>
                <asp:Panel runat="server" ID="pnl_Jornadas">
                    <%--<fieldset style="width: auto; height: auto">--%>
                    <div id="div_Scrolls" style="width: 100%; height: 600px; overflow: auto;">
                        <asp:GridView ID="gv_Jornadas" runat="server" AutoGenerateColumns="False" CssClass="gridSISSA"
                            DataKeyNames="Id_Jornada,TipoFalta" Width="100%">
                            <RowStyle BackColor="White" ForeColor="#333333" />
                            <Columns>
                                <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave">
                                    <ItemStyle Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre">
                                    <ItemStyle Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Dia" HeaderText="Dia" SortExpression="Dia">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Jornada1" HeaderText="Jornada1">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Jornada2" HeaderText="Jornada2">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Turno" HeaderText="Turno">
                                    <ItemStyle Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="A" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle Width="30" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="F" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle Width="30" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="D" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle Width="30" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="V" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle Width="30" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="I" HeaderStyle-HorizontalAlign="Center">
                                    <ItemStyle Width="30" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Checada1" HeaderText="Checada1">
                                    <ItemStyle Width="30" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Checada2" HeaderText="Checada2">
                                    <ItemStyle Width="30" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Checada3" HeaderText="Checada3">
                                    <ItemStyle Width="30" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Checada4" HeaderText="Checada4">
                                    <ItemStyle Width="30" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Retardo" HeaderText="Retardo">
                                    <ItemStyle Width="30" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="HorasExtra" HeaderText="H.Extra">
                                    <ItemStyle Width="30" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Recupera" HeaderText="Recupera">
                                    <ItemStyle Width="30" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TipoFalta" HeaderText="TipoFalta" Visible="False" />
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="Black" />
                            <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle Font-Bold="True" ForeColor="Red" />
                            <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                        </asp:GridView>
                        <%--</fieldset>--%>
                    </div>
                    <br />
                    <table style="width: 100%">
                        <tr>
                            <td align="right">
                                <asp:Button ID="btn_Cerrar" runat="server" Text="Cerrar" Width="80px" />
                                <asp:Button ID="MpeFakeTarget" runat="server" CausesValidation="False" Style="display: none" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <ajax:ModalPopupExtender runat="server" ID="mpe_Consulta" TargetControlID="MpeFakeTarget"
                    BackgroundCssClass="modalBackground" PopupControlID="pnl_Jornadas" CancelControlID="btn_Cerrar" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</body>
</html>
