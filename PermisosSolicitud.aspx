<%@ Page Title="CAMBIOS EN HORARIOS DE TRABAJO" Language="vb" MasterPageFile="~/MP_Principal.master" AutoEventWireup="false"
    CodeBehind="PermisosSolicitud.aspx.vb" Inherits="IntranetSIAC.PermisosSolicitud" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="ContentPermisos" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="Div_Principal">
        <br />
        <div id="div1">
            <asp:GridView ID="gv_Empleados" runat="server"
                AutoGenerateColumns="False" CssClass="gv_general"
                DataKeyNames="Id_Empleado" Width="99%">

                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select"
                                ImageUrl="~/Imagenes/1rightarrow.png" Text="Seleccionar" />
                        </ItemTemplate>
                        <ItemStyle Width="15px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="70px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre">
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
                <RowStyle CssClass="rowHover" />
                <SelectedRowStyle CssClass="FilaSeleccionada_gv" />

                <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
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
                            <asp:TextBox ID="tbx_Fecha" runat="server" CssClass="calendarioAjax" AutoPostBack="true"
                                Style="vertical-align: text-bottom" MaxLength="1"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbx_Fecha"
                                FilterType="Custom, Numbers" ValidChars="/">
                            </ajax:FilteredTextBoxExtender>
                            <ajax:CalendarExtender runat="server" ID="CalendarExtender1" TargetControlID="tbx_Fecha"
                                CssClass="calendarioAjax" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
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
                        <td style="width: 120px;"></td>
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
                        <td class="celdaMargenDer10" style="width: 100px">
                            <asp:TextBox ID="tbx_Jornada1" runat="server" CssClass="calendarioAjax" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:Label ID="Label3" runat="server" Text="De"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            <asp:DropDownList ID="ddl_Jornada1De" runat="server" Width="70px" CssClass="DropDownList18"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:Label ID="Label8" runat="server" Text="A"></asp:Label>
                        </td>
                        <td style="width: 100px">
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
                            <asp:TextBox ID="tbx_Jornada2" runat="server" CssClass="calendarioAjax" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:Label ID="Label1" runat="server" Text="De"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_Jornada2De" runat="server" Width="70px" CssClass="DropDownList18"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:Label ID="Label2" runat="server" Text="A"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_Jornada2A" runat="server" Width="70px" CssClass="DropDownList18"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 120px; text-align: right" valign="top">
                            <asp:Label ID="lbl_Motivos" runat="server" Text="Motivo"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10" colspan="5">
                            <asp:TextBox ID="tbx_Motivos" runat="server" Style="width: 400px; height: 100px"
                                CssClass="tbx_Mayusculas" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="5" class="celdaMargenDer10">
                            <asp:Label ID="Label4" runat="server" Text="Describa claramente el Motivo de la Solicitud."></asp:Label>

                        </td>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="Label5" runat="server" Text="Contraseña"></asp:Label>
                            </td>
                            <td style="text-align: left" class="celdaMargenDer10" colspan="5">
                                <asp:TextBox ID="tbx_Contrasena" runat="server" TextMode="Password" CssClass="textbox1"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="5" class="celdaMargenDer10">
                                <asp:Label ID="Label6" runat="server" Text="Nota: La firma electrónica a través de INTRANET SIAC, según las"></asp:Label>

                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="5" class="celdaMargenDer10">
                                <asp:Label ID="Label7" runat="server" Text="políticas de la Empresa, tiene la misma validez que una firma manuscrita."></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="right" colspan="5">
                                <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" Enabled="false" CssClass="buttonB"
                                    Style="height: 26px" Width="90px" />
                            </td>
                        </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <%--<table class="tablaSISSA1">
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
                    <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" Enabled="false" />
                </td>
            </tr>
        </table>--%>
    </div>
</asp:Content>
