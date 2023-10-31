<%@ Page Title="Solicitud de Insumos" Language="VB" MasterPageFile="~/MP_Principal.master" AutoEventWireup="false"
   CodeBehind="Insumos.aspx.vb" Inherits="IntranetSIAC.Insumos"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="Div_Principal">
        <br />
        <ajax:TabContainer ID="tc_Insumos" runat="server" Width="100%"
            ActiveTabIndex="0">
            <ajax:TabPanel ID="tab_Solicitud" runat="server" HeaderText="Solicitud de Insumos">
                <HeaderTemplate>
                    Solicitud de Insumos
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:UpdatePanel runat="server" ID="udp_Consumibles">
                        <ContentTemplate>
                            <p>
                                <asp:RadioButton ID="rdb_Accesorios" Text="Accesorios" AutoPostBack="True" OnCheckedChanged="rdbs_CheckedChanged"
                                    GroupName="Consumibles" runat="server" />
                                <asp:RadioButton ID="rdb_Consumibles" Text="Consumibles" AutoPostBack="True" OnCheckedChanged="rdbs_CheckedChanged"
                                    GroupName="Consumibles" runat="server" />
                            </p>
                            <asp:GridView runat="server" ID="gv_Consumibles" AutoGenerateColumns="False" DataKeyNames="Id_Consumible,Clave"
                                CssClass="gv_general" Width="60%">

                                <Columns>
                                    <asp:BoundField HeaderText="Id_Consumible" Visible="False" DataField="Id_Consumible">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Clave" HeaderText="Clave" Visible="False" />
                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción">
                                        <ItemStyle Width="800px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Cantidad">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="tbx_Cantidad" Width="50px" Height="15px" MaxLength="3"
                                                Style="text-align: center"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender runat="server" ID="ftb_Cantidad" TargetControlID="tbx_Cantidad"
                                                FilterType="Numbers">
                                            </ajax:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle Width="40px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle CssClass="rowHover" />
                                <SelectedRowStyle CssClass="FilaSeleccionada_gv" />

                                <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                                <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                            </asp:GridView>
                            <table width="60%">
                                <tr align="right">
                                    <td>
                                        <asp:Button ID="btn_Agregar" runat="server" Text="Agregar" Width="90px" CssClass="buttonB" />
                                    </td>
                                </tr>
                            </table>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <p>
                        <a id="Solicitud"></a>
                    </p>
                    <p style="text-align: center; width: 60%">
                        <strong>Insumos Agregados</strong>
                    </p>
                    <asp:UpdatePanel ID="udp_Solicitud" runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="gv_Solicitud" CssClass="gv_general" AutoGenerateColumns="False"
                                Width="60%" DataKeyNames="Id_Consumible,Clave,Descripcion,Cantidad_Ant">
                                <Columns>
                                    <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Imagenes/HoraNo.png" ShowDeleteButton="True">
                                        <ItemStyle Width="20px" />
                                    </asp:CommandField>
                                    <asp:CommandField ButtonType="Image" EditImageUrl="~/Imagenes/edit.gif" UpdateImageUrl="~/Imagenes/save.gif"
                                        CancelImageUrl="~/Imagenes/cancel.gif" ShowEditButton="True">
                                        <ItemStyle Width="40px" />
                                    </asp:CommandField>
                                    <asp:BoundField HeaderText="Id_Consumible" DataField="Id_Consumible" Visible="False" />
                                    <asp:BoundField HeaderText="Clave" DataField="Clave" ReadOnly="true" Visible="False" />
                                    <asp:BoundField HeaderText="Descripción" DataField="Descripcion" HtmlEncode="false"
                                        ReadOnly="true">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Cantidad">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="tbx_CantidadSolicitada" runat="server" Text='<%# Bind("Cantidad") %>'
                                                Width="100px" Style="text-align: center" MaxLength="3" ForeColor="Blue"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender runat="server" ID="ftb_CantidadSolicitada" TargetControlID="tbx_CantidadSolicitada"
                                                FilterType="Numbers">
                                            </ajax:FilteredTextBoxExtender>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_CantidadSolicitada" runat="server" Text='<%# Bind("Cantidad") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle Width="70px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Cantidad_Ant" HeaderText="Cantidad_Ant" ReadOnly="True"
                                        Visible="False" />
                                </Columns>

                                <RowStyle CssClass="rowHover" />


                                <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                                <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel runat="server" ID="udp_Guardar">
                        <ContentTemplate>
                            <table style="width: 60%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_Observaciones" runat="server" Text="Observaciones"></asp:Label>
                                        <asp:TextBox ID="txt_Observaciones" MaxLength="500" Width="99%" TextMode="MultiLine"
                                            runat="server" Height="70px" Style="text-transform: uppercase"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="right">
                                    <td>
                                        <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" OnClientClick="this.disabled=true"
                                            OnClick="btn_Guardar_Click" UseSubmitBehavior="false" Enabled="False" Width="90px" CssClass="buttonB" />
                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                            <ProgressTemplate>
                                                <img src="Imagenes/Img_Progres.gif" alt=" " />
                                                Espere un Momento......
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <%--    <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" OnClientClick="this.disabled=true" UseSubmitBehavior="false"/>--%>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </ajax:TabPanel>














            <ajax:TabPanel ID="tab_Consulta" runat="server" HeaderText="Consulta">
                <HeaderTemplate>
                    Consulta
                </HeaderTemplate>
                <ContentTemplate>
                    <br />
                    <asp:UpdatePanel ID="udp_Consulta" runat="server">
                        <ContentTemplate>
                            <table class="tablaSISSA1">
                                <tr>
                                    <td style="width: 120px; text-align: right">
                                        <asp:Label ID="lbl_Fecha_Inicio" runat="server" Text="Fecha Inicio"></asp:Label>
                                    </td>
                                    <td class="celdaMargenDer10">
                                        <asp:TextBox ID="tbx_FechaIni" runat="server" CssClass="calendarioAjax" AutoPostBack="True"
                                            MaxLength="1"></asp:TextBox>
                                        <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbx_FechaIni"
                                            FilterType="Custom, Numbers" ValidChars="/" Enabled="True">
                                        </ajax:FilteredTextBoxExtender>
                                        <ajax:CalendarExtender runat="server" ID="CalendarExtender1" TargetControlID="tbx_FechaIni"
                                            CssClass="calendarioAjax" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy"
                                            Enabled="True">
                                        </ajax:CalendarExtender>
                                    </td>
                                    <td align="right" style="width: 70px">
                                        <asp:Label ID="Lbl_FechaFin" runat="server" Text="Fecha Fin"></asp:Label>
                                    </td>
                                    <td class="celdaMargenDer10">
                                        <asp:TextBox ID="tbx_FechaFin" runat="server" CssClass="calendarioAjax" AutoPostBack="True"
                                            MaxLength="1"></asp:TextBox>
                                        <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tbx_FechaFin"
                                            FilterType="Custom, Numbers" ValidChars="/" Enabled="True">
                                        </ajax:FilteredTextBoxExtender>
                                        <ajax:CalendarExtender runat="server" ID="CalendarExtender2" TargetControlID="tbx_FechaFin"
                                            CssClass="calendarioAjax" Format="dd/MM/yyyy" TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy"
                                            Enabled="True">
                                        </ajax:CalendarExtender>
                                    </td>
                                    <td style="width: 30px"></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lbl_Usuario" runat="server" Text="Status"></asp:Label>
                                    </td>
                                    <td class="celdaMargenDer10" colspan="3">
                                        <asp:DropDownList ID="ddl_Status" runat="server" CssClass="DropDownList18" AutoPostBack="True"
                                            Width="270px" Enabled="False">
                                            <asp:ListItem Value="0" Text="Seleccione..."></asp:ListItem>
                                            <asp:ListItem Value="A" Text="PENDIENTE"></asp:ListItem>
                                            <asp:ListItem Value="V" Text="VALIDADA"></asp:ListItem>
                                            <asp:ListItem Value="S" Text="SURTIDA"></asp:ListItem>
                                            <asp:ListItem Value="C" Text="CANCELADA"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbx_Status" runat="server" Text="Todos" AutoPostBack="True" Width="70px"
                                            Checked="True" />
                                    </td>
                                    <td class="celdaMargenDer10">
                                        <asp:Button ID="btn_Mostrar" runat="server" Text="Mostrar" Width="90px" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <asp:UpdatePanel ID="udp_Solicitudes" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gv_Solicitudes" runat="server"
                                DataKeyNames="Id_Solicitud" AllowPaging="True"
                                Width="100%" CssClass="gv_general" AutoGenerateColumns="False">

                                <Columns>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select"
                                                ImageUrl="~/Imagenes/1rightarrow.png" />
                                        </ItemTemplate>
                                        <ItemStyle Width="15px" />
                                    </asp:TemplateField>
                                    <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Imagenes/HoraNo.png" ShowDeleteButton="True">
                                        <ItemStyle Width="20px" />
                                    </asp:CommandField>
                                    <asp:BoundField DataField="Id_Solicitud" HeaderText="Id_Solicitud" Visible="False" />
                                    <asp:BoundField DataField="Numero" HeaderText="Numero" />
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                    <asp:BoundField DataField="Hora" HeaderText="Hora" />
                                    <asp:BoundField DataField="UsuarioSolicita" HeaderText="Usuario Solicita" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                </Columns>

                                <RowStyle CssClass="rowHover" />
                                <SelectedRowStyle CssClass="FilaSeleccionada_gv" />

                                <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                                <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <asp:UpdatePanel ID="udp_Detalle" runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="gv_Detalle" AutoGenerateColumns="false" CssClass="gv_general"
                                Width="100%" DataKeyNames="Id_Consumible">

                                <Columns>
                                    <asp:BoundField HeaderText="Id_Consumible" DataField="Id_Consumible" Visible="False">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Clave" DataField="Clave" ReadOnly="True" Visible="False">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Descripción" DataField="Descripcion" HtmlEncode="False"
                                        ReadOnly="True">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Cantidad Solicitada">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_CantidadSolicitada" runat="server" Text='<%# Bind("CantidadSolicitada") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="tbx_CantidadSolicitada" runat="server" Text='<%# Bind("Cantidad") %>'
                                                Width="100px"></asp:TextBox><ajax:FilteredTextBoxExtender runat="server" ID="ftb_CantSol"
                                                    TargetControlID="tbx_CantidadSolicitada" FilterType="Numbers">
                                                </ajax:FilteredTextBoxExtender>
                                        </EditItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle Width="110px" HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CantidadValidada" HeaderText="CantidadValidada">
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle Width="70px" HorizontalAlign="Right" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="CantidadSurtida" HeaderText="CantidadSurtida">
                                        <HeaderStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" Width="70px" />
                                    </asp:BoundField>
                                </Columns>
                                <RowStyle CssClass="rowHover" />
                                <SelectedRowStyle CssClass="FilaSeleccionada_gv" />

                                <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                                <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                            </asp:GridView>
                            <br />
                            <asp:TextBox ID="txt_ObservacionesD" TextMode="MultiLine" ReadOnly="true" Width="100%"
                                Height="50pt" runat="server"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </ajax:TabPanel>
        </ajax:TabContainer>
    </div>
</asp:Content>
