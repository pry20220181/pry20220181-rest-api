using pry20220181_core_layer.Modules.Master.DTOs.Input;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Services
{
    public interface IReminderService
    {
        public Task<int> CreateReminderAsync(ReminderCreationDTO reminderCreationDTO);
        public Task<List<VaccinationAppointmentReminderDTO>> GetAllVaccinationAppointmentRemindersAsync(DateTime sendDate);
        public Task<VaccinationAppointmentReminderDTO> GetVaccinationAppointmentReminderByIdAsync(int reminderId);
        public Task<List<VaccinationCampaignReminderDTO>> GetAllVaccinationCampaignRemindersAsync(DateTime sendDate);
        public Task<VaccinationCampaignReminderDTO> GetVaccinationCampaignReminderByIdAsync(int reminderId);
        public Task<List<DoseReminderDTO>> GetAllDoseRemindersByParentIdAsync(int parentId);
        public Task<List<DoseReminderDTO>> GetAllDoseRemindersAsync(DateTime sendDate);
        public Task<int> DeleteAlreadySentReminders(List<int> AlreadySentReminders);
    }
}
