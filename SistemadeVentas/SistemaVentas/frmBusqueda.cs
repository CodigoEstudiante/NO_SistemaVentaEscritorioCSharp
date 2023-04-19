using CapaDatos;
using CapaModelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SistemaVentas.Reutilizable.EnumModelo;

namespace SistemaVentas
{
    public partial class frmBusqueda : Form
    {
        private DataTable tabla = new DataTable();
        private Modelo ModeloSeleccionado;
        private int IdFiltro = 0;
        public frmBusqueda(Modelo oModelo,int pIdFiltro = 0)
        {
            InitializeComponent();
            ModeloSeleccionado = oModelo;
            IdFiltro = pIdFiltro;
            if (oModelo == Modelo.Producto)
            {
                BusquedaProducto();
            }
            else if (oModelo == Modelo.Tienda)
            {
                BusquedaTienda();
            }
            else if (oModelo == Modelo.Proveedor)
            {
                BusquedaProveedor();
            }
            else if (oModelo == Modelo.ProductoTienda)
            {
                BusquedaProductoTienda();
            }
        }


        private void frmBusqueda_Load(object sender, EventArgs e)
        {

            dgvLista.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLista.MultiSelect = false;
            dgvLista.ReadOnly = true;
            dgvLista.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLista.AllowUserToAddRows = false;
        }
        //*********************************** BUSQUEDA PRODUCTO *****************************************
        private void BusquedaProducto()
        {
            lblDescripcionLista.Text = "Lista de Productos";
            List<Producto> oListaProducto = CD_Producto.ObtenerProducto();
            List<ProductoTienda> oListaProductoTienda = CD_ProductoTienda.ObtenerProductoTienda();

            oListaProducto = oListaProducto.Where(x => x.Activo == true).ToList();
            if (IdFiltro != 0)
            {
                oListaProductoTienda = oListaProductoTienda.Where(x => x.oTienda.IdTienda == IdFiltro).ToList();
                oListaProducto = (from producto in oListaProducto
                                  join productotienda in oListaProductoTienda on producto.IdProducto equals productotienda.oProducto.IdProducto
                                  where productotienda.oTienda.IdTienda == IdFiltro
                                  select producto).ToList();
            }

            if (oListaProducto.Count > 0 && oListaProducto != null)
            {
                lblTotalRegistros.Text = oListaProducto.Count.ToString();
                tabla = new DataTable();
                tabla.Columns.Clear();
                tabla.Rows.Clear();

                tabla.Columns.Add("IdProducto", typeof(int));
                tabla.Columns.Add("Codigo", typeof(string));
                tabla.Columns.Add("Nombre", typeof(string));
                tabla.Columns.Add("Descripcion", typeof(string));
                tabla.Columns.Add("Categoria", typeof(string));


                //AGREGAR BOTON SELECCIONAR
                DataGridViewButtonColumn BotonSeleccionar = new DataGridViewButtonColumn();
                BotonSeleccionar.HeaderText = "Seleccionar";
                BotonSeleccionar.Text = "Seleccionar";
                BotonSeleccionar.Width = 70;
                BotonSeleccionar.Name = "btnSeleccionar";
                BotonSeleccionar.FlatStyle = FlatStyle.Flat;
                BotonSeleccionar.UseColumnTextForButtonValue = true;
                BotonSeleccionar.CellTemplate.Style.BackColor = Color.AliceBlue;
                BotonSeleccionar.CellTemplate.Style.SelectionBackColor = Color.CadetBlue;
                //AGREGAMOS EL BOTON
                dgvLista.Columns.Add(BotonSeleccionar);

                foreach (Producto row in oListaProducto)
                {
                    tabla.Rows.Add(row.IdProducto, row.Codigo, row.Nombre, row.Descripcion, row.oCategoria.Descripcion);
                }
                dgvLista.DataSource = tabla;
                dgvLista.Columns["IdProducto"].Visible = false;

                foreach (DataGridViewColumn cl in dgvLista.Columns)
                {
                    if (cl.Visible == true && cl.HeaderText != "Seleccionar")
                        cboFiltro.Items.Add(cl.HeaderText);
                }
                cboFiltro.SelectedIndex = 0;
            }
        }
        //*********************************** BUSQUEDA TIENDA *******************************************
        private void BusquedaTienda()
        {
            lblDescripcionLista.Text = "Lista de Tiendas";
            List<Tienda> oListaTienda = CD_Tienda.ObtenerTiendas();
            oListaTienda = oListaTienda.Where(x => x.Activo == true).ToList();
            if (oListaTienda.Count > 0)
            {
                lblTotalRegistros.Text = oListaTienda.Count.ToString();

                tabla = new DataTable();
                tabla.Columns.Clear();
                tabla.Rows.Clear();
                cboFiltro.Items.Clear();

                tabla.Columns.Add("IdTienda", typeof(int));
                tabla.Columns.Add("RUC", typeof(string));
                tabla.Columns.Add("Razon Social", typeof(string));
                tabla.Columns.Add("Direccion", typeof(string));

                //AGREGAR BOTON SELECCIONAR
                DataGridViewButtonColumn BotonSeleccionar = new DataGridViewButtonColumn();
                BotonSeleccionar.HeaderText = "Seleccionar";
                BotonSeleccionar.Text = "Seleccionar";
                BotonSeleccionar.Width = 70;
                BotonSeleccionar.Name = "btnSeleccionar";
                BotonSeleccionar.FlatStyle = FlatStyle.Flat;
                BotonSeleccionar.UseColumnTextForButtonValue = true;
                BotonSeleccionar.CellTemplate.Style.BackColor = Color.AliceBlue;
                BotonSeleccionar.CellTemplate.Style.SelectionBackColor = Color.CadetBlue;
                //AGREGAMOS EL BOTON
                dgvLista.Columns.Add(BotonSeleccionar);

                foreach (Tienda row in oListaTienda)
                {
                    tabla.Rows.Add(row.IdTienda, row.RUC, row.Nombre, row.Direccion);
                }

                dgvLista.DataSource = tabla;

                dgvLista.Columns["IdTienda"].Visible = false;

                foreach (DataGridViewColumn cl in dgvLista.Columns)
                {
                    if (cl.Visible == true && cl.HeaderText != "Seleccionar")
                        cboFiltro.Items.Add(cl.HeaderText);
                }
                cboFiltro.SelectedIndex = 0;
            }

        }
        //*********************************** BUSQUEDA PROVEEDOR ****************************************
        private void BusquedaProveedor()
        {
            lblDescripcionLista.Text = "Lista de Proveedores";
            List<Proveedor> oListaProveedor = CD_Proveedor.ObtenerProveedor();
            oListaProveedor = oListaProveedor.Where(x => x.Activo == true).ToList();
            if (oListaProveedor.Count > 0)
            {
                lblTotalRegistros.Text = oListaProveedor.Count.ToString();

                tabla = new DataTable();
                tabla.Columns.Clear();
                tabla.Rows.Clear();
                cboFiltro.Items.Clear();

                tabla.Columns.Add("IdProveedor", typeof(int));
                tabla.Columns.Add("RUC", typeof(string));
                tabla.Columns.Add("Razon Social", typeof(string));
                tabla.Columns.Add("Telefono", typeof(string));
                tabla.Columns.Add("Correo", typeof(string));
                tabla.Columns.Add("Direccion", typeof(string));

                //AGREGAR BOTON SELECCIONAR
                DataGridViewButtonColumn BotonSeleccionar = new DataGridViewButtonColumn();
                BotonSeleccionar.HeaderText = "Seleccionar";
                BotonSeleccionar.Text = "Seleccionar";
                BotonSeleccionar.Width = 70;
                BotonSeleccionar.Name = "btnSeleccionar";
                BotonSeleccionar.FlatStyle = FlatStyle.Flat;
                BotonSeleccionar.UseColumnTextForButtonValue = true;
                BotonSeleccionar.CellTemplate.Style.BackColor = Color.AliceBlue;
                BotonSeleccionar.CellTemplate.Style.SelectionBackColor = Color.CadetBlue;
                //AGREGAMOS EL BOTON
                dgvLista.Columns.Add(BotonSeleccionar);

                foreach (Proveedor row in oListaProveedor)
                {
                    tabla.Rows.Add(row.IdProveedor, row.Ruc, row.RazonSocial, row.Telefono, row.Correo, row.Direccion);
                }

                dgvLista.DataSource = tabla;


                dgvLista.Columns["IdProveedor"].Visible = false;

                foreach (DataGridViewColumn cl in dgvLista.Columns)
                {
                    if (cl.Visible == true && cl.HeaderText != "Seleccionar")
                    {
                        cboFiltro.Items.Add(cl.HeaderText);
                    }
                }
                cboFiltro.SelectedIndex = 0;
            }

        }

