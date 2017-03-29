using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZenithSociety.Models.Business;

namespace ZenithSociety.Models
{
    public class HomeViewModel
    {
        public List<EventJson> Mon { get; set; }

        public List<EventJson> Tue { get; set; }

        public List<EventJson> Wed { get; set; }

        public List<EventJson> Thu { get; set; }

        public List<EventJson> Fri { get; set; }

        public List<EventJson> Sat { get; set; }

        public List<EventJson> Sun { get; set; }
    }
}
