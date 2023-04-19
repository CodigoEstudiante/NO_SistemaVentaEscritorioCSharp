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
using System.Xml.Linq;
using static SistemaVentas.Reutilizable.EnumModelo;

namespace SistemaVentas
{
    public partial class frmRegistrarCompra : Form
    {
        public static object oObjecto;
        public frmRegistrarCompra()
        {
            InitializeComponent();
        }
        DataTable tabla = new DataTable();

        private void frmRegistrarCompra_Load(object sender, EventArgs e)
        {
            //AGREGAR BOTON ELIMINAR
            DataGridViewButtonColumn BotonElimar = new DataGridViewButtonColumn();

            BotonElimar.HeaderText = "Eliminar";
            BotonElimar.Width = 50;
            BotonElimar.Text = "Eliminar";
            BotonElimar.Name = "btnEliminar";
            BotonElimar.FlatStyle = FlatStyle.Flat;
            BotonElimar.UseColumnTextForButtonValue = true;
            BotonElimar.CellTemplate.Style.BackColor = Color.Red;
            BotonElimar.CellTemplate.Style.ForeColor = Color.White;
            BotonElimar.CellTemplate.Style.SelectionBackColor = Color.Red;
            BotonElimar.CellTemplate.Style.SelectionForeColor = Color.White;

            //AGREGAMOS LOS BOTONES
            dgvDetalleCompra.Columns.Add(BotonElimar);


            dgvDetalleCompra.Columns.Add("IdProveedor", "IdProveedor");
            dgvDetalleCompra.Columns.Add("Ruc Proveedor", "Ruc Proveedor");
            dgvDetalleCompra.Columns.Add("IdTienda", "IdTienda");
            dgvDetalleCompra.Columns.Add("Ruc Tienda", "Ruc Tienda");
            dgvDetalleCompra.Columns.Add("IdProducto", "IdProducto");
            dgvDetalleCompra.Columns.Add("Codigo Producto", "Codigo Producto");
            dgvDetalleCompra.Columns.Add("Cantidad", "Cantidad");
            dgvDetalleCompra.Columns.Add("PrecioCompra", "PrecioCompra");
            dgvDetalleCompra.Columns.Add("PrecioVenta", "PrecioVenta");

            dgvDetalleCompra.Columns["IdProveedor"].Visible = false;
            dgvDetalleCompra.Columns["IdTienda"].Visible = false;
            dgvDetalleCompra.Columns["IdProducto"].Visible = false;



            dgvDetalleCompra.MultiSelect = false;
            dgvDetalleCompra.ReadOnly = true;
            dgvDetalleCompra.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetalleCompra.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetalleCompra.AllowUserToAddRows = false;


        }
        private void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            oObjecto = null;
            frmBusqueda frm = new frmBusqueda(Modelo.Proveedor);
            frm.ShowDialog();

