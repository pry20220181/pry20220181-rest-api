using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Utils
{
    public static class GetAgeFromBirthdate
    {
        public static int GetAge(DateTime birthdate)
        {
            int age = 0;
            age = DateTime.UtcNow.AddHours(-5).Year - birthdate.Year;
            if (DateTime.UtcNow.AddHours(-5).DayOfYear < birthdate.DayOfYear)
                age = age - 1;
            return age;
        }

        public static int GetAgeInMonths(DateTime birthdate)
        {
            int age = 0;
            age = DateTime.UtcNow.AddHours(-5).Year - birthdate.Year;
            if (DateTime.UtcNow.AddHours(-5).DayOfYear < birthdate.DayOfYear)
                age = age - 1;
            return age * 12 + DateTime.UtcNow.AddHours(-5).Month - birthdate.Month;
        }
    }
}
