using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TextEditor
{
    public static class LogFile
    {
        static string fileName = "";
        static StreamWriter writer;
        
        static public void Initialize(string _fileName)
        {
            fileName = _fileName;

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            writer = new StreamWriter(fileName, true)
            {
                AutoFlush = true
            };
        }

        static public void WriteLine(string text)
        {
            WriteLine(text, false);
        }

        static public void WriteLine(string text, bool timeStamp)
        {
            if (File.Exists(fileName))
            {
                if (timeStamp)
                {
                    writer.WriteLine($"{ DateTime.Now.ToString() }: { text }");
                }
                else
                {
                    writer.WriteLine(text);
                }
                
            }   
        }

        static public void Seperator()
        {
            writer.WriteLine("");
        }

        static public void Close()
        {
            writer.Close();
            writer.Dispose();
        }
    }
}
