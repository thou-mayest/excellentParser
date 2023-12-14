using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace excellent_parser
{
    internal class ParserService : ICSVService
    {
        private readonly IFileService _fileservice;
        public ParserService(IFileService fileService) 
        {
            _fileservice = fileService;
        }


        public string Query(string query)
        {
            string[] args = query.Split(' ');
            Console.WriteLine(GetPath(args));

            string[] columns = _fileservice.ReadFileLines(GetPath(args)).First().Split(',');
            string result ="";
            
            return result;
        }

        string GetPath (string[] args)
        {
            
            var indexOfPath = Array.IndexOf(args,"FROM");
            indexOfPath = indexOfPath == -1 ? Array.IndexOf(args,"from") : indexOfPath;

            return args[indexOfPath+1];
        }

        string[] GetColumns(string[] args)
        {

            var indexOfColumn = Array.IndexOf(args,"SELECT");
            indexOfColumn = indexOfColumn == -1 ? Array.IndexOf(args,"select") : indexOfColumn;
            var columns =  args[indexOfColumn+1].Split(',');
            return columns;
        }

        string GetCondition(string[] args)
        {
            return "";
        }
    }
}
