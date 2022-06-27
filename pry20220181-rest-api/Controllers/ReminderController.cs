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
    }
}
