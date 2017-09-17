using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace TextEditor
{
    public class ActiveDirectoryInfo
    {
        public string UserName { get; }

        public ActiveDirectoryInfo()
        {
            this.UserName = GetCurrentUserName();
            Console.WriteLine(GetCurrentUserName());
        }

        /// <summary>
        /// Returns a string with the current username.
        /// </summary>
        /// <returns></returns>
        private string GetCurrentUserName()
        {
            string[] userNamePart = WindowsIdentity.GetCurrent().Name.Split('\\');
            if (userNamePart.Length > 1)
            {
                return userNamePart[1];
            }
            else
            {
                return userNamePart[0];
            }
        }

        /// <summary>
        /// Gets all the groups a user is member of.
        /// </summary>
        /// <returns>A list of strings with all current user groups.</returns>
        public List<string> GetCurrentUserGroups()
        {
            List<string> result = new List<string>();

            DirectorySearcher ds = new DirectorySearcher
            {
                Filter = $"(&(objectClass=user)(sAMAccountName={this.UserName}))"
            };

            SearchResult sr = ds.FindOne();

            DirectoryEntry userEntry = sr.GetDirectoryEntry();
            userEntry.RefreshCache(new string[] { "tokenGroups" });

            for (int i = 0; i < userEntry.Properties["tokenGroups"].Count; i++)
            {
                SecurityIdentifier sid = new SecurityIdentifier((byte[])userEntry.Properties["tokenGroups"][i], 0);
                NTAccount nt = (NTAccount)sid.Translate(typeof(NTAccount));

                result.Add(nt.Value.Split('\\')[1]);
            }

            ds.Dispose();
            userEntry.Close();
            userEntry.Dispose();

            return result;
        }


        /// <summary>
        /// Returns the Notes (info) field of all the groups the users is a member of
        /// </summary>
        /// <returns>List of string with Notes from AD group</returns>
        public List<string> GetNotesFromCurrentUserGroups()
        {
            List<string> result = new List<string>();
            List<string> groups = GetCurrentUserGroups();

            foreach (string group in groups)
            {
                DirectorySearcher ds = new DirectorySearcher
                {
                    Filter = $"(&(objectClass=group)(sAMAccountName={group}))"
                };

                SearchResult sr = ds.FindOne();

                DirectoryEntry groupEntry = sr.GetDirectoryEntry();
                groupEntry.RefreshCache(new string[] { "info" });

                string res = (string)groupEntry.Properties["info"].Value;
                
                if (res != null)
                {
                    result.Add(res.Trim());
                }

                ds.Dispose();
                groupEntry.Close();
                groupEntry.Dispose();
            }

            return result;
        }
    }
}
