var hashFromSeed = "$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyYzpLhJ5vMe";
var testPassword = "password123";

Console.WriteLine("Testing BCrypt password verification:");
Console.WriteLine($"Password: {testPassword}");
Console.WriteLine($"Hash from seed: {hashFromSeed}");
Console.WriteLine($"Verification result: {BCrypt.Net.BCrypt.Verify(testPassword, hashFromSeed)}");
Console.WriteLine();

// Generate a new hash
var newHash = BCrypt.Net.BCrypt.HashPassword(testPassword, workFactor: 12);
Console.WriteLine($"Newly generated hash: {newHash}");
Console.WriteLine($"New hash verifies: {BCrypt.Net.BCrypt.Verify(testPassword, newHash)}");
