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
    public partial class mtnCategoria : Form
    {
        private bool modoEditar = false;
        public mtnCategoria(Categoria pCategoria = null)
        {
            InitializeComponent();
            cboActivo.Items.Add(new ComboBoxItem() { Value = true, Text = "Si" });
            cboActivo.Items.Add(new ComboBoxItem() { Value = false, Text = "No" });
            cboActivo.DisplayMember = "Text";
            cboActivo.ValueMember = "Value";
            cboActivo.SelectedIndex = 0;
            cboActivo.Enabled = false;


            if (pCategoria != null)
            {
                modoEditar = true;
                txtIdCategoria.Text = pCategoria.IdCategoria.ToString();
                txtDescripcion.Text = pCategoria.Descripcion.ToString();
                cboActivo.Enabled = true;

                foreach (ComboBoxItem item in cboActivo.Items)
                {
                    if ((bool)item.Value == pCategoria.Activo)
                    {
                        int index = cboActivo.Items.IndexOf(item);
                        cboActivo.SelectedIndex = index;
                        break;
                    }
                }
            }
        }

        private void mtnCategoria_Load(object sender, EventArgs e)
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

            Categoria oCategoria = new Categoria()
            {
                IdCategoria = Convert.ToInt32(txtIdCategoria.Text.Trim()),
                Descripcion = txtDescripcion.Text.Trim(),
                Activo = Convert.ToBoolean(((ComboBoxItem)cboActivo.SelectedItem).Value)
            };


            bool Respuesta = false;
            string msgSuccess = "";
            string msgError = "";
            if (modoEditar)
            {
                Respuesta = CD_Categoria.ModificarCategoria(oCategoria);
                msgSuccess = "Categoria Modificada \n¿Desea registrar una nueva categoria ahora?";
                msgError = "No se pudo modificar la categoria, \nes posible que ya se encuentre registrada";
            }
            else
            {

                Respuesta = CD_Categoria.RegistrarCategoria(oCategoria);
                msgSuccess = "Categoria Registrada \n¿Desea registrar una nueva categoria ahora?";
                msgError = "No se pudo registrar la categoria, \nes posible que ya se encuentre registrada";

            }

            if (Respuesta)
            {
                if (MessageBox.Show(msgSuccess, "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    txtDescripcion.Text = "";
                    txtIdCategoria.Text = "0";
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
