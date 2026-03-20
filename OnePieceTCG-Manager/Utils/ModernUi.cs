using System.Drawing;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Utils
{
    internal static class ModernUi
    {
        public static readonly Color AppBack = Color.FromArgb(245, 247, 250);
        public static readonly Color Surface = Color.White;
        public static readonly Color SurfaceAlt = Color.FromArgb(233, 239, 245);
        public static readonly Color Border = Color.FromArgb(218, 225, 233);
        public static readonly Color TextPrimary = Color.FromArgb(26, 35, 49);
        public static readonly Color TextMuted = Color.FromArgb(103, 118, 138);
        public static readonly Color Accent = Color.FromArgb(210, 92, 35);
        public static readonly Color AccentDark = Color.FromArgb(158, 60, 18);
        public static readonly Color Success = Color.FromArgb(38, 143, 85);
        public static readonly Color Warning = Color.FromArgb(204, 134, 24);
        public static readonly Color Danger = Color.FromArgb(189, 63, 63);
        public static readonly Color Navy = Color.FromArgb(33, 47, 72);

        public static void ApplyFormTheme(Form form)
        {
            form.BackColor = AppBack;
            form.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            form.ForeColor = TextPrimary;
        }

        public static void StyleButton(Button button, Color backColor, Color foreColor)
        {
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
        }

        public static void StyleOutlineButton(Button button)
        {
            button.BackColor = Surface;
            button.ForeColor = TextPrimary;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Border;
            button.FlatAppearance.BorderSize = 1;
            button.Cursor = Cursors.Hand;
        }

        public static void StylePanelCard(Panel panel)
        {
            panel.BackColor = Surface;
            panel.BorderStyle = BorderStyle.FixedSingle;
        }

        public static void StyleInput(Control control)
        {
            control.BackColor = Surface;
            control.ForeColor = TextPrimary;
            if (control is TextBox textBox)
            {
                textBox.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control is ComboBox comboBox)
            {
                comboBox.FlatStyle = FlatStyle.Flat;
            }
        }

        public static void StyleDataGridView(DataGridView grid)
        {
            grid.BackgroundColor = Surface;
            grid.BorderStyle = BorderStyle.None;
            grid.GridColor = Border;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Navy;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Navy;
            grid.ColumnHeadersHeight = 42;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 238, 228);
            grid.DefaultCellStyle.SelectionForeColor = TextPrimary;
            grid.DefaultCellStyle.BackColor = Surface;
            grid.DefaultCellStyle.ForeColor = TextPrimary;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 251, 252);
            grid.RowHeadersVisible = false;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        public static Panel CreateKpiCard(string title, Color accentColor, out Label valueLabel, out Label subtitleLabel)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(8),
                Padding = new Padding(16),
                BackColor = Surface,
                BorderStyle = BorderStyle.FixedSingle
            };

            var bar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 4,
                BackColor = accentColor
            };

            var titleLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 22,
                Text = title,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = TextMuted
            };

            valueLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 40,
                Text = "0",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = TextPrimary
            };

            subtitleLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 32,
                Text = "",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = TextMuted
            };

            card.Controls.Add(subtitleLabel);
            card.Controls.Add(valueLabel);
            card.Controls.Add(titleLabel);
            card.Controls.Add(bar);

            return card;
        }
    }
}
