using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KeenReloaded.Framework.Utilities
{
    public static class FileIOUtilities
    {
        public static string GetResourcesPath()
        {
            string currentDirectory = Assembly.GetCallingAssembly().Location;
            string parentPath = Directory.GetParent(currentDirectory).Parent.Parent.Parent.FullName;
            string assemblyName = Assembly.GetAssembly(typeof(Map)).GetName().Name;
            string resourcesPath = parentPath + $@"\{assemblyName}\Resources\";
            return resourcesPath;
        }
    }
}
