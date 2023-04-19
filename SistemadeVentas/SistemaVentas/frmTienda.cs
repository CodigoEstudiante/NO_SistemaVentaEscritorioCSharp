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
    public partial class frmTienda : Form
    {
        public frmTienda()
        {
            InitializeComponent();
        }

        DataTable tablaTienda = new DataTable();

        private void frmTienda_Load(object sender, EventArgs e)
        {
            CargarDatos();

            dgvTienda.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTienda.MultiSelect = false;
            dgvTienda.ReadOnly = true;
            dgvTienda.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTienda.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTienda.AllowUserToAddRows = false;

        }

        private void CargarDatos()
        {

            List<Tienda> oListaTienda = CD_Tienda.ObtenerTiendas();
            if (oListaTienda.Count > 0)
            {
                lblTotalRegistros.Text = oListaTienda.Count.ToString();

                tablaTienda = new DataTable();
                tablaTienda.Columns.Clear();
                tablaTienda.Rows.Clear();
                cboFiltro.Items.Clear();

                tablaTienda.Columns.Add("IdTienda", typeof(int));
                tablaTienda.Columns.Add("Nombre", typeof(string));
                tablaTienda.Columns.Add("RUC", typeof(string));
                tablaTienda.Columns.Add("Direccion", typeof(string));
                tablaTienda.Columns.Add("Telefono", typeof(string));
                tablaTienda.Columns.Add("Estado", typeof(string));
                tablaTienda.Columns.Add("Activo", typeof(bool));

                foreach (Tienda row in oListaTienda)
                {
                    tablaTienda.Rows.Add(row.IdTienda, row.Nombre, row.RUC, row.Direccion, row.Telefono, row.Activo == true ? "Activo":"No Activo", row.Activo);
                }

                dgvTienda.DataSource = tablaTienda;


                dgvTienda.Columns["IdTienda"].Visible = false;
                dgvTienda.Columns["Activo"].Visible = false;

                foreach (DataGridViewColumn cl in dgvTienda.Columns)
                {
                    if (cl.Visible == true)
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
            (dgvTienda.DataSource as DataTable).DefaultView.RowFilter = string.Format(columnaFiltro + " like '%{0}%'", txtFilter.Text);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            mtnTienda form = new mtnTienda();
            form.ShowDialog();
            CargarDatos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvTienda.SelectedRows.Count > 0)
            {
                DataGridViewRow currentRow = dgvTienda.SelectedRows[0];
                int index = currentRow.Index;
                Tienda oTienda = new Tienda() {
                    IdTienda = Convert.ToInt32( dgvTienda.Rows[index].Cells["IdTienda"].Value),
                    Nombre = Convert.ToString( dgvTienda.Rows[index].Cells["Nombre"].Value),
                    RUC = dgvTienda.Rows[index].Cells["RUC"].Value.ToString(),
                    Direccion = dgvTienda.Rows[index].Cells["Direccion"].Value.ToString(),
                    Telefono = dgvTienda.Rows[index].Cells["Telefono"].Value.ToString(),
                    Activo  = Convert.ToBoolean(dgvTienda.Rows[index].Cells["Activo"].Value)
                };
                mtnTienda form = new mtnTienda(oTienda);
                form.ShowDialog();
                CargarDatos();
            }
            else
            {
                MessageBox.Show("Selecciona un registro de la lista");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvTienda.SelectedRows.Count > 0)
            {
                DataGridViewRow currentRow = dgvTienda.SelectedRows[0];
                int index = currentRow.Index;

                int IdTienda = Convert.ToInt32(dgvTienda.Rows[index].Cells["IdTienda"].Value);
                string NombreTienda = Convert.ToString(dgvTienda.Rows[index].Cells["Nombre"].Value);

                if (MessageBox.Show(string.Format("{0} '{1}' {2}","¿Desea eliminar la tienda" , NombreTienda,"permanentemente?"), "Mensaje", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    bool Respuesta = CD_Tienda.EliminarTienda(IdTienda);
                    if (Respuesta)
                    {
                        MessageBox.Show(string.Format("{0} {1} {2}", "La tienda", NombreTienda, "fue eliminada"), "Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        CargarDatos();
                    }
                    else
                    {
                        MessageBox.Show(string.Format("{0} {1} {2} \n{3}", "La tienda", NombreTienda, "no fue eliminada.","La tienda se encuentra asignada a algunos usuarios actualmente"), "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }


            }
            else
            {
                MessageBox.Show("Selecciona un registro de la lista");
            }
        }
    }
}
