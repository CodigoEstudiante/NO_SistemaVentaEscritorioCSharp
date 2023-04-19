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
    public partial class mtnRol : Form
    {
        private bool modoEditar = false;
        public mtnRol(Rol pRol = null)
        {
            InitializeComponent();
            cboActivo.Items.Add(new ComboBoxItem() { Value = true, Text = "Si" });
            cboActivo.Items.Add(new ComboBoxItem() { Value = false, Text = "No" });
            cboActivo.DisplayMember = "Text";
            cboActivo.ValueMember = "Value";
            cboActivo.SelectedIndex = 0;
            cboActivo.Enabled = false;

            if (pRol != null)
            {
                modoEditar = true;
                txtIdRol.Text = pRol.IdRol.ToString();
                txtDescripcion.Text = pRol.Descripcion.ToString();
                cboActivo.Enabled = true;

                foreach (ComboBoxItem item in cboActivo.Items)
                {
                    if ((bool)item.Value == pRol.Activo)
                    {
                        int index = cboActivo.Items.IndexOf(item);
                        cboActivo.SelectedIndex = index;
                        break;
                    }
                }
            }

        }

        private void mtnRol_Load(object sender, EventArgs e)
        {
            txtDescripcion.Focus();
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            

            if (txtDescripcion.Text.Trim() == "")
            {
                MessageBox.Show("Debe completar todos los campos", "Mensaje");
                return;
            }

            Rol oRol = new Rol()
            {
                IdRol = Convert.ToInt32(txtIdRol.Text.Trim()),
                Descripcion = txtDescripcion.Text.Trim(),
                Activo = Convert.ToBoolean(((ComboBoxItem)cboActivo.SelectedItem).Value)
            };
            

            bool Respuesta = false;
            string msgSuccess = "";
            string msgError = "";
            if (modoEditar)
            {
                Respuesta = CD_Rol.ModificarRol(oRol);
                msgSuccess = "Rol Modificado \n¿Desea registrar un nuevo rol ahora?";
                msgError = "No se pudo modificar el rol, \nes posible que ya se encuentre registrado";
            }
            else
            {

                Respuesta = CD_Rol.RegistrarRol(oRol);
                msgSuccess = "Rol Registrado \n¿Desea registrar un nuevo rol ahora?";
                msgError = "No se pudo registrar el rol, \nes posible que ya se encuentre registrado";

            }

            if (Respuesta)
            {
                if (MessageBox.Show(msgSuccess, "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    txtDescripcion.Text = "";
                    txtIdRol.Text = "0";
                    foreach (ComboBoxItem item in cboActivo.Items)
                    {
                        if ((bool)item.Value == true)
                        {
                            int index = cboActivo.Items.IndexOf(item);
                            cboActivo.SelectedIndex = index;
                            break;
                        }
                    }
                    cboActivo.Enabled = false;
                    modoEditar = false;
                    txtDescripcion.Focus();
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show(msgError, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
