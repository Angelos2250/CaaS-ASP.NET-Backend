using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Serializable]
    public class Shop
    {
        public Shop(int idShop, string name, int appKey)
        {
            this.idShop = idShop;
            this.name = name;
            this.appKey = appKey;
        }

        public Shop(int idShop, string name)
        {
            this.idShop = idShop;
            this.name = name;
        }
        public int idShop { get; set; }
        public string name { get; set; }
        public int appKey { get; set; }

        public override string ToString()
        {
            return $"Shop(idShop:{idShop}, Name:{name}, AppKey:{appKey})";
        }
        public override bool Equals(object? obj)
        {
            return this.Equals(obj as Shop);
        }
        public bool Equals(Shop? obj)
        {
            if (obj == null) return false;
            if (obj.idShop == this.idShop && obj.name == this.name && obj.appKey == this.appKey) return true;
            return false;
        }
    }
}
