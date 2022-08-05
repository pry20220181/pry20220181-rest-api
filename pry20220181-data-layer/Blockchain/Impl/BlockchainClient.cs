using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_data_layer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Blockchain.Impl
{
    public class BlockchainClient : IBlockchainClient
    {
        readonly HttpClient client = new HttpClient();
        private ILogger<BlockchainClient> _logger { get; set; }
        private BlockchainClientConfiguration _blockchainClientConfiguration { get; set; }

        public BlockchainClient(BlockchainClientConfiguration blockchainClientConfiguration, ILogger<BlockchainClient> logger)
        {
            _blockchainClientConfiguration = blockchainClientConfiguration;
            _logger = logger;
        }

        public async Task<string> CreateAsync(AdministeredDose administeredDose)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Api-Key", _blockchainClientConfiguration.BlockchainServiceKey);
                string json = JsonConvert.SerializeObject(new
                {
                    doseId =  administeredDose.DoseDetailId,
                    childId = administeredDose.ChildId,
                    healthCenterId = administeredDose.HealthCenterId,
                    healthPersonnelId = administeredDose.HealthPersonnelId,
                    doseDate = administeredDose.DoseDate,
                    vaccinationCampaignId = administeredDose.VaccinationCampaignId,
                    vaccinationAppointmentId = administeredDose.VaccinationAppointmentId
                });

                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                _logger.LogInformation("Llamada a API de Blockchain iniciada en " + DateTime.Now);

                var response = await httpClient.PostAsync($"{_blockchainClientConfiguration.BlockchainServiceUrl}/administered-doses", httpContent);
                var workatoResponse = await response.Content.ReadFromJsonAsync<BlockchainServiceResponse>();
                _logger.LogInformation("Llamada a API de Blockchain finalizada en " + DateTime.Now);
                _logger.LogInformation($"Respuesta del Servicio: ");
                //return workatoResponse;
            }

            BlockchainInMemory.AdministeredDoses.Add(administeredDose);

            return "nuevoGuid";
        }
        //TODO: LOS ENDPOINTS HARDCODEADOS, LUEGO PARAMETRIZADOS
        //TODO
        public async Task<List<AdministeredDose>> GetByChildIdAsync(int childId)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Api-Key", _blockchainClientConfiguration.BlockchainServiceKey);
                //string json = JsonConvert.SerializeObject(new
                //{
                //    doseId = administeredDose.DoseDetailId,
                //    childId = administeredDose.ChildId,
                //    healthCenterId = administeredDose.HealthCenterId,
                //    healthPersonnelId = administeredDose.HealthPersonnelId,
                //    doseDate = administeredDose.DoseDate,
                //    vaccinationCampaignId = administeredDose.VaccinationCampaignId,
                //    vaccinationAppointmentId = administeredDose.VaccinationAppointmentId
                //});

                //StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                _logger.LogInformation("Llamada a API de Blockchain iniciada en " + DateTime.Now);

                var response = await httpClient.GetAsync($"{_blockchainClientConfiguration.BlockchainServiceUrl}/administered-doses?childId={childId}");
                var workatoResponse = await response.Content.ReadFromJsonAsync<List<BlockchainServiceResponse>>();
                _logger.LogInformation("Llamada a API de Blockchain finalizada en " + DateTime.Now);
                _logger.LogInformation($"Respuesta del Servicio: ");
                //return workatoResponse;
            }

            return BlockchainInMemory.AdministeredDoses.Where(d => d.ChildId == childId).ToList();
        }
    }
}
