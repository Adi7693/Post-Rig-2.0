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

        //public bool IsNew { get; set; }

        //private bool VehicleDataNeedsToRecalculate;
        //private bool ResponseToICNeedsToRecalculate;
        //private bool ResponseToHarmonicIPNeedsToRecalculate;
        //private bool TotalResponseNeedsToRecalculate;

        //private bool InputDataNeedsToRecalculate;

        //private double _tTime = 0.0;
        //private double _tCosinTerm = 0.0;
        //private double _tHarmonicForce = 0.0;
        //private double _tResponseToHarmonicIP = 0.0;
        //private double _tResponseToInitialConditions = 0.0;
        //private double _tTotalResponse = 0.0;
        //private double _tStepInput = 0.0;





        private bool singleStepIPNeedsToRecalculate;

        public bool SingleStepIPNeedsToRecalculate
        {
            get
            {
                return singleStepIPNeedsToRecalculate;
            }

            set
            {
                if (value.Equals(singleStepIPNeedsToRecalculate))
                {
                    singleStepIPNeedsToRecalculate = true;
                }
                else
                {
                    singleStepIPNeedsToRecalculate = false;
                }
            }
        }


        private bool multipleStepIPNeedsToRecalculate;

        public bool MultipleStepIPNeedsToRecalculate
        {
            get
            {
                return multipleStepIPNeedsToRecalculate;
            }

            set
            {
                if (value.Equals(multipleStepIPNeedsToRecalculate))
                {
                    multipleStepIPNeedsToRecalculate = true;
                }
                else
                {
                    multipleStepIPNeedsToRecalculate = false;
                }

            }
        }

        private bool customIPCalculate;

        public bool CustomIPCalculate
        {
            get
            {
                return customIPCalculate;
            }

            set
            {
                if (value.Equals(customIPCalculate))
                {
                    customIPCalculate = true;
                }

                else
                {
                    customIPCalculate = false;
                }
            }
        }



        #region Constructor

        public InputData()
        {
            TimeNeedsToRecalculate = false;
            singleStepIPNeedsToRecalculate = false;
            multipleStepIPNeedsToRecalculate = false;
            FrequencyNeedsToRecalculate = false;
            ForceNeedsToRecalculate = false;
            ResponseNeedsToRecalculate = false;

            //VehicleDataNeedsToRecalculate = false;
            //ResponseToICNeedsToRecalculate = false;
            //ResponseToHarmonicIPNeedsToRecalculate = false;
            //TotalResponseNeedsToRecalculate = false;
            //InputDataNeedsToRecalculate = false;


            StartTime = 0.0;
            EndTime = 5.0;
            TimeStep = 0.01;

            StepStartTime = 1.0;
            StepAmplitude = 1.0;
            StepLength = 1.0;
            IntervalBetweenSteps = 1.0;

            ExcitationFrequencyHz = 1.0;
            InputForce = 10.0;

            VehicleMass = 1.0;
            SpringStiffness = 1.0;
            DampingCoefficient = 1.0;
            InitialDisplacement = StepAmplitude;
            InitialVelocity = 0.0;


            //_tTime = 0.0;
            //_tCosinTerm = 0.0;
            //_tHarmonicForce = 0.0;
            //_tResponseToHarmonicIP = 0.0;
            //_tResponseToInitialConditions = 0.0;
            //_tTotalResponse = 0.0;
            //_tStepInput = 0.0;
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
                    singleStepIPNeedsToRecalculate = true;
                    multipleStepIPNeedsToRecalculate = true;

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
                    singleStepIPNeedsToRecalculate = true;
                    multipleStepIPNeedsToRecalculate = true;
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
                        singleStepIPNeedsToRecalculate = true;
                        multipleStepIPNeedsToRecalculate = true;
                    }
                }

                //else
                //{
                //    TimeNeedsToRecalculate = false;
                //    SingleStepIPNeedsToRecalculate = false;
                //}
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
                        multipleStepIPNeedsToRecalculate = true;

                        //InputDataNeedsToRecalculate = true;
                        //ResponseNeedsToRecalculate = true;
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
                    singleStepIPNeedsToRecalculate = true;
                    multipleStepIPNeedsToRecalculate = true;

                    //InputDataNeedsToRecalculate = true;
                    //ResponseNeedsToRecalculate = true;

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
                    multipleStepIPNeedsToRecalculate = true;
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
                    multipleStepIPNeedsToRecalculate = true;
                }
            }
        }


        private double _excitationFrequencyHz;
        // In Hz
        public double ExcitationFrequencyHz
        {
            get
            {
                return _excitationFrequencyHz;
            }

            set
            {
                if (!value.Equals(_excitationFrequencyHz))
                {
                    _excitationFrequencyHz = value;
                    FrequencyNeedsToRecalculate = true;
                    ResponseNeedsToRecalculate = true;

                    //InputDataNeedsToRecalculate = true;
                    //ResponseNeedsToRecalculate = true;
                    //ResponseToHarmonicIPNeedsToRecalculate = true;
                    //TotalResponseNeedsToRecalculate = true;
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


        private double _force;
        // In N
        public double InputForce
        {
            get
            {
                return _force;
            }

            set
            {
                if (!value.Equals(_force))
                {
                    _force = value;
                    ResponseNeedsToRecalculate = true;

                    //ForceNeedsToRecalculate = true;
                    //ResponseToHarmonicIPNeedsToRecalculate = true;
                    //TotalResponseNeedsToRecalculate = true;
                    //InputDataNeedsToRecalculate = true;
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
        public double ExcitationFrequencyRad
        {
            get
            {
                return 2.0 * Math.PI * ExcitationFrequencyHz;
                //double we = 2.0 * Math.PI * ExcitationFrequencyHz;
                //return Math.Round(we, 4);
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
                double Fn = Math.Round((1.0 / (2.0 * Math.PI)) * Math.Sqrt(SpringStiffness / VehicleMass), 3);
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



        public double FrequencyRatio
        {
            get
            {
                //return ExcitationFrequencyRad / NaturalFrequencyRad;
                double Fr = Math.Round(ExcitationFrequencyRad / NaturalFrequencyRad, 3);
                return Fr;
            }
        }


        public double Phy
        {
            get
            {
                double num = -2.0 * DampingRatio * FrequencyRatio;
                double den = 1.0 - Math.Pow(FrequencyRatio, 2);
                return Math.Atan(num / den);
                //double phy = Math.Round(Math.Atan(num / den), 4);
                //return Phy;
            }
        }

        public double StaticDisplacement
        {
            get
            {
                return InputForce / SpringStiffness;
            }
        }


        public double TransferFunction
        {
            get
            {
                double denFirstTerm = Math.Pow(1.0 - Math.Pow(FrequencyRatio, 2), 2);
                double denSecondTerm = Math.Pow(2.0 * DampingRatio * FrequencyRatio, 2);
                return 1.0 / Math.Sqrt(denFirstTerm + denSecondTerm);

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

        public List<double> CosineOscillation { get; private set; }


        public List<double> SingleStepInput { get; private set; }

        public List<double> MultipleStepInput { get; private set; }


        public List<double> CustomInput { get; set; }

        public List<double> CustomInputVelocity { get; private set; }


        public List<double> InputForceOscillations { get; private set; }


        public List<double> SingleStepInputForResponse { get; private set; }

        public List<double> MultipleStepInputForResponse { get; private set; }



        public List<double> ResponseToHarmonicInput { get; private set; }

        public List<double> ResponseToInitialConditions { get; private set; }

        public List<double> TotalResponse { get; private set; }



        public List<double> VelocityICR { get; private set; }

        public List<double> VelocityHR { get; private set; }

        public List<double> VelocityTR { get; private set; }



        public List<double> AccelerationICR { get; private set; }

        public List<double> AccelerationHR { get; private set; }

        public List<double> AccelerationTR { get; private set; }


        public List<double> SpringForceICR { get; private set; }

        public List<double> SpringForceHR { get; private set; }

        public List<double> SpringForceTR { get; private set; }



        public List<double> DamperForceICR { get; private set; }

        public List<double> DamperForceHR { get; private set; }

        public List<double> DamperForceTR { get; private set; }



        public List<double> BodyForceICR { get; private set; }

        public List<double> BodyForceHR { get; private set; }

        public List<double> BodyForceTR { get; private set; }

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
            if (singleStepIPNeedsToRecalculate)
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


        //private void SingleStepIPForResponseCalculate()
        //{
        //    if (singleStepIPNeedsToRecalculate)
        //    {
        //        if(SingleStepInputForResponse == null)
        //        {
        //            SingleStepInputForResponse = new List<double>();
        //        }

        //        SingleStepInputForResponse.Clear();

        //        for(double i = StartTime; i<= EndTime; i+= TimeStep)
        //        {
        //            if(i == StepStartTime)
        //            {
        //                SingleStepInputForResponse.Add(StepAmplitude);
        //            }

        //            else
        //            {
        //                SingleStepInputForResponse.Add(0);
        //            }
        //        }
        //    }
        //}

        //private void MultipleStepIPForResponse()
        //{
        //    if (MultipleStepIPNeedsToRecalculate)
        //    {
        //        if (MultipleStepInputForResponse == null)
        //        {
        //            MultipleStepInputForResponse = new List<double>();
        //        }


        //        MultipleStepInputForResponse.Clear();

        //        for(int i = 0; i<= NumberOfSteps; i++)
        //        {
        //            for(double time = StartTime; time < StepStartTime; time += TimeStep)
        //            {
        //                MultipleStepInputForResponse.Add(0.0);
        //            }

        //            MultipleStepInputForResponse.Add(StepAmplitude);

        //            for(double time = StepStartTime+1; time<StepLength; time += TimeStep)
        //            {
        //                MultipleStepInputForResponse.Add(0.0);
        //            }

        //            MultipleStepInputForResponse.Add(-StepAmplitude);

        //            for(double time = StepStartTime+StepLength+1; time <= EndTime; time += TimeStep)
        //            {
        //                MultipleStepInputForResponse.Add(0.0);
        //            }

        //        }

        //    }
        //}

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
            if (multipleStepIPNeedsToRecalculate)
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

        private void CosineFuntionCalculate()
        {
            if (FrequencyNeedsToRecalculate)
            {
                if (CosineOscillation == null)
                {
                    CosineOscillation = new List<double>();
                }

                CosineOscillation.Clear();

                //DateTime time = DateTime.Now;

                //Access Time Data from TimeData Project
                foreach (double item in TimeIntervals)
                {
                    double frequency = Math.Cos(2 * Math.PI * ExcitationFrequencyHz * item);
                    double w = Math.Round(frequency, 6);

                    CosineOscillation.Add(w);
                }


                //_tCosinTerm = (DateTime.Now - time).TotalMilliseconds;


                FrequencyNeedsToRecalculate = false;

                ForceNeedsToRecalculate = true;

            }
        }

        private void HarmonicForceCalculate()
        {
            if (ForceNeedsToRecalculate)
            {
                if (InputForceOscillations == null)
                {
                    InputForceOscillations = new List<double>();
                }


                InputForceOscillations.Clear();

                //DateTime time = DateTime.Now;


                foreach (double item in CosineOscillation)


                {
                    double force = InputForce * item;
                    double F = Math.Round(force, 6);

                    InputForceOscillations.Add(F);
                }

                //_tHarmonicForce = (DateTime.Now - time).TotalMilliseconds;

                ForceNeedsToRecalculate = false;
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

        private void ResponseToHarmonicIPCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (ResponseToHarmonicInput == null)
                {
                    ResponseToHarmonicInput = new List<double>();
                }

                ResponseToHarmonicInput.Clear();

                if (InputForce == 0.0 || ExcitationFrequencyHz == 0.0)
                {
                    foreach (double item in TimeIntervals)
                    {

                        ResponseToHarmonicInput.Add(0.0);
                    }

                }

                else
                {
                    foreach (double item in TimeIntervals)
                    {
                        double xOfTime = StaticDisplacement * TransferFunction * Math.Cos((ExcitationFrequencyRad * item) + Phy);
                        ResponseToHarmonicInput.Add(xOfTime);
                    }
                }
            }
        }

        private void TotalResponseCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (TotalResponse == null)
                {
                    TotalResponse = new List<double>();
                }

                TotalResponse.Clear();
                //DateTime time = DateTime.Now;

                for (int i = 0; i < TimeIntervals.Count; i++)
                {
                    double x = ResponseToInitialConditions[i] + ResponseToHarmonicInput[i];
                    TotalResponse.Add(x);
                }

                //_tTotalResponse = (DateTime.Now - time).TotalMilliseconds;
            }
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

        private void VelocityHRCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (VelocityHR == null)
                {
                    VelocityHR = new List<double>();
                }

                VelocityHR.Clear();

                VelocityHR.Add(0.0);

                for (int i = 1; i < ResponseToHarmonicInput.Count; i++)
                {
                    double dsH = (ResponseToHarmonicInput[i - 1] - ResponseToHarmonicInput[i]) / TimeStep;

                    VelocityHR.Add(dsH);

                }
            }
        }

        private void VelocityTRCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (VelocityTR == null)
                {
                    VelocityTR = new List<double>();
                }

                VelocityTR.Clear();

                VelocityTR.Add(0.0);

                for (int i = 1; i < TotalResponse.Count; i++)
                {
                    double dsT = (TotalResponse[i - 1] - TotalResponse[i]) / TimeStep;

                    VelocityTR.Add(dsT);
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

        private void AccelerationHRCalcuclate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (AccelerationHR == null)
                {
                    AccelerationHR = new List<double>();
                }

                AccelerationHR.Clear();

                AccelerationHR.Add(0.0);

                for (int i = 1; i < VelocityHR.Count; i++)
                {
                    double dvH = (VelocityHR[i - 1] - VelocityHR[i]) / TimeStep;

                    AccelerationHR.Add(dvH);
                }
            }
        }

        private void AccelerationTRCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (AccelerationTR == null)
                {
                    AccelerationTR = new List<double>();
                }

                AccelerationTR.Clear();

                AccelerationTR.Add(0.0);

                for (int i = 1; i < VelocityTR.Count; i++)
                {
                    double dvT = (VelocityTR[i - 1] - VelocityTR[i]) / TimeStep;

                    AccelerationTR.Add(dvT);
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

        private void SpringForceHRCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (SpringForceHR == null)
                {
                    SpringForceHR = new List<double>();
                }

                SpringForceHR.Clear();

                foreach (double item in ResponseToHarmonicInput)
                {
                    double SF = SpringStiffness * item;

                    SpringForceHR.Add(SF);
                }
            }
        }

        private void SpringForceTRCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (SpringForceTR == null)
                {
                    SpringForceTR = new List<double>();
                }

                SpringForceTR.Clear();

                foreach (double item in TotalResponse)
                {
                    double SF = SpringStiffness * item;

                    SpringForceTR.Add(SF);
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

        private void DamperForceHRCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (DamperForceHR == null)
                {
                    DamperForceHR = new List<double>();
                }

                DamperForceHR.Clear();

                foreach (double item in VelocityHR)
                {
                    double DF = DampingCoefficient * item;
                    DamperForceHR.Add(DF);
                }
            }
        }

        private void DamperForceTRCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (DamperForceTR == null)
                {
                    DamperForceTR = new List<double>();
                }

                DamperForceTR.Clear();

                foreach (double item in VelocityTR)
                {
                    double DF = DampingCoefficient * item;
                    DamperForceTR.Add(DF);
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

        private void BodyForceHRCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (BodyForceHR == null)
                {
                    BodyForceHR = new List<double>();
                }

                BodyForceHR.Clear();

                foreach (double item in AccelerationHR)
                {
                    double BF = VehicleMass * item;

                    BodyForceHR.Add(BF);
                }
            }
        }

        private void BodyForceTRCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (BodyForceTR == null)
                {
                    BodyForceTR = new List<double>();
                }

                BodyForceTR.Clear();

                foreach (double item in AccelerationTR)
                {
                    double BF = VehicleMass * item;

                    BodyForceTR.Add(BF);
                }

                ResponseNeedsToRecalculate = false;
                //IsNew = true;
            }
        }

        #endregion

        public void InputDataCalculate()
        {


            TimeCalculate();
            CosineFuntionCalculate();
            HarmonicForceCalculate();

            if (singleStepIPNeedsToRecalculate)
            {
                SingleStepIPCalculate();
            }

            else if (multipleStepIPNeedsToRecalculate)
            {
                MultipleStepIPCalculate();
            }



            
        }

        public void OutputDataCalculate()
        {
            ResponseToInitialConditionsCalculate();
            ResponseToHarmonicIPCalculate();
            TotalResponseCalculate();

            VelocityICRCalculate();
            VelocityHRCalculate();
            VelocityTRCalculate();

            AccelerationICRCalculate();
            AccelerationHRCalcuclate();
            AccelerationTRCalculate();

            SpringForceICRCalculate();
            SpringForceHRCalculate();
            SpringForceTRCalculate();

            DamperForceICRCalculate();
            DamperForceHRCalculate();
            DamperForceTRCalculate();

            BodyForceICRCalculate();
            BodyForceHRCalculate();
            BodyForceTRCalculate();
        }


        public bool NeedsToRecalculate
        {
            get
            {
                return TimeNeedsToRecalculate || FrequencyNeedsToRecalculate || ForceNeedsToRecalculate;
            }
        }

    }
}
