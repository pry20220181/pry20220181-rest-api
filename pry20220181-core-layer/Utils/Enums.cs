using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Utils
{
    //enum DoseDetailStatus
    //{
    //    CanBePut,
    //    CanNotBePut,
    //    Overdue
    //}
    public static class ReminderVias
    {
        public const string SMS = "SMS";
        public const string Email = "Email";
    }

    public static class GetVaccinesMode
    {
        public const string WithAllInfo = "all";
        public const string OnlyIdAndName = "minimal";
    }
}
