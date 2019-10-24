using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bayoen.star.Variables
{
    public class MiniData
    {
        public MiniData()
        {
            this.Stars = new List<int>(4) { 0, 0, 0, 0 };
            this.Games = new List<int>(4) { 0, 0, 0, 0 };
        }

        public List<int> Stars { get; set; }
        public List<int> Games { get; set; }

        public void Reset()
        {
            this.Stars = new List<int>(4) { 0, 0, 0, 0 };
            this.Games = new List<int>(4) { 0, 0, 0, 0 };
        }
    }
}
