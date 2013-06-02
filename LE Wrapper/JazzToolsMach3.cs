using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using LE_Wrapper;
using LEWrapperResponse;
using LE_Wrapper.Properties;
using System.Data;
//using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Configuration;
using LE_DataStructures;

namespace Jazz_Tools_Mach_3
{
    class Program
    {
        
        //private LEWrapper le;
        static void Main(string[] args)
        {
/*            string userInput;
            do
            {
                Console.Write("Enter EmpireName:");
                userInput = Console.ReadLine();
                if (string.IsNullOrEmpty(userInput))
                { Console.WriteLine("Thanks"); }
                else
                {
                    Console.WriteLine("Input [" + userInput + "]");
                    // other code to process user input
                }
            } while (!string.IsNullOrEmpty(userInput));

            Console.ReadKey();
 */
            string UserName = "Jazz";
            string Password = "Muir$ak4";
            string Server = "us1";
            
            LEWrapper le = new LEWrapper("Jazz","M","us1");
            PrintPlanetList p = new PrintPlanetList();
            p.Subscribe(le);
            le.EmpireLogin();
            //Dictionary<string, string> planetList = p.SortPlanetList();
            Dictionary<string, string> planetList = p.planetList;
            foreach (KeyValuePair<string, string> pair in planetList)
                le.BodyGetBuildings(pair.Key.ToString());
            //planetList = 
            //le.CloseLog();
            Console.ReadKey();
            //le.CloseLog();
        }
        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            //Program p = sender as Program;
            //CloseLog();

        }
    }

    class PrintPlanetList
    {
        public Dictionary<string, Response.Result.Building[]> habitableBuildingInfo;
        public Dictionary<string, Response.Result.Building[]> ssBuildingInfo;
        //private List<string> SortedPlanetList;
        public Dictionary<string, string> planetList;
        private Dictionary<string, string> HabitablesList;
        private Dictionary<string, string> SSList;
        public void Subscribe(LEWrapper le)
        {
            le.ServerResponseEvent += new LEWrapper.ServerResponseHandler(PrintList);
        }
        private void PrintList(LEWrapper le, Response e)
        {
            planetList = e.result.status.empire.planets;
           // foreach (KeyValuePair<string, string> pair in planetList)
           //     Console.WriteLine("{0}, {1}", pair.Key, pair.Value);
           // Console.ReadKey();
           // SortPlanetList();
        }
        public Dictionary<string,string> SortPlanetList()
        {
            planetList = planetList.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            Console.WriteLine("printing sorted list");
                foreach (KeyValuePair<string, string> pair in planetList)
                {
                    Console.WriteLine("{0}, {1}", pair.Key, pair.Value);
                    //SortedPlanetList.Add(pair.Value);
                }
            Console.ReadKey();
            return planetList;
            //SortedPlanetList.Sort();
            //foreach(SortedPlanetList)

        }
        public void SeperatePlanetsFromSS(LEWrapper le)
        {
            le.ServerResponseEvent += new LEWrapper.ServerResponseHandler(SeperatePlanetsEventHandler);
            foreach (KeyValuePair<string, string> pair in planetList)
            {
                le.BodyGetBuildings(pair.Key.ToString());
            }
        }
        public void SeperatePlanetsEventHandler(LEWrapper le, Response e)
        {
            if (e.id == 401)
            {
                if (e.result.body.surface_image == "surface-station")
                {
                    SSList.Add(e.result.status.body.id.ToString(), e.result.status.body.name);
                    //ssBuildingInfo.Add(e.result
                    //foreach (KeyValuePair<string, Response.Result.Building> v in e.result.buildings)
                        
                        //v.Value.efficiency;
                }
                else
                {
                    HabitablesList.Add(e.result.status.body.id.ToString(), e.result.status.body.name);
                }
            }
        }
    }
}
