using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Services
{
    public interface IVaccineService
    {
        public List<Vaccine> GetVaccines();

        /// <summary>
        /// Get a Vaccine with the id passed as a parameter
        /// </summary>
        /// <param name="id">The id of the vaccine</param>
        /// <returns></returns>
        public Vaccine GetVaccineById(int id);
    }
}
