<%@ Control Language="VB" AutoEventWireup="false"
    Inherits="SISSAIntranet.wuc_Jornadas" Codebehind="wuc_Jornadas.ascx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/Alerta.ascx" TagName="wuc_Alerta" TagPrefix="Alerta" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../Estilos/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        // toggle state of the checkbox between selected and not selected!
        // alternar estado de los checkboxes entre marcado o no marcado  
        function toggleCheckBoxes(gvId, isChecked) {
            var checkboxes = getCheckBoxesFrom(document.getElementById(gvId));
            for (i = 0; i <= checkboxes.length - 1; i++) {
                checkboxes[i].checked = isChecked;
            }
        } 
    </script>

    <script type="text/javascript">
        function getCheckBoxesFrom(gv) {
            var checkboxesArray = new Array();
            var inputElements = gv.getElementsByTagName("input");
            if (inputElements.length == 0) null;
            for (i = 0; i <= inputElements.length - 1; i++) {
                if (isCheckBox(inputElements[i])) {
                    checkboxesArray.push(inputElements[i]);
                }
            }
            return checkboxesArray;
        } 
    </script>

    <script type="text/javascript">
        // checks if the elements is a checkbox or not
        // identificar si los elementos son o no checkboxes 
        function isCheckBox(element) {
            return element.type == "checkbox";
        } 
    </script>

