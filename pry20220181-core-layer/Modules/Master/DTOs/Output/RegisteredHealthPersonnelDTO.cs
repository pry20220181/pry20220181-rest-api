using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.DTOs.Output
{
    public class RegisteredHealthPersonnelDTO
    {
        public string Username { get; set; }
        public int HealthPersonnelId { get; set; }
        public string UserId { get; set; }
        public string Jwt { get; set; }
    }
}