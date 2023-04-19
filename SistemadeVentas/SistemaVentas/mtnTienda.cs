using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos;
using CapaModelo;
using SistemaVentas.Reutilizable;

namespace SistemaVentas
{
    public partial class mtnTienda : Form
    {
        bool modoEditar = false;
        public mtnTienda(Tienda pTienda = null)
        {
            InitializeComponent();

            cboActivo.Items.Add(new ComboBoxItem() { Value = true, Text = "Si" });
            cboActivo.Items.Add(new ComboBoxItem() { Value = false, Text = "No" });
            cboActivo.DisplayMember = "Text";
            cboActivo.ValueMember = "Value";
            cboActivo.SelectedIndex = 0;

            if (pTienda != null)
            {
                modoEditar = true;
                txtIdTienda.Text = pTienda.IdTienda.ToString();
                txtNombre.Text = pTienda.Nombre;
                txtRuc.Text = pTienda.RUC;
                txtDireccion.Text = pTienda.Direccion;
                txtTelefono.Text = pTienda.Telefono;
                cboActivo.Enabled = true;

                foreach(ComboBoxItem item in cboActivo.Items)
                {
                    if((bool)item.Value == pTienda.Activo)
                    {
                        int index =  cboActivo.Items.IndexOf(item);
                        cboActivo.SelectedIndex = index;
                        break;
                    }
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mtnTienda_Load(object sender, EventArgs e)
        {
            txtNombre.Focus();
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            if(txtNombre.Text.Trim() == "" && txtRuc.Text.Trim() == "" && txtDireccion.Text.Trim() == "" && txtTelefono.Text.Trim() == "")
            {
                MessageBox.Show("Debe completar todos los campos", "Mensaje");
                return;
            }



            Tienda oTienda = new Tienda()
            {
                IdTienda = Convert.ToInt32(txtIdTienda.Text.ToString()),
                Nombre = txtNombre.Text.Trim(),
                RUC = txtRuc.Text.Trim(),
                Direccion = txtDireccion.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Activo = Convert.ToBoolean( ((ComboBoxItem)cboActivo.SelectedItem).Value)
            };

            bool Respuesta = false;
            string msgSuccess = "";
            string msgError = "";
            if (modoEditar)
            {
                Respuesta = CD_Tienda.ModificarTienda(oTienda);
                msgSuccess = "Tienda Modificada \n¿Desea registrar una nueva tienda ahora?";
                msgError = "No se pudo modificar la tienda, \nes posible que ya se encuentre registrada";
            }
            else
            {
                
                Respuesta = CD_Tienda.RegistrarTienda(oTienda);
                msgSuccess = "Tienda Registrada \n¿Desea registrar una nueva tienda ahora?";
                msgError = "No se pudo registrar la tienda, \nes posible que ya se encuentre registrada";

            }

            if (Respuesta)
            {
                if (MessageBox.Show(msgSuccess, "Mensaje", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    txtNombre.Text = "";
                    txtRuc.Text = "";
                    txtDireccion.Text = "";
                    txtTelefono.Text = "";
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
                    txtNombre.Focus();
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show(msgError,"Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
            

        }
    }
}
