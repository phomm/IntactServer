using Intact.BusinessLogic.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Intact.BusinessLogic.Data
{
    public class IntactDbContext : DbContext
    {
        private bool IsSqlServer => Database.IsSqlServer();

        public IntactDbContext(DbContextOptions<IntactDbContext> options) : base(options) { }

        public DbSet<LocalizationDao> Localizations { get; set; }
        public DbSet<FactionDao> Factions { get; set; }
        public DbSet<ProtoBuildingDao> ProtoBuildings { get; set; }
        public DbSet<ProtoWarriorDao> ProtoWarriors { get; set; }
        public DbSet<MapDao> Maps { get; set; }
        public DbSet<MapBuildingDao> MapBuildings { get; set; }
        public DbSet<PlayerOptionsDao> PlayerOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            AddComputedLocalizableColumns<FactionDao>(builder);
            AddComputedLocalizableColumns<ProtoBuildingDao>(builder);
            AddComputedLocalizableColumns<ProtoWarriorDao>(builder);
            AddComputedLocalizableColumns<MapDao>(builder);
            
            builder.Entity<LocalizationDao>().HasKey(x => new { x.TermId, x.LanguageCode});
            builder.Entity<ProtoWarriorDao>().Property(p => p.InLife).HasDefaultValue(1);
            builder.Entity<ProtoWarriorDao>().Property(p => p.InMana).HasDefaultValue(0);
            builder.Entity<ProtoWarriorDao>().Property(p => p.InMoves).HasDefaultValue(1);
            builder.Entity<ProtoWarriorDao>().Property(p => p.InActs).HasDefaultValue(1);
            builder.Entity<ProtoWarriorDao>().Property(p => p.InShots).HasDefaultValue(0);
            builder.Entity<ProtoWarriorDao>().Property(p => p.Cost).HasDefaultValue(0);
            builder.Entity<ProtoWarriorDao>().Property(p => p.IsHero).HasDefaultValue(0);
            builder.Entity<ProtoWarriorDao>().Property(p => p.IsMelee).HasDefaultValue(0);
            builder.Entity<ProtoWarriorDao>().Property(p => p.IsRanged).HasDefaultValue(0);
            builder.Entity<ProtoWarriorDao>().Property(p => p.IsBlockFree).HasDefaultValue(0);
            builder.Entity<ProtoWarriorDao>().Property(p => p.IsImmune).HasDefaultValue(0);
            builder.Entity<MapDao>().HasKey(x => new { x.Id, x.Version });
            builder.Entity<PlayerOptionsDao>().HasKey(x => new { x.MapId, x.Number });
            builder.Entity<MapBuildingDao>().HasKey(x => new { x.MapId, x.Number });
        }

        private void AddComputedLocalizableColumns<T>(ModelBuilder builder) where T : LocalizableDao
        {
            var name = typeof(T).Name.Replace("Dao", "").Replace("Proto", "");
            var prefix = IsSqlServer ? "Id +" : "\"Id\" ||"; // postgres otherwise
            builder.Entity<T>().Property(p => p.TermName).HasComputedColumnSql($"{prefix} '{name}Name'", !IsSqlServer);
            builder.Entity<T>().Property(p => p.TermDescription).HasComputedColumnSql($"{prefix} '{name}Description'", !IsSqlServer);
        }
    }
}
