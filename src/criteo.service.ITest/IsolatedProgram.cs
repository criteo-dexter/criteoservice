using System.Collections.Generic;
using System.Linq;
using Criteo.Testing.Isolation;
using Criteo.Testing.Isolation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Sdk.ProductionResources.Consul;

namespace Criteo.Api.Exam.Demo.ITest
{
    class IsolatedProgram
    {
        private static void Main(string[] args)
        {
            using (var inProcessTestHelper = new InProcessTestHelper(nameof(IsolatedProgram)))
            {
                var isolatedHostBuilder = inProcessTestHelper
                    .CreateAspNetCoreBuilder(builder => builder.UseStartup<TestStartup>());

                var program = new IsolatedAspNetCoreProgram(args, isolatedHostBuilder);
                program.RunHost();
            }
        }
    }
}
