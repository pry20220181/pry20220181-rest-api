using Microsoft.Extensions.Logging;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Services.Impl
{
    public class UbigeoService : IUbigeoService
    {
        IUbigeoRepository _ubigeoRepository {get;set;}
        ILogger<UbigeoService> _logger { get; set; }

        public UbigeoService(ILogger<UbigeoService> logger, IUbigeoRepository ubigeoRepository)
        {
            _logger = logger;
            _ubigeoRepository = ubigeoRepository;
        }

        public async Task<List<UbigeoDTO>> GetUbigeosAsync()
        {
            var ubigeosFromDb = await _ubigeoRepository.GetUbigeosAsync();
            return ubigeosFromDb.Select(u => new UbigeoDTO(){
                UbigeoId = u.UbigeoId,
                UbigeoCode = u.UbigeoCode,
                Region = u.Region,
                Province = u.Province,
                District = u.District
            }).ToList();
        }
    }
}
