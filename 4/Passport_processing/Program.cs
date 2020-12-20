using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Passport_processing
{
    class Program
    {
        static readonly string[] toFind = {"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"};
        static readonly string[] eye_colors = {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};
        const string cid = "cid";
        static void Main(string[] args)
        {
            int matches = 0;
            String input = null;
            try{
                input = File.ReadAllText("input.txt");
            }
            catch(IOException e){
                Console.WriteLine(e);
            }
            string[] output = input.Split("\n\n");
            foreach (string intermediate in output)
            {
                bool all = true;
                foreach(string str in toFind){
                    if(!intermediate.Contains(str)) all = false;
                }
                if(!all)continue;

                string correctlyFormatted = intermediate.Replace("\n"," ");
                string[] toCheck = correctlyFormatted.Split(" ");
                foreach (string item in toCheck)
                {
                    string[] splitted = item.Split(":");
                    string key = splitted[0];
                    string value = splitted[1];

                    if(key.Equals("byr")){
                        int year;
                        if(!int.TryParse(value, out year)) all = false;
                        if (year < 1920 || year > 2002) all = false;
                    }else if(key.Equals("iyr")){
                        int year;
                        if(!int.TryParse(value, out year)) all = false;
                        if(year < 2010 || year > 2020) all = false;
                    }else if(key.Equals("eyr")){
                        int year;
                        if(!int.TryParse(value, out year)) all = false;
                        if (year < 2020 || year > 2030) all = false;
                    }else if(key.Equals("hgt")){
                        if(value.Contains("cm")){
                            string toConvert = value.Remove(value.IndexOf("c"));
                            int height;
                            if(!int.TryParse(toConvert, out height)) all = false;
                            if (height < 150 || height > 193) all = false;
                        }else if(value.Contains("in")){
                            string toConvert = value.Remove(value.IndexOf("i"));
                            int height;
                            if(!int.TryParse(toConvert, out height)) all = false;
                            if (height < 59 || height > 76) all = false;
                        }else{
                            all = false;
                        }
                    }else if(key.Equals("hcl")){
                        //We will use a regular expression to check this
                        Regex re = new Regex(@"^#[a-f|0-9]{6}$");
                        if(!re.IsMatch(value)) all = false;
                    }else if(key.Equals("ecl")){
                        bool checks = false;
                        foreach(string str in eye_colors){
                            if(value.Equals(str)){ checks = true; break;}
                        }
                        if(!checks) all = false;
                    }else if(key.Equals("pid")){
                        Regex re = new Regex(@"^[0-9]{9}$");
                        if(!re.IsMatch(value)) all = false;
                    }
                }
                

                if(all) matches++;
                else continue;
                
            }
            Console.WriteLine(matches);
            //Console.Write(input);
        }
    }
}
