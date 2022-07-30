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
                    ReminderId = reminder.ReminderId,
                    SendDate = reminder.SendDate,
                    VaccinationAppointmentId = reminder.VaccinationAppointmentId,
                    Via = reminder.Via
                });
            }
            return remindersToReturn;
        }

        public async Task<VaccinationAppointmentReminderDTO> GetVaccinationAppointmentReminderByIdAsync(int reminderId)
        {
            var appointmentReminderFromDb = await _reminderRepository.GetVaccinationAppointmentReminderByIdAsync(reminderId);
            if(appointmentReminderFromDb is null)
            {
                return null;
            }
            return new VaccinationAppointmentReminderDTO() {
                ReminderId = appointmentReminderFromDb.ReminderId,
                VaccinationAppointmentId = appointmentReminderFromDb.VaccinationAppointmentId,
                AppointmentDateTime = appointmentReminderFromDb.VaccinationAppointment.AppointmentDateTime,
                Via = appointmentReminderFromDb.Via,
                SendDate = appointmentReminderFromDb.SendDate,
                ParentId = appointmentReminderFromDb.ParentId,
                HealthCenterId = appointmentReminderFromDb.VaccinationAppointment.HealthCenterId,
                HealthCenterName = appointmentReminderFromDb.VaccinationAppointment.HealthCenter.Name
            };
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
                    ReminderId = reminder.ReminderId,
                    SendDate = reminder.SendDate,
                    VaccinationCampaignId = reminder.VaccinationCampaignId,
                    Via = reminder.Via
                });
            }
            return remindersToReturn;
        }

        public async Task<List<DoseReminderDTO>> GetAllDoseRemindersByParentIdAsync(int parentId)
        {
            var doseRemindersFromDb = await _reminderRepository.GetAllDoseReminderByParentIdAsync(parentId);
            List<DoseReminderDTO> remindersToReturn = new List<DoseReminderDTO>();
            foreach (var reminder in doseRemindersFromDb)
            {
                remindersToReturn.Add(new DoseReminderDTO()
                {
                    ParentId = reminder.ParentId,
                    ReminderId = reminder.ReminderId,
                    SendDate = reminder.SendDate,
                    DoseDetailId = reminder.DoseDetailId,
                    ChildId = reminder.ChildId,
                    Via = reminder.Via
                });
            }
            return remindersToReturn;
        }

    }
}
