using Easyyyyy_Reborn.Core;
using Easyyyyy_Reborn.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Easyyyyy_Reborn
{
    public partial class App : Application
    {
        public static Configuration? ApplicationConfiguration { get; set; }

        public static bool ApplicationIsWorking = true;

        public static int TotalClicks = 0;

        public static bool GlobalIsWorking = false;

        public static Assembly GetAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ApplicationConfiguration = ImportConfiguration();
        }

        private static void CheckFilesIntoExists()
        {
            if (!Directory.Exists(Constants.DirectoryLocation))
            {
                Directory.CreateDirectory(Constants.DirectoryLocation); 
                CreateConfiguration(true);
            } else if (!File.Exists(Constants.ConfigurationLocation))
            {
                CreateConfiguration(true);
            }
        }

        private static void CreateConfiguration(bool IsCreateMode)
        {
            if (IsCreateMode) File.Create(Constants.ConfigurationLocation).Close();

            JObject config = new JObject(
                new JProperty("toggle_mode", ApplicationConfiguration != null ? ApplicationConfiguration.IsToggleMode : false),
                new JProperty("default_clicks", ApplicationConfiguration != null ? ApplicationConfiguration.IsDefaultClicks : false),
                new JProperty("count_cps", ApplicationConfiguration != null ? ApplicationConfiguration.CountClicksPerSecond : 7),
                new JProperty("enabled_random", ApplicationConfiguration != null ? ApplicationConfiguration.IsEnabledRandom : true),
                new JProperty("bind_key", ApplicationConfiguration != null ? ApplicationConfiguration.BindKey : null),
                new JProperty("is_left_click", ApplicationConfiguration != null ? ApplicationConfiguration.IsLeftClick : true),
                new JProperty("int_bind_key", ApplicationConfiguration != null ? ApplicationConfiguration.IntBindKey : 0));

            File.WriteAllText(Constants.ConfigurationLocation, config.ToString());
        }

        private Configuration? ImportConfiguration()
        {
            CheckFilesIntoExists();

            using (var reader = new StreamReader(Constants.ConfigurationLocation))
            {
                var Data = Newtonsoft.Json.JsonConvert.DeserializeObject<Configuration>(reader.ReadToEnd());

                reader.Dispose();
                reader.Close();

                return Data;
            }
        }

        public static void ImplementUpdateConfiguration()
        {
            CheckFilesIntoExists(); CreateConfiguration(false);
        }
    }
}
