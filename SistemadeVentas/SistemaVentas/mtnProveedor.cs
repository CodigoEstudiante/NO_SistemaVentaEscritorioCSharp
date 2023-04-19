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
    public partial class mtnProveedor : Form
    {
        bool modoEditar = false;
        public mtnProveedor(Proveedor pProveedor = null)
        {
            InitializeComponent();
            cboActivo.Items.Add(new ComboBoxItem() { Value = true, Text = "Si" });
            cboActivo.Items.Add(new ComboBoxItem() { Value = false, Text = "No" });
            cboActivo.DisplayMember = "Text";
            cboActivo.ValueMember = "Value";
            cboActivo.SelectedIndex = 0;

            if (pProveedor != null)
            {
                modoEditar = true;
                txtIdProveedor.Text = pProveedor.IdProveedor.ToString();
                txtRuc.Text = pProveedor.Ruc;
                txtRazonSocial.Text = pProveedor.RazonSocial;
                txtTelefono.Text = pProveedor.Telefono;
                txtCorreo.Text = pProveedor.Correo;
                txtDireccion.Text = pProveedor.Direccion;
                cboActivo.Enabled = true;

                foreach (ComboBoxItem item in cboActivo.Items)
                {
                    if ((bool)item.Value == pProveedor.Activo)
                    {
                        int index = cboActivo.Items.IndexOf(item);
                        cboActivo.SelectedIndex = index;
                        break;
                    }
                }
            }
        }

        private void mtnProveedor_Load(object sender, EventArgs e)
        {
            txtRuc.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            if (txtRuc.Text.Trim() == "" && txtRazonSocial.Text.Trim() == "" && txtTelefono.Text.Trim() == "" && txtCorreo.Text.Trim() == "" && txtDireccion.Text.Trim() == "")
            {
                MessageBox.Show("Debe completar todos los campos", "Mensaje");
                return;
            }
            

            Proveedor oProveedor = new Proveedor()
            {
                IdProveedor = Convert.ToInt32(txtIdProveedor.Text.ToString()),
                Ruc = txtRuc.Text.Trim(),
                RazonSocial = txtRazonSocial.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Correo = txtCorreo.Text.Trim(),
                Direccion = txtDireccion.Text.Trim(),
                Activo = Convert.ToBoolean(((ComboBoxItem)cboActivo.SelectedItem).Value)
            };

            bool Respuesta = false;
            string msgSuccess = "";
            string msgError = "";
            if (modoEditar)
            {
                Respuesta = CD_Proveedor.ModificarProveedor(oProveedor);
                msgSuccess = "Proveedor Modificado \n¿Desea registrar un nuevo proveedor ahora?";
                msgError = "No se pudo modificar el proveedor, \nes posible que ya se encuentre registrado";
            }
            else
            {

                Respuesta = CD_Proveedor.RegistrarProveedor(oProveedor);
                msgSuccess = "Proveedor Registrado \n¿Desea registrar un nuevo proveedor ahora?";
                msgError = "No se pudo registrar el proveedor, \nes posible que ya se encuentre registrado";

            }

            if (Respuesta)
            {
                if (MessageBox.Show(msgSuccess, "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    txtRuc.Text = "";
                    txtRazonSocial.Text = "";
                    txtTelefono.Text = "";
                    txtCorreo.Text = "";
                    txtDireccion.Text = "";
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
                    txtRuc.Focus();
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
    }
}
