using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_data_layer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Blockchain.Impl
{
    public class BlockchainClientDummy : IBlockchainClient
    {
        public async Task<string> CreateAsync(AdministeredDose administeredDose)
        {
            administeredDose.AdministeredDoseId = Guid.NewGuid().ToString();
            BlockchainInMemory.AdministeredDoses.Add(administeredDose);

            return administeredDose.AdministeredDoseId;
        }

        public async Task<AdministeredDose> GetAdministeredDoseByIdAsync(string id)
        {
            return BlockchainInMemory.AdministeredDoses.FirstOrDefault(d=>d.AdministeredDoseId == id);
        }

        public async Task<List<AdministeredDose>> GetByChildIdAsync(int childId)
        {
            return BlockchainInMemory.AdministeredDoses.Where(d => d.ChildId == childId).ToList();
        }
    }
}
