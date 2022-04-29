using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RPFBE.Model.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Auth
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<AppliedJob> AppliedJobs { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<JobSpecFile> SpecFiles { get; set; }  
        public DbSet<UserCV> UserCVs { get; set; }
        public DbSet<RequisitionProgress> RequisitionProgress { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
