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

namespace SistemaVentas
{
    public partial class frmConsultarVenta : Form
    {
        public frmConsultarVenta()
        {
            InitializeComponent();
        }
        DataTable tabla = new DataTable();
        private void frmConsultarVenta_Load(object sender, EventArgs e)
        {
            CargarDatos();

            dgvVentas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVentas.MultiSelect = false;
            dgvVentas.ReadOnly = true;
            dgvVentas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVentas.AllowUserToAddRows = false;
        }
        private void CargarDatos()
        {
            dgvVentas.Columns.Clear(); 

            List <Venta> oListaVenta = CD_Venta.ObtenerListaVenta(txtCodigoVenta.Text.Trim(), dtpFechaInicio.Value, dtpFechaFin.Value,txtDocumentoCliente.Text.Trim(),txtNombreCliente.Text.Trim());
            if (oListaVenta.Count > 0)
            {
                lblTotalRegistros.Text = oListaVenta.Count.ToString();
                tabla = new DataTable();
                tabla.Columns.Clear();
                tabla.Rows.Clear();
                cboFiltro.Items.Clear();

                tabla.Columns.Add("IdVenta", typeof(int));
                tabla.Columns.Add("Tipo Documento", typeof(string));
                tabla.Columns.Add("Codigo Documento", typeof(string));
                tabla.Columns.Add("Fecha Creación", typeof(string));
                tabla.Columns.Add("Documento Cliente", typeof(string));
                tabla.Columns.Add("Nombre Cliente", typeof(string));
                tabla.Columns.Add("Total Venta", typeof(string));

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
                dgvVentas.Columns.Add(BotonSeleccionar);

                foreach (Venta row in oListaVenta)
                {
                    tabla.Rows.Add(row.IdVenta, row.TipoDocumento, row.Codigo, row.VFechaRegistro.ToString("dd/MM/yyyy"), row.oCliente.NumeroDocumento, row.oCliente.Nombre, row.TotalCosto);
                };

                dgvVentas.DataSource = tabla;


                dgvVentas.Columns["IdVenta"].Visible = false;

                foreach (DataGridViewColumn cl in dgvVentas.Columns)
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
            (dgvVentas.DataSource as DataTable).DefaultView.RowFilter = string.Format(columnaFiltro + " like '%{0}%'", txtFilter.Text);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarDatos();
            //string dat = dtpFechaInicio.Value.ToString();
        }

        private void dgvVentas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvVentas.Columns[e.ColumnIndex].Name == "Ver")
            {
                int IdVenta = Convert.ToInt32(dgvVentas.Rows[e.RowIndex].Cells["IdVenta"].Value.ToString());
                frmDocumento frm = new frmDocumento(IdVenta);
                frm.Show();
            }
            
        }
    }
}
