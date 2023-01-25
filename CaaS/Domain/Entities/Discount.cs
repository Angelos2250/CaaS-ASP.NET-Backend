using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Serializable]
    public class Discount
    {
        public int idDiscount { get; set; }
        public string rule { get; set; }
        public int value { get; set; }
        public int type { get; set; }
        public int idShop { get; set; }

        public Discount(int idDiscount, string rule, int value, int type, int idShop)
        {
            this.idDiscount = idDiscount;
            this.rule = rule;
            this.type = type;
            this.idShop = idShop;
            this.value = value;
        }

        public Discount(int idDiscount, int value, int type, int idShop)
        {
            this.idDiscount = idDiscount;
            this.rule = "";
            this.type = type;
            this.idShop = idShop;
            this.value = value;
        }
    }
}
