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
    public partial class frmUsuario : Form
    {
        public frmUsuario()
        {
            InitializeComponent();
        }
        DataTable tablaUsuario = new DataTable();

        private void frmUsuario_Load(object sender, EventArgs e)
        {
            CargarDatos();

            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.AllowUserToAddRows = false;

        }

        private void CargarDatos()
        {

            List<Usuario> oListaUsuario = CD_Usuario.ObtenerUsuarios();
            if (oListaUsuario.Count > 0)
            {
                tablaUsuario = new DataTable();
                tablaUsuario.Columns.Clear();
                tablaUsuario.Rows.Clear();
                cboFiltro.Items.Clear();

                lblTotalRegistros.Text = oListaUsuario.Count.ToString();


                tablaUsuario.Columns.Add("IdUsuario", typeof(int));
                tablaUsuario.Columns.Add("Rol", typeof(string));
                tablaUsuario.Columns.Add("Usuario", typeof(string));
                tablaUsuario.Columns.Add("Nombres", typeof(string));
                tablaUsuario.Columns.Add("Apellidos", typeof(string));
                tablaUsuario.Columns.Add("Correo", typeof(string));
                tablaUsuario.Columns.Add("Estado", typeof(string));
                tablaUsuario.Columns.Add("Clave", typeof(string));
                tablaUsuario.Columns.Add("IdTienda", typeof(int));
                tablaUsuario.Columns.Add("IdRol", typeof(int));
                tablaUsuario.Columns.Add("Activo", typeof(int));

                foreach (Usuario row in oListaUsuario)
                {
                    tablaUsuario.Rows.Add(row.IdUsuario, row.oRol.Descripcion, row.NombreUsuario, row.Nombres, row.Apellidos, row.Correo,row.Activo == true ?"Activo":"No Activo" , row.Clave, row.IdTienda, row.IdRol,row.Activo);
                }

                dgvUsuarios.DataSource = tablaUsuario;

                dgvUsuarios.Columns["IdUsuario"].Visible = false;
                dgvUsuarios.Columns["IdTienda"].Visible = false;
                dgvUsuarios.Columns["IdRol"].Visible = false;
                dgvUsuarios.Columns["Clave"].Visible = false;
                dgvUsuarios.Columns["Activo"].Visible = false;

                foreach (DataGridViewColumn cl in dgvUsuarios.Columns)
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
            (dgvUsuarios.DataSource as DataTable).DefaultView.RowFilter = string.Format(columnaFiltro + " like '%{0}%'", txtFilter.Text);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            mtnUsuario form = new mtnUsuario();
            form.ShowDialog();
            CargarDatos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                DataGridViewRow currentRow = dgvUsuarios.SelectedRows[0];
                int index = currentRow.Index;

                int IdUsuario = Convert.ToInt32(dgvUsuarios.Rows[index].Cells["IdUsuario"].Value);
                string NombreUsuario = Convert.ToString(dgvUsuarios.Rows[index].Cells["Usuario"].Value);

                if (MessageBox.Show(string.Format("{0} '{1}' {2}", "¿Desea eliminar el usuario ", NombreUsuario, "permanentemente?"), "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    bool Respuesta = CD_Usuario.EliminarUsuario(IdUsuario);
                    if (Respuesta)
                    {
                        MessageBox.Show(string.Format("{0} '{1}' {2}", "El usuario", NombreUsuario, "fue eliminado"), "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarDatos();
                    }
                    else
                    {
                        MessageBox.Show(string.Format("{0} '{1}' {2} \n{3}", "El usuario", NombreUsuario, "no fue eliminado.", "El usuario se encuentra asignado a registro de ventas actualmente"), "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }


            }
            else
            {
                MessageBox.Show("Selecciona un registro de la lista");
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                DataGridViewRow currentRow = dgvUsuarios.SelectedRows[0];
                int index = currentRow.Index;
                Usuario oUsuario = new Usuario()
                {
                    IdUsuario = Convert.ToInt32(dgvUsuarios.Rows[index].Cells["IdUsuario"].Value),
                    Nombres = dgvUsuarios.Rows[index].Cells["Nombres"].Value.ToString(),
                    Apellidos = dgvUsuarios.Rows[index].Cells["Apellidos"].Value.ToString(),
                    Correo = dgvUsuarios.Rows[index].Cells["Correo"].Value.ToString(),
                    NombreUsuario = dgvUsuarios.Rows[index].Cells["Usuario"].Value.ToString(),
                    Clave = dgvUsuarios.Rows[index].Cells["Clave"].Value.ToString(),
                    IdTienda = Convert.ToInt32(dgvUsuarios.Rows[index].Cells["IdTienda"].Value),
                    IdRol = Convert.ToInt32(dgvUsuarios.Rows[index].Cells["IdRol"].Value),
                    Activo = Convert.ToBoolean(dgvUsuarios.Rows[index].Cells["Activo"].Value)
                };
                mtnUsuario form = new mtnUsuario(oUsuario);
                form.ShowDialog();
                CargarDatos();

            }
            else
            {
                MessageBox.Show("Selecciona un registro de la lista","Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }
    }
}
