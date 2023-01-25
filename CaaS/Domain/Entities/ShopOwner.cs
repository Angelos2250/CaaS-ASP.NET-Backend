using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Serializable]
    public class ShopOwner
    {
        public int idShopOwner { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int idShop { get; set; }

        public ShopOwner(int idShopOwner, string firstName, string lastName, int idShop)
        {
            this.idShopOwner = idShopOwner;
            this.firstName = firstName;
            this.lastName = lastName;
            this.idShop = idShop;
        }

        public override bool Equals(object? obj)
        {
            return this.Equals(obj as ShopOwner);
        }
        public bool Equals(ShopOwner? obj)
        {
            if (obj == null) return false;
            if (obj.idShopOwner == this.idShopOwner && obj.firstName == this.firstName && obj.lastName == this.lastName && obj.idShop == this.idShop) return true;
            return false;
        }
    }
}
