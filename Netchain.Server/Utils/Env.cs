namespace Netchain.Server;

public static class Env
{
    public static IEnumerable<string> GetEnvironmentValues(string key) => (Environment.GetEnvironmentVariable(key) ?? "").Split(" ");
}
