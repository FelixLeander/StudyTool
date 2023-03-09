using Aspose.Slides;
using Aspose.Slides.SmartArt;
using System.Security.Cryptography;
using System.Text;

namespace StudyTool.BusinessLogic;

public static class PresentationConversion
{
    public static List<string>? ConvertToSvgSlides(string? fullFilePath)
    {
        if (!File.Exists(fullFilePath))
            return null;

        var slides = new List<string>();

        var presentation = new Presentation(fullFilePath);
        foreach (ISlide sld in presentation.Slides)
        {
            var SvgStream = new MemoryStream();

            sld.WriteAsSvg(SvgStream);
            SvgStream.Position = 0;

            string rawSvg = Encoding.UTF8.GetString(SvgStream.ToArray());
            int startIndex = rawSvg.IndexOf("<svg version");
            var processedSVG = rawSvg[startIndex..]
                .Replace("width=\"13.333333in\" height=\"7.5in\"", "height=\"100%\""); //Formatting for the site


            int startPos = processedSVG.IndexOf("<text font-family=\"Arial\" font-style=\"normal\" font-weight=\"bold\" font-stretch=\"normal\" font-size=\"18pt\" fill=\"#FFD8CF\"");
            string closingText = "</text>";
            int endPos = processedSVG.IndexOf(closingText, startPos) + closingText.Length;

            int amount = endPos - startPos;
            processedSVG = processedSVG.Remove(startPos, amount);

            slides.Add(processedSVG);

            SvgStream.Close();
        }

        return slides;
    }
}
