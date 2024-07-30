using Microsoft.EntityFrameworkCore;
using Otus.Teaching.Pcf.ReceivingFromPartner.Core.Domain;
using Otus.Teaching.Pcf.ReceivingFromPartner.DataAccess.Data;
using System;
using System.Reflection;

namespace Otus.Teaching.Pcf.ReceivingFromPartner.DataAccess
{
    public class DataContext
        : DbContext
    {


        public DbSet<Partner> Partners { get; set; }


        public DataContext()
        {

        }
        static DataContext()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());//Your assembly here

            //TODO ???
            base.OnModelCreating(modelBuilder);

        }
    }
}