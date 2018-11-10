using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace IOExtensions
{
    public static class AccessRightsChecker
    {
        /// <summary>
        /// Test a directory for file access permissions
        /// Taken from https://stackoverflow.com/a/21996345/613299
        /// </summary>
        /// <param name="directoryPath">Full path to directory </param>
        /// <param name="accessRight">File System right tested</param>
        /// <returns>State [bool]</returns>
        public static bool DirectoryHasPermission(string directoryPath, FileSystemRights accessRight)
        {
            if (string.IsNullOrEmpty(directoryPath)) return false;
            if (!Directory.Exists(directoryPath)) return false;

            try
            {
                var rules = Directory.GetAccessControl(directoryPath).GetAccessRules(true, true, typeof(SecurityIdentifier));
                var identity = WindowsIdentity.GetCurrent();

                foreach (FileSystemAccessRule rule in rules)
                {
                    if (identity.Groups.Contains(rule.IdentityReference))
                    {
                        if ((accessRight & rule.FileSystemRights) == accessRight)
                        {
                            if (rule.AccessControlType == AccessControlType.Deny)
                            {
                                return false;
                            }

                            if (rule.AccessControlType == AccessControlType.Allow)
                                return true;
                        }
                    }
                }
            }
            catch { }
            return false;
        }
    }
}
