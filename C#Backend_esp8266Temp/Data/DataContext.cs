using esp8266Temp.Models;
using Microsoft.EntityFrameworkCore;

namespace esp8266Temp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Temperature> Temperatures { get; set; }

    }
}