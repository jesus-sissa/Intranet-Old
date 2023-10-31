<%@ Page Title="Actualizaciones" Language="VB" MasterPageFile="~/MP_Principal.master" 
    AutoEventWireup="false" Inherits="IntranetSIAC.Actualizaciones" CodeBehind="Actualizaciones.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

       <asp:GridView ID="gv_Actualizaciones" runat="server"
            AutoGenerateColumns="False" DataKeyNames="Id_Actualizacion,Status,Descripcion"
            CssClass="gv_general" AllowPaging="True" Width="100%" PageSize="25">

            <Columns>
                <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/1rightarrow.png"
                    ShowSelectButton="True">
                    <ItemStyle Width="15px" />
                </asp:CommandField>
                <asp:BoundField DataField="Id_Actualizacion" HeaderText="ACT_ID" Visible="False">
                    <ItemStyle HorizontalAlign="Right" Wrap="True" />
                </asp:BoundField>
                <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                    <ItemStyle Width="50px" />
                </asp:BoundField>
                <asp:BoundField DataField="Modulo" HeaderText="Módulo">
                    <ItemStyle HorizontalAlign="Left" Wrap="True" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Menu" HeaderText="Menú">
                    <ItemStyle HorizontalAlign="Left" Wrap="True" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Opcion" HeaderText="Opción">
                    <ItemStyle HorizontalAlign="Left" Wrap="True" Width="300px" />
                </asp:BoundField>
                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" Visible="false">
                    <ItemStyle HorizontalAlign="Left" Wrap="True" Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="Status" HeaderText="Status" Visible="false">
                    <ItemStyle HorizontalAlign="Left" Wrap="True" />
                </asp:BoundField>
            </Columns>

            <RowStyle CssClass="rowHover" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>

        <br />
        <asp:Label runat="server" Text="Descripción" CssClass="textbox1" ID="lbl_Descripcion"></asp:Label>
        <asp:TextBox runat="server" ID="tbx_Descripcion" Width="100%" Height="150px" CssClass="textbox1"
         TextMode="MultiLine" ReadOnly="True"></asp:TextBox>


</asp:Content>

