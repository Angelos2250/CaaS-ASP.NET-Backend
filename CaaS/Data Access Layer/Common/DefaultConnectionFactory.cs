namespace Data_Access_Layer.Common;

using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class DefaultConnectionFactory : IConnectionFactory
{
    private readonly DbProviderFactory dbProviderFactory;

    public static IConnectionFactory FromConfiguration(string connectionStringConfigName)
    {
        (string connectionString, string providerName) =
          ConfigurationUtil.GetConnectionParameters(connectionStringConfigName);
        return new DefaultConnectionFactory(connectionString, providerName);
    }

    public DefaultConnectionFactory(string connectionString, string providerName)
    {
        ConnectionString = connectionString;
        ProviderName = providerName;

        DbUtil.RegisterAdoProviders();
        dbProviderFactory = DbProviderFactories.GetFactory(providerName);
    }

    public string ConnectionString { get; }

    public string ProviderName { get; }

    public async Task<DbConnection> CreateConnectionAsync()
    {
        var connection = dbProviderFactory.CreateConnection();
        if (connection == null)
        {
            throw new InvalidOperationException("DbProviderFactory.CreateConnection() returned null");
        }
        connection.ConnectionString = ConnectionString;
        await connection.OpenAsync();
        return connection;
    }
}
