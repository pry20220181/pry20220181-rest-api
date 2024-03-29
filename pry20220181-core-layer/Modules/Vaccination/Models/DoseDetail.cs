﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Models
{
    public class DoseDetail
    {
        public int DoseDetailId { get; set; }
        public int VaccinationSchemeDetailId { get; set; }
        public int DoseNumber { get; set; }
        public bool PutWhenNewBorn { get; set; }
        public int PutWhenHasMonths { get; set; }
        public int PutMonthsAfterPreviousDosis { get; set; }
        public int PutBetweenStartMonth { get; set; }
        public int PutBetweenEndMonth { get; set; }
        public int PutEveryYear { get; set; }
        public bool CanBePut { get; set; } = false;
        #region Relations with another tables
        public VaccinationSchemeDetail VaccinationSchemeDetail { get; set; }
        #endregion
    }
}
