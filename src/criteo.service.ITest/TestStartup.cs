using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;

namespace Criteo.Api.Exam.Demo.ITest
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration, IHostingEnvironment env)
            : base(configuration, env)
        {
        }

        protected override void ConfigureMvcOptions(MvcOptions options)
        {
            // Remove default Authentication: DO NOT DO THIS AT HOME
        }
    }
}
