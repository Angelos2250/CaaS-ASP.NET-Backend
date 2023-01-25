using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Mappers
{
    public static class Mappers
    {
        public static Shop MapRowToShop(IDataRecord row)
        {
            return new Shop(idShop: (int)row["idShop"],
                            name: (string)row["Name"],
                            appKey: (int)row["AppKey"]);
        }

        public static ShopOwner MapRowToShopOwner(IDataRecord row)
        {
            return new ShopOwner(idShopOwner: (int)row["idShopOwner"],
                            firstName: (string)row["firstName"],
                            lastName: (string)row["lastName"],
                            idShop: (int)row["idShop"]);
        }

        public static Product MapRowToProduct(IDataRecord row)
        {
            return new Product(idProduct: (int)row["idProduct"],
                            shortDesc: (string)row["shortDesc"],
                            downloadLink: (string)row["downloadLink"],
                            price: (int)row["price"],
                            description: (string)row["description"],
                            deletedFlag: Convert.ToInt32(row["deletedFlag"]),
                            idShop: (int)row["idShop"],
                            filler: 0);
        }

        public static Product MapRowToProduct2(IDataRecord row)
        {
            return new Product(idProduct: (int)row["idProduct"],
                            shortDesc: (string)row["shortDesc"],
                            downloadLink: (string)row["downloadLink"],
                            price: (int)row["price"],
                            description: (string)row["description"],
                            idShop: (int)row["idShop"]
                            );
        }

        public static Cart_has_Product MapRowToCart_has_Product(IDataRecord row)
        {
            return new Cart_has_Product(idProduct: (int)row["idProduct"],
                               idShop: (int)row["idShop"],
                               idCart: (int)row["idCart"],
                               idCustomer: (int)row["idCustomer"],
                               qty: (int)row["qty"]);
        }


        public static ProductWithQty MapRowToProductWithQty(IDataRecord row)
        {
            return new ProductWithQty(idProduct: (int)row["idProduct"],
                                shortDesc: (string)row["shortDesc"],
                                price: (int)row["price"],
                                description: (string)row["description"],
                                downloadLink: (string)row["downloadLink"],
                                deletedFlag: Convert.ToInt32(row["deletedFlag"]),
                                qty: (int)row["qty"],
                                idShop: (int)row["idShop"]);
        }

        public static ProductWithQty MapRowToProductWithQtyWithoutPrice(IDataRecord row)
        {
            return new ProductWithQty(idProduct: (int)row["idProduct"],
                                qty: Convert.ToInt32(row["bought"]));
        }

        public static Order MapRowToOrder(IDataRecord row)
        {
            return new Order(idOrder: (int)row["idOrder"],
                            dateOfOrder: (DateTime)row["dateOfOrder"],
                            sumOfDiscount: (float)row["sumOfDiscount"],
                            idCustomer: (int)row["idCustomer"],
                            sumAmount: (float)row["sumAmount"]);
        }

        public static Customer MapRowToCustomer(IDataRecord row)
        {
            return new Customer(idCustomer: (int)row["idCustomer"],
                                firstName: (string)row["firstName"],
                                lastName: (string)row["lastName"],
                                email: (string)row["email"]);
        }

        public static Cart MapRowToCart(IDataRecord row)
        {
            return new Cart(idCart: (int)row["idCart"],
                            idCustomer: (int)row["idCustomer"]);
        }

        public static Discount MapRowToDiscount(IDataRecord row)
        {
            return new Discount(idDiscount: (int)row["idDiscount"],
                                rule: (string)row["rule"],
                                type: (int)row["type"],
                                value: (int)row["value"],
                                idShop: (int)row["idShop"]);
        }
    }
}
