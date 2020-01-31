using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Tapend.Ssl.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        var certificateFile = Path.Combine(homeDirectory, ".localhost-ssl", "te-localhost.pfx");
                        if (File.Exists(certificateFile))
                            serverOptions.Listen(IPAddress.Loopback, 5001,
                                listenOptions =>
                                {
                                    listenOptions.UseHttps(
                                        certificateFile,
                                        "<Your Password Here>");
                                });
                    });
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}