using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Master.DTOs.Input;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Services;
using pry20220181_core_layer.Modules.Master.Services.Impl;
using Swashbuckle.AspNetCore.Annotations;

namespace pry20220181_rest_api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("reminders")]
    public class ReminderController
    {
        private IReminderService _reminderService { get; set; }
        private ILogger<ReminderController> _logger { get; set; }

        public ReminderController(IReminderService reminderService, ILogger<ReminderController> logger)
        {
            _reminderService = reminderService;
            _logger = logger;
        }

        //public async Task<IResult> CreateVaccinationAppointmentReminder(ReminderCreationDTO reminderCreationDTO)
        //{
        //    var reminderId = await _reminderService.CreateReminderAsync(reminderCreationDTO);

        //}
        [HttpGet("vaccination-appointments", Name = "GetVaccinationAppointmentReminders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Get Vaccination Appointment Reminders", typeof(List<VaccinationAppointmentReminderDTO>))]
        public async Task<IResult> GetVaccinationAppointmentReminders()
        {
            try
            {
                var remindersFromDb = await _reminderService.GetAllVaccinationAppointmentRemindersAsync();
                return Results.Ok(new
                {
                    VaccinationAppointmentReminders = remindersFromDb
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }

        [HttpGet("vaccination-appointments/{reminderId}", Name = "GetVaccinationAppointmentReminderById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Get Vaccination Appointment Reminder By Id", typeof(List<VaccinationAppointmentReminderDTO>))]
        public async Task<IResult> GetVaccinationAppointmentReminderById([FromRoute] int reminderId)
        {
            try
            {
                var reminderFromDb = await _reminderService.GetVaccinationAppointmentReminderByIdAsync(reminderId);
                return Results.Ok(new
                {
                    VaccinationAppointmentReminder = reminderFromDb
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }

        [HttpGet("vaccination-campaigns", Name = "GetVaccinationCampaignsReminders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Get Vaccination Campaigns Reminders", typeof(List<VaccinationCampaignReminderDTO>))]
        public async Task<IResult> GetVaccinationCampaignsReminders()
        {
            try
            {
                var remindersFromDb = await _reminderService.GetAllVaccinationCampaignRemindersAsync();
                return Results.Ok(new
                {
                    VaccinationCampaignReminders = remindersFromDb
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }

        [HttpGet("doses", Name = "GetDosesReminders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Get Doses Reminders", typeof(List<DoseReminderDTO>))]
        public async Task<IResult> GetDosesReminders([FromQuery] int parentId)
        {
            try
            {
                var remindersFromDb = await _reminderService.GetAllDoseRemindersByParentIdAsync(parentId);
                return Results.Ok(new
                {
                    DosesReminders = remindersFromDb
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }
    }
}
