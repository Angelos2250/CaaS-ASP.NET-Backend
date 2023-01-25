using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Serializable]
    public class Order
    {
        public int idOrder { get; set; }
        public DateTime dateOfOrder { get; set; }
        public float sumOfDiscount { get; set; }
        public int idCustomer { get; set; }
        public float sumAmount { get; set; }

        public Order(int idOrder, DateTime dateOfOrder, float sumOfDiscount, int idCustomer, float sumAmount)
        {
            this.idOrder = idOrder;
            this.dateOfOrder = dateOfOrder;
            this.sumOfDiscount = sumOfDiscount;
            this.idCustomer = idCustomer;
            this.sumAmount = sumAmount;
        }

        public override bool Equals(object? obj)
        {
            return this.Equals(obj as Order);
        }
        public bool Equals(Order? obj)
        {
            if (obj == null) return false;
            if (obj.idOrder == this.idOrder && obj.dateOfOrder == this.dateOfOrder &&
                obj.sumOfDiscount == this.sumOfDiscount && obj.idCustomer == this.idCustomer
                && obj.sumAmount == this.sumAmount) return true;
            return false;
        }
    }
}
