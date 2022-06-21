using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestHospitalAPI.Models;

namespace TestHospitalAPI.Data
{
    public class TestHospitalAPIContext : DbContext
    {
        public TestHospitalAPIContext (DbContextOptions<TestHospitalAPIContext> options)
            : base(options)
        {
        }

        public DbSet<TestHospitalAPI.Models.patient>? patient { get; set; }
    }
}
