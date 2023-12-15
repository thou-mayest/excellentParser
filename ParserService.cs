using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            
            if (GetColumns(args).Count() == 1 && GetColumns(args)[0] == "*" && query.IndexOf("WHERE") == -1)
                return _fileservice.ReadFile(GetPath(args));

            string[] FileColumnNames = _fileservice.ReadFileLines(GetPath(args)).First().Split(',');
            
            string[] reqColumns = GetColumns(args);
            List<int> indexes = new ();


            foreach(string n in reqColumns)
            {
                if(Array.IndexOf(FileColumnNames,n) != -1)
                {
                    indexes.Add(Array.IndexOf(FileColumnNames,n));
                }
            }


            List<string> lines = _fileservice.ReadFileLines(GetPath(args)).ToList();
            lines.RemoveAt(0);
            
            foreach(string l in lines)
            {
                l.Replace("\"",string.Empty);
            }
           
            //filter
            lines = Filter(args,lines,FileColumnNames);

            JObject jsonObject = new JObject();
            JObject row = new JObject();

            //  foreach (int i in indexes)
            // {
            //     JArray values = new();
            //     foreach(string l in lines)
            //     {
            //         //values.Add(l.Split(',')[i]);
            //         jsonObject.Add(FileColumnNames[i],l.Split(',')[i]);
            //     }
            //     //jsonObject.Add(FileColumnNames[i],values);
                
            // }


            int rowId=0;
            foreach(string line in lines)
            {
                
                JArray values = new ();
                row = new JObject();
                foreach(int i in indexes)
                {
                    row.Add(FileColumnNames[i],line.Split(',')[i]);
                }
                jsonObject.Add(rowId.ToString(),row);
                rowId++;
            }

            return jsonObject.ToString();
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


        List<string> Filter(string[] args,List<string> lines,string[] FileColumns)
        {
            var indexOfCond = Array.IndexOf(args,"WHERE");
            indexOfCond = indexOfCond == -1 ? Array.IndexOf(args,"where") : indexOfCond;
            
            var result = new List<string>();

            if (indexOfCond == -1 )
                return lines;

            var ColumnIndex = Array.IndexOf(FileColumns,args[indexOfCond+1]);
            var columnValue = args[indexOfCond+3];

            //condition = if line(columnKey) == conditionVal 

            //loop remove if condition
            var count = lines.Count;

            if(args[indexOfCond+2].Contains("=="))
            {
                for(int i=0;i<count;i++)
                {
                    if (lines[i].Split(',')[ColumnIndex] == columnValue)
                    {
                        result.Add(lines[i]);
                    }
                }
            }

            if(args[indexOfCond+2].Contains("!="))
            {
                for(int i=0;i<count;i++)
                {
                    if (lines[i].Split(',')[ColumnIndex] != columnValue)
                    {
                        result.Add(lines[i]);
                    }
                }
            }
            

            return result;
        }
    }
}
