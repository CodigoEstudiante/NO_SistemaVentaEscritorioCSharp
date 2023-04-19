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
    public partial class frmProveedor : Form
    {
        public frmProveedor()
        {
            InitializeComponent();
        }
        DataTable tablaProveedor = new DataTable();
        private void frmProveedor_Load(object sender, EventArgs e)
        {
            CargarDatos();
            dgvProveedor.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProveedor.MultiSelect = false;
            dgvProveedor.ReadOnly = true;
            dgvProveedor.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProveedor.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProveedor.AllowUserToAddRows = false;
        }

        private void CargarDatos()
        {

            List<Proveedor> oListaProveedor = CD_Proveedor.ObtenerProveedor();
            if (oListaProveedor.Count > 0)
            {
                lblTotalRegistros.Text = oListaProveedor.Count.ToString();

                tablaProveedor = new DataTable();
                tablaProveedor.Columns.Clear();
                tablaProveedor.Rows.Clear();
                cboFiltro.Items.Clear();

                tablaProveedor.Columns.Add("IdProveedor", typeof(int));
                tablaProveedor.Columns.Add("RUC", typeof(string));
                tablaProveedor.Columns.Add("RazonSocial", typeof(string));
                tablaProveedor.Columns.Add("Telefono", typeof(string));
                tablaProveedor.Columns.Add("Correo", typeof(string));
                tablaProveedor.Columns.Add("Direccion", typeof(string));
                tablaProveedor.Columns.Add("Estado", typeof(string));
                tablaProveedor.Columns.Add("Activo", typeof(bool));

                
                foreach (Proveedor row in oListaProveedor)
                {
                    tablaProveedor.Rows.Add(row.IdProveedor, row.Ruc, row.RazonSocial, row.Telefono, row.Correo, row.Direccion, row.Activo == true ? "Activo" : "No Activo", row.Activo);
                }

                dgvProveedor.DataSource = tablaProveedor;


                dgvProveedor.Columns["IdProveedor"].Visible = false;
                dgvProveedor.Columns["Activo"].Visible = false;

                foreach (DataGridViewColumn cl in dgvProveedor.Columns)
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
            mtnProveedor form = new mtnProveedor();
            form.ShowDialog();
            CargarDatos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProveedor.SelectedRows.Count > 0)
            {
                DataGridViewRow currentRow = dgvProveedor.SelectedRows[0];
                int index = currentRow.Index;
                Proveedor oProveedor = new Proveedor()
                {
                    IdProveedor = Convert.ToInt32(dgvProveedor.Rows[index].Cells["IdProveedor"].Value),
                    Ruc = Convert.ToString(dgvProveedor.Rows[index].Cells["Ruc"].Value),
                    RazonSocial = dgvProveedor.Rows[index].Cells["RazonSocial"].Value.ToString(),
                    Telefono = dgvProveedor.Rows[index].Cells["Telefono"].Value.ToString(),
                    Correo = dgvProveedor.Rows[index].Cells["Correo"].Value.ToString(),
                    Direccion = dgvProveedor.Rows[index].Cells["Direccion"].Value.ToString(),
                    Activo = Convert.ToBoolean(dgvProveedor.Rows[index].Cells["Activo"].Value)
                };
                mtnProveedor form = new mtnProveedor(oProveedor);
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

            if (dgvProveedor.SelectedRows.Count > 0)
            {
                DataGridViewRow currentRow = dgvProveedor.SelectedRows[0];
                int index = currentRow.Index;

                int IdProveedor = Convert.ToInt32(dgvProveedor.Rows[index].Cells["IdProveedor"].Value);
                string NombreProveedor = Convert.ToString(dgvProveedor.Rows[index].Cells["RazonSocial"].Value);

                if (MessageBox.Show(string.Format("{0} '{1}' {2}", "¿Desea eliminar el Proveedor", NombreProveedor, "permanentemente?"), "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    bool Respuesta = CD_Tienda.EliminarTienda(IdProveedor);
                    if (Respuesta)
                    {
                        MessageBox.Show(string.Format("{0} {1} {2}", "El Proveedor", NombreProveedor, "fue eliminado"), "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarDatos();
                    }
                    else
                    {
                        MessageBox.Show(string.Format("{0} {1} {2} \n{3}", "El proveedor", NombreProveedor, "no fue eliminado.", "El proveedor se encuentra asignada a algunas compras actualmente"), "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            (dgvProveedor.DataSource as DataTable).DefaultView.RowFilter = string.Format(columnaFiltro + " like '%{0}%'", txtFilter.Text);
        }
    }
}
