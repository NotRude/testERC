using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

using Microsoft.EntityFrameworkCore;

namespace ERC
{
    public class AppContext : DbContext
    {
        public DbSet<MetersData> MetersDatas { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ERC.db");
        }
    }
}
