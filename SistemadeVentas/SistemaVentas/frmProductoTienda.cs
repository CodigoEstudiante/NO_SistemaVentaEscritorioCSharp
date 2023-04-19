using CapaDatos;
using CapaModelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SistemaVentas.Reutilizable.EnumModelo;

namespace SistemaVentas
{
    public partial class frmProductoTienda : Form
    {
        public static object oObjecto;
        DataTable tablaProductoTienda = new DataTable();
        public frmProductoTienda()
        {
            InitializeComponent();
        }
        private void frmProductoTienda_Load(object sender, EventArgs e)
        {
            DiseñoInicial();
            
            dgvProductoTienda.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductoTienda.MultiSelect = false;
            dgvProductoTienda.ReadOnly = true;
            dgvProductoTienda.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductoTienda.AllowUserToAddRows = false;
            CargarDatos();
        }
        private void txtCodigoProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                List<Producto> oListaProducto = CD_Producto.ObtenerProducto();
                if(oListaProducto.Count > 0 && oListaProducto != null)
                {
                    Producto oProducto = oListaProducto.Where(x => x.Codigo.Equals(txtCodigoProducto.Text.Trim())).FirstOrDefault();
                    if(oProducto != null)
                    {
                        txtIdProducto.Text = oProducto.IdProducto.ToString();
                        txtNombreProducto.Text = oProducto.Nombre;
                        txtDescripcionProducto.Text = oProducto.Descripcion;
                    }
                    else
                    {
                        txtIdProducto.Text = "0";
                        txtCodigoProducto.Text = "";
                        txtNombreProducto.Text = "";
                        txtDescripcionProducto.Text = "";
                        MessageBox.Show("No se encontraron resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("No se encontraron resultados","Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                }
            }
        }

