<%@ Page Title="Enviar Mensajes" Language="VB" MasterPageFile="~/MP_Principal.master"
    AutoEventWireup="false" Inherits="IntranetSIAC.EnviarMensajes" CodeBehind="EnviarMensajes.aspx.vb" %>

<%--<%@ Register Src="~/UserControls/wuc_EnviarMensajes.ascx" TagName="EnviarMensajes" TagPrefix="uc1" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">

        function SelectAllCheckboxes(spanChk) {

            // Added as ASPX uses SPAN for checkbox
            var oItem = spanChk.children;
            var theBox = (spanChk.type == "checkbox") ?
                spanChk : spanChk.children.item[0];
            xState = theBox.checked;
            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++)
                if (elm[i].type == "checkbox" &&
                  elm[i].id != theBox.id) {
                    //elm[i].click();
                    if (elm[i].checked != xState)
                        elm[i].click();
                    //elm[i].checked=xState;
                }
        }
    </script>

    <%--<uc1:EnviarMensajes ID="uc_MensajesEnviar" runat="server" />--%>
    <br />
    <div style="float: left; width: 400px">
        <ajax:TabContainer runat="server" Width="100%" ActiveTabIndex="0" Height="550px"
            AutoPostBack="true" ID="tbc_Destinos">
            <ajax:TabPanel runat="server" ID="tab_Usuarios" HeaderText="Por Usuarios">
                <ContentTemplate>
                    <table style="width: 100%">
                        <tr style="width: 400px">
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Usuario" Style="font-family: Verdana; font-size: x-small"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList runat="server" ID="ddl_Usuario" Width="100%" CssClass="DropDownList18">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button runat="server" ID="btn_Agregar" Text="Agregar" Width="90px" CssClass="button" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView runat="server" ID="gv_Usuarios" Width="100%" CssClass="gv_general" Height="100%"
                                    AutoGenerateColumns="False" DataKeyNames="Id_Empleado">
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

                                    <RowStyle CssClass="rowHover" />

                                    <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </ajax:TabPanel>
            <ajax:TabPanel runat="server" ID="tab_Modulos" Height="500px" HeaderText="Por Módulos">
                <ContentTemplate>
                    <asp:GridView runat="server" ID="gv_Modulos" Width="100%" CssClass="gv_general" Height="100%"
                        AutoGenerateColumns="False" DataKeyNames="Clave">

                        <Columns>
                            <asp:BoundField DataField="Clave" HeaderText="Clave" Visible="False">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbx_Modulo" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="30px" />
                                <HeaderTemplate>
                                    <input id="chkAll" onclick="javascript: SelectAllCheckboxes(this);" runat="server"
                                        type="checkbox" />
                                </HeaderTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                        </Columns>
                        <RowStyle CssClass="rowHover" />
                        <SelectedRowStyle CssClass="FilaSeleccionada_gv" />

                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                        <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />

                    </asp:GridView>
                </ContentTemplate>
            </ajax:TabPanel>
        </ajax:TabContainer>
    </div>
    <div style="float: left; width: 400px; margin-left: 20px">
        <table style="width: 100%; font-family: Verdana; font-size: x-small">
            <tr>
                <td>
                    <asp:Label ID="lbl_Asunto" runat="server" Text="Asunto"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="tbx_Asunto" Width="100%" CssClass="tbx_Mayusculas"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Mensaje"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="tbx_Mensaje" runat="server" TextMode="MultiLine" Style="width: 100%; height: 520px;"
                        CssClass="tbx_Mayusculas"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Button runat="server" ID="btn_Enviar" Text="Enviar" Width="90px" CssClass="buttonU" />
                </td>
            </tr>
        </table>
        <br />
    </div>
</asp:Content>
