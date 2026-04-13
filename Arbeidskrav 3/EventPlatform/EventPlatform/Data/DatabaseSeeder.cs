namespace EventPlatform.Data;

/// <summary>
/// Runs schema.sql and seed.sql on first startup if the database does not exist.
/// </summary>
public class DatabaseSeeder
{
    private readonly AppDatabase _db;

    public DatabaseSeeder(AppDatabase db)
    {
        _db = db;
    }

    /// <summary>Initialises the database schema and inserts seed data.</summary>
    public void Seed() => throw new NotImplementedException();
}
