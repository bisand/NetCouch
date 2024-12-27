namespace NetCouchTests;

public static class DotEnv
{
    public static void Load(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("File not found", path);
        }

        var lines = File.ReadAllLines(path);
        foreach (var line in lines)
        {
            var parts = line.Split('=');
            if (parts.Length != 2)
            {
                throw new FormatException("Invalid format");
            }

            var key = parts[0];
            var value = parts[1];
            Environment.SetEnvironmentVariable(key, value);
        }
    }
}