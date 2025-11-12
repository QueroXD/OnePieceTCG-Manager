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
    }
}
