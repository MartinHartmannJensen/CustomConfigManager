using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace CustomConfigProject
{
    class GenerateTool
    {
        static string DerivedName = "ConfigMgnr";
        static string PathInput = Environment.CurrentDirectory + @"\input.txt";
        static string PathOutput = Environment.CurrentDirectory + @"\output.txt";

        public void GenerateClass()
        {
            var cool = ReadInputs();
            WriteToOutput(cool);
        }


        private string[] ReadInputs()
        {
            string longstringjohnson;
            
            using (StreamReader sr = new StreamReader(PathInput))
            {
                longstringjohnson = sr.ReadToEnd();
            }
            longstringjohnson = Regex.Replace(longstringjohnson, @"^#.*|\n|\r", "", RegexOptions.Multiline);
            string[] returnArr = Regex.Split(longstringjohnson, @";");

            return returnArr;
        }

        private void WriteToOutput(string[] lines)
        {
            List<string[]> presets = new List<string[]>();

            foreach (var item in lines)
            {
                string[] preset = Regex.Split(item, @"\s+");
                presets.Add(preset);
            }

            using (StreamWriter sw = new StreamWriter(PathOutput))
            {
                sw.WriteLine("class {0} : CustomConfigProject.Base.SettingsManager", DerivedName);
                sw.WriteLine("{");
                // Init
                sw.WriteLine("private static {0} instance = new {0}();", DerivedName);
                sw.WriteLine("public static " + DerivedName + " I { get { return instance; } }");
                sw.WriteLine("public override string ConfigPath { get { return null; }}");
                sw.WriteLine("");
                sw.WriteLine("private " + DerivedName + "() : base() {");
                
                foreach (var item in presets)
                {
                    sw.WriteLine("DefaultCollection[\"{0}\"] = \"{1}\";", item[1], item[2]);
                }

                sw.WriteLine("RunStartup();");
                sw.WriteLine("}");

                // Props

                foreach (var item in presets)
                {
                    string itemName = item[1];
                    switch (item[0].ToUpper())
                    {
                        case "INT":
                            sw.WriteLine("public int " +  itemName + "  { get { return int.Parse(Get(\"" + itemName + "\")); } set { Set(\"" + itemName + "\", value); } }");
                            break;
                        case "BOOL":
                            sw.WriteLine("public bool " + itemName + " { get { return bool.Parse(Get(\"" + itemName + "\")); } set { Set(\"" + itemName + "\", value); } }");
                            break;
                        default:
                            sw.WriteLine("public string " + itemName + " { get { return Get(\"" + itemName + "\"); } set { Set(\"" + itemName + "\", value); } }");
                            break;
                    }
                }

                sw.WriteLine("}");
            }
        }
    }
}
