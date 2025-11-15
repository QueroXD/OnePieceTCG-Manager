using OnePieceTCG_Manager.Data;
using OnePieceTCG_Manager.Gestion.Views;
using OnePieceTCG_Manager.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            LoadFilters();
            ShowListView();
        }

        // --------------------------
        // Carga inicial de filtros
        // --------------------------
        private void LoadFilters()
        {
            using (var db = new OnePieceContext())
            {
                // COLOR
                cbColor.Items.Add("Todos");
                cbColor.Items.AddRange(
                    db.CardStock.Select(c => c.color).Where(x => x != null).Distinct().ToArray()
                );
                cbColor.SelectedIndex = 0;

                // TYPE
                cbType.Items.Add("Todos");
                cbType.Items.AddRange(
                    db.CardStock.Select(c => c.type).Where(x => x != null).Distinct().ToArray()
                );
                cbType.SelectedIndex = 0;

                // SUBTYPE
                cbSubType.Items.Add("Todos");
                cbSubType.Items.AddRange(
                    db.CardStock.Select(c => c.subType).Where(x => x != null).Distinct().ToArray()
                );
                cbSubType.SelectedIndex = 0;

                // ORDENACIÓN
                cbOrder.Items.AddRange(new[] {
                    "Nombre",
                    "Rareza",
                    "Color",
                    "Coste",
                    "Unidades"
                });
                cbOrder.SelectedIndex = 0;
            }

            btnApplyFilters.Click += (s, e) => RefreshView();
        }

        // --------------------------
        // Obtiene los datos filtrados
        // --------------------------
        private List<CardStock> GetFilteredData()
        {
            using (var db = new OnePieceContext())
            {
                var query = db.CardStock.AsQueryable();

                // FILTRO COLOR
                if (cbColor.SelectedIndex > 0)
                    query = query.Where(c => c.color == cbColor.SelectedItem.ToString());

                // FILTRO TYPE
                if (cbType.SelectedIndex > 0)
                    query = query.Where(c => c.type == cbType.SelectedItem.ToString());

                // FILTRO SUBTYPE
                if (cbSubType.SelectedIndex > 0)
                    query = query.Where(c => c.subType == cbSubType.SelectedItem.ToString());

                // ORDENACIÓN
                switch (cbOrder.SelectedItem.ToString())
                {
                    case "Nombre":
                        query = query.OrderBy(c => c.cardName);
                        break;
                    case "Rareza":
                        query = query.OrderBy(c => c.rarity);
                        break;
                    case "Color":
                        query = query.OrderBy(c => c.color);
                        break;
                    case "Coste":
                        query = query.OrderBy(c => c.cost);
                        break;
                    case "Unidades":
                        query = query.OrderBy(c => c.units);
                        break;
                }

                return query.ToList();
            }
        }

        // --------------------------
        // Refresca la vista activa
        // --------------------------
        private void RefreshView()
        {
            var data = GetFilteredData();

            if (lblMode.Text.Contains("Lista"))
            {
                if (panelContainer.Controls[0] is UC_StockListView listView)
                    listView.LoadData(data);
            }
            else
            {
                if (panelContainer.Controls[0] is UC_StockGalleryView gallery)
                    _ = gallery.LoadDataAsync(data);
            }
        }

        // --------------------------
        // Mostrar vista LISTA
        // --------------------------
        private void ShowListView()
        {
            lblMode.Text = "Modo: Lista";

            var data = GetFilteredData();

            panelContainer.Controls.Clear();
            var listView = new UC_StockListView();
            listView.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(listView);

            listView.LoadData(data);
        }

        // --------------------------
        // Mostrar vista GALERÍA
        // --------------------------
        private void ShowGalleryView()
        {
            lblMode.Text = "Modo: Galería";

            var data = GetFilteredData();

            panelContainer.Controls.Clear();
            var gallery = new UC_StockGalleryView();
            gallery.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(gallery);

            _ = gallery.LoadDataAsync(data);
        }

        private void toolListView_Click(object sender, EventArgs e)
        {
            ShowListView();
        }

        private void toolGalleryView_Click(object sender, EventArgs e)
        {
            ShowGalleryView();
        }
    }
}
