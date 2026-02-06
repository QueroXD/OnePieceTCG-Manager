using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OnePieceTCG_Manager.Models;

namespace OnePieceTCG_Manager.Gestion.Views
{
    public partial class UC_StockListView : UserControl
    {
        public event Action<CardStock> CardDoubleClicked;

        private List<CardStock> _data = new List<CardStock>();

        public UC_StockListView()
        {
            InitializeComponent();

            dataGrid.CellDoubleClick += DataGrid_CellDoubleClick;
        }

        public void LoadData(IEnumerable<CardStock> data)
        {
            _data = data.ToList();

            var list = _data.Select(c => new
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
        }

        private void DataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _data.Count)
                return;

            var card = _data[e.RowIndex];
            CardDoubleClicked?.Invoke(card);
        }
    }
}
