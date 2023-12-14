using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace excellent_parser
{
    internal class FileService : IFileService
    {
        public IEnumerable<string> ReadFileLines(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            return lines.AsEnumerable<string>() ;
        }

        public string ReadFile(string fileName)
        {
            string content = File.ReadAllText(fileName);
            return content;
        }
    }
}
