using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Serializable]
    public class Customer
    {
        public int idCustomer { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }

        public Customer(int idCustomer, string firstName, string lastName, string email)
        {
            this.idCustomer = idCustomer;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
        }

        public override bool Equals(object? obj)
        {
            return this.Equals(obj as Customer);
        }
        public bool Equals(Customer? obj)
        {
            if (obj == null) return false;
            if (obj.idCustomer == this.idCustomer && obj.firstName == this.firstName &&
                obj.lastName == this.lastName && obj.email == this.email) return true;
            return false;
        }
    }
}