        private void txtRucTienda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                List<Tienda> oListaTienda = CD_Tienda.ObtenerTiendas();
                if (oListaTienda.Count > 0 && oListaTienda != null)
                {
                    Tienda oTienda = oListaTienda.Where(x => x.RUC.Equals(txtRucTienda.Text.Trim())).FirstOrDefault();
                    if (oTienda != null)
                    {
                        txtIdTienda.Text = oTienda.IdTienda.ToString();
                        txtRazonSocial.Text = oTienda.Nombre;
                        txtDireccionTienda.Text = oTienda.Direccion;
                    }
                    else
                    {
                        txtIdTienda.Text = "0";
                        txtRucTienda.Text = "";
                        txtRazonSocial.Text = "";
                        txtDireccionTienda.Text = "";
                        MessageBox.Show("No se encontraron resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("No se encontraron resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            oObjecto = null;
            frmBusqueda frm = new frmBusqueda(Modelo.Producto);
            frm.ShowDialog();
            
            if(oObjecto != null)
            {
                Producto oProducto = (Producto)oObjecto;
                if (oProducto != null)
                {
                    txtIdProducto.Text = oProducto.IdProducto.ToString();
                    txtCodigoProducto.Text = oProducto.Codigo;
                    txtNombreProducto.Text = oProducto.Nombre;
                    txtDescripcionProducto.Text = oProducto.Descripcion;
                }
            }
            
            

        }

        private void btnBuscarTienda_Click(object sender, EventArgs e)
        {
            oObjecto = null;
            frmBusqueda frm = new frmBusqueda(Modelo.Tienda);
            frm.ShowDialog();

            if(oObjecto != null)
            {
                Tienda oTienda = (Tienda)oObjecto;
                if (oTienda != null)
                {
                    txtIdTienda.Text = oTienda.IdTienda.ToString();
                    txtRucTienda.Text = oTienda.RUC;
                    txtRazonSocial.Text = oTienda.Nombre;
                    txtDireccionTienda.Text = oTienda.Direccion;
                }
            }
        }

        

        private void CargarDatos()
        {

            List<ProductoTienda> oListaProductoTienda = CD_ProductoTienda.ObtenerProductoTienda();
            if (oListaProductoTienda == null)
                return;

            if (oListaProductoTienda.Count > 0)
            {
                dgvProductoTienda.DataSource = null;
                dgvProductoTienda.Rows.Clear();
                dgvProductoTienda.Columns.Clear();

                lblTotalRegistros.Text = oListaProductoTienda.Count.ToString();
                tablaProductoTienda = new DataTable();
                tablaProductoTienda.Columns.Clear();
                tablaProductoTienda.Rows.Clear();
                
                cboFiltro.Items.Clear();

                
                tablaProductoTienda.Columns.Add("IdProductoTienda", typeof(int));
                tablaProductoTienda.Columns.Add("IdProducto", typeof(string));
                tablaProductoTienda.Columns.Add("Codigo Producto", typeof(string));
                tablaProductoTienda.Columns.Add("Nombre Producto", typeof(string));
                tablaProductoTienda.Columns.Add("Descripcion Producto", typeof(string));
                tablaProductoTienda.Columns.Add("IdTienda", typeof(string));
                tablaProductoTienda.Columns.Add("RUC Tienda", typeof(string));
                tablaProductoTienda.Columns.Add("Nombre Tienda", typeof(string));
                tablaProductoTienda.Columns.Add("Direccion Tienda", typeof(string));
                tablaProductoTienda.Columns.Add("Stock", typeof(string));
                tablaProductoTienda.Columns.Add("Iniciado", typeof(bool));

                //AGREGAR BOTON EDITAR
                DataGridViewButtonColumn BotonEditar = new DataGridViewButtonColumn();

                BotonEditar.HeaderText = "Editar";
                BotonEditar.Width = 50;
                BotonEditar.Text = "Editar";
                BotonEditar.Name = "btnEditar";
                BotonEditar.FlatStyle = FlatStyle.Flat;
                BotonEditar.UseColumnTextForButtonValue = true;
                BotonEditar.CellTemplate.Style.BackColor = Color.AliceBlue;
                BotonEditar.CellTemplate.Style.SelectionBackColor = Color.CadetBlue;

                //AGREGAR BOTON ELIMINAR
                DataGridViewButtonColumn BotonElimar = new DataGridViewButtonColumn();

                BotonElimar.HeaderText = "Eliminar";
                BotonElimar.Width = 50;
                BotonElimar.Text = "Eliminar";
                BotonElimar.Name = "btnEliminar";
                BotonElimar.FlatStyle = FlatStyle.Flat;
                BotonElimar.UseColumnTextForButtonValue = true;
                BotonElimar.CellTemplate.Style.BackColor = Color.AliceBlue;
                BotonElimar.CellTemplate.Style.SelectionBackColor = Color.CadetBlue;

                //AGREGAMOS LOS BOTONES
                dgvProductoTienda.Columns.Add(BotonEditar);
                dgvProductoTienda.Columns.Add(BotonElimar);

                foreach (ProductoTienda row in oListaProductoTienda)
                {
                    tablaProductoTienda.Rows.Add(row.IdProductoTienda, 
                        row.oProducto.IdProducto, row.oProducto.Codigo, row.oProducto.Nombre, row.oProducto.Descripcion,
                        row.oTienda.IdTienda, row.oTienda.RUC, row.oTienda.Nombre, row.oTienda.Direccion,
                        row.Stock.ToString(),row.Iniciado);
                }

                dgvProductoTienda.DataSource = tablaProductoTienda;


                dgvProductoTienda.Columns["IdProductoTienda"].Visible = false;
                dgvProductoTienda.Columns["IdProducto"].Visible = false;
                dgvProductoTienda.Columns["Descripcion Producto"].Visible = false;
                dgvProductoTienda.Columns["IdTienda"].Visible = false;
                dgvProductoTienda.Columns["Direccion Tienda"].Visible = false;
                dgvProductoTienda.Columns["Iniciado"].Visible = false;

                foreach (DataGridViewColumn cl in dgvProductoTienda.Columns)
                {
                    if (cl.Visible == true && cl.HeaderText != "Editar" && cl.HeaderText != "Eliminar")
                    {
                        cboFiltro.Items.Add(cl.HeaderText);
                    }
                }
                cboFiltro.SelectedIndex = 0;

                dgvProductoTienda.ClearSelection();
            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if(txtIdProducto.Text.Trim() == "0" && txtCodigoProducto.Text.Trim()== "" && txtNombreProducto.Text.Trim() == ""
                && txtIdTienda.Text.Trim() == "0" && txtRucTienda.Text.Trim() == "" && txtRazonSocial.Text.Trim() == "")
            {
                MessageBox.Show("Debe completar todos los campos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            ProductoTienda oProductoTienda = new ProductoTienda() {
                oProducto = new Producto() { IdProducto = int.Parse(txtIdProducto.Text.Trim())},
                oTienda = new Tienda() { IdTienda = int.Parse(txtIdTienda.Text.Trim())}
            };
            
            bool Respuesta = CD_ProductoTienda.RegistrarProductoTienda(oProductoTienda);

            if (Respuesta)
            {
                MessageBox.Show("Asignación Registrada", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
                DiseñoInicial();
            }
            else
            {
                MessageBox.Show("No se pudo registrar la asignación,\nes posible que ya se encuentre registrado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            if (txtIdProducto.Text.Trim() == "0" && txtCodigoProducto.Text.Trim() == "" && txtNombreProducto.Text.Trim() == ""
                && txtIdTienda.Text.Trim() == "0" && txtRucTienda.Text.Trim() == "" && txtRazonSocial.Text.Trim() == "")
            {
                MessageBox.Show("Debe completar todos los campos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            ProductoTienda oProductoTienda = new ProductoTienda()
            {
                IdProductoTienda = int.Parse(txtIdProductoTienda.Text.Trim()),
                oProducto = new Producto() { IdProducto = int.Parse(txtIdProducto.Text.Trim()) },
                oTienda = new Tienda() { IdTienda = int.Parse(txtIdTienda.Text.Trim()) }
            };

            bool Respuesta = CD_ProductoTienda.ModificarProductoTienda(oProductoTienda);

            if (Respuesta)
            {
                MessageBox.Show("Asignación Modificada", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
                DiseñoInicial();

            }
            else
            {
                MessageBox.Show("No se pudo modificar la asignación,\nes posible que ya se encuentre registrado stock", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void dgvProductoTienda_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (dgvProductoTienda.Columns[e.ColumnIndex].Name == "btnEditar")
            {
                btnAgregar.Visible = false;
                btnGuardarCambios.Visible = true;
                btnCancelar.Visible = true;

                txtIdProductoTienda.Text = dgvProductoTienda.Rows[index].Cells["IdProductoTienda"].Value.ToString();

                txtIdProducto.Text = dgvProductoTienda.Rows[index].Cells["IdProducto"].Value.ToString();
                txtCodigoProducto.Text = dgvProductoTienda.Rows[index].Cells["Codigo Producto"].Value.ToString();
                txtNombreProducto.Text = dgvProductoTienda.Rows[index].Cells["Nombre Producto"].Value.ToString();
                txtDescripcionProducto.Text = dgvProductoTienda.Rows[index].Cells["Descripcion Producto"].Value.ToString();


                txtIdTienda.Text = dgvProductoTienda.Rows[index].Cells["IdTienda"].Value.ToString();
                txtRucTienda.Text = dgvProductoTienda.Rows[index].Cells["RUC Tienda"].Value.ToString();
                txtRazonSocial.Text = dgvProductoTienda.Rows[index].Cells["Nombre Tienda"].Value.ToString();
                txtDireccionTienda.Text = dgvProductoTienda.Rows[index].Cells["Direccion Tienda"].Value.ToString();

            }

            if (dgvProductoTienda.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int IdProductoTienda = Convert.ToInt32(dgvProductoTienda.Rows[index].Cells["IdProductoTienda"].Value);
      
                if (MessageBox.Show("¿Desea eliminar la asignación?", "Mensaje", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    bool Respuesta = CD_ProductoTienda.EliminarProductoTienda(IdProductoTienda);
                    if (Respuesta)
                    {
                        MessageBox.Show("Asignación eliminada", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarDatos();
                    }
                    else
                    {
                        MessageBox.Show("No se puedo eliminar la asignación,\nes posible que ya cuente con stock en tienda", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }
            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DiseñoInicial();
        }

        private void DiseñoInicial()
        {
            txtIdProductoTienda.Text = "0";

            txtIdProducto.Text = "0";
            txtCodigoProducto.Text = "";
            txtNombreProducto.Text = "";
            txtDescripcionProducto.Text = "";

            txtIdTienda.Text = "0";
            txtRucTienda.Text = "";
            txtRazonSocial.Text = "";
            txtDireccionTienda.Text = "";

            btnAgregar.Visible = true;
            btnGuardarCambios.Visible = false;
            btnCancelar.Visible = false;
            dgvProductoTienda.ClearSelection();

        }
    }
}
