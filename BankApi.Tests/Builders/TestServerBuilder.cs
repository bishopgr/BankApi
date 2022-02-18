using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Xunit.Abstractions;

namespace BankApi.Tests.Builders
{

    internal class TestServerBuilder : WebApplicationFactory<Program>
    {
       public ITestOutputHelper? Output { get; set; }

        public TestServerBuilder(ITestOutputHelper output)
        {
            Output = output ?? throw new ArgumentNullException($"{nameof(output)}");
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureLogging(logging =>
            {
                logging.AddXUnit(Output);
                logging.AddConsole();
            });

            builder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseTestServer(configuration =>
                {
                    configuration.BaseAddress = new Uri("http://localhost:5001");
                });

            });

            var host = base.CreateHost(builder);

            return host;
        }
    }
}
