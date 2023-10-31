CREATE PROCEDURE WEB_UsuariosIntranet_GetMenus
(
	@Id_Empleado	int
)
AS
BEGIN
	Select distinct	m.Id_Menu,
			m.Descripcion
	From	Usr_Menus		m
	Join	Usr_Opciones	o	on	o.Id_Menu		=	m.Id_Menu
								and	o.Status		=	'A'
	Join	Usr_Permisos	p	on	p.Id_Opcion		=	o.Id_Opcion
	Where	p.Id_Empleado	=	@Id_Empleado
	And		m.Status		=	'A'
	Order by
			m.Id_Menu
END
GO
CREATE PROCEDURE WEB_Usuarios_GetOpciones
(
	@Id_Empleado	int
)
AS
BEGIN
	Select	o.Id_Menu,
			o.Descripcion
	From	Usr_Opciones	o	
	Join	Usr_Permisos	p	on	p.Id_Opcion		=	o.Id_Opcion
	Where	p.Id_Empleado	=	@Id_Empleado
	And		o.Status		=	'A'
	Order by
			o.Id_Menu
END
