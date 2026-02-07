using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageMagick;

namespace OnePieceTCG_Manager.Utils
{
    public static class ImageUtils
    {
        /// <summary>
        /// Carga una imagen en un PictureBox. Usa LoadAsync para formatos normales
        /// y Magick.NET solo para WebP.
        /// </summary>
        public static async Task CargarImagenAsync(PictureBox pictureBox, string ruta)
        {
            if (pictureBox == null)
                throw new ArgumentNullException(nameof(pictureBox));

            if (string.IsNullOrWhiteSpace(ruta))
                throw new ArgumentException("La ruta no puede estar vacía.", nameof(ruta));

            // limpiar imagen anterior
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }

            string extension = Path.GetExtension(ruta)?.ToLowerInvariant();

            try
            {
                if (extension == ".webp")
                {
                    // Descargar y convertir solo si es .webp
                    using (var client = new WebClient())
                    {
                        byte[] data = await client.DownloadDataTaskAsync(ruta);
                        using (var ms = new MemoryStream(data))
                        using (var image = new MagickImage(ms))
                        {
                            byte[] bmpBytes = image.ToByteArray(MagickFormat.Bmp);
                            using (var bmpStream = new MemoryStream(bmpBytes))
                            using (var temp = new Bitmap(bmpStream))
                            {
                                pictureBox.Image = new Bitmap(temp);
                            }
                        }
                    }
                }
                else
                {
                    // Para el resto: el método nativo rápido
                    pictureBox.LoadAsync(ruta);
                }
            }
            catch (Exception ex)
            {
                // si algo falla, mostramos un marcador nulo
                pictureBox.Image = null;
                Console.WriteLine($"Error cargando imagen: {ex.Message}");
            }
        }

        /// <summary>
        /// Carga una imagen en un PictureBox. Usa Load para formatos normales
        /// y Magick.NET solo para WebP.
        /// </summary>
        public static void CargarImagen(PictureBox pictureBox, string ruta)
        {
            if (pictureBox == null)
                throw new ArgumentNullException(nameof(pictureBox));

            if (string.IsNullOrWhiteSpace(ruta))
                throw new ArgumentException("La ruta no puede estar vacía.", nameof(ruta));

            // Limpiar imagen anterior
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }

            string extension = Path.GetExtension(ruta)?.ToLowerInvariant();

            try
            {
                if (extension == ".webp")
                {
                    // Descargar y convertir solo si es .webp
                    using (var client = new WebClient())
                    {
                        byte[] data = client.DownloadData(ruta);  // Sincrónico
                        using (var ms = new MemoryStream(data))
                        using (var image = new MagickImage(ms))
                        {
                            byte[] bmpBytes = image.ToByteArray(MagickFormat.Bmp);
                            using (var bmpStream = new MemoryStream(bmpBytes))
                            using (var temp = new Bitmap(bmpStream))
                            {
                                pictureBox.Image = new Bitmap(temp);
                            }
                        }
                    }
                }
                else
                {
                    // Para el resto: el método nativo rápido
                    pictureBox.Load(ruta);  // Sincrónico
                }
            }
            catch (Exception ex)
            {
                // Si algo falla, mostramos un marcador nulo
                pictureBox.Image = null;
                Console.WriteLine($"Error cargando imagen: {ex.Message}");
            }
        }


        public static async Task<Image> TryLoadImageAsync(string pathOrUrl)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (System.IO.File.Exists(pathOrUrl))
                    {
                        using (var fs = new System.IO.FileStream(pathOrUrl, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                        {
                            return Image.FromStream(fs);
                        }
                    }

                    if (Uri.TryCreate(pathOrUrl, UriKind.Absolute, out var uri) &&
                        (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                    {
                        using (var wc = new System.Net.WebClient())
                        {
                            var bytes = wc.DownloadData(uri);
                            using (var ms = new System.IO.MemoryStream(bytes))
                            {
                                return Image.FromStream(ms);
                            }
                        }
                    }
                }
                catch { }

                return null;
            });
        }
    }
}
