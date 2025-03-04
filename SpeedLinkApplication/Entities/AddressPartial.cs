using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedLinkApplication.Entities
{
    public partial class Address
    {
        public string FullName => string.Join(", ", new string[] { Region, City, Street, Room });
    }
}
