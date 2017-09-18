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
    public class ActiveDirectoryData
    {
        public string UserName { get; }

        public ActiveDirectoryData()
        {
            this.UserName = GetCurrentUserName();
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

            LogFile.WriteLine("Gather Current User Groups: ");
            LogFile.WriteLine("---------------------------");

            using (DirectorySearcher ds = new DirectorySearcher())
            {
                ds.Filter = $"(&(objectClass=user)(sAMAccountName={this.UserName}))";

                SearchResult sr = ds.FindOne();

                DirectoryEntry userEntry = sr.GetDirectoryEntry();
                userEntry.RefreshCache(new string[] { "tokenGroups" });

                for (int i = 0; i < userEntry.Properties["tokenGroups"].Count; i++)
                {
                    SecurityIdentifier sid = new SecurityIdentifier((byte[])userEntry.Properties["tokenGroups"][i], 0);
                    NTAccount nt = (NTAccount)sid.Translate(typeof(NTAccount));

                    string groupName = nt.Value.Split('\\')[1];

                    result.Add(groupName);
                    LogFile.WriteLine($"Group found: { groupName }");
                }
                
                userEntry.Close();
                userEntry.Dispose();
            }

            LogFile.Seperator();

            return result;
        }


        /// <summary>
        /// Returns the Notes (info) field of all the groups the users is a member of
        /// </summary>
        /// <param name="groups">A list of AD groups to gather the notes field from.</param>
        /// <returns>List of string with Notes from AD group</returns>
        public List<string> GetNotesFromCurrentUserGroups(List<string> groups)
        {
            List<string> result = new List<string>();

            LogFile.WriteLine("Gather Notes field from all groups of the current User: ");
            LogFile.WriteLine("-------------------------------------------------------");

            foreach (string group in groups)
            {
                using (DirectorySearcher ds = new DirectorySearcher())
                {
                    ds.Filter = $"(&(objectClass=group)(sAMAccountName={group}))";

                    SearchResult sr = ds.FindOne();

                    DirectoryEntry groupEntry = sr.GetDirectoryEntry();
                    groupEntry.RefreshCache(new string[] { "info" });

                    string res = (string)groupEntry.Properties["info"].Value;

                    if (res != null)
                    {
                        result.Add(res.Trim());
                        LogFile.WriteLine($"Notes data found: { res.Trim() }");
                    }

                    groupEntry.Close();
                    groupEntry.Dispose();
                } 
            }

            LogFile.Seperator();

            return result;
        }
    }
}
