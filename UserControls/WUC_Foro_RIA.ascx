<%@ Control Language="VB" AutoEventWireup="false" Inherits="IntranetSIAC.WUC_Foro_RIA" Codebehind="WUC_Foro_RIA.ascx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../Estilos/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
<div class="Div_Principal">
        <br />
        <div id="divEnca" title="prueba">
            <strong>Foro de Reportes de Incidentes/Accidentes</strong>
        </div>
        <br />
        <div>
            <asp:GridView ID="gv_RIA" runat="server" AutoGenerateColumns="False" DataKeyNames="Id_RIA,Sucursal,Status"
                AllowPaging="True" Width="100%" CssClass="gv_general">
                <Columns>
                    <asp:BoundField DataField="Id_RIA" HeaderText="RIAID" Visible="False">
                        <ItemStyle HorizontalAlign="Right" Wrap="True" />
                    </asp:BoundField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" 
                                CommandName="Select" ImageUrl="~/Imagenes/1rightarrow.png" Text="Seleccionar" />
                        </ItemTemplate>
                        <ItemStyle Width="15px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Numero" HeaderText="Num">
                    <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Sucursal" HeaderText="Sucursal">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Hora" HeaderText="Hora">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Tipo" HeaderText="Tipo">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="240px" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Entidad" DataField="Entidad" >
                    <ItemStyle Width="240px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Status">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" />
                    </asp:BoundField>
                    
                </Columns>
<RowStyle CssClass="rowHover" />
 <SelectedRowStyle CssClass="FilaSeleccionada_gv" />

  <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
   <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />

            </asp:GridView>
            
            <br />

        </div>
        <br />
        <div>
            <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                <div id="div_Comentarios">
                </div>
            </asp:PlaceHolder>
        </div>
        
        <%--<marquee direction:"left">Hello visitor, this is a message that goes to left.
            
        </marquee>--%>
        
        
<%--        <div class='forosMessage' id='div1258908' >
            <div style='padding:5px 0px 0px 0px;'>
                <div>
                    <table border='0' cellspacing='2' cellpadding='0' width='100%'>
                        <tr>
                            <td class='list_title'>Asunto:</td>
                            <td width='100%' class='list_contentBold' id='idT1258908'>
                            Como publicar una aplicaion ASP.NET 2.0 en servidor windows</td>
                        </tr>
                        <tr>
                            <td class='list_title' width='10px'>Autor:</td>
                            <td width='100%' class='list_content'>
                                <span style='float:left;'>Javi&nbsp;<a href='mailto:molina1987_1@hotmail.com' class='link3'>molina1987_1@hotmail.com</a></span>&nbsp;(<a href='/foros/intervenciones.php?id=1258908' class='link3'>3 intervenciones</a>)</td>
                        </tr>
                        <tr>
                            <td class='list_title'>Fecha:</td>
                            <td width='100%' class='list_content'>29/03/2011 18:45:04</td>
                        </tr>
                    </table>
                    <div class='list_description' style='width:620px;padding:2px;margin:1px 0px 5px 2px;overflow:hidden;'>Hola programadores, vereis tengo un problema con las aplicaciones ASP.NET 2.0,tengo alquilado un alojamiento web de windows con iis alquilado en sync.es. he llamado por telefono y me ha disho que las aplicaciones asp.net 2.0 deben correr sin ningun problema, ademas tambien tiene .net framework 2.0 instalado<br />
                    <br />
                    Pues bien he hecho una aplicacion muy simple un boton que alpulsarlo dice en un textbox la palabra hola, en mi windows xp en localhost la aplicacion funciona bien pero en el alojamiento no rula no funciona os pongo lo que dice al intentar ejecutarlo:<br />
                    <br />
                    &quot;Server Error in '/' Application.<br />
                    Runtime Error <br />
                    Description: An application error occurred on the server. The current custom error settings for this application prevent the details of the application error from being viewed remotely (for security reasons). It could, however, be viewed by browsers running on the local server machine. <br />
                    <br />
                    Details: To enable the details of this specific error message to be viewable on remote machines, please create a &lt;customErrors&gt; tag within a &quot;web.config&quot; configuration file located in the root directory of the current web application. This &lt;customErrors&gt; tag should then have its &quot;mode&quot; attribute set to &quot;Off&quot;.<br />
                    <br />
                    &lt;!-- Web.Config Configuration File --&gt;<br />
                    <br />
                    &lt;configuration&gt;<br />
                        &lt;system.web&gt;<br />
                            &lt;customErrors mode=&quot;Off&quot;/&gt;<br />
                        &lt;/system.web&gt;<br />
                    &lt;/configuration&gt;<br />
                    <br />
                    <br />
                    Notes: The current error page you are seeing can be replaced by a custom error page by modifying the &quot;defaultRedirect&quot; attribute of the application's &lt;customErrors&gt; configuration tag to point to a custom error page URL.<br />
                    <br />
                    &lt;!-- Web.Config Configuration File --&gt;<br />
                    <br />
                    &lt;configuration&gt;<br />
                        &lt;system.web&gt;<br />
                            &lt;customErrors mode=&quot;RemoteOnly&quot; defaultRedirect=&quot;mycustompage.htm&quot;/&gt;<br />
                        &lt;/system.web&gt;<br />
                    &lt;/configuration&gt;&quot;<br />
                    <br />
                    <br />
                    <br />
                    <br />
                    &iquest;Alguna idea de lo que pasa??&iquest;Ha alguien le ha pasado lo mismo que a mi alguna vez?&iquest;Que puedo hacer para solucionarlo?<br />
                    <br />
                    PD:La aplicacion la he hecho con visual studio 2005 professional<br />
                    <br />
                    Un saludo espero vuestras respuestas</div></div></div><div class='forosBtnComment'><span><img src='/img/btn_responder.gif' width='74' height='14' border='0' onclick="javascript:showForm(1258908, 'foros')" style='cursor:pointer;cursor:hand;' title='Responder al autor' alt='Responder al autor' ></span><span>&nbsp;&nbsp;<a href='#s'><img src='/img/up.png' border='0' alt='Subir' title='Subir' ></a></span></div><div class='publi_otherSections' style='margin-ttop:25px;'><a href='/cursos/ASP.NET/index1.html' class='link2'>Cursos de ASP.NET</a></div><div id='idshowForm_1258908' class='forosForm' style='display:none;'></div></div>
