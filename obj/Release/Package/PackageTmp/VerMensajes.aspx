<%@ Page Title="Ver Mensajes" Language="VB" MasterPageFile="~/MP_Principal.master" AutoEventWireup="false" Inherits="IntranetSIAC.VerMensajes" Culture="Auto" UICulture="Auto" CodeBehind="VerMensajes.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <br />
    <div style="float: left; width: 400px">
              <table style="width: 100%; font-family: Verdana; font-size: x-small">
            <tr>
                <td style="width: 95px" align="right">
                    <asp:Label ID="label7" runat="server" Text="Fecha Inicio"></asp:Label>
                </td>
                <td class="celdaMargenDer10" style="width: 70px">
                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbx_FechaIni"
                        FilterType="Custom, Numbers" ValidChars="/">
                    </ajax:FilteredTextBoxExtender>
                    <asp:TextBox runat="server" ID="tbx_FechaIni" Height="14px" AutoPostBack="true"
                        MaxLength="10" Style="width: 70px; font-family: Verdana; font-size: x-small"></asp:TextBox>
                    <ajax:CalendarExtender runat="server" ID="CalendarExtender1" TargetControlID="tbx_FechaIni"
                        CssClass="calendarioAjax" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                    </ajax:CalendarExtender>
                </td>
                <td style="width: 70px" align="right">
                    <asp:Label ID="label8" runat="server" Text="Fecha Fin"></asp:Label>
                </td>
                <td class="celdaMargenDer10" style="width: 101px">
                    <asp:TextBox runat="server" ID="tbx_FechaFin" Style="width: 70px; font-family: Verdana; font-size: x-small"
                        Height="14px" AutoPostBack="True"></asp:TextBox>
                    <ajax:CalendarExtender runat="server" ID="CalendarExtender2" TargetControlID="tbx_FechaFin"
                        CssClass="calendarioAjax" Format="dd/MM/yyyy">
                    </ajax:CalendarExtender>
                </td>
                <td style="width: 70px">
                    <asp:CheckBox runat="server" ID="chk_TodasF" Text="Todos" Checked="False" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td align="right" style="font-family: Verdana; font-size: x-small">
                    <asp:Label ID="lbl_Usuario" runat="server" Text="Remitente"></asp:Label>
                </td>
                <td class="celdaMargenDer10" colspan="3">
                    <asp:DropDownList runat="server" ID="ddl_Usuario" Width="245px" Style="font-family: Verdana; font-size: x-small">
                    </asp:DropDownList>
                </td>
                <td style="width: 105px">
                    <asp:CheckBox runat="server" ID="chk_Usuario" Text="Todos" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td style="width: 70px"></td>
                <td>&nbsp;</td>
                <td align="right" style="width: 101px">
                    <asp:Button runat="server" ID="btn_Mostrar" Text="Mostrar" Width="90px" CssClass="buttonB" />
                </td>
                <td></td>
            </tr>
        </table>
     
        <br />
        <ajax:TabContainer runat="server" Width="100%" ActiveTabIndex="0" Height="490px"
            AutoPostBack="true" ID="tbc_Mensajes">
            <ajax:TabPanel runat="server" ID="tab_Recibidos" HeaderText="Recibidos" Width="100%">
                <HeaderTemplate>
                    Recibidos
                </HeaderTemplate>
                <ContentTemplate>
                    <table style="width: 100%; font-family: Verdana; font-size: x-small">
                        <tr>
                            <td>
                                <asp:CheckBox runat="server" ID="chk_Status" Text="Mostrar sólo Pendientes" Checked="True"
                                    AutoPostBack="True" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView runat="server" ID="gv_Recibidos" Width="100%" CssClass="gv_general"
                                    AutoGenerateColumns="False" DataKeyNames="Id_Mensaje" AllowPaging="True" PageSize="15">
                                    <Columns>
                                        <asp:BoundField DataField="Id_Mensajes" HeaderText="Id_Mensajes" Visible="False">
                                            <ItemStyle Width="50px" />
                                        </asp:BoundField>
                                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/mail_recibido.png"
                                            ShowSelectButton="True">
                                            <ControlStyle Height="15px" />
                                        </asp:CommandField>
                                        <asp:BoundField DataField="Fecha de Registro" HeaderText="Fecha Registro">
                                            <ItemStyle Width="90px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Asunto" HeaderText="Asunto" Visible="False" />
                                        <asp:BoundField DataField="Remitente" HeaderText="Remitente" />
                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" Visible="False" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                    </Columns>

                                    <RowStyle CssClass="rowHover" />
                                    <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                                    <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                                    <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </ajax:TabPanel>
            <ajax:TabPanel runat="server" ID="tab_Enviados" HeaderText="Enviados" Width="100%">
                <HeaderTemplate>
                    Enviados
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:GridView runat="server" ID="gv_Enviados" Width="100%"
                        CssClass="gv_general" AutoGenerateColumns="False"
                        DataKeyNames="Id_Mensaje" AllowPaging="True" PageSize="15">
                        <Columns>
                            <asp:BoundField DataField="Clave" HeaderText="Id_Mensaje,Asunto" Visible="False">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/mail_enviado.png"
                                ShowSelectButton="True" />
                            <asp:BoundField DataField="Fecha de Registro" HeaderText="Fecha Registro">
                                <ItemStyle Width="90px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Asunto" HeaderText="Asunto" Visible="False" />
                            <asp:BoundField DataField="Destinatario" HeaderText="Destinatario" />
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                        </Columns>

                        <RowStyle CssClass="rowHover" />
                        <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                        <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                    </asp:GridView>
                </ContentTemplate>
            </ajax:TabPanel>
        </ajax:TabContainer>
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:Button runat="server" ID="btn_Atendido" Text="Atendido" Width="100px" Enabled="false" CssClass="button" />
                </td>
            </tr>
        </table>
    </div>
    <div style="float: left; width: 390px; margin-left: 15px">
        <table style="width: 100%; font-family: Verdana; font-size: x-small">
            <tr>
                <td align="right" style="width: 60px">
                    <asp:Label ID="Label4" runat="server" Text="Fecha"></asp:Label>
                </td>
                <td class="celdaMargenDer10" style="width: 80px">
                    <asp:TextBox ID="tbx_FechaReg" runat="server" Style="width: 70px; font-family: Verdana; font-size: x-small"
                        ReadOnly="true"></asp:TextBox>
                </td>
                <td align="right" style="width: 40px">
                    <asp:Label ID="Label5" runat="server" Text="Hora"></asp:Label>
                </td>
                <td class="celdaMargenDer10" style="width: 220px">
                    <asp:TextBox ID="tbx_Hora" runat="server" Style="width: 50px; font-family: Verdana; font-size: x-small"
                        ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 60px">
                    <asp:Label ID="Label2" runat="server" Text="Asunto"></asp:Label>
                </td>
                <td colspan="3" class="celdaMargenDer10">
                    <asp:TextBox runat="server" ID="tbx_Asunto" Width="100%" ReadOnly="true" Style="font-family: Verdana; font-size: x-small"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="vertical-align: top">
                    <asp:Label ID="Label3" runat="server" Text="Mensaje"></asp:Label>
                </td>
                <td style="height: 260px" colspan="3" class="celdaMargenDer10">
                    <asp:TextBox ID="tbx_Mensaje" runat="server" TextMode="MultiLine" Style="width: 100%; height: 260px; font-family: Verdana; font-size: x-small"
                        ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <table style="width: 100%; font-family: Verdana; font-size: x-small">
            <tr>
                <td align="right" style="vertical-align: top; width: 60px">
                    <asp:Label ID="Label6" runat="server" Text="Respuesta"></asp:Label>
                </td>
                <td style="height: 278px;" colspan="3" class="celdaMargenDer10">
                    <asp:TextBox ID="tbx_MensajeR" runat="server" TextMode="MultiLine" Style="width: 100%; height: 278px; text-transform: uppercase; font-family: Verdana; font-size: x-small"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td align="left" class="celdaMargenDer10">
                    <asp:Button runat="server"  ID="btn_Responder" Text="Responder" Width="100px" Enabled="false" CssClass="button" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

