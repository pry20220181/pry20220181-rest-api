﻿using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.DTOs.Input
{
    public class VaccineCreationDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float MinTemperature { get; set; }
        public float MaxTemperature { get; set; }
        //public List<int> VaccinationSchemes { get; set; }
        public List<VaccinationSchemeCreationDTO> VaccinationSchemes { get; set; } = new List<VaccinationSchemeCreationDTO>();
    }
}
