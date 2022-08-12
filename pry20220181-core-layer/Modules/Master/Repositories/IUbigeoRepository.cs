using pry20220181_core_layer.Modules.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Repositories
{
    public interface IUbigeoRepository
    {
        public Task<List<Ubigeo>> GetUbigeosAsync();
    }
}
