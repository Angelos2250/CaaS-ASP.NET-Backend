namespace Data_Access_Layer.Common;

using System.Data.Common;

public static class DbUtil
{
    public static void RegisterAdoProviders()
    {
        // Use new Implementation of MS SQL Provider
        DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", Microsoft.Data.SqlClient.SqlClientFactory.Instance);
        // DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
        DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySql.Data.MySqlClient.MySqlClientFactory.Instance);
    }
}
