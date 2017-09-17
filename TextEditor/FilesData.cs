using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace TextEditor
{
    class FilesData
    {

        private string fileMask = "ADCTE_PATH";

        public Dictionary<string, string> FilesList { get; set; }
        
        public FilesData()
        {
            FilesList = new Dictionary<string, string>();
        }


        /// <summary>
        /// Populates the fileslist with files the user is allowed to open.
        /// </summary>
        /// <param name="notesDataFromAD">Raw notes data from AD</param>
        public void PopulateFilesList(List<string> notesDataFromAD)
        {
            List<string> notes = this.ParseNotes(notesDataFromAD);

            foreach (string note in notes)
            {
                string[] parts = note.Split(';');
                if (parts.Length == 3)
                {
                    if (parts[0] == fileMask)
                    {
                        if (!FilesList.ContainsKey(parts[1]))
                        {
                            FilesList.Add(parts[1], parts[2]);
                        }
                        else
                        {
                            // TODO: ADD LOGGING 
                            // Error message that the Name already exists in the list.
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Parse through the raw AD Notes data.
        /// Make sure the list contains information and all data is split into individiual lines.
        /// </summary>
        /// <param name="notesDataFromAD">Raw notes data from AD</param>
        /// <returns></returns>
        private List<string> ParseNotes(List<string> notesDataFromAD)
        {
            List<string> notes = new List<string>();

            foreach (string n in notesDataFromAD)
            {
                string[] parts = n.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (string part in parts)
                {
                    if (part != "")
                    {
                        notes.Add(part.Trim());
                    }
                }
            }

            return notes;
        }
    }
}
