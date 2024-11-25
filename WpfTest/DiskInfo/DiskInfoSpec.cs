using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfTest.DiskInfo
{
    class DiskInfoSpec
    {
        public IEnumerable<PhysDisk> PhysDisks { get; set; }

        public IEnumerable<Partition> Patitions { get; set; }

        public IEnumerable<Volume> Volumes { get; set; }
    }
}
