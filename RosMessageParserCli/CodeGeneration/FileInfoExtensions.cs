using System;
using System.IO;

namespace Joanneum.Robotics.Ros.MessageParser.Cli.CodeGeneration
{
    public static class FileInfoExtensions
    {
        public static RosMessageType GetRosMessageType(this FileInfo fileInfo)
        {
            if (fileInfo == null) return RosMessageType.None;

            var ext = fileInfo.Extension.ToLowerInvariant();

            switch (ext)
            {
                case ".msg":
                    return RosMessageType.Message;
                case ".srv":
                    return RosMessageType.Service;
                case ".action":
                    return RosMessageType.Action;
                default:
                    return RosMessageType.None;
            }
        }

        public static string NameWithoutExtension(this FileInfo fileInfo)
        {
            if (fileInfo == null) throw new ArgumentNullException(nameof(fileInfo));

            return fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
        }
    }
}