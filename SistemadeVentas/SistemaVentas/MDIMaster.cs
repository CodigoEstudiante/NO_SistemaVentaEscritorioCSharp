using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos;
using CapaModelo;

namespace SistemaVentas
{
    public partial class MDIMaster : Form
    {
        public static int IdUsuario;
        public MDIMaster(int pIdUsuario = 0)
        {
            InitializeComponent();
            IdUsuario = pIdUsuario;
            //DEFINIMOS DISEÑO DEL FORMULARIO MDI
            this.IsMdiContainer = true;
            //this.MaximizeBox = false;
            //this.MinimizeBox = false;
            this.WindowState = FormWindowState.Maximized;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(IdUsuario == 0)
            {
                this.Close();
            }

            Configuracion.oUsuario = CD_Usuario.ObtenerDetalleUsuario(IdUsuario);


            //Configuracion.oUsuario = CD_Usuario.ObtenerDetalleUsuario("Tienda", "Tienda123");

            //string pathImage= Path.GetFullPath(Path.Combine(Application.StartupPath, @"../../") + @"\Iconos\print.png");
            //Image image1 = Image.FromFile(pathImage);

            StatusStrip sttStrip = new StatusStrip();
            sttStrip.Dock = DockStyle.Top;
            sttStrip.Font = new System.Drawing.Font("Segoe UI", 12F);

            ToolStripStatusLabel tslblUsuario = new ToolStripStatusLabel("Usuario: ");
            ToolStripStatusLabel tslblData1 = new ToolStripStatusLabel(Configuracion.oUsuario.Nombres + " " + Configuracion.oUsuario.Apellidos);
            tslblUsuario.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tslblUsuario.Spring = true;
            tslblUsuario.TextAlign = ContentAlignment.MiddleRight;

            ToolStripStatusLabel tslblTipoUsuario = new ToolStripStatusLabel("Rol: ");
            ToolStripStatusLabel tslblData2 = new ToolStripStatusLabel(Configuracion.oUsuario.oRol.Descripcion);
            tslblTipoUsuario.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            ToolStripStatusLabel tslblTienda = new ToolStripStatusLabel("Tienda: ");
            ToolStripStatusLabel tslblData3 = new ToolStripStatusLabel(Configuracion.oUsuario.oTienda.Nombre);
            tslblTienda.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));



            sttStrip.Items.Add(tslblUsuario);
            sttStrip.Items.Add(tslblData1);
            sttStrip.Items.Add(tslblTipoUsuario);
            sttStrip.Items.Add(tslblData2);
            sttStrip.Items.Add(tslblTienda);
            sttStrip.Items.Add(tslblData3);
            Controls.Add(sttStrip);


            MenuStrip MnuStrip = new MenuStrip();
            MnuStrip.BackColor = Color.LightSkyBlue;
            
            foreach (CapaModelo.Menu oMenu in Configuracion.oUsuario.oListaMenu)
            {
                ToolStripMenuItem MnuStripItem = new ToolStripMenuItem(oMenu.Nombre);
                MnuStripItem.Font = new System.Drawing.Font("Segoe UI", 11F, FontStyle.Bold);
                string pathImage1 = Path.GetFullPath(Path.Combine(Application.StartupPath, @"../../") + @"\Iconos\" + oMenu.Icono);
                MnuStripItem.Image = new Bitmap(pathImage1);

                if (oMenu.oSubMenu != null)
                {
                    foreach (SubMenu oSubMenu in oMenu.oSubMenu.Where(x => x.Activo == true))
                    {
                        ToolStripMenuItem SubMenuStringItem = new ToolStripMenuItem(oSubMenu.Nombre, null, ToolStripMenuItem_Click, oSubMenu.NombreFormulario);
                        SubMenuStringItem.Font = new System.Drawing.Font("Segoe UI", 12F);
                        string pathImage2 = Path.GetFullPath(Path.Combine(Application.StartupPath, @"../../") + @"\Iconos\icon_item.png");
                        SubMenuStringItem.Image = new Bitmap(pathImage2);
                        MnuStripItem.DropDownItems.Add(SubMenuStringItem);
                    }
                }
                MnuStrip.Items.Add(MnuStripItem);
            }

            ToolStripMenuItem MnuStripItemExit = new ToolStripMenuItem("Salir",null,ToolStripMenuItemSalir_Click,"");
            MnuStripItemExit.Font = new System.Drawing.Font("Segoe UI", 11F,FontStyle.Bold);
            string pathImage3 = Path.GetFullPath(Path.Combine(Application.StartupPath, @"../../") + @"\Iconos\icon_exit.png");
            MnuStripItemExit.Image = new Bitmap(pathImage3);
            MnuStrip.Items.Add(MnuStripItemExit);

            this.MainMenuStrip = MnuStrip;
            Controls.Add(MnuStrip);

        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void ToolStripMenuItemSalir_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar sesión?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                
                this.Close();
            }
        }

        private void ToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ToolStripMenuItem oMenuSelect = (ToolStripMenuItem)sender;

            if(oMenuSelect.Name != "")
            {
                Assembly asm = Assembly.GetEntryAssembly();

                Type formtype = asm.GetType(string.Format("{0}.{1}", asm.GetName().Name, oMenuSelect.Name));

                if(formtype == null)
                {
                    MessageBox.Show("Formulario no encontrado");
                }
                else
                {
                    Form formulario = (Form)Activator.CreateInstance(formtype);
                    MostrarFormulario(formulario, this);

                    formulario.WindowState = FormWindowState.Normal;
                    formulario.StartPosition = FormStartPosition.CenterScreen;
                    formulario.Activate();
                }
            }
        }

        public void MostrarFormulario(Form frmhijo, Form frmpapa)
        {
            Form FormularioEncontrado = new Form();
            bool cargado = false;
            foreach (Form Formulario in frmpapa.MdiChildren)
            {
                if (Formulario.Name == frmhijo.Name)
                {
                    FormularioEncontrado = Formulario;
                    cargado = true;
                    break;
                }
            }
            
            if (!cargado)
            {
                frmhijo.MdiParent = frmpapa;
                frmhijo.Show();
            }
            else
            {
                FormularioEncontrado.WindowState = FormWindowState.Normal;
                FormularioEncontrado.Activate();
            }
           
        }

      
    }

}
