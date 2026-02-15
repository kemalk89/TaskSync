namespace TaskSync.Common;

/**
 * Used for development only.
 */
public static class DotEnv
{
    public static void Load(Action<string> loggerFn)
    {
        var root = Directory.GetCurrentDirectory();
        var filePath = Path.Combine(root, "./../.env");
                
        if (!File.Exists(filePath))
        {
            // 2nd try because running in docker container
            filePath = Path.Combine(root, "./.env");

            if (!File.Exists(filePath))
            {
                loggerFn("No .env file found.");
                
                return;
            }
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

            if (parts[0].StartsWith("#"))
            {
                // ignore comments
                continue;
            }

            var value = parts[1];
            if (line.EndsWith("="))
            {
                value += "=";
            }
            
            Environment.SetEnvironmentVariable(parts[0], value);
        }
    }
}