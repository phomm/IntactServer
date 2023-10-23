using Intact.BusinessLogic.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Intact.BusinessLogic.Data
{
    public class IntactDbContext : DbContext
    {
        public IntactDbContext(DbContextOptions<IntactDbContext> options) : base(options) { }

        public DbSet<LocalizationDao> Localizations { get; set; }
        public DbSet<FactionDao> Factions { get; set; }
        public DbSet<ProtoBuildingDao> ProtoBuildings { get; set; }
        public DbSet<ProtoWarriorDao> ProtoWarriors { get; set; }
        public DbSet<MapDao> Maps { get; set; }
        public DbSet<MapBuildingDao> MapBuildings { get; set; }
        public DbSet<PlayerOptionsDao> PlayerOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalizationDao>().HasKey(x => new { x.TermId, x.LanguageCode});
            modelBuilder.Entity<FactionDao>().Property(p => p.TermName).HasComputedColumnSql("[Id] + 'FactionName'");
            modelBuilder.Entity<FactionDao>().Property(p => p.TermDescription).HasComputedColumnSql("[Id] + 'FactionDescription'");
            modelBuilder.Entity<ProtoBuildingDao>().Property(p => p.TermName).HasComputedColumnSql("[Id] + 'BuildingName'");
            modelBuilder.Entity<ProtoBuildingDao>().Property(p => p.TermDescription).HasComputedColumnSql("[Id] + 'BuildingDescription'");
            modelBuilder.Entity<ProtoWarriorDao>().Property(p => p.TermName).HasComputedColumnSql("[Id] + 'WarriorName'");
            modelBuilder.Entity<ProtoWarriorDao>().Property(p => p.TermDescription).HasComputedColumnSql("[Id] + 'WarriorDescription'");
            modelBuilder.Entity<ProtoWarriorDao>().Property(p => p.InLife).HasDefaultValue(1);
            modelBuilder.Entity<ProtoWarriorDao>().Property(p => p.InMana).HasDefaultValue(0);
            modelBuilder.Entity<ProtoWarriorDao>().Property(p => p.InMoves).HasDefaultValue(1);
            modelBuilder.Entity<ProtoWarriorDao>().Property(p => p.InActs).HasDefaultValue(1);
            modelBuilder.Entity<ProtoWarriorDao>().Property(p => p.InShots).HasDefaultValue(0);
            modelBuilder.Entity<ProtoWarriorDao>().Property(p => p.Cost).HasDefaultValue(0);
            modelBuilder.Entity<ProtoWarriorDao>().Property(p => p.IsHero).HasDefaultValue(0);
            modelBuilder.Entity<ProtoWarriorDao>().Property(p => p.IsMelee).HasDefaultValue(0);
            modelBuilder.Entity<ProtoWarriorDao>().Property(p => p.IsRanged).HasDefaultValue(0);
            modelBuilder.Entity<ProtoWarriorDao>().Property(p => p.IsBlockFree).HasDefaultValue(0);
            modelBuilder.Entity<ProtoWarriorDao>().Property(p => p.IsImmune).HasDefaultValue(0);
            modelBuilder.Entity<MapDao>().Property(p => p.TermName).HasComputedColumnSql("[Id] + 'MapName'");
            modelBuilder.Entity<MapDao>().Property(p => p.TermDescription).HasComputedColumnSql("[Id] + 'MapDescription'");
            modelBuilder.Entity<MapDao>().HasKey(x => new { x.Id, x.Version });
            modelBuilder.Entity<PlayerOptionsDao>().HasKey(x => new { x.MapId, x.Number });
            modelBuilder.Entity<MapBuildingDao>().HasKey(x => new { x.MapId, x.Number });
        }
    }
}
