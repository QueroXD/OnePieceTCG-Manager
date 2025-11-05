using OnePieceTCG_Manager.Data;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Gestion
{
    public partial class FrmVerStock : Form
    {
        public FrmVerStock()
        {
            InitializeComponent();
        }

        private void FrmVerStock_Load(object sender, EventArgs e)
        {
            LoadStockData();
        }

        private void LoadStockData()
        {
            try
            {
                using (var db = new OnePieceContext())
                {
                    // Traer todos los registros de CardStock
                    var stockList = db.CardStock
                        .Select(c => new
                        {
                            ID = c.cardId,
                            Nombre = c.cardName,
                            Rareza = c.rarity,
                            Tipo = c.type,
                            Subtipo = c.subType,
                            Atributo = c.attribute,
                            Color = c.color,
                            Coste = c.cost,
                            Counter = c.counter,
                            Poder = c.power,
                            SetDesc = c.setDesc,
                            Alter = c.isAlter,
                            Descripción = c.description,
                            Unidades = c.units
                        })
                        .ToList();

                    dataStock.DataSource = stockList;
                }

                // Ajuste visual
                dataStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataStock.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataStock.ReadOnly = true;
                dataStock.AllowUserToAddRows = false;
                dataStock.AllowUserToDeleteRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al cargar los datos del stock:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
