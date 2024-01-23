using Intact.BusinessLogic.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Intact.BusinessLogic.Data;

public class AppDbContext : DbContext
{
    private bool IsSqlServer => Database.IsSqlServer();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<LocalizationDao> Localizations { get; init; }
    public DbSet<FactionDao> Factions { get; init; }
    public DbSet<ProtoBuildingDao> ProtoBuildings { get; init; }
    public DbSet<ProtoWarriorDao> ProtoWarriors { get; init; }
    public DbSet<MapDao> Maps { get; init; }
    public DbSet<MapBuildingDao> MapBuildings { get; init; }
    public DbSet<PlayerOptionsDao> PlayerOptions { get; init; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        AddComputedLocalizableColumns<FactionDao>();
        AddComputedLocalizableColumns<ProtoBuildingDao>();
        AddComputedLocalizableColumns<ProtoWarriorDao>();
        AddComputedLocalizableColumns<MapDao>();
            
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
            
        void AddComputedLocalizableColumns<T>() where T : LocalizableDao
        {
            var name = typeof(T).Name.Replace("Dao", "").Replace("Proto", "");
            var prefix = IsSqlServer ? "Id +" : "\"Id\" ||"; // postgres otherwise
            builder.Entity<T>().Property(p => p.TermName).HasComputedColumnSql($"{prefix} '{name}Name'", !IsSqlServer);
            builder.Entity<T>().Property(p => p.TermDescription).HasComputedColumnSql($"{prefix} '{name}Description'", !IsSqlServer);
        }    
    }
}