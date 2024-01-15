using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AiCorb.Utils
{
    public class AssemblyLoader: IDisposable {
        private static string ExecutingPath => Assembly.GetExecutingAssembly().Location;

        public AssemblyLoader() {
            AppDomain.CurrentDomain.AssemblyResolve += LoadMaterialDesign;
        }

        private static Assembly LoadMaterialDesign(object sender, ResolveEventArgs args) {
            if (null == ExecutingPath) return null;
            string assemlyToLoad = string.Empty;

            string GetAssemblyName(string fullName) => fullName.Substring(0, fullName.IndexOf(','));

            var path = ExecutingPath;
            var dir = new FileInfo(path).Directory;

            var assemblies = from file in dir.EnumerateFiles()
                where file.Name.EndsWith(".dll") ||
                      file.Name.EndsWith(".exe")
                select Assembly.LoadFrom(file.FullName);

            foreach(var assembly in assemblies) {

                var assemName = GetAssemblyName(assembly.FullName);
                var requested = GetAssemblyName(args.Name);

                try {
                    if (assemName == requested) {
                        return assembly;
                    }
                } catch (Exception) {
                    continue;
                }

                //}
            }
            return null;
        }

        void IDisposable.Dispose() {
            AppDomain.CurrentDomain.AssemblyResolve -= LoadMaterialDesign;
        }
    }
}