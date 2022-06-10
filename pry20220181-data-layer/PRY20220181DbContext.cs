using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pry20220181_core_layer.Modules.Campaigns.Models;
using pry20220181_core_layer.Modules.Inventory.Models;
using pry20220181_core_layer.Modules.Master.Models;
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

            #region DosesDetail
            modelBuilder.Entity<DoseDetail>()
                .Property(v => v.DoseDetailId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<DoseDetail>()
                .HasKey(v => v.DoseDetailId);

            modelBuilder.Entity<DoseDetail>()
                .HasOne(v => v.VaccinationSchemeDetail)
                .WithMany(v=>v.DosesDetails)
                .HasForeignKey(v => v.VaccinationSchemeDetailId);
            #endregion

            #region AdministeredDose
            modelBuilder.Entity<AdministeredDose>()
                .Property(a => a.AdministeredDoseId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<AdministeredDose>()
                .HasKey(a => a.AdministeredDoseId);

            modelBuilder.Entity<AdministeredDose>()
                .HasOne(a => a.Child)
                .WithMany(c=>c.AdministeredDoses)
                .HasForeignKey(a => a.ChildId);

            modelBuilder.Entity<AdministeredDose>()
                .HasOne(a => a.DoseDetail)
                .WithMany()
                .HasForeignKey(a => a.DoseDetailId);

            modelBuilder.Entity<AdministeredDose>()
                .HasOne(a => a.HealthCenter)
                .WithMany()
                .HasForeignKey(a => a.HealthCenterId);

            modelBuilder.Entity<AdministeredDose>()
                .HasOne(a => a.HealthPersonnel)
                .WithMany()
                .HasForeignKey(a => a.HealthPersonnelId);

            modelBuilder.Entity<AdministeredDose>()
                .HasOne(a => a.VaccinationCampaign)
                .WithMany()
                .HasForeignKey(a => a.VaccinationCampaignId);

            modelBuilder.Entity<AdministeredDose>()
                .HasOne(a => a.VaccinationAppointment)
                .WithMany()
                .HasForeignKey(a => a.VaccinationAppointmentId);
            #endregion

            #region Vaccination Appointment
            //VaccinationAppointment
            modelBuilder.Entity<VaccinationAppointment>()
                .Property(v => v.VaccinationAppointmentId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<VaccinationAppointment>()
                .HasKey(v => v.VaccinationAppointmentId);
            #endregion

            #region VaccineForAppointment
            modelBuilder.Entity<VaccineForAppointment>()
                .Property(v => v.VaccineForAppointmentId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<VaccineForAppointment>()
                .HasKey(v => v.VaccineForAppointmentId);

            modelBuilder.Entity<VaccineForAppointment>()
                .HasOne(v => v.Vaccine)
                .WithMany()
                .HasForeignKey(v => v.VaccineId);

            modelBuilder.Entity<VaccineForAppointment>()
                .HasOne(v => v.VaccinationAppointment)
                .WithMany(v=>v.VaccinesForAppointment)
                .HasForeignKey(v => v.VaccinationAppointmentId);
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

            #region Child
            modelBuilder.Entity<Child>()
                .Property(p => p.ChildId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Child>()
                .HasKey(p => p.ChildId);
            #endregion

            #region Parent
            modelBuilder.Entity<Parent>()
                .Property(p => p.ParentId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Parent>()
                .HasKey(p => p.ParentId);

            modelBuilder.Entity<Parent>()
                .HasOne(p => p.Ubigeo)
                .WithMany()
                .HasForeignKey(p => p.UbigeoId);

            modelBuilder.Entity<Parent>()
                .HasOne(p=>p.User)
                .WithOne(u => u.Parent)
                .HasForeignKey<Parent>(p => p.UserId);
            #endregion

            #region ChildParent
            modelBuilder.Entity<ChildParent>()
                .Property(p => p.ChildParentId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<ChildParent>()
                .HasKey(p => p.ChildParentId);

            modelBuilder.Entity<ChildParent>()
                .HasOne(c => c.Child)
                .WithMany(c=>c.ChildParents)
                .HasForeignKey(c => c.ChildId);

            modelBuilder.Entity<ChildParent>()
                .HasOne(c => c.Parent)
                .WithMany(p => p.ChildParents)
                .HasForeignKey(c => c.ParentId);
            #endregion

            #region HealthPersonnel
            modelBuilder.Entity<HealthPersonnel>()
                .Property(p => p.HealthPersonnelId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<HealthPersonnel>()
                .HasKey(p => p.HealthPersonnelId);

            modelBuilder.Entity<HealthPersonnel>()
                .HasOne(p => p.User)
                .WithOne(u => u.HealthPersonnel)
                .HasForeignKey<HealthPersonnel>(p => p.UserId);
            #endregion

            #endregion

            #region Campaigns Module
            #region VaccinationCampaign
            modelBuilder.Entity<VaccinationCampaign>()
                .Property(v => v.VaccinationCampaignId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<VaccinationCampaign>()
                .HasKey(v => v.VaccinationCampaignId);
            #endregion

            #region VaccinationCampaignDetail
            modelBuilder.Entity<VaccinationCampaignDetail>()
                .Property(v => v.VaccinationCampaignDetailId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<VaccinationCampaignDetail>()
                .HasKey(v => v.VaccinationCampaignDetailId);

            modelBuilder.Entity<VaccinationCampaignDetail>()
                .HasOne(v => v.Vaccine)
                .WithMany()
                .HasForeignKey(v => v.VaccineId);

            modelBuilder.Entity<VaccinationCampaignDetail>()
                .HasOne(v => v.VaccinationCampaign)
                .WithMany(c=>c.VaccinationCampaignDetails)
                .HasForeignKey(v => v.VaccinationCampaignId);
            #endregion

            #region VaccinationCampaignLocation
            modelBuilder.Entity<VaccinationCampaignLocation>()
                .Property(v => v.VaccinationCampaignLocationId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<VaccinationCampaignLocation>()
                .HasKey(v => v.VaccinationCampaignLocationId);

            modelBuilder.Entity<VaccinationCampaignLocation>()
                .HasOne(v => v.VaccinationCampaign)
                .WithMany(c=>c.VaccinationCampaignLocations)
                .HasForeignKey(v => v.VaccinationCampaignId);

            modelBuilder.Entity<VaccinationCampaignLocation>()
                .HasOne(v => v.HealthCenter)
                .WithMany()
                .HasForeignKey(v => v.HealthCenterId);
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

            #region Master Module
            #region Reminder
            modelBuilder.Entity<Reminder>()
                .Property(r => r.ReminderId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Reminder>()
                .HasKey(r => r.ReminderId);

            modelBuilder.Entity<Reminder>()
                .HasOne(r => r.Parent)
                .WithMany(p=>p.Reminders)
                .HasForeignKey(r => r.ParentId);

            modelBuilder.Entity<Reminder>()
                .HasOne(r => r.VaccinationCampaign)
                .WithMany()
                .HasForeignKey(r => r.VaccinationCampaignId);

            modelBuilder.Entity<Reminder>()
                .HasOne(r => r.VaccinationAppointment)
                .WithMany()
                .HasForeignKey(r => r.VaccinationAppointmentId);

            modelBuilder.Entity<Reminder>()
                .HasOne(r => r.DoseDetail)
                .WithMany()
                .HasForeignKey(r => r.DoseDetailId);
            #endregion
            #endregion

        }

        private static bool Initialized = false;
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<AdministeredDose> AdministeredDoses { get; set; }
        public DbSet<DoseDetail> DosesDetails { get; set; }
        public DbSet<VaccinationSchemeDetail> VaccinationSchemeDetails { get; set; }
        public DbSet<VaccinationScheme> VaccinationSchemes { get; set; }
        public DbSet<VaccinationAppointment> VaccinationAppointments { get; set; }
        public DbSet<VaccineForAppointment> VaccinesForAppointments { get; set; }
        public DbSet<VaccinationCampaign> VaccinationCampaigns { get; set; }
        public DbSet<VaccinationCampaignDetail> VaccinationCampaignDetails { get; set; }
        public DbSet<VaccinationCampaignLocation> VaccinationCampaignLocations { get; set; }
        public DbSet<VaccineInventory> VaccineInventory { get; set; }
        public DbSet<HealthCenter> HealthCenters { get; set; }
        public DbSet<Ubigeo> Ubigeo { get; set; }
        public DbSet<HealthPersonnel> HealthPersonnel { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<ChildParent> ChildrenParents { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
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
