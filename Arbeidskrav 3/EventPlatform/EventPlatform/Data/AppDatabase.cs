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
    public object GetConnection() => throw new NotImplementedException();
    // TODO: Return Microsoft.Data.Sqlite.SqliteConnection
}
