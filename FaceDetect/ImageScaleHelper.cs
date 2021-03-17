using System.Drawing;

namespace FaceDetect
{
    static internal class ImageScaleHelper
    {

        static internal void ScaleAndDraw(Image srcImage, Image destImage)
        {
            using (var graphics = Graphics.FromImage(destImage))
            {
                var dx = destImage.Width - srcImage.Width;
                var dy = destImage.Height - srcImage.Height;
                int width, height;
                if (dx < dy)
                {
                    var proportion = (double)srcImage.Height / srcImage.Width;
                    width = srcImage.Width + dx;
                    height = (int)(width * proportion);
                }
                else
                {
                    var proportion = (double)srcImage.Width / srcImage.Height;
                    height = srcImage.Height + dy;
                    width = (int)(height * proportion);
                }
                graphics.DrawImage(srcImage,
                    (destImage.Width - width) / 2, (destImage.Height - height) / 2,
                    width, height);
            }
        }
    }
}
