
go
use DBPRUEBAS
go
--REGISTROS EN TABLA ROL
INSERT INTO ROL(Descripcion) VALUES ('ADMINISTRADOR'),('EMPLEADO')

GO
--REGISTROS EN TABLA MENU
INSERT INTO MENU(Nombre,Icono) VALUES ('Mantenedor','icon_fix.png'),('Compras','icon_shop.png'),('Ventas','icon_sales.png'),('Reportes','icon_report.png')

GO
--REGISTROS EN TABLA SUBMENU
INSERT INTO SUBMENU(IdMenu,Nombre,NombreFormulario) VALUES
((SELECT TOP 1 IdMenu FROM MENU WHERE Nombre = 'Mantenedor'),'Rol','frmRol'),
((SELECT TOP 1 IdMenu FROM MENU WHERE Nombre = 'Mantenedor'),'Asignar rol permisos','frmRolPermiso'),
((SELECT TOP 1 IdMenu FROM MENU WHERE Nombre = 'Mantenedor'),'Usuario','frmUsuario'),
((SELECT TOP 1 IdMenu FROM MENU WHERE Nombre = 'Mantenedor'),'Proveedor','frmProveedor'),
((SELECT TOP 1 IdMenu FROM MENU WHERE Nombre = 'Mantenedor'),'Tienda','frmTienda'),
((SELECT TOP 1 IdMenu FROM MENU WHERE Nombre = 'Mantenedor'),'Categoria','frmCategoria'),
((SELECT TOP 1 IdMenu FROM MENU WHERE Nombre = 'Mantenedor'),'Producto','frmProducto'),
((SELECT TOP 1 IdMenu FROM MENU WHERE Nombre = 'Mantenedor'),'Asignar producto a tienda','frmProductoTienda'),
((SELECT TOP 1 IdMenu FROM MENU WHERE Nombre = 'Compras'),'Registrar','frmRegistrarCompra'),
((SELECT TOP 1 IdMenu FROM MENU WHERE Nombre = 'Compras'),'Consultas','frmConsultarCompra'),
((SELECT TOP 1 IdMenu FROM MENU WHERE Nombre = 'Ventas'),'Crear Nueva','frmCrearVenta'),
((SELECT TOP 1 IdMenu FROM MENU WHERE Nombre = 'Ventas'),'Consultar','frmConsultarVenta'),
((SELECT TOP 1 IdMenu FROM MENU WHERE Nombre = 'Reportes'),'Productos por tienda','rptProductoTienda'),
((SELECT TOP 1 IdMenu FROM MENU WHERE Nombre = 'Reportes'),'Ventas','rptVentas')
GO
--REGISTROS EN TABLA TIENDA
INSERT INTO TIENDA(Nombre,RUC,Direccion,Telefono) VALUES ('Tienda 001','25689789654','AV.GRANDE 123','963852896')

GO
--REGISTROS USUARIO
insert into usuario(Nombres,Apellidos,Correo,Usuario,Clave,IdTienda,IdRol)
values('Eren','Thopsom','snk@gmail.com','Admin','Admin123',(select TOP 1 IdTienda from TIENDA where Nombre = 'Tienda 001'),(select TOP 1 IdRol from ROL where Descripcion = 'ADMINISTRADOR'))
go
insert into usuario(Nombres,Apellidos,Correo,Usuario,Clave,IdTienda,IdRol)
values('Mika','azgun','snk@gmail.com','Tienda','Tienda123',(select TOP 1 IdTienda from TIENDA where Nombre = 'Tienda 001'),(select TOP 1 IdRol from ROL where Descripcion = 'EMPLEADO'))


GO
--REGISTROS EN TABLA PERMISOS
INSERT INTO PERMISOS(IdRol,IdSubMenu)
SELECT (select TOP 1 IdRol from ROL where Descripcion = 'ADMINISTRADOR'), IdSubMenu FROM SUBMENU
go
INSERT INTO PERMISOS(IdRol,IdSubMenu,Activo)
SELECT (select TOP 1 IdRol from ROL where Descripcion = 'EMPLEADO'), IdSubMenu, 0 FROM SUBMENU 

go

update p set p.Activo = 1 from PERMISOS p
inner join SUBMENU sm on sm.IdSubMenu = p.IdSubMenu
where sm.NombreFormulario in ('frmCrearVenta','frmConsultarVenta') and p.IdRol = (select TOP 1 IdRol from ROL where Descripcion = 'EMPLEADO')


GO
--REGISTRO EN TABLA CATEGORIA
INSERT INTO CATEGORIA(Descripcion) VALUES
('Bebidas'),
('Frutas'),
('Embutidos'),
('Lacteos')

GO
--REGISTRO EN TABLA PRODUCTO
INSERT INTO PRODUCTO(Codigo,ValorCodigo,Nombre,Descripcion,IdCategoria)
values
(
RIGHT('000000' + convert(varchar(max),(select isnull(max(ValorCodigo),0) + 1 from PRODUCTO)),6),
(select isnull(max(ValorCodigo),0) + 1 from PRODUCTO),
'Inca Kola',
'Gaseosa 3Lts',
(select top 1 IdCategoria from CATEGORIA where Descripcion = 'Bebidas')
)
GO
INSERT INTO PRODUCTO(Codigo,ValorCodigo,Nombre,Descripcion,IdCategoria)
values
(
RIGHT('000000' + convert(varchar(max),(select isnull(max(ValorCodigo),0) + 1 from PRODUCTO)),6),
(select isnull(max(ValorCodigo),0) + 1 from PRODUCTO),
'Mantequilla Gloria',
'bote de 500 mg',
(select top 1 IdCategoria from CATEGORIA where Descripcion = 'Embutidos')
)
GO
INSERT INTO PRODUCTO(Codigo,ValorCodigo,Nombre,Descripcion,IdCategoria)
values
(
RIGHT('000000' + convert(varchar(max),(select isnull(max(ValorCodigo),0) + 1 from PRODUCTO)),6),
(select isnull(max(ValorCodigo),0) + 1 from PRODUCTO),
'Coca Cola',
'1.5 Lts',
(select top 1 IdCategoria from CATEGORIA where Descripcion = 'Bebidas')
)
GO
INSERT INTO PRODUCTO(Codigo,ValorCodigo,Nombre,Descripcion,IdCategoria)
values
(
RIGHT('000000' + convert(varchar(max),(select isnull(max(ValorCodigo),0) + 1 from PRODUCTO)),6),
(select isnull(max(ValorCodigo),0) + 1 from PRODUCTO),
'Mermelada Fanny',
'Bote 310g',
(select top 1 IdCategoria from CATEGORIA where Descripcion = 'Embutidos')
)
GO
INSERT INTO PRODUCTO(Codigo,ValorCodigo,Nombre,Descripcion,IdCategoria)
values
(
RIGHT('000000' + convert(varchar(max),(select isnull(max(ValorCodigo),0) + 1 from PRODUCTO)),6),
(select isnull(max(ValorCodigo),0) + 1 from PRODUCTO),
'Queso Perfecta',
'Empaque 350g',
(select top 1 IdCategoria from CATEGORIA where Descripcion = 'Lacteos')
)


