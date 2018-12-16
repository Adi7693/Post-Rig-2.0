using System;
using System.Collections.Generic;

namespace Input
{
    public class InputData
    {
        private bool TimeNeedsToRecalculate;

        private bool FrequencyNeedsToRecalculate;
        private bool ForceNeedsToRecalculate;
        private bool ResponseNeedsToRecalculate;

        public bool SingleStepIPNeedsToRecalculate { get; set; }

        public bool MultipleStepIPNeedsToRecalculate { get; set; }

        public bool CustomIPCalculate { get; set; }



        #region Constructor

        public InputData()
        {
            TimeNeedsToRecalculate = false;
            SingleStepIPNeedsToRecalculate = false;
            MultipleStepIPNeedsToRecalculate = false;
            CustomIPCalculate = false;
            FrequencyNeedsToRecalculate = false;
            ForceNeedsToRecalculate = false;
            ResponseNeedsToRecalculate = false;

            StartTime = 0.0;
            EndTime = 5.0;
            TimeStep = 0.01;

            StepStartTime = 1.0;
            StepAmplitude = 1.0;
            StepLength = 1.0;
            IntervalBetweenSteps = 1.0;

            VehicleMass = 1.0;
            SpringStiffness = 1.0;
            DampingCoefficient = 1.0;
            InitialDisplacement = StepAmplitude;
            InitialVelocity = 0.0;
        }
        #endregion

        #region Input Properties
        private double _startTime;

