using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.UnitTests.TestUtilities;

public static class InMemoryTestDatabase
{
    public static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}

public sealed class SqliteTestDatabase : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<AppDbContext> _options;

    public SqliteTestDatabase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _connection.CreateCollation("my_case_insensitive",
            (x, y) => string.Compare(x, y, StringComparison.OrdinalIgnoreCase));

        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .ReplaceService<IModelCustomizer, SqliteModelCustomizer>()
            .Options;

        using var context = CreateContext();
        context.Database.EnsureCreated();
    }

    public AppDbContext CreateContext() => new(_options);

    public void Dispose() => _connection.Dispose();
}

internal sealed class SqliteModelCustomizer(ModelCustomizerDependencies dependencies) : RelationalModelCustomizer(dependencies)
{
    public override void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        base.Customize(modelBuilder, context);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var checkConstraint in entityType.GetCheckConstraints().ToList())
                entityType.RemoveCheckConstraint(checkConstraint.ModelName);
        }
    }
}
