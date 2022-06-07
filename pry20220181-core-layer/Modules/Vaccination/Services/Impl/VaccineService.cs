using pry20220181_core_layer.Modules.Vaccination.DTOs.Input;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
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

        public async Task<VaccineDTO> GetVaccineByIdAsync(int id)
        {
            //TODO: Implement Validation logic
            var vaccineFromDb = await _vaccineRepository.GetByIdAsync(id);

            var vaccineToReturn = new VaccineDTO()
            {
                Id = vaccineFromDb.Id,
                Name = vaccineFromDb.Name,
                Description = vaccineFromDb.Description
            };

            return vaccineToReturn;
        }

        public async Task<List<VaccineDTO>> GetVaccinesAsync()
        {
            //TODO: Implement Validation and Pagination logic
            var vaccinesToReturn = new List<VaccineDTO>();
            var vaccinesFromDb = await _vaccineRepository.GetAsync();
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

        public async Task<int> CreateVaccineAsync(VaccineCreationDTO vaccineCreationDTO)
        {
            //TODO: Implement Validation logic
            var vaccineToCreate = new Vaccine()
            {
                Name = vaccineCreationDTO.Name,
                Description = vaccineCreationDTO.Description
            };

            var vaccineId = await _vaccineRepository.CreateAsync(vaccineToCreate);

            return vaccineId;
        }

        public async Task<VaccineDTO> UpdateVaccineAsync(int id, VaccineUpdateDTO vaccineUpdateDTO)
        {
            //TODO: Implement Validation logic
            var vaccineToUpdate = new Vaccine()
            {
                Id = id,
                Name = vaccineUpdateDTO.Name,
                Description = vaccineUpdateDTO.Description
            };

            var updatedVaccine = await _vaccineRepository.UpdateAsync(vaccineToUpdate);

            var vaccineToReturn = new VaccineDTO()
            {
                Id = updatedVaccine.Id,
                Name = updatedVaccine.Name,
                Description = updatedVaccine.Description
            };

            return vaccineToReturn;
        }

        public async Task<bool> DeleteVaccineAsync(int id)
        {
            var result = await _vaccineRepository.DeleteAsync(id);

            return result;
        }
    }
}
