﻿using pry20220181_core_layer.Modules.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Repositories
{
    public interface IChildRepository
    {
        public Task<List<Child>> GetChildrenAsync();
        public Task<Child> GetByDniAsync(string DNI);
        public Task<Child> GetByIdAsync(int childId);
        public Task<Child> GetByIdWithParentsIdAsync(int childId);
    }
}
