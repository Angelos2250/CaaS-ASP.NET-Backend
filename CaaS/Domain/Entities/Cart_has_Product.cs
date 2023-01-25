using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Serializable]
    public class Cart_has_Product
    {
        public int idProduct { get; set; }
        public int idShop { get; set; }
        public int idCart { get; set; }
        public int idCustomer { get; set; }
        public int qty { get; set; }

        public Cart_has_Product(int idProduct, int idShop, int idCart, int idCustomer, int qty)
        {
            this.idProduct = idProduct;
            this.idShop = idShop;
            this.idCart = idCart;
            this.idCustomer = idCustomer;
            this.qty = qty;
        }
    }
}
