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

        public async Task<List<VaccinationAppointmentReminderDTO>> GetAllVaccinationAppointmentRemindersAsync(DateTime sendDate)
        {
            var appointmentRemindersFromDb = await _reminderRepository.GetAllVaccinationAppointmentRemindersAsync(sendDate);
            List<VaccinationAppointmentReminderDTO> remindersToReturn = new List<VaccinationAppointmentReminderDTO>();
            foreach (var reminder in appointmentRemindersFromDb)
            {
                remindersToReturn.Add(new VaccinationAppointmentReminderDTO()
                {
                    ParentId = reminder.ParentId,
                    ChildId = reminder.ChildId,
                    ReminderId = reminder.ReminderId,
                    SendDate = reminder.SendDate,
                    VaccinationAppointmentId = reminder.VaccinationAppointmentId,
                    Via = reminder.Via,
                    AppointmentDateTime = reminder.VaccinationAppointment.AppointmentDateTime,
                    ChildDNI = reminder.Child.DNI,
                    ChildFullname = reminder.Child.Firstname + " " + reminder.Child.Lastname,
                    HealthCenterName = reminder.VaccinationAppointment.HealthCenter.Name,
                    Vaccines = reminder.VaccinationAppointment.VaccinesForAppointment.Select(v => v.Vaccine.Name).ToList()
                });
            }
            return remindersToReturn;
        }

        public async Task<VaccinationAppointmentReminderDTO> GetVaccinationAppointmentReminderByIdAsync(int reminderId)
        {
            var appointmentReminderFromDb = await _reminderRepository.GetVaccinationAppointmentReminderByIdAsync(reminderId);
            if (appointmentReminderFromDb is null)
            {
                return null;
            }
            return new VaccinationAppointmentReminderDTO()
            {
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

        public async Task<List<VaccinationCampaignReminderDTO>> GetAllVaccinationCampaignRemindersAsync(DateTime sendDate)
        {
            var campaignRemindersFromDb = await _reminderRepository.GetAllVaccinationCampaignRemindersAsync(sendDate);
            List<VaccinationCampaignReminderDTO> remindersToReturn = new List<VaccinationCampaignReminderDTO>();
            foreach (var reminder in campaignRemindersFromDb)
            {
                remindersToReturn.Add(new VaccinationCampaignReminderDTO()
                {
                    ParentId = reminder.ParentId,
                    ReminderId = reminder.ReminderId,
                    SendDate = reminder.SendDate,
                    VaccinationCampaignId = reminder.VaccinationCampaignId,
                    Via = reminder.Via,
                    Name = reminder.VaccinationCampaign.Name,
                    Description = reminder.VaccinationCampaign.Description,
                    EndDateTime = reminder.VaccinationCampaign.EndDateTime,
                    StartDateTime = reminder.VaccinationCampaign.StartDateTime,
                    HealthCenters = reminder.VaccinationCampaign.VaccinationCampaignLocations
                        .Select(l => new VaccinationCampaignReminderDTO.VaccinationCampaignReminderHealthCenter
                        {
                            HealthCenterId = l.HealthCenter.HealthCenterId,
                            Name = l.HealthCenter.Name,
                            Address = l.HealthCenter.Address
                        }).ToList(),
                    Vaccines = reminder.VaccinationCampaign.VaccinationCampaignDetails.Select(d => d.Vaccine.Name).ToList()
                });
            }
            return remindersToReturn;
        }
        public async Task<VaccinationCampaignReminderDTO> GetVaccinationCampaignReminderByIdAsync(int reminderId)
        {
            var campaignReminderFromDb = await _reminderRepository.GetVaccinationCampaignReminderByIdAsync(reminderId);
            if (campaignReminderFromDb is null)
            {
                return null;
            }
            return new VaccinationCampaignReminderDTO()
            {
                ReminderId = campaignReminderFromDb.ReminderId,
                VaccinationCampaignId = campaignReminderFromDb.VaccinationCampaignId,
                StartDateTime = campaignReminderFromDb.VaccinationCampaign.StartDateTime,
                EndDateTime = campaignReminderFromDb.VaccinationCampaign.EndDateTime,
                Via = campaignReminderFromDb.Via,
                SendDate = campaignReminderFromDb.SendDate,
                ParentId = campaignReminderFromDb.ParentId,
                Name = campaignReminderFromDb.VaccinationCampaign.Name,
                Description = campaignReminderFromDb.VaccinationCampaign.Description
            };
        }

        public async Task<List<DoseReminderDTO>> GetAllDoseRemindersByParentIdAsync(int parentId)
        {
            var doseRemindersFromDb = await _reminderRepository.GetAllDoseReminderByParentIdAsync(parentId);
            List<DoseReminderDTO> remindersToReturn = new List<DoseReminderDTO>();
            foreach (var reminder in doseRemindersFromDb)
            {
                var child = reminder.Parent.ChildParents.Where(c=>c.ChildId == reminder.ChildId).FirstOrDefault().Child;
                remindersToReturn.Add(new DoseReminderDTO()
                {
                    ParentId = reminder.ParentId,
                    ReminderId = reminder.ReminderId,
                    SendDate = reminder.SendDate,
                    Dose = new DoseReminderDTO.DoseDTO
                    {
                        DoseDetailId = reminder.DoseDetailId,
                        VaccineName = reminder.DoseDetail.VaccinationSchemeDetail.Vaccine.Name,
                        DoseNumber = reminder.DoseDetail.DoseNumber
                    },
                    Child = new DoseReminderDTO.DoseReminderChildDTO
                    {
                        ChildId = reminder.ChildId,
                        Name = child.Firstname + " " + child.Lastname,
                        DNI = child.DNI
                    },
                    Via = reminder.Via
                });
            }
            return remindersToReturn;
        }

        public async Task<List<DoseReminderDTO>> GetAllDoseRemindersAsync(DateTime sendDate)
        {
            var doseRemindersFromDb = await _reminderRepository.GetAllDoseReminderAsync(sendDate);
            List<DoseReminderDTO> remindersToReturn = new List<DoseReminderDTO>();
            foreach (var reminder in doseRemindersFromDb)
            {
                var child = reminder.Parent.ChildParents.Where(c=>c.ChildId == reminder.ChildId).FirstOrDefault().Child;
                remindersToReturn.Add(new DoseReminderDTO()
                {
                    ParentId = reminder.ParentId,
                    ReminderId = reminder.ReminderId,
                    SendDate = reminder.SendDate,
                    Dose = new DoseReminderDTO.DoseDTO
                    {
                        DoseDetailId = reminder.DoseDetailId,
                        VaccineName = reminder.DoseDetail.VaccinationSchemeDetail.Vaccine.Name,
                        DoseNumber = reminder.DoseDetail.DoseNumber
                    },
                    Child = new DoseReminderDTO.DoseReminderChildDTO
                    {
                        ChildId = reminder.ChildId,
                        Name = child.Firstname + " " + child.Lastname,
                        DNI = child.DNI
                    },
                    Via = reminder.Via
                });
            }
            return remindersToReturn;
        }

        public async Task<int> DeleteAlreadySentReminders(List<int> AlreadySentReminders)
        {
            return await _reminderRepository.DeleteAlreadySentReminders(AlreadySentReminders);
        }
    }
}
