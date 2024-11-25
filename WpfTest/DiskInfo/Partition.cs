using System;
using System.Text;

namespace WpfTest.DiskInfo
{
    class Partition
    {
        public PhysDisk PhysDisk { get; set; }
        
        public Volume Volume { get; set; }

        public String Name { get; set; }

        public UInt64 Size { get; set; }
        
        public String Caption { get; set; }

        public String Description { get; set; }

        public UInt32 DiskIndex { get; set; }              

        public Boolean BootPartition { get; set; }

        public Boolean Bootable { get; set; }
        
        public UInt64 BlockSize { get; set; }
        
        public UInt64 NumberOfBlocks { get; set; }
        
        public Boolean PrimaryPartition { get; set; }
        
        public UInt64 StartingOffset { get; set; }
        
        public String Type { get; set; }
                
        public Boolean HasVolume
        {
            get { return Volume != null; }
        }

        public Double SizeGB
        {
            get { return (Double)Size / 1024 / 1024 / 1024; }
        }

        public String AsString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendFormat("Name: {0}\n", Name);
            if (PrimaryPartition)
                strBuilder.AppendLine("Primary partition");
            if (BootPartition)
                strBuilder.AppendLine("Boot partition");
            if (Bootable)
                strBuilder.AppendLine("Bootable");
            strBuilder.AppendFormat("Disk index: {0}\n", DiskIndex);
            strBuilder.AppendFormat("Type: {0}\n", Type);
            strBuilder.AppendFormat("Description: {0}\n", Description);
            strBuilder.AppendFormat("Caption: {0}\n", Caption);    
            strBuilder.AppendFormat("Size: {0} bytes\n", Size.ToString("#,##0"));
            strBuilder.AppendFormat("Block size: {0} bytes\n", BlockSize);
            strBuilder.AppendFormat("Number of blocks: {0}\n", NumberOfBlocks.ToString("#,##0"));
            strBuilder.AppendFormat("Starting offset: {0}\n", StartingOffset);

            return strBuilder.ToString();
        }
    }
}
