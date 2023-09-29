namespace ExchangeRate.Aggregator.Shared.Abstractions.Swagger;

public class SwaggerOptions
{
    public bool Enabled { get; set; }

    public bool ReDocEnabled { get; set; }

    public string Name { get; set; }

    public string DocumentationFileBasePath { get; set; }

    public string AssemblyName { get; set; }

    public string Title { get; set; }

    public string Version { get; set; }
        
    /// <summary>
    /// Custom API description goes here, including HTML or
    /// plain text
    /// </summary>
    public string Description { get; set; }
        
    /// <summary>
    /// Custom security description goes here, including HTML or
    /// plain text
    /// </summary>
    public string SecurityDescription { get; set; }
        
    public string RoutePrefix { get; set; }

    public bool IncludeSecurity { get; set; }

    public bool IncludeOptionalHeaders { get; set; }

    public bool SerializeAsOpenApiV2 { get; set; }
}