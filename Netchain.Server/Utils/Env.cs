namespace Netchain.Server;

public static class Env
{
    public static IEnumerable<string> GetEnvironmentValues(string key)
    {
        var value = Environment.GetEnvironmentVariable(key);
        return string.IsNullOrEmpty(value) ? [] : value.Split(' ');
    }
}