--%>                            
        <%--<span class="pBody postableBody">
  When do you want to fill in the data? You can try using a datatable to store the data and bind it to the gridview:<br /><br />&nbsp; &nbsp; &nbsp; &nbsp; Dim tbl As New DataTable()<br />&nbsp; &nbsp; &nbsp; &nbsp; tbl.Columns.Add(&quot;col1&quot;)<br />&nbsp; &nbsp; &nbsp; &nbsp; tbl.Columns.Add(&quot;col2&quot;)<br />&nbsp; &nbsp; &nbsp; &nbsp; tbl.Columns.Add(&quot;col3&quot;)<br />&nbsp; &nbsp; &nbsp; &nbsp; tbl.Rows.Add(&quot;blah&quot;, &quot;something&quot;, &quot;somethingelse&quot;)<br />&nbsp; &nbsp; &nbsp; &nbsp; gv.DataSource = tbl<br />&nbsp; &nbsp; &nbsp; &nbsp; gv.DataBind()</span>
<span class="pBody postableBody">
  Try adding placeholders where you would like a GridView, then adding the GridView later. &nbsp;For instance, add this<br /><br />&lt;asp:PlaceHolder id=&quot;PlaceHolder1&quot; runat=&quot;server&quot;&gt;&lt;/asp:Place<wbr />Holder&gt;<br /><br />to your HTML just after the &lt;form&gt; tag, then adding<br /><br />Dim dg1 as New GridView<br />.<br />.<br />.<br />PlaceHolder1.Controls.Add(<wbr />dg1)<br /><br />to your Page_Load event. &nbsp;Do this for each GridView you want to add.<br /></span>

</div>

<div class="comment-right-col">
        <span class="comment-tail"></span>
        <ul class="comment-nav">                    
            
                         
                          <li><a href="">Reply</a></li>
                     
             <li>&nbsp;</li>              
        </ul>                  
        <a name="4410375"></a>      
        <h3>Centralize Web Content </h3>
        <p class="date">May 07, 2011 07:00 AM
        
        <div>
            
<p>When I minimize the IE window scale from 100 to 60 percent,&nbsp; how do I centralize the main content to my web page &nbsp;(e.g., http://www2.goldmansachs.com/, http://www.credit-When I agricole.com/)</p>
<p>The website is located at http://cforedu.com</p>
 
        </div>--%>
                        
</div>


</body>
</html>
