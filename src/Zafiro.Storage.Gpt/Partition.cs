﻿using System;
using ByteSizeLib;

namespace Zafiro.Storage.Windows.Gpt
{
    public class Partition
    {
        private readonly uint bytesPerSector;
        private ulong firstSector;
        private ulong lastSector;
        private ulong sizeInSectors;

        public Partition(string name, GptType gptType, uint bytesPerSector)
        {
            GptType = gptType;
            Name = name;
            this.bytesPerSector = bytesPerSector;
        }

        public ulong Attributes { get; set; }
        public Guid Guid { get; set; }
        public GptType GptType { get; set; }
        public string Name { get; }

        private ByteSize Size => new ByteSize(SizeInSectors * bytesPerSector);

        private ulong SizeInSectors
        {
            get
            {
                if (sizeInSectors != 0)
                {
                    return sizeInSectors;
                }

                return LastSector - FirstSector + 1;
            }
            set
            {
                sizeInSectors = value;
                if (FirstSector != 0)
                {
                    LastSector = FirstSector + sizeInSectors - 1;
                }
            }
        }

        public ulong FirstSector // 0x08
        {
            get => firstSector;
            set
            {
                firstSector = value;
                if (sizeInSectors != 0)
                {
                    lastSector = FirstSector + sizeInSectors - 1;
                }
            }
        }

        public ulong LastSector // 0x08
        {
            get => lastSector;
            set
            {
                lastSector = value;
                sizeInSectors = 0;
            }
        }

        public string Volume => @"\\?\Volume" + Guid.ToString("b") + @"\";

        protected bool Equals(Partition other)
        {
            return Guid.Equals(other.Guid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Partition) obj);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public override string ToString()
        {
            return $@"Partition '{Name ?? "Unnamed"}', Guid '{Guid}'. ";
        }
    }
}