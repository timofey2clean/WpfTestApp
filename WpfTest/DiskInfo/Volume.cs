using System;
using System.Text;

namespace WpfTest.DiskInfo
{
    class Volume
    {
        public Partition Partition { get; set; }

        public UInt32 Index { get; set; }

        public UInt32 DiskIndex { get; set; }

        public String Name { get; set; }

        public String Caption { get; set; }
                
        public String DeviceID { get; set; }
        
        public UInt64 Size { get; set; }

        public Double SizeGB { get { return (Double)Size / 1024 / 1024 / 1024; } }

        public UInt64 FreeSpace { get; set; }

        public Double FreeSpaceGB { get { return (Double)FreeSpace / 1024 / 1024 / 1024; } }
        
        public Double FreePrcnt
        {
            get
            {
                if (Size == 0)
                    return 0;

                return (Double)FreeSpace / Size * 100;
            }
        }

        public String FileSystem { get; set; }
        
        public UInt32 DriveType { get; set; }

        public String DriveTypeString
        {
            get
            {
                switch (DriveType)
                {
                    case 1:
                        {
                            return "No root directory";
                        }
                    case 2:
                        {
                            return "Removable drive";
                        }
                    case 3:
                        {
                            return "Local hard disk";
                        }
                    case 4:
                        {
                            return "Network disk";
                        }
                    case 5:
                        {
                            return "Compact disk";
                        }
                    case 6:
                        {
                            return "RAM disk";
                        }
                    default:
                        {
                            return "Drive type could not be determined";
                        }
                }
            }
        }

        public Boolean Compressed { get; set; }

        public PhysDisk PhysDisk { get; set; }

        public String Description { get; set; }

        public UInt32 MediaType { get; set; }

        public Boolean SupportsFileBasedCompression { get; set; }

        public Boolean VolumeDirty { get; set; }

        public String VolumeName { get; set; }

        public String VolumeSerialNumber { get; set; }

        public String AsString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendFormat("Name: {0}\n", Name);
            strBuilder.AppendFormat("VolumeName: {0}\n", VolumeName);
            strBuilder.AppendFormat("Description: {0}\n", Description);
            strBuilder.AppendFormat("Caption: {0}\n", Caption);
            strBuilder.AppendFormat("DeviceID: {0}\n", DeviceID);
            strBuilder.AppendFormat("DriveType: {0}\n", DriveTypeString);  
            strBuilder.AppendFormat(Compressed ? "Compressed\n" : "Not compressed\n");
            strBuilder.AppendFormat("FileSystem: {0}\n", FileSystem);
            strBuilder.AppendFormat("Free: {0} bytes\n", FreeSpace.ToString("#,###,##0"));
            strBuilder.AppendFormat("Size: {0} bytes\n", Size.ToString("#,###,##0"));
            strBuilder.AppendFormat("MediaType: {0}\n", MediaType);
            strBuilder.AppendFormat("VolumeSerialNumber: {0}\n", VolumeSerialNumber);

            return strBuilder.ToString();
        }
    }
}
