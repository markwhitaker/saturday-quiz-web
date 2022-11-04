namespace SaturdayQuizWeb.Utils;

public class Utf8ContentTypeProvider : FileExtensionContentTypeProvider
{
    public Utf8ContentTypeProvider()
    {
        Mappings[".css"] = WithCharsetUtf8(MimeType.Text.Css);
        Mappings[".html"] = WithCharsetUtf8(MimeType.Text.Html);
        Mappings[".js"] = WithCharsetUtf8(MimeType.Text.Javascript);
        Mappings[".svg"] = WithCharsetUtf8(MimeType.Image.SvgXml);
    }

    private static string WithCharsetUtf8(string mimeType) => $"{mimeType}; charset=utf-8";
}
