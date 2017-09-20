using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    public class FileInfo
    {
        public string GroupOwnerName { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public FileInfo()
        {
            GroupOwnerName = "";
            Name = "";
            Path = "";
        }
    }
}
