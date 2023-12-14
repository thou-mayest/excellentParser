using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace excellent_parser
{
    public interface IFileService
    {
        IEnumerable<string> ReadFileLines(string fileName);
        string ReadFile(string fileName);
    }
}
