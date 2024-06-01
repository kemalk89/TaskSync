namespace TaskSync.Common;

public static class DotEnv
{
    public static void Load(Action<string> loggerFn)
    {
        var root = Directory.GetCurrentDirectory();
        var filePath = Path.Combine(root, "./../.env");
        
        if (!File.Exists(filePath))
        {
            return;
        }

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split(
                '=',
                StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                continue;
            }

            loggerFn($"Found {parts[0]}");
            
            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }
}