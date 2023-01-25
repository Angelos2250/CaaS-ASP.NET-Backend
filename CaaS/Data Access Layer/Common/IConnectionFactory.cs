using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Common
{
    public interface IConnectionFactory
    {
        string ConnectionString { get; }
        string ProviderName
        { get; }
        Task<DbConnection> CreateConnectionAsync();
    }
}
