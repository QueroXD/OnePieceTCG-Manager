using OnePieceTCG_Manager.Models;
using OnePieceTCG_Manager.Services;
using OnePieceTCG_Manager.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace OnePieceTCG_Manager.Gestion
{
    public class FrmKpiStats : Form
    {
        private readonly CardStockService _cardStockService = new CardStockService();

        private Label _lblUniqueCards;
        private Label _lblUniqueSub;
        private Label _lblUnits;
        private Label _lblUnitsSub;
        private Label _lblUsed;
        private Label _lblUsedSub;
        private Label _lblAvailability;
        private Label _lblAvailabilitySub;
        private Chart _chartByColor;
        private Chart _chartByType;
        private ListView _lvHighlights;
        private Button _btnRefresh;

        public FrmKpiStats()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            ModernUi.ApplyFormTheme(this);
            Text = "Estadisticas Stock";
            WindowState = FormWindowState.Maximized;
            BackColor = ModernUi.AppBack;

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = ModernUi.AppBack,
                Padding = new Padding(16)
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 134F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var topBar = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ModernUi.AppBack
            };

            var lblTitle = new Label
            {
                AutoSize = true,
                Text = "KPIs de stock",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = ModernUi.TextPrimary,
                Location = new Point(0, 8)
            };

            var lblSub = new Label
            {
                AutoSize = true,
                Text = "Vista consolidada del inventario, uso en decks y disponibilidad real.",
                Font = new Font("Segoe UI", 10F),
                ForeColor = ModernUi.TextMuted,
                Location = new Point(2, 42)
            };

            _btnRefresh = new Button
            {
                Text = "Actualizar",
                Size = new Size(120, 38),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            ModernUi.StyleButton(_btnRefresh, ModernUi.Accent, Color.White);
            _btnRefresh.Location = new Point(Width - 180, 16);
            _btnRefresh.Click += async (s, e) => await LoadStatsAsync();
            topBar.Resize += (s, e) => _btnRefresh.Location = new Point(topBar.ClientSize.Width - _btnRefresh.Width, 16);

            topBar.Controls.Add(lblTitle);
            topBar.Controls.Add(lblSub);
            topBar.Controls.Add(_btnRefresh);

            var kpis = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = ModernUi.AppBack
            };
            for (int i = 0; i < 4; i++)
                kpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            kpis.Controls.Add(ModernUi.CreateKpiCard("Cartas unicas", ModernUi.Accent, out _lblUniqueCards, out _lblUniqueSub), 0, 0);
            kpis.Controls.Add(ModernUi.CreateKpiCard("Unidades", ModernUi.Navy, out _lblUnits, out _lblUnitsSub), 1, 0);
            kpis.Controls.Add(ModernUi.CreateKpiCard("Reservadas en decks", ModernUi.Warning, out _lblUsed, out _lblUsedSub), 2, 0);
            kpis.Controls.Add(ModernUi.CreateKpiCard("Disponibilidad", ModernUi.Success, out _lblAvailability, out _lblAvailabilitySub), 3, 0);

            var bottom = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = ModernUi.AppBack
            };
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));

            _chartByColor = CreateChart("Stock por color", SeriesChartType.Doughnut);
            _chartByType = CreateChart("Tipos mas presentes", SeriesChartType.Column);
            _lvHighlights = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable,
                BorderStyle = BorderStyle.None,
                BackColor = ModernUi.Surface,
                ForeColor = ModernUi.TextPrimary
            };
            _lvHighlights.Columns.Add("Carta", 190);
            _lvHighlights.Columns.Add("Dato", 90, HorizontalAlignment.Right);
            _lvHighlights.Columns.Add("Detalle", 150);

            bottom.Controls.Add(WrapSection("Distribucion de color", _chartByColor), 0, 0);
            bottom.Controls.Add(WrapSection("Mapa de tipos", _chartByType), 1, 0);
            bottom.Controls.Add(WrapSection("Mas informacion", _lvHighlights), 2, 0);

            root.Controls.Add(topBar, 0, 0);
            root.Controls.Add(kpis, 0, 1);
            root.Controls.Add(bottom, 0, 2);
            Controls.Add(root);

            Load += async (s, e) => await LoadStatsAsync();
        }

        private async System.Threading.Tasks.Task LoadStatsAsync()
        {
            var data = await _cardStockService.GetAllAsync(true);
            UpdateDashboard(data ?? new List<CardStock>());
        }

        private void UpdateDashboard(List<CardStock> data)
        {
            int uniqueCards = data.Count;
            int totalUnits = data.Sum(c => c.units);
            int totalUsed = data.Sum(c => c.usedCards);
            int totalAvailable = data.Sum(c => Math.Max(0, c.units - c.usedCards));
            double availabilityRatio = totalUnits == 0 ? 0 : (double)totalAvailable / totalUnits;
            double avgCost = data.Count == 0 ? 0 : data.Average(c => c.cost);
            int alterCount = data.Count(c => c.isAlter);

            _lblUniqueCards.Text = uniqueCards.ToString();
            _lblUniqueSub.Text = string.Format("{0} alternas registradas", alterCount);
            _lblUnits.Text = totalUnits.ToString();
            _lblUnitsSub.Text = string.Format("Coste medio {0:0.0}", avgCost);
            _lblUsed.Text = totalUsed.ToString();
            _lblUsedSub.Text = totalUsed == 0 ? "Sin cartas comprometidas" : "Cartas usadas en decks";
            _lblAvailability.Text = string.Format("{0:0}%", availabilityRatio * 100);
            _lblAvailabilitySub.Text = string.Format("{0} copias libres", totalAvailable);

            UpdateColorChart(data);
            UpdateTypeChart(data);
            UpdateHighlights(data);
        }

        private Chart CreateChart(string seriesName, SeriesChartType chartType)
        {
            var chart = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = ModernUi.Surface,
                Palette = ChartColorPalette.None
            };

            var area = new ChartArea("main") { BackColor = ModernUi.Surface };
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = ModernUi.Border;
            area.AxisX.LabelStyle.ForeColor = ModernUi.TextMuted;
            area.AxisY.LabelStyle.ForeColor = ModernUi.TextMuted;
            area.AxisX.LineColor = ModernUi.Border;
            area.AxisY.LineColor = ModernUi.Border;
            chart.ChartAreas.Add(area);
            chart.Legends.Add(new Legend("legend")
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center,
                BackColor = ModernUi.Surface,
                ForeColor = ModernUi.TextMuted,
                Font = new Font("Segoe UI", 8F)
            });

            var series = new Series(seriesName)
            {
                ChartArea = "main",
                ChartType = chartType,
                IsValueShownAsLabel = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold)
            };

            if (chartType == SeriesChartType.Doughnut)
            {
                series["PieLabelStyle"] = "Disabled";
                series["DoughnutRadius"] = "54";
            }
            else
            {
                series.Color = ModernUi.Accent;
            }

            chart.Series.Add(series);
            return chart;
        }

        private Control WrapSection(string title, Control content)
        {
            var host = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(8),
                Padding = new Padding(12),
                BackColor = ModernUi.Surface,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 26,
                Text = title,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = ModernUi.TextPrimary
            };

            content.Dock = DockStyle.Fill;
            host.Controls.Add(content);
            host.Controls.Add(lblTitle);
            return host;
        }

        private void UpdateColorChart(List<CardStock> data)
        {
            var series = _chartByColor.Series[0];
            series.Points.Clear();
            _chartByColor.Legends[0].Enabled = true;

            var colors = new[]
            {
                Color.FromArgb(202, 57, 57),
                Color.FromArgb(44, 112, 196),
                Color.FromArgb(32, 147, 84),
                Color.FromArgb(162, 74, 173),
                Color.FromArgb(210, 168, 36),
                Color.FromArgb(88, 96, 110)
            };

            var grouped = data.GroupBy(c => string.IsNullOrWhiteSpace(c.color) ? "Sin color" : c.color)
                .Select(g => new { Color = g.Key, Units = g.Sum(x => x.units) })
                .OrderByDescending(x => x.Units)
                .Take(6)
                .ToList();

            for (int i = 0; i < grouped.Count; i++)
            {
                var point = series.Points.Add(grouped[i].Units);
                point.LegendText = grouped[i].Color;
                point.Label = grouped[i].Units.ToString();
                point.Color = colors[i % colors.Length];
            }
        }

        private void UpdateTypeChart(List<CardStock> data)
        {
            var series = _chartByType.Series[0];
            series.Points.Clear();
            _chartByType.Legends[0].Enabled = false;

            var grouped = data.GroupBy(c => string.IsNullOrWhiteSpace(c.type) ? "Desconocido" : c.type)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(6)
                .ToList();

            foreach (var item in grouped)
            {
                var point = series.Points.Add(item.Count);
                point.AxisLabel = item.Type;
                point.Label = item.Count.ToString();
                point.Color = ModernUi.Accent;
            }
        }

        private void UpdateHighlights(List<CardStock> data)
        {
            _lvHighlights.BeginUpdate();
            _lvHighlights.Items.Clear();

            foreach (var card in data.OrderByDescending(c => c.usedCards).ThenByDescending(c => c.units).Take(5))
            {
                var available = Math.Max(0, card.units - card.usedCards);
                var item = new ListViewItem(card.cardName ?? card.cardId ?? "Carta");
                item.SubItems.Add(card.usedCards.ToString());
                item.SubItems.Add(string.Format("{0} libres", available));
                _lvHighlights.Items.Add(item);
            }

            foreach (var card in data.OrderByDescending(c => c.lastUpdatedCardDate).Take(Math.Max(0, 5 - _lvHighlights.Items.Count)))
            {
                var item = new ListViewItem(card.cardName ?? card.cardId ?? "Carta");
                item.SubItems.Add(card.units.ToString());
                item.SubItems.Add(card.lastUpdatedCardDate.ToString("dd/MM HH:mm"));
                _lvHighlights.Items.Add(item);
            }

            _lvHighlights.EndUpdate();
        }
    }
}
