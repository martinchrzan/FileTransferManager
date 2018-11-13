using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace IOExtensions
{
    public static class AccessRightsChecker
    {
        /// <summary>
        /// Test a directory or file for file access permissions
        /// </summary>
        /// <param name="itemPath">Full path to file or directory </param>
        /// <param name="accessRight">File System right tested</param>
        /// <returns>State [bool]</returns>
        public static bool ItemHasPermision(string itemPath, FileSystemRights accessRight)
        {
            if (string.IsNullOrEmpty(itemPath)) return false;
            var isDir = itemPath.IsDirFile();
            if (isDir == null) return false;

            try
            {
                AuthorizationRuleCollection rules;
                if (isDir == true)
                {
                    rules = Directory.GetAccessControl(itemPath).GetAccessRules(true, true, typeof(SecurityIdentifier));
                }
                else
                {
                    rules = File.GetAccessControl(itemPath).GetAccessRules(true, true, typeof(SecurityIdentifier));
                }
                
                var identity = WindowsIdentity.GetCurrent();
                string userSID = identity.User.Value;

                foreach (FileSystemAccessRule rule in rules)
                {
                    if (rule.IdentityReference.ToString() == userSID || identity.Groups.Contains(rule.IdentityReference))
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
