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
    public partial class mtnProducto : Form
    {
        private bool modoEditar = false;
        public mtnProducto(Producto pProducto = null)
        {
            InitializeComponent();
            cboActivo.Enabled = false;
            cargarCombosSeleccion();

            if (pProducto != null)
            {
                modoEditar = true;
                cboActivo.Enabled = true;
                txtIdProducto.Text = pProducto.IdProducto.ToString();
                txtCodigo.Text = pProducto.Codigo;
                txtNombre.Text = pProducto.Nombre;
                txtDescripcion.Text = pProducto.Descripcion;

                foreach (ComboBoxItem item in cboCategoria.Items)
                {
                    if ((int)item.Value == pProducto.IdCategoria)
                    {
                        int index = cboCategoria.Items.IndexOf(item);
                        cboCategoria.SelectedIndex = index;
                        break;
                    }
                }

                foreach (ComboBoxItem item in cboActivo.Items)
                {
                    if ((bool)item.Value == pProducto.Activo)
                    {
                        int index = cboActivo.Items.IndexOf(item);
                        cboActivo.SelectedIndex = index;
                        break;
                    }
                }
            }
        }

        private void cargarCombosSeleccion()
        {
            cboActivo.Items.Add(new ComboBoxItem() { Value = true, Text = "Si" });
            cboActivo.Items.Add(new ComboBoxItem() { Value = false, Text = "No" });
            cboActivo.DisplayMember = "Text";
            cboActivo.ValueMember = "Value";
            cboActivo.SelectedIndex = 0;

            List<Categoria> oListaCategoria = CD_Categoria.ObtenerCategoria();


            foreach (Categoria row in oListaCategoria.Where(x => x.Activo == true))
            {
                cboCategoria.Items.Add(new ComboBoxItem() { Value = row.IdCategoria, Text = row.Descripcion });
            }
            cboCategoria.DisplayMember = "Text";
            cboCategoria.ValueMember = "Value";
            cboCategoria.SelectedIndex = 0;
        }

        private void mtnProducto_Load(object sender, EventArgs e)
        {
            txtNombre.Focus();
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text.Trim() == "" && txtDescripcion.Text.Trim() == "")
            {
                MessageBox.Show("Debe completar todos los campos", "Mensaje");
                return;
            }


            Producto oProducto = new Producto()
            {
                IdProducto = Convert.ToInt32(txtIdProducto.Text.Trim()),
                Nombre = txtNombre.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim(),
                IdCategoria = Convert.ToInt32(((ComboBoxItem)cboCategoria.SelectedItem).Value),
                Activo = Convert.ToBoolean(((ComboBoxItem)cboActivo.SelectedItem).Value)
            };

            bool Respuesta = false;
            string msgSuccess = "";
            string msgError = "";
            if (modoEditar)
            {
                Respuesta = CD_Producto.ModificarProducto(oProducto);
                msgSuccess = "Producto Modificado \n¿Desea registrar un nuevo Producto ahora?";
                msgError = "No se pudo modificar el Producto, \nes posible que ya se encuentre registrado";
            }
            else
            {

                Respuesta = CD_Producto.RegistrarProducto(oProducto);
                msgSuccess = "Producto Registrado \n¿Desea registrar un nuevo Producto ahora?";
                msgError = "No se pudo registrar el Producto, \nes posible que ya se encuentre registrado";

            }

            if (Respuesta)
            {
                if (MessageBox.Show(msgSuccess, "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    txtIdProducto.Text = "0";
                    txtCodigo.Text = "Autogenerado";
                    txtCodigo.ForeColor = Color.Red;
                    txtNombre.Text = "";
                    txtDescripcion.Text = "";
                    cboCategoria.SelectedIndex = 0;

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
                MessageBox.Show(msgError, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
