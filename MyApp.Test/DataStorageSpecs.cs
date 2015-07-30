using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using DataStorage;

using FluentAssertions;

using Mono.Cecil;

using Xunit;

namespace MyApp.Test
{
    public class DataStorageSpecs
    {
        private const string PortableAssemblyFilePath = @"..\..\..\ExtraLib\PCL\DataStorage\bin\Debug\DataStorage.dll";
        private readonly Assembly DroidAssembly = Assembly.GetAssembly(typeof(Storage));

        [Fact]
        public void When_overriding_a_portable_library_the_footprint_of_the_platform_libraries_must_be_identical_to_the_portable_library()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var portableModule = ModuleDefinition.ReadModule(PortableAssemblyFilePath);
            var droidModule = ModuleDefinition.ReadModule(DroidAssembly.Location);
    
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var portableFileName = portableModule.Name;
            var droidFileName = droidModule.Name;

            var portableFullyQualifiedAssemblyName = portableModule.Assembly.FullName;
            var droidFullyQualifiedAssemblyName = droidModule.Assembly.FullName;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------

            droidFileName.ShouldBeEquivalentTo(portableFileName
                , because: "When overriding portable libraries the file names must be identical");

            droidFullyQualifiedAssemblyName.ShouldBeEquivalentTo(portableFullyQualifiedAssemblyName
                , because: "When overriding portable libraries the 'Fully Qualified Assembly Names' must be identical");
        }

        [Fact]
        public void When_overriding_a_portable_library_the_platform_libraries_must_be_matching_to_the_portable_library()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var portableModule = ModuleDefinition.ReadModule(PortableAssemblyFilePath);            
            var droidModule = ModuleDefinition.ReadModule(DroidAssembly.Location);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var portableTypes = portableModule.GetTypes().Where(assType => assType.IsPublic).ToArray();
            var droidTypes = droidModule.GetTypes().Where(assType => assType.IsPublic).ToArray();
            
            var portableMethodsFullNames = portableTypes
                    .SelectMany(portableType => portableType.Methods.Where(portableMethod => portableMethod.IsPublic))
                    .Select(portableType => portableType.FullName);
            var droidMethodsFullNames = droidTypes.SelectMany(GetAllMethodsFullNames).ToArray();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            
            // Get all PortableTypes that can't be found in the DroidTypes
            var missingDroidTypes = 
                portableTypes.Where(portableType => droidTypes.All(droidType => droidType.FullName != portableType.FullName)).ToArray();
            missingDroidTypes.Should().BeEmpty(
                string.Format("the Droid library of '{0}' should not miss any types that the portable library has", portableModule.Name));

            // Get all PortableMethods that can't be found in the DroidMethods
            var missingDroidMethods =
                portableMethodsFullNames.Where(portableMethod => droidMethodsFullNames.All(droidMethod => droidMethod != portableMethod));
            missingDroidMethods.Should().BeEmpty(
                string.Format("the Droid library of '{0}' should not miss any methods and constructors that the portable library has", portableModule.Name));

        }

        private List<string> GetAllMethodsFullNames(TypeDefinition typeDefinition)
        {
            var collectingTypeDefinition = typeDefinition;
            var superTypeFullName = typeDefinition.FullName;
            var collectedMethodDefinition = collectingTypeDefinition.Methods.Select(method => method.FullName).ToList();

            // Get Inherited types and their Methods and adjust their namespaces
            while (collectingTypeDefinition.BaseType != null
                && collectingTypeDefinition.BaseType.MetadataType == MetadataType.Class)
            {
                collectingTypeDefinition = collectingTypeDefinition.BaseType.Resolve();
                var inheritedMethods = collectingTypeDefinition.Methods.Where(method => method.IsPublic);

                var convertedInheritedMethodsNames =
                    inheritedMethods.Select(methodDefinition =>
                        methodDefinition.FullName.Replace(methodDefinition.DeclaringType.FullName, superTypeFullName)).ToList();

                collectedMethodDefinition.AddRange(convertedInheritedMethodsNames);
            }

            return collectedMethodDefinition;
        }
    }
}
