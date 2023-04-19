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
    public partial class frmCategoria : Form
    {
        public frmCategoria()
        {
            InitializeComponent();
        }
        DataTable tablaCategoria = new DataTable();
        private void frmCategoria_Load(object sender, EventArgs e)
        {
            CargarDatos();

            dgvCategoria.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategoria.MultiSelect = false;
            dgvCategoria.ReadOnly = true;
            dgvCategoria.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCategoria.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategoria.AllowUserToAddRows = false;
        }
        private void CargarDatos()
        {

            List<Categoria> oListaCategoria = CD_Categoria.ObtenerCategoria();
            if (oListaCategoria.Count > 0)
            {
                lblTotalRegistros.Text = oListaCategoria.Count.ToString();
                tablaCategoria = new DataTable();
                tablaCategoria.Columns.Clear();
                tablaCategoria.Rows.Clear();
                cboFiltro.Items.Clear();

                tablaCategoria.Columns.Add("IdCategoria", typeof(int));
                tablaCategoria.Columns.Add("Descripcion", typeof(string));
                tablaCategoria.Columns.Add("Estado", typeof(string));
                tablaCategoria.Columns.Add("Activo", typeof(bool));

                foreach (Categoria row in oListaCategoria)
                {
                    tablaCategoria.Rows.Add(row.IdCategoria, row.Descripcion, row.Activo == true ? "Activo" : "No Activo", row.Activo);
                }

                dgvCategoria.DataSource = tablaCategoria;


                dgvCategoria.Columns["IdCategoria"].Visible = false;
                dgvCategoria.Columns["Activo"].Visible = false;

                foreach (DataGridViewColumn cl in dgvCategoria.Columns)
                {
                    if (cl.Visible == true)
                    {
                        cboFiltro.Items.Add(cl.HeaderText);
                    }
                }
                cboFiltro.SelectedIndex = 0;
            }

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            mtnCategoria form = new mtnCategoria();
            form.ShowDialog();
            CargarDatos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvCategoria.SelectedRows.Count > 0)
            {
                DataGridViewRow currentRow = dgvCategoria.SelectedRows[0];
                int index = currentRow.Index;
                Categoria oCategoria = new Categoria()
                {
                    IdCategoria = Convert.ToInt32(dgvCategoria.Rows[index].Cells["IdCategoria"].Value),
                    Descripcion = Convert.ToString(dgvCategoria.Rows[index].Cells["Descripcion"].Value),
                    Activo = Convert.ToBoolean(dgvCategoria.Rows[index].Cells["Activo"].Value)
                };
                mtnCategoria form = new mtnCategoria(oCategoria);
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
            if (dgvCategoria.SelectedRows.Count > 0)
            {
                DataGridViewRow currentRow = dgvCategoria.SelectedRows[0];
                int index = currentRow.Index;

                int IdCategoria = Convert.ToInt32(dgvCategoria.Rows[index].Cells["IdCategoria"].Value);
                string NombreCategoria = Convert.ToString(dgvCategoria.Rows[index].Cells["Descripcion"].Value);

                if (MessageBox.Show(string.Format("{0} '{1}' {2}", "¿Desea eliminar la categoria", NombreCategoria, "permanentemente?"), "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    bool Respuesta = CD_Categoria.EliminarCategoria(IdCategoria);
                    if (Respuesta)
                    {
                        MessageBox.Show(string.Format("{0} {1} {2}", "La Categoria", NombreCategoria, "fue eliminada"), "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarDatos();
                    }
                    else
                    {
                        MessageBox.Show(string.Format("{0} {1} {2} \n{3}", "La categoria", NombreCategoria, "no fue eliminada.", "La categoria se encuentra asignado a algun producto actualmente"), "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }


            }
            else
            {
                MessageBox.Show("Selecciona un registro de la lista");
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string columnaFiltro = cboFiltro.SelectedItem.ToString();
            (dgvCategoria.DataSource as DataTable).DefaultView.RowFilter = string.Format(columnaFiltro + " like '%{0}%'", txtFilter.Text);
        }
    }
}
