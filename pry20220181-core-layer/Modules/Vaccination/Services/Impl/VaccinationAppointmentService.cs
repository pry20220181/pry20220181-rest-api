using Microsoft.Extensions.Logging;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using pry20220181_core_layer.Modules.Vaccination.DTOs.Input;
using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_core_layer.Modules.Vaccination.Repositories;
using pry20220181_core_layer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_core_layer.Modules.Vaccination.Services.Impl
{
    public class VaccinationAppointmentService : IVaccinationAppointmentService
    {
        private IVaccinationAppointmentRepository _vaccinationAppointmentRepository;
        private IReminderRepository _reminderRepository;
        private ILogger<VaccinationAppointmentService> _logger { get; set; }
        public VaccinationAppointmentService(IVaccinationAppointmentRepository vaccinationAppointmentRepository, IReminderRepository reminderRepository, ILogger<VaccinationAppointmentService> logger)
        {
            _vaccinationAppointmentRepository = vaccinationAppointmentRepository;
            _reminderRepository = reminderRepository;
            _logger = logger;
        }

        public async Task<int> CreateVaccinationAppointmentAsync(VaccinationAppointmentCreationDTO vaccinationAppointmentCreationDTO)
        {
            if(vaccinationAppointmentCreationDTO is null)
            {
                return 0;
            }

            VaccinationAppointment vaccinationAppointment = new VaccinationAppointment()
            {
                ParentId = vaccinationAppointmentCreationDTO.ParentId,
                HealthCenterId = vaccinationAppointmentCreationDTO.HealthCenterId,
                AppointmentDateTime = vaccinationAppointmentCreationDTO.AppointmentDateTime,
                VaccinesForAppointment = new List<VaccineForAppointment>()
            };

            foreach (var vaccineId in vaccinationAppointmentCreationDTO.VaccinesIds)
            {
                vaccinationAppointment.VaccinesForAppointment.Add(new VaccineForAppointment()
                {
                    VaccineId = vaccineId,
                    VaccinationAppointment = vaccinationAppointment,
                    VaccinationAppointmentId = vaccinationAppointment.VaccinationAppointmentId
                });
            }

            var createdAppointmentId = await _vaccinationAppointmentRepository.CreateAsync(vaccinationAppointment);
            _logger.LogInformation($"A appointment with ID {createdAppointmentId} was created for the parent with ID {vaccinationAppointment.ParentId}");

            if (createdAppointmentId > 0)
            {
                Reminder reminder = new Reminder()
                {
                    ParentId = vaccinationAppointment.ParentId,
                    SendDate = vaccinationAppointment.AppointmentDateTime.AddDays(-3),
                    VaccinationAppointmentId = vaccinationAppointment.VaccinationAppointmentId,
                    Via = ReminderVias.SMS
                };
                int createdReminderId = await _reminderRepository.CreateAsync(reminder);
                _logger.LogInformation($"A Reminder with ID {createdReminderId} was created for the appointment with ID {createdAppointmentId}");
            }

            return createdAppointmentId;
        }
    }
}
