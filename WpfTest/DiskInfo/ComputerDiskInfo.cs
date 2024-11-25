using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using WpfTest.Common;

namespace WpfTest.DiskInfo
{
    class ComputerDiskInfo
    {
        public DiskInfoSpec GetAllDisks()
        {
            IEnumerable<PhysDisk> physDisks = GetPhysDisks();
            IEnumerable<Partition> partitions = GetPartitions();
            IEnumerable<Volume> volumes = GetVolumes();

            LinkPartitionsToDisks(physDisks, partitions);
            LinkVolumesToDisks(physDisks, volumes);
            LinkVolumesToPartitions(volumes, partitions);

            return new DiskInfoSpec { Volumes = volumes, Patitions = partitions, PhysDisks = physDisks };
        }

        private static ManagementObjectCollection GetClassObjects(String className)
        {
            ManagementScope namespaceScope = new ManagementScope("\\\\.\\ROOT\\CIMV2");
            ObjectQuery diskQuery = new ObjectQuery("SELECT * FROM " + className);
            ManagementObjectSearcher mgmtObjSearcher = new ManagementObjectSearcher(namespaceScope, diskQuery);

            return mgmtObjSearcher.Get();
        }

        private IEnumerable<PhysDisk> GetPhysDisks()
        {
            List<PhysDisk> physDisks = new List<PhysDisk>();
            foreach (ManagementObject o in GetClassObjects("Win32_DiskDrive"))
            {
                physDisks.Add(CManagementObjectParser.GetPhysDiskOptions(o));
            }

            return physDisks.OrderBy(_ => _.Index);
        }

        private IEnumerable<Volume> GetVolumes()
        {
            List<Volume> volumes = new List<Volume>();
            foreach (ManagementBaseObject o in GetClassObjects("Win32_LogicalDisk"))
            {
                volumes.Add(CManagementObjectParser.GetVolumeOptions(o));
            }
            
            return volumes.OrderBy(_ => _.Name);
        }

        private IEnumerable<Partition> GetPartitions()
        {
            List<Partition> partitions = new List<Partition>();
            foreach (ManagementBaseObject o in GetClassObjects("Win32_DiskPartition"))
            {
                partitions.Add(CManagementObjectParser.GetPartitionOptions(o));
            }

            return partitions.OrderBy(_ => _.Name);
        }

        private void LinkPartitionsToDisks(IEnumerable<PhysDisk> disks, IEnumerable<Partition> partitions)
        {
            foreach (PhysDisk disk in disks)
            {
                foreach (Partition partition in partitions)
                {
                    if (disk.Index == partition.DiskIndex)
                    {
                        partition.PhysDisk = disk;
                        disk.Partitions.Add(partition);
                    }
                }
            }
        }

        private void LinkVolumesToPartitions(IEnumerable<Volume> volumes, IEnumerable<Partition> partitions)
        {
            foreach (Volume volume in volumes)
            {
                if (String.IsNullOrEmpty(volume.Name))
                    continue;

                String driveLetter = volume.Name.Trim('\\');
                String queryString = String.Format("ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID='{0}'}} WHERE ResultClass=Win32_DiskPartition", driveLetter);
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(queryString);
                ManagementObjectCollection diskPartitionCollection = searcher.Get();

                foreach (ManagementBaseObject mo in diskPartitionCollection)
                {
                    String partitionName = mo["Name"].ToString();
                    Partition partition = partitions.FirstOrDefault(_ => String.Equals(_.Name, partitionName));
                    if (!ReferenceEquals(null, partition))
                    {
                        partition.Volume = volume;
                        volume.Partition = partition;                      
                    }
                }
            }
        }

        private void LinkVolumesToDisks(IEnumerable<PhysDisk> disks, IEnumerable<Volume> volumes)
        {
            foreach (Volume volume in volumes)
            {
                if (String.IsNullOrEmpty(volume.Name))
                    continue;

                String driveName = volume.Name.Trim('\\');
                String queryString = String.Format("ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID='{0}'}} WHERE ResultClass=Win32_DiskPartition", driveName);
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(queryString);
                ManagementObjectCollection diskPartitionCollection = searcher.Get();

                foreach (ManagementBaseObject mo in diskPartitionCollection)
                {
                    String partitionDiskIndex = mo["DiskIndex"].ToString().Trim();
                    UInt32 diskIndex = Convert.ToUInt32(partitionDiskIndex);
                    volume.PhysDisk = disks.FirstOrDefault(_ => _.Index == diskIndex);
                }
            }
        }

