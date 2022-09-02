using Microsoft.AspNetCore.Mvc;
using pry20220181_core_layer.Modules.Master.DTOs.Input;
using pry20220181_core_layer.Modules.Master.DTOs.Output;
using pry20220181_core_layer.Modules.Master.Services;
using pry20220181_core_layer.Modules.Master.Services.Impl;
using Swashbuckle.AspNetCore.Annotations;
using pry20220181_rest_api.Utils;
using Microsoft.AspNetCore.Authorization;

namespace pry20220181_rest_api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("reminders")]
    public class ReminderController : ControllerBase
    {
        private IReminderService _reminderService { get; set; }
        private ILogger<ReminderController> _logger { get; set; }

        public ReminderController(IReminderService reminderService, ILogger<ReminderController> logger)
        {
            _reminderService = reminderService;
            _logger = logger;
        }

        [HttpGet("vaccination-appointments", Name = "GetVaccinationAppointmentReminders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Get Vaccination Appointment Reminders", typeof(List<VaccinationAppointmentReminderDTO>))]
        public async Task<IResult> GetVaccinationAppointmentReminders([FromQuery] string? sendDate = "None")
        {
            try
            {
                DateTime sendDateParameter = DateTime.Now;
                if (sendDate != "None")
                {
                    sendDateParameter = DateTime.Parse(sendDate);
                }
                var remindersFromDb = await _reminderService.GetAllVaccinationAppointmentRemindersAsync(sendDateParameter);
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
        [SwaggerResponse(200, "Get Vaccination Appointment Reminder By Id", typeof(VaccinationAppointmentReminderDTO))]
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
        public async Task<IResult> GetVaccinationCampaignsReminders([FromQuery] string? sendDate = "None")
        {
            try
            {
                DateTime sendDateParameter = DateTime.Now;
                if (sendDate != "None")
                {
                    sendDateParameter = DateTime.Parse(sendDate);
                }

                var remindersFromDb = await _reminderService.GetAllVaccinationCampaignRemindersAsync(sendDateParameter);
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

        [HttpGet("vaccination-campaigns/{reminderId}", Name = "GetVaccinationCampaignsReminderById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Get Vaccination Campaigns Reminder By Id", typeof(VaccinationCampaignReminderDTO))]
        public async Task<IResult> GetVaccinationCampaignsReminderById([FromRoute] int reminderId)
        {
            try
            {
                var remindersFromDb = await _reminderService.GetVaccinationCampaignReminderByIdAsync(reminderId);
                return Results.Ok(new
                {
                    VaccinationCampaignReminder = remindersFromDb
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
        public async Task<IResult> GetDosesReminders([FromQuery] int? parentId = 0, [FromQuery] string? sendDate = "None")
        {
            try
            {
                DateTime sendDateParameter = DateTime.Now;
                if (sendDate != "None")
                {
                    sendDateParameter = DateTime.Parse(sendDate);
                }

                if(parentId == 0)
                {
                    var allRemindersFromDb = await _reminderService.GetAllDoseRemindersAsync(sendDateParameter);
                    return Results.Ok(new
                    {
                        DosesReminders = allRemindersFromDb
                    });
                }
                //TODO: Si no viene parentId trae todos pero filtrando por fecha nada mas
                var remindersFromDb = await _reminderService.GetAllDoseRemindersByParentIdAsync(parentId.Value);
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

        #region Get Reminders by Parent
        [Authorize]
        [HttpGet("doses-by-parent", Name = "GetDosesRemindersByParent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Get Doses Reminders By Parent", typeof(List<DoseReminderDTO>))]
        public async Task<IResult> GetDosesRemindersByParent()
        {
            try
            {
                var user = HttpContext.User;
                var parentId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.EntityId).Value);

                //var remindersFromDb = await _reminderService.GetAllDoseRemindersByParentIdAsync(parentId.Value);
                return Results.Ok(new
                {
                    Message = "Reminder for parent with id " + parentId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }

        [Authorize]
        [HttpGet("vaccination-campaigns-by-parent", Name = "GetVaccinationCampaignRemindersByParent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Get Vaccination Campaign Reminders By Parent", typeof(List<VaccinationCampaignReminderDTO>))]
        public async Task<IResult> GetVaccinationCampaignRemindersByParent()
        {
            try
            {
                var user = HttpContext.User;
                var parentId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.EntityId).Value);

                //var remindersFromDb = await _reminderService.GetAllDoseRemindersByParentIdAsync(parentId.Value);
                return Results.Ok(new
                {
                    Message = "Reminder for parent with id " + parentId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }

        [Authorize]
        [HttpGet("vaccination-appointments-by-parent", Name = "GetVaccinationAppointmentRemindersByParent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Get Vaccination Appointment Reminders By Parent", typeof(List<VaccinationAppointmentReminderDTO>))]
        public async Task<IResult> GetVaccinationAppointmentRemindersByParent()
        {
            try
            {
                var user = HttpContext.User;
                var parentId = Convert.ToInt32(user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.EntityId).Value);

                //var remindersFromDb = await _reminderService.GetAllDoseRemindersByParentIdAsync(parentId.Value);
                return Results.Ok(new
                {
                    Message = "Reminder for parent with id " + parentId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\nStacktrace " + ex.StackTrace);
                return Results.Problem("Internal error", statusCode: 500);
            }
        }
        #endregion

        [HttpDelete(Name = "DeleteAlreadySentReminders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerResponse(200, "Delete Already Sent Reminders", typeof(int))]
        public async Task<IResult> DeleteAlreadySentReminders([FromBody] List<int> AlreadySentReminders)
        {
            Console.WriteLine("Hello", AlreadySentReminders);
            var response = await _reminderService.DeleteAlreadySentReminders(AlreadySentReminders);
            return Results.Ok(new
            {
                DeletedReminders = response
            });
        }
    }
}
