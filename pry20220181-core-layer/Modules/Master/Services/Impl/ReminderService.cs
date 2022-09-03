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
                    ReminderId = reminder.ReminderId,
                    SendDate = reminder.SendDate,
                    Via = reminder.Via,
                    Parent = new VaccinationAppointmentReminderDTO.VAReminderParentDTO()
                    {
                        ParentId = reminder.ParentId,
                        Firstname = reminder.Parent.User.FirstName,
                        Lastname = reminder.Parent.User.LastName,
                        Email = reminder.Parent.User.Email
                    },
                    VaccinationAppointment = new VaccinationAppointmentReminderDTO.VaccinationAppointmentPayload()
                    {
                        Child = new VaccinationAppointmentReminderDTO.VAReminderChildDTO()
                        {
                            ChildId = reminder.ChildId,
                            DNI = reminder.Child.DNI,
                            Fullname = reminder.Child.Firstname + " " + reminder.Child.Lastname
                        },
                        HealthCenter = new VaccinationAppointmentReminderDTO.VAReminderHealthCenterDTO()
                        {
                            HealthCenterId = reminder.VaccinationAppointment.HealthCenterId,
                            Name = reminder.VaccinationAppointment.HealthCenter.Name,
                            Address = reminder.VaccinationAppointment.HealthCenter.Address
                        },

                        VaccinationAppointmentId = reminder.VaccinationAppointmentId,
                        AppointmentDateTime = reminder.VaccinationAppointment.AppointmentDateTime,
                        Vaccines = reminder.VaccinationAppointment.VaccinesForAppointment.Select(v => v.Vaccine.Name).ToList()
                    }
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
                Via = appointmentReminderFromDb.Via,
                SendDate = appointmentReminderFromDb.SendDate,
                Parent = new VaccinationAppointmentReminderDTO.VAReminderParentDTO()
                {
                    ParentId = appointmentReminderFromDb.ParentId,
                    Firstname = appointmentReminderFromDb.Parent.User.FirstName,
                    Lastname = appointmentReminderFromDb.Parent.User.LastName,
                    Email = appointmentReminderFromDb.Parent.User.Email
                },
                VaccinationAppointment = new VaccinationAppointmentReminderDTO.VaccinationAppointmentPayload()
                {
                    VaccinationAppointmentId = appointmentReminderFromDb.VaccinationAppointmentId,
                    AppointmentDateTime = appointmentReminderFromDb.VaccinationAppointment.AppointmentDateTime,
                    HealthCenter = new VaccinationAppointmentReminderDTO.VAReminderHealthCenterDTO()
                    {
                        HealthCenterId = appointmentReminderFromDb.VaccinationAppointment.HealthCenterId,
                        Name = appointmentReminderFromDb.VaccinationAppointment.HealthCenter.Name,
                        Address = appointmentReminderFromDb.VaccinationAppointment.HealthCenter.Address
                    },
                }
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
                    Parent = new VaccinationCampaignReminderDTO.VaccinationCampaignReminderParent
                    {
                        ParentId = reminder.ParentId,
                        Firstname = reminder.Parent.User.FirstName,
                        Lastname = reminder.Parent.User.LastName,
                        Email = reminder.Parent.User.Email
                    },
                    ReminderId = reminder.ReminderId,
                    SendDate = reminder.SendDate,
                    Via = reminder.Via,
                    VaccinationCampaign = new VaccinationCampaignReminderDTO.VaccinationCampaignPayload
                    {
                        VaccinationCampaignId = reminder.VaccinationCampaignId,

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
                    }
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
                Parent = new VaccinationCampaignReminderDTO.VaccinationCampaignReminderParent
                {
                    ParentId = campaignReminderFromDb.ParentId,
                    Email = campaignReminderFromDb.Parent.User.Email,
                    Firstname = campaignReminderFromDb.Parent.User.FirstName,
                    Lastname = campaignReminderFromDb.Parent.User.LastName
                },
                ReminderId = campaignReminderFromDb.ReminderId,
                SendDate = campaignReminderFromDb.SendDate,
                Via = campaignReminderFromDb.Via,
                VaccinationCampaign = new VaccinationCampaignReminderDTO.VaccinationCampaignPayload
                {
                    VaccinationCampaignId = campaignReminderFromDb.VaccinationCampaignId,

                    Name = campaignReminderFromDb.VaccinationCampaign.Name,
                    Description = campaignReminderFromDb.VaccinationCampaign.Description,
                    EndDateTime = campaignReminderFromDb.VaccinationCampaign.EndDateTime,
                    StartDateTime = campaignReminderFromDb.VaccinationCampaign.StartDateTime,
                    HealthCenters = campaignReminderFromDb.VaccinationCampaign.VaccinationCampaignLocations
                        .Select(l => new VaccinationCampaignReminderDTO.VaccinationCampaignReminderHealthCenter
                        {
                            HealthCenterId = l.HealthCenter.HealthCenterId,
                            Name = l.HealthCenter.Name,
                            Address = l.HealthCenter.Address
                        }).ToList(),
                    Vaccines = campaignReminderFromDb.VaccinationCampaign.VaccinationCampaignDetails.Select(d => d.Vaccine.Name).ToList()
                }
            };
        }

        public async Task<List<DoseReminderDTO>> GetAllDoseRemindersAsync(DateTime sendDate)
        {
            var doseRemindersFromDb = await _reminderRepository.GetAllDoseReminderAsync(sendDate);
            List<DoseReminderDTO> remindersToReturn = new List<DoseReminderDTO>();
            foreach (var reminder in doseRemindersFromDb)
            {
                var child = reminder.Parent.ChildParents.Where(c => c.ChildId == reminder.ChildId).FirstOrDefault().Child;
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

        public async Task<List<DoseReminderDTO>> GetAllDoseRemindersByParentIdAsync(int parentId)
        {
            var doseRemindersFromDb = await _reminderRepository.GetAllDoseRemindersByParentIdAsync(parentId);
            List<DoseReminderDTO> remindersToReturn = new List<DoseReminderDTO>();
            foreach (var reminder in doseRemindersFromDb)
            {
                var child = reminder.Parent.ChildParents.Where(c => c.ChildId == reminder.ChildId).FirstOrDefault().Child;
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

        public async Task<List<VaccinationCampaignReminderDTO>> GetAllVaccinationCampaignRemindersByParentIdAsync(int parentId)
        {
            var remindersFromDb = await _reminderRepository.GetAllVaccinationCampaignRemindersByParentIdAsync(parentId);
            List<VaccinationCampaignReminderDTO> remindersToReturn = new List<VaccinationCampaignReminderDTO>();

            foreach (var reminder in remindersFromDb)
            {
                remindersToReturn.Add(new VaccinationCampaignReminderDTO()
                {
                    ReminderId = reminder.ReminderId,
                    SendDate = reminder.SendDate,
                    Via = reminder.Via,
                    VaccinationCampaign = new VaccinationCampaignReminderDTO.VaccinationCampaignPayload()
                    {
                        VaccinationCampaignId = reminder.VaccinationCampaignId,

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
                    }
                });
            }

            return remindersToReturn;
        }

        public async Task<List<VaccinationAppointmentReminderDTO>> GetAllVaccinationAppointmentRemindersByParentIdAsync(int parentId)
        {
            var remindersFromDb = await _reminderRepository.GetAllVaccinationAppointmentRemindersByParentIdAsync(parentId);
            List<VaccinationAppointmentReminderDTO> remindersToReturn = new List<VaccinationAppointmentReminderDTO>();

            foreach (var reminder in remindersFromDb)
            {
                remindersToReturn.Add(new VaccinationAppointmentReminderDTO()
                {
                    ReminderId = reminder.ReminderId,
                    SendDate = reminder.SendDate,
                    Via = reminder.Via,
                    VaccinationAppointment = new VaccinationAppointmentReminderDTO.VaccinationAppointmentPayload()
                    {
                        Child = new VaccinationAppointmentReminderDTO.VAReminderChildDTO()
                        {
                            ChildId = reminder.ChildId,
                            DNI = reminder.Child.DNI,
                            Fullname = reminder.Child.Firstname + " " + reminder.Child.Lastname
                        },
                        HealthCenter = new VaccinationAppointmentReminderDTO.VAReminderHealthCenterDTO()
                        {
                            HealthCenterId = reminder.VaccinationAppointment.HealthCenterId,
                            Name = reminder.VaccinationAppointment.HealthCenter.Name,
                            Address = reminder.VaccinationAppointment.HealthCenter.Address
                        },

                        VaccinationAppointmentId = reminder.VaccinationAppointmentId,
                        AppointmentDateTime = reminder.VaccinationAppointment.AppointmentDateTime,
                        Vaccines = reminder.VaccinationAppointment.VaccinesForAppointment.Select(v => v.Vaccine.Name).ToList()
                    }
                });
            }

            return remindersToReturn;
        }
    }
}
