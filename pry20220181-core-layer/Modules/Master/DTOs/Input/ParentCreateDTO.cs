using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.DTOs.Input
{
    public class ParentCreateDTO
    {
        public string UserId { get; set; } = "0";
        public string DNI { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Telephone { get; set; }
        public int UbigeoId { get; set; }
        public string Relationship { get; set; } //Father, Mother
        public List<ChildCreateDTO> Children { get; set; }
        public class ChildCreateDTO
        {
            public string DNI { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime Birthdate { get; set; }
            public char Gender { get; set; }
        }
    }
}
