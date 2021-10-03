using Microsoft.EntityFrameworkCore;
using TrayTool.Repository.Model;

namespace TrayTool.Repository
{
    public class TrayToolDb : DbContext
    {
        public DbSet<BaseModel> BaseModels { get; set; }
        public DbSet<Seperator> Seperators { get; set; }
        public DbSet<AbstractItem> AbstractItems { get; set; }
        public DbSet<Directory> Directories { get; set; }
        public DbSet<Item> Items { get; set;}
        public DbSet<Argument> Arguments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite("data source=testdb.sqlite;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BaseModel>().Property(p => p.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<BaseModel>().ToTable("BaseModel");
        }
    }
}
