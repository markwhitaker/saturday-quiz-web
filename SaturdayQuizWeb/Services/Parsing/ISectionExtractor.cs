using SaturdayQuizWeb.Models.Parsing;

namespace SaturdayQuizWeb.Services.Parsing;

public interface ISectionExtractor
{
    Sections ExtractSections(string wholePageHtml);
}