using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace Solstice.API.Scanning
{
    public class CoreAssemblyScanning : AssemblyLoadContext
    {
        private readonly string _directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public CoreAssemblyScanning()
        {
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
            var assemblyFiles = Directory.GetFiles(_directoryPath, "*.dll");

            foreach (var assemblyFile in assemblyFiles)
            {
                try
                {
                    LoadFromAssemblyPath(assemblyFile);
                    Console.WriteLine($"Loaded {assemblyFile}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load {assemblyFile}: {ex.Message}");
                }
            }
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = Path.Combine(_directoryPath, $"{assemblyName.Name}.dll");
            if (File.Exists(assemblyPath))
            {
                return LoadFromAssemblyPath(assemblyPath);
            }
            return null;
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
                        Assembly loadedAssembly = AppDomain.CurrentDomain.Load(assemblyName);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                
            
        }
    }
}