using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;

namespace Spestqnko.Data
{
    public class CleanUpMigrations
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("This utility will clean up migration files to resolve Entity Framework model conflicts.");
            Console.WriteLine("Press Y to proceed or any other key to exit.");
            
            var key = Console.ReadKey();
            if (key.Key != ConsoleKey.Y)
                return;
                
            Console.WriteLine("\nCleaning up migration files...");
            
            // Get the migration directory
            var migrationsDir = Path.Combine(Directory.GetCurrentDirectory(), "Migrations");
            
            if (!Directory.Exists(migrationsDir))
            {
                Console.WriteLine($"Migration directory not found: {migrationsDir}");
                return;
            }
            
            // Delete all existing migration files (except this utility)
            foreach (var file in Directory.GetFiles(migrationsDir, "*.cs"))
            {
                if (!file.Contains("CleanUpMigrations") && !file.Contains("MigrationHelper"))
                {
                    try
                    {
                        File.Delete(file);
                        Console.WriteLine($"Deleted: {file}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting {file}: {ex.Message}");
                    }
                }
            }
            
            Console.WriteLine("\nMigration files have been cleaned up.");
            Console.WriteLine("Now you can run: dotnet ef migrations add InitialCreate");
            Console.WriteLine("Followed by: dotnet ef database update");
        }
    }
} 