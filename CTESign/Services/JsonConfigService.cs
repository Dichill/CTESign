using CTESign.MVVM.Model;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CTESign.Services
{
    public class JsonConfigService
    {
        private readonly string _filePath;

        public JsonConfigService(string filePath)
        {
            _filePath = filePath;
        }

        // Create or update the JSON file with the provided ConfigModel
        public async Task SaveConfigAsync(ConfigModel config)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true // For pretty printing of the JSON
            };
            var jsonString = JsonSerializer.Serialize(config, options);

            await File.WriteAllTextAsync(_filePath, jsonString);
        }

        // Read and return the ConfigModel from the JSON file
        public async Task<ConfigModel> LoadConfigAsync()
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException("Config file not found.");
            }

            var jsonString = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<ConfigModel>(jsonString);
        }

        // Update specific values in the existing JSON file
        public async Task UpdateConfigAsync(string? department = null, DateTime? issueDate = null, string? afkLogoPath = null, int afkLogoSize = -1, string? afkSignageText = null)
        {
            var config = await LoadConfigAsync();

            if (department != null)
            {
                config.Department = department;
            }

            if (issueDate.HasValue)
            {
                config.IssueDate = issueDate.Value;
            }

            if (afkLogoPath != null)
            {
                config.AFKLogoPath = afkLogoPath;
            }

            if (afkLogoSize != -1)
            {
                config.AFKLogoSize = afkLogoSize;
            }

            if (afkSignageText != null)
            {
                config.AFKSignageText = afkSignageText;
            }

            await SaveConfigAsync(config);
        }

        // Check if the config file exists
        public bool ConfigExists()
        {
            return File.Exists(_filePath);
        }
    }
}
