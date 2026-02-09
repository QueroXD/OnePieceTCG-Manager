using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Utils
{
    public class LoadingService
    {
        private readonly Form _form;
        private Panel _overlay;
        private PictureBox _gif;
        private Label _label;

        private const int DEFAULT_MIN_MS = 800;

        public LoadingService(Form form)
        {
            _form = form;
            CreateOverlay();
        }

        private void CreateOverlay()
        {
            _overlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Visible = false
            };

            _gif = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(800, 600),
                Image = Properties.Resources.loading,
                Anchor = AnchorStyles.None
            };

            _label = new Label
            {
                ForeColor = Color.Black,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.None
            };

            _overlay.Controls.Add(_gif);
            _overlay.Controls.Add(_label);
            _form.Controls.Add(_overlay);

            _overlay.BringToFront();
            _overlay.Resize += Center;
        }

        private void Center(object sender, EventArgs e)
        {
            _gif.Location = new Point(
                (_overlay.Width - _gif.Width) / 2,
                (_overlay.Height - _gif.Height) / 2 - 10
            );

            _label.Location = new Point(
                (_overlay.Width - _label.Width) / 2,
                _gif.Bottom + 10
            );
        }

        public async Task RunAsync(
            Func<Task> action,
            string text = "Cargando...",
            int minDurationMs = DEFAULT_MIN_MS)
        {
            Show(text);

            var sw = Stopwatch.StartNew();

            try
            {
                await action();
            }
            finally
            {
                sw.Stop();

                var remaining = minDurationMs - (int)sw.ElapsedMilliseconds;
                if (remaining > 0)
                    await Task.Delay(remaining);

                Hide();
            }
        }

        private void Show(string text)
        {
            _label.Text = text;
            _gif.Image?.Dispose();
            _gif.Image = (Image)Properties.Resources.loading.Clone();
            _overlay.Visible = true;
            _overlay.BringToFront();
            _form.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
        }

        private void Hide()
        {
            _overlay.Visible = false;
            _form.Cursor = Cursors.Default;
        }
    }
}
