<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="wuc_CartasAccesoGenerar.ascx.vb"
    Inherits="SISSAIntranet.wuc_CartasAccesoGenerar" %>
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
        <%--<asp:UpdatePanel runat="server" ID="tab_Usuarios">
            <ContentTemplate>--%>
                <table class="tablaSISSA1" style="width: 650px">
                    <tr>
                        <td class="celda14" style="width: 100px">
                            <asp:Label ID="lbl_Tipo" runat="server" Text="Tipo"></asp:Label>
                        </td>
                        <td style="width: 400px" class="celdaMargenDer10">
                            <asp:RadioButton ID="rdb_Empleado" runat="server" Text="Empleado" GroupName="grp_Tipo" AutoPostBack="true"/>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdb_Visitante" runat="server" Text="Visitante" GroupName="grp_Tipo" AutoPostBack="true"/>
                        </td>
                        <td style="width: 100px">
                        </td>
                    </tr>
                    <tr>
                        <td class="celda14">
                            <asp:Label ID="lbl_Empleado" runat="server" Text="Empleado"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:DropDownList runat="server" ID="ddl_Empleado" Width="356px" CssClass="DropDownList18"
                                DataValueField="Id_Empleado" DataTextField="Nombre" Enabled="false">
                            </asp:DropDownList>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="celda14">
                            <asp:Label ID="lbl_Nombre" runat="server" Text="Nombre" Enabled="false"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:TextBox runat="server" ID="tbx_Nombre" Width="350px" CssClass="tbx_Mayusculas" Enabled="false" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="celda14">
                            <asp:Label ID="lbl_Empresa" runat="server" Text="Empresa" Enabled="false"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:TextBox runat="server" ID="tbx_Empresa" Width="350px" CssClass="tbx_Mayusculas" Enabled="false" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Button runat="server" ID="btn_Agregar" Text="Agregar" Enabled="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="celdaMargenDer10" colspan="2">
                            <asp:GridView runat="server" ID="gv_Empleados" Width="100%" CssClass="gridSISSA"
                                Height="100%" AutoGenerateColumns="False" DataKeyNames="Id_Empleado">
                                <Columns>
                                    <asp:BoundField DataField="Id_Empleado" HeaderText="Id_Empleado" Visible="False">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Imagenes/EliminarEmpleado.png"
                                        ShowDeleteButton="True">
                                        <ItemStyle Width="30px" />
                                    </asp:CommandField>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                </Columns>
                                <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                                <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
        <br />
        <%--<asp:UpdatePanel ID="udp_Datos" runat="server">
            <ContentTemplate>--%>
                <table class="tablaSISSA1" style="width: 650px">
                    <tr>
                        <td align="right" style="width: 100px">
                            <asp:Label ID="lbl_Fecha" runat="server" Text="Fecha"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:TextBox ID="tbx_FechaInicio" runat="server" CssClass="CalendarTextbox" AutoPostBack="true"
                                Style="vertical-align: text-bottom" MaxLength="1"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftb_FechaInicio" runat="server" TargetControlID="tbx_FechaInicio"
                                FilterType="Custom, Numbers" ValidChars="/">
                            </ajax:FilteredTextBoxExtender>
                            <ajax:CalendarExtender runat="server" ID="cal_FechaInicio" TargetControlID="tbx_FechaInicio"
                                CssClass="calendarSISSA" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                            </ajax:CalendarExtender>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbl_TipoCarta" runat="server" Text="Tipo de Carta"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10" colspan="2">
                            <asp:RadioButton ID="rdb_NuevoIngreso" runat="server" Text="Nuevo Ingreso" GroupName="grp_TipoCarta" AutoPostBack="true"/>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdb_FaltaRetardo" runat="server" Text="Acceso por Falta o Retardo" GroupName="grp_TipoCarta" AutoPostBack="true"/>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdb_Externo" runat="server" Text="Personal Externo" GroupName="grp_TipoCarta" AutoPostBack="true"/>&nbsp;&nbsp;
                            <asp:RadioButton ID="rdb_Otro" runat="server" Text="Otro" GroupName="grp_TipoCarta" AutoPostBack="true"/>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="right">
                            <asp:Label ID="lbl_Asunto" runat="server" Text="Asunto"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10" colspan="2">
                            <asp:TextBox ID="tbx_Asunto" runat="server" CssClass="tbx_Mayusculas" Width="100%" Height="85px"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbl_Autoriza" runat="server" Text="Autoriza"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:DropDownList ID="ddl_Autoriza" runat="server" DataTextField="Nombre" CssClass="DropDownList18"
                                DataValueField="Id_Empleado" Width="350px" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbl_Dirigida" runat="server" Text="Dirigida A"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:DropDownList ID="ddl_Dirigida" runat="server" DataTextField="Nombre" CssClass="DropDownList18"
                                DataValueField="Id_Empleado" Width="350px" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td align="right"><asp:Button runat="server" ID="btn_Guardar" Text="Guardar" /></td>
                    </tr>
                </table>
            <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
</body>
</html>
