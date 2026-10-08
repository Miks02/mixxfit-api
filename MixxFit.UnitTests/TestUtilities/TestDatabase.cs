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

/// <summary>
/// In-memory SQLite database for handlers that rely on relational-only features
/// (ExecuteDelete/ExecuteUpdate, cascade deletes, FK constraints), which the EF InMemory provider does not support.
/// </summary>
public sealed class SqliteTestDatabase : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<AppDbContext> _options;

    public SqliteTestDatabase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        // Postgres ICU collation used by the Exercise.Name column; SQLite needs an equivalent registered by name.
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

/// <summary>
/// Removes check constraints from the model. They are written for Postgres, and SQLite stores decimals as TEXT,
/// which always compares greater than a number, so constraints like <c>"Weight" &lt; 400</c> reject every row.
/// </summary>
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
