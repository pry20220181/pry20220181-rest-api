using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.DTOs.Output
{
    public class UbigeoDTO
    {
        public int UbigeoId { get; set; }
        public string UbigeoCode { get; set; }
        public string Region { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
    }
}
