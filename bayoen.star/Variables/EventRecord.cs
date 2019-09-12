using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bayoen.star.Variables
{
    public class EventRecord
    {
        public int ID { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }

        public List<MatchRecord> Matches { get; set; }
    }
}
