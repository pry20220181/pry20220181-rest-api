using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Master.DTOs.Input;
using pry20220181_core_layer.Modules.Master.Services;
using pry20220181_core_layer.Modules.Master.Services.Impl;

namespace pry20220181_rest_api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("reminders")]
    public class ReminderController
    {
        private IReminderService _reminderService { get; set; }

        public ReminderController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }

        //public async Task<IResult> CreateVaccinationAppointmentReminder(ReminderCreationDTO reminderCreationDTO)
        //{
        //    var reminderId = await _reminderService.CreateReminderAsync(reminderCreationDTO);

        //}
        [HttpGet("vaccination-appointments", Name = "GetVaccinationAppointmentReminders")]
        public async Task<IResult> GetVaccinationAppointmentReminders()
        {
            var remindersFromDb = await _reminderService.GetAllVaccinationAppointmentRemindersAsync();
            return Results.Ok(new
            {
                VaccinationAppointmentReminders = remindersFromDb
            });
        }

        [HttpGet("vaccination-campaigns", Name = "GetVaccinationCampaignsReminders")]
        public async Task<IResult> GetVaccinationCampaignsReminders()
        {
            var remindersFromDb = await _reminderService.GetAllVaccinationCampaignRemindersAsync();
            return Results.Ok(new
            {
                VaccinationCampaignReminders = remindersFromDb
            });
        }

        [HttpGet("doses", Name = "GetDosesReminders")]
        public async Task<IResult> GetDosesReminders([FromQuery] int parentId)
        {
            var remindersFromDb = await _reminderService.GetAllDoseRemindersByParentIdAsync(parentId);
            return Results.Ok(new
            {
                DosesReminders = remindersFromDb
            });
        }
    }
}
