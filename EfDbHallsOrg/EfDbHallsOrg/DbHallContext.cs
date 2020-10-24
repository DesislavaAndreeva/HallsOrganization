using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfDbHallsOrg
{
    public class DbHallContext : DbContext
    {
        public DbHallContext() : base("DbHallContext") { }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
    }
}
