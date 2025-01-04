using Microsoft.EntityFrameworkCore;

namespace Intact.BusinessLogic.Data;

public class DataSetupDbContext(DbContextOptions<DataSetupDbContext> options) : DbContext(options) { }