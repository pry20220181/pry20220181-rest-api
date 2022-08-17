using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Utils
{
    public static class DosesAnalyzer
    {
        public static List<DoseDetail> EvaluateIfTheDosesCanBePut(Child child, List<AdministeredDose> administeredDoses, List<DoseDetail> remainingDoses)
        {
            int childAge = GetAgeFromBirthdate.GetAge(child.Birthdate);
            int childAgeInMonth = GetAgeFromBirthdate.GetAgeInMonths(child.Birthdate);
            foreach (var dose in remainingDoses)
            {
                if(dose.PutWhenNewBorn && childAge >= 0)
                {
                    dose.CanBePut = true;
                }
                if (dose.PutWhenHasMonths >= 0)
                {
                    
                    if(childAgeInMonth >= dose.PutWhenHasMonths)
                    {
                        dose.CanBePut = true;
                    }
                }
                if(dose.PutBetweenStartMonth > 0 && dose.PutBetweenEndMonth > 0)
                {
                    if (childAgeInMonth >= dose.PutBetweenStartMonth && childAgeInMonth <= dose.PutBetweenEndMonth)
                    {
                        dose.CanBePut = true;
                    }
                }
                //TODO: Implement remainig logic for AfterPreviousDosis, Put Every Year
            }
            return remainingDoses;
        }
    }
}
