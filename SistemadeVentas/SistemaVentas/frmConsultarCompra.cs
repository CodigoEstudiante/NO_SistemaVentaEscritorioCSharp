using CapaDatos;
using CapaModelo;
using SistemaVentas.Reutilizable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaVentas
{
    public partial class frmConsultarCompra : Form
    {
        DataTable tabla = new DataTable();
        public frmConsultarCompra()
        {
            InitializeComponent();
        }

        private void frmConsultarCompra_Load(object sender, EventArgs e)
        {
            List<Tienda> oListaTienda = CD_Tienda.ObtenerTiendas();

            cboTienda.Items.Add(new ComboBoxItem() { Value = 0, Text = "Seleccionar Todos" });
            foreach (Tienda row in oListaTienda.Where(x => x.Activo == true))
            {
                cboTienda.Items.Add(new ComboBoxItem() { Value = row.IdTienda, Text = row.Nombre });
            }
            cboTienda.DisplayMember = "Text";
            cboTienda.ValueMember = "Value";
            cboTienda.SelectedIndex = 0;

            List<Proveedor> oListaProveedor = CD_Proveedor.ObtenerProveedor();

            cboProveedor.Items.Add(new ComboBoxItem() { Value = 0, Text = "Seleccionar Todos" });
            foreach (Proveedor row in oListaProveedor.Where(x => x.Activo == true))
            {
                cboProveedor.Items.Add(new ComboBoxItem() { Value = row.IdProveedor, Text = row.RazonSocial });
            }
            cboProveedor.DisplayMember = "Text";
            cboProveedor.ValueMember = "Value";
            cboProveedor.SelectedIndex = 0;



            CargarDatos();

            dgvCompras.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCompras.MultiSelect = false;
            dgvCompras.ReadOnly = true;
            dgvCompras.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCompras.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCompras.AllowUserToAddRows = false;
        }

        private void CargarDatos()
        {
            dgvCompras.Columns.Clear();

            List<Compra> oListaCompra = CD_Compra.ObtenerListaCompra(dtpFechaInicio.Value, dtpFechaFin.Value,int.Parse(((ComboBoxItem)cboProveedor.SelectedItem).Value.ToString()) , int.Parse(((ComboBoxItem)cboTienda.SelectedItem).Value.ToString()));
            if (oListaCompra.Count > 0)
            {
                lblTotalRegistros.Text = oListaCompra.Count.ToString();
                tabla = new DataTable();
                tabla.Columns.Clear();
                tabla.Rows.Clear();
                cboFiltro.Items.Clear();

                tabla.Columns.Add("IdCompra", typeof(int));
                tabla.Columns.Add("Numero Compra", typeof(string));
                tabla.Columns.Add("Proveedor", typeof(string));
                tabla.Columns.Add("Tienda", typeof(string));
                tabla.Columns.Add("Fecha Compra", typeof(string));
                tabla.Columns.Add("Total Costo", typeof(string));

                //AGREGAR BOTON SELECCIONAR
                DataGridViewButtonColumn BotonSeleccionar = new DataGridViewButtonColumn();
                BotonSeleccionar.HeaderText = "Acción";
                BotonSeleccionar.Text = "Ver";
                BotonSeleccionar.Width = 60;
                BotonSeleccionar.Name = "Ver";
                BotonSeleccionar.FlatStyle = FlatStyle.Flat;
                BotonSeleccionar.UseColumnTextForButtonValue = true;
                BotonSeleccionar.CellTemplate.Style.BackColor = Color.AliceBlue;
                BotonSeleccionar.CellTemplate.Style.SelectionBackColor = Color.CadetBlue;
                //AGREGAMOS EL BOTON
                dgvCompras.Columns.Add(BotonSeleccionar);

                foreach (Compra row in oListaCompra)
                {
                    tabla.Rows.Add(row.IdCompra, row.NumeroCompra,row.oProveedor.RazonSocial,row.oTienda.Nombre, row.FechaCompra, row.TotalCosto);
                };

                dgvCompras.DataSource = tabla;


                dgvCompras.Columns["IdCompra"].Visible = false;

                foreach (DataGridViewColumn cl in dgvCompras.Columns)
                {
                    if (cl.Visible == true && cl.HeaderText != "Acción")
                    {
                        cboFiltro.Items.Add(cl.HeaderText);
                    }
                }
                cboFiltro.SelectedIndex = 0;
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string columnaFiltro = cboFiltro.SelectedItem.ToString();
            (dgvCompras.DataSource as DataTable).DefaultView.RowFilter = string.Format(columnaFiltro + " like '%{0}%'", txtFilter.Text);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void dgvCompras_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCompras.Columns[e.ColumnIndex].Name == "Ver")
            {
                int IdCompra = Convert.ToInt32(dgvCompras.Rows[e.RowIndex].Cells["IdCompra"].Value.ToString());
                frmDocumentoCompra frm = new frmDocumentoCompra(IdCompra);
                frm.Show();
            }
        }
    }
}
