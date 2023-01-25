namespace Data_Access_Layer.Common;

using Microsoft.Extensions.Configuration;

public static class ConfigurationUtil
{
    private static IConfiguration? configuration = null;

    public static IConfiguration GetConfiguration() =>
      configuration ??= new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

    public static (string connectionString, string providerName) GetConnectionParameters(string configName)
    {
        var connectionConfig = GetConfiguration().GetSection("ConnectionStrings").GetSection(configName);
        return (connectionConfig["ConnectionString"], connectionConfig["ProviderName"]);
    }
}
