using Microsoft.Data.Sqlite;

var newHash = "$2a$12$AncAxFHnLCGvOPZcPov5eO46GxshgGKZY90hRsY9tjVnKhHKmBxdy";
var dbPath = Path.GetFullPath("../data/DashyBoard_Dev.db");

Console.WriteLine($"Updating password hash in: {dbPath}");

using var db = new SqliteConnection($"Data Source={dbPath}");
db.Open();
var cmd = db.CreateCommand();
cmd.CommandText = "UPDATE Admins SET PasswordHash = @hash";
cmd.Parameters.AddWithValue("@hash", newHash);
var rows = cmd.ExecuteNonQuery();
Console.WriteLine($"Updated {rows} admin row(s). Password is now: admin123");
