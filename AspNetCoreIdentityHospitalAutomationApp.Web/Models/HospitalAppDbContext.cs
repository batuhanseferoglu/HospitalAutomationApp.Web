using AspNetCoreIdentityHospitalAutomationApp.Web.ViewModels;
using HospitalAutomationApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.Models
{
    public class HospitalAppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public HospitalAppDbContext()
        {

        }

        public HospitalAppDbContext(DbContextOptions<HospitalAppDbContext> options) : base(options)
        {

        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);

        //    builder.Entity<Event>()
        //            .Property(e => e.EventStart)
        //            .HasColumnType("datetime2");

        //    builder.Entity<Event>()
        //            .Property(e => e.EventEnd)
        //            .HasColumnType("datetime2");
        //}

        public DbSet<Event> Events { get; set; }

        public DbSet<MemberEventViewModel> MemberEvents { get; set; }

        public DbSet<MedicineViewModel> Meds { get; set; }

        public DbSet<PatientFileViewModel> Files { get; set; }

        public DbSet<FormViewModel> Forms { get; set; }



    }




}
