using Microsoft.Data.Sqlite;

namespace EventPlatform.Data;

/// <summary>
/// SQLite connection factory. Inject this into repository constructors.
/// </summary>
public class AppDatabase
{
    private readonly string _connectionString;

    public AppDatabase(string dbPath = "eventplatform.db")
    {
        _connectionString = $"Data Source={dbPath}";
    }

    /// <summary>Opens and returns a new SQLite connection.</summary>
    public SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
