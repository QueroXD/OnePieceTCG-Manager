using OnePieceTCG_Manager.Data;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Gestion.Views
{
    public partial class UC_StockListView : UserControl
    {
        public UC_StockListView()
        {
            InitializeComponent();
            LoadStockData();
        }

        private void LoadStockData()
        {
            using (var db = new OnePieceContext())
            {
                var stockList = db.CardStock
                    .Select(c => new
                    {
                        ID = c.cardId,
                        Nombre = c.cardName,
                        Rareza = c.rarity,
                        Tipo = c.type,
                        Color = c.color,
                        Coste = c.cost,
                        Unidades = c.units
                    })
                    .ToList();

                dataGrid.DataSource = stockList;
            }

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
                    LoadStockData();
                }
            };
        }
    }
}
