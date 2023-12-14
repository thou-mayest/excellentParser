using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            
            if (GetColumns(args).Count() == 1 && GetColumns(args)[0] == "*")
                return _fileservice.ReadFile(GetPath(args));

            string[] FileColumnNames = _fileservice.ReadFileLines(GetPath(args)).First().Split(',');
            var reqColumns = GetColumns(args);

            List<int> indexes = new ();


            foreach(string n in reqColumns)
            {
                if(Array.IndexOf(FileColumnNames,n) != -1)
                {
                    indexes.Add(Array.IndexOf(FileColumnNames,n));
                }
            }


            List<string> lines = _fileservice.ReadFileLines(GetPath(args)).ToList();
            List<string> result = new();

            foreach (string line in lines)
            {
                string temp = "";
                foreach(int i in indexes)
                {
                    temp+=',';
                    temp+=line.Split(',')[i];
                }

                result.Add(temp);
            }

            return String.Join(",", result);
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
