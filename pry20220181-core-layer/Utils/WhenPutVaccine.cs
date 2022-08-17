using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Utils
{
    public static class WhenPutVaccine
    {
        public static string ToString(DoseDetail doseDetail)
        {
            if (doseDetail.PutWhenNewBorn)
            {
                return "Recien Nacido (24h)";
            }
            if (doseDetail.PutWhenHasMonths >= 0)
            {
                if (doseDetail.PutWhenHasMonths == 1)
                {
                    return "1 Mes";
                }
                return $"{doseDetail.PutWhenHasMonths} Meses";
            }
            if (doseDetail.PutMonthsAfterPreviousDosis > 0)
            {
                if (doseDetail.PutMonthsAfterPreviousDosis == 1)
                {
                    return $"al mes de la {GetCardinal(doseDetail.DoseNumber - 1)} dosis";
}
                return $"a {doseDetail.PutMonthsAfterPreviousDosis} meses de la {GetCardinal(doseDetail.DoseNumber - 1)} dosis";
            }
            if (doseDetail.PutBetweenStartMonth > 0 && doseDetail.PutBetweenEndMonth > 0)
            {
                return $"entre {doseDetail.PutBetweenStartMonth} y {doseDetail.PutBetweenEndMonth} meses";
            }
            if (doseDetail.PutEveryYear > 0)
            {
                if (doseDetail.PutEveryYear == 1)
                {
                    return "Cada año";
                }
                return $"Cada {doseDetail.PutEveryYear} años";
            }

            return string.Empty;
        }

        private static string GetCardinal(int cardinal)
        {
            if(cardinal == 0)
            {
                throw new ArgumentException("cardinal must be greater than 0");
            }
            switch (cardinal)
            {
                case 1:
                    return "primera";
                case 2:
                    return "segunda";
                case 3:
                    return "tercera";
                case 4:
                    return "cuarta";
                case 5:
                    return "quinta";
                default:
                    return $"{cardinal}ª";
            }
        }
    }
}
