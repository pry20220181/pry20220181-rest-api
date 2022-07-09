﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private ILogger<ReminderRepository> _logger { get; set; }

        public ReminderRepository(PRY20220181DbContext dbContext, ILogger<ReminderRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<int> CreateAsync(Reminder reminder)
        {
            var createdReminder = await _dbContext.Reminders.AddAsync(reminder);
            await _dbContext.SaveChangesAsync();
            var createdReminderId = createdReminder.Entity.ReminderId;
            _logger.LogInformation($"Reminder {createdReminderId} created");
            return createdReminderId;
        }

        public async Task<List<Reminder>> GetAllVaccinationAppointmentRemindersAsync()
        {
            return await _dbContext.Reminders
                .Where(r => r.VaccinationAppointmentId != 0).ToListAsync();
        }

        public async Task<List<Reminder>> GetAllVaccinationCampaignRemindersAsync()
        {
            return await _dbContext.Reminders
                    .Include(r => r.Parent)
                    .Include(r => r.VaccinationCampaign)
                    .Where(r => r.VaccinationCampaignId != 0).ToListAsync();
        }

        public async Task CreateRangeAsync(List<Reminder> reminders)
        {
            await _dbContext.Reminders.AddRangeAsync(reminders);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"{reminders.Count} Reminders created");
        }

        public async Task<int> GetReminderByDoseDetailAndChildIdAsync(int doseDetailId, int childId)
        {
            var reminder = await _dbContext.Reminders
                .Where(r => r.DoseDetailId == doseDetailId && r.ChildId == childId)
                .FirstOrDefaultAsync();
            if(reminder is null)
            {
                return 0;
            }
            return reminder.ReminderId;
        }

        public async Task DeleteReminderAsync(int reminderId)
        {
            var reminderToDelete = await _dbContext.Reminders.FindAsync(reminderId);
            if(reminderToDelete is null)
            {
                _logger.LogWarning($"Removing reminder {reminderId} failed. That reminder does not exist");
                return;
            }
            _dbContext.Reminders.Remove(reminderToDelete);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Reminder {reminderId} removed");
        }

        public async Task<List<Reminder>> GetAllDoseReminderByParentIdAsync(int parentId)
        {
            return await _dbContext.Reminders.Where(r => r.ParentId == parentId).ToListAsync();
        }

        public async Task DeleteRemindersByDoseDetailAndChildIdAsync(int doseDetailId, int childId)
        {
            var remindersToDelete = _dbContext.Reminders.Where(r => r.DoseDetailId == doseDetailId && r.ChildId == childId);
            _dbContext.Reminders.RemoveRange(remindersToDelete);
            _logger.LogInformation($"{remindersToDelete.Count()} reminders removed");
            await _dbContext.SaveChangesAsync();
        }
    }
}
