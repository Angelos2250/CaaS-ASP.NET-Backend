using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Common
{
    public delegate T RowMapper<T>(IDataRecord row);
    //new Person(
    //                        id: (int) reader["ID"],
    //                        firstName: (string) reader["first_name"],
    //                        lastName: (string) reader["last_name"],
    //                        dateOfBirth: (DateTime) reader["date_of_birth"]
}
