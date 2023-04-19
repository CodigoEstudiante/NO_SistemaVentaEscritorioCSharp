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
    public partial class frmRol : Form
    {
        public frmRol()
        {
            InitializeComponent();
        }
        DataTable tablaRol = new DataTable();
        

        private void frmRol_Load(object sender, EventArgs e)
        {

            CargarDatos();
            
            dgvRol.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRol.MultiSelect = false;
            dgvRol.ReadOnly = true;
            dgvRol.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRol.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRol.AllowUserToAddRows = false;
        }

        private void CargarDatos()
        {

            List<Rol> oListaRol = CD_Rol.ObtenerRol();
            if (oListaRol.Count > 0)
            {
                lblTotalRegistros.Text = oListaRol.Count.ToString();
                tablaRol = new DataTable();
                tablaRol.Columns.Clear();
                tablaRol.Rows.Clear();
                cboFiltro.Items.Clear();

                tablaRol.Columns.Add("IdRol", typeof(int));
                tablaRol.Columns.Add("Descripcion", typeof(string));
                tablaRol.Columns.Add("Estado", typeof(string));
                tablaRol.Columns.Add("Activo", typeof(bool));

                foreach (Rol row in oListaRol)
                {
                    tablaRol.Rows.Add(row.IdRol, row.Descripcion, row.Activo == true ? "Activo" : "No Activo", row.Activo);
                }

                dgvRol.DataSource = tablaRol;


                dgvRol.Columns["IdRol"].Visible = false;
                dgvRol.Columns["Activo"].Visible = false;

                foreach (DataGridViewColumn cl in dgvRol.Columns)
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
            (dgvRol.DataSource as DataTable).DefaultView.RowFilter = string.Format(columnaFiltro + " like '%{0}%'", txtFilter.Text);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            mtnRol form = new mtnRol();
            form.ShowDialog();
            CargarDatos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvRol.SelectedRows.Count > 0)
            {
                DataGridViewRow currentRow = dgvRol.SelectedRows[0];
                int index = currentRow.Index;
                Rol oRol = new Rol()
                {
                    IdRol = Convert.ToInt32(dgvRol.Rows[index].Cells["IdRol"].Value),
                    Descripcion = Convert.ToString(dgvRol.Rows[index].Cells["Descripcion"].Value),
                    Activo = Convert.ToBoolean(dgvRol.Rows[index].Cells["Activo"].Value)
                };
                mtnRol form = new mtnRol(oRol);
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
            if (dgvRol.SelectedRows.Count > 0)
            {
                DataGridViewRow currentRow = dgvRol.SelectedRows[0];
                int index = currentRow.Index;

                int IdRol = Convert.ToInt32(dgvRol.Rows[index].Cells["IdRol"].Value);
                string NombreTienda = Convert.ToString(dgvRol.Rows[index].Cells["Descripcion"].Value);

                if (MessageBox.Show(string.Format("{0} '{1}' {2}", "¿Desea eliminar el Rol", NombreTienda, "permanentemente?"), "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    bool Respuesta = CD_Rol.EliminarRol(IdRol);
                    if (Respuesta)
                    {
                        MessageBox.Show(string.Format("{0} {1} {2}", "El rol", NombreTienda, "fue eliminado"), "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarDatos();
                    }
                    else
                    {
                        MessageBox.Show(string.Format("{0} {1} {2} \n{3}", "El rol", NombreTienda, "no fue eliminado.", "El rol se encuentra asignado a algun usuario actualmente"), "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
