using LIN.Types.Developer.Models.Projects;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace LIN.Developer.Data;

public class MongoContext : DbContext
{

    public DbSet<ProjectModel> Projects { get; init; }


    public static MongoContext Create(IMongoDatabase database) =>
        new(new DbContextOptionsBuilder<MongoContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);

    public MongoContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProjectModel>().ToCollection("projects");
    }


}
