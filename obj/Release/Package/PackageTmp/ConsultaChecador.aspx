<%@ Page Title="CONSULTA DE HORAS CHECADAS" Language="VB" AutoEventWireup="false" Inherits="IntranetSIAC.ConsultaChecador"
    ValidateRequest="false" MasterPageFile="~/MP_Principal.master" CodeBehind="ConsultaChecador.aspx.vb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="Div_Principal" runat="server">

        <div runat="server" style="width: 68%; float: left;">
            <asp:UpdatePanel ID="udp_filtroHC" runat="server">
                <ContentTemplate>

                    <table class="tablaSISSA1" runat="server">
                        <tr>
                            <td style="width: 100px; text-align: right">
                                <asp:Label ID="lbl_Fecha_Inicio" runat="server" Text="Fecha Inicio"></asp:Label>
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:TextBox ID="tbx_FechaIni" runat="server" CssClass="calendarioAjax"
                                    AutoPostBack="true" Style="vertical-align: text-bottom" MaxLength="1" ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario"></asp:TextBox>

                                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbx_FechaIni"
                                    FilterType="Custom, Numbers" ValidChars="/">
                                </ajax:FilteredTextBoxExtender>

                                <ajax:CalendarExtender runat="server" ID="CalendarExtender1" TargetControlID="tbx_FechaIni"
                                    CssClass="calendarioAjax" Format="dd/MM/yyyy"
                                    TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                                </ajax:CalendarExtender>
                            </td>
                            <td></td>

                            <td></td>
                            <td style="text-align: right; font-size: x-small;">
                                <asp:Label ID="Lbl_FechaFin" runat="server" Text="Fecha Fin"></asp:Label>

                                <asp:TextBox ID="tbx_FechaFin" runat="server" CssClass="calendarioAjax"
                                    AutoPostBack="true" MaxLength="1" ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbx_FechaFin"
                                    FilterType="Custom, Numbers" ValidChars="/">
                                </ajax:FilteredTextBoxExtender>
                                <ajax:CalendarExtender runat="server" ID="CalendarExtender2" TargetControlID="tbx_FechaFin"
                                    CssClass="calendarioAjax" Format="dd/MM/yyyy"
                                    TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                                </ajax:CalendarExtender>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right">
                                <asp:Label ID="Label1" runat="server" Text="Empleado"></asp:Label>
                            </td>
                            <td class="celdaMargenDer10" colspan="4" style="width: 406px">
                                <asp:DropDownList ID="ddl_Empleado" runat="server" DataTextField="CveNombre" CssClass="DropDownList18"
                                    AutoPostBack="true" DataValueField="Id_Empleado" Width="406px" Enabled="true">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 64px">
                                <asp:CheckBox ID="chk_Empleados" runat="server" Text="Todos" AutoPostBack="true"
                                    Checked="False" />

                            </td>
                            <td align="left" style="width: 90px;">
                                <asp:Button ID="btn_Mostrar" runat="server" Text="Mostrar" Width="90px" CssClass="buttonB" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div id="div_GrillaHC" style="float: left; width: 100%;">
            <asp:UpdatePanel ID="udp_listaHC" runat="server">
                <ContentTemplate>

                    <asp:GridView ID="gv_horasChec" runat="server" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="50"
                        CssClass="gv_general">

                        <Columns>
                            <asp:BoundField HeaderText="Clave" DataField="Clave" />
                            <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                            <asp:BoundField HeaderText="Departamento" DataField="Departamento" />
                            <asp:BoundField HeaderText="Puesto" DataField="Puesto" />
                            <asp:BoundField HeaderText="Fecha" DataField="Fecha" />
                            <asp:BoundField HeaderText="Hora" DataField="Hora" />
                        </Columns>

                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                        <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                    </asp:GridView>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="float: left; text-align: center; width: 100%;">
            <asp:Button ID="btn_Exportar" runat="server" Text="Exportar" Width="90px" CssClass="button" />
        </div>
    </div>
</asp:Content>