        //*********************************** BUSQUEDA PRoDUCTO TIENDA **********************************
        private void BusquedaProductoTienda()
        {
            lblDescripcionLista.Text = "Lista de productos por tienda";
            List<ProductoTienda> oListaProductoTienda = CD_ProductoTienda.ObtenerProductoTienda();
            oListaProductoTienda = oListaProductoTienda.Where(x => x.oTienda.IdTienda == IdFiltro && x.Stock > 0).ToList();
            if (oListaProductoTienda == null)
                return;

            if (oListaProductoTienda.Count > 0)
            {
                dgvLista.DataSource = null;
                dgvLista.Rows.Clear();
                dgvLista.Columns.Clear();

                lblTotalRegistros.Text = oListaProductoTienda.Count.ToString();
                tabla = new DataTable();
                tabla.Columns.Clear();
                tabla.Rows.Clear();

                cboFiltro.Items.Clear();


                tabla.Columns.Add("IdProductoTienda", typeof(int));
                tabla.Columns.Add("IdProducto", typeof(string));
                tabla.Columns.Add("Codigo Producto", typeof(string));
                tabla.Columns.Add("Nombre Producto", typeof(string));
                tabla.Columns.Add("Descripcion Producto", typeof(string));
                tabla.Columns.Add("Precio Unidad Compra", typeof(string));
                tabla.Columns.Add("Precio Unidad Venta", typeof(string));
                tabla.Columns.Add("Stock", typeof(string));

                //AGREGAR BOTON SELECCIONAR
                DataGridViewButtonColumn BotonSeleccionar = new DataGridViewButtonColumn();
                BotonSeleccionar.HeaderText = "Seleccionar";
                BotonSeleccionar.Text = "Seleccionar";
                BotonSeleccionar.Width = 70;
                BotonSeleccionar.Name = "btnSeleccionar";
                BotonSeleccionar.FlatStyle = FlatStyle.Flat;
                BotonSeleccionar.UseColumnTextForButtonValue = true;
                BotonSeleccionar.CellTemplate.Style.BackColor = Color.AliceBlue;
                BotonSeleccionar.CellTemplate.Style.SelectionBackColor = Color.CadetBlue;
                //AGREGAMOS EL BOTON
                dgvLista.Columns.Add(BotonSeleccionar);

                foreach (ProductoTienda row in oListaProductoTienda)
                {
                    tabla.Rows.Add(row.IdProductoTienda,
                        row.oProducto.IdProducto, row.oProducto.Codigo, row.oProducto.Nombre, row.oProducto.Descripcion,
                        row.PrecioUnidadCompra, row.PrecioUnidadVenta,row.Stock.ToString());
                }
                dgvLista.DataSource = tabla;

                dgvLista.Columns["IdProductoTienda"].Visible = false;
                dgvLista.Columns["IdProducto"].Visible = false;
                dgvLista.Columns["Precio Unidad Compra"].Visible = false;
                
                foreach (DataGridViewColumn cl in dgvLista.Columns)
                {
                    if (cl.Visible == true && cl.HeaderText != "Seleccionar")
                    {
                        cboFiltro.Items.Add(cl.HeaderText);
                    }
                }
                cboFiltro.SelectedIndex = 0;

                dgvLista.ClearSelection();
            }

        }


        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string columnaFiltro = cboFiltro.SelectedItem.ToString();
            (dgvLista.DataSource as DataTable).DefaultView.RowFilter = string.Format("[" +columnaFiltro + "]" + " like '%{0}%'", txtFilter.Text);
        }

