using SkiaSharp;
using Svg.Skia;

class Program
{
    static void Main(string[] args)
    {
        var svgPath = Path.Combine("MarketAlly.Dialogs.Maui", "Resources", "Images");
        var svgFiles = Directory.GetFiles(svgPath, "*.svg");

        Console.WriteLine($"Found {svgFiles.Length} SVG files to convert");

        foreach (var svgFile in svgFiles)
        {
            var pngFile = Path.ChangeExtension(svgFile, ".png");
            Console.WriteLine($"Converting {Path.GetFileName(svgFile)} to {Path.GetFileName(pngFile)}...");

            try
            {
                // Load and render SVG
                using var svg = new SKSvg();
                svg.Load(svgFile);

                // Create bitmap at 48x48 (matching our BaseSize)
                var info = new SKImageInfo(48, 48, SKColorType.Rgba8888, SKAlphaType.Premul);
                using var surface = SKSurface.Create(info);
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.Transparent);

                // Scale to fit
                var bounds = svg.Picture?.CullRect ?? SKRect.Empty;
                if (bounds.Width > 0 && bounds.Height > 0)
                {
                    var scaleX = 48f / bounds.Width;
                    var scaleY = 48f / bounds.Height;
                    var scale = Math.Min(scaleX, scaleY);

                    canvas.Translate(48f / 2f, 48f / 2f);
                    canvas.Scale(scale, scale);
                    canvas.Translate(-bounds.MidX, -bounds.MidY);
                }

                // Draw the SVG
                canvas.DrawPicture(svg.Picture);

                // Save as PNG
                using var image = surface.Snapshot();
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);
                using var stream = File.OpenWrite(pngFile);
                data.SaveTo(stream);

                Console.WriteLine($"  ✓ Saved {Path.GetFileName(pngFile)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ✗ Error: {ex.Message}");
            }
        }

        Console.WriteLine("Conversion complete!");
    }
}