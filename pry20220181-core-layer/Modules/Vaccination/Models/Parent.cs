using pry20220181_core_layer.Modules.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Models
{
    public class Parent
    {
        public int ParentId { get; set; }
        public string UserId { get; set; }
        public int UbigeoId { get; set; }
        public string DNI { get; set; }
        public string Telephone { get; set; }

        #region Relations with another tables
        public Ubigeo Ubigeo { get; set; }
        public User User { get; set; }
        public List<ChildParent> ChildParents { get; set; }
        public List<Reminder> Reminders { get; set; }
        public List<VaccinationAppointment> VaccinationAppointments { get; set; }
        #endregion
    }
}
