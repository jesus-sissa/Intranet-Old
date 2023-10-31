<%@ Control Language="VB" AutoEventWireup="false" Inherits="SISSAIntranet.wuc_RegistroAsistencia"
    CodeBehind="wuc_RegistroAsistencia.ascx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>

    <script language="javascript" type="text/javascript">
        function fnClickOK(sender, e) {
            __doPostBack(sender, e);
            //        document.getElementById("<%=hf_Respuesta.ClientID%>").value = 1;
        }

        function mpeSeleccionOnOk() {
            //        document.getElementById("<%=hf_Respuesta.ClientID%>").value = 1;
            //        document.getElementById("<%=pnl_TiposAsistencia.ClientID%>").disabled = false;
            //        document.getElementById("<%=pnl_TiposAsistencia.ClientID%>").removeAttribute('disabled');
            //        var domArr = $get("<%=pnl_TiposAsistencia.ClientID %>").getElementsByTagName('input');
            //        for (var i = 0; i < domArr.length; i++) {
            //            domArr[i].removeAttribute('disabled');
            //        }
        }

        function mpeSeleccionOnCancel() {
            document.getElementById("<%=hf_Respuesta.ClientID%>").value = 0;
        }    
    </script>

    <title></title>
    <link href="../Estilos/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="Div_Principal">
        <br />
        <div>
            <asp:UpdatePanel runat="server" ID="udp_Empleado">
                <ContentTemplate>
                    <table class="tablaSISSA1">
                        <tr>
                            <td style="width: 120px; text-align: right">
                                <asp:Label ID="Label9" runat="server" Text="Fecha Asistencia"></asp:Label>
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:TextBox ID="tbx_FechaAsistencia" runat="server" AutoPostBack="true" CssClass="CalendarTextbox"
                                    MaxLength="1" ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="tbx_FechaAsistencia"
                                    FilterType="Custom, Numbers" ValidChars="/">
                                </ajax:FilteredTextBoxExtender>
                                <ajax:CalendarExtender runat="server" ID="CalendarExtender3" TargetControlID="tbx_FechaAsistencia"
                                    CssClass="calendarSISSA" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                                </ajax:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lbl_Empleado" runat="server" Text="Empleado"></asp:Label>
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:TextBox ID="tbx_Clave" runat="server" Width="50px" AutoPostBack="true" CssClass="Textbox14"
                                    MaxLength="6"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbx_Clave"
                                    FilterType="Numbers" Enabled="True">
                                </ajax:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_Empleado" runat="server" DataTextField="CveNombre" CssClass="DropDownList18"
                                    DataValueField="Id_Empleado" Width="350px" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel runat="server" ID="up_TipoAsistencia">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnl_TiposAsistencia" Enabled="false">
                        <table class="tablaSISSA1">
                            <tr>
                                <td>
                                </td>
                                <td class="celdaMargenDer10" style="width: 80px">
                                    <asp:RadioButton ID="rdb_Asistencia" runat="server" Text="Asistencia" GroupName="TipoU"
                                        AutoPostBack="true" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_Retardo" runat="server" Text="Retardo" GroupName="TipoU"
                                        AutoPostBack="true" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_HorasExtra" runat="server" Text="Horas Extra" GroupName="TipoU"
                                        AutoPostBack="true" />
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdb_Falta" runat="server" Text="Falta" GroupName="TipoU" AutoPostBack="true" />
                                </td>
                                <td class="celdaMargenDer10">
                                    <asp:RadioButton ID="rdb_RecuperarFalta" runat="server" Text="Recuperar Falta" GroupName="TipoU"
                                        AutoPostBack="true" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label1" runat="server" Text="Retardo"></asp:Label>
                                </td>
                                <td class="celdaMargenDer10">
                                    <asp:TextBox runat="server" ID="tbx_Retardo" Width="50px" AutoPostBack="true" CssClass="Textbox14"
                                        MaxLength="2" Enabled="false"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbx_Retardo"
                                        FilterType="Numbers" Enabled="True">
                                    </ajax:FilteredTextBoxExtender>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Label3" runat="server" Text="Horas Extra"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbx_HorasExtra" Width="50px" AutoPostBack="true"
                                        CssClass="Textbox14" MaxLength="2" Enabled="false"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="tbx_HorasExtra"
                                        FilterType="Numbers" Enabled="True">
                                    </ajax:FilteredTextBoxExtender>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px; text-align: right; vertical-align: top">
                                    <asp:Label ID="lbl_Observaciones" runat="server" Text="Observaciones"></asp:Label>
                                </td>
                                <td class="celdaMargenDer10" colspan="5">
                                    <asp:TextBox ID="tbx_Observaciones" runat="server" Width="425px" Height="85px" TextMode="MultiLine"
                                        CssClass="textbox1" Style="text-transform: uppercase"></asp:TextBox>
                                </td>
                                <asp:RequiredFieldValidator ID="rfv_Descripcion" ControlToValidate="tbx_Observaciones"
                                    ValidationGroup="Val1" runat="server" ErrorMessage="Capture la Descripción."
                                    ForeColor="Red">*</asp:RequiredFieldValidator>
                                <td style="width: 100px; text-align: left; vertical-align: bottom">
                                    <asp:Button runat="server" ID="btn_Guardar" Text="Guardar" Enabled="False" OnClick="btn_Guardar_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:HiddenField runat="server" ID="hf_Respuesta" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <div id="div2" style="float: left; width: 50%; margin-left: 135px">
                <asp:UpdatePanel ID="udp_ListaFaltas" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gv_Jornadas" runat="server" AutoGenerateColumns="False" CssClass="gridSISSA"
                            DataKeyNames="Id_Jornada" Width="100%" PageSize="15">
                            <RowStyle BackColor="White" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select"
                                            ImageUrl="~/Imagenes/1rightarrow.png" Text="Seleccionar" />
                                    </ItemTemplate>
                                    <ItemStyle Width="15px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Dia" HeaderText="Dia" SortExpression="Dia">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Jornada1" HeaderText="Jornada1">
                                    <ItemStyle Width="150px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Jornada2" HeaderText="Jornada2">
                                    <ItemStyle Width="150px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Turno" HeaderText="Turno">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                            </Columns>
                            <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#C0A062" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel runat="server" ID="up_Alerta">
                <ContentTemplate>
                    <asp:Panel ID="pnl_Alerta" runat="server" Width="400px" CssClass="ModalWindow">
                        <table cellspacing="0" cellpadding="0" style="width: 100%; height: 100%">
                            <tr valign="middle">
                                <td align="left" style="border: 1px solid #000000; width: 40px;">
                                    <asp:Image ID="Logo" runat="server" ImageUrl="~/Imagenes/LogoSISSALittle.jpg" />
                                </td>
                                <td align="center" style="background-color: Black; width: 250px;">
                                    <label style="color: White; font-weight: bold; font-size: large;">
                                        INTRANET
                                    </label>
                                </td>
                                <td align="right" style="border: 1px solid #000000; width: 40px;">
                                    <asp:ImageButton ID="btn_Cerrar" runat="server" Text="Cerrar" ImageUrl="~/Imagenes/Cerrar.png"
                                        ImageAlign="Right" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3" style="width: 400px; height: 80px;">
                                    <asp:Label ID="lbl_Mensaje" runat="server" Font-Bold="True" Font-Size="10pt" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btn_OK" runat="server" Text="Aceptar" Width="80px" />
                                </td>
                                <td>
                                </td>
                                <asp:Button ID="MpeFakeTarget" runat="server" CausesValidation="False" Style="display: none" />
                                <td>
                                    <asp:Button ID="btn_Cancelar" runat="server" Text="Cancelar" Width="80px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <ajax:ModalPopupExtender runat="server" ID="mpe_Ejemplo" TargetControlID="MpeFakeTarget"
                        BackgroundCssClass="modalBackground" PopupControlID="pnl_Alerta" OkControlID="btn_OK"
                        CancelControlID="btn_Cancelar" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</body>
</html>
