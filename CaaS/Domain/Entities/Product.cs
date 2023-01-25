using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Serializable]
    public class Product
    {
        public int idProduct { get; set; }
        public string shortDesc { get; set; }
        public string downloadLink { get; set; }
        public int price { get; set; }
        public string description { get; set; }
        public int idShop { get; set; }
        public int qty { get; set; }
        public int deletedFlag { get; set; }

        public Product(int idProduct, string shortDesc, string downloadLink, int price, string description, int idShop, int deletedFlag, int filler)
        {
            this.idProduct = idProduct;
            this.shortDesc = shortDesc;
            this.downloadLink = downloadLink;
            this.price = price;
            this.description = description;
            this.idShop = idShop;
            this.deletedFlag = deletedFlag;
        }

        public Product(int idProduct, string shortDesc, string downloadLink, int price, string description, int idShop)
        {
            this.idProduct = idProduct;
            this.shortDesc = shortDesc;
            this.downloadLink = downloadLink;
            this.price = price;
            this.description = description;
            this.idShop = idShop;
            this.deletedFlag = 0;
        }

        public Product(int idProduct_, string shortDesc_, string downloadLink_, int price_, string description_, int idShop_, int qty_)
        {
            this.idProduct = idProduct_;
            this.shortDesc = shortDesc_;
            this.downloadLink = downloadLink_;
            this.price = price_;
            this.description = description_;
            this.idShop = idShop_;
            this.qty = qty_;
        }

        public override bool Equals(object? obj)
        {
            return this.Equals(obj as Product);
        }
        public bool Equals(Product? obj)
        {
            if (obj == null) return false;
            if (obj.idProduct == this.idProduct && obj.shortDesc == this.shortDesc &&
                obj.downloadLink == this.downloadLink && obj.price == this.price &&
                obj.description == this.description && obj.idShop == this.idShop) return true;
            return false;
        }
    }
}
