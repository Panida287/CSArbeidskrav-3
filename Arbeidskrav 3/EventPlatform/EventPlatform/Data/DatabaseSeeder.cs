using Microsoft.Data.Sqlite;

namespace EventPlatform.Data;

/// <summary>
/// Runs schema.sql and seed.sql on first startup if the database does not exist.
/// </summary>
public class DatabaseSeeder
{
    private readonly AppDatabase _db;
    private readonly string _schemaPath;
    private readonly string _seedPath;

    public DatabaseSeeder(AppDatabase db, string sqlFolder = "docs/sql")
    {
        _db = db;
        _schemaPath = Path.Combine(sqlFolder, "schema.sql");
        _seedPath   = Path.Combine(sqlFolder, "seed.sql");
    }

    /// <summary>
    /// Creates the database schema and inserts seed data on first run.
    /// Skips if the Users table already exists.
    /// Prints [DB] Test data loaded. on success.
    /// </summary>
    public void Seed()
    {
        using var connection = _db.GetConnection();

        if (IsAlreadySeeded(connection))
        {
            Console.WriteLine("[DB] Database already initialised. Skipping seed.");
            return;
        }

        RunSqlFile(connection, _schemaPath);
        RunSqlFile(connection, _seedPath);

        Console.WriteLine("[DB] Test data loaded.");
    }

    // --- private helpers ---

    private static bool IsAlreadySeeded(SqliteConnection connection)
    {
        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Users';";
        return (long)(cmd.ExecuteScalar() ?? 0L) > 0;
    }

    private static void RunSqlFile(SqliteConnection connection, string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"[DB] Warning: SQL file not found at {path}");
            return;
        }

        var sql = File.ReadAllText(path);
        var cmd = connection.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }
}
