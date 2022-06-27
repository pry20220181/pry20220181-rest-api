using Microsoft.EntityFrameworkCore;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Master.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer.Repositories.Master
{
    public class ReminderRepository : IReminderRepository
    {
        PRY20220181DbContext _dbContext { get; set; }

        public ReminderRepository(PRY20220181DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateAsync(Reminder reminder)
        {
            var createdReminder = await _dbContext.Reminders.AddAsync(reminder);
            await _dbContext.SaveChangesAsync();
            return createdReminder.Entity.ReminderId;
        }

        public async Task<List<Reminder>> GetAllVaccinationAppointmentRemindersAsync()
        {
            return await _dbContext.Reminders
                .Include(r=>r.Parent)
                .Include(r=>r.VaccinationAppointment)
                .Where(r => r.VaccinationAppointmentId != 0).ToListAsync();
        }
    }
}
