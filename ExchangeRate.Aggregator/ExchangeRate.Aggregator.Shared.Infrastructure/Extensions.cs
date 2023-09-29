using System.Reflection;
using System.Runtime.CompilerServices;
using ExchangeRate.Aggregator.Shared.Abstractions.Modules;
using ExchangeRate.Aggregator.Shared.Abstractions.Swagger;
using ExchangeRate.Aggregator.Shared.Infrastructure.Api;
using ExchangeRate.Aggregator.Shared.Infrastructure.Behaviours;
using ExchangeRate.Aggregator.Shared.Infrastructure.Exceptions;
using ExchangeRate.Aggregator.Shared.Infrastructure.Swagger;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.ReDoc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ExchangeRate.Aggregator.Shared.Infrastructure;

public static class Extensions
{
    private const string CorrelationIdKey = "correlation-id";
    
    private const string SectionName = "swagger";

    private const string RegistryName = "docs.swagger";

    public static IServiceCollection AddModularInfrastructure(
        this IServiceCollection services,
        IList<Assembly> assemblies,
        IList<IModule> modules)
    {
        var disabledModules = new List<string>();
        using (var serviceProvider = services.BuildServiceProvider())
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            foreach (var (key, value) in configuration.AsEnumerable())
            {
                if (!key.Contains(":module:enabled"))
                {
                    continue;
                }

                if (!bool.Parse(value))
                {
                    disabledModules.Add(key.Split(":")[0]);
                }
            }
        }

        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
        });
        
        services.AddHttpContextAccessor();

        services.AddSwaggerDocs();

        services.AddSwaggerGen();

        services.AddControllers(opt =>
            {
                opt.SuppressAsyncSuffixInActionNames = false;
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            })
            .ConfigureApplicationPartManager(manager =>
            {
                var removedParts = new List<ApplicationPart>();
                foreach (var disabledModule in disabledModules)
                {
                    var parts = manager.ApplicationParts.Where(x => x.Name.Contains(
                        disabledModule,
                        StringComparison.InvariantCultureIgnoreCase));
                    removedParts.AddRange(parts);
                }

                foreach (var part in removedParts)
                {
                    manager.ApplicationParts.Remove(part);
                }

                manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            });

        return services;
    }
    
    public static IApplicationBuilder UseModularInfrastructure(
        this IApplicationBuilder app,
        IConfiguration configuration, IWebHostEnvironment env)
    {
        app.UseDeveloperExceptionPage();
        app.UseSwaggerDocs();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseHttpExceptionHandler();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", context => context.Response.WriteAsync("Exchange rate API"));
            endpoints.MapControllers();
        });
        
        return app;
    }

    public static T GetOptions<T>(this IServiceCollection services, string sectionName)
        where T : new()
    {
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        return configuration.GetOptions<T>(sectionName);
    }

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName)
        where T : new()
    {
        var options = new T();
        configuration.GetSection(sectionName).Bind(options);
        return options;
    }

    public static string GetModuleName(this object value)
        => value?.GetType().GetModuleName() ?? string.Empty;

    public static string GetModuleName(this Type type, string namespacePart = "Modules", int splitIndex = 2)
    {
        if (type?.Namespace is null)
        {
            return string.Empty;
        }

        return type.Namespace.Contains(namespacePart)
            ? type.Namespace.Split(".")[splitIndex].ToLowerInvariant()
            : string.Empty;
    }

    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        => app.Use((ctx, next) =>
        {
            ctx.Items.Add(CorrelationIdKey, Guid.NewGuid());
            return next();
        });

    public static Guid? TryGetCorrelationId(this HttpContext context)
        => context.Items.TryGetValue(CorrelationIdKey, out var id) ? (Guid)id : null;
    
    public static IServiceCollection AddValidationBehaviours(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        
        return services;
    }
    
    public static IServiceCollection AddVersioning(
        this IServiceCollection services)
    {
        services
            .AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
            .AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
            });

        return services;
    }

    public static IServiceCollection AddSwaggerDocs(this IServiceCollection builder, string sectionName = "swagger",
        Action<SwaggerGenOptions> action = null)
    {
        if (string.IsNullOrWhiteSpace(sectionName))
        {
            sectionName = "swagger";
        }

        SwaggerOptions options = builder.GetOptions<SwaggerOptions>(sectionName);
        return builder.AddSwaggerDocs(options, action);
    }

    public static IServiceCollection AddSwaggerDocs(
        this IServiceCollection builder,
        Func<ISwaggerOptionsBuilder, ISwaggerOptionsBuilder> buildOptions, Action<SwaggerGenOptions> action = null)
    {
        SwaggerOptions options = buildOptions(new SwaggerOptionsBuilder()).Build();
        return builder.AddSwaggerDocs(options, action);
    }

    public static IServiceCollection AddSwaggerDocs(this IServiceCollection builder, SwaggerOptions options,
        Action<SwaggerGenOptions> action = null)
    {
        if (!options.Enabled)
        {
            return builder;
        }

        builder.AddSingleton(options);
        builder.AddSwaggerGen(delegate(SwaggerGenOptions c)
        {
            c.SwaggerDoc(options.Name, new OpenApiInfo
            {
                Title = options.Title,
                Version = options.Version
            });
            c.DescribeAllParametersInCamelCase();
            c.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date"
            });
            var filePath = Path.Combine(
                options.DocumentationFileBasePath ?? AppContext.BaseDirectory,
                options.AssemblyName + ".xml");
            c.IncludeXmlComments(filePath);
            if (options.IncludeSecurity)
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    },
                });
            }

            if (options.IncludeOptionalHeaders)
            {
                action(c);
            }
        });
        return builder;
    }

    public static IApplicationBuilder UseSwaggerDocs(this IApplicationBuilder builder)
    {
        SwaggerOptions options = builder.ApplicationServices.GetService<SwaggerOptions>();
        if (options is not { Enabled: true })
        {
            return builder;
        }

        var routePrefix = string.IsNullOrWhiteSpace(options.RoutePrefix) ? string.Empty : options.RoutePrefix;
        builder.UseStaticFiles().UseSwagger(delegate(Swashbuckle.AspNetCore.Swagger.SwaggerOptions c)
        {
            c.RouteTemplate = routePrefix + "/{documentName}/swagger.json";
            c.SerializeAsV2 = options.SerializeAsOpenApiV2;
        });
        if (options.ReDocEnabled)
        {
            builder.UseReDoc(delegate(ReDocOptions c)
            {
                c.DocumentTitle = options.Title;
                c.SpecUrl = "/" + options.Name + "/swagger.json";
            });
        }

        return builder.UseSwaggerUI(delegate(SwaggerUIOptions c)
        {
            c.RoutePrefix = routePrefix;

            DefaultInterpolatedStringHandler defaultInterpolatedStringHandler =
                new DefaultInterpolatedStringHandler(15, 2);
            defaultInterpolatedStringHandler.AppendLiteral("/");
            defaultInterpolatedStringHandler.AppendFormatted(routePrefix);
            defaultInterpolatedStringHandler.AppendLiteral("/");
            defaultInterpolatedStringHandler.AppendFormatted(options.Name);
            defaultInterpolatedStringHandler.AppendLiteral("/swagger.json");
            c.SwaggerEndpoint(
                defaultInterpolatedStringHandler.ToStringAndClear().FormatEmptyRoutePrefix(),
                options.Title);
        });
    }
    
    // Summary:
    //     Replaces leading double forward slash caused by an empty route prefix
    //
    // Parameters:
    //   route:
    //     Route.
    //
    // Returns:
    //     Formatted route.
    private static string FormatEmptyRoutePrefix(this string route)
    {
        return route.Replace("//", "/");
    }
}