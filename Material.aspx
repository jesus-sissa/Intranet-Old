<%@ Page Title="Solicitud de Material" Language="VB" MasterPageFile="~/MP_Principal.master" AutoEventWireup="false"
    CodeBehind="Material.aspx.vb" Inherits="IntranetSIAC.Material" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="Div_Principal">
  
       
                <asp:Button ID="Button1" runat="server" Text="Mostrar Materiales" CssClass="buttonB" />
                <asp:GridView runat="server" ID="gv_Material" AutoGenerateColumns="False" DataKeyNames="Id_Inventario,Clave"
                    CssClass="gv_general" Width="60%">

                    <Columns>
                        <asp:BoundField HeaderText="Id_Inventario" Visible="False" DataField="Id_Inventario">
                            <ItemStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Clave" HeaderText="Clave" Visible="False" />
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción">
                            <ItemStyle Width="800px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Cantidad">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="tbx_Cantidad" Width="50px" Height="15px" MaxLength="5"
                                    Style="text-align: center">
                                </asp:TextBox>
                                <ajax:FilteredTextBoxExtender runat="server" ID="ftb_Cantidad" TargetControlID="tbx_Cantidad"
                                    FilterType="Numbers">
                                </ajax:FilteredTextBoxExtender>

                            </ItemTemplate>
                            <ItemStyle Width="40px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="rowHover" />
                    <SelectedRowStyle CssClass="FilaSeleccionada_gv" />

                    <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Center" />
                </asp:GridView>
                <table width="60%">
                    <tr align="right">
                        <td>
                            <asp:Button ID="btn_Agregar" runat="server" Text="Agregar" Width="90px" Visible="False" CssClass="button" />
                        </td>
                    </tr>
                </table>

                <asp:GridView runat="server" ID="gv_Solicitud" CssClass="gv_general" AutoGenerateColumns="False" Width="60%" DataKeyNames="Id_Inventario,Clave,Descripcion,Cantidad_Ant">
                    <Columns>
                        <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Imagenes/HoraNo.png" ShowDeleteButton="True">
                            <ItemStyle Width="20px" />
                        </asp:CommandField>
                        <asp:CommandField ButtonType="Image" EditImageUrl="~/Imagenes/edit.gif" UpdateImageUrl="~/Imagenes/save.gif"
                            CancelImageUrl="~/Imagenes/cancel.gif" ShowEditButton="True">
                            <ItemStyle Width="40px" />
                        </asp:CommandField>
                        <asp:BoundField HeaderText="Id_Inventario" DataField="Id_Inventario" Visible="False" />
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
            
        


                <table style="width: 60%">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Observaciones" runat="server" Text="Observaciones"></asp:Label>
                            <asp:TextBox ID="txt_Observaciones" MaxLength="500" Width="99%" TextMode="MultiLine"
                                runat="server" Height="70px" Style="text-transform: uppercase"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="right">
                        <td style="height: 30px">
                            <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" OnClientClick="this.disabled=true" CssClass="buttonB"
                                OnClick="btn_Guardar_Click" UseSubmitBehavior="false" Width="90px" Visible="False" />

                         <%--   <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                <ProgressTemplate>
                                    <img src="Imagenes/Img_Progres.gif" alt=" " />
                                    Espere un Momento......
                                </ProgressTemplate>
                            </asp:UpdateProgress>--%>
                            <%--    <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" OnClientClick="this.disabled=true" UseSubmitBehavior="false" />--%>
                        </td>
                    </tr>
                </table>
        


    </div>
</asp:Content>