            if(oObjecto != null)
            {
                Proveedor oProveedor = (Proveedor)oObjecto;
                if (oProveedor != null)
                {
                    txtIdProveedor.Text = oProveedor.IdProveedor.ToString();
                    txtRucProveedor.Text = oProveedor.Ruc;
                    txtRazonSocialProveedor.Text = oProveedor.RazonSocial;
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
                    txtNombreTienda.Text = oTienda.Nombre;
                }
            }

        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            if(txtIdTienda.Text.Trim() == "0" && txtRucTienda.Text.Trim() == "" || txtNombreTienda.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione una tienda primero", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            oObjecto = null;
            frmBusqueda frm = new frmBusqueda(Modelo.Producto,int.Parse(txtIdTienda.Text.Trim()));
            frm.ShowDialog();
            
            if(oObjecto != null)
            {
                Producto oProducto = (Producto)oObjecto;
                if (oProducto != null)
                {
                    txtIdProducto.Text = oProducto.IdProducto.ToString();
                    txtCodigoProducto.Text = oProducto.Codigo;
                    txtNombreProducto.Text = oProducto.Nombre;
                }
            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            bool existeRegistro = false;
            //validar que los campos esten completos
            if (txtIdProducto.Text.Trim() == "0" || txtIdTienda.Text.Trim() == "0" || txtIdProducto.Text.Trim() == "0" ||
                txtCantidad.Value == 0|| txtPrecioCompra.Text.Trim() == "" || txtPrecioVenta.Text.Trim() == "")
            {
                MessageBox.Show("Debe completar toda los campos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            float PrecioUnidadCompra;
            float PrecioUnidadVenta;
            bool validate1 = float.TryParse(txtPrecioCompra.Text.Trim(), out PrecioUnidadCompra);
            bool validate2 = float.TryParse(txtPrecioVenta.Text.Trim(), out PrecioUnidadVenta);
            if(!(validate1 && validate2))
            {
                MessageBox.Show("Error al agregar el valor del precio,\nrevise el formato decimal tipo moneda", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            //validar que no exista el producto
            foreach (DataGridViewRow r in dgvDetalleCompra.Rows)
            {
                if(r.Cells["IdTienda"].Value.ToString() == txtIdTienda.Text.Trim() &&
                   r.Cells["IdProducto"].Value.ToString() == txtIdProducto.Text.Trim())
                {
                    existeRegistro = true;
                    break;
                }
            }

            if (!existeRegistro)
            {
                int rowId = dgvDetalleCompra.Rows.Add();
                DataGridViewRow row = dgvDetalleCompra.Rows[rowId];
                row.Cells["IdProveedor"].Value = txtIdProveedor.Text.Trim();
                row.Cells["Ruc Proveedor"].Value = txtRucProveedor.Text.Trim();
                row.Cells["IdTienda"].Value = txtIdTienda.Text.Trim();
                row.Cells["Ruc Tienda"].Value = txtRucTienda.Text.Trim();
                row.Cells["IdProducto"].Value = txtIdProducto.Text.Trim();
                row.Cells["Codigo Producto"].Value = txtCodigoProducto.Text.Trim();
                row.Cells["Cantidad"].Value = txtCantidad.Value.ToString();
                row.Cells["PrecioCompra"].Value = txtPrecioCompra.Text.Trim();
                row.Cells["PrecioVenta"].Value = txtPrecioVenta.Text.Trim();

                txtIdProducto.Text = "0";
                txtCodigoProducto.Text = "";
                txtNombreProducto.Text = "";

                txtCantidad.Value = 0;
                txtPrecioCompra.Text = "";
                txtPrecioVenta.Text = "";


            }
            else
            {
                MessageBox.Show("Compra Repetida.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            

        }

        private void dgvDetalleCompra_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalleCompra.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int index = e.RowIndex;
                if(index >= 0)
                {
                    dgvDetalleCompra.Rows.RemoveAt(index);
                }
            }
        }

        private void btnGuardarCompra_Click(object sender, EventArgs e)
        {
            if (dgvDetalleCompra.Rows.Count < 1)
            {
                MessageBox.Show("Debe añadir un detalle de producto en la compra", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            float totalCosto = 0;
            XElement Detalle_Compra = new XElement("DETALLE_COMPRA");
            foreach (DataGridViewRow row in dgvDetalleCompra.Rows)
            {
                Detalle_Compra.Add(new XElement("DETALLE",
                    new XElement("IdCompra", "0"),
                    new XElement("IdProducto", row.Cells["IdProducto"].Value),
                    new XElement("Cantidad", row.Cells["Cantidad"].Value),
                    new XElement("PrecioUnidadCompra", row.Cells["PrecioCompra"].Value),
                    new XElement("PrecioUnidadVenta", row.Cells["PrecioVenta"].Value),
                    new XElement("TotalCosto", float.Parse(row.Cells["PrecioCompra"].Value.ToString()) * float.Parse(row.Cells["Cantidad"].Value.ToString()))
                    ));
                totalCosto += float.Parse(row.Cells["PrecioCompra"].Value.ToString()) * float.Parse(row.Cells["Cantidad"].Value.ToString());
            }

            XElement RegistroCompra = new XElement("DETALLE",
               new XElement("COMPRA",
               new XElement("IdUsuario", Configuracion.oUsuario.IdUsuario),
               new XElement("IdProveedor", txtIdProveedor.Text.Trim()),
               new XElement("IdTienda", txtIdTienda.Text.Trim()),
               new XElement("TotalCosto", totalCosto))
               );

            RegistroCompra.Add(Detalle_Compra);

            bool Respuesta = CD_Compra.RegistrarCompra(RegistroCompra.ToString());

            if (Respuesta)
            {

                MessageBox.Show("La compra fue registrada correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtIdProveedor.Text = "0";
                txtRucProveedor.Text = "";
                txtRazonSocialProveedor.Text = "";

                txtIdTienda.Text = "0";
                txtRucTienda.Text = "";
                txtNombreTienda.Text = "";

                txtIdProducto.Text = "0";
                txtCodigoProducto.Text = "";
                txtNombreProducto.Text = "";

                txtCantidad.Value = 0;
                txtPrecioCompra.Text = "";
                txtPrecioVenta.Text = "";

                dgvDetalleCompra.DataSource = null;
                //dgvDetalleCompra.Columns.Clear();
                dgvDetalleCompra.Rows.Clear();
            }
            else
            {
                MessageBox.Show("Ocurrió un error al registrar la compra", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPrecioCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            char signo_decimal = (char)46;
            //Para obligar a que sólo se introduzcan números
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
                if (Char.IsControl(e.KeyChar) || e.KeyChar == signo_decimal) //permitir teclas de control como retroceso
                {
                    e.Handled = false;
                }
                else
                {
                    //el resto de teclas pulsadas se desactivan
                    e.Handled = true;
                }
        }

        private void txtPrecioVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            char signo_decimal = (char)46;
            //Para obligar a que sólo se introduzcan números
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
                if (Char.IsControl(e.KeyChar) || e.KeyChar == signo_decimal) //permitir teclas de control como retroceso
                {
                    e.Handled = false;
                }
                else
                {
                    //el resto de teclas pulsadas se desactivan
                    e.Handled = true;
                }
        }
    }
}
