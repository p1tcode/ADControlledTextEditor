using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    public class GroupInfo
    {
        public string Name { get; set; }
        public string Notes { get; set; }

        public GroupInfo()
        {
            Name = "";
            Notes = "";
        }

        /// <summary>
        /// Converts all notes information into single lines of text.
        /// </summary>
        /// <returns>A list of individual lines of text.</returns>
        public List<string> NotesAsIndividualStrings()
        {
            string[] parts = Notes.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            List<string> result = new List<string>();

            foreach (string part in parts)
            {
                if (part != "")
                {
                    result.Add(part.Trim());
                }
            }

            return result;
        }
    }
}
