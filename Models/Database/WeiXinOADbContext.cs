using Microsoft.EntityFrameworkCore;
using WeiXinOA.Models.Account;
using WeiXinOA.Models.ApplyForm;
using WeiXinOA.Models.Services;

namespace WeiXinOA.Models.Database
{
    public class WeiXinOADbContext : DbContext
    {
        //private readonly string _connectionString;

        public WeiXinOADbContext(DbContextOptions<WeiXinOADbContext> options) : base(options)
        {
        }

        //public WeiXinOADbContext(WXHostEnvHelper hostEnvHelper)
        //{
        //    _connectionString = hostEnvHelper.ConnectionString;
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(_connectionString, b => b.MigrationsAssembly("WeiXinOA"));
        //    base.OnConfiguring(optionsBuilder);
        //}


        public DbSet<VolunteerDetails> Tbl_Volunteer { get; set; }

        public DbSet<VolunteerFamily> Tbl_VolunteerFamily { get; set; }

        public DbSet<ElderDetails> Tbl_Elder { get; set; }

        public DbSet<ElderFamily> Tbl_ElderFamily { get; set; }

        public DbSet<LoginUser> Tbl_LoginUser { get; set; }

        //public DbSet<LoginUserRole> Tbl_LoginUserRole { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //modelBuilder.Entity<ElderDetails>().HasMany(e => e.ElderFamilys).WithOne(e => e.ElderDetails).HasForeignKey(f => f.ElderIdNumber).HasPrincipalKey(e => e.IdNumber);           
        //}
    }
}
