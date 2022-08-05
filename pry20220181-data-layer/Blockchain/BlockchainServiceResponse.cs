using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Blockchain
{
    public class BlockchainServiceResponse
    {
        public string administeredDoseId { get; set; }
        public int doseId { get; set; }
        public int childId { get; set; }
        public int healthCenterId { get; set; }
        public int healthPersonnelId { get; set; }
        public DateTime doseDate { get; set; }
        public int vaccinationCampaignId { get; set; }
        public int vaccinationAppointmentId { get; set; }
    }
}
