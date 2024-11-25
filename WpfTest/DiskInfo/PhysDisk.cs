using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfTest.DiskInfo
{
    class PhysDisk
    {
        private readonly List<Partition> _partitions = new List<Partition>();

        public List<Partition> Partitions
        {
            get
            {
                return _partitions;
            }
        }

        public UInt32 Index { get; set; }

        public String Name { get; set; }

        public String Model { get; set; }

        public String InterfaceType { get; set; }

        public UInt64 Size { get; set; }

        public Double SizeGB { get { return (Double)Size / 1024 / 1024 / 1024; } }

        public UInt32 PartitionCount { get; set; }

        public UInt32 Signature { get; set; }
        
        public UInt64 TotalCylinders { get; set; }

        public UInt64 TotalSectors { get; set; }

        public UInt64 TotalHeads { get; set; }

        public UInt64 TotalTracks { get; set; }

        public UInt32 BytesPerSector { get; set; }

        public UInt32 SectorsPerTrack { get; set; }

        public UInt32 TracksPerCylinder { get; set; }

        public String AsString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendFormat("Index: {0}\n", Index);
            strBuilder.AppendFormat("Name: {0}\n", Name);
            strBuilder.AppendFormat("Model: {0}\n", Model);
            strBuilder.AppendFormat("InterfaceType: {0}\n", InterfaceType);
            strBuilder.AppendFormat("Size: {0} bytes\n", Size.ToString("#,###,##0"));
            strBuilder.AppendFormat("Partition count: {0}\n", PartitionCount);
            strBuilder.AppendFormat("Signature: {0}\n", Signature);
            strBuilder.AppendFormat("TotalCylinders: {0}\n", TotalCylinders);
            strBuilder.AppendFormat("TotalSectors: {0}\n", TotalSectors);
            strBuilder.AppendFormat("TotalHeads: {0}\n", TotalHeads);
            strBuilder.AppendFormat("TotalTracks: {0}\n", TotalTracks);
            strBuilder.AppendFormat("BytesPerSector: {0}\n", BytesPerSector);
            strBuilder.AppendFormat("SectorsPerTrack: {0}\n", SectorsPerTrack);
            strBuilder.AppendFormat("TracksPerCylinder: {0}\n", TracksPerCylinder);

            return strBuilder.ToString();
        }
    }
}
