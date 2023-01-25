using Data_Access_Layer.Ados;
using Data_Access_Layer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.MySQLDaos
{
    public class MySQLCustomerDao : AdoCustomerDao
    {
        public MySQLCustomerDao(IConnectionFactory connectionFactory): base(connectionFactory) { }

        protected override string LastInsertedIdQuery => "select LAST_INSERT_ID()";
    }
}
