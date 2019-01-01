using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostRig2_0
{
    public class CarData
    {
        public InputData Parent { get; set; }

        public string FileName { get; set; }

        public double VehicleMass { get; set; }

        public double SpringStiffness { get; set; }

        public double DampingCoefficient { get; set; }

        public double SpringFreeLength { get; set; }

        public double SpringExtendedLength { get; set; }

        public double SpringCompressedLength { get; set; }

        public int Version { get; set; }

        public CarData(InputData input)
        {
            Parent = input;

            VehicleMass = 1.0;

            SpringStiffness = 1.0;

            DampingCoefficient = 1.0;

            SpringFreeLength = 1.0;
            SpringExtendedLength = 1.0;
            SpringCompressedLength = 1.0;
        }


        public CarData(InputData input, string fileName) : this(input)
        {
            Load(fileName);
        }


        public void Load(string fileName)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                    {
                        try
                        {
                            Version = reader.ReadInt32();

                            VehicleMass = reader.ReadDouble();

                            SpringStiffness = reader.ReadDouble();

                            DampingCoefficient = reader.ReadDouble();

                            SpringFreeLength = reader.ReadDouble();

                            SpringCompressedLength = reader.ReadDouble();

                            SpringExtendedLength = reader.ReadDouble();

                            reader.Close();

                            FileName = fileName;
                        }
                        catch { }
                    }
                }
                catch { }
            }
        }


        public void Save(string fileName)
        {
            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
                {
                    if (Version == 0)
                    {
                        Version = 1;
                    }

                    try
                    {
                        writer.Write(Version);

                        writer.Write(VehicleMass);

                        writer.Write(SpringStiffness);

                        writer.Write(DampingCoefficient);

                        writer.Write(SpringFreeLength);

                        writer.Write(SpringCompressedLength);

                        writer.Write(SpringExtendedLength);

                        writer.Close();

                        if (FileName != fileName)
                        {
                            FileName = fileName;
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }
    }
}
