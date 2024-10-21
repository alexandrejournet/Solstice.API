using System.Reflection;
using System.Runtime.Loader;

namespace Solstice.API.Scanning;

public class CoreAssemblyScanning : AssemblyLoadContext
{
    private readonly string _directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

    public static void Scan(string directoryPath)
    {
        new CoreAssemblyScanning(directoryPath).ScanAndLoadAssemblies();
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
}