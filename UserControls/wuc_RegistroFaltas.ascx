<%@ Control Language="VB" AutoEventWireup="false"
    Inherits="SISSAIntranet.wuc_RegistroFaltas" Codebehind="wuc_RegistroFaltas.ascx.vb" %>
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
            <strong>Registro de Faltas</strong>
        </div>--%>
        <br />
        <div>
            <asp:UpdatePanel runat="server" ID="udp_Empleado">
                <ContentTemplate>
                    <table class="tablaSISSA1">
                        <tr>
                            <td align="right">
                                <asp:Label ID="lbl_Empleado" runat="server" Text="Empleado"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="tbx_Clave" Width="50px" AutoPostBack="true" CssClass="textbox1" MaxLength="6"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbx_Clave"
                                    FilterType="Numbers" Enabled="True">
                                </ajax:FilteredTextBoxExtender>
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:DropDownList ID="ddl_Empleado" runat="server" DataTextField="CveNombre" CssClass="DropDownList18"
                                    DataValueField="Id_Empleado" Width="406px" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:Button ID="btn_Mostrar" runat="server" Text="Mostrar" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
            <br />
        <div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <fieldset style="width:750px; height:auto">
                        <table>
                            <tr>
                                <td class="celdaMargenDer10">
                                    <asp:Label ID="Label1" runat="server" Text="Las Jornadas mostradas corresponden al rango:"
                                        CssClass="textbox1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="celdaMargenDer10">
                                    <asp:Label ID="Label2" runat="server" Text="Inicio: 5 dias anteriores a la fecha actual." CssClass="textbox1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="celdaMargenDer10">
                                    <asp:Label ID="Label3" runat="server" Text="Final: 30 dias posteriores a la fecha actual." CssClass="textbox1"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gv_Jornadas" runat="server" AutoGenerateColumns="False" CssClass="gridSISSA"
                            DataKeyNames="Id_Jornada,TipoFalta" Width="750px">
                            <RowStyle BackColor="White" ForeColor="#333333" />
                            <Columns>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Dia" HeaderText="Dia" SortExpression="Dia">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Jornada1" HeaderText="Jornada1">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Jornada2" HeaderText="Jornada2">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Turno" HeaderText="Turno">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="A" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_Asistencia" runat="server" OnCheckedChanged="ValidaChecks" AutoPostBack="true" />
                                    </ItemTemplate>
                                    <ItemStyle Width="30px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="F" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_Falta" runat="server" OnCheckedChanged="ValidaChecks" AutoPostBack="true" />
                                    </ItemTemplate>
                                    <ItemStyle Width="30px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="D" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_Descanso" runat="server" OnCheckedChanged="ValidaChecks" AutoPostBack="true"/>
                                    </ItemTemplate>
                                    <ItemStyle Width="30px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="V" HeaderStyle-HorizontalAlign="Center">
                                   <ItemTemplate>
                                        <asp:CheckBox ID="chk_Vacaciones" runat="server" OnCheckedChanged="ValidaChecks" AutoPostBack="true"/>
                                    </ItemTemplate>
                                    <ItemStyle Width="30px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="I" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_Incapacidad" runat="server" OnCheckedChanged="ValidaChecks" AutoPostBack="true"/>
                                    </ItemTemplate>
                                    <ItemStyle Width="30px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="TipoFalta" HeaderText="TipoFalta" 
                                    Visible="False" />
                                <asp:TemplateField HeaderText="Retraso" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbx_MinutosRetraso" runat="server" Width="40px" />
                                        <ajax:FilteredTextBoxExtender ID="ftb_MinutosRetraso" runat="server" TargetControlID="tbx_MinutosRetraso"
                                        FilterType="Custom, Numbers">
                                    </ajax:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="HrsEx" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbx_HorasExtra" runat="server" Width="40px" />
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Recupera" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="tbx_FechaRecupera" runat="server" Width="75px" Height="22px" CssClass="textbox1"
                                        AutoPostBack="true" Style="vertical-align: text-bottom" MaxLength="1" ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbx_FechaRecupera"
                                        FilterType="Custom, Numbers" ValidChars="/">
                                    </ajax:FilteredTextBoxExtender>
                                    <ajax:CalendarExtender runat="server" ID="CalendarExtender3" TargetControlID="tbx_FechaRecupera"
                                        CssClass="calendarSISSA" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                                    </ajax:CalendarExtender>
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="Black" />
                            <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle Font-Bold="True" ForeColor="Red" />
                            <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                        </asp:GridView>
                        <br />
                        <table class="tablaSISSA1" width="100%">
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</body>
</html>
