using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Serializable]
    public class ProductWithQty
    {

        public int idProduct { get; set; }
        public string shortDesc { get; set; }
        public int price { get; set; }
        public string description { get; set; }
        public string downloadLink { get; set; }
        public int deletedFlag { get; set; }
        public int qty { get; set; }
        public int idShop { get; set; }
        public ProductWithQty(int idProduct, string shortDesc, int price, string description,string downloadLink, int deletedFlag, int qty, int idShop)
        {
            this.idProduct = idProduct;
            this.shortDesc = shortDesc;
            this.price = price;
            this.description = description;
            this.downloadLink = downloadLink;
            this.deletedFlag = deletedFlag;
            this.qty = qty;
            this.idShop = idShop;
        }

        //public ProductWithQty(int idProduct, int qty, int price)
        //{
        //    this.idProduct = idProduct;
        //    this.price = price;
        //    this.qty = qty;
        //}

        public ProductWithQty(int idProduct, int qty)
        {
            this.idProduct = idProduct;
            shortDesc = "";
            description = "";
            deletedFlag = 0;
            downloadLink = "";
            this.price = -1;
            this.qty = qty;
            idShop = 0;
        }
    }
}
