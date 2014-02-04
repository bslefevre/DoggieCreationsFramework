using System.Drawing;
using System.Drawing.Drawing2D;

namespace DoggieCreationsFramework
{
    public static class DcImage
    {
        /// <summary>
        /// Convert an image to a 32x32 icon
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Icon ToIcon(this Image image)
        {
            if (image == null) return null;
            using (var square = new Bitmap(32, 32))
            {
                // allow drawing to the bitmap
                using (var g = Graphics.FromImage(square))
                {
                    int x, y, w, h; // dimensions for new image

                    if (image.Height == image.Width)
                    {
                        // just fill the square
                        x = y = 0; // set x and y to 0
                        w = h = 32; // set width and height to size
                    }
                    else
                    {
                        // work out the aspect ratio
                        var r = image.Width / (float)image.Height;

                        // set dimensions accordingly to fit inside size^2 square
                        if (r > 1)
                        { // w is bigger, so divide h by r
                            w = 32;
                            h = (int)(32 / r);
                            x = 0; y = (32 - h) / 2; // center the image
                        }
                        else
                        { // h is bigger, so multiply w by r
                            w = (int)(32 * r);
                            h = 32;
                            y = 0; x = (32 - w) / 2; // center the image
                        }
                    }

                    // make the image shrink nicely by using HighQualityBicubic mode
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.DrawImage(image, x, y, w, h); // draw image with specified dimensions
                    g.Flush(); // make sure all drawing operations complete before we get the icon

                    // following line would work directly on any image, but then
                    // it wouldn't look as nice.
                    return Icon.FromHandle(square.GetHicon());
                }
            }
        }

        public static Image ResizeImage(this Image imageToResize, Size size)
        {
            if (imageToResize == null) return null;
            var sourceWidth = imageToResize.Width;
            var sourceHeight = imageToResize.Height;

            var nPercentW = (size.Width / (float)sourceWidth);
            var nPercentH = (size.Height / (float)sourceHeight);

            var nPercent = nPercentH < nPercentW ? nPercentH : nPercentW;

            var destWidth = (int)(sourceWidth * nPercent);
            var destHeight = (int)(sourceHeight * nPercent);
            var bitmap = new Bitmap(destWidth, destHeight);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(imageToResize, 0, 0, destWidth, destHeight);
            }

            return bitmap;
        }
    }
}
