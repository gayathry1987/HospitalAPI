using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HospitalWebApplication.Models;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HospitalWebApplication.Data
{
    public class HospitalWebApplicationContext : DbContext
    {
        public HospitalWebApplicationContext (DbContextOptions<HospitalWebApplicationContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          //  modelBuilder.Entity<patient>()
           //     .HasNoKey()
           //     .ToView("patient");
           
        }
        
        public DbSet<HospitalWebApplication.Models.patient>? patient { get; set; }
        public DbSet<HospitalWebApplication.Models.patientDetails>? patientDetails { get; set; }
        public DbSet<HospitalWebApplication.Models.patientComments>? patientComments { get; set; }
    }
   
}