        public double StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                if (!value.Equals(_startTime))
                {
                    _startTime = value;
                    TimeNeedsToRecalculate = true;
                    SingleStepIPNeedsToRecalculate = true;
                    MultipleStepIPNeedsToRecalculate = true;
                }
            }
        }


        private double _timeStep;

        public double TimeStep
        {
            get
            {
                return _timeStep;
            }
            set
            {
                if (!value.Equals(_timeStep))
                {
                    _timeStep = value;
                    TimeNeedsToRecalculate = true;
                    SingleStepIPNeedsToRecalculate = true;
                    MultipleStepIPNeedsToRecalculate = true;
                }
            }
        }


        private double _endTime;

        public double EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {

                if (!value.Equals(_endTime))
                {
                    if (value > StartTime)
                    {
                        _endTime = value;
                        TimeNeedsToRecalculate = true;
                        SingleStepIPNeedsToRecalculate = true;
                        MultipleStepIPNeedsToRecalculate = true;
                    }
                }
            }
        }



        private double stepStartTime;

        public double StepStartTime
        {
            get
            {
                return stepStartTime;
            }

            set
            {
                if (!value.Equals(stepStartTime))
                {
                    if (value > StartTime && value < EndTime)
                    {
                        stepStartTime = value;
                        MultipleStepIPNeedsToRecalculate = true;
                    }
                }
            }
        }

        private double stepAmplitude;

        public double StepAmplitude
        {
            get
            {
                return stepAmplitude;
            }

            set
            {
                if (!value.Equals(stepAmplitude))
                {
                    stepAmplitude = value;
                    SingleStepIPNeedsToRecalculate = true;
                    MultipleStepIPNeedsToRecalculate = true;
                }
            }
        }




        private double intervalBetweenStep;

        public double IntervalBetweenSteps
        {
            get
            {
                return intervalBetweenStep;
            }

            set
            {
                if (!value.Equals(intervalBetweenStep))
                {
                    intervalBetweenStep = value;
                    MultipleStepIPNeedsToRecalculate = true;
                }
            }
        }


        private double stepLength;

        public double StepLength
        {
            get
            {
                return stepLength;
            }

            set
            {
                if (!value.Equals(stepLength))
                {
                    stepLength = value;
                    MultipleStepIPNeedsToRecalculate = true;
                }
            }
        }


        // Initial Displacement in m
        private double initialDisplacement;

        public double InitialDisplacement
        {
            get
            {
                return initialDisplacement;
            }

            set
            {
                if (!value.Equals(initialDisplacement))
                {
                    initialDisplacement = value;
                    ResponseNeedsToRecalculate = true;

                    //ResponseToICNeedsToRecalculate = true;
                    //TotalResponseNeedsToRecalculate = true;
                    //InputDataNeedsToRecalculate = true;
                }
            }
        }


        // Initial Velocity in m/s

        private double initialVelocity;

        public double InitialVelocity
        {
            get
            {
                return initialVelocity;
            }

            set
            {
                if (!value.Equals(initialVelocity))
                {
                    initialVelocity = value;
                    ResponseNeedsToRecalculate = true;

                    //InputDataNeedsToRecalculate = true;
                    //ResponseToICNeedsToRecalculate = true;
                    //TotalResponseNeedsToRecalculate = true;
                }
            }
        }


        private double _vehicleMass;
        // In Kg

        public double VehicleMass
        {
            get
            {
                return _vehicleMass;
            }

            set
            {
                if (!value.Equals(_vehicleMass))
                {
                    if (value > 0)
                    {
                        _vehicleMass = value;
                        ResponseNeedsToRecalculate = true;

                        //InputDataNeedsToRecalculate = true;
                        //VehicleDataNeedsToRecalculate = true;
                        //ResponseToICNeedsToRecalculate=true;
                        //ResponseToHarmonicIPNeedsToRecalculate=true;
                        //TotalResponseNeedsToRecalculate=true;
                    }
                }
            }
        }


        private double _springStiffness;
        // In N/m
        public double SpringStiffness
        {
            get
            {
                return _springStiffness;
            }

            set
            {
                if (!value.Equals(_springStiffness))
                {
                    _springStiffness = value;
                    ResponseNeedsToRecalculate = true;

                    //InputDataNeedsToRecalculate = true;
                    //VehicleDataNeedsToRecalculate = true;
                    //ResponseToICNeedsToRecalculate = true;
                    //ResponseToHarmonicIPNeedsToRecalculate = true;
                    //TotalResponseNeedsToRecalculate = true;
                }

            }
        }


        private double _dampingCoefficient;
        // In N/(m/s)
        public double DampingCoefficient
        {
            get
            {
                return _dampingCoefficient;
            }

            set
            {
                if (!value.Equals(_dampingCoefficient))
                {
                    _dampingCoefficient = value;
                    ResponseNeedsToRecalculate = true;

                    //InputDataNeedsToRecalculate = true;
                    //VehicleDataNeedsToRecalculate = true;
                    //ResponseToICNeedsToRecalculate = true;
                    //ResponseToHarmonicIPNeedsToRecalculate = true;
                    //TotalResponseNeedsToRecalculate = true;
                }
            }

        }
        #endregion



        #region Derived Properties

        public double NumberOfSteps
        {
            get
            {
                double N = Math.Floor((EndTime - StepStartTime) / (StepLength + IntervalBetweenSteps));
                return N;
            }
        }


        // In rad/s
        public double NaturalFrequencyRad
        {
            get
            {
                //return Math.Sqrt(SpringStiffness / VehicleMass);
                double wn = Math.Round(Math.Sqrt(SpringStiffness / VehicleMass), 3);
                return wn;
            }

        }

        // In Hz
        public double NaturalFrequencyHz
        {
            get
            {
                //return (1.0 / (2.0 * Math.PI)) * Math.Sqrt(SpringStiffness / VehicleMass);
                double Fn = Math.Round(NaturalFrequencyRad / (2.0 * Math.PI), 3);
                return Fn;
            }
        }

        // In N/(m/s)
        public double CriticalDamping
        {
            get
            {
                //return 2.0 * Math.Sqrt(VehicleMass * SpringStiffness);
                double Cc = Math.Round(2.0 * Math.Sqrt(VehicleMass * SpringStiffness), 3);
                return Cc;
            }
        }

        public double DampingRatio
        {
            get
            {
                //return DampingCoefficient / CriticalDamping;
                double Zeta = Math.Round(DampingCoefficient / CriticalDamping, 3);
                return Zeta;
            }
        }

        //private double dampedNaturalFrequency;

        public double DampedNaturalFrequency
        {
            get
            {
                //return Math.Sqrt(1.0 - Math.Pow(DampingRatio, 2)) * NaturalFrequencyRad;
                double wd = Math.Round(Math.Sqrt(1.0 - Math.Pow(DampingRatio, 2)) * NaturalFrequencyRad, 4);
                return wd;
            }
        }
        #endregion

        #region Lists of Data

        public List<double> TimeIntervals { get; set; }

        public List<double> SingleStepInput { get; private set; }

        public List<double> MultipleStepInput { get; private set; }


        public List<double> CustomInput { get; set; }

        public List<double> CustomInputVelocity { get; private set; }


        public List<double> BodyDisplacement { get; private set; }

        public List<double> BodyVelocity { get; private set; }

        public List<double> BodyAcceln { get; private set; }

        
        public List<double> ResponseToInitialConditions { get; private set; }
        
        public List<double> VelocityICR { get; private set; }
        
        public List<double> AccelerationICR { get; private set; }
        

        public List<double> SpringForceICR { get; private set; }
        
        public List<double> DamperForceICR { get; private set; }

        public List<double> BodyForceICR { get; private set; }

        #endregion


        private void TimeCalculate()
        {
            if (TimeNeedsToRecalculate)
            {
                if (TimeIntervals == null)
                {
                    TimeIntervals = new List<double>();
                }

                TimeIntervals.Clear();

                //DateTime time = DateTime.Now;

                for (double i = StartTime; i <= EndTime + TimeStep / 2.0; i += TimeStep)
                {
                    double interval = Math.Round(i, 6);
                    TimeIntervals.Add(interval);
                }

                //_tTime = (DateTime.Now - time).TotalMilliseconds;

                TimeNeedsToRecalculate = false;
            }
        }

        private void SingleStepIPCalculate()
        {
            if (SingleStepIPNeedsToRecalculate)
            {
                if (SingleStepInput == null)
                {
                    SingleStepInput = new List<double>();
                }

                SingleStepInput.Clear();

                foreach (double item in TimeIntervals)
                {

                    if (item < StepStartTime)
                    {
                        SingleStepInput.Add(0.0);
                    }

                    else if (item >= StepStartTime)
                    {
                        SingleStepInput.Add(StepAmplitude);
                    }
                }
            }
        }

        private void CustomIPVelocity()
        {
            if (CustomIPCalculate)
            {
                if (CustomInputVelocity == null)
                {
                    CustomInputVelocity = new List<double>();
                }

                CustomInputVelocity.Clear();

                CustomInputVelocity.Add(0.0);

                for (int i = 1; i < CustomInput.Count; i++)
                {
                    
                }
            }
        } 

        private void MultipleStepIPCalculate()
        {
            if (MultipleStepIPNeedsToRecalculate)
            {
                if (MultipleStepInput == null)
                {
                    MultipleStepInput = new List<double>();
                }

                MultipleStepInput.Clear();
                
                for (double time = 0.0; time < StepStartTime; time += TimeStep)
                {
                    MultipleStepInput.Add(0.0);
                }


                for (int count = 0; count < NumberOfSteps; count++)
                {
                    //double a = 0.0;
                    //double d = a + stepAmplitude;

                    for (double time = StepStartTime; time <= StepStartTime + StepLength; time += TimeStep)
                    {
                        //StepInput.Add(d);
                        MultipleStepInput.Add(stepAmplitude);
                    }

                    //double c = a - stepAmplitude;
                    for (double time = StepStartTime + StepLength; time <= StepStartTime + StepLength + IntervalBetweenSteps; time += TimeStep)
                    {
                        //StepInput.Add(c);
                        MultipleStepInput.Add(0.0);
                    }
                }

                double StepInputEndTime = (StepLength + IntervalBetweenSteps) * NumberOfSteps + StepStartTime;

                for (double time = StepInputEndTime; time <= EndTime; time += TimeStep)
                {
                    MultipleStepInput.Add(0.0);
                }

            }
        }


        #region Response Calculations

        private void ResponseToInitialConditionsCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (ResponseToInitialConditions == null)
                {
                    ResponseToInitialConditions = new List<double>();
                }

                ResponseToInitialConditions.Clear();

                // DateTime time = DateTime.Now;

                if (InitialDisplacement == 0.0 && InitialVelocity == 0.0)
                {
                    foreach (double item in TimeIntervals)
                    {
                        ResponseToInitialConditions.Add(0.0);
                    }
                }

                else
                {
                    foreach (double item in TimeIntervals)
                    {
                        if(item < StepStartTime)
                        {
                            ResponseToInitialConditions.Add(0.0);
                        }

                        else
                        {
                            if (DampingRatio < 1.0)
                            {
                                double C1 = InitialDisplacement;
                                double C2 = (InitialVelocity + (DampingRatio * NaturalFrequencyRad * InitialDisplacement)) / DampedNaturalFrequency;
                                //double X = Math.Sqrt(Math.Pow(C1, 2) + Math.Pow(C2, 2));
                                double Phy = Math.Atan((DampedNaturalFrequency * InitialDisplacement) / (InitialVelocity + (DampingRatio * NaturalFrequencyRad * InitialDisplacement)));
                                //double X = InitialDisplacement / Math.Sin(Phy);

                                double x = Math.Exp(-DampingRatio * NaturalFrequencyRad * item) * ((C1 * Math.Cos(DampedNaturalFrequency * item)) + (C2 * Math.Sin(DampedNaturalFrequency * item)));
                                //double x = X * Math.Exp(-DampingRatio * NaturalFrequencyRad * item) * Math.Cos((DampedNaturalFrequency * item) - Phy);
                                ResponseToInitialConditions.Add(x);
                            }

                            else if (DampingRatio == 1.0)
                            {
                                double C1 = InitialDisplacement;
                                double C2 = InitialVelocity + (NaturalFrequencyRad * InitialDisplacement);

                                double x = (C1 + (C2 * item)) * Math.Exp(-NaturalFrequencyRad * item);
                                ResponseToInitialConditions.Add(x);
                            }

                            else if (DampingRatio > 1.0)
                            {
                                double C1 = (InitialDisplacement * NaturalFrequencyRad * (DampingRatio + Math.Sqrt(Math.Pow(DampingRatio, 2) - 1)) + InitialVelocity) / (2 * NaturalFrequencyRad * Math.Sqrt(Math.Pow(DampingRatio, 2) - 1));
                                double C2 = (-InitialDisplacement * NaturalFrequencyRad * (DampingRatio - Math.Sqrt(Math.Pow(DampingRatio, 2) - 1)) - InitialVelocity) / (2 * NaturalFrequencyRad * Math.Sqrt(Math.Pow(DampingRatio, 2) - 1));

                                double x = (C1 * Math.Exp((-DampingRatio + Math.Sqrt(Math.Pow(DampingRatio, 2) - 1)) * NaturalFrequencyRad * item)) + (C2 * Math.Exp((-DampingRatio - Math.Sqrt(Math.Pow(DampingRatio, 2) - 1)) * NaturalFrequencyRad * item));
                                ResponseToInitialConditions.Add(x);
                            }
                        }
                        

                    }
                }
            }
            //_tResponseToInitialConditions = (DateTime.Now - time).TotalMilliseconds;
        }


        #endregion

        #region Velocity Calculations

        private void VelocityICRCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (VelocityICR == null)
                {
                    VelocityICR = new List<double>();
                }

                VelocityICR.Clear();

                VelocityICR.Add(0.0);

                for (int i = 1; i < ResponseToInitialConditions.Count; i++)
                {
                    double dsIC = (ResponseToInitialConditions[i - 1] - ResponseToInitialConditions[i]) / TimeStep;

                    VelocityICR.Add(dsIC);
                }

            }
        }

        #endregion

        #region Acceleration Calculations

        private void AccelerationICRCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (AccelerationICR == null)
                {
                    AccelerationICR = new List<double>();
                }

                AccelerationICR.Clear();

                AccelerationICR.Add(0.0);

                for (int i = 1; i < VelocityICR.Count; i++)
                {
                    double dvIC = (VelocityICR[i - 1] - VelocityICR[i]) / TimeStep;

                    AccelerationICR.Add(dvIC);
                }
            }
        }

        #endregion

        #region Spring Force Calculations

        private void SpringForceICRCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (SpringForceICR == null)
                {
                    SpringForceICR = new List<double>();
                }

                SpringForceICR.Clear();

                foreach (double item in ResponseToInitialConditions)
                {
                    double SF = SpringStiffness * item;

                    SpringForceICR.Add(SF);
                }
            }
        }

        #endregion

        #region Damper Force Calculations

        private void DamperForceICRCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (DamperForceICR == null)
                {
                    DamperForceICR = new List<double>();
                }

                DamperForceICR.Clear();

                foreach (double item in VelocityICR)
                {
                    double DF = DampingCoefficient * item;
                    DamperForceICR.Add(DF);
                }
            }
        }

        #endregion

        #region BodyForce

        private void BodyForceICRCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (BodyForceICR == null)
                {
                    BodyForceICR = new List<double>();
                }

                BodyForceICR.Clear();

                foreach (double item in AccelerationICR)
                {
                    double BF = VehicleMass * item;

                    BodyForceICR.Add(BF);
                }
            }
        }

        #endregion

        public void InputDataCalculate()
        {
           
            TimeCalculate();

            if (SingleStepIPNeedsToRecalculate)
            {
                SingleStepIPCalculate();
            }

            else if (MultipleStepIPNeedsToRecalculate)
            {
                MultipleStepIPCalculate();
            }
        }

        public void OutputDataCalculate()
        {
            ResponseToInitialConditionsCalculate();
            VelocityICRCalculate();
            AccelerationICRCalculate();
            SpringForceICRCalculate();
            DamperForceICRCalculate();
            BodyForceICRCalculate();
        }
    }
}
