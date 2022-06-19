using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Services
{
    public interface IChildService
    {
        public Task<ChildDTO> GetChildByDniAsync(string DNI);
        public Task<VaccinationCardDTO> GetVaccinationCardAsync(int childId);
    }
}
