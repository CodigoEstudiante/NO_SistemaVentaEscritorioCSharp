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
using System.Xml.Linq;

namespace SistemaVentas
{
    public partial class frmRolPermiso : Form
    {
        public frmRolPermiso()
        {
            InitializeComponent();
        }
        private void frmRolPermiso_Load(object sender, EventArgs e)
        {
            List<Rol> oListaRol = CD_Rol.ObtenerRol();

            cboRol.Items.Add(new ComboBoxItem() { Value = 0, Text = "Seleccione Rol" });
            foreach (Rol row in oListaRol.Where(x => x.Activo == true))
            {
                cboRol.Items.Add(new ComboBoxItem() { Value = row.IdRol, Text = row.Descripcion });
            }
            cboRol.DisplayMember = "Text";
            cboRol.ValueMember = "Value";
            cboRol.SelectedIndex = 0;

            

            dgvPermisos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPermisos.MultiSelect = false;
            dgvPermisos.ReadOnly = true;
            dgvPermisos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPermisos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPermisos.AllowUserToAddRows = false;

            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.Name = "Activar";
            checkColumn.HeaderText = "Activar";
            checkColumn.Width = 50;
            checkColumn.ReadOnly = false;
            checkColumn.FillWeight = 30;
            dgvPermisos.Columns.Add(checkColumn);

            dgvPermisos.Columns.Add("IdPermisos", "IdPermisos");
            dgvPermisos.Columns.Add("Menu", "Menu");
            dgvPermisos.Columns.Add("Sub Menu", "Sub Menu");
            dgvPermisos.Columns.Add("Activo", "Activo");

            dgvPermisos.Columns["Menu"].FillWeight = 60;
            dgvPermisos.Columns["IdPermisos"].Visible = false;
            dgvPermisos.Columns["Activo"].Visible = false;
        }

        private void btnBuscarPermisos_Click(object sender, EventArgs e)
        {
            if( int.Parse(((ComboBoxItem)cboRol.SelectedItem).Value.ToString()) == 0)
            {
                MessageBox.Show("Debe seleccionar un rol", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            CargarDatos(int.Parse(((ComboBoxItem)cboRol.SelectedItem).Value.ToString()));

        }

        private void CargarDatos(int IdRol)
        {
            dgvPermisos.Rows.Clear();
            List<Permisos> oListaPermisos = CD_Permisos.ObtenerPermisos(IdRol);
            if (oListaPermisos.Count > 0)
            {
                foreach (Permisos r in oListaPermisos)
                {
                    int rowId = dgvPermisos.Rows.Add();
                    DataGridViewRow row = dgvPermisos.Rows[rowId];
                    row.Cells["IdPermisos"].Value = r.IdPermisos;
                    row.Cells["Menu"].Value = r.Menu;
                    row.Cells["Sub Menu"].Value = r.SubMenu;
                    row.Cells["Activar"].Value = r.Activo;
                    row.Cells["Activo"].Value = r.Activo;
                }
                
                dgvPermisos.Columns["IdPermisos"].Visible = false;
                dgvPermisos.Columns["Activo"].Visible = false;

                
            }

        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            if(dgvPermisos.Rows.Count > 0)
            {
                XElement Detalle_Permisos = new XElement("DETALLE");
                foreach (DataGridViewRow row in dgvPermisos.Rows)
                {
                    Detalle_Permisos.Add(new XElement("PERMISO",
                        new XElement("IdPermisos", row.Cells["IdPermisos"].Value),
                        new XElement("Activo", bool.Parse(row.Cells["Activar"].Value.ToString()) == true ? "1" : "0")
                        ));
                }

                bool Respuesta = CD_Permisos.ActualizarPermisos(Detalle_Permisos.ToString());

                if (Respuesta)
                {
                    MessageBox.Show("Se actualizaron los permisos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvPermisos.Rows.Clear();
                    cboRol.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Error al actualizar los permisos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un rol", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            

            
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvPermisos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPermisos.Columns[e.ColumnIndex].Name == "Activar")
            {
                int index = e.RowIndex;
                if (index >= 0)
                {
                    bool estado = Convert.ToBoolean(dgvPermisos.Rows[index].Cells["Activar"].Value);
                    if (estado)
                        dgvPermisos.Rows[index].Cells["Activar"].Value = false;
                    else
                        dgvPermisos.Rows[index].Cells["Activar"].Value = true;


                }
            }
        }
    }
}
