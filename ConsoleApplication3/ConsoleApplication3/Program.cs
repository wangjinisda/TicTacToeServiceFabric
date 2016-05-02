using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    class SearchFilter
    {
        public string Name { get; set; }
        public string RequestStatus { get; set; }
    }

    class SearchFilters : KeyedCollection<SearchFilter>
    {
        public string DefaultSearchFilter { get; set; }
    }
}
