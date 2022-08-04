using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Utils
{
    /// <summary>
    /// Used for simulated the persistence of the records
    /// </summary>
    public static class BlockchainInMemory
    {
        public static List<AdministeredDose> AdministeredDoses { get; set; } = new List<AdministeredDose>();
    }
}
