using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Hosting;
using System;

namespace Demo.Extensions
{
    public static class EnvironmentExtensions
    {
        public static bool IsProd(this IHostEnvironment hosting) =>
            hosting?.EnvironmentName == "prod";
        public static bool IsQa(this IHostEnvironment hosting) =>
            hosting?.EnvironmentName == "qa";
        public static bool IsTest(this IHostEnvironment hosting) =>
            hosting?.EnvironmentName == "test";
        public static bool IsDev(this IHostEnvironment hosting) =>
            hosting?.EnvironmentName == "Development";

        public static void Set(this IHostEnvironment hosting, string env)
        {
            hosting.EnvironmentName = env;
            ApplicationEnvironment.SetAppEnvironment(env);
        }
        public static void SetDefault(this IHostEnvironment hosting)
        {
            if (hosting.EnvironmentName?.Length == 0)
                throw new Exception("Environment variable not defined");
            ApplicationEnvironment.SetAppEnvironment(hosting);
        }
    }
    public static class ApplicationEnvironment
    {
        public static bool IsProd { get; private set; }
        public static bool IsQa { get; private set; }
        public static bool IsDev { get; private set; }
        public static bool IsTest { get; private set; }
        public static string Name { get; private set; }
        public static void SetAppEnvironment(IHostEnvironment hostEnvironment)
        {
            Name = hostEnvironment?.EnvironmentName;
            IsProd = hostEnvironment.IsProd();
            IsQa = hostEnvironment.IsQa();
            IsDev = hostEnvironment.IsDev();
            IsTest = hostEnvironment.IsTest();
        }
        public static void SetAppEnvironment(string env)
        {
            Name = env;
            IsProd = env == "prod";
            IsQa = env == "qa";
            IsTest = env == "test";
            IsDev = env == "Development";
        }
    }
}
