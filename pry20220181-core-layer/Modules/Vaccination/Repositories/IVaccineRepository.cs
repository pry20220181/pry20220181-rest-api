using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Repositories
{
    public interface IVaccineRepository
    {
        /// <summary>
        /// Obtain the list of Vaccines
        /// </summary>
        /// <returns></returns>
        public List<Vaccine> Get();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">The id of the vaccine</param>
        /// <returns></returns>
        public Vaccine GetById(int id);
        public Vaccine Create(Vaccine vaccine);
        public Vaccine Update(Vaccine vaccine);
        public bool Delete(int id);
    }
}
