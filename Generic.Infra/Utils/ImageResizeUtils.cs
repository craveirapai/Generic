using ImageResizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Infra.Utils
{
    public static class ImageResizerUtils
    {
        public static MemoryStream ResizeImage(byte[] source, int maxWidth, int maxHeight, string mode, int? quality = null)
        {
            ResizeSettings settings = new ResizeSettings();
            settings.Height = maxHeight;
            settings.Width = maxWidth;
            settings.Format = "jpg";
            settings.Scale = ScaleMode.Both;

            settings.Add("mode", mode);

            if (quality.HasValue)
                settings.Quality = quality.Value;

            MemoryStream sourceBuffer = new MemoryStream(source);
            MemoryStream targetBuffer = new MemoryStream();
            ImageBuilder.Current.Build(sourceBuffer, targetBuffer, settings);
            return targetBuffer;
        }
    }
}
