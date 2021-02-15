using System;
using System.IO;

namespace Hexarc.Pact.Tool.Internals
{
    public static class DirectoryOperations
    {
        public static void Clear(String path)
        {
            Array.ForEach(Directory.GetDirectories(path), x => Directory.Delete(x, true));
            Array.ForEach(Directory.GetFiles(path), File.Delete);
        }

        public static void CreateOrClear(String path)
        {
            if (Directory.Exists(path)) Clear(path);
            else Directory.CreateDirectory(path);
        }
    }
}
