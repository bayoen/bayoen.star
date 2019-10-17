using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bayoen.star.Variables
{
    public class DownloadData
    {
        public DownloadData()
        {
            this.UpdatingNow = false;
        }

        public bool UpdatingNow { get; set; }
        public List<string> FilePaths { get; set; }
        public List<long> FileLengths { get; set; }
        public int FileIndex { get; set; }
        public int FileMax { get; set; }
        public int Trial { get; set; }
    }
}
