#!/usr/bin/env dotnet-script
#r "nuget: BCrypt.Net-Next, 4.1.0"

using BCrypt.Net;

var password = "password123";
var hash = BCrypt.HashPassword(password, workFactor: 12);

Console.WriteLine($"Password: {password}");
Console.WriteLine($"Hash: {hash}");
Console.WriteLine();
Console.WriteLine("Verification test:");
Console.WriteLine($"Verify result: {BCrypt.Verify(password, hash)}");
