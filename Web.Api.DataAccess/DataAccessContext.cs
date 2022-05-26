using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Api.DataAccess
{
   public  class DataAccessContext:DbContext
    {
        public DataAccessContext(DbContextOptions<DataAccessContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
        public DbSet<Users> Users { get; set; }
    }
}
