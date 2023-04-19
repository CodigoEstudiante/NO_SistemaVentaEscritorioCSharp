using CapaDatos;
using CapaModelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaVentas
{
    public partial class frmDocumentoCompra : Form
    {
        string LeerDocumento = "";
        int IdComra = 0;
        public frmDocumentoCompra(int pIdCompra = 0)
        {
            InitializeComponent();
            this.IdComra = pIdCompra;
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintDialog();
        }
        private void frmDocumentoCompra_Load(object sender, EventArgs e)
        {
            Compra oCompra = CD_Compra.ObtenerDetalleCompra(IdComra);

            if (oCompra != null)
            {
                string filasproductos = "";
                string NombreDocumento = "";
                string Plantilla = "";
                string PlantillaEditar = "";

                NombreDocumento = string.Format("c{0}.html", oCompra.Codigo);


                Plantilla = Path.GetFullPath(Path.Combine(Application.StartupPath, @"../../") + @"\Documento\DocumentoCompra.html");
                PlantillaEditar = System.IO.File.ReadAllText(Plantilla);

                
                PlantillaEditar = PlantillaEditar.Replace("!codigo¡", oCompra.Codigo);
                PlantillaEditar = PlantillaEditar.Replace("!fechacompra¡",  oCompra.FechaCompra);

                PlantillaEditar = PlantillaEditar.Replace("!rucproveedor¡",  oCompra.oProveedor.Ruc);
                PlantillaEditar = PlantillaEditar.Replace("!nombreproveedor¡",  oCompra.oProveedor.RazonSocial);


                PlantillaEditar = PlantillaEditar.Replace("!ructienda¡",  oCompra.oTienda.RUC);
                PlantillaEditar = PlantillaEditar.Replace("!nombretienda¡",  oCompra.oTienda.Nombre);
                

                foreach (DetalleCompra r in oCompra.oListaDetalleCompra)
                {
                    filasproductos += string.Format("<tr><td><center>{0}</center></td><td><center>{1}</center></td><td><center>{2}</center></td><td><center>{3}<center></td></tr>",
                        r.Cantidad, r.oProducto.Nombre, r.PrecioUnitarioCompra, r.TotalCosto);
                }
                PlantillaEditar = PlantillaEditar.Replace("!filasproductos¡", filasproductos);
                

                PlantillaEditar = PlantillaEditar.Replace("!totacosto¡", oCompra.TotalCosto.ToString());


                File.WriteAllText(Path.GetFullPath(Path.Combine(Application.StartupPath, @"../../") + @"\Documento\" + NombreDocumento), PlantillaEditar);
                LeerDocumento = Path.GetFullPath(Path.Combine(Application.StartupPath, @"../../") + @"\Documento\" + NombreDocumento);
                this.webBrowser1.Url = new Uri(String.Format("file:///{0}", LeerDocumento));
            }
        }
        private void frmDocumentoCompra_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (File.Exists(LeerDocumento))
            {
                File.Delete(LeerDocumento);
            }
        }

        
    }
}
