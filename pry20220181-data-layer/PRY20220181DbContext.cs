using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using pry20220181_core_layer.Modules.Campaigns.Models;
using pry20220181_core_layer.Modules.Inventory.Models;
using pry20220181_core_layer.Modules.Master.Models;
using pry20220181_core_layer.Modules.Vaccination.Models;
using pry20220181_core_layer.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pry20220181_data_layer
{
    public class PRY20220181DbContext : IdentityDbContext<User>
    {
        private ILogger<PRY20220181DbContext> _logger { get; set; }
        public PRY20220181DbContext(DbContextOptions<PRY20220181DbContext> options, ILogger<PRY20220181DbContext> logger) : base(options)
        {
            _logger = logger;
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
                .WithMany(v => v.VaccinationSchemeDetails)
                .HasForeignKey(v => v.VaccinationSchemeId);

            modelBuilder.Entity<VaccinationSchemeDetail>()
                .HasOne(v => v.Vaccine)
                .WithMany(v => v.VaccinationSchemeDetails)
                .HasForeignKey(v => v.VaccineId);
            #endregion

            #region DosesDetail
            modelBuilder.Entity<DoseDetail>()
                .Property(v => v.DoseDetailId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<DoseDetail>()
                .HasKey(v => v.DoseDetailId);
            modelBuilder.Entity<DoseDetail>()
                .Ignore(v => v.CanBePut);

            modelBuilder.Entity<DoseDetail>()
                .HasOne(v => v.VaccinationSchemeDetail)
                .WithMany(v => v.DosesDetails)
                .HasForeignKey(v => v.VaccinationSchemeDetailId);
            #endregion

            // #region AdministeredDose
            // modelBuilder.Entity<AdministeredDose>()
            //     .Property(a => a.AdministeredDoseId)
            //     .ValueGeneratedOnAdd();
            // modelBuilder.Entity<AdministeredDose>()
            //     .HasKey(a => a.AdministeredDoseId);

            // modelBuilder.Entity<AdministeredDose>()
            //     .HasOne(a => a.Child)
            //     .WithMany(c => c.AdministeredDoses)
            //     .HasForeignKey(a => a.ChildId);

            // modelBuilder.Entity<AdministeredDose>()
            //     .HasOne(a => a.DoseDetail)
            //     .WithMany()
            //     .HasForeignKey(a => a.DoseDetailId);

            // modelBuilder.Entity<AdministeredDose>()
            //     .HasOne(a => a.HealthCenter)
            //     .WithMany()
            //     .HasForeignKey(a => a.HealthCenterId);

            // modelBuilder.Entity<AdministeredDose>()
            //     .HasOne(a => a.HealthPersonnel)
            //     .WithMany()
            //     .HasForeignKey(a => a.HealthPersonnelId);

            // modelBuilder.Entity<AdministeredDose>()
            //     .HasOne(a => a.VaccinationCampaign)
            //     .WithMany()
            //     .HasForeignKey(a => a.VaccinationCampaignId);

            // modelBuilder.Entity<AdministeredDose>()
            //     .HasOne(a => a.VaccinationAppointment)
            //     .WithMany()
            //     .HasForeignKey(a => a.VaccinationAppointmentId);
            // #endregion

            #region Vaccination Appointment
            //VaccinationAppointment
            modelBuilder.Entity<VaccinationAppointment>()
                .Property(v => v.VaccinationAppointmentId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<VaccinationAppointment>()
                .HasKey(v => v.VaccinationAppointmentId);

            modelBuilder.Entity<VaccinationAppointment>()
                .HasOne(v => v.Parent)
                .WithMany(p => p.VaccinationAppointments)
                .HasForeignKey(v => v.ParentId);

            modelBuilder.Entity<VaccinationAppointment>()
                .HasOne(v => v.Child)
                .WithMany()
                .HasForeignKey(v => v.ChildId);
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
                .WithMany(v => v.VaccinesForAppointment)
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
                .WithMany(u => u.HealthCenters)
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
            modelBuilder.Entity<Child>()
                .HasIndex(c => c.DNI)
                .IsUnique();
            #endregion

            #region Parent
            modelBuilder.Entity<Parent>()
                .Property(p => p.ParentId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Parent>()
                .HasKey(p => p.ParentId);
            modelBuilder.Entity<Parent>()
                .HasIndex(p => p.DNI)
                .IsUnique();

            modelBuilder.Entity<Parent>()
                .HasOne(p => p.Ubigeo)
                .WithMany()
                .HasForeignKey(p => p.UbigeoId);

            modelBuilder.Entity<Parent>()
                .HasOne(p => p.User)
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
                .WithMany(c => c.ChildParents)
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
                .HasIndex(p => p.DNI)
                .IsUnique();

            modelBuilder.Entity<HealthPersonnel>()
                .HasOne(p => p.User)
                .WithOne(u => u.HealthPersonnel)
                .HasForeignKey<HealthPersonnel>(p => p.UserId);

            modelBuilder.Entity<HealthPersonnel>()
                .HasOne(c=> c.HealthCenter)
                .WithMany(c=>c.HealthPersonnels)
                .HasForeignKey(h=>h.HealthCenterId);
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
                .WithMany(c => c.VaccinationCampaignDetails)
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
                .WithMany(c => c.VaccinationCampaignLocations)
                .HasForeignKey(v => v.VaccinationCampaignId);

            modelBuilder.Entity<VaccinationCampaignLocation>()
                .HasOne(v => v.HealthCenter)
                .WithMany(h => h.VaccinationCampaignLocations)
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
                .WithMany(h => h.VaccineInventories)
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
                .WithMany(p => p.Reminders)
                .HasForeignKey(r => r.ParentId);

            modelBuilder.Entity<Reminder>()
                .HasOne(r => r.Child)
                .WithMany()
                .HasForeignKey(r => r.ChildId);

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
        #region DbSets
        public DbSet<Vaccine> Vaccines { get; set; }
        // public DbSet<AdministeredDose> AdministeredDoses { get; set; }
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
        #endregion

        private void SeedData()
        {
            #region Ubigeo
            Ubigeo Tumbes = new Ubigeo { UbigeoCode = "230101", Region = "Tumbes", Province = "Tumbes", District = "Tumbes" };
            Ubigeo Corrales = new Ubigeo { UbigeoCode = "230102", Region = "Tumbes", Province = "Tumbes", District = "Corrales" };
            Ubigeo Ate = new Ubigeo { UbigeoCode = "140103", Region = "Lima", Province = "Lima", District = "Ate" };
            Ubigeo Breña = new Ubigeo { UbigeoCode = "140104", Region = "Lima", Province = "Lima", District = "Breña" };
            if (!Ubigeo.Any())
            {//https://www.reniec.gob.pe/Adherentes/jsp/ListaUbigeos.jsp
                List<Ubigeo> ubigeosToInsert = new List<Ubigeo>()
                {
                    Tumbes, Corrales, Ate, Breña
                };
                Ubigeo.AddRange(ubigeosToInsert);
                _logger.LogInformation("4 Ubigeos Creados");
            }
            #endregion

            #region HealthCenters
            HealthCenter TumbesCS = new HealthCenter { Name = "CS Tumbes", UbigeoId = Tumbes.UbigeoId, Address = "Av Tumbes 123" };
            HealthCenter CorralesCS = new HealthCenter { Name = " CS Corrales", UbigeoId = Corrales.UbigeoId, Address = "Av Corrales 123" };
            HealthCenter AteCS = new HealthCenter { Name = "CS Ate", UbigeoId = Ate.UbigeoId, Address = "Av Ate 123" };
            HealthCenter BreñaCS = new HealthCenter { Name = "CS Breña", UbigeoId = Breña.UbigeoId, Address = "Av Breña 123" };
            if (!HealthCenters.Any())
            {
                List<HealthCenter> healthCentersToInsert = new List<HealthCenter>()
                {
                    TumbesCS, CorralesCS, AteCS, BreñaCS
                };
                HealthCenters.AddRange(healthCentersToInsert);
                _logger.LogInformation("4 Centros de Salud Creados");
            }
            #endregion


            #region Vaccines
            #region Esquema Vacunacion Niño Menor 1 Año
            Vaccine BCG = new Vaccine { Name = "BCG", MinTemperature = 2, MaxTemperature = 8, Description = "Vacuna para la BCG" };
            Vaccine HVB = new Vaccine { Name = "HVB", MinTemperature = 2, MaxTemperature = 8, Description = "Vacuna para la HVB" };
            Vaccine VacunaPentavalente = new Vaccine { Name = "Vacuna Pentavalente", MinTemperature = 2, MaxTemperature = 8, Description = "Vacuna para la Pentavalente" };
            Vaccine VacunaAntipolioInactivadaInyectable = new Vaccine
            {
                Name = "Vacuna Antipolio Inactivada Inyectable (IPV)",
                MinTemperature = 2,
                MaxTemperature = 8,
                Description = "Vacuna Antipolio Inactivada Inyectable (IPV)"
            };
            Vaccine VacunaAntipolioOral = new Vaccine
            {
                Name = "Vacuna Antipolio oral (APO)",
                MinTemperature = 2,
                MaxTemperature = 8,
                Description = "Vacuna Antipolio oral (APO)"
            };
            Vaccine VacunaAntineumococica = new Vaccine
            {
                Name = "Vacuna Antineumococica",
                MinTemperature = 2,
                MaxTemperature = 8,
                Description = "Vacuna Antineumococica"
            };
            Vaccine VacunaContraElRotavirus = new Vaccine
            {
                Name = "Vacuna contra el Rotavirus",
                MinTemperature = 2,
                MaxTemperature = 8,
                Description = "Vacuna contra el Rotavirus"
            };
            Vaccine VacunaContraLaInfluenzaPediatrica = new Vaccine
            {
                Name = "Vacuna contra la Influenza Pediátrica",
                MinTemperature = 2,
                MaxTemperature = 8,
                Description = "Vacuna contra la Influenza Pediátrica"
            };
            Vaccine VacunaDTPediátricaHIBHepatitisB = new Vaccine
            {
                Name = "Vacuna DT pediátrica, HIB y Hepatitis B",
                MinTemperature = 2,
                MaxTemperature = 8,
                Description = "Vacuna DT pediátrica, HIB y Hepatitis B"
            };
            #endregion

            #region Esquema Vacunación Niño 1 Año
            Vaccine VacunaContraLaVaricela = new Vaccine { Name = "Vacuna Contra La Varicela", MinTemperature = 2, MaxTemperature = 8, Description = "Vacuna Contra La Varicela" };
            Vaccine VacunaSPR = new Vaccine { Name = "Vacuna SPR", MinTemperature = 2, MaxTemperature = 8, Description = "Vacuna SPR" };
            Vaccine VacunaAntiamarilica = new Vaccine { Name = "Vacuna Antiamarílica", MinTemperature = 2, MaxTemperature = 8, Description = "Vacuna Antiamarílica (AMA)" };
            Vaccine VacunaDPT = new Vaccine { Name = "Vacuna DPT", MinTemperature = 2, MaxTemperature = 8, Description = "Vacuna DPT" };
            #endregion

            #region Esquema Vacunacion Niño 2,3 y 4 años
            //Already registered: Varicela, Influenza Pediatrica, DPT, Antipolio Oral
            #endregion

            #region Esquema Vacunacion Adolescente
            Vaccine VacunaContraVirusPapilomaHumano = new Vaccine
            {
                Name = "Vacuna contra el virus del Papiloma Humano",
                MinTemperature = 2,
                MaxTemperature = 8,
                Description = "Vacuna contra el virus del Papiloma Humano"
            };
            Vaccine VacunaContraInfluenzaAdulto = new Vaccine { Name = "Vacuna contra la Influenza Adulto", MinTemperature = 2, MaxTemperature = 8, Description = "Vacuna contra la Influenza Adulto" };
            //VacunaAntiamarilica
            Vaccine VacunaContraDTAdulto = new Vaccine { Name = "Vacuna contra DT Adulto", MinTemperature = 2, MaxTemperature = 8, Description = "Vacuna contra DT Adulto" };
            Vaccine VacunaContraHepatitisB = new Vaccine { Name = "Vacuna contra la Hepatitis B (HvB)", MinTemperature = 2, MaxTemperature = 8, Description = "Vacuna contra la Hepatitis B (HvB)" };
            //VacunaSPR

            #endregion

            if (!Vaccines.Any())
            {
                var vaccinesToAdd = new List<Vaccine>() {
                    BCG, HVB, VacunaPentavalente, VacunaAntipolioInactivadaInyectable, VacunaAntipolioOral, VacunaAntineumococica,
                    VacunaContraElRotavirus, VacunaContraLaInfluenzaPediatrica, VacunaDTPediátricaHIBHepatitisB, VacunaContraLaVaricela,
                    VacunaSPR, VacunaAntiamarilica, VacunaDPT,
                    VacunaContraVirusPapilomaHumano, VacunaContraInfluenzaAdulto, VacunaContraDTAdulto, VacunaContraHepatitisB
                };
                Vaccines.AddRange(vaccinesToAdd);
                _logger.LogInformation($"{vaccinesToAdd.Count} Vacunadas Creadas");
            }
            #endregion

            #region Vaccine Inventory
            VaccineInventory vaccineInventory1 = new VaccineInventory { HealthCenterId = TumbesCS.HealthCenterId, VaccineId = BCG.VaccineId, Stock = 15 };
            VaccineInventory vaccineInventory2 = new VaccineInventory { HealthCenterId = TumbesCS.HealthCenterId, VaccineId = VacunaPentavalente.VaccineId, Stock = 18 };
            VaccineInventory vaccineInventory3 = new VaccineInventory { HealthCenterId = TumbesCS.HealthCenterId, VaccineId = VacunaAntineumococica.VaccineId, Stock = 13 };
            VaccineInventory vaccineInventory4 = new VaccineInventory { HealthCenterId = TumbesCS.HealthCenterId, VaccineId = VacunaContraLaVaricela.VaccineId, Stock = 9 };
            if (!VaccineInventory.Any())
            {
                VaccineInventory.AddRange(new List<VaccineInventory>() {
                    vaccineInventory1, vaccineInventory2, vaccineInventory3, vaccineInventory4
                });
                _logger.LogInformation("4 VaccineInventory Creados");
            }
            #endregion

            #region VaccinationSchemes
            VaccinationScheme vaccinationScheme1 = new VaccinationScheme
            {
                Name = "Esquema de Vacunacion para el Niño Menor de 1 Año",
                Description = "Esquema de Vacunacion para el Niño Menor de 1 Año",
                InitialAge = 0,
                FinalAge = 1,
            };

            VaccinationScheme vaccinationScheme2 = new VaccinationScheme
            {
                Name = "Esquema de Vacunacion para el Niño de 1 Año",
                InitialAge = 1,
                Description = "Esquema de Vacunacion para el Niño de 1 Año",
                FinalAge = 1,
            };

            VaccinationScheme vaccinationScheme3 = new VaccinationScheme
            {
                Name = "Esquema de Vacunacion para el Niño de 2, 3 y 4 Años",
                InitialAge = 2,
                Description = "Esquema de Vacunacion para el Niño de 2, 3 y 4 Años",
                FinalAge = 4,
            };

            VaccinationScheme vaccinationScheme4 = new VaccinationScheme
            {
                Name = "Esquema de Vacunacion para el Adolescente",
                InitialAge = 9,
                Description = "Esquema de Vacunacion para el Adolescente",
                FinalAge = 17,
            };

            if (!VaccinationSchemes.Any())
            {
                var schemesToAdd = new List<VaccinationScheme>() {
                    vaccinationScheme1, vaccinationScheme2, vaccinationScheme3, vaccinationScheme4
                };
                VaccinationSchemes.AddRange(schemesToAdd);
                _logger.LogInformation($"{schemesToAdd.Count} Esquemas de Vacunaciones Creados");
            }
            #endregion

            #region VaccinationSchemeDetails
            #region Esquema 1
            VaccinationSchemeDetail vaccinationSchemeDetail1A = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme1.VaccinationSchemeId,
                VaccineId = BCG.VaccineId,
                NumberOfDosesToAdminister = 1,
                PossibleEffectsPostVaccine = "Nódulo de induración, puede durar semanas o ulcerarse"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail1B = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme1.VaccinationSchemeId,
                VaccineId = HVB.VaccineId,
                NumberOfDosesToAdminister = 1,
                PossibleEffectsPostVaccine = "Local: dolor, eritema, edema e induración. Sistémico malestar general, cefalea, fatiga o irritabilidad"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail1C = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme1.VaccinationSchemeId,
                VaccineId = VacunaPentavalente.VaccineId,
                NumberOfDosesToAdminister = 3,
                PossibleEffectsPostVaccine = "Enrojecimiento, edema e induración en sitio de vacunación, llanto persistente, irritabilidad, fiebre, en raras ocasiones convulsiones"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail1D = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme1.VaccinationSchemeId,
                VaccineId = VacunaAntipolioInactivadaInyectable.VaccineId,
                NumberOfDosesToAdminister = 2,
                PossibleEffectsPostVaccine = "Dolor en el sitio de vacunación"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail1E = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme1.VaccinationSchemeId,
                VaccineId = VacunaAntipolioOral.VaccineId,
                NumberOfDosesToAdminister = 1,
                PossibleEffectsPostVaccine = "Vacuna segura; raras ocasiones eventos adversos"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail1F = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme1.VaccinationSchemeId,
                VaccineId = VacunaAntineumococica.VaccineId,
                NumberOfDosesToAdminister = 2,
                PossibleEffectsPostVaccine = "Local: dolor, eritema, edema e induración. Sistémico alza térmica, irritabilidad, reacción cutánea"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail1G = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme1.VaccinationSchemeId,
                VaccineId = VacunaContraElRotavirus.VaccineId,
                NumberOfDosesToAdminister = 2,
                PossibleEffectsPostVaccine = "Puede presentarse alza térmica, diarrea, vómitos, irritabilidad, vómitos, pérdida de apetito etc"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail1H = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme1.VaccinationSchemeId,
                VaccineId = VacunaContraLaInfluenzaPediatrica.VaccineId,
                NumberOfDosesToAdminister = 2,
                PossibleEffectsPostVaccine = "Local: dolor, eritema, edema e induración. Sistémico alza térmica, malestar general, mialgias"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail1I = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme1.VaccinationSchemeId,
                VaccineId = VacunaDTPediátricaHIBHepatitisB.VaccineId,
                NumberOfDosesToAdminister = 2,
                PossibleEffectsPostVaccine = ""
            };
            #endregion

            #region Esquema 2
            VaccinationSchemeDetail vaccinationSchemeDetail2A = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme2.VaccinationSchemeId,
                VaccineId = VacunaAntineumococica.VaccineId,
                NumberOfDosesToAdminister = 1,
                PossibleEffectsPostVaccine = "Local: dolor, eritema, edema e induración. Sistémico alza térmica, irritabilidad reacción cutánea"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail2B = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme2.VaccinationSchemeId,
                VaccineId = VacunaContraLaVaricela.VaccineId,
                NumberOfDosesToAdminister = 1,
                PossibleEffectsPostVaccine = "Dolor en la zona. A partir 5to día: irritabilidad, alza térmica, reacción cutánea, somnolencia, pérdida de apetito"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail2C = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme2.VaccinationSchemeId,
                VaccineId = VacunaSPR.VaccineId,
                NumberOfDosesToAdminister = 2,
                PossibleEffectsPostVaccine = "Alza térmica, exantema, tos, coriza, conjuntivitis (ASA) Fiebre, exantema, linfoadenopatias y artralgias (Rubeola) fiebre, hipertrofia parotídea, a partir del 7mo día post vacunación. (antiparotidico)"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail2D = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme2.VaccinationSchemeId,
                VaccineId = VacunaAntiamarilica.VaccineId,
                NumberOfDosesToAdminister = 1,
                PossibleEffectsPostVaccine = "Local: dolor, eritema, e induración. Sistémico cefalea. Mialgias, malestar Raro anafilaxia y encefalitis (mayores 60ª)"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail2E = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme2.VaccinationSchemeId,
                VaccineId = VacunaAntipolioOral.VaccineId,
                NumberOfDosesToAdminister = 1,
                PossibleEffectsPostVaccine = "Vacuna segura; raras ocasiones eventos adversos"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail2F = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme2.VaccinationSchemeId,
                VaccineId = VacunaContraLaInfluenzaPediatrica.VaccineId,
                NumberOfDosesToAdminister = 1,
                PossibleEffectsPostVaccine = "Local: dolor, eritema, edema e induración. Sistémico alza térmica, malestar general, mialgias"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail2G = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme2.VaccinationSchemeId,
                VaccineId = VacunaDPT.VaccineId,
                NumberOfDosesToAdminister = 1,
                PossibleEffectsPostVaccine = "Enrojecimiento, edema e induración en sitio de vacunación"
            };
            #endregion

            #region Esquema 3
            VaccinationSchemeDetail vaccinationSchemeDetail3A = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme3.VaccinationSchemeId,
                VaccineId = VacunaContraLaVaricela.VaccineId,
                NumberOfDosesToAdminister = 1,
                PossibleEffectsPostVaccine = "Dolor en la zona. A partir 5to día irritabilidad, alza térmica, reacción cutánea , somnolencia, pérdida de apetito"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail3B = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme3.VaccinationSchemeId,
                VaccineId = VacunaContraLaInfluenzaPediatrica.VaccineId,
                NumberOfDosesToAdminister = 1,
                PossibleEffectsPostVaccine = "Local: dolor, eritema, edema e induración. Sistémico alza térmica, malestar general, mialgias"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail3C = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme3.VaccinationSchemeId,
                VaccineId = VacunaDPT.VaccineId,
                NumberOfDosesToAdminister = 1,
                PossibleEffectsPostVaccine = "Enrojecimiento, edema e induración en sitio de vacunación, fiebre, irritabilidad, llanto persistente"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail3D = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme3.VaccinationSchemeId,
                VaccineId = VacunaAntipolioOral.VaccineId,
                NumberOfDosesToAdminister = 1,
                PossibleEffectsPostVaccine = "Vacuna segura; raras ocasiones eventos adversos"
            };
            #endregion

            #region Esquema 4
            VaccinationSchemeDetail vaccinationSchemeDetail4A = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme4.VaccinationSchemeId,
                VaccineId = VacunaContraVirusPapilomaHumano.VaccineId,
                NumberOfDosesToAdminister = 2,
                PossibleEffectsPostVaccine = "Enrojecimientos, adormecimientos de zona de inyección, alza térmica y sensación de fatiga"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail4B = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme4.VaccinationSchemeId,
                VaccineId = VacunaContraInfluenzaAdulto.VaccineId,
                NumberOfDosesToAdminister = 1,
                PossibleEffectsPostVaccine = "Local: dolor, eritema, edema e induración. Sistémico alza térmica, malestar general, mialgias"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail4C = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme4.VaccinationSchemeId,
                VaccineId = VacunaAntiamarilica.VaccineId,
                NumberOfDosesToAdminister = 1,
                PossibleEffectsPostVaccine = "Local: dolor, eritema, e induración. Sistémico cefalea. Mialgias, malestar Raro anafilaxia y encefalitis (mayores 60ª)"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail4D = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme4.VaccinationSchemeId,
                VaccineId = VacunaContraDTAdulto.VaccineId,
                NumberOfDosesToAdminister = 3,
                PossibleEffectsPostVaccine = "Local: dolor, eritema, e induración. Sistémica: malestar general"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail4E = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme4.VaccinationSchemeId,
                VaccineId = VacunaContraHepatitisB.VaccineId,
                NumberOfDosesToAdminister = 3,
                PossibleEffectsPostVaccine = "Local: dolor, eritema, edema e induración. Sistémico malestar general, cefalea, fatiga o irritabilidad"
            };

            VaccinationSchemeDetail vaccinationSchemeDetail4F = new VaccinationSchemeDetail
            {
                VaccinationSchemeId = vaccinationScheme4.VaccinationSchemeId,
                VaccineId = VacunaSPR.VaccineId,
                NumberOfDosesToAdminister = 1,
                PossibleEffectsPostVaccine = "Alza térmica, exantema, tos, coriza, conjuntivitis (ASA) Fiebre, exantema, linfoadenopatias y artralgias (Rubeola) fiebre, hipertrofia parotídea, entre el día 7mo día post vacunación. (antiparotidico)"
            };
            #endregion

            if (!VaccinationSchemeDetails.Any())
            {
                var schemeDetailsToAdd = new List<VaccinationSchemeDetail>() {
                    vaccinationSchemeDetail1A, vaccinationSchemeDetail1B, vaccinationSchemeDetail1C, vaccinationSchemeDetail1D, vaccinationSchemeDetail1E, vaccinationSchemeDetail1F, vaccinationSchemeDetail1G, vaccinationSchemeDetail1H, vaccinationSchemeDetail1I,
                    vaccinationSchemeDetail2A, vaccinationSchemeDetail2B, vaccinationSchemeDetail2C, vaccinationSchemeDetail2D, vaccinationSchemeDetail2E, vaccinationSchemeDetail2F, vaccinationSchemeDetail2G,
                    vaccinationSchemeDetail3A, vaccinationSchemeDetail3B, vaccinationSchemeDetail3C, vaccinationSchemeDetail3D,
                    vaccinationSchemeDetail4A, vaccinationSchemeDetail4B, vaccinationSchemeDetail4C, vaccinationSchemeDetail4D, vaccinationSchemeDetail4E, vaccinationSchemeDetail4F
                };
                VaccinationSchemeDetails.AddRange(schemeDetailsToAdd);
                _logger.LogInformation($"{schemeDetailsToAdd.Count} Detalles de Esquemas de Vacunación Creados");
            }
            #endregion

            #region DosesDetails
            //13/08 Me quede registrando por aqui
            #region Esquema 1
            DoseDetail dosis1VacunaBCG = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail1A.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenNewBorn = true,
            };

            DoseDetail dosis1VacunaHVB = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail1B.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenNewBorn = true,
            };

            DoseDetail dosis1VacunaPentavalente = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail1C.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 2,
            };

            DoseDetail dosis2VacunaPentavalente = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail1C.VaccinationSchemeDetailId,
                DoseNumber = 2,
                PutWhenHasMonths = 4,
            };

            DoseDetail dosis3VacunaPentavalente = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail1C.VaccinationSchemeDetailId,
                DoseNumber = 3,
                PutWhenHasMonths = 6,
            };

            DoseDetail dosis1VacunaIPV = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail1D.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 2
            };

            DoseDetail dosis2VacunaIPV = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail1D.VaccinationSchemeDetailId,
                DoseNumber = 2,
                PutWhenHasMonths = 4
            };

            DoseDetail dosis1VacunaAPO = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail1E.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 6
            };

            DoseDetail dosis1VacunaAntineumococica = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail1F.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 2
            };

            DoseDetail dosis2VacunaAntineumococica = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail1F.VaccinationSchemeDetailId,
                DoseNumber = 2,
                PutWhenHasMonths = 4
            };

            DoseDetail dosis1VacunaRotavirus = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail1G.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 2
            };

            DoseDetail dosis2VacunaRotavirus = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail1G.VaccinationSchemeDetailId,
                DoseNumber = 2,
                PutWhenHasMonths = 4
            };

            DoseDetail dosis1VacunaInfluenzaPediatrica = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail1H.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 6
            };

            DoseDetail dosis2VacunaInfluenzaPediatrica = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail1H.VaccinationSchemeDetailId,
                DoseNumber = 2,
                PutWhenHasMonths = 7
            };

            DoseDetail dosis1VacunaDTPediatricaHIBHepatitis = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail1I.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 0
            };
            DoseDetail dosis2VacunaDTPediatricaHIBHepatitis = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail1I.VaccinationSchemeDetailId,
                DoseNumber = 2,
                PutWhenHasMonths = 0
            };
            #endregion

            #region Esquema 2
            DoseDetail dosis1S2VacunaAntineumococica = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail2A.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 12,
            };

            DoseDetail dosis1VacunaContraVaricela = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail2B.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 12,
            };

            DoseDetail dosis1S2VacunaSPR = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail2C.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 12
            };
            DoseDetail dosis2S2VacunaSPR = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail2C.VaccinationSchemeDetailId,
                DoseNumber = 2,
                PutWhenHasMonths = 18
            };

            DoseDetail dosis1S2VacunaAMA = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail2D.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 15
            };

            DoseDetail dosis1S2VacunaOPA = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail2E.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 18
            };

            DoseDetail dosis1S2VacunaInfluenzaPediatrica = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail2F.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 12
            };

            DoseDetail dosis1S2VacunaDPT = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail2G.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 18
            };
            #endregion

            #region Esquema 3
            DoseDetail dosis1S3VacunaVaricela = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail3A.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 24,
            };

            DoseDetail dosis1S3VacunaInfluenzaPediatrica = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail3B.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutBetweenStartMonth = 24,
                PutBetweenEndMonth = 48
            };

            DoseDetail dosis1S3VacunaDPT = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail3C.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 48
            };

            DoseDetail dosis1S3VacunaAPO = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail3D.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 48
            };
            #endregion

            #region Esquema 4
            DoseDetail dosis1S4VacunaPapiloma = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail4A.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutBetweenStartMonth = 108,
                PutBetweenEndMonth = 156
            };

            DoseDetail dosis2S4VacunaPapiloma = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail4A.VaccinationSchemeDetailId,
                DoseNumber = 2,
                PutMonthsAfterPreviousDosis = 6,
            };

            DoseDetail dosis1S4VacunaInfluenzaAdulto = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail4B.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutEveryYear = 1,
            };

            DoseDetail dosis1S4VacunaAMA = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail4C.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 0
            };

            DoseDetail dosis1S4VacunaDTAdulto = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail4D.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 0
            };

            DoseDetail dosis2S4VacunaDTAdulto = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail4D.VaccinationSchemeDetailId,
                DoseNumber = 2,
                PutMonthsAfterPreviousDosis = 2
            };

            DoseDetail dosis3S4VacunaDTAdulto = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail4D.VaccinationSchemeDetailId,
                DoseNumber = 3,
                PutMonthsAfterPreviousDosis = 4
            };

            DoseDetail dosis1S4VacunaHvB = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail4E.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutWhenHasMonths = 0
            };

            DoseDetail dosis2S4VacunaHvB = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail4E.VaccinationSchemeDetailId,
                DoseNumber = 2,
                PutMonthsAfterPreviousDosis = 1
            };

            DoseDetail dosis3S4VacunaHvB = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail4E.VaccinationSchemeDetailId,
                DoseNumber = 3,
                PutMonthsAfterPreviousDosis = 1
            };

            DoseDetail dosis3S4VacunaSPR = new DoseDetail()
            {
                VaccinationSchemeDetailId = vaccinationSchemeDetail4F.VaccinationSchemeDetailId,
                DoseNumber = 1,
                PutBetweenStartMonth = 144,
                PutBetweenEndMonth = 204
            };
            #endregion

            if (!DosesDetails.Any())
            {
                var dosesDetailToAdd = new List<DoseDetail>()
                {
                    dosis1VacunaBCG, dosis1VacunaHVB, dosis1VacunaPentavalente, dosis2VacunaPentavalente, dosis3VacunaPentavalente, dosis1VacunaIPV, dosis2VacunaIPV,
                    dosis1VacunaAPO, dosis1VacunaAntineumococica, dosis2VacunaAntineumococica, dosis1VacunaRotavirus, dosis2VacunaRotavirus, dosis1VacunaInfluenzaPediatrica,
                    dosis2VacunaInfluenzaPediatrica, dosis1VacunaDTPediatricaHIBHepatitis, dosis2VacunaDTPediatricaHIBHepatitis,

                    dosis1S2VacunaAntineumococica, dosis1VacunaContraVaricela, dosis1S2VacunaSPR, dosis2S2VacunaSPR, dosis1S2VacunaAMA, dosis1S2VacunaOPA,dosis1S2VacunaInfluenzaPediatrica, dosis1S2VacunaDPT,

                    dosis1S3VacunaVaricela, dosis1S3VacunaInfluenzaPediatrica, dosis1S3VacunaDPT, dosis1S3VacunaAPO,

                    dosis1S4VacunaPapiloma, dosis2S4VacunaPapiloma, dosis1S4VacunaInfluenzaAdulto, dosis1S4VacunaAMA, dosis1S4VacunaDTAdulto, dosis2S4VacunaDTAdulto,
                    dosis3S4VacunaDTAdulto, dosis1S4VacunaHvB, dosis2S4VacunaHvB, dosis3S4VacunaHvB, dosis3S4VacunaSPR
                };
                DosesDetails.AddRange(dosesDetailToAdd);
                _logger.LogInformation($"{dosesDetailToAdd.Count} Doses Detail Creados");
            }
            #endregion



            #region Childs
            Child child1 = new Child()
            {
                DNI = "12345678",
                Firstname = "Arthur",
                Lastname = "Nole",
                Birthdate = new DateTime(2022, 01, 01),
                Gender = 'M'
            };

            if (!Children.Any())
            {
                Children.AddRange(new List<Child>()
                {
                    child1
                });
                _logger.LogInformation("1 Niño Creado");
            }
            #endregion


            #region Parents
            User userParent = new User()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Arthur",
                LastName = "Valladares",
                Email = "arthur1610@live.com"
            };
            base.Users.Add(userParent);
            Parent parent1 = new Parent()
            {
                DNI = "12345678",
                Telephone = "953265685",
                UbigeoId = 1,
                UserId = userParent.Id
            };

            if (!Parents.Any())
            {
                Parents.AddRange(new List<Parent>()
                {
                    parent1
                });
                _logger.LogInformation("1 Padre Creado");
            }
            #endregion

            #region ChildParents
            ChildParent childParent1 = new ChildParent()
            {
                ChildId = child1.ChildId,
                ParentId = 1,
                Relationship = 'P'
            };
            if (!ChildrenParents.Any())
            {
                ChildrenParents.AddRange(new List<ChildParent>()
                {
                    childParent1
                });
                _logger.LogInformation("1 ChildParent Creado");
            }
            #endregion

            #region Health Personnel
            User userHP = new User()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Javier",
                LastName = "Valladares"
            };
            base.Users.Add(userHP);
            HealthPersonnel healthPersonnel1 = new HealthPersonnel()
            {
                UserId = userHP.Id,
                DNI = "71224453"
            };

            if (!HealthPersonnel.Any())
            {
                HealthPersonnel.AddRange(new List<HealthPersonnel>()
                {
                    healthPersonnel1
                });
                _logger.LogInformation("1 Personal de Salud Creado");
            }
            #endregion

            #region VaccinationCampaign
            VaccinationCampaign vaccinationCampaign1 = new VaccinationCampaign()
            {
                Name = "Campaña Vacunación Infantes Norte",
                StartDateTime = DateTime.UtcNow.AddDays(3),
                EndDateTime = DateTime.UtcNow.AddDays(10),
                Description = "Campaña de vacunación para los niños de los CS del Norte",
                Image = "https://www.tumbes.gob.pe/wp-content/uploads/2019/01/campaña-vacunacion-infantes.jpg",
            };

            VaccinationCampaign vaccinationCampaign2 = new VaccinationCampaign()
            {
                Name = "Campaña Vacunación Niños Lima",
                StartDateTime = DateTime.UtcNow.AddDays(8),
                EndDateTime = DateTime.UtcNow.AddDays(15),
                Description = "Campaña de vacunación para los niños de los CS de Lima",
                Image = "https://www.tumbes.gob.pe/wp-content/uploads/2019/01/campaña-vacunacion-infantes.jpg",
            };

            if (!VaccinationCampaigns.Any())
            {
                VaccinationCampaigns.AddRange(new List<VaccinationCampaign>()
                {
                    vaccinationCampaign1, vaccinationCampaign2
                });
                _logger.LogInformation("2 Campañas de Vacunación Creadas");
            }
            #endregion

            #region VaccinationCampaignLocations
            VaccinationCampaignLocation vaccinationCampaignLocation1 = new VaccinationCampaignLocation()
            {
                VaccinationCampaignId = 1,
                HealthCenterId = 1
            };

            VaccinationCampaignLocation vaccinationCampaignLocation2 = new VaccinationCampaignLocation()
            {
                VaccinationCampaignId = 1,
                HealthCenterId = 2
            };

            VaccinationCampaignLocation vaccinationCampaignLocation3 = new VaccinationCampaignLocation()
            {
                VaccinationCampaignId = 2,
                HealthCenterId = 3
            };

            VaccinationCampaignLocation vaccinationCampaignLocation4 = new VaccinationCampaignLocation()
            {
                VaccinationCampaignId = 2,
                HealthCenterId = 4
            };

            if (!VaccinationCampaignLocations.Any())
            {
                VaccinationCampaignLocations.AddRange(new List<VaccinationCampaignLocation>()
                {
                    vaccinationCampaignLocation1, vaccinationCampaignLocation2, vaccinationCampaignLocation3, vaccinationCampaignLocation4
                });
            }
            _logger.LogInformation("4 Ubicaciones de Campaña de Vacunación Creadas");
            #endregion

            #region VaccinationCampaignDetails
            VaccinationCampaignDetail vaccinationCampaignDetail1 = new VaccinationCampaignDetail()
            {
                VaccinationCampaignId = 1,
                VaccineId = 1
            };

            VaccinationCampaignDetail vaccinationCampaignDetail2 = new VaccinationCampaignDetail()
            {
                VaccinationCampaignId = 1,
                VaccineId = 2
            };

            VaccinationCampaignDetail vaccinationCampaignDetail3 = new VaccinationCampaignDetail()
            {
                VaccinationCampaignId = 1,
                VaccineId = 3
            };

            VaccinationCampaignDetail vaccinationCampaignDetail4 = new VaccinationCampaignDetail()
            {
                VaccinationCampaignId = 2,
                VaccineId = 3
            };

            VaccinationCampaignDetail vaccinationCampaignDetail5 = new VaccinationCampaignDetail()
            {
                VaccinationCampaignId = 2,
                VaccineId = 4
            };

            VaccinationCampaignDetail vaccinationCampaignDetail6 = new VaccinationCampaignDetail()
            {
                VaccinationCampaignId = 2,
                VaccineId = 5
            };

            if (!VaccinationCampaignDetails.Any())
            {
                VaccinationCampaignDetails.AddRange(new List<VaccinationCampaignDetail>()
                {
                    vaccinationCampaignDetail1, vaccinationCampaignDetail2, vaccinationCampaignDetail3, vaccinationCampaignDetail4, vaccinationCampaignDetail5, vaccinationCampaignDetail6
                });
            }
            _logger.LogInformation("6 Detalle de Campaña de Vacunación Creados");
            #endregion

            #region VaccinationCampaignReminders
            Reminder reminderCampaign1 = new Reminder()
            {
                ParentId = parent1.ParentId,
                SendDate = DateTime.Now,
                VaccinationCampaignId = vaccinationCampaign1.VaccinationCampaignId,
                Via = ReminderVias.Email
            };
            Reminder reminderCampaign2 = new Reminder()
            {
                ParentId = parent1.ParentId,
                SendDate = DateTime.Now,
                VaccinationCampaignId = vaccinationCampaign2.VaccinationCampaignId,
                Via = ReminderVias.Email
            };
            if (!Reminders.Any())
            {
                Reminders.AddRange(new List<Reminder>()
                {
                    reminderCampaign1, reminderCampaign2
                });
                _logger.LogInformation("2 Recordatorios de Campañas de Vacunación Creadas");
            }
            #endregion
            SaveChanges();
        }
    }
}
