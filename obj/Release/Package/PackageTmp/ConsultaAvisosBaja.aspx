<%@ Page Title="Consulta de Avisos de Baja" Language="vb" MasterPageFile="~/MP_Principal.master" AutoEventWireup="false"
    CodeBehind="ConsultaAvisosBaja.aspx.vb" Inherits="IntranetSIAC.ConsultaAvisosBaja" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="ContentPlantilla" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <table class="tablaSISSA1">
        <tr>
            <td style="width: 100px; text-align: right; height: 26px;">
                <asp:Label ID="lbl_Fecha_Inicio" runat="server" Text="Fecha Inicio"></asp:Label>
            </td>
            <td class="celdaMargenDer10" style="height: 26px">
                <asp:TextBox ID="tbx_FechaIni" runat="server" CssClass="calendarioAjax"
                    AutoPostBack="true" Style="vertical-align: text-bottom" MaxLength="1" ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario"></asp:TextBox>
                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbx_FechaIni"
                    FilterType="Custom, Numbers" ValidChars="/">
                </ajax:FilteredTextBoxExtender>
                <ajax:CalendarExtender runat="server" ID="CalendarExtender1" TargetControlID="tbx_FechaIni"
                    CssClass="calendarioAjax" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                </ajax:CalendarExtender>
            </td>
            <td style="width: 30px; height: 26px;"></td>
            <td align="right" style="width: 70px; height: 26px;">
                <asp:Label ID="Lbl_FechaFin" runat="server" Text="Fecha Fin"></asp:Label>
            </td>
            <td class="celdaMargenDer10" style="height: 26px">
                <asp:TextBox ID="tbx_FechaFin" runat="server" CssClass="calendarioAjax"
                    AutoPostBack="true" MaxLength="1" ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario"></asp:TextBox>
                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbx_FechaFin"
                    FilterType="Custom, Numbers" ValidChars="/">
                </ajax:FilteredTextBoxExtender>
                <ajax:CalendarExtender runat="server" ID="CalendarExtender2" TargetControlID="tbx_FechaFin"
                    CssClass="calendarioAjax" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                </ajax:CalendarExtender>
            </td>
            <td style="width: 30px; height: 26px;"></td>
            <td style="height: 26px">
                <asp:Button ID="btn_Mostrar" runat="server" Text="Mostrar" Width="90px" CssClass="buttonB" />
            </td>
        </tr>
    </table>

    <div id="div_ConsultaAvisosBaja" style="float: left; width: 100%;">
        <asp:GridView ID="gv_Empleados" runat="server" AutoGenerateColumns="False" CssClass="gv_general"
            DataKeyNames="Id_BajaAviso,Observaciones" Width="100%" AllowPaging="True" PageSize="30">

            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select"
                            ImageUrl="~/Imagenes/1rightarrow.png" Text="Seleccionar" />
                    </ItemTemplate>
                    <ItemStyle Width="15px" />
                </asp:TemplateField>
                <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha" HeaderStyle-Width="95px" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="UsuarioRegistro" HeaderText="Usuario Registro" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="FechaBaja" HeaderText="Fecha Baja" HeaderStyle-Width="95px" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Clave" HeaderText="Clave" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="EmpleadoBaja" HeaderText="Empleado Baja" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Departamento" HeaderText="Departamento" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Puesto" HeaderText="Puesto" HeaderStyle-HorizontalAlign="Left" />
            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>

        <asp:Label ID="Label1" runat="server" Text="Observaciones" Font-Size="X-Small"></asp:Label>
        <asp:TextBox ID="tbx_Observaciones" Width="100%" Height="45px" runat="server" Style="text-transform: uppercase" ReadOnly="True" Font-Size="X-Small" CssClass="textbox1" TextMode="MultiLine" MaxLength="300"></asp:TextBox>

    </div>

</asp:Content>
