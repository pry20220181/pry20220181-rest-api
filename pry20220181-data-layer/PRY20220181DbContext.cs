﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pry20220181_core_layer.Modules.Vaccination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer
{
    public class PRY20220181DbContext : IdentityDbContext<User>
    {
        public PRY20220181DbContext(DbContextOptions<PRY20220181DbContext> options) : base(options)
        {
            if (!Initialized)
            {
                SeedData();//Agrega la data a los DbSet
                Initialized = true;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region Vaccine
            modelBuilder.Entity<Vaccine>()
                .Property(v => v.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Vaccine>()
                .HasKey(v => v.Id);
            #endregion

        }

        private static bool Initialized = false;
        public DbSet<Vaccine> Vaccines { get; set; }
        private void SeedData()
        {
            if (!Vaccines.Any())
            {
                Vaccines.AddRange(new List<Vaccine>() {
                    new Vaccine() { Name = "Influenza", Description = "Vacuna para la influenza" },
                    new Vaccine() { Name = "Neumococo", Description = "Vacuna para la neumococo" }
                });
            }

            SaveChanges();
        }
    }
}
