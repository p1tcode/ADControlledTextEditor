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
    public class ADDataManager
    {
        private string userName;
        public string UserName
        {
            get { return userName; }
        }

        private string domainName;
        public string DomainName
        {
            get { return domainName; }
        }

        private List<FileInfo> files;
        public List<FileInfo> Files
        {
            get { return files; }
        }

        private List<GroupInfo> groups;
        public List<GroupInfo> Groups
        {
            get { return groups; }
        }


        private string fileMask = "ADCTE_PATH";


        public ADDataManager()
        {
            files = new List<FileInfo>();
            groups = new List<GroupInfo>();
            GetCurrentUserInfo();

            LogFile.WriteLine($"UserName: { UserName }");
            LogFile.WriteLine($"Domain: { DomainName }");
            LogFile.Seperator();
        }

        /// <summary>
        /// Returns a string with the current username.
        /// </summary>
        /// <returns></returns>
        private void GetCurrentUserInfo()
        {
            string[] userNamePart = WindowsIdentity.GetCurrent().Name.Split('\\');
            if (userNamePart.Length > 1)
            {
                userName = userNamePart[1];
                domainName = userNamePart[0];
            }
            else
            {
                this.userName = userNamePart[0];
            }
        }


        /// <summary>
        /// Gets all the groups a user is member of and populates the Files list with all the groups.
        /// </summary>
        public void GetCurrentUserGroups()
        {
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

                    groups.Add(new GroupInfo() { Name = groupName });
                    LogFile.WriteLine($"Group found: { groupName }");
                }
                
                userEntry.Close();
                userEntry.Dispose();
            }

            LogFile.Seperator();

        }


        /// <summary>
        /// Returns the Notes (info) field of all the groups the users is a member of.
        /// </summary>
        public void GetNotesFromCurrentUserGroups()
        {

            LogFile.WriteLine("Gather Notes field from all groups of the current User: ");
            LogFile.WriteLine("-------------------------------------------------------");

            foreach (GroupInfo group in Groups)
            {
                using (DirectorySearcher ds = new DirectorySearcher())
                {
                    ds.Filter = $"(&(objectClass=group)(sAMAccountName={ group.Name }))";

                    SearchResult sr = ds.FindOne();

                    DirectoryEntry groupEntry = sr.GetDirectoryEntry();
                    groupEntry.RefreshCache(new string[] { "info" });

                    string res = (string)groupEntry.Properties["info"].Value;

                    if (res != null)
                    {
                        group.Notes = res.Trim();
                        LogFile.WriteLine($"Notes data found in group: { group.Name }");
                        LogFile.WriteLine(group.Notes);
                        LogFile.Seperator();
                    }

                    groupEntry.Close();
                    groupEntry.Dispose();
                } 
            }

            LogFile.Seperator();
        }

        /// <summary>
        /// Populates the fileslist with files the user is allowed to open.
        /// </summary>
        public void ConvertNotesToFileData()
        {
            LogFile.WriteLine("Finding allowed files from Notes data:");
            LogFile.WriteLine("--------------------------------------");

            

            foreach (GroupInfo group in Groups)
            {

                List<string> notes = group.NotesAsIndividualStrings();


                foreach (string note in notes)
                {
                    string[] parts = note.Split(';');
                    if (parts.Length == 3)
                    {
                        if (parts[0] == fileMask)
                        {
                            files.Add(new FileInfo() { FileName = parts[1], Path = parts[2], GroupOwnerName = group.Name });
                            LogFile.WriteLine($"Adding file: { parts[1] } , {parts[2] } from Group: { group.Name }");
                        }
                    }
                }
                
            }
        }
    }
}
