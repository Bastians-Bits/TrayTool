using Microsoft.EntityFrameworkCore;
using System;
using TrayTool.Repository.Model;

namespace TrayTool.Repository
{
    public class TrayToolDb : DbContext
    {
        public DbSet<BaseModelEntity> BaseModels { get; set; }
        public DbSet<SeperatorEntity> Seperators { get; set; }
        public DbSet<AbstractItemEntity> AbstractItems { get; set; }
        public DbSet<DirectoryEntity> Directories { get; set; }
        public DbSet<ItemEntity> Items { get; set;}
        public DbSet<ArgumentEntity> Arguments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite("data source=testdb.sqlite;");
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BaseModelEntity>().ToTable("BaseModel");

            modelBuilder.Entity<DirectoryEntity>().HasMany(e => e.Children).WithOne(e => e.Parent).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
