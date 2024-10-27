using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Builder;

namespace Solstice.API.Scanning;

public class CoreAssemblyScanning : AssemblyLoadContext
{
    private readonly string _directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
    
    public static void ScanByPrefix(string assemblyPrefix)
    {
        string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        if (!Directory.Exists(assemblyPath))
        {
            Console.WriteLine("Assembly path does not exist.");
            return;
        }

        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        {
            var assemblyName = new AssemblyName(args.Name).Name;
            var assemblyFilePath = Path.Combine(assemblyPath, $"{assemblyName}.dll");

            if (File.Exists(assemblyFilePath) && assemblyName.StartsWith(assemblyPrefix))
            {
                return Assembly.LoadFrom(assemblyFilePath);
            }
            return null;
        };

        // Pre-load only assemblies with the specified prefix
        foreach (var assemblyFile in Directory.GetFiles(assemblyPath, "*.dll"))
        {
            var assemblyName = Path.GetFileNameWithoutExtension(assemblyFile);
            if (assemblyName.StartsWith(assemblyPrefix))
            {
                Assembly.LoadFrom(assemblyFile);
            }
        }
    }
    
    public static void ScanByCompanyName(string companyName)
    {
        string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        if (!Directory.Exists(assemblyPath))
        {
            Console.WriteLine("Assembly path does not exist.");
            return;
        }

        // Subscribe to the AssemblyResolve event
        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        {
            var assemblyName = new AssemblyName(args.Name).Name;
            var assemblyFilePath = Path.Combine(assemblyPath, $"{assemblyName}.dll");

            if (File.Exists(assemblyFilePath) && IsSolutionAssembly(assemblyFilePath, companyName))
            {
                return Assembly.LoadFrom(assemblyFilePath);
            }
            return null;
        };

        // Pre-load only solution assemblies
        foreach (var assemblyFile in Directory.GetFiles(assemblyPath, "*.dll"))
        {
            if (IsSolutionAssembly(assemblyFile, companyName))
            {
                Assembly.LoadFrom(assemblyFile);
            }
        }
    }
    
    public static void ScanByMetadata()
    {
        string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        if (!Directory.Exists(assemblyPath))
        {
            Console.WriteLine("Assembly path does not exist.");
            return;
        }

        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        {
            var assemblyName = new AssemblyName(args.Name).Name;
            var assemblyFilePath = Path.Combine(assemblyPath, $"{assemblyName}.dll");

            if (File.Exists(assemblyFilePath) && HasSolutionMetadata(assemblyFilePath))
            {
                return Assembly.LoadFrom(assemblyFilePath);
            }
            return null;
        };

        // Pre-load only assemblies with the custom metadata
        foreach (var assemblyFile in Directory.GetFiles(assemblyPath, "*.dll"))
        {
            if (HasSolutionMetadata(assemblyFile))
            {
                Assembly.LoadFrom(assemblyFile);
            }
        }
    }

    
    public static void Scan(Assembly assembly)
    {
        new CoreAssemblyScanning(Path.GetDirectoryName(assembly.Location)).ScanAndLoadAssemblies();
    }
    
    public CoreAssemblyScanning()
    {
    }

    public CoreAssemblyScanning(Assembly assembly)
    {
        _directoryPath = Path.GetDirectoryName(assembly.Location) ?? string.Empty;
    }
        
    public CoreAssemblyScanning(string directoryPath)
    {
        _directoryPath = directoryPath;
    }
        
    public void ScanAndLoadAssemblies()
    {
        LoadAllAssemblies();
        LoadAssembliesIntoCurrentAppDomain();
    }

    private void LoadAllAssemblies()
    {
        if (string.Empty.Equals(_directoryPath)) return;
        var assemblyFiles = Directory.GetFiles(_directoryPath, "*.dll");

        foreach (var assemblyFile in assemblyFiles)
        {
            try
            {
                LoadFromAssemblyPath(assemblyFile);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }

    protected override Assembly Load(AssemblyName assemblyName)
    {
        string assemblyPath = Path.Combine(_directoryPath, $"{assemblyName.Name}.dll");
        return (File.Exists(assemblyPath) ? LoadFromAssemblyPath(assemblyPath) : null) ?? throw new InvalidOperationException();
    }
    private void LoadAssembliesIntoCurrentAppDomain()
    {
        foreach (var assembly in Assemblies)
        {
            try
            {
                // Get the assembly name
                AssemblyName assemblyName = assembly.GetName();
            
                // Load the assembly into the current AppDomain
                AppDomain.CurrentDomain.Load(assemblyName);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
    
    private static bool IsSolutionAssembly(string assemblyFile, string companyName)
    {
        try
        {
            var assembly = Assembly.LoadFrom(assemblyFile);
            var companyAttribute = assembly
                .GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;

            // Only load assemblies with matching company/product name
            return companyAttribute == companyName;
        }
        catch
        {
            return false; // Ignore assemblies that fail to load
        }
    }
    
    private static bool HasSolutionMetadata(string assemblyFile)
    {
        try
        {
            var assembly = Assembly.LoadFrom(assemblyFile);

            // Retrieve all AssemblyMetadataAttributes
            var metadataAttributes = assembly.GetCustomAttributes<AssemblyMetadataAttribute>();

            // Check if any attribute has Key="IsSolutionAssembly" and Value="true"
            foreach (var attribute in metadataAttributes)
            {
                if (attribute is { Key: "IsSolutionAssembly", Value: "true" })
                {
                    return true;
                }
            }

            return false;
        }
        catch
        {
            return false;
        }
    }
}

public static class CoreAssemblyScanningExtensions
{
    private static void ScanAndLoadAssemblies(this WebApplicationBuilder builder)
    {
        new CoreAssemblyScanning().ScanAndLoadAssemblies();
    }
    
    /// <summary>
    /// Scan and load assemblies by prefix.
    /// </summary>
    /// <param name="builder">Your web api builder</param>
    /// <param name="assemblyPrefix">The assembly prefix of your projects</param>
    public static void ScanAndLoadAssembliesByPrefix(this WebApplicationBuilder builder, string assemblyPrefix)
    {
        CoreAssemblyScanning.ScanByPrefix(assemblyPrefix);
    }
    
    /// <summary>
    /// Scan and load assemblies by company name.
    /// </summary>
    /// <param name="builder">Your web api builder</param>
    /// <param name="companyName">Your company name set in assemblies settings</param>
    public static void ScanAndLoadAssembliesByCompanyName(this WebApplicationBuilder builder, string companyName)
    {
        CoreAssemblyScanning.ScanByCompanyName(companyName);
    }
    
    /// <summary>
    /// Scan and load assemblies by metadata.
    /// Add the metadata attribute "IsSolutionAssembly" to true, surrounded by PropertyGroup.
    /// </summary>
    /// <remarks>
    /// &lt;PropertyGroup&gt;&lt;IsSolutionAssembly&gt;true&lt;/IsSolutionAssembly&gt;&lt;/PropertyGroup&gt;
    /// </remarks>
    /// <param name="builder">Your web api builder</param>
    public static void ScanAndLoadAssembliesByMetadata(this WebApplicationBuilder builder)
    {
        CoreAssemblyScanning.ScanByMetadata();
    }
    
    public static void ScanAndLoadAssemblies(this WebApplicationBuilder builder, Assembly assembly)
    {
        new CoreAssemblyScanning(assembly).ScanAndLoadAssemblies();
    }
    
}