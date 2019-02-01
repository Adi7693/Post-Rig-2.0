using System.IO;
using ExcelDataReader;
using System.Data;
using System.Collections.Generic;
using System;

namespace PostRig2_0
{
    public class Document
    {
        public MainForm MainForm { get; private set; }

        public CarData Car { get; set; }

        public SimData Sim { get; set; }

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

        public Document(MainForm form)
        {
            Input = new InputData();

            Version = 0;

            MainForm = form;
        }

        public Document(CarData car)
        {
            Car = new CarData(Input);

            Version = 0;

        }

        public Document(MainForm form, string fileName) : this(form)
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

            Input.VehicleMass = reader.ReadDouble();

            Input.SpringStiffness = reader.ReadDouble();

            Input.DampingCoefficient = reader.ReadDouble();

            MainForm.NewCarBuilt = reader.ReadBoolean();
            MainForm.NewSimSetup = reader.ReadBoolean();
            MainForm.ViewResults = reader.ReadBoolean();
            MainForm.SingleStepIP = reader.ReadBoolean();
            MainForm.HarmonicIP = reader.ReadBoolean();
            MainForm.CustomIP = reader.ReadBoolean();

            Input.ForceAmplitude = reader.ReadDouble();

            if (MainForm.CustomIP)
            {
                Input.RoadDisplacement = new List<double>();
                Input.TimeIntervals = new List<double>();

                int count = reader.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    Input.TimeIntervals.Add(reader.ReadDouble());
                    Input.RoadDisplacement.Add(reader.ReadDouble());
                }
            }

            reader.Close();
        }


        public void Save()
        {
            SaveAs(FileName);
        }

        public void SaveAs(string fileName)
        {
            //BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create));

            //if (Version == 0)
            //{
            //    Version = 1;
            //}
            //writer.Write(Version);

            //writer.Write(Input.StartTime);

            //writer.Write(Input.EndTime);

            //writer.Write(Input.TimeStep);

            //writer.Write(Input.StepStartTime);

            //writer.Write(Input.StepAmplitude);

            //writer.Write(Input.VehicleMass);

            //writer.Write(Input.SpringStiffness);

            //writer.Write(Input.DampingCoefficient);

            //writer.Write(MainForm.NewCarBuilt);
            //writer.Write(MainForm.NewSimSetup);
            //writer.Write(MainForm.ViewResults);
            //writer.Write(MainForm.SingleStepIP);
            //writer.Write(MainForm.MultipleStepIP);
            //writer.Write(MainForm.CustomIP);

            //writer.Write(Input.ForceAmplitude);

            //if (MainForm.CustomIP)
            //{
            //    writer.Write(Input.TimeIntervals.Count);
            //    for (int i = 0; i < Input.RoadDisplacement.Count; i++)
            //    {
            //        writer.Write(Input.TimeIntervals[i]);
            //        writer.Write(Input.RoadDisplacement[i]);
            //    }
            //}

            //writer.Close();


            string tempFolderPath = Path.GetTempPath() + Guid.NewGuid().ToString() + "\\";

            Directory.CreateDirectory(tempFolderPath);

            Input.SaveCar(tempFolderPath + "carFile.car");

            Input.SaveSimData(tempFolderPath + "simFile.sim");



        }




        public void CustomInputExcelRead(string fileName)
        {
            FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read);
            IExcelDataReader reader = ExcelReaderFactory.CreateCsvReader(fs);

            DataSet CustomIPDataSet = reader.AsDataSet();

            DataRowCollection rows = CustomIPDataSet.Tables[0].Rows;

            if (Input.TimeIntervals == null)
            {
                Input.TimeIntervals = new List<double>();
            }

            if (Input.RoadDisplacement == null)
            {
                Input.RoadDisplacement = new List<double>();
            }


            Input.TimeIntervals.Clear();
            Input.RoadDisplacement.Clear();

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
                    Input.RoadDisplacement.Add(dblval);
                }
            }
            
            reader.Close();

            fs.Close();
        }

    }
}
