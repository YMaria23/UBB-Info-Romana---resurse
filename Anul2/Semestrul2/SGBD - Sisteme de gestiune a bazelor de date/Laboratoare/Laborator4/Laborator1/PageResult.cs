using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laborator1
{
    internal class PageResult <T>
    {
        private List<T> items;
        private int total;
        public PageResult(List<T> items, int total){
            Items = items;
            TotalCount = total;

        }

        public List<T> Items { get; private set; }
        public int TotalCount { get; private set; }
        
    }
}
