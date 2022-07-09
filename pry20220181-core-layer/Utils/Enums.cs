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

    public static class Roles
    {
        public const string Parent = "Parent";
        public const string HealthPersonnel = "HealthPersonnel";
    }

    public static class Relationship
    {
        public const char Father = 'F';
        public const char Mother = 'M';
    }
}
