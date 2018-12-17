using System;
using System.Collections.Generic;

namespace Input
{
    public class InputData
    {
        public bool TimeNeedsToRecalculate;

        private bool ResponseNeedsToRecalculate;

        public bool SingleStepIPNeedsToRecalculate { get; set; }

        public bool MultipleStepIPNeedsToRecalculate { get; set; }

        public bool CustomIPCalculate { get; set; }

        public bool ResponseCalculationComplete { get; private set; }



        #region Constructor

        public InputData()
        {
            TimeNeedsToRecalculate = true;
            SingleStepIPNeedsToRecalculate = false;
            MultipleStepIPNeedsToRecalculate = false;
            CustomIPCalculate = false;
            ResponseNeedsToRecalculate = false;
            ResponseCalculationComplete = false;

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

            InitialDisplacement = 0.0;
            InitialVelocity = 0.0;
        }
        #endregion

        #region Input Properties

        // Time Frame Calculate


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
                    }
                }
            }
        }


        // Step Input Data

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
                        ResponseNeedsToRecalculate = true;
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
                    ResponseNeedsToRecalculate = true;
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
                    ResponseNeedsToRecalculate = true;
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
                    ResponseNeedsToRecalculate = true;
                }
            }
        }


        /* Initial Displacement in m
         * Initial Displacement of the Body at Time t=0
         * 
         * X1(0)
         * 

        */

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
                }
            }
        }


        /*
         * Initial Velocity in m/s
         * Initial Velocity of the body at time t = 0
         * X1[Dot](0) = X2
         * 
         */

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

        // RoadDisplacement = Zr
        public List<double> RoadDisplacement { get; set; }

        // RoadVelocity = Zr[Dot]
        public List<double> RoadVerticalVelocity { get; private set; }

        // Body Displacment = Zb = X1
        public List<double> BodyDisplacement { get; private set; }

        // Body Velocity = Zb[Dot] = X1[dot] = X2
        public List<double> BodyVelocity { get; private set; }

        // Body Acceleration = Zb[Double Dot] = X2[Dot]
        public List<double> BodyAcceln { get; private set; }


        public List<double> SpringForce { get; private set; }

        public List<double> DamperForce { get; private set; }

        public List<double> BodyForce { get; private set; }

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
                if (RoadDisplacement == null)
                {
                    RoadDisplacement = new List<double>();
                }

                RoadDisplacement.Clear();

                foreach (double item in TimeIntervals)
                {

                    if (item < StepStartTime)
                    {
                        RoadDisplacement.Add(0.0);
                    }

                    else if (item >= StepStartTime)
                    {
                        RoadDisplacement.Add(StepAmplitude);
                    }
                }
            }
        }


        // Custom Road Input Vertical Velocity

        private void RoadVerticalVelocityCalculate()
        {

            if (ResponseNeedsToRecalculate)
            {
                if (RoadVerticalVelocity == null)
                {
                    RoadVerticalVelocity = new List<double>();
                }

                RoadVerticalVelocity.Clear();

                RoadVerticalVelocity.Add(0.0);

                for (int i = 1; i < TimeIntervals.Count; i++)
                {
                    double ZrDot = (RoadDisplacement[i - 1]) - RoadDisplacement[i] / (TimeIntervals[i - 1] - TimeIntervals[i]);

                    RoadVerticalVelocity.Add(ZrDot);
                }
            }
            

        } 

        private void MultipleStepIPCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (MultipleStepIPNeedsToRecalculate)
                {
                    if (RoadDisplacement == null)
                    {
                        RoadDisplacement = new List<double>();
                    }

                    RoadDisplacement.Clear();

                    for (double time = 0.0; time < StepStartTime; time += TimeStep)
                    {
                        RoadDisplacement.Add(0.0);
                    }


                    for (int count = 0; count < NumberOfSteps; count++)
                    {
                        //double a = 0.0;
                        //double d = a + stepAmplitude;

                        for (double time = StepStartTime; time <= StepStartTime + StepLength; time += TimeStep)
                        {
                            //StepInput.Add(d);
                            RoadDisplacement.Add(stepAmplitude);
                        }

                        //double c = a - stepAmplitude;
                        for (double time = StepStartTime + StepLength; time <= StepStartTime + StepLength + IntervalBetweenSteps; time += TimeStep)
                        {
                            //StepInput.Add(c);
                            RoadDisplacement.Add(0.0);
                        }
                    }

                    double StepInputEndTime = (StepLength + IntervalBetweenSteps) * NumberOfSteps + StepStartTime;

                    for (double time = StepInputEndTime; time <= EndTime; time += TimeStep)
                    {
                        RoadDisplacement.Add(0.0);
                    }
                }
            

            }
        }


        /*  X[dot]2 = -(c/m) X2 - (k/m) X1 + (c/m) Zr[Dot] + (k/m) Zr
         * 
         * 
         * 
         * 
         */
        private void ResponseCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (BodyDisplacement == null)
                {
                    BodyDisplacement = new List<double>();
                }

                if (BodyVelocity == null)
                {
                    BodyVelocity = new List<double>();
                }

                if (BodyAcceln == null)
                {
                    BodyAcceln = new List<double>();
                }

                BodyDisplacement.Clear();
                BodyVelocity.Clear();
                BodyAcceln.Clear();

                BodyDisplacement.Add(InitialDisplacement);
                BodyVelocity.Add(InitialVelocity);

                for (int i = 1; i < TimeIntervals.Count; i++)
                {
                    double A = DampingCoefficient / VehicleMass;
                    double B = SpringStiffness / VehicleMass;

                    double X2Dot = -(A * BodyVelocity[i - 1]) - (B * BodyDisplacement[i - 1]) + (A * RoadVerticalVelocity[i - 1]) + (B * RoadDisplacement[i - 1]);

                    BodyAcceln.Add(X2Dot);

                    double X2 = (BodyAcceln[i - 1] * (TimeIntervals[i] - TimeIntervals[i - 1])) + BodyVelocity[i - 1];

                    BodyVelocity.Add(X2);

                    double X1 = (BodyVelocity[i - 1] * (TimeIntervals[i] - TimeIntervals[i - 1])) + BodyDisplacement[i - 1];

                    BodyDisplacement.Add(X1);
                }

                ResponseCalculationComplete = true;

                ResponseNeedsToRecalculate = false;

            }
            
        }

        #region Spring Force Calculations


        private void SpringForceCalculate()
        {
            if (ResponseCalculationComplete)
            {
                if (SpringForce == null)
                {
                    SpringForce = new List<double>();
                }

                SpringForce.Clear();

                for (int i=0; i<RoadDisplacement.Count; i++)
                {
                    double Fs = SpringStiffness * (BodyDisplacement[i] - RoadDisplacement[i]);

                    SpringForce.Add(Fs);
                }
            }
        }

        #endregion

        #region Damper Force Calculations

        private void DamperForceCalculate()
        {
            if (ResponseCalculationComplete)
            {
                if (DamperForce == null)
                {
                    DamperForce = new List<double>();
                }

                DamperForce.Clear();

                for(int i = 0; i<BodyVelocity.Count; i++)
                {
                    double Fd = DampingCoefficient * (BodyVelocity[i] - RoadVerticalVelocity[i]);

                    DamperForce.Add(Fd);
                }
            }
        }

        #endregion

        #region BodyForce

        private void BodyForceCalculate()
        {
            if (ResponseCalculationComplete)
            {
                if (BodyForce == null)
                {
                    BodyForce = new List<double>();
                }

                BodyForce.Clear();

                foreach (double item in BodyAcceln)
                {
                    double BF = VehicleMass * item;

                    BodyForce.Add(BF);
                }
            }
        }

        #endregion

        public void InputDataCalculate()
        {
            if (TimeNeedsToRecalculate)
            {
                TimeCalculate();

                if (ResponseNeedsToRecalculate)
                {
                    if (SingleStepIPNeedsToRecalculate)
                    {
                        SingleStepIPCalculate();
                    }

                    else if (MultipleStepIPNeedsToRecalculate)
                    {
                        MultipleStepIPCalculate();
                    }
                }
            }


            TimeNeedsToRecalculate = false;
            MultipleStepIPNeedsToRecalculate = false;
            SingleStepIPNeedsToRecalculate = false;
            

            
        }

        public void OutputDataCalculate()
        {

            RoadVerticalVelocityCalculate();

            ResponseCalculate();

            SpringForceCalculate();

            DamperForceCalculate();

            BodyForceCalculate();

            ResponseNeedsToRecalculate = false;
            
        }
    }
}
