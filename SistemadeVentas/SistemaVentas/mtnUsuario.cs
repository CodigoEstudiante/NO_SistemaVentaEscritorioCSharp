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
    public partial class mtnUsuario : Form
    {
        private bool modoEditar = false;
        public mtnUsuario(Usuario pUsuario = null)
        {
            InitializeComponent();

            cboActivo.Enabled = false;
            cargarCombosSeleccion();

            if (pUsuario != null)
            {
                modoEditar = true;
                cboActivo.Enabled = true;
                txtIdUsuario.Text = pUsuario.IdUsuario.ToString();
                txtNombres.Text = pUsuario.Nombres;
                txtApellidos.Text = pUsuario.Apellidos;
                txtCorreo.Text = pUsuario.Correo;
                txtUsuario.Text = pUsuario.NombreUsuario;
                txtClave.Text = pUsuario.Clave;

                foreach(ComboBoxItem item in cboTienda.Items)
                {
                    if((int)item.Value == pUsuario.IdTienda)
                    {
                        int index = cboTienda.Items.IndexOf(item);
                        cboTienda.SelectedIndex = index;
                        break;
                    }
                }

                foreach(ComboBoxItem item in cboRol.Items)
                {
                    if((int)item.Value == pUsuario.IdRol)
                    {
                        int index = cboRol.Items.IndexOf(item);
                        cboRol.SelectedIndex = index;
                        break;
                    }
                }

                foreach (ComboBoxItem item in cboActivo.Items)
                {
                    if ((bool)item.Value == pUsuario.Activo)
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

            List<Rol> oListaRol = CD_Rol.ObtenerRol();
            List<Tienda> oListaTienda = CD_Tienda.ObtenerTiendas();

            foreach (Tienda row in oListaTienda.Where(x => x.Activo == true))
            {
                cboTienda.Items.Add(new ComboBoxItem() { Value = row.IdTienda, Text = row.Nombre });
            }
            cboTienda.DisplayMember = "Text";
            cboTienda.ValueMember = "Value";
            cboTienda.SelectedIndex = 0;


            foreach (Rol row in oListaRol.Where(x => x.Activo == true))
            {
                cboRol.Items.Add(new ComboBoxItem() { Value = row.IdRol, Text = row.Descripcion });
            }
            cboRol.DisplayMember = "Text";
            cboRol.ValueMember = "Value";
            cboRol.SelectedIndex = 0;
        }

        private void mtnUsuario_Load(object sender, EventArgs e)
        {
            txtNombres.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            if (txtNombres.Text.Trim() == "" && txtApellidos.Text.Trim() == "" && txtCorreo.Text.Trim() == "" && txtUsuario.Text.Trim() == "" && txtClave.Text.Trim() == "")
            {
                MessageBox.Show("Debe completar todos los campos", "Mensaje");
                return;
            }
            

            Usuario oUsuario = new Usuario()
            {
                IdUsuario = Convert.ToInt32( txtIdUsuario.Text.Trim()),
                Nombres = txtNombres.Text.Trim(),
                Apellidos = txtApellidos.Text.Trim(),
                Correo = txtCorreo.Text.Trim(),
                NombreUsuario = txtUsuario.Text.Trim(),
                Clave = txtClave.Text.Trim(),
                IdTienda = Convert.ToInt32(((ComboBoxItem)cboTienda.SelectedItem).Value),
                IdRol = Convert.ToInt32(((ComboBoxItem)cboRol.SelectedItem).Value),
                Activo = Convert.ToBoolean(((ComboBoxItem)cboActivo.SelectedItem).Value)
            };

            bool Respuesta = false;
            string msgSuccess = "";
            string msgError = "";
            if (modoEditar)
            {
                Respuesta = CD_Usuario.ModificarUsuario(oUsuario);
                msgSuccess = "Usuario Modificado \n¿Desea registrar un nuevo usuario ahora?";
                msgError = "No se pudo modificar el usuario, \nes posible que ya se encuentre registrado";
            }
            else
            {

                Respuesta = CD_Usuario.RegistrarUsuario(oUsuario);
                msgSuccess = "Usuario Registrado \n¿Desea registrar un nuevo usuario ahora?";
                msgError = "No se pudo registrar el usuario, \nes posible que ya se encuentre registrado";

            }

            if (Respuesta)
            {
                if (MessageBox.Show(msgSuccess, "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    txtNombres.Text = "";
                    txtApellidos.Text = "";
                    txtCorreo.Text = "";
                    txtUsuario.Text = "";
                    txtClave.Text = "";
                    cboRol.SelectedIndex = 0;
                    cboTienda.SelectedIndex = 0;

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
                    txtNombres.Focus();
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
