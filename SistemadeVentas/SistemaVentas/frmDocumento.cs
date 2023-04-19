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
    public partial class frmDocumento : Form
    {
        string LeerDocumento = "";
        int IdVenta = 0;
        public frmDocumento(int pIdVenta = 0)
        {
            InitializeComponent();
            IdVenta = pIdVenta;
        }

        private void frmDocumento_Load(object sender, EventArgs e)
        {
            Venta oVenta = CD_Venta.ObtenerDetalleVenta(IdVenta);

            if(oVenta != null)
            {
                string filasproductos = "";
                string NombreDocumento = "";
                string Plantilla = "";
                string PlantillaEditar = "";
                string TablaFacture = "<table width='250' border='1'>" +
                                "<tr style='border:1px solid black;'>" +
                                    "<td bgcolor='#D9D9D9'>Pago con S/.</td>" +
                                    "<td>{0}</td>" +
                                    "<td bgcolor='#D9D9D9'>Cambio S/.</td>" +
                                    "<td>{1}</td>" +
                                "</tr>" +
                            "</table>";

                NombreDocumento = string.Format("{0}.html", oVenta.Codigo);


                Plantilla = Path.GetFullPath(Path.Combine(Application.StartupPath, @"../../") + @"\Documento\BoletaVenta.html");
                PlantillaEditar = System.IO.File.ReadAllText(Plantilla);


                PlantillaEditar = PlantillaEditar.Replace("!ructienda¡", oVenta.oTienda.RUC);
                PlantillaEditar = PlantillaEditar.Replace("!codigo¡", oVenta.Codigo);
                PlantillaEditar = PlantillaEditar.Replace("!nombreempleado¡", string.Format("{0} {1}", oVenta.oUsuario.Nombres, oVenta.oUsuario.Apellidos));
                PlantillaEditar = PlantillaEditar.Replace("!tienda_direccion¡", string.Format("{0} - {1}", oVenta.oTienda.Nombre, oVenta.oTienda.Direccion));
                PlantillaEditar = PlantillaEditar.Replace("!nombrecliente¡", oVenta.oCliente.Nombre);
                PlantillaEditar = PlantillaEditar.Replace("!direccioncliente¡", oVenta.oCliente.Direccion);
                PlantillaEditar = PlantillaEditar.Replace("!documentocliente¡", oVenta.oCliente.NumeroDocumento);
                PlantillaEditar = PlantillaEditar.Replace("!telefonocliente¡", oVenta.oCliente.Telefono);
                PlantillaEditar = PlantillaEditar.Replace("!fecharegistro¡", oVenta.FechaRegistro);
                PlantillaEditar = PlantillaEditar.Replace("!totacosto¡", Convert.ToDecimal(oVenta.TotalCosto.ToString()).ToString());
                TablaFacture = string.Format(TablaFacture, oVenta.ImporteRecibido, oVenta.ImporteCambio);
                PlantillaEditar = PlantillaEditar.Replace("!tablafactura¡", TablaFacture);

                foreach (DetalleVenta r in oVenta.oListaDetalleVenta)
                {
                    filasproductos += string.Format("<tr><td><center>{0}</center></td><td><center>{1}</center></td><td><center>{2}</center></td><td><center>{3}<center></td></tr>",
                        r.Cantidad, r.NombreProducto, r.PrecioUnidad, r.ImporteTotal);
                }
                PlantillaEditar = PlantillaEditar.Replace("!filasproductos¡", filasproductos);



                PlantillaEditar = PlantillaEditar.Replace("!tipodocumento¡", oVenta.TipoDocumento == "Boleta" ? "BOLETA DE VENTA" : "FACTURA");
                PlantillaEditar = PlantillaEditar.Replace("!color¡", oVenta.TipoDocumento == "Boleta" ? "#F36F1C" : "#549AEE");




                File.WriteAllText(Path.GetFullPath(Path.Combine(Application.StartupPath, @"../../") + @"\Documento\" + NombreDocumento), PlantillaEditar);
                LeerDocumento = Path.GetFullPath(Path.Combine(Application.StartupPath, @"../../") + @"\Documento\" + NombreDocumento);
                this.webBrowser1.Url = new Uri(String.Format("file:///{0}", LeerDocumento));
            }
        }

        private void frmDocumento_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (File.Exists(LeerDocumento))
            {
                File.Delete(LeerDocumento);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintDialog();
        }
    }
}
