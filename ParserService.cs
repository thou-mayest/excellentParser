using System;
using System.Collections.Generic;
using System.Drawing;
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

            string[] FileColumnNames = _fileservice.ReadFileLines(GetPath(args)).Where(x => x.Contains(',')).First().Split(',');

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("columns:" + string.Join(" , ",FileColumnNames));
            Console.ForegroundColor = ConsoleColor.White;

            string[] reqColumns = GetColumns(args);

            if (GetColumns(args).Count() == 1 && GetColumns(args)[0] == "*")
                reqColumns =  _fileservice.ReadFileLines(GetPath(args)).First().Split(',');

            List<int> indexes = new ();


            foreach(string n in reqColumns)
            {
                if(Array.IndexOf(FileColumnNames,n) != -1)
                {
                    indexes.Add(Array.IndexOf(FileColumnNames,n));
                }
            }


            List<string> lines = _fileservice.ReadFileLines(GetPath(args)).Where(x => x.Contains(',')).ToList();
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
            var indexOfColumn = Array.IndexOf(args, "SELECT") != -1 ? Array.IndexOf(args, "SELECT")  : Array.IndexOf(args, "select");
            var indexOfFrom = Array.IndexOf(args, "FROM") != -1 ? Array.IndexOf(args, "FROM") : Array.IndexOf(args, "from");
            indexOfColumn = indexOfColumn == -1 ? Array.IndexOf(args, "select") : indexOfColumn;
            var columns = args.Skip(indexOfColumn+1).Take(indexOfFrom - indexOfColumn -1).ToArray();
            string result = string.Join(" ", columns);
            return result.Split(',') ;
        }


        List<string> Filter(string[] args,List<string> lines,string[] FileColumns)
        {
            var indexOfCond = Array.IndexOf(args,"WHERE");
            indexOfCond = indexOfCond == -1 ? Array.IndexOf(args,"where") : indexOfCond;
            
            var result = new List<string>();

            if (indexOfCond == -1 )
                return lines;
                
            string[] fullClauseArray = args.Skip(indexOfCond+1).Take(args.Length - indexOfCond - 1).ToArray();

            string fullClauseString = string.Join(" ",fullClauseArray);
                    
            //var ColumnIndex = Array.IndexOf(FileColumns,args[indexOfCond+1]);

            //var columnValue = args[indexOfCond+3];
            
            //condition = if line(columnKey) == conditionVal 

            //loop remove if condition
            var count = lines.Count;

            if(fullClauseString.Contains("=="))
            {
                string[] Clause = fullClauseString.Split("==");

                var ColumnIndex = Array.IndexOf(FileColumns,Clause[0].Trim(' '));
                var columnValue = Clause[1].Trim(' ');

                for(int i=0;i<count;i++)
                {
                    if (lines[i].Split(',')[ColumnIndex].Trim(' ').Contains(columnValue))
                    {
                        result.Add(lines[i]);
                    }
                }
            }

            if(fullClauseString.Contains("!="))
            {
                string[] Clause = fullClauseString.Split("!=");

                var ColumnIndex = Array.IndexOf(FileColumns,Clause[0].Trim(' '));
                var columnValue = Clause[1].Trim(' ');

                

                for(int i=0;i<count;i++)
                {

                    if (lines[i].Split(',')[ColumnIndex].Trim(' ') != columnValue)
                    {
                        result.Add(lines[i]);
                    }
                }
            }
            

            return result;
        }
    }
}
