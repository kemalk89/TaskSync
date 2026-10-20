namespace TaskSync.Common;

public class Utils
{
    public static List<int> ParseIntegerList(string? input)
    {
        var values = input?
            .Split(",")?
            .Select(s => int.TryParse(s, out var id) ? id : (int?)null)
            .Where(id => id.HasValue)
            .Select(id => id!.Value)
            .ToList();

        return values ?? [];
    }
    
    public static List<string> ParseList(string? input)
    {
        var values = input?
            .Split(",")?
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Select(id => id)
            .ToList();

        return values ?? [];
    }
}