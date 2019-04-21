using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace HandlerTopshelfService
{
    public class MutateImageHelper
    {
        public void MutateEncodedImage(string encoded, string outputPath)
        {
            using (var image = Image.Load(Base64ToByteArray(encoded)))
            {
                MutateImage(image);
                image.Save(outputPath);
            }
        }

        private byte[] Base64ToByteArray(string data)
        {
            return Convert.FromBase64String(data);
        }

        private Image<Rgba32> MutateImage(Image<Rgba32> image)
        {
            double width, height;
            if (image.Width > image.Height)
            {
                width = 200;
                height = image.Height * (width / image.Width);
            }
            else
            {
                height = 200;
                width = image.Width * (height / image.Height);
            }

            image.Mutate(x => x
                .Resize((int)width, (int)height)
                .Grayscale());
            return image;
        }
    }
}
