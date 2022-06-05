using pry20220181_core_layer.Modules.Vaccination.DTOs;
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

        public VaccineDTO GetVaccineById(int id)
        {
            var vaccineFromDb = _vaccineRepository.GetById(id);

            var vaccineToReturn = new VaccineDTO()
            {
                Id = vaccineFromDb.Id,
                Name = vaccineFromDb.Name,
                Description = vaccineFromDb.Description
            };

            return vaccineToReturn;
        }

        public List<VaccineDTO> GetVaccines()
        {
            var vaccinesToReturn = new List<VaccineDTO>();
            var vaccinesFromDb = _vaccineRepository.Get();
            foreach (var vaccine in vaccinesFromDb)
            {
                vaccinesToReturn.Add(new VaccineDTO()
                {
                    Id = vaccine.Id,
                    Name = vaccine.Name,
                    Description = vaccine.Description
                });
            }
            return vaccinesToReturn;
        }
    }
}