</head>
<body>
    <div class="Div_Principal">
        <%--<br />
        <div id="divEnca">
            <strong>Jornadas</strong>
        </div>--%>
        <br />
        <div>
            <asp:UpdatePanel ID="udp_Filtros" runat="server">
                <ContentTemplate>
                    <table class="tablaSISSA1">
                        <tr>
                            <td align="right">
                                <asp:Label ID="lbl_Puesto" runat="server" Text="Puesto"></asp:Label>
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:DropDownList ID="ddl_Puesto" runat="server" CssClass="DropDownList18"
                                    AutoPostBack="true" Width="406px" Enabled="false" DataTextField="Descripcion" DataValueField="Id_Puesto">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_Puesto" runat="server" Text="Todos" AutoPostBack="True"
                                    Checked="True" />
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:Button ID="btn_Mostrar" runat="server" Text="Mostrar" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="div1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gv_Empleados" runat="server" AutoGenerateColumns="False" CssClass="gridSISSA"
                        DataKeyNames="Id_Empleado" Width="99%">
                        <RowStyle BackColor="White" ForeColor="#333333" />
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select"
                                        ImageUrl="~/Imagenes/1rightarrow.png"/>
                                </ItemTemplate>
                                <ItemStyle Width="15px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_Empleado" runat="server" />
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <br />
        <div id="div2" style="float: left; width: 50%">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <fieldset style="height: 510px">
                        <table>
                            <tr>
                                <td class="celdaMargenDer10">
                                    <asp:Label runat="server" Text="Las Jornadas mostradas corresponden al rango:"
                                        CssClass="textbox1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="celdaMargenDer10">
                                    <asp:Label runat="server" Text="Inicio: 5 dias anteriores a la fecha actual." CssClass="textbox1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="celdaMargenDer10">
                                    <asp:Label runat="server" Text="Final: 30 dias posteriores a la fecha actual." CssClass="textbox1"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gv_Jornadas" runat="server" AutoGenerateColumns="False" CssClass="gridSISSA"
                            DataKeyNames="Id_Jornada" Width="100%" AllowPaging="True" PageSize="15">
                            <RowStyle BackColor="White" ForeColor="#333333" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <input type="checkbox" onclick="toggleCheckBoxes('<%= gv_Jornadas.ClientID %>',this.checked)"
                                            id="chkAll" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_Dia" runat="server" />
                                    </ItemTemplate>
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
                            <FooterStyle BackColor="White" ForeColor="Black" />
                            <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#C0A062" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                        </asp:GridView>
                        <br />
                        <table class="tablaSISSA1" width="100%">
                            <tr>
                                <td style="width: 250px; text-align: right">
                                    <asp:Label ID="Label9" runat="server" Text="Desde"></asp:Label>
                                </td>
                                <td class="celdaMargenDer10">
                                    <asp:TextBox ID="tbx_DesdeEliminar" runat="server" CssClass="CalendarTextbox"
                                        AutoPostBack="true" MaxLength="1" ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="tbx_DesdeEliminar"
                                        FilterType="Custom, Numbers" ValidChars="/">
                                    </ajax:FilteredTextBoxExtender>
                                    <ajax:CalendarExtender runat="server" ID="CalendarExtender3" TargetControlID="tbx_DesdeEliminar"
                                        CssClass="calendarSISSA" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                                    </ajax:CalendarExtender>
                                </td>
                                <td align="right" style="width: 70px">
                                    <asp:Label ID="Label10" runat="server" Text="Hasta"></asp:Label>
                                </td>
                                <td class="celdaMargenDer10">
                                    <asp:TextBox ID="tbx_HastaEliminar" runat="server" CssClass="CalendarTextbox"
                                        AutoPostBack="true" MaxLength="1" ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="tbx_HastaEliminar"
                                        FilterType="Custom, Numbers" ValidChars="/">
                                    </ajax:FilteredTextBoxExtender>
                                    <ajax:CalendarExtender runat="server" ID="CalendarExtender4" TargetControlID="tbx_HastaEliminar"
                                        CssClass="calendarSISSA" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                                    </ajax:CalendarExtender>
                                </td>
                                <td style="width: 100px">
                                    <asp:Button runat="server" ID="btn_EliminarRango" Width="130px" Text="Eliminar" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="4">
                                </td>
                                <td style="text-align: center">
                                    <asp:Button runat="server" ID="btn_Eliminar" Width="130px" Text="Eliminar Selección"
                                        Enabled="false" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
        </div>
        <div id="div3" style="float: left; width: 48%; margin-left: 10px">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <fieldset style="height: 510px">
                        <br />
                        <table class="tablaSISSA1">
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 250px; text-align: right">
                                    <asp:Label ID="lbl_Desde" runat="server" Text="Desde"></asp:Label>
                                </td>
                                <td class="celdaMargenDer10">
                                    <asp:TextBox ID="tbx_Desde" runat="server" CssClass="CalendarTextbox"
                                        AutoPostBack="true" MaxLength="1" ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbx_Desde"
                                        FilterType="Custom, Numbers" ValidChars="/">
                                    </ajax:FilteredTextBoxExtender>
                                    <ajax:CalendarExtender runat="server" ID="CalendarExtender1" TargetControlID="tbx_Desde"
                                        CssClass="calendarSISSA" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                                    </ajax:CalendarExtender>
                                </td>
                                <td align="right" style="width: 70px">
                                    <asp:Label ID="Lbl_Hasta" runat="server" Text="Hasta"></asp:Label>
                                </td>
                                <td class="celdaMargenDer10">
                                    <asp:TextBox ID="tbx_Hasta" runat="server" CssClass="CalendarTextbox"
                                        AutoPostBack="true" MaxLength="1" ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbx_Hasta"
                                        FilterType="Custom, Numbers" ValidChars="/">
                                    </ajax:FilteredTextBoxExtender>
                                    <ajax:CalendarExtender runat="server" ID="CalendarExtender2" TargetControlID="tbx_Hasta"
                                        CssClass="calendarSISSA" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                                    </ajax:CalendarExtender>
                                </td>
                                <td style="width: 30px">
                                </td>
                            </tr>
                        </table>
                        <br />
                        <ajax:TabContainer runat="server" Width="100%" ActiveTabIndex="0" Height="327px"
                            AutoPostBack="false" ID="tbc_Jornadas">
                            <ajax:TabPanel runat="server" ID="tab_Plantillas" HeaderText="Plantilla">
                                <ContentTemplate>
                                    <table class="tablaSISSA1" style="width: 100%">
                                        <tr style="width: 400px">
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text="Jornada" Style="font-family: Verdana;
                                                    font-size: x-small"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddl_Jornada" Width="100%" Style="font-family: Verdana;
                                                    font-size: x-small" AutoPostBack="True" DataTextField="Descripcion" 
                                                    DataValueField="Id_Jornada">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gv_Plantillas" runat="server" AutoGenerateColumns="False" CssClass="gridSISSA"
                                                    DataKeyNames="Id_Jornada" Width="100%" AllowPaging="True" PageSize="7">
                                                    <RowStyle BackColor="White" ForeColor="#333333" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Turno" HeaderText="Turno" SortExpression="Turno">
                                                            <ItemStyle Width="70px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Dia" HeaderText="Dia" SortExpression="Dia">
                                                            <ItemStyle Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PrimerJornada" HeaderText="Jornada1">
                                                            <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SegundaJornada" HeaderText="Jornada2">
                                                            <ItemStyle Width="150px" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <FooterStyle BackColor="White" ForeColor="Black" />
                                                    <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#C0A062" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </ajax:TabPanel>
                            <ajax:TabPanel runat="server" ID="tab_Manual" HeaderText="Manual">
                                <ContentTemplate>
                                    <table class="tablaSISSA1">
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" Text="Día"></asp:Label>
                                            </td>
                                            <td colspan="4" class="celdaMargenDer10">
                                                <asp:DropDownList ID="ddl_Dia" runat="server" Width="220px" CssClass="DropDownList18">
                                                    <asp:ListItem Value="0" Text="Seleccione..."></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="DOMINGO"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="LUNES"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="MARTES"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="MIERCOLES"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="JUEVES"></asp:ListItem>
                                                    <asp:ListItem Value="6" Text="VIERNES"></asp:ListItem>
                                                    <asp:ListItem Value="7" Text="SABADO"></asp:ListItem>
                                                    <asp:ListItem Value="8" Text="LUNES A VIERNES"></asp:ListItem>
                                                    <asp:ListItem Value="9" Text="LUNES A SABADO"></asp:ListItem>
                                                    <asp:ListItem Value="10" Text="LUNES A DOMINGO"></asp:ListItem>
                                                    <asp:ListItem Value="11" Text="SABADO Y DOMINGO"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label2" runat="server" Text="Turno"></asp:Label>
                                            </td>
                                            <td colspan="3" class="celdaMargenDer10">
                                                <asp:DropDownList ID="ddl_Turno" runat="server" Width="150px" CssClass="DropDownList18">
                                                    <asp:ListItem Value="0" Text="Seleccione..."></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="DIURNO"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="NOCTURNO"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 90px; text-align: right">
                                                <asp:Label ID="Label6" runat="server" Text="Jornada 1"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label3" runat="server" Text="De"></asp:Label>
                                            </td>
                                            <td class="celdaMargenDer10">
                                                <asp:DropDownList ID="ddl_Jornada1De" runat="server" Width="70px" CssClass="DropDownList18">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label8" runat="server" Text="A"></asp:Label>
                                            </td>
                                            <td class="celdaMargenDer10">
                                                <asp:DropDownList ID="ddl_Jornada1A" runat="server" Width="70px" CssClass="DropDownList18">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 90px; text-align: right">
                                                <asp:Label ID="Label5" runat="server" Text="Jornada 2"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label4" runat="server" Text="De"></asp:Label>
                                            </td>
                                            <td class="celdaMargenDer10">
                                                <asp:DropDownList ID="ddl_Jornada2De" runat="server" Width="70px" CssClass="DropDownList18">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label7" runat="server" Text="A"></asp:Label>
                                            </td>
                                            <td class="celdaMargenDer10">
                                                <asp:DropDownList ID="ddl_Jornada2A" runat="server" Width="70px" CssClass="DropDownList18">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 100px">
                                                <asp:CheckBox runat="server" ID="chk_NoAplica" Text="No Aplica" AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </ajax:TabPanel>
                        </ajax:TabContainer>
                        <br />
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Button runat="server" ID="btn_Guardar" Text="Guardar" Width="130px" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
        </div>
        <%--<Alerta:wuc_Alerta runat="server" ID="alerta"/>--%>
    </div>
</body>
</html>
