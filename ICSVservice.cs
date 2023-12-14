using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace excellent_parser
{
    public interface ICSVService
    {
        // Returns a JSON string that contains an array of objects, where each 
        // object represents a row of the CSV file
        // The keys are the column names and the values are the column values
        // The query parameter is a string that follows the SQL-like syntax
        // For example: "SELECT name, age FROM users.csv WHERE age > 20"
        string Query(string query);
    }
}
