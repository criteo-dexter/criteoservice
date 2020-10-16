using Criteo.AspNetCore.Helpers;
using Criteo.AspNetCore.Monitoring;
using Criteo.AspNetCore.Swagger;
using Criteo.Logging;
using Criteo.Services;
using JwtAuth;
using JwtAuth.Issuers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using System;
using System.IdentityModel.Tokens.Jwt;
using Criteo.AspNetCore.Middlewares;

namespace criteo.service
{
    public class Startup
    {
        private static readonly ILogger _logger = LogManager.GetLogger(typeof(Startup).FullName);

        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _env = env;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();

            ConfigureCriteoServices(services);
            ConfigureMvc(services);
            ConfigureAuthentication(services);
            ConfigureSwagger(services);
        }

        protected virtual void ConfigureCriteoServices(IServiceCollection services)
        {
            services.AddCriteoServices(registrar =>
            {
                var metricsRegistry = registrar.AddMetricsRegistry();
                var consul = registrar.AddServiceLocator(metricsRegistry);
                var keyValueStore = registrar.AddConsulKeyValueStore(metricsRegistry);
                var sdkConfigurationService = registrar.AddSdkConfigurationService(keyValueStore, metricsRegistry, consul);

                var kafkaProducer = registrar.AddKafkaProducer(metricsRegistry, consul, sdkConfigurationService);
                registrar.AddTracing(metricsRegistry, kafkaProducer);
            });
        }

        protected virtual void ConfigureMvc(IServiceCollection services)
        {
            services
                .AddMvc(ConfigureMvcOptions)
                .AddJsonOptions(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));
        }

        protected virtual void ConfigureMvcOptions(MvcOptions options)
        {
            options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
        }

        protected virtual void ConfigureAuthentication(IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtChecker = new JwtChecker(CriteoIssuerManagerHelper.CreatePublicIssuerManager(JwtCheckerErrorHandler));

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        SignatureValidator = (token, __) => jwtChecker.ValidateToken(token, out _) ? new JwtSecurityToken(token) : null
                    };
                });
        }

        protected virtual void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerDocument<Startup>(_configuration, _env);
        }

        public virtual void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime)
        {
            // TracingMiddleware must be added before GlobalExceptionMiddleware to get the traceId in exception log
            app.UseMiddleware<TracingMiddleware>();

            app.UseResponseCompression();
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger(_configuration, _env);
        }

        private static void JwtCheckerErrorHandler(object sender, EventArgs args)
        {
            _logger.Log(LogLevel.Error, $"Error encountered while checking a Jwt. sender: {sender} args: {args}");
        }
    }
}
