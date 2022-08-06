using pry20220181_core_layer.Modules.Master.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Repositories
{
    public interface IReminderRepository
    {
        public Task<int> CreateAsync(Reminder reminder);
        public Task CreateRangeAsync(List<Reminder> reminders);
        public Task<List<Reminder>> GetAllVaccinationAppointmentRemindersAsync(DateTime sendDate);
        public Task<Reminder> GetVaccinationAppointmentReminderByIdAsync(int reminderId);
        public Task<List<Reminder>> GetAllVaccinationCampaignRemindersAsync(DateTime sendDate);
        public Task<Reminder> GetVaccinationCampaignReminderByIdAsync(int reminderId);
        public Task<List<Reminder>> GetAllDoseReminderByParentIdAsync(int parentId);
        public Task<List<Reminder>> GetAllDoseReminderAsync(DateTime sendDate);
        public Task<int> GetReminderByDoseDetailAndChildIdAsync(int doseDetailId, int childId);
        public Task DeleteReminderAsync(int reminderId);
        public Task DeleteRemindersByDoseDetailAndChildIdAsync(int doseDetailId, int childId);
        public Task<int> DeleteAlreadySentReminders(List<int> AlreadySentReminders);
    }
}
