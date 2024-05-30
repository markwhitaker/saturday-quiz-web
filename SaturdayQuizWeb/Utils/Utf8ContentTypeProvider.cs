namespace SaturdayQuizWeb.Utils;

public class Utf8ContentTypeProvider : FileExtensionContentTypeProvider
{
    private const string CharsetUtf8 = "; charset=utf-8";

    public Utf8ContentTypeProvider()
    {
        Mappings[".css"] = MimeType.Text.Css + CharsetUtf8;
        Mappings[".html"] = MimeType.Text.Html + CharsetUtf8;
        Mappings[".js"] = MimeType.Text.Javascript + CharsetUtf8;
        Mappings[".svg"] = MimeType.Image.SvgXml + CharsetUtf8;
    }
}
