using System;
using CustomConfigProject.Base;

namespace CustomConfigProject
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateTool gt = new GenerateTool();
            gt.GenerateClass();
            System.Diagnostics.Process.Start("notepad", Environment.CurrentDirectory + @"\output.txt");
        }
    }

    class ConfigTest : SettingsManager
    {
        private static ConfigTest instance = new ConfigTest();
        public static ConfigTest I { get { return instance; } }

        //public override string ConfigPath { get { return @"C:\Users\Martin-PC\Desktop\Config\config.ini"; } }
        public override string ConfigPath { get { return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\ANConfig\config.ini"; } }

        private ConfigTest() : base()
        {
            // Set default values
            DefaultCollection["MyProperty"] = "40";
            DefaultCollection["Stringey"] = "heya";
            DefaultCollection["Booley"] = "True";
            // Startup checks
            RunStartup();
        }

        // Settings properties
        public int MyProperty { get { return int.Parse(Get("MyProperty")); } set { Set("MyProperty", value); } }

        public string Stringey { get { return Get("Stringey"); } set { Set("Stringey", value); } }

        public bool Booley { get { return bool.Parse(Get("Booley")); } set { Set("Booley", value); } }
    }

    class ConfigMgnr : SettingsManager
    {
        private static ConfigMgnr instance = new ConfigMgnr();
        public static ConfigMgnr I { get { return instance; } }
        public override string ConfigPath { get { return null; } }

        private ConfigMgnr() : base()
        {
            DefaultCollection["IsTrue"] = "false";
            DefaultCollection["Name"] = "martin";
            DefaultCollection["number"] = "42";
            RunStartup();
        }
        public bool IsTrue { get { return bool.Parse(Get("IsTrue")); } set { Set("IsTrue", value); } }
        public string Name { get { return Get("Name"); } set { Set("Name", value); } }
        public int number { get { return int.Parse(Get("number")); } set { Set("number", value); } }
    }

}
