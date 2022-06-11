﻿using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_core_layer.Utils;
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
        public Task<List<Vaccine>> GetAsync(PaginationParameter paginationParameter);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">The id of the vaccine</param>
        /// <returns></returns>
        public Task<Vaccine> GetByIdAsync(int id);
        
        /// <summary>
        /// Create the Vaccine record in the Database
        /// </summary>
        /// <param name="vaccine"></param>
        /// <returns>The id of the new Vaccine</returns>
        public Task<int> CreateAsync(Vaccine vaccine);

        /// <summary>
        /// Update the specified Vaccine with the new data
        /// </summary>
        /// <param name="vaccine"></param>
        /// <returns>The Vaccine with its updated data</returns>
        public Task<Vaccine> UpdateAsync(Vaccine vaccine);

        /// <summary>
        /// Delete the specified Vaccine
        /// </summary>
        /// <param name="id">Id of the Vaccine to delete</param>
        /// <returns>True if the deleting was successful, False if an error has occurred deleting</returns>
        public Task<bool> DeleteAsync(int id);
    }
}
