using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pry20220181_core_layer.Modules.Inventory.Models;
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
            #region Vaccination Module
            #region Vaccine
            modelBuilder.Entity<Vaccine>()
                .Property(v => v.VaccineId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Vaccine>()
                .HasKey(v => v.VaccineId);
            #endregion

            #region Vaccination Scheme
            modelBuilder.Entity<VaccinationScheme>()
                .Property(v => v.VaccinationSchemeId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<VaccinationScheme>()
                .HasKey(v => v.VaccinationSchemeId);
            #endregion

            #region Vaccination Scheme Detail
            modelBuilder.Entity<VaccinationSchemeDetail>()
                .Property(v => v.VaccinationSchemeDetailId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<VaccinationSchemeDetail>()
                .HasKey(v => v.VaccinationSchemeDetailId);

            modelBuilder.Entity<VaccinationSchemeDetail>()
                .HasOne(v => v.VaccinationScheme)
                .WithMany()
                .HasForeignKey(v => v.VaccinationSchemeId);

            modelBuilder.Entity<VaccinationSchemeDetail>()
                .HasOne(v => v.Vaccine)
                .WithMany()
                .HasForeignKey(v => v.VaccineId);
            #endregion

            #region Health Center
            modelBuilder.Entity<HealthCenter>()
                .Property(h => h.HealthCenterId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<HealthCenter>()
                .HasKey(h => h.HealthCenterId);

            modelBuilder.Entity<HealthCenter>()
                .HasOne(h => h.Ubigeo)
                .WithMany()
                .HasForeignKey(h => h.UbigeoId);
            #endregion

            #region Ubigeo
            modelBuilder.Entity<Ubigeo>()
                .Property(u => u.UbigeoId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Ubigeo>()
                .HasKey(u => u.UbigeoId);
            #endregion
            #endregion

            #region Inventory Module
            #region VaccineInventory
            modelBuilder.Entity<VaccineInventory>()
                .Property(v => v.VaccineInventoryId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<VaccineInventory>()
                .HasKey(v => v.VaccineInventoryId);

            modelBuilder.Entity<VaccineInventory>()
                .HasOne(v => v.Vaccine)
                .WithMany()
                .HasForeignKey(v => v.VaccineId);

            modelBuilder.Entity<VaccineInventory>()
                .HasOne(v => v.HealthCenter)
                .WithMany()
                .HasForeignKey(v => v.HealthCenterId);
            #endregion
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
