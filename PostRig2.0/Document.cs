using System.IO;
using ExcelDataReader;
using System.Data;
using Input;
using System.Collections.Generic;

namespace PostRig2._0
{
    public class Document
    {
        public InputData Input { get; set; }

        public int Version
        {
            get;
            private set;
        }

        public string FileName { get; private set; }

        public string Name
        {
            get
            {
                if (FileName != null)
                {
                    return Path.GetFileName(FileName);
                }

                return null;
            }
        }

        public Document()
        {
            Input = new InputData();

            Version = 0;
        }


        public Document(string fileName) : this()
        {
            Open(fileName);
        }


        private void LoadCarData(BinaryReader reader)
        {

        }


        public void Open(string fileName)
        {
            FileName = fileName;

            BinaryReader reader = new BinaryReader(File.Open(FileName, FileMode.Open));

            Version = reader.ReadInt32();

            if (Input == null)
            {
                Input = new InputData();
            }

            Input.StartTime = reader.ReadDouble();
            Input.EndTime = reader.ReadDouble();
            Input.TimeStep = reader.ReadDouble();

            Input.StepAmplitudeChangeTime = reader.ReadDouble();
            Input.StepAmplitude = reader.ReadDouble();
            Input.ExcitationFrequencyHz = reader.ReadDouble();

            Input.InputForce = reader.ReadDouble();

            Input.VehicleMass = reader.ReadDouble();

            Input.SpringStiffness = reader.ReadDouble();

            Input.DampingCoefficient = reader.ReadDouble();

            Input.InitialDisplacement = reader.ReadDouble();

            Input.InitialVelocity = reader.ReadDouble();

            reader.Close();
        }


        public void Save()
        {
            SaveAs(FileName);
        }

        public void SaveAs(string fileName)
        {
            BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create));

            if (Version == 0)
            {
                Version = 1;
            }
            writer.Write(Version);

            writer.Write(Input.StartTime);

            writer.Write(Input.EndTime);

            writer.Write(Input.TimeStep);

            writer.Write(Input.StepAmplitudeChangeTime);

            writer.Write(Input.StepAmplitude);

            writer.Write(Input.ExcitationFrequencyHz);

            writer.Write(Input.InputForce);

            writer.Write(Input.VehicleMass);

            writer.Write(Input.SpringStiffness);

            writer.Write(Input.DampingCoefficient);

            writer.Write(Input.InitialDisplacement);

            writer.Write(Input.InitialVelocity);


            writer.Close();
        }

        public DataSet CustomIPDataSet { get; private set; }

        public void CustomInputExcelRead(string fileName)
        {
            FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read);
            IExcelDataReader reader = ExcelReaderFactory.CreateCsvReader(fs);

            CustomIPDataSet = reader.AsDataSet();

            DataRowCollection rows = CustomIPDataSet.Tables[0].Rows;

            if (Input.TimeIntervals == null)
            {
                Input.TimeIntervals = new List<double>();
            }

            if (Input.CustomInput == null)
            {
                Input.CustomInput = new List<double>();
            }


            Input.TimeIntervals.Clear();
            Input.CustomInput.Clear();

            for (int i = 0; i < rows.Count; i++)
            {
                string strval = (string)rows[i][0];
                double dblval = -1.0;
                if (double.TryParse(strval, out dblval))
                {
                    Input.TimeIntervals.Add(dblval);
                }

                strval = (string)rows[i][1];

                if (double.TryParse(strval, out dblval))
                {
                    Input.CustomInput.Add(dblval);
                }


            }


            reader.Close();

            fs.Close();
        }

    }
}
