using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_core_layer.Modules.Vaccination.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Services.Impl
{
    public class VaccineService : IVaccineService
    {
        IVaccineRepository _vaccineRepository { get; set; }

        public VaccineService(IVaccineRepository vaccineRepository)
        {
            _vaccineRepository = vaccineRepository;
        }

        public Vaccine GetVaccineById(int id)
        {
            return _vaccineRepository.GetById(id);
        }

        public List<Vaccine> GetVaccines()
        {
            return _vaccineRepository.Get();
        }
    }
}
