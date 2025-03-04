using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedLinkApplication.Entities
{
    public partial class Order
    {
        public string OrderText
        {
            get
            {
                if(OrderStatus.Id >= 3) return OrderStatus.Name + " (" + CompleteDatetime.Value.ToString("dd.MM.yyyy HH:mm:ss") + ")";
                return OrderStatus.Name;            
            }
        }
    }
}
