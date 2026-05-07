using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TicketingSystemFightNight.Models;

namespace TicketingSystemFightNight.Helpers
{
    public static class FighterImageHelper
    {
        private static readonly string[] ImageExtensions = { ".png", ".jpg", ".jpeg", ".svg", ".webp" };

        public static string GetFighterImageUrl(IWebHostEnvironment environment, Fighter? fighter)
        {
            if (fighter == null)
            {
                return "/images/fighter-placeholder.svg";
            }

            var candidates = new[]
            {
                fighter.Name,
                fighter.Name.Replace(" ", "-"),
                fighter.Name.Replace(" ", "_"),
                NormalizeFileName(fighter.Name)
            };

            foreach (var candidate in candidates.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                foreach (var extension in ImageExtensions)
                {
                    var fileName = candidate + extension;
                    var physicalPath = Path.Combine(environment.WebRootPath, "images", fileName);
                    if (File.Exists(physicalPath))
                    {
                        return "/images/" + Uri.EscapeDataString(fileName);
                    }

                    // Try lowercase version
                    var lowerFileName = candidate.ToLowerInvariant() + extension;
                    var lowerPhysicalPath = Path.Combine(environment.WebRootPath, "images", lowerFileName);
                    if (File.Exists(lowerPhysicalPath))
                    {
                        return "/images/" + Uri.EscapeDataString(lowerFileName);
                    }
                }
            }

            return "/images/fighter-placeholder.svg";
        }

        private static string NormalizeFileName(string input)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = new string(input.Select(ch => invalidChars.Contains(ch) ? '-' : ch).ToArray());
            sanitized = Regex.Replace(sanitized, "\\s+", "-").Trim('-');
            return sanitized;
        }
    }
}
