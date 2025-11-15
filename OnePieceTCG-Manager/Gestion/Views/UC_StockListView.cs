using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OnePieceTCG_Manager.Models;

namespace OnePieceTCG_Manager.Gestion.Views
{
    public partial class UC_StockListView : UserControl
    {
        public UC_StockListView()
        {
            InitializeComponent();
        }

        public void LoadData(IEnumerable<CardStock> data)
        {
            var list = data.Select(c => new
            {
                ID = c.cardId,
                Nombre = c.cardName,
                Rareza = c.rarity,
                Tipo = c.type,
                Color = c.color,
                Coste = c.cost,
                Unidades = c.units
            }).ToList();

            dataGrid.DataSource = list;

            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.ReadOnly = true;
            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;

            dataGrid.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    string id = dataGrid.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    var frm = new FrmAddStock(id, modoSoloUnidades: true);
                    frm.ShowDialog();

                    // no refresca aquí, lo hará FrmVerStock
                }
            };
        }
    }
}
