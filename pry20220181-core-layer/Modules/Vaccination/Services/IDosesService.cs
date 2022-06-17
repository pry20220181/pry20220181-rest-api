using pry20220181_core_layer.Modules.Vaccination.DTOs.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Services
{
    public interface IDosesService
    {
        public Task<List<RemainingDoseDTO>> GetRemainingDosesByChild(int childId);
    }
}
