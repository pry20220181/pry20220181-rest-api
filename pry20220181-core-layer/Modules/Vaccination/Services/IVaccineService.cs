using pry20220181_core_layer.Modules.Vaccination.DTOs.Input;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
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
        /// <summary>
        /// Get the list of Vaccines
        /// </summary>
        /// <returns></returns>
        public Task<List<VaccineDTO>> GetVaccinesAsync();

        /// <summary>
        /// Get a Vaccine with the id passed as a parameter
        /// </summary>
        /// <param name="id">The id of the vaccine</param>
        /// <returns></returns>
        public Task<VaccineDTO> GetVaccineByIdAsync(int id);

        /// <summary>
        /// Create a Vaccine with the specified data        
        /// </summary>
        /// <param name="vaccineCreationDTO">The vaccine to create</param>
        /// <returns>The id of the new Vaccine</returns>
        public Task<int> CreateVaccineAsync(VaccineCreationDTO vaccineCreationDTO);

        /// <summary>
        /// Update an existing Vaccine with the new specified data
        /// </summary>
        /// <param name="id">The id of the Vaccine to update</param>
        /// <param name="vaccineUpdateDTO"></param>
        /// <returns>The Vaccine with its updated data</returns>
        public Task<VaccineDTO> UpdateVaccineAsync(int id, VaccineUpdateDTO vaccineUpdateDTO);

        /// <summary>
        /// Delete the specified Vaccine
        /// </summary>
        /// <param name="id">Id of the Vaccine to delete</param>
        /// <returns>True if the deleting was successful, False if an error has occurred deleting</returns>
        public Task<bool> DeleteVaccineAsync(int id);
    }
}
