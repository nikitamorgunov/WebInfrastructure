﻿namespace Web
{
    using System;
    using System.Reflection;
    using System.Text;
    using Application.Services;
    using Autofac;
    using Domain.Dtos;
    using Installers;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Skeleton.Web;
    using Skeleton.Web.Authentication.JwtBearer;
    using Skeleton.Web.Authentication.JwtBearer.Configuration;
    using Skeleton.Web.Configuration;
    using Swashbuckle.Swagger.Model;
    using Swashbuckle.SwaggerGen.Application;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Startup : WebApiBaseStartup
    {

        public Startup(IHostingEnvironment env, CommandLineArgumentsProvider commandLineArgumentsProvider)
            : base(env, commandLineArgumentsProvider)
        {
        }

        protected override void ConfigureSwaggerDocumentator(SwaggerGenOptions options)
        {
            options.SingleApiVersion(new Info
                                     {
                                         Version = "v1",
                                         Title = "Values providing API",
                                         Description = "A dummy to get configuration values",
                                         TermsOfService = "None"
                                     });
        }

        protected override void ConfigureOptions(IServiceCollection services)
        {
            services
                .Configure<DefaultConfigurationValues>(Configuration.GetSection("DefaultConfigurationValues"));
        }

        protected override void RegisterDependencies(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyModules(typeof(DataAccessInstaller).GetTypeInfo().Assembly);
        }

        protected override Func<IApplicationBuilder, IApplicationBuilder> CreatePipelineConfigurator(
            IHostingEnvironment env, ILoggerFactory loggerFactory,
            Func<IApplicationBuilder, IApplicationBuilder> pipelineBaseConfigurator)
        {
            return x => pipelineBaseConfigurator(x
                       .UseJwtBearerAuthorisationTokens(
                           b => b
                               .ConfigureSigningKey(
                                   SecurityAlgorithms.HmacSha256,
                                   new SymmetricSecurityKey(
                                       Encoding.UTF8.GetBytes(
                                           "23j79h675s78T904gldUt0M5SftPg50H3W85s5A8u68zUV4AIJ")))
                               .ConfigureTokensIssuingOptions(
                                   i => i
                                       .WithGetEndpotint("/api/Account/Token")
                                       .WithLifetime(TimeSpan.FromHours(2))
                                       .WithTokenIssueEventHandler(new TokenIssueEventHandler(loggerFactory.CreateLogger<TokenIssueEventHandler>())))
                               .ConfigureJwtBearerOptions(
                                   o => o
                                       .WithTokenValidationParameters(
                                           v => v
                                               .WithLifetimeValidation()
                                               .WithoutAudienceValidation()
                                               .WithoutIssuerValidation()))));
        }
    }
}