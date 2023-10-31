﻿<%@ Control Language="VB" AutoEventWireup="false"
    Inherits="SISSAIntranet.UserControls_wuc_RIAConsulta" Codebehind="wuc_RIAConsulta.ascx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../Estilos/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <script type="text/javascript" language="javascript">
        // Abrir en ventana sin barra de scroll

        function AbrirSinScroll(url) {
            web = url
            alto = 600
            ancho = 800
            izq = (screen.width - ancho) / 2
            arr = ((screen.height - alto) / 2) - 15
            popupWin = window.open(web, "_blank", "scroll='no',width=" + ancho + ",height=" + alto + ",top=" + arr + ",left=" + izq + ",titlebar='no',Toolbar='no',Status='yes',Location='no'")
        }
    </script>

    <div class="Div_Principal">
        <%--<br />
        <div id="divEnca" title="prueba">
            <strong>Consulta de Reportes de Incidentes/Accidentes</strong>
        </div>--%>
        <br />
        <div>
            <table class="tablaSISSA1">
                <tr>
                    <td style="width: 120px; text-align: right">
                        <asp:Label ID="lbl_Fecha_Inicio" runat="server" Text="Fecha Inicio"></asp:Label>
                    </td>
                    <td class="celdaMargenDer10">
                        <asp:TextBox ID="tbx_FechaIni" runat="server" 
                            CssClass="CalendarTextbox" AutoPostBack="true"
                            Style="vertical-align: text-bottom" MaxLength="1"></asp:TextBox>
                        <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbx_FechaIni"
                            FilterType="Custom, Numbers" ValidChars="/">
                        </ajax:FilteredTextBoxExtender>
                        <ajax:CalendarExtender runat="server" ID="CalendarExtender1" 
                            TargetControlID="tbx_FechaIni" CssClass="calendarSISSA"
                            Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" 
                            DaysModeTitleFormat="MMMM  yyyy">
                        </ajax:CalendarExtender>
                    </td>
                    <td style="width: 30px;">
                    </td>
                    <td style="width: 100px;">
                    </td>
                    <td align="right" style="width: 70px">
                        <asp:Label ID="Lbl_FechaFin" runat="server" Text="Fecha Fin"></asp:Label>
                    </td>
                    <td class="celdaMargenDer10">
                        <asp:TextBox ID="tbx_FechaFin" runat="server"  
                            CssClass="CalendarTextbox" AutoPostBack="true" MaxLength="1"></asp:TextBox>
                        <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbx_FechaFin"
                            FilterType="Custom, Numbers" ValidChars="/">
                        </ajax:FilteredTextBoxExtender>
                        <ajax:CalendarExtender runat="server" ID="CalendarExtender2" 
                            TargetControlID="tbx_FechaFin" CssClass="calendarSISSA"
                            Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" 
                            DaysModeTitleFormat="MMMM  yyyy">
                        </ajax:CalendarExtender>
                    </td>
                    <td style="width: 30px">
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px; text-align: right">
                        <asp:Label ID="Label1" runat="server" Text="Tipo"></asp:Label>
                    </td>
                    <td class="celdaMargenDer10" colspan="6" style="width: 406px">
                        <asp:DropDownList ID="ddl_Tipo" runat="server" DataTextField="Tipo" CssClass="DropDownList18"
                            AutoPostBack="true" DataValueField="Clave_Tipo" Width="406px" Enabled="false">
                            <asp:ListItem Value="0" Text="Seleccione..."></asp:ListItem>
                            <asp:ListItem Value="1" Text="INCIDENTE DE UNIDAD (VEHICULO)"></asp:ListItem>
                            <asp:ListItem Value="2" Text="INCIDENTE DE GUARDIA DE SEGURIDAD PATRIMONIAL"></asp:ListItem>
                            <asp:ListItem Value="3" Text="INCIDENTE EN AREA SEGURA"></asp:ListItem>
                            <asp:ListItem Value="4" Text="INCIDENTE EN AREA NO SEGURA"></asp:ListItem>
                            <asp:ListItem Value="5" Text="INCIDENTE CON CLIENTES"></asp:ListItem>
                            <asp:ListItem Value="6" Text="INCIDENTE EN RUTA"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="cbx_Tipos" runat="server" CssClass="Gera1" Text="Todos" AutoPostBack="true"
                            Checked="True" />
                    </td>
                    <td align="left">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lbl_Usuario" runat="server" Text="Status"></asp:Label>
                    </td>
                    <td class="celdaMargenDer10" colspan="6">
                        <asp:DropDownList ID="ddl_Status" runat="server" CssClass="DropDownList18" 
                            AutoPostBack="true" Width="406px" Enabled="False">
                            <asp:ListItem Value="0" Text="Seleccione..."></asp:ListItem>
                            <asp:ListItem Value="A" Text="PENDIENTE"></asp:ListItem>
                            <asp:ListItem Value="I" Text="ABIERTO"></asp:ListItem>
                            <asp:ListItem Value="V" Text="FINALIZADO"></asp:ListItem>
                            <asp:ListItem Value="B" Text="CANCELADO"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="cbx_Status" runat="server" CssClass="Gera1" Text="Todos" AutoPostBack="True"
                            Checked="True" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label6" runat="server" Text="Usuario Seguimiento"></asp:Label>
                    </td>
                    <td class="celdaMargenDer10" colspan="6">
                        <asp:DropDownList ID="ddl_Usuarios" runat="server" DataTextField="Nombre" CssClass="DropDownList18"
                            DataValueField="Id_Empleado" Width="406px" Enabled="False">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="cbx_Usuarios" runat="server" CssClass="Gera1" Text="Todos" AutoPostBack="True"
                            Checked="True" />
                    </td>
                    <td class="celdaMargenDer10">
                        <asp:Button ID="btn_Mostrar" runat="server" Text="Mostrar" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div>
            <asp:GridView ID="gv_RIA" runat="server" AutoGenerateColumns="False" DataKeyNames="Id_RIA,Sucursal,Status"
                CssClass="gridSISSA" AllowPaging="True" Width="100%" Style="color: Black; font-family: Verdana;
                font-size: x-small">
                <Columns>
                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/1rightarrow.png"
                        ShowSelectButton="True">
                        <ItemStyle Width="15px" />
                    </asp:CommandField>
                    <asp:BoundField DataField="Id_RIA" HeaderText="RIAID" Visible="False">
                        <ItemStyle HorizontalAlign="Right" Wrap="True" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Numero" HeaderText="Num">
                        <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Sucursal" HeaderText="Sucursal">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="120px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Hora" HeaderText="Hora">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="60px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="UsuarioSeg" HeaderText="Usuario Seguimiento">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="300px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Tipo" HeaderText="Tipo">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="330px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Entidad" HeaderText="Entidad" />
                    <asp:BoundField DataField="Status" HeaderText="Status">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="80px" />
                    </asp:BoundField>
                </Columns>
                <%--<SelectedRowStyle BackColor="#C0A062" Font-Bold="True" ForeColor="White" />--%>
                <SelectedRowStyle CssClass="SelectedRowSISSA" />
                <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
            </asp:GridView>
        </div>
        <br />
        <div>
            <asp:GridView runat="server" ID="gv_Usuarios" Width="50%" CssClass="gridSISSA" AutoGenerateColumns="False"
                DataKeyNames="Id_Entidad">
                <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                <Columns>
                    <asp:BoundField DataField="Id_Entidad" HeaderText="Id_Entidad" Visible="False">
                        <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Nombre" HeaderText="Usuarios para Seguimiento" />
                    <asp:BoundField DataField="Rol" HeaderText="Tipo">
                    <ItemStyle Width="50px" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
            </asp:GridView>
        </div>
        <br />
        <div>
            <%--<asp:GridView ID="gv_Detalle" runat="server" AutoGenerateColumns="False" DataKeyNames="Id_RIAD,Id_RIA"
                Width="100%" Style="color: Black; font-family: Verdana; font-size: x-small">
                <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                <Columns>
                    <asp:BoundField DataField="Id_RIAD" HeaderText="IDRIAD" Visible="False" />
                    <asp:BoundField DataField="Id_RIA" HeaderText="IDRIA" Visible="False" />
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-Width="100">
                        <ItemStyle Width="120px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Tipo" HeaderText="Origen">
                        <ItemStyle Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Entidad" HeaderText="Registró" ItemStyle-Width="100">
                        <ItemStyle Width="250px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                </Columns>
                <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
            </asp:GridView>--%>
            <asp:PlaceHolder runat="server" ID="PlaceHolderConsulta">
                <div id="div_Comentarios">
                </div>
            </asp:PlaceHolder>
        </div>
        <br />
        <div style="float: left">
            <br />
            <table class="tablaSISSA1">
                <tr>
                    <td style="width: 100px; text-align: right">
                        <asp:Label ID="lbl_UsuarioA" runat="server" Text="Asignar a Usuario" Enabled="false"></asp:Label>
                    </td>
                    <td class="celdaMargenDer10" colspan="2">
                        <asp:DropDownList ID="ddl_UsuarioA" runat="server" DataTextField="Nombre" CssClass="DropDownList18"
                            DataValueField="Id_Empleado" Width="350px" Enabled="False">
                        </asp:DropDownList>
                    </td>
                    <%--<td align="right">
                        <asp:Button ID="btn_Asignar" runat="server" Text="Asignar" Enabled="false" />
                    </td>--%>
                </tr>
                <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lbl_TipoUsuario" runat="server" Text="Tipo Usuarios" Enabled="False"></asp:Label>
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:RadioButton ID="rdb_Principal" runat="server" Text="Principal" GroupName="TipoU" Enabled="False"/>
                            &nbsp;&nbsp;<asp:RadioButton ID="rdb_Secundario" runat="server" Text="Secundario" GroupName="TipoU" Enabled="False"/>
                            
                        </td>
                        <td align="right">
                        <asp:Button ID="btn_Asignar" runat="server" Text="Asignar" Enabled="False" />
                    </td>
                </tr>
                <%--<tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>--%>
            </table>
        </div>
        <div style="float: right">
            <asp:GridView runat="server" ID="gv_Imagenes" AutoGenerateColumns="False" DataKeyNames="ID_RIAI"
                Style="margin-top: 20px" CssClass="gridSISSA" Width="20%">
                <Columns>
                    <asp:BoundField DataField="Id_RIAI" HeaderText="ID" Visible="False" />
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                        <ItemStyle VerticalAlign="Top" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Imagen" FooterText="EJEMPLO">
                        <ItemTemplate>
                            <a href="javascript:;" onclick="AbrirSinScroll('MostrarImagenes.aspx?ID=<%# Eval("ID_RIAI")%>');">
                                <asp:Image ID="Image1" runat="server" Height="150px" Width="150px" ImageUrl='<%# "~/MostrarImagenes.ashx?ID=" & Eval("Id_RIAI") %>' />
                            </a>
                        </ItemTemplate>
                        <ItemStyle Height="150px" Width="150px" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
            </asp:GridView>
        </div>
    </div>
</body>
</html>
