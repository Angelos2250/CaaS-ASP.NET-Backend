using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Common
{
    public class AdoTemplate
    {
        private readonly IConnectionFactory connectionFactory;
        public AdoTemplate(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, RowMapper<T> rowMapper, params QueryParameter[] parameters)
        {
            await using DbConnection connection = await connectionFactory.CreateConnectionAsync();
            await using DbCommand command = connection.CreateCommand();

            command.CommandText = sql;
            AddParameters(command, parameters);
            var items = new List<T>();
            await using (DbDataReader reader = await command.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    items.Add(rowMapper(reader));
                }
            }
            return items;

        }
        public async Task<T?> QuerySingleAsync<T>(string sql, RowMapper<T> rowMapper, params QueryParameter[] parameters)
        {
            return (await QueryAsync(sql, rowMapper, parameters)).SingleOrDefault();
        }

        public async Task<int> ExecuteAsync(string sql, params QueryParameter[] parameters)
        {
            await using DbConnection connection = await connectionFactory.CreateConnectionAsync();
            await using DbCommand command = connection.CreateCommand();

            command.CommandText = sql;
            AddParameters(command, parameters);

            return await command.ExecuteNonQueryAsync();
        }

        public async Task<R?> ExecuteScalarAsync<R>(string sql, params QueryParameter[] parameters)
        {
            await using DbConnection connection = await connectionFactory.CreateConnectionAsync();
            await using DbCommand command = connection.CreateCommand();

            command.CommandText = sql;
            AddParameters(command, parameters);

            return (R?)await command.ExecuteScalarAsync();
        }

        private void AddParameters(DbCommand command, params QueryParameter[] parameters)
        {
            foreach (var parameter in parameters)
            {
                DbParameter dbParam = command.CreateParameter();
                dbParam.ParameterName = parameter.Name;
                dbParam.Value = parameter.Value;
                command.Parameters.Add(dbParam);
            }
        }
    }
}