        class CManagementObjectParser
        {
            public static PhysDisk GetPhysDiskOptions(ManagementBaseObject moDisk)
            {
                String objName = "physical disk";
                PhysDisk disk = new PhysDisk();
                disk.Name = GetOptionSafely<String>(moDisk, "Name", objName);
                if (!String.IsNullOrEmpty(disk.Name))
                    objName = "physical disk " + disk.Name;
                disk.Model = GetOptionSafely<String>(moDisk, "Model", objName);
                disk.InterfaceType = GetOptionSafely<String>(moDisk, "InterfaceType", objName);
                disk.Size = GetOptionSafely<UInt64>(moDisk, "Size", objName);
                disk.PartitionCount = GetOptionSafely<UInt32>(moDisk, "Partitions", objName);
                disk.Signature = GetOptionSafely<UInt32>(moDisk, "Signature", objName);
                disk.TotalCylinders = GetOptionSafely<UInt64>(moDisk, "TotalCylinders", objName);
                disk.TotalSectors = GetOptionSafely<UInt64>(moDisk, "TotalSectors", objName);
                disk.TotalHeads = GetOptionSafely<UInt32>(moDisk, "TotalHeads", objName);
                disk.TotalTracks = GetOptionSafely<UInt64>(moDisk, "TotalTracks", objName);
                disk.BytesPerSector = GetOptionSafely<UInt32>(moDisk, "BytesPerSector", objName);
                disk.SectorsPerTrack = GetOptionSafely<UInt32>(moDisk, "SectorsPerTrack", objName);
                disk.TracksPerCylinder = GetOptionSafely<UInt32>(moDisk, "TracksPerCylinder", objName);
                disk.Index = GetOptionSafely<UInt32>(moDisk, "Index", objName);

                return disk;
            }

            public static Volume GetVolumeOptions(ManagementBaseObject moDisk)
            {
                String objName = "volume";

                Volume volume = new Volume();
                volume.Name = GetOptionSafely<String>(moDisk, "Name", objName);
                if (!String.IsNullOrEmpty(volume.Name))
                    objName = "volume " + volume.Name;
                volume.DeviceID = GetOptionSafely<String>(moDisk, "DeviceID", objName);
                volume.FileSystem = GetOptionSafely<String>(moDisk, "FileSystem", objName);
                volume.Size = GetOptionSafely<UInt64>(moDisk, "Size", objName);
                volume.FreeSpace = GetOptionSafely<UInt64>(moDisk, "FreeSpace", objName);
                volume.Caption = GetOptionSafely<String>(moDisk, "Caption", objName);
                volume.DriveType = GetOptionSafely<UInt32>(moDisk, "DriveType", objName);
                volume.Compressed = GetOptionSafely<Boolean>(moDisk, "Compressed", objName);
                volume.Description = GetOptionSafely<String>(moDisk, "Description", objName);
                volume.MediaType = GetOptionSafely<UInt32>(moDisk, "MediaType", objName);
                volume.SupportsFileBasedCompression = GetOptionSafely<Boolean>(moDisk, "SupportsFileBasedCompression", objName);
                volume.VolumeDirty = GetOptionSafely<Boolean>(moDisk, "VolumeDirty", objName);
                volume.VolumeName = GetOptionSafely<String>(moDisk, "VolumeName", objName);
                volume.VolumeSerialNumber = GetOptionSafely<String>(moDisk, "VolumeSerialNumber", objName);

                return volume;
            }

            public static Partition GetPartitionOptions(ManagementBaseObject mo)
            {                
                String objName = "partition";

                Partition partition = new Partition();
                partition.Name = GetOptionSafely<String>(mo, "Name", objName);
                if (!String.IsNullOrEmpty(partition.Name))
                    objName = "partition " + partition.Name;
                partition.BlockSize = GetOptionSafely<UInt64>(mo, "BlockSize", objName);
                partition.BootPartition = GetOptionSafely<Boolean>(mo, "BootPartition", objName);
                partition.Bootable = GetOptionSafely<Boolean>(mo, "Bootable", objName);
                partition.Caption = GetOptionSafely<String>(mo, "Caption", objName);
                partition.Description = GetOptionSafely<String>(mo, "Description", objName);
                partition.DiskIndex = GetOptionSafely<UInt32>(mo, "DiskIndex", objName);
                partition.NumberOfBlocks = GetOptionSafely<UInt64>(mo, "NumberOfBlocks", objName);
                partition.PrimaryPartition = GetOptionSafely<Boolean>(mo, "PrimaryPartition", objName);
                partition.Size = GetOptionSafely<UInt64>(mo, "Size", objName);
                partition.StartingOffset = GetOptionSafely<UInt64>(mo, "StartingOffset", objName);
                partition.Type = GetOptionSafely<String>(mo, "Type", objName);

                return partition;
            }

            private static T GetOptionSafely<T>(ManagementBaseObject mo, String optionName, String objectName)
            {
                String objNameMsg = String.IsNullOrEmpty(objectName) ? String.Empty : String.Format(" of {0}", objectName);

                try
                {                    
                    return (T)mo[optionName];
                }
                catch (NullReferenceException)
                {
                    Log.Inst().Message("Property{0} {1} is not set.", objNameMsg, optionName);
                }
                catch (InvalidCastException)
                {
                    Log.Inst().Message("Failed to convert option{0} '{1}' to {2}.", objNameMsg, mo[optionName], typeof(T));
                }
                catch (Exception ex)
                {
                    Log.Inst().Message("Failed to get option{0} '{1}'. {2}.", objNameMsg, optionName, ex.Message);
                }

                return default(T);
            }
        }
    }
}
