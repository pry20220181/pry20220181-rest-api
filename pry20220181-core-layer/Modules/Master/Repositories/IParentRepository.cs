﻿using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Repositories
{
    public interface IParentRepository
    {
        /// <summary>
        /// Create the Parent record in the Database
        /// </summary>
        /// <param name="parent"></param>
        /// <returns>The id of the new Parent</returns>
        public Task<int> CreateAsync(Parent parent);
    }
}