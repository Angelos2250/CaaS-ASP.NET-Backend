using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Serializable]
    public class Cart
    {
        public int idCart { get; set; }
        public int idCustomer { get; set; }

        public Cart(int idCart, int idCustomer)
        {
            this.idCart = idCart;
            this.idCustomer = idCustomer;
        }

        public override bool Equals(object? obj)
        {
            return this.Equals(obj as Cart);
        }
        public bool Equals(Cart? obj)
        {
            if (obj == null) return false;
            if (obj.idCart == this.idCart && obj.idCustomer == this.idCustomer) return true;
            return false;
        }
    }
}
