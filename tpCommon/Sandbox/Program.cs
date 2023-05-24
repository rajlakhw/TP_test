using System;
using Microsoft.Extensions.Configuration;
using Global_Settings;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox
{
    class Program
    {
        public readonly IConfiguration configuration;

        public Program(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        static void Main(string[] args)
        {
            //var a = new GetStringFromCFG();
            //DateTime today = DateTime.Today.AddDays(7);
            //var thisWeekEnd = today.AddDays(-7);

            //Console.WriteLine(today);
            //Console.WriteLine(thisWeekEnd);

            //var org = new Data.EmployeeOwnershipRelationship();
            //var propertyInfos = org.GetType().GetProperties();
            //foreach (var prop in propertyInfos)
            //{
            //    Console.WriteLine(prop.Name);
            //}

            var dbRelations = new List<int>() { 1,23,4,5,6,7,8 };
            var newRelations = new List<int>() { 2,4,5,8,34,9,10,12 };

            var toDelete = dbRelations.Where(x => !newRelations.Contains(x));
            var toCreate = newRelations.Where(x => !dbRelations.Contains(x));

            Console.WriteLine("To delete: " + String.Join(", ", toDelete));
            Console.WriteLine("To create: " + String.Join(", ", toCreate));
        }

        public string GetStringFromCFG()
        {
            var GlobalVars = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);

            return GlobalVars.LondonJobDriveBaseDirectoryPathForApp;
        }
    }
}
