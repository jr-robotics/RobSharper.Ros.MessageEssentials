using System;
using Joanneum.Robotics.Ros.MessageParser.Antlr;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public class CodeGenerationPackageContext
    {
        private readonly CodeGenerationContext _context;
        
        public RosPackageInfo PackageInfo { get; }
        public IRosMessagePackageParser Parser { get; }

        public CodeGenerationPackageContext(CodeGenerationContext context, RosPackageInfo packageInfo, IRosMessagePackageParser parser)
        {
            if (parser == null) throw new ArgumentNullException(nameof(parser));
            
            _context = context ?? throw new ArgumentNullException(nameof(context));
            PackageInfo = packageInfo ?? throw new ArgumentNullException(nameof(packageInfo));
            Parser = new PackageRegistryMessageParserAdapter(context.PackageRegistry, parser);
        }
    }
}