<%@ Page Title="Cursos Programados" Language="VB" MasterPageFile="~/MP_Principal.master"
    Inherits="IntranetSIAC.CursosProgramados" CodeBehind="CursosProgramados.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="Principal" class="Div_Principal">
        <div class="Div_Encabezado">
            <asp:GridView runat="server" ID="gv_Cursos"
                CssClass="gv_general" AutoGenerateColumns="False"
                AllowPaging="True"
                DataKeyNames="Id_Programacion,Id_Curso,Instructor,Sitio,Duracion,HoraInicio,HoraFin,AsistMax">
                <Columns>
                    <asp:CommandField HeaderStyle-Width="15px" ButtonType="Image" SelectImageUrl="~/Imagenes/1rightarrow.png"
                        ShowSelectButton="True" />

                    <asp:BoundField DataField="Id_Programacion" HeaderText="IDP" Visible="False" />
                    <asp:BoundField HeaderText="Clave" DataField="Clave" />
                    <asp:BoundField HeaderText="Curso" DataField="Curso" />
                    <asp:BoundField HeaderText="Fecha" DataField="Fecha" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HoraInicio" HeaderText="Hora Inicio" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Id_Curso" Visible="False" />
                    <asp:BoundField DataField="Instructor" HeaderText="Instructor" Visible="False" />
                    <asp:BoundField DataField="Sitio" HeaderText="Sitio" Visible="False" />
                    <asp:BoundField DataField="Duracion" HeaderText="Duracion" Visible="False" />
                    <asp:BoundField DataField="AsistMin" HeaderText="AsistenciaMin" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="AsistMax" HeaderText="AsistenciaMax" HeaderStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField HeaderText="HoraFin" Visible="False" />
                </Columns>

                <RowStyle CssClass="rowHover" />
                <SelectedRowStyle CssClass="FilaSeleccionada_gv" />

                <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
            </asp:GridView>
            <br />
        </div>
        <div id="divDetallesEnca">
            <strong>Detalles del Curso seleccionado</strong>
        </div>
        <br />
      
        <div id="div_Cursos" class="izq" runat="server">
            <asp:GridView ID="gv_Temas" runat="server"
                AutoGenerateColumns="False" CssClass="gv_general" Width="700px">
                <Columns>
                    <asp:BoundField DataField="Clave" HeaderText="Clave" ItemStyle-Width="10%">
                        <ItemStyle Width="10%"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Descripcion" HeaderText="Temas" ItemStyle-Width="60%">
                        <ItemStyle Width="60%"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Horas" HeaderText="Duración (hrs)" ItemStyle-Width="20%">
                        <ItemStyle Width="20%"></ItemStyle>
                    </asp:BoundField>
                </Columns>
                <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
            </asp:GridView>
        </div>

        <div id="div_datosCursos" class="izq">
            <table style="font-size: x-small">
                <tr>
                    <td style="text-align: right; width: 100px">Instructor
                    </td>
                    <td class="celdaMargenDer10">
                        <asp:TextBox ID="tbx_Instructor" runat="server" Width="350px" ReadOnly="True" Style="font-size: x-small" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">Sitio
                    </td>
                    <td class="celdaMargenDer10">
                        <asp:TextBox ID="tbx_Sitio" runat="server" Width="350px" ReadOnly="True" Style="font-size: x-small" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">Duración
                    </td>
                    <td class="celdaMargenDer10">
                        <asp:TextBox ID="tbx_Duracion" runat="server" Width="30px" ReadOnly="True" Style="font-size: x-small; text-align: right" />
                        &nbsp;<asp:Label ID="Label1" runat="server" Text="horas"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">Hora Inicio
                    </td>
                    <td class="celdaMargenDer10">
                        <asp:TextBox ID="tbx_HoraInicio" runat="server" Width="30px" ReadOnly="True" Style="font-size: x-small; text-align: right" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">Hora Fin
                    </td>
                    <td class="celdaMargenDer10">
                        <asp:TextBox ID="tbx_HoraFin" runat="server" Width="30px" ReadOnly="True" Style="font-size: x-small; text-align: right" />
                    </td>
                </tr>
            </table>
        </div>
     
        <div id="divOtro">
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div id="divAgregarEnca">
                        <strong>Agregar Empleados al Curso seleccionado</strong>
                    </div>
                    <br />
                    <table style="font-size: x-small">
                        <tr>
                            <td style="text-align: right; width: 100px">Empleado
                            </td>
                            <td class="celdaMargenDer10" style="width: 370px;">
                                <asp:DropDownList ID="ddl_Empleados" runat="server" Width="350px" Style="font-size: x-small" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btn_Agregar" Text="Agregar" Width="90px" CssClass="buttonB" />
                            </td>
                        </tr>
                    </table>
                    </div>
                    <div id="divAgregarEmpleados">
                        <asp:GridView ID="gv_EmpleadosAgregados" runat="server"
                            AutoGenerateColumns="False" DataKeyNames="Id_Empleado"
                            CssClass="gv_general"
                            Width="700px">

                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Delete"
                                            ImageUrl="~/Imagenes/EliminarEmpleado.png" Text="Eliminar" />
                                    </ItemTemplate>
                                    <ItemStyle Width="50px" />
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Clave" DataField="Clave" ItemStyle-Width="70px">
                                    <ItemStyle Width="70px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                                <asp:BoundField DataField="Puesto" HeaderText="Puesto" />
                                <asp:BoundField DataField="FechaReg" HeaderText="Fecha Registro" ItemStyle-Width="100px">
                                    <ItemStyle Width="100px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Id_Empleado" HeaderText="EmpleadoID" Visible="False" />
                            </Columns>
                            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
