using pry20220181_core_layer.Modules.Master.DTOs.Input;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Master.Services.Impl
{
    public class ReminderService : IReminderService
    {
    private IReminderRepository _reminderRepository { get; set; }

        public ReminderService(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        public async Task<int> CreateReminderAsync(ReminderCreationDTO reminderCreationDTO)
        {
            var reminderToCreate = new Reminder()
            {
                Via = reminderCreationDTO.Via,
                DoseDetailId = reminderCreationDTO.DoseDetailId,
                ParentId = reminderCreationDTO.ParentId,
                SendDate = reminderCreationDTO.SendDate,
                VaccinationAppointmentId = reminderCreationDTO.VaccinationAppointmentId,
                VaccinationCampaignId = reminderCreationDTO.VaccinationCampaignId
            };

            return await _reminderRepository.CreateAsync(reminderToCreate);
        }

        public async Task<List<VaccinationAppointmentReminderDTO>> GetAllVaccinationAppointmentRemindersAsync()
        {
            var appointmentRemindersFromDb = await _reminderRepository.GetAllVaccinationAppointmentRemindersAsync();
            List<VaccinationAppointmentReminderDTO> remindersToReturn = new List<VaccinationAppointmentReminderDTO>();
            foreach (var reminder in appointmentRemindersFromDb)
            {
                remindersToReturn.Add(new VaccinationAppointmentReminderDTO()
                {
                    ParentId = reminder.ParentId,
                    //Parent = reminder.Parent,
                    ReminderId = reminder.ReminderId,
                    SendDate = reminder.SendDate,
                    VaccinationAppointmentId = reminder.VaccinationAppointmentId,
                    //VaccinationAppointment = reminder.VaccinationAppointment
                    Via = reminder.Via
                });
            }
            return remindersToReturn;
        }

        public async Task<List<VaccinationCampaignReminderDTO>> GetAllVaccinationCampaignRemindersAsync()
        {
            var campaignRemindersFromDb = await _reminderRepository.GetAllVaccinationCampaignRemindersAsync();
            List<VaccinationCampaignReminderDTO> remindersToReturn = new List<VaccinationCampaignReminderDTO>();
            foreach (var reminder in campaignRemindersFromDb)
            {
                remindersToReturn.Add(new VaccinationCampaignReminderDTO()
                {
                    ParentId = reminder.ParentId,
                    //Parent = reminder.Parent,
                    ReminderId = reminder.ReminderId,
                    SendDate = reminder.SendDate,
                    VaccinationCampaignId = reminder.VaccinationCampaignId,
                    Via = reminder.Via
                });
            }
            return remindersToReturn;
        }
    }
}
