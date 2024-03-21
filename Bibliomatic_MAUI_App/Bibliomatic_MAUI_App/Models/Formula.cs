using SkiaSharp;
using CSharpMath.SkiaSharp;

namespace Bibliomatic_MAUI_App.Models
{
    public class Formula
    {
        public string Latex { get; set; }
        public string Description { get; set; }
        public string FormulaImageSource { get; set; }
        public double ImageScale { get; set; }

        public static Stream GetFormulaStream(string latex, MathPainter mathPainter)
        {
            mathPainter.LaTeX = latex;
            var size = mathPainter.Measure(100).Size;

            size.Width = 500;
            size.Height = 500;

            if (size.Width is 0) size.Width = 1;
            if (size.Height is 0) size.Height = 1;

            using var surface = SKSurface.Create(new SKImageInfo((int)size.Width, (int)size.Height));
            mathPainter.Draw(surface.Canvas, CSharpMath.Rendering.FrontEnd.TextAlignment.Center);

            using var snapshot = surface.Snapshot();
            return snapshot.Encode(SKEncodedImageFormat.Png, 100).AsStream();
        }
    }
}
