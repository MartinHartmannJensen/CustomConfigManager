using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace CustomConfigProject.Base
{
    /// <summary>
    /// Class containing basic functions for a configuration system, using .ini text files.
    /// Made to be inherited by a class with singleton instantiating and containing properties that reference,
    /// Get and Set.
    /// Important to do:
    /// defaultCollection values,
    /// RunStartup() method in contructor
    /// </summary>
    abstract class SettingsManager
    {
        public abstract string ConfigPath { get; }
        private string FolderPath;

        protected Dictionary<string, string> PropertiesCollection = new Dictionary<string, string>();
        protected Dictionary<string, string> DefaultCollection = new Dictionary<string, string>();

        protected void RunStartup()
        {
            string[] folders = Regex.Split(ConfigPath, @"\\");
            FolderPath = folders[0];
            for (int i = 1; i < folders.Length - 1; i++)
            {
                FolderPath += @"\" + folders[i];
            }

            if (File.Exists(ConfigPath))
            {
                ReadConfiguration();
            }
            else
            {
                SetValuesFromDefault();
                Save();
            }
        }

        protected string Get(string prop)
        {
            if (PropertiesCollection.ContainsKey(prop))
                return PropertiesCollection[prop];
            return DefaultCollection[prop];
        }

        protected void Set(string prop, object value)
        {
            var val = value.ToString();
            PropertiesCollection[prop] = val;
        }

        // Write to config file
        public void Save()
        {
            Directory.CreateDirectory(FolderPath);
            using (StreamWriter sr = new StreamWriter(ConfigPath))
            {
                foreach (var item in PropertiesCollection)
                {
                    sr.WriteLine(item.Key + "=" + item.Value);
                }
            }
        }

        // Read config file and set values
        private void ReadConfiguration()
        {
            foreach (string item in File.ReadLines(ConfigPath))
            {
                string[] key_value = Regex.Split(item, @"\s+=\s+|=\s+|\s+=|=");
                if (DefaultCollection.ContainsKey(key_value[0]))
                    PropertiesCollection[key_value[0]] = key_value[1];
            }
        }

        // In case of no file, set all from default
        private void SetValuesFromDefault()
        {
            foreach (var item in DefaultCollection)
            {
                PropertiesCollection[item.Key] = item.Value;
            }
        }
    }
}
