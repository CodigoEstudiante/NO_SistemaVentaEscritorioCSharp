using CapaDatos;
using CapaModelo;
using SistemaVentas.Reutilizable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static SistemaVentas.Reutilizable.EnumModelo;

namespace SistemaVentas
{
    public partial class frmCrearVenta : Form
    {
        public static object oObjecto;
        float SubTotal = 0;
        float IGV = 0;
        float Total = 0;
        public frmCrearVenta()
        {
            InitializeComponent();
        }

        private void frmCrearVenta_Load(object sender, EventArgs e)
        {
            Usuario oUsuario = Configuracion.oUsuario;

            DateTime hoy = DateTime.Now;
            txtFechaVenta.Text = hoy.ToString("dd/MM/yyyy");

            txtIdEmpleado.Text = oUsuario.IdUsuario.ToString();
            txtEmpleadoNombre.Text = oUsuario.Nombres.ToString();
            txtEmpleadoApellidos.Text = oUsuario.Apellidos.ToString();
            txtEmpleadoCorreo.Text = oUsuario.Correo.ToString();

            lblRuc.Text = oUsuario.oTienda.RUC.ToString();
            txtIdTienda.Text = oUsuario.oTienda.IdTienda.ToString();
            txtTiendaNombre.Text = oUsuario.oTienda.Nombre.ToString();
            txtTiendaRuc.Text = oUsuario.oTienda.RUC.ToString();
            txtTiendaDireccion.Text = oUsuario.oTienda.Direccion.ToString();

            cboTipoDocumentoVenta.Items.Add(new ComboBoxItem() { Value = "Boleta", Text = "Boleta" });
            cboTipoDocumentoVenta.Items.Add(new ComboBoxItem() { Value = "Factura", Text = "Factura" });
            cboTipoDocumentoVenta.DisplayMember = "Text";
            cboTipoDocumentoVenta.ValueMember = "Value";
            cboTipoDocumentoVenta.SelectedIndex = 0;

            cboTipoDocumentoCliente.Items.Add(new ComboBoxItem() { Value = "DNI", Text = "DNI" });
            cboTipoDocumentoCliente.Items.Add(new ComboBoxItem() { Value = "Carnet Extranjeria", Text = "Carnet Extranjeria" });
            cboTipoDocumentoCliente.Items.Add(new ComboBoxItem() { Value = "RUC", Text = "RUC" });
            cboTipoDocumentoCliente.DisplayMember = "Text";
            cboTipoDocumentoCliente.ValueMember = "Value";
            cboTipoDocumentoCliente.SelectedIndex = 0;



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
            dgvVenta.Columns.Add(BotonElimar);


            dgvVenta.Columns.Add("IdProducto", "IdProducto");
            dgvVenta.Columns.Add("Cantidad", "Cantidad");
            dgvVenta.Columns.Add("Producto", "Producto");
            dgvVenta.Columns.Add("Descripción", "Descripción");
            dgvVenta.Columns.Add("PrecioUnidad", "Precio Unidad");
            dgvVenta.Columns.Add("Precio Unidad", "Precio Unidad");
            dgvVenta.Columns.Add("ImporteTotal", "Importe Total");
            dgvVenta.Columns.Add("Importe Total", "Importe Total");


            dgvVenta.Columns["IdProducto"].Visible = false;
            dgvVenta.Columns["PrecioUnidad"].Visible = false;
            dgvVenta.Columns["ImporteTotal"].Visible = false;



            dgvVenta.MultiSelect = false;
            dgvVenta.ReadOnly = true;
            dgvVenta.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVenta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVenta.AllowUserToAddRows = false;


        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            if (txtIdTienda.Text.Trim() == "0" && txtTiendaNombre.Text.Trim() == "" || txtTiendaRuc.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione una tienda primero", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            oObjecto = null;
            frmBusqueda frm = new frmBusqueda(Modelo.ProductoTienda, int.Parse(txtIdTienda.Text.Trim()));
            frm.ShowDialog();

            if (oObjecto != null)
            {
                ProductoTienda oProductoTienda = (ProductoTienda)oObjecto;
                if (oProductoTienda != null)
                {
                    txtIdProducto.Text = oProductoTienda.oProducto.IdProducto.ToString();
                    txtProductoCodigo.Text = oProductoTienda.oProducto.Codigo;
                    txtProductoNombre.Text = oProductoTienda.oProducto.Nombre;
                    txtProductoDescripcion.Text = oProductoTienda.oProducto.Descripcion;
                    txtStock.Text = oProductoTienda.Stock.ToString();
                    txtPrecioUnidad.Text = oProductoTienda.PrecioUnidadVenta.ToString();
                    txtCantidad.Value = 1;
                    txtCantidad.Maximum = Convert.ToDecimal(oProductoTienda.Stock.ToString());
                }
            }
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (txtIdProducto.Text.Trim() == "0" || txtCantidad.Value < 1 || txtProductoCodigo.Text.Trim() == "")
            {
                MessageBox.Show("Seleccione un producto primero", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            bool productoexiste = false;
            foreach(DataGridViewRow r in dgvVenta.Rows)
            {
                if(r.Cells["IdProducto"].Value.ToString().Equals(txtIdProducto.Text.Trim())){
                    productoexiste = true;
                    break;
                }
            }
            

            if (!productoexiste)
            {
                bool Respuesta = CD_ProductoTienda.ControlarStock(int.Parse(txtIdProducto.Text.Trim()), int.Parse(txtIdTienda.Text.Trim()), int.Parse(txtCantidad.Value.ToString()), true);

                int rowId = dgvVenta.Rows.Add();
                DataGridViewRow row = dgvVenta.Rows[rowId];
                row.Cells["IdProducto"].Value = txtIdProducto.Text.Trim();
                row.Cells["Cantidad"].Value = txtCantidad.Value.ToString();
                row.Cells["Producto"].Value = txtProductoNombre.Text.ToString();
                row.Cells["Descripción"].Value = txtProductoDescripcion.Text.Trim();
                row.Cells["PrecioUnidad"].Value = txtPrecioUnidad.Text.Trim();
                row.Cells["Precio Unidad"].Value = string.Format("S/.{0}", txtPrecioUnidad.Text.Trim());
                row.Cells["ImporteTotal"].Value = float.Parse(txtCantidad.Value.ToString()) * float.Parse(txtPrecioUnidad.Text.Trim());
                row.Cells["Importe Total"].Value = string.Format("S/.{0}", float.Parse(txtCantidad.Value.ToString()) * float.Parse(txtPrecioUnidad.Text.Trim()));

                
                txtIdProducto.Text = "0";
                txtProductoCodigo.Text = "";
                txtProductoNombre.Text = "";
                txtProductoDescripcion.Text = "";
                txtStock.Text = "";
                txtPrecioUnidad.Text = "";
                txtCantidad.Value = 1;
                btnAgregarProducto.Focus();

                CalcularTotal();
            }
            else
            {
                MessageBox.Show("Producto ya fue agregado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            
        }

        private void dgvVenta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvVenta.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int index = e.RowIndex;
                if (index >= 0)
                {
                    
                    int IdProducto = int.Parse(dgvVenta.Rows[index].Cells["IdProducto"].Value.ToString());
                    int Cantidad = int.Parse(dgvVenta.Rows[index].Cells["Cantidad"].Value.ToString());
                    bool Respuesta = CD_ProductoTienda.ControlarStock(IdProducto, int.Parse(txtIdTienda.Text.Trim()), Cantidad, false);

                    dgvVenta.Rows.RemoveAt(index);
                    CalcularTotal();
                }
            }
        }

        private void CalcularTotal()
        {
            Total = 0;
            SubTotal = 0;
            IGV = 0;
            foreach (DataGridViewRow r in dgvVenta.Rows)
            {
                Total += float.Parse(r.Cells["ImporteTotal"].Value.ToString());
            }

            IGV = float.Parse((Total * 0.18).ToString());
            SubTotal = Total - IGV;

            lblSubTotal.Text = SubTotal.ToString("0.00");
            lblIGV.Text = IGV.ToString("0.00");
            lblTotal.Text = Total.ToString("0.00");
            lblTotalProductos.Text = dgvVenta.Rows.Count.ToString();
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            if(txtMontoPago.Text.Trim() != "" && txtMontoPago.Text.Trim() != "0")
            {
                float MontoPago;
                bool validate = float.TryParse(txtMontoPago.Text.Trim(), out MontoPago);
                if (!validate)
                {
                    MessageBox.Show("Formato de Monto de Pago incorrecto,\nrevise el formato decimal tipo moneda", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                float cambio = 0;
                cambio = float.Parse(txtMontoPago.Text.Trim()) - Total;
                cambio = cambio < 0 ? 0 : cambio;
                txtCambio.Text = cambio.ToString("0.00");
            }
            else
            {
                MessageBox.Show("Ingrese Monto pago", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtMontoPago.Focus();
            }
                

        }

        private void btnImprimirTerminar_Click(object sender, EventArgs e)
        {
            if (txtCambio.Text.Trim() == "0")
            {
                MessageBox.Show("Debe ingresar el monto recibido y calcular cambio", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                btnCalcular.Focus();
                return;
            }

            if (txtDocumentoCliente.Text.Trim() == "" || txtNombreCliente.Text.Trim() == "" || txtDireccionCliente.Text.Trim() == "")
            {
                MessageBox.Show("Debe ingresar los datos del cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDocumentoCliente.Focus();
                return;
            }

            float MontoPago;
            bool validate = float.TryParse(txtMontoPago.Text.Trim(), out MontoPago);
            if (!validate)
            {
                MessageBox.Show("Formato de Monto de Pago incorrecto,\nrevise el formato decimal tipo moneda", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            int TotalCantidad = 0;
            foreach(DataGridViewRow r in dgvVenta.Rows)
            {
                TotalCantidad += int.Parse(r.Cells["Cantidad"].Value.ToString());
            }


            XElement Detalle_Cliente = new XElement("DETALLE_CLIENTE");

            Detalle_Cliente.Add(new XElement("DATOS",
                new XElement("TipoDocumento", ((ComboBoxItem)cboTipoDocumentoCliente.SelectedItem).Value.ToString()),
                new XElement("NumeroDocumento", txtDocumentoCliente.Text.Trim()),
                new XElement("Nombre", txtNombreCliente.Text.Trim()),
                new XElement("Direccion", txtDireccionCliente.Text.Trim()),
                new XElement("Telefono", txtTelefonoCliente.Text.Trim())
                ));


            XElement Detalle_Venta = new XElement("DETALLE_VENTA");
            foreach (DataGridViewRow row in dgvVenta.Rows)
            {
                Detalle_Venta.Add(new XElement("DATOS",
                    new XElement("IdVenta", "0"),
                    new XElement("IdProducto", row.Cells["IdProducto"].Value),
                    new XElement("Cantidad", row.Cells["Cantidad"].Value),
                    new XElement("PrecioUnidad", row.Cells["PrecioUnidad"].Value),
                    new XElement("ImporteTotal", row.Cells["ImporteTotal"].Value)
                    ));
            }

            XElement RegistroVenta = new XElement("DETALLE",
               new XElement("VENTA",
               new XElement("IdTienda", txtIdTienda.Text.Trim()),
               new XElement("IdUsuario", Configuracion.oUsuario.IdUsuario),
               new XElement("IdCliente", "0"),
               new XElement("TipoDocumento", ((ComboBoxItem)cboTipoDocumentoVenta.SelectedItem).Value),
               new XElement("CantidadProducto", lblTotalProductos.Text),
               new XElement("CantidadTotal", TotalCantidad),
               new XElement("TotalCosto", lblTotal.Text),
               new XElement("ImporteRecibido", txtMontoPago.Text),
               new XElement("ImporteCambio", txtCambio.Text)
               ));

            RegistroVenta.Add(Detalle_Cliente, Detalle_Venta);

            int Respuesta = CD_Venta.RegistrarVenta(RegistroVenta.ToString());

            if (Respuesta > 0)
            {
                MessageBox.Show("Registro exitoso!", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                limpiarData();
                frmDocumento frm = new frmDocumento(Respuesta);
                frm.Show();
            }
            else
            {
                MessageBox.Show("Error al registrar, revise los datos.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void txtMontoPago_KeyPress(object sender, KeyPressEventArgs e)
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

        private void limpiarData()
        {
            cboTipoDocumentoVenta.SelectedIndex = 0;

            //PRODUCTO
            txtIdProducto.Text = "0";
            txtProductoCodigo.Text = "";
            txtProductoNombre.Text = "";
            txtProductoDescripcion.Text = "";
            txtStock.Text = "";
            txtPrecioUnidad.Text = "";
            txtCantidad.Value = 1;
            btnAgregarProducto.Focus();

            //IMPORTES
            Total = 0;
            SubTotal = 0;
            IGV = 0;
            lblSubTotal.Text = "0";
            lblIGV.Text = "0";
            lblTotal.Text = "0";
            lblTotalProductos.Text = "0";

            txtMontoPago.Text = "0";
            txtCambio.Text = "0";

            //CLIENTE
            cboTipoDocumentoCliente.SelectedIndex = 0;
            txtDocumentoCliente.Text = "";
            txtNombreCliente.Text = "";
            txtDireccionCliente.Text = "";
            txtTelefonoCliente.Text = "";

            //COMBO PRODUCTO
            dgvVenta.Rows.Clear();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void txtProductoCodigo_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                int IdTienda = int.Parse(txtIdTienda.Text.Trim());
                List<ProductoTienda> oListaProductoTienda = CD_ProductoTienda.ObtenerProductoTienda();
                ProductoTienda  oProductoTienda = oListaProductoTienda.Where(x => x.oProducto.Codigo == txtProductoCodigo.Text.Trim() &&  x.oTienda.IdTienda == IdTienda).FirstOrDefault();
                if (oProductoTienda != null)
                {
                    txtIdProducto.Text = oProductoTienda.oProducto.IdProducto.ToString();
                    txtProductoCodigo.Text = oProductoTienda.oProducto.Codigo;
                    txtProductoNombre.Text = oProductoTienda.oProducto.Nombre;
                    txtProductoDescripcion.Text = oProductoTienda.oProducto.Descripcion;
                    txtStock.Text = oProductoTienda.Stock.ToString();
                    txtPrecioUnidad.Text = oProductoTienda.PrecioUnidadVenta.ToString();
                    txtCantidad.Value = 1;
                    txtCantidad.Maximum = Convert.ToDecimal(oProductoTienda.Stock.ToString());
                }
                else
                {
                    txtIdProducto.Text = "0";
                    txtProductoNombre.Text = "";
                    txtProductoDescripcion.Text = "";
                    txtStock.Text = "";
                    txtPrecioUnidad.Text = "";
                    txtCantidad.Value = 1;
                    txtProductoCodigo.Focus();
                    MessageBox.Show("No se encontraron resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                
           
            }
        }

        private void frmCrearVenta_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(dgvVenta.Rows.Count > 0)
            {
                int IdTienda = int.Parse(txtIdTienda.Text.Trim());
                foreach (DataGridViewRow r in dgvVenta.Rows)
                {
                    int IdProducto = int.Parse(r.Cells["IdProducto"].Value.ToString());
                    int Cantidad = int.Parse(r.Cells["Cantidad"].Value.ToString());
                    bool Respuesta = CD_ProductoTienda.ControlarStock(IdProducto, IdTienda, Cantidad, false);
                }
            }
        }
    }
}
