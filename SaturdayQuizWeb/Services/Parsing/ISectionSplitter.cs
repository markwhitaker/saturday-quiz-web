namespace SaturdayQuizWeb.Services.Parsing;

public interface ISectionSplitter
{
    IEnumerable<string> SplitSection(string section);
}