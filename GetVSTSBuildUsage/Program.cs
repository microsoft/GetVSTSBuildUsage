using System;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.VisualStudio.Services.Client;

namespace GetVSTSBuildUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                ShowUsage();
                return;
            }

            string accountUrl = args[0];
            var minFinishTime = DateTime.Parse(args[1]);
            var maxFinishTime = GetMaxFinishTime(DateTime.Parse(args[2]));

            Console.WriteLine("Getting projects in the account:");
            VssConnection connection = new VssConnection(new Uri(accountUrl), new VssAadCredential());

            var projectClient = connection.GetClient<ProjectHttpClient>();
            var projects = projectClient.GetProjects().Result;

            var buildClient = connection.GetClient<BuildHttpClient>();

            foreach (var project in projects)
            {
                var builds = buildClient.GetBuildsAsync(project.Id, minFinishTime: minFinishTime, maxFinishTime: maxFinishTime).Result;

                if (builds.Count > 0)
                {
                    Console.WriteLine($"{project.Name} project had {builds.Count} builds run between {minFinishTime} and {maxFinishTime}");
                    foreach (var build in builds)
                    {
                        ReportBuildInformation(build);
                    }
                }
                else
                {
                    Console.WriteLine($"Project {project.Name} did not having any builds during that time.");
                }
            }
            Console.WriteLine("Press a key.");
            Console.ReadKey();
        }

        private static void ReportBuildInformation(Build build)
        {
            if (build.Status == BuildStatus.Completed)
            {
                Console.WriteLine(
                    $"Build occured on {build.StartTime}, was requested by {build.RequestedBy.DisplayName} and for {build.RequestedFor.DisplayName}");
            }
            else
            {
                Console.WriteLine(
                    $"Build status of {build.Status} occured for {build.RequestedFor.DisplayName} on {build.QueueTime}");
            }
        }

        private static DateTime? GetMaxFinishTime(DateTime maxTime)
        {
            if (maxTime > DateTime.Now)
                return DateTime.Now;

            return maxTime;
        }

        private static void ShowUsage()
        {
            Console.WriteLine("GetVSTSBuildUsage [account url and collection] [min build finish date] [max build finish date]");
            Console.WriteLine("Example: GetVSTSBuildUsage http://myaccount.visualstudio.com/DefaultCollection 1/1/2016 1/31/2016");

        }
    }
}
