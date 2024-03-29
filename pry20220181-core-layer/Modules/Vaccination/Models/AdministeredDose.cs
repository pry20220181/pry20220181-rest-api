﻿using pry20220181_core_layer.Modules.Campaigns.Models;
using pry20220181_core_layer.Modules.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Models
{
    public class AdministeredDose
    {
        public string AdministeredDoseId { get; set; }
        public int DoseDetailId { get; set; }
        public int ChildId { get; set; }
        public int HealthCenterId { get; set; }
        public int HealthPersonnelId { get; set; }
        public DateTime DoseDate { get; set; }
        public int VaccinationCampaignId { get; set; }
        public int VaccinationAppointmentId { get; set; }
        public string Observations { get; set; }

        #region Relations with another tables
        public DoseDetail DoseDetail { get; set; }
        public Child Child { get; set; }
        public HealthCenter HealthCenter { get; set; }
        public HealthPersonnel HealthPersonnel { get; set; }
        public VaccinationCampaign VaccinationCampaign { get; set; }
        public VaccinationAppointment VaccinationAppointment { get; set; }
        #endregion
    }
}
