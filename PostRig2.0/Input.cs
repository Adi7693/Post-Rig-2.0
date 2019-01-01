using System;
using System.Collections.Generic;
using System.Numerics;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace Input
{
    public class InputData
    {
        public bool TimeNeedsToRecalculate;

        public bool ResponseNeedsToRecalculate;

        public bool SingleStepIPNeedsToRecalculate { get; set; }

        public bool MultipleStepIPNeedsToRecalculate { get; set; }

        public bool CustomIPCalculate { get; set; }

        public bool ResponseCalculationComplete { get; private set; }

        // Acceleration due to gravity
        readonly double g = 9.81;

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
            StepAmplitude = 0.005;
            StepLength = 1.0;
            IntervalBetweenSteps = 1.0;

            VehicleMass = 1.0;
            SpringStiffness = 1.0;
            DampingCoefficient = 1.0;

            ForceAmplitude = 0.0;

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
                //double c = CriticalDamping * DampingRatio;

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

        private double _dampingRatio;

        public double DampingRatio
        {
            get
            {
                //return DampingCoefficient / CriticalDamping;
                double Zeta = Math.Round(DampingCoefficient / CriticalDamping, 3);
                return Zeta;
            }

            //set
            //{
            //    if (!value.Equals(_dampingRatio))
            //    {
            //        _dampingRatio = value;
            //        ResponseNeedsToRecalculate = true ;
            //    }
            //}


        }

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


        // Step Signal List
        public List<double> SingleStepInput { get; private set; }

        public List<double> MultipleStepInput { get; private set; }


        // RoadDisplacement = Zr
        public List<double> RoadDisplacement { get; set; }

        // RoadVelocity = Zr[Dot]
        public List<double> RoadVerticalVelocity { get; private set; }


        public List<double> HomogeneousSolution { get; private set; }

        public List<double> ForceVector { get; private set; }

        public List<double> ResponseToUnitStepFunction { get; private set; } //x(t)

        public List<double> StepForceResponse { get; private set; } 

        public List<double> UnitStepFunctionVelocity { get; private set; } //xdot(t)

        public List<double> UnitStepFunctionAcceleration { get; private set; } //xdoubledot(t)

        // Body Displacment = Zb = X1
        public List<double> BodyDisplacement { get; private set; }

        // Body Velocity = Zb[Dot] = X1[dot] = X2
        public List<double> BodyVelocity { get; private set; }

        // Body Acceleration = Zb[Double Dot] = X2[Dot]
        public List<double> BodyAcceln { get; private set; }

        //Body Acceleration in G's
        public List<double> BodyAccelnG { get; private set; }


        public List<double> SpringForce { get; private set; }

        public List<double> DamperForce { get; private set; }

        public List<double> BodyForce { get; private set; }

        #endregion

        # region Time Frame Calculation
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
        #endregion

        #region Single Step Calculate
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
        #endregion

        #region Multiple Step Calculate
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

                        for (double time = StepStartTime; time < StepStartTime + StepLength; time += TimeStep)
                        {
                            //StepInput.Add(d);
                            RoadDisplacement.Add(stepAmplitude);
                        }

                        //double c = a - stepAmplitude;
                        for (double time = StepStartTime + StepLength; time < StepStartTime + StepLength + IntervalBetweenSteps; time += TimeStep)
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
        #endregion

        // Custom Road Input Vertical Velocity

        #region Road Vertical Velocity
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
                    double ZrDot = (RoadDisplacement[i - 1] - RoadDisplacement[i]) / (TimeIntervals[i - 1] - TimeIntervals[i]);

                    RoadVerticalVelocity.Add(ZrDot);
                }
            }
        }
        #endregion

        //  X[dot]2 = -(c/m) X2 - (k/m) X1 + (c/m) Zr[Dot] + (k/m) Zr

        //#region Response Calculation
        //private void ResponseCalculate()
        //{
        //    if (ResponseNeedsToRecalculate)
        //    {
        //        if (BodyDisplacement == null)
        //        {
        //            BodyDisplacement = new List<double>();
        //        }

        //        if (BodyVelocity == null)
        //        {
        //            BodyVelocity = new List<double>();
        //        }

        //        if (BodyAcceln == null)
        //        {
        //            BodyAcceln = new List<double>();
        //        }

        //        BodyDisplacement.Clear();
        //        BodyVelocity.Clear();
        //        BodyAcceln.Clear();

        //        BodyDisplacement.Add(0.0);
        //        BodyVelocity.Add(0.0);
        //        BodyAcceln.Add(0.0);

        //        for (int i = 1; i < TimeIntervals.Count; i++)
        //        {
        //            double A = DampingCoefficient / VehicleMass;
        //            double B = SpringStiffness / VehicleMass;

        //            double X2Dot = -(A * BodyVelocity[i - 1]) - (B * BodyDisplacement[i - 1]) + (A * RoadVerticalVelocity[i - 1]) + (B * RoadDisplacement[i - 1]);

        //            BodyAcceln.Add(X2Dot);

        //            double X2 = (BodyAcceln[i - 1] * (TimeIntervals[i] - TimeIntervals[i - 1])) + BodyVelocity[i - 1];

        //            BodyVelocity.Add(X2);

        //            double X1 = (BodyVelocity[i - 1] * (TimeIntervals[i] - TimeIntervals[i - 1])) + BodyDisplacement[i - 1];

        //            BodyDisplacement.Add(X1);
        //        }

        //        ResponseCalculationComplete = true;

        //    }

        //}
        //#endregion




        #region Road Step To Impulse force on body
        private double ImpulseForceDueToStep
        {
            get
            {
                double F0 = (StepAmplitude * SpringStiffness) + (StepAmplitude / TimeStep) * DampingCoefficient;
                return F0;
            }
        }

        private void ModelResponse()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (ResponseToUnitStepFunction == null)
                {
                    ResponseToUnitStepFunction = new List<double>();
                }

                ResponseToUnitStepFunction.Clear();




                foreach (double item in TimeIntervals)
                {
                    if (DampingRatio < 1.0)
                    {
                        double xt = ImpulseForceDueToStep * Math.Exp(-DampingRatio * NaturalFrequencyRad * item) * Math.Sin(DampedNaturalFrequency * item) / (VehicleMass * DampedNaturalFrequency);

                        ResponseToUnitStepFunction.Add(xt);
                    }

                    else if (DampingRatio == 1.0)
                    {
                        double xt = ImpulseForceDueToStep * Math.Exp(-NaturalFrequencyRad * item) * item / VehicleMass;

                        ResponseToUnitStepFunction.Add(xt);
                    }

                    else if (DampingRatio > 1.0)
                    {
                        double C1 = ImpulseForceDueToStep / (2.0 * VehicleMass * NaturalFrequencyRad * Math.Sqrt(Math.Pow(DampingRatio, 2) - 1));

                        double C2 = -ImpulseForceDueToStep / (2.0 * VehicleMass * NaturalFrequencyRad * Math.Sqrt(Math.Pow(DampingRatio, 2) - 1));

                        double xt = (C1 * Math.Exp((-DampingRatio + Math.Sqrt(Math.Pow(DampingRatio, 2) - 1)) * NaturalFrequencyRad * item)) + (C2 * Math.Exp((-DampingRatio - Math.Sqrt(Math.Pow(DampingRatio, 2) - 1)) * NaturalFrequencyRad * item));

                        ResponseToUnitStepFunction.Add(xt);


                    }
                }

            }
        }


        private void HomogeneousSolutionCalculation()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (HomogeneousSolution == null)
                {
                    HomogeneousSolution = new List<double>();
                }

                HomogeneousSolution.Clear();


                //for(double i = 0.0; i < StepStartTime; i+= TimeStep)
                //{
                //    HomogeneousSolution.Add(0.0);
                //}

                foreach (double item in TimeIntervals)
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
                        HomogeneousSolution.Add(x);
                    }

                    else if (DampingRatio == 1.0)
                    {
                        double C1 = InitialDisplacement;
                        double C2 = InitialVelocity + (NaturalFrequencyRad * InitialDisplacement);

                        double x = (C1 + (C2 * item)) * Math.Exp(-NaturalFrequencyRad * item);
                        HomogeneousSolution.Add(x);
                    }

                    else if (DampingRatio > 1.0)
                    {
                        double C1 = (InitialDisplacement * NaturalFrequencyRad * (DampingRatio + Math.Sqrt(Math.Pow(DampingRatio, 2) - 1)) + InitialVelocity) / (2 * NaturalFrequencyRad * Math.Sqrt(Math.Pow(DampingRatio, 2) - 1));
                        double C2 = (-InitialDisplacement * NaturalFrequencyRad * (DampingRatio - Math.Sqrt(Math.Pow(DampingRatio, 2) - 1)) - InitialVelocity) / (2 * NaturalFrequencyRad * Math.Sqrt(Math.Pow(DampingRatio, 2) - 1));

                        double x = (C1 * Math.Exp((-DampingRatio + Math.Sqrt(Math.Pow(DampingRatio, 2) - 1)) * NaturalFrequencyRad * item)) + (C2 * Math.Exp((-DampingRatio - Math.Sqrt(Math.Pow(DampingRatio, 2) - 1)) * NaturalFrequencyRad * item));
                        HomogeneousSolution.Add(x);
                    }
                }


            }
        }

        #endregion

        private double Phy
        {
            get
            {
                double a = Math.Atan(DampingRatio / Math.Sqrt(1 - Math.Pow(DampingRatio, 2)));

                return a;
            }
        }

        public double ForceAmplitude { get; set; }


        private void ResponseUnitToStepFunctionCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (ResponseToUnitStepFunction == null)
                {
                    ResponseToUnitStepFunction = new List<double>();
                }

                ResponseToUnitStepFunction.Clear();

                for (int i = 0; i < TimeIntervals.Count; i++)
                {

                    if (DampingRatio < 1.0)
                    {
                        double xt = 1.0 / SpringStiffness * (1.0 - (Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]) * Math.Cos(DampedNaturalFrequency * TimeIntervals[i] - Phy) * (1.0 / Math.Sqrt(1 - Math.Pow(DampingRatio, 2)))));

                        ResponseToUnitStepFunction.Add(xt);
                    }

                    else if (DampingRatio == 1.0)
                    {
                        double xt = (-StepAmplitude * Math.Exp(-NaturalFrequencyRad * TimeIntervals[i]) / SpringStiffness) * (1 + (TimeIntervals[i] * NaturalFrequencyRad));

                        ResponseToUnitStepFunction.Add(xt);
                    }

                    else if (DampingRatio > 1.0)
                    {
                        double a = Math.Sqrt(Math.Pow(DampingRatio, 2) - 1);

                        double _a1 = -DampingRatio + a;
                        double _a2 = -DampingRatio - a;

                        double A = 1 / (2 * SpringStiffness * NaturalFrequencyRad * a);

                        double B = _a2 * NaturalFrequencyRad * (Math.Exp(_a1 * NaturalFrequencyRad * TimeIntervals[i]) - 1);

                        double C = _a1 * NaturalFrequencyRad * (Math.Exp(_a2 * NaturalFrequencyRad * TimeIntervals[i]) - 1);

                        double xt = A * (B - C);

                        ResponseToUnitStepFunction.Add(xt);

                    }
                }
            }
        }


        private void UnitStepVelocityCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if(UnitStepFunctionVelocity == null)
                {
                    UnitStepFunctionVelocity = new List<double>();
                }

                UnitStepFunctionVelocity.Clear();

                for(int i = 0; i< TimeIntervals.Count; i++)
                {
                    if(DampingRatio < 1.0)
                    {
                        double A = 1.0 / (SpringStiffness*(Math.Sqrt(1.0 - Math.Pow(DampingRatio, 2))));

                        double B = DampedNaturalFrequency * Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]) * Math.Sin(DampedNaturalFrequency * TimeIntervals[i] - Phy);

                        double C = DampingRatio * NaturalFrequencyRad * Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]) * Math.Cos(DampedNaturalFrequency * TimeIntervals[i] - Phy);

                        double xDotT = A * (B + C);

                        UnitStepFunctionVelocity.Add(xDotT);
                    }

                    else if (DampingRatio > 1)
                    {
                        double a_ = Math.Sqrt(Math.Pow(DampingRatio, 2) - 1);

                        double A = NaturalFrequencyRad / (2.0 * SpringStiffness * a_);

                        double _b1 = -DampingRatio + a_;

                        double _c1 = -DampingRatio - a_;

                        double B = Math.Exp(_b1 * NaturalFrequencyRad * TimeIntervals[i]);

                        double C = Math.Exp(_c1 * NaturalFrequencyRad * TimeIntervals[i]);

                        double xDotT = A * (B - C);

                        UnitStepFunctionVelocity.Add(xDotT);
                    }
                }
            }
        }


        private void UnitStepAccelerationCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (UnitStepFunctionAcceleration == null)
                {
                    UnitStepFunctionAcceleration = new List<double>();
                }

                UnitStepFunctionAcceleration.Clear();

                for (int i = 0; i < TimeIntervals.Count; i++)
                {
                    if (DampingRatio < 1.0)
                    {
                        double A = 1.0 / (SpringStiffness*(Math.Sqrt(1.0 - Math.Pow(DampingRatio, 2))));

                        
                        double _b1 = DampedNaturalFrequency * Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]) * Math.Cos(DampedNaturalFrequency * TimeIntervals[i] - Phy);

                        double _b2 = DampingRatio * NaturalFrequencyRad * Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]) * Math.Sin(DampedNaturalFrequency * TimeIntervals[i] - Phy);


                        double B = DampedNaturalFrequency*(_b1 - _b2);


                        double _c1 = DampedNaturalFrequency * Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]) * Math.Sin(DampedNaturalFrequency * TimeIntervals[i] - Phy);

                        double _c2 = DampingRatio * NaturalFrequencyRad * Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]) * Math.Sin(DampedNaturalFrequency * TimeIntervals[i] - Phy);


                        double C = -DampingRatio * NaturalFrequencyRad * (_c1 + _c2);

                        double xDDotT = A * (B + C);

                        UnitStepFunctionAcceleration.Add(xDDotT);
                    }

                    if (DampingRatio > 1.0)
                    {
                        double a_ = Math.Sqrt(Math.Pow(DampingRatio, 2) - 1);

                        double A = 1.0 / (2.0 * VehicleMass * a_);

                        double _b1 = -DampingRatio + a_;

                        double _c1 = -DampingRatio - a_;

                        double B = _b1 * Math.Exp(_b1 * NaturalFrequencyRad * TimeIntervals[i]);

                        double C = _c1 * Math.Exp(_c1 * NaturalFrequencyRad * TimeIntervals[i]);

                        double xDDotT = A * (B - C);

                        UnitStepFunctionAcceleration.Add(xDDotT);
                    }

                    
                }
            }
        }


        

        public void ForceVectorCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (ForceVector == null)
                {
                    ForceVector = new List<double>();
                }

                ForceVector.Clear();

                for (int i = 0; i < TimeIntervals.Count; i++)
                {
                    ForceVector.Add(ForceAmplitude);
                }
            }
        }



        private void BodyVelocityCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if(BodyVelocity == null)
                {
                    BodyVelocity = new List<double>();
                }

                BodyVelocity.Clear();

                for(int i = 0; i< ForceVector.Count; i++)
                {
                    double a = ForceVector[i] * UnitStepFunctionVelocity[i];

                    BodyVelocity.Add(a);

                }
            }
        }

        private void BodyAccelnCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (BodyAccelnG == null)
                {
                    BodyAccelnG = new List<double>();
                }

                BodyAccelnG.Clear();

                for (int i = 0; i < ForceVector.Count; i++)
                {
                    double a = ForceVector[i] * UnitStepFunctionAcceleration[i] / 9.81;

                    BodyAccelnG.Add(a);
                }
            }
        }


       

        private void StepForceResponseCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (StepForceResponse == null)
                {
                    StepForceResponse = new List<double>();
                }

                StepForceResponse.Clear();

                for (int i = 0; i < ForceVector.Count; i++)
                {
                    double a = ForceVector[i] * ResponseToUnitStepFunction[i];

                    StepForceResponse.Add(a);
                }
            }
        }

        #region Spring Force Calculations

        private void SpringForceCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (SpringForce == null)
                {
                    SpringForce = new List<double>();
                }

                SpringForce.Clear();

                for (int i = 0; i < StepForceResponse.Count; i++)
                {
                    double Fs = SpringStiffness * StepForceResponse[i];

                    SpringForce.Add(Fs);
                }
            }
        }

        #endregion

        #region Damper Force Calculations

        private void DamperForceCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (DamperForce == null)
                {
                    DamperForce = new List<double>();
                }

                DamperForce.Clear();

                for (int i = 0; i < BodyVelocity.Count; i++)
                {
                    double Fd = DampingCoefficient * BodyVelocity[i];

                    DamperForce.Add(Fd);
                }
            }
        }

        #endregion



        //#region Body Acceleration
        //private void BodyAccelnGCalculate()
        //{
        //    if (ResponseNeedsToRecalculate)
        //    {
        //        if (BodyAccelnG == null)
        //        {
        //            BodyAccelnG = new List<double>();
        //        }

        //        BodyAccelnG.Clear();

        //        for (int i =0; i<ForceVector.Count;i++)
        //        {
        //            double G = ForceVector[i] * UnitStepFunctionAcceleration[i];

        //            BodyAccelnG.Add(G);
        //        }
        //    }
        //}
        //#endregion

        //#region BodyForce

        //private void BodyForceCalculate()
        //{
        //    if (ResponseNeedsToRecalculate)
        //    {
        //        if (BodyForce == null)
        //        {
        //            BodyForce = new List<double>();
        //        }

        //        BodyForce.Clear();

        //        foreach (double item in BodyAcceln)
        //        {
        //            double BF = VehicleMass * item;

        //            BodyForce.Add(BF);
        //        }
        //    }
        //}

        //#endregion


        #region Input Data Calculation
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
                        ForceVectorCalculate();


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
        #endregion

        #region Output Data Calculation
        public void OutputDataCalculate()
        {

            if (ResponseNeedsToRecalculate)
            {
                //RoadVerticalVelocityCalculate();

                //ResponseCalculate();

                ResponseUnitToStepFunctionCalculate();
                UnitStepVelocityCalculate();
                UnitStepAccelerationCalculate();


                StepForceResponseCalculate();
                BodyVelocityCalculate();
                BodyAccelnCalculate();

                



                //ModelResponse();

                //BodyAccelnGCalculate();

                SpringForceCalculate();

                DamperForceCalculate();

                //BodyForceCalculate();
            }

            ResponseCalculationComplete = true;

            ResponseNeedsToRecalculate = false;

        }
        #endregion


        private double springFreeLength;

        public double SpringFreeLength
        {
            get
            {
                return springFreeLength;
            }

            set
            {
                if (!value.Equals(springFreeLength))
                {
                    springFreeLength = value;
                }
            }
        }


        private double springCompressedLength;

        public double SpringCompressedLength
        {
            get
            {
                return springCompressedLength;
            }

            set
            {
                if (value < springFreeLength)
                {
                    if (!value.Equals(SpringCompressedLength))
                    {
                        springCompressedLength = value;
                    }
                }

                else
                {
                    // Display error if Compressed Length is not less than Free lenght
                }


            }
        }

        private double springExtendedLength;

        public double SpringExtendedLength
        {
            get
            {
                return springExtendedLength;
            }

            set
            {

                if (value > springFreeLength)
                {
                    if (!value.Equals(springExtendedLength))
                    {
                        springExtendedLength = value;
                    }
                }

                else
                {
                    // Display Error if Extended Length is not greater than Free Length
                }


            }
        }

        // Spring Deflection due to mass on top.

        public double LoadedSpringLength
        {
            get
            {
                double DeltaL = VehicleMass * g / SpringStiffness;

                return DeltaL;
            }

        }

        // Static Spring Deflection

        public double StaticSpringDeflection
        {
            get
            {
                return SpringFreeLength - LoadedSpringLength;
            }

            //private set
            //{
            //    double ls = SpringFreeLength - LoadedSpringLength;

            //    if (ls > springCompressedLength)
            //    {
            //        staticSpringDeflection = ls;
            //    }

            //    else
            //    {
            //        // Error if ls < CompressedLength, Suspension Collapsed
            //    }
            //}
        }

        public double StaticSpringForce
        {
            get
            {
                return StaticSpringDeflection * SpringStiffness;
            }

            //private set
            //{
            //    double SSF = 

            //    staticSpringForce = SSF;
            //}
        }






        // Attempt To carry out state space modelling

        #region State Space Modelling Quarter Car Model
        private double Sm { get; } //Sprung Mass

        private double USm { get; } // Unsprung Mass

        private double Ks { get; } // Suspension Spring Stiffness

        private double Cs { get; } // Suspension Damping Coefficient

        private double Kt { get; } // Tyre Vertical Stiffness

        private List<double> Zr { get; } // Road Displacement

        private List<double> Zs { get; } // Sprung Mass Displacement

        private List<double> Za { get; } // Unsprung Mass Displacement

        private List<double> ZDotS { get; } // Sprung Mass Vertical Velocity

        private List<double> ZdotA { get; } // Unsprung Vertical Velcoity

        private List<double> ZdDotS { get; } // Sprung Mass Vertical Acceln

        private List<double> ZdDotA { get; } // Unsprung Mass Vertical Acceln

        private List<double> DeltaFz { get; } // Dynamic Tyre Load

        private List<double> DeltaZ { get; } // Suspension Travel





        private Matrix<double> SystemMatrix
        {
            get
            {
                if (SystemMatrix == null)
                {
                    SystemMatrix = Matrix<double>.Build.Dense(4, 4);
                }

                return SystemMatrix;
            }

            set
            {
                SystemMatrix[0, 0] = -Cs / Sm;
                SystemMatrix[0, 1] = Cs / Sm;
                SystemMatrix[0, 2] = -Ks / Sm;
                SystemMatrix[0, 3] = Ks / Sm;

                SystemMatrix[1, 0] = Cs / USm;
                SystemMatrix[1, 1] = -Cs / USm;
                SystemMatrix[1, 2] = Ks / USm;
                SystemMatrix[1, 3] = -Ks / USm;

                SystemMatrix[2, 0] = 1.0;
                SystemMatrix[2, 1] = 0.0;
                SystemMatrix[2, 2] = 0.0;
                SystemMatrix[2, 3] = 0.0;

                SystemMatrix[3, 0] = 0.0;
                SystemMatrix[3, 1] = 1.0;
                SystemMatrix[3, 2] = 0.0;
                SystemMatrix[3, 3] = 0.0;

            }
        }

        private Vector<double> StateVector
        {
            get
            {


                return StateVector;
            }

            set
            {
                for (int i = 0; i < Zr.Count; i++)
                {

                }


            }
        }

        private Vector<double> InputVector
        {
            get
            {
                if (InputVector == null)
                {
                    InputVector = Vector<double>.Build.Dense(4);
                }

                return InputVector;
            }

            set
            {
                InputVector[0] = 0.0;

                InputVector[1] = -Kt / USm;

                InputVector[2] = 0.0;

                InputVector[3] = 0.0;

            }
        }


        private Vector<double> OutputVector
        {
            get
            {
                if (OutputVector == null)
                {
                    OutputVector = Vector<double>.Build.Dense(6);
                }

                return OutputVector;
            }

            set
            {



            }
        }

        private Matrix<double> OutputMatrix
        {
            get
            {


                return OutputMatrix;
            }

            set
            {

            }
        }

        private Vector<double> DVector
        {
            get
            {


                return DVector;

            }

            set
            {
                DVector[0] = 0.0;

                DVector[1] = Kt / USm;

                DVector[2] = 0.0;

                DVector[3] = 0.0;

                DVector[4] = 0.0;

                DVector[5] = 0.0;

            }
        }



        private void StateSpace()
        {
            if (StateVector == null)
            {
                StateVector = Vector<double>.Build.Dense(4);
            }

            if (OutputMatrix == null)
            {
                OutputMatrix = Matrix<double>.Build.Dense(6, 4);
            }

            if (DVector == null)
            {
                DVector = Vector<double>.Build.Dense(6);
            }



            OutputMatrix[0, 0] = -Cs / Sm;
            OutputMatrix[0, 1] = Cs / Sm;
            OutputMatrix[0, 3] = -Ks / Sm;
            OutputMatrix[0, 4] = Ks / Sm;

            OutputMatrix[1, 0] = Cs / USm;
            OutputMatrix[1, 1] = -Cs / USm;
            OutputMatrix[1, 3] = Ks / USm;
            OutputMatrix[1, 4] = -Ks / USm;

            OutputMatrix[2, 0] = 0.0;
            OutputMatrix[2, 1] = 0.0;
            OutputMatrix[2, 3] = 0.0;
            OutputMatrix[2, 4] = -Kt;

            OutputMatrix[3, 0] = 0.0;
            OutputMatrix[3, 1] = 0.0;
            OutputMatrix[3, 3] = -1.0;
            OutputMatrix[3, 4] = 1.0;

            OutputMatrix[4, 0] = 0.0;
            OutputMatrix[4, 1] = 0.0;
            OutputMatrix[4, 3] = 1.0;
            OutputMatrix[4, 4] = 0.0;

            OutputMatrix[5, 0] = 0.0;
            OutputMatrix[5, 1] = 0.0;
            OutputMatrix[5, 3] = 0.0;
            OutputMatrix[5, 4] = 1.0;


            for (int i = 0; i < Zr.Count; i++)
            {
                StateVector[0] = ZDotS[i];

                StateVector[1] = ZdotA[i];

                StateVector[2] = Zs[i];

                StateVector[3] = Za[i];


                OutputVector[0] = ZdDotS[i];

                OutputVector[1] = ZdDotA[i];

                OutputVector[2] = DeltaFz[i];

                OutputVector[3] = DeltaZ[i];

                OutputVector[4] = Zs[i];

                OutputVector[5] = Za[i];
            }


        }

        #endregion

    }
}