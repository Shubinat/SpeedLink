using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedLinkApplication.Entities
{
    public partial class OrderEquipment
    {
        public decimal Sum => Math.Round(Equipment.Price * Amount, 2);
    }
}
