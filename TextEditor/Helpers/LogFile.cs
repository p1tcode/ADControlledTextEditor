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
        

        /// <summary>
        /// Creates a file and readies it for writing.
        /// </summary>
        /// <param name="_fileName"></param>
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

        /// <summary>
        /// Write a line of text to the logfile.
        /// </summary>
        /// <param name="text"></param>
        static public void WriteLine(string text)
        {
            WriteLine(text, false);
        }


        /// <summary>
        /// Write a line of text to the logfile and prefix the line with a timestamp.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="timeStamp"></param>
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

        /// <summary>
        /// Add a seperator line to the logfile.
        /// </summary>
        static public void Seperator()
        {
            writer.WriteLine("");
        }


        /// <summary>
        /// Close the StreamWriter for the logfile.
        /// </summary>
        static public void Close()
        {
            writer.Close();
            writer.Dispose();
        }
    }
}