        private void dgvLista_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvLista.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                if (ModeloSeleccionado == Modelo.Producto)
                {
                    Producto oProducto = new Producto()
                    {
                        IdProducto = Convert.ToInt32(dgvLista.Rows[e.RowIndex].Cells["IdProducto"].Value.ToString()),
                        Codigo = dgvLista.Rows[e.RowIndex].Cells["Codigo"].Value.ToString(),
                        Nombre = dgvLista.Rows[e.RowIndex].Cells["Nombre"].Value.ToString(),
                        Descripcion = dgvLista.Rows[e.RowIndex].Cells["Descripcion"].Value.ToString()

                    };
                    frmProductoTienda.oObjecto = null;
                    frmRegistrarCompra.oObjecto = null;
                    frmProductoTienda.oObjecto = oProducto;
                    frmRegistrarCompra.oObjecto = oProducto;
                }
                else if (ModeloSeleccionado == Modelo.Tienda)
                {
                    Tienda oTienda = new Tienda()
                    {
                        IdTienda = Convert.ToInt32(dgvLista.Rows[e.RowIndex].Cells["IdTienda"].Value.ToString()),
                        RUC = dgvLista.Rows[e.RowIndex].Cells["RUC"].Value.ToString(),
                        Nombre = dgvLista.Rows[e.RowIndex].Cells["Razon Social"].Value.ToString(),
                        Direccion = dgvLista.Rows[e.RowIndex].Cells["Direccion"].Value.ToString()
                    };
                    frmProductoTienda.oObjecto = null;
                    frmRegistrarCompra.oObjecto = null;
                    frmProductoTienda.oObjecto = oTienda;
                    frmRegistrarCompra.oObjecto = oTienda;
                }
                else if (ModeloSeleccionado == Modelo.Proveedor)
                {
                    Proveedor oProveedor = new Proveedor()
                    {
                        IdProveedor = Convert.ToInt32(dgvLista.Rows[e.RowIndex].Cells["IdProveedor"].Value.ToString()),
                        Ruc = dgvLista.Rows[e.RowIndex].Cells["RUC"].Value.ToString(),
                        RazonSocial = dgvLista.Rows[e.RowIndex].Cells["Razon Social"].Value.ToString(),
                    };
                    frmRegistrarCompra.oObjecto = null;
                    frmRegistrarCompra.oObjecto = oProveedor;
                }
                else if (ModeloSeleccionado == Modelo.ProductoTienda)
                {
                    ProductoTienda oProductoTienda = new ProductoTienda()
                    {
                        oProducto = new Producto()
                        {
                            IdProducto = Convert.ToInt32(dgvLista.Rows[e.RowIndex].Cells["IdProducto"].Value.ToString()),
                            Codigo = dgvLista.Rows[e.RowIndex].Cells["Codigo Producto"].Value.ToString(),
                            Nombre = dgvLista.Rows[e.RowIndex].Cells["Nombre Producto"].Value.ToString(),
                            Descripcion = dgvLista.Rows[e.RowIndex].Cells["Descripcion Producto"].Value.ToString()
                        },
                        PrecioUnidadCompra = float.Parse(dgvLista.Rows[e.RowIndex].Cells["Precio Unidad Compra"].Value.ToString()),
                        PrecioUnidadVenta = float.Parse(dgvLista.Rows[e.RowIndex].Cells["Precio Unidad Venta"].Value.ToString()),
                        Stock = float.Parse(dgvLista.Rows[e.RowIndex].Cells["Stock"].Value.ToString())
                    };
                    frmCrearVenta.oObjecto = null;
                    frmCrearVenta.oObjecto = oProductoTienda;
                }

                this.Close();
            }
        }
    }
}
