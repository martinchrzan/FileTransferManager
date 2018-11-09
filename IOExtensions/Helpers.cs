using System;
using System.IO;

namespace IOExtensions
{
    internal static class Helpers
    {
        // Returns true if the path is a dir, false if it's a file and null if it's neither or doesn't exist. 
        public static bool? IsDirFile(this string path)
        {
            bool? result = null; if (Directory.Exists(path) || File.Exists(path))
            {
                // get the file attributes for file or directory 
                var fileAttr = File.GetAttributes(path);
                if (fileAttr.HasFlag(FileAttributes.Directory))
                    result = true;
                else result = false;
            }
            return result;
        }

        // corrects destination path for folder if provided destination is only directory not a full filename 
        internal static string CorrectFileDestinationPath(string source, string destination)
        {
            var destinationFile = destination;
            if (destination.IsDirFile() == true)
            {
                destinationFile = Path.Combine(destination, Path.GetFileName(source));
            }
            return destinationFile;
        }


        internal static DirectorySizeInfo DirSize(DirectoryInfo d)
        {
            DirectorySizeInfo size = new DirectorySizeInfo();

            try
            {
                // Add file sizes.
                var fis = d.GetFiles();
                foreach (var fi in fis)
                {
                    size.Size += fi.Length;
                }
                size.FileCount += fis.Length;

                // Add subdirectory sizes.
                var dis = d.GetDirectories();
                size.DirectoryCount += dis.Length;
                foreach (var di in dis)
                {
                    size += DirSize(di);
                }
            }
            catch (Exception ex)
            {
            }

            return size;
        }

        internal sealed class DirectorySizeInfo
        {
            public long FileCount = 0;
            public long DirectoryCount = 0;
            public long Size = 0;

            public static DirectorySizeInfo operator +(DirectorySizeInfo s1, DirectorySizeInfo s2)
            {
                return new DirectorySizeInfo()
                {
                    DirectoryCount = s1.DirectoryCount + s2.DirectoryCount,
                    FileCount = s1.FileCount + s2.FileCount,
                    Size = s1.Size + s2.Size
                };
            }
        }
    }
}
