using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using static ModSystem.Utils;

namespace ModSystem
{
    internal static class ModCompiler
    {
        public static List<ModConstants> CompiledMods = new List<ModConstants>();
        private static List<string> _defaultReferences = new List<string>();

        public static void CompileMods() 
        {
            _defaultReferences = GetDefaultReferences();

            var modDirectories = Directory.GetDirectories(Paths.ModsPath);

            if (modDirectories.Length == 0)
                return;

            foreach (var modDirectory in modDirectories) 
            {
                if (!IsModValid(modDirectory))
                    continue;

                var modConstants = DeserializeModJson(modDirectory);

                modConstants.ModDirectory = modDirectory;
                modConstants.DllPath = CompileMod(modConstants);

                CompiledMods.Add(modConstants);
            }
        }

        private static string CompileMod(ModConstants modConstants)
        {
            List<string> modReferences = new List<string>();
            modReferences.AddRange(_defaultReferences);
            modReferences.AddRange(GetModDependencies(modConstants.ModDirectory));

            List<MetadataReference> modMetadataReferences = CreateMetadataReferences(modReferences);
            List<string> sources = GetModSources(modConstants.ModDirectory);
            List<SyntaxTree> syntaxTrees = CreateSyntaxTreeList(sources);

            if (syntaxTrees.Count == 0)
                return null;

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName: Guid.NewGuid().ToString().Normalize(),
                syntaxTrees: syntaxTrees,
                references: modMetadataReferences,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            return EmitMod(modConstants, compilation);
        }

        private static string EmitMod(ModConstants modConstants, CSharpCompilation compilation)
        {
            string outputFileName = $"{modConstants.Author}-{modConstants.ModName}-v{modConstants.Version}".Replace(" ", "");
            string outputPath = Paths.Combine(Paths.CompilationsPath, $"{outputFileName}.dll");

            EmitResult emitResult = CSharpFileSystemExtensions.Emit(compilation, outputPath);

            if (!emitResult.Success)
            {
                File.Delete(outputPath);
                return null;
            }

            return outputPath;
        }

        private static List<string> GetDefaultReferences()
        {
            var references = new List<string>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.IsDynamic)
                    continue;
                
                references.Add(assembly.Location);
            }
            return references;
        }

        private static List<string> GetModDependencies(string modDirectory)
        {
            var depsDir = Paths.Combine(modDirectory, "dependencies");

            if (!Directory.Exists(depsDir))
                return new List<string>();

            return Directory.GetFiles(depsDir, "*.dll").ToList();
        }

        private static List<MetadataReference> CreateMetadataReferences(List<string> references)
        {
            var metadataReferences = new List<MetadataReference>();
            foreach (var reference in references)
            {
                PortableExecutableReference metadataReference = MetadataReference.CreateFromFile(reference);
                metadataReferences.Add(metadataReference);
            }
            return metadataReferences;
        }

        private static List<SyntaxTree> CreateSyntaxTreeList(List<string> sources)
        {
            var syntaxTreeList = new List<SyntaxTree>();
            foreach (var source in sources)
            {
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);
                syntaxTreeList.Add(syntaxTree);
            }

            return syntaxTreeList;
        }

        private static List<string> GetModSources(string modDirectory)
        {
            var sources = new List<string>();
            var scriptFiles = Directory.GetFiles(modDirectory, "*.cs");

            foreach (var scriptFile in scriptFiles)
            {
                var fileText = File.ReadAllText(scriptFile);

                if (string.IsNullOrEmpty(fileText))
                    continue;

                sources.Add(fileText);
            }

            return sources;
        }
    }
}