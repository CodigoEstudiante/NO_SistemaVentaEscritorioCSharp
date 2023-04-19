
using CapaDatos;
using CapaModelo;
using ClosedXML.Excel;
using SistemaVentas.Reutilizable;
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
    public partial class rptProductoTienda : Form
    {
        public rptProductoTienda()
        {
            InitializeComponent();
        }
        DataTable dtReporte = new DataTable();
        private void rptProductoTienda_Load(object sender, EventArgs e)
        {
            List<Tienda> oListaTienda = CD_Tienda.ObtenerTiendas();

            cboTienda.Items.Add(new ComboBoxItem() { Value = 0, Text = "Seleccionar Todos" });
            foreach (Tienda row in oListaTienda.Where(x => x.Activo == true))
            {
                cboTienda.Items.Add(new ComboBoxItem() { Value = row.IdTienda, Text = row.Nombre });
            }
            cboTienda.DisplayMember = "Text";
            cboTienda.ValueMember = "Value";
            cboTienda.SelectedIndex = 0;

            dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReporte.MultiSelect = false;
            dgvReporte.ReadOnly = true;
            dgvReporte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReporte.AllowUserToAddRows = false;

        }
        private void cboBuscar_Click(object sender, EventArgs e)
        {

            dtReporte = CD_Reportes.ReporteProductoTienda(int.Parse(((ComboBoxItem)cboTienda.SelectedItem).Value.ToString()), txtCodigoProducto.Text.Trim());
            if(dtReporte != null)
            {
                dgvReporte.DataSource = dtReporte;
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if(dgvReporte.Rows.Count > 0)
            {
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = string.Format("Reporte_{0}.xlsx", DateTime.Today.ToString("ddMMyyyy"));
                savefile.Filter = "Excel Files|*.xlsx";
                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string informe = "Informe";
                        XLWorkbook wb = new XLWorkbook();
                        wb.Worksheets.Add(dtReporte, informe);
                        wb.SaveAs(savefile.FileName);
                        MessageBox.Show("Reporte Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Error al generar reporte", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }


                }
            }
            else
            {
                MessageBox.Show("No existen datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            
        }
    }
}
