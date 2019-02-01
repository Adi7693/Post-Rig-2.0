using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PostRig2_0
{
    public class SimData
    {

        public InputData Parent { get; set; }

        public string FileName { get; set; }

        public double StartTime { get; set; }

        public double EndTime { get; set; }

        public double TimeStep { get; set; }

        public double ForceAmplitude { get; set; }

        public double ExcitationFrequencyHz { get; set; }

        public double InitialDisplacement { get; set; }

        public double InitialVelocity { get; set; }

        private double TimeIntervalCount { get; set; }

        private double HarmonicInputCount { get; set; }

        private double ForceVectorCount { get; set; }

        private double UnitStepResponseCount { get; set; }

        private double StepForceResponseCount { get; set; }

        private double UnitStepVelocityCount { get; set; }

        private double UnitStepAccelnCount { get; set; }

        private double HomogeneousSolutionCount { get; set; }

        private double ParticularSolutionCount { get; set; }

        private double TotalResponseCount { get; set; }

        private double BodyVelocity { get; set; }

        private double BodyAccelnCount { get; set; }

        private double SpringForceCount { get; set; }

        private double DamperForceCount { get; set; }

        public int Version { get; set; }

        public SimData(InputData input)
        {
            Parent = input;

            StartTime = 1.0;

            EndTime = 1.0;

            TimeStep = 1.0;

            ForceAmplitude = 1.0;
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


                        writer.Write("Begin_TimeFrame");

                        writer.Write(StartTime);

                        writer.Write(EndTime);

                        writer.Write(TimeStep);

                        writer.Write("End_TimeFrame");


                        writer.Write("Begin_Setup_Values");

                        writer.Write(ForceAmplitude);

                        writer.Write(ExcitationFrequencyHz);

                        writer.Write(InitialDisplacement);

                        writer.Write(InitialVelocity);

                        writer.Write("End_Setup_Values");


                        writer.Write("Begin_TimeIntervalList");

                        writer.Write(Parent.TimeIntervals.Count);

                        for (int i = 0; i < Parent.TimeIntervals.Count; i++)
                        {
                            writer.Write(Parent.TimeIntervals[i]);
                        }

                        writer.Write("End_TimeIntervalList");


                        writer.Write("Begin_Results");

                        if (Parent.ResponseCalculationComplete)
                        {
                            if (Parent.HarmonicInput != null)
                            {
                                writer.Write("Begin_HarmonicInputSignal");

                                writer.Write(Parent.HarmonicInput.Count);

                                for (int i = 0; i < Parent.HarmonicInput.Count; i++)
                                {
                                    writer.Write(Parent.HarmonicInput[i]);
                                }

                                writer.Write("End_HarmonicInputSignal");
                            }

                            if (Parent.ForceVector != null)
                            {
                                writer.Write("Begin_ForceVetor");

                                for (int i = 0; i < Parent.ForceVector.Count; i++)
                                {
                                    writer.Write(Parent.ForceVector[i]);
                                }

                                writer.Write("End_ForceVetor");

                            }


                            if(Parent.ResponseToUnitStepFunction != null)
                            {
                                writer.Write("Begin_UnitStepResponse");

                                for (int i = 0; i < Parent.ResponseToUnitStepFunction.Count; i++)
                                {
                                    writer.Write(Parent.ResponseToUnitStepFunction[i]);
                                }

                                writer.Write("End_UnitStepResponse");
                            }


                            if (Parent.StepForceResponse != null)
                            {
                                writer.Write("Begin_StepForceResponse");

                                for (int i = 0; i < Parent.StepForceResponse.Count; i++)
                                {
                                    writer.Write(Parent.StepForceResponse[i]);
                                }

                                writer.Write("End_StepForceResponse");
                            }



                            if(Parent.UnitStepFunctionVelocity != null)
                            {
                                writer.Write("Begin_UnitStepVelocity");

                                for (int i = 0; i < Parent.UnitStepFunctionVelocity.Count; i++)
                                {
                                    writer.Write(Parent.UnitStepFunctionVelocity[i]);
                                }

                                writer.Write("End_UnitStepVelocity");
                            }

                            if (Parent.UnitStepFunctionAcceleration != null)
                            {
                                writer.Write("Begin_UnitStepAcceln");

                                for (int i = 0; i < Parent.UnitStepFunctionAcceleration.Count; i++)
                                {
                                    writer.Write(Parent.UnitStepFunctionAcceleration[i]);
                                }

                                writer.Write("End_UnitStepAcceln");
                            }


                            if(Parent.HomogeneousSolution != null)
                            {
                                writer.Write("Begin_HomogeneousSolution");

                                for (int i = 0; i < Parent.HomogeneousSolution.Count; i++)
                                {
                                    writer.Write(Parent.HomogeneousSolution[i]);
                                }

                                writer.Write("End_HomogeneousSolution");
                            }


                            if (Parent.ParticularSolution != null)
                            {
                                writer.Write("Begin_ParticularSolution");

                                for (int i = 0; i < Parent.ParticularSolution.Count; i++)
                                {
                                    writer.Write(Parent.ParticularSolution[i]);
                                }

                                writer.Write("End_ParticularSolution");
                            }

                            if(Parent.TotalResponse != null)
                            {
                                writer.Write("Begin_TotalSolution");

                                for (int i = 0; i < Parent.TotalResponse.Count; i++)
                                {
                                    writer.Write(Parent.TotalResponse[i]);
                                }

                                writer.Write("End_TotalSolution");
                            }

                            if(Parent.BodyVelocity != null)
                            {
                                writer.Write("Begin_BodyVelocity");

                                for (int i = 0; i < Parent.BodyVelocity.Count; i++)
                                {
                                    writer.Write(Parent.BodyVelocity[i]);
                                }

                                writer.Write("End_BodyVelocity");
                            }


                            if (Parent.BodyAcceln != null)
                            {
                                writer.Write("Begin_BodyAcceln");

                                for (int i = 0; i < Parent.BodyAcceln.Count; i++)
                                {
                                    writer.Write(Parent.BodyAcceln[i]);
                                }

                                writer.Write("End_BodyAcceln");
                            }


                            if(Parent.SpringForce != null)
                            {
                                writer.Write("Begin_SpringForce");

                                for (int i = 0; i < Parent.SpringForce.Count; i++)
                                {
                                    writer.Write(Parent.SpringForce[i]);
                                }

                                writer.Write("End_SpringForce");
                            }

                            if (Parent.DamperForce != null)
                            {
                                writer.Write("Begin_DamperForce");

                                for (int i = 0; i < Parent.DamperForce.Count; i++)
                                {
                                    writer.Write(Parent.DamperForce[i]);
                                }

                                writer.Write("End_DamperForce");
                            }
                            
                        }
                        writer.Write("End_Results");
                    }
                    catch { }
                }

            }
            catch { }
        }


        private void ReadTimeFrames(BinaryReader reader)
        {
            StartTime = reader.ReadDouble();

            EndTime = reader.ReadDouble();

            TimeStep = reader.ReadDouble();

            reader.ReadString();
        }

        private void ReadSetupValues(BinaryReader reader)
        {
            ForceAmplitude = reader.ReadDouble();

            ExcitationFrequencyHz = reader.ReadDouble();

            InitialDisplacement = reader.ReadDouble();

            InitialVelocity = reader.ReadDouble();

            reader.ReadString();

        }

        private void ReadTimeIntervalList(BinaryReader reader)
        {
            TimeIntervalCount = reader.ReadDouble();

            for(int i=0; i<TimeIntervalCount; i++)
            {
                Parent.TimeIntervals.Add(reader.ReadDouble());
            }

            reader.ReadString();
        }

        private void ReadHarmonicInputList(BinaryReader reader)
        {
            HarmonicInputCount = reader.ReadDouble();

            for(int i = 0; i<HarmonicInputCount; i++)
            {
                Parent.HarmonicInput.Add(reader.ReadDouble());
            }

            reader.ReadString();
        }

        private void ReadForceVector(BinaryReader reader)
        {
            // Start from here again
        }

        public void Load(string fileName)
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                {
                    Version = reader.ReadInt32();

                    while (true)
                    {
                        string section = reader.ReadString();

                        if (section == "Begin_TimeFrame")
                        {
                            ReadTimeFrames(reader);
                        }
                        else if (section == "Begin_Setup_Values")
                        {
                            ReadSetupValues(reader);
                        }
                        else if (section == "Begin_TimeIntervalList")
                        {
                            ReadTimeIntervalList(reader);
                        }
                        else if(section == "Begin_Results")
                        {
                            if(section == "Begin_HarmonicInputSignal") 
                            {
                                


                            }
                        }

                        
                    }
                }
            }
            catch { }
        }

        
    }
}
