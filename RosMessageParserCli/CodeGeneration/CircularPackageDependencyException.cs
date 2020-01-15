using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class CircularPackageDependencyException : InvalidOperationException
    {
        private IEnumerable<Tuple<string, IEnumerable<string>>> _packages = Enumerable.Empty<Tuple<string, IEnumerable<string>>>();

        /// <summary>
        /// An enumeration of packages and its dependencies.
        /// </summary>
        public IEnumerable<Tuple<string, IEnumerable<string>>> Packages => _packages;

        public CircularPackageDependencyException()
        {
        }

        protected CircularPackageDependencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CircularPackageDependencyException(string message) : base(message)
        {
        }

        public CircularPackageDependencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CircularPackageDependencyException(IEnumerable<CodeGenerationPackageContext> packages) : this()
        {
            SetPackages(packages);
        }

        public CircularPackageDependencyException(string message, IEnumerable<CodeGenerationPackageContext> packages) : this(message)
        {
            SetPackages(packages);
        }

        public CircularPackageDependencyException(string message, Exception innerException, IEnumerable<CodeGenerationPackageContext> packages) : this(message, innerException)
        {
            SetPackages(packages);
        }

        private void SetPackages(IEnumerable<CodeGenerationPackageContext> packages)
        {
            if(packages == null)
                return;
            
            _packages = packages.Select(x =>
                new Tuple<string, IEnumerable<string>>(x.PackageInfo.Name, x.Parser.PackageDependencies)).ToList();
        }
    }
}