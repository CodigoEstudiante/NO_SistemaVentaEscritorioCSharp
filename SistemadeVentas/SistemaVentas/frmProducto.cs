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
    public partial class frmProducto : Form
    {
        DataTable tablaRol = new DataTable();

        public frmProducto()
        {
            InitializeComponent();
        }

        private void frmProducto_Load(object sender, EventArgs e)
        {
            CargarDatos();

            dgvProducto.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducto.MultiSelect = false;
            dgvProducto.ReadOnly = true;
            dgvProducto.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProducto.AllowUserToAddRows = false;
        
        }

        private void CargarDatos()
        {

            List<Producto> oListaProducto = CD_Producto.ObtenerProducto();
            if (oListaProducto.Count > 0  && oListaProducto != null)
            {
                lblTotalRegistros.Text = oListaProducto.Count.ToString();
                tablaRol = new DataTable();
                tablaRol.Columns.Clear();
                tablaRol.Rows.Clear();
                cboFiltro.Items.Clear();

                tablaRol.Columns.Add("IdProducto", typeof(int));
                tablaRol.Columns.Add("Codigo", typeof(string));
                tablaRol.Columns.Add("ValorCodigo", typeof(string));
                tablaRol.Columns.Add("Nombre", typeof(string));
                tablaRol.Columns.Add("Descripcion", typeof(string));
                tablaRol.Columns.Add("IdCategoria", typeof(string));
                tablaRol.Columns.Add("Activo", typeof(bool));
                tablaRol.Columns.Add("Categoria", typeof(string));
                tablaRol.Columns.Add("Estado", typeof(string));


                foreach (Producto row in oListaProducto)
                {
                    tablaRol.Rows.Add(row.IdProducto, row.Codigo, row.ValorCodigo, row.Nombre, row.Descripcion, row.IdCategoria, row.Activo,row.oCategoria.Descripcion, row.Activo == true ? "Activo" : "No Activo");
                }

                dgvProducto.DataSource = tablaRol;


                dgvProducto.Columns["IdProducto"].Visible = false;
                dgvProducto.Columns["ValorCodigo"].Visible = false;
                dgvProducto.Columns["IdCategoria"].Visible = false;
                dgvProducto.Columns["Activo"].Visible = false;

                foreach (DataGridViewColumn cl in dgvProducto.Columns)
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
            (dgvProducto.DataSource as DataTable).DefaultView.RowFilter = string.Format(columnaFiltro + " like '%{0}%'", txtFilter.Text);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            mtnProducto form = new mtnProducto();
            form.ShowDialog();
            CargarDatos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvProducto.SelectedRows.Count > 0)
            {
                DataGridViewRow currentRow = dgvProducto.SelectedRows[0];
                int index = currentRow.Index;
                Producto oProducto = new Producto()
                {
                    IdProducto = Convert.ToInt32(dgvProducto.Rows[index].Cells["IdProducto"].Value),
                    Codigo = Convert.ToString(dgvProducto.Rows[index].Cells["Codigo"].Value),
                    Nombre = Convert.ToString(dgvProducto.Rows[index].Cells["Nombre"].Value),
                    Descripcion = Convert.ToString(dgvProducto.Rows[index].Cells["Descripcion"].Value),
                    IdCategoria = Convert.ToInt32(dgvProducto.Rows[index].Cells["IdCategoria"].Value),
                    Activo = Convert.ToBoolean(dgvProducto.Rows[index].Cells["Activo"].Value)
                };
                mtnProducto form = new mtnProducto(oProducto);
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
            if (dgvProducto.SelectedRows.Count > 0)
            {
                DataGridViewRow currentRow = dgvProducto.SelectedRows[0];
                int index = currentRow.Index;

                int IdProducto = Convert.ToInt32(dgvProducto.Rows[index].Cells["IdProducto"].Value);
                string NombreProducto = Convert.ToString(dgvProducto.Rows[index].Cells["Nombre"].Value);

                if (MessageBox.Show(string.Format("{0} '{1}' {2}", "¿Desea eliminar el producto", NombreProducto, "permanentemente?"), "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    bool Respuesta = CD_Producto.EliminarProducto(IdProducto);
                    if (Respuesta)
                    {
                        MessageBox.Show(string.Format("{0} {1} {2}", "El producto", NombreProducto, "fue eliminado"), "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarDatos();
                    }
                    else
                    {
                        MessageBox.Show(string.Format("{0} {1} {2} \n{3}", "El producto", NombreProducto, "no fue eliminado.", "El producto se encuentra asignado a alguna venta actualmente"), "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
