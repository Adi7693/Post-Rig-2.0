﻿using System;
using System.Collections.Generic;
using System.Numerics;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace PostRig2_0
{
    public class InputData
    {
        public bool TimeNeedsToRecalculate;

        public bool ResponseNeedsToRecalculate;

        public bool StepIPNeedsToRecalculate { get; set; }

        public bool HarmonicIPNeedsToRecalculate { get; set; }

        public bool CustomIPCalculate { get; set; }

        public bool ResponseCalculationComplete { get; private set; }

        // Acceleration due to gravity
        readonly double g = 9.81;

        public CarData Car { get; set; }

        public SimData Sim { get; set; }

        #region Constructor

        public InputData()
        {
            Car = new CarData(this);

            Sim = new SimData(this);


            TimeNeedsToRecalculate = true;
            StepIPNeedsToRecalculate = false;
            HarmonicIPNeedsToRecalculate = false;
            CustomIPCalculate = false;
            ResponseNeedsToRecalculate = false;
            ResponseCalculationComplete = false;



            VehicleMass = 1.0;
            SpringStiffness = 1.0;
            DampingCoefficient = 1.0;

            SpringFreeLength = 1.0;
            SpringExtendedLength = 1.0;
            SpringCompressedLength = 1.0;

            StartTime = 0.0;
            EndTime = 5.0;
            TimeStep = 0.01;

            ForceAmplitude = 0.0;

            InitialDisplacement = 0.0;
            InitialVelocity = 0.0;
        }
        #endregion

        public InputData(CarData car)
        {
            Car = new CarData(this);

            VehicleMass = 1.0;
            SpringStiffness = 1.0;
            DampingCoefficient = 1.0;

            SpringFreeLength = 1.0;
            SpringExtendedLength = 1.0;
            SpringCompressedLength = 1.0;

            TimeNeedsToRecalculate = true;
            StepIPNeedsToRecalculate = false;
            HarmonicIPNeedsToRecalculate = false;
            CustomIPCalculate = false;
            ResponseNeedsToRecalculate = false;
            ResponseCalculationComplete = false;
        }
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


        // In Kg

        public double VehicleMass
        {
            get
            {
                return Car.VehicleMass;
            }

            set
            {
                if (!value.Equals(Car.VehicleMass))
                {
                    if (value > 0)
                    {
                        Car.VehicleMass = value;
                        ResponseNeedsToRecalculate = true;

                    }
                }
            }
        }


        // In N/m
        public double SpringStiffness
        {
            get
            {
                return Car.SpringStiffness;
            }

            set
            {
                if (!value.Equals(Car.SpringStiffness))
                {
                    Car.SpringStiffness = value;
                    ResponseNeedsToRecalculate = true;

                }

            }
        }




        // In N/(m/s)
        public double DampingCoefficient
        {
            get
            {
                //double c = CriticalDamping * DampingRatio;

                return Car.DampingCoefficient;
            }

            set
            {
                if (!value.Equals(Car.DampingCoefficient))
                {
                    Car.DampingCoefficient = value;
                    ResponseNeedsToRecalculate = true;
                }
            }

        }



        private double excitationFrequencyHz;

        public double ExcitationFrequencyHz
        {
            get
            {
                return excitationFrequencyHz;
            }

            set
            {
                if (!value.Equals(excitationFrequencyHz))
                {
                    excitationFrequencyHz = value;
                    ResponseNeedsToRecalculate = true; ;
                }
            }
        }

        public double SpringFreeLength
        {
            get
            {
                return Car.SpringFreeLength;
            }

            set
            {
                if (!value.Equals(Car.SpringFreeLength))
                {
                    Car.SpringFreeLength = value;
                }
            }
        }



        public double SpringCompressedLength
        {
            get
            {
                return Car.SpringCompressedLength;
            }

            set
            {
                if (value < SpringFreeLength)
                {
                    if (!value.Equals(SpringCompressedLength))
                    {
                        Car.SpringCompressedLength = value;
                    }
                }

                else
                {
                    // Display error if Compressed Length is not less than Free lenght
                }


            }
        }


        public double SpringExtendedLength
        {
            get
            {
                return Car.SpringExtendedLength;
            }

            set
            {

                if (value > SpringFreeLength)
                {
                    if (!value.Equals(Car.SpringExtendedLength))
                    {
                        Car.SpringExtendedLength = value;
                    }
                }

                else
                {
                    // Display Error if Extended Length is not greater than Free Length
                }


            }
        }


        private double forceAmplitude;

        public double ForceAmplitude
        {
            get
            {
                return forceAmplitude;
            }

            set
            {
                if (!value.Equals(forceAmplitude))
                {
                    forceAmplitude = value;
                    ResponseNeedsToRecalculate = true;
                }
            }
        }


        #endregion




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

        public double DampingRatio
        {
            get
            {
                double Zeta = Math.Round(DampingCoefficient / CriticalDamping, 3);
                return Zeta;
            }
        }




        #region Derived Properties

        // In rad/s
        public double NaturalFrequencyRad
        {
            get
            {

                double wn = Math.Round(Math.Sqrt(SpringStiffness / VehicleMass), 3);
                return wn;
            }

        }

        // In Hz
        public double NaturalFrequencyHz
        {
            get
            {

                double Fn = Math.Round(NaturalFrequencyRad / (2.0 * Math.PI), 3);
                return Fn;
            }
        }

        // In N/(m/s)
        public double CriticalDamping
        {
            get
            {
                double Cc = Math.Round(2.0 * Math.Sqrt(VehicleMass * SpringStiffness), 3);
                return Cc;
            }
        }

        public double DampedNaturalFrequency
        {
            get
            {
                double wd = Math.Round(Math.Sqrt(1.0 - Math.Pow(DampingRatio, 2)) * NaturalFrequencyRad, 4);
                return wd;
            }
        }



        private double ExcitationFrequencyRad
        {
            get
            {
                return 2.0 * Math.PI * ExcitationFrequencyHz;
            }
        }

        private double FrequencyRatio
        {
            get
            {
                return ExcitationFrequencyRad / NaturalFrequencyRad;
            }
        }

        // Deflection Under Static Load

        private double DeltaST
        {
            get
            {
                return ForceAmplitude / SpringStiffness;
            }
        }


        private double X
        {
            get
            {
                double tf1 = Math.Pow(1.0 - Math.Pow(FrequencyRatio, 2), 2);
                double tf2 = Math.Pow(2.0 * DampingRatio * FrequencyRatio, 2);

                double X = DeltaST / Math.Sqrt(tf1 + tf2);

                return X;
            }
        }

        private double PhyStep
        {
            get
            {
                double a = Math.Atan(DampingRatio / Math.Sqrt(1 - Math.Pow(DampingRatio, 2)));

                return a;
            }
        }


        private double PhyHarmonic
        {
            get
            {
                double a = 2.0 * DampingRatio * FrequencyRatio;

                double b = 1.0 - Math.Pow(FrequencyRatio, 2);

                double phyH = Math.Atan(a / b);

                return phyH;
            }
        }



        #endregion

        #region Lists of Data

        // Time Frame Data List

        public List<double> TimeIntervals { get; set; }

        // Harmonic Input Signal

        public List<double> HarmonicInput { get; private set; }

        // Step Vector

        public List<double> ForceVector { get; private set; }

        // RoadDisplacement = Zr
        public List<double> RoadDisplacement { get; set; }

        // RoadVelocity = Zr[Dot]
        public List<double> RoadVerticalVelocity { get; private set; }

        // Unit Step Input

        public List<double> ResponseToUnitStepFunction { get; private set; }            
                                                                                        
        public List<double> StepForceResponse { get; private set; }                     
                                                                                        
        public List<double> UnitStepFunctionVelocity { get; private set; }              
                                                                                        
        public List<double> UnitStepFunctionAcceleration { get; private set; }          



        public List<double> HomogeneousSolution { get; private set; }

        public List<double> ParticularSolution { get; private set; }

        public List<double> TotalResponse { get; private set; }


        public List<double> BodyVelocity { get; private set; }

        public List<double> BodyAcceln { get; private set; }

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

                for (double i = StartTime; i <= EndTime + TimeStep / 2.0; i += TimeStep)
                {
                    double interval = Math.Round(i, 6);
                    TimeIntervals.Add(interval);
                }

                TimeNeedsToRecalculate = false;
            }
        }
        #endregion

        private void HarmonicInputCalcualte()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (HarmonicInput == null)
                {
                    HarmonicInput = new List<double>();
                }

                HarmonicInput.Clear();

                for (int i = 0; i < TimeIntervals.Count; i++)
                {
                    double hIP = ForceAmplitude * Math.Cos(ExcitationFrequencyRad * TimeIntervals[i]);

                    HarmonicInput.Add(hIP);
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

        #region Road Vertical Velocity

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
                double F0 = (ForceAmplitude * SpringStiffness) + (ForceAmplitude / TimeStep) * DampingCoefficient;
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

        #endregion


        private void ResponseToUnitStepFunctionCalculate()
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
                        double xt = 1.0 / SpringStiffness * (1.0 - (Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]) * Math.Cos(DampedNaturalFrequency * TimeIntervals[i] - PhyStep) * (1.0 / Math.Sqrt(1 - Math.Pow(DampingRatio, 2)))));

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


        private void StepForceResponseCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (StepForceResponse == null)
                {
                    StepForceResponse = new List<double>();
                }

                StepForceResponse.Clear();

                for (int i = 0; i < TimeIntervals.Count; i++)
                {
                    double a = ForceVector[i] * ResponseToUnitStepFunction[i];

                    StepForceResponse.Add(a);
                }
            }
        }


        private void UnitStepVelocityCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (UnitStepFunctionVelocity == null)
                {
                    UnitStepFunctionVelocity = new List<double>();
                }

                UnitStepFunctionVelocity.Clear();

                for (int i = 0; i < TimeIntervals.Count; i++)
                {
                    if (DampingRatio < 1.0)
                    {
                        double A = 1.0 / (SpringStiffness * (Math.Sqrt(1.0 - Math.Pow(DampingRatio, 2))));

                        double B = DampedNaturalFrequency * Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]) * Math.Sin(DampedNaturalFrequency * TimeIntervals[i] - PhyStep);

                        double C = DampingRatio * NaturalFrequencyRad * Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]) * Math.Cos(DampedNaturalFrequency * TimeIntervals[i] - PhyStep);

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
                        double A = 1.0 / (SpringStiffness * (Math.Sqrt(1.0 - Math.Pow(DampingRatio, 2))));


                        double _b1 = DampedNaturalFrequency * Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]) * Math.Cos(DampedNaturalFrequency * TimeIntervals[i] - PhyStep);

                        double _b2 = DampingRatio * NaturalFrequencyRad * Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]) * Math.Sin(DampedNaturalFrequency * TimeIntervals[i] - PhyStep);

                        double B = DampedNaturalFrequency * (_b1 - _b2);


                        double _c1 = DampedNaturalFrequency * Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]) * Math.Sin(DampedNaturalFrequency * TimeIntervals[i] - PhyStep);

                        double _c2 = DampingRatio * NaturalFrequencyRad * Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]) * Math.Sin(DampedNaturalFrequency * TimeIntervals[i] - PhyStep);


                        double C = -DampingRatio * NaturalFrequencyRad * (_c1 + _c2);


                        double xDDotT = A * (B + C);

                        UnitStepFunctionAcceleration.Add(xDDotT);
                    }

                    else if (DampingRatio > 1.0)
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


        private void HomogeneousSolutionCalculation()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (HomogeneousSolution == null)
                {
                    HomogeneousSolution = new List<double>();
                }

                HomogeneousSolution.Clear();

                for(int i = 0; i<TimeIntervals.Count;i++)
                {

                    if (DampingRatio < 1.0)
                    {

                        if (StepIPNeedsToRecalculate)
                        {
                            double C1 = InitialDisplacement;

                            double C2 = (InitialVelocity + (DampingRatio * NaturalFrequencyRad * InitialDisplacement)) / DampedNaturalFrequency;

                            double xtHomo = Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]) * (C1 * Math.Cos(DampedNaturalFrequency * TimeIntervals[i]) + C2 * Math.Sin(DampedNaturalFrequency * TimeIntervals[i]));

                            HomogeneousSolution.Add(xtHomo);
                        }

                        else if (HarmonicIPNeedsToRecalculate)
                        {
                            double a = InitialDisplacement - (X* Math.Cos(PhyHarmonic));
                            double A = Math.Pow(a, 2);

                            double _b = DampingRatio * NaturalFrequencyRad;
                            double b1 = _b * InitialDisplacement;
                            double b2 = _b * X * Math.Cos(PhyHarmonic);
                            double b3 = ExcitationFrequencyRad * X * Math.Sin(PhyHarmonic);
                            double b = (b1 + InitialDisplacement - b2 - b3)/DampedNaturalFrequency;
                            double B = Math.Pow(b, 2);

                            double X0 = Math.Sqrt(A + B);

                            double phy0 = Math.Atan(b / a);

                            double xtHomo = X0 * Math.Exp(-_b * TimeIntervals[i]) * Math.Cos(DampedNaturalFrequency * TimeIntervals[i] - phy0);

                            HomogeneousSolution.Add(xtHomo);

                        }
                    }

                    else if (DampingRatio > 1.0)
                    {

                        if (StepIPNeedsToRecalculate )
                        {
                            double C1 = (InitialDisplacement * NaturalFrequencyRad * (DampingRatio + Math.Sqrt(Math.Pow(DampingRatio, 2) - 1)) + InitialVelocity) / (2 * NaturalFrequencyRad * Math.Sqrt(Math.Pow(DampingRatio, 2) - 1));
                            double C2 = (-InitialDisplacement * NaturalFrequencyRad * (DampingRatio - Math.Sqrt(Math.Pow(DampingRatio, 2) - 1)) - InitialVelocity) / (2 * NaturalFrequencyRad * Math.Sqrt(Math.Pow(DampingRatio, 2) - 1));

                            double xtHomo = (C1 * Math.Exp((-DampingRatio + Math.Sqrt(Math.Pow(DampingRatio, 2) - 1)) * NaturalFrequencyRad * TimeIntervals[i])) + (C2 * Math.Exp((-DampingRatio - Math.Sqrt(Math.Pow(DampingRatio, 2) - 1)) * NaturalFrequencyRad * TimeIntervals[i]));
                            HomogeneousSolution.Add(xtHomo);
                        }

                        else if (HarmonicIPNeedsToRecalculate)
                        {
                            double a = Math.Sqrt(Math.Pow(DampingRatio, 2) - 1);

                            double D = 2.0 * NaturalFrequencyRad * a;

                            double _c1 = 3.0 * a - DampingRatio;
                            double c11 = NaturalFrequencyRad * InitialDisplacement * _c1;
                            double c12 = ExcitationFrequencyRad * X * Math.Sin(PhyHarmonic);


                            double C1 = (c11 + c12 - InitialVelocity)/D;

                            double c2 = (-DampingRatio + a) * NaturalFrequencyRad*InitialDisplacement;

                            double C2 = (InitialVelocity - c2 - c12) / D;

                            double exp1 = -DampingRatio + a;
                            double exp2 = -DampingRatio - a;

                            double xtHomo = C1 * Math.Exp(exp1 * NaturalFrequencyRad * TimeIntervals[i]) + C2 * Math.Exp(exp2 * NaturalFrequencyRad * TimeIntervals[i]);

                            HomogeneousSolution.Add(xtHomo);
                        }
                    }
                }
            }
        }


        private void ParticularSolutionCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if(ParticularSolution == null)
                {
                    ParticularSolution = new List<double>();
                }

                ParticularSolution.Clear();

                for(int i = 0; i<TimeIntervals.Count; i++)
                {

                    if (StepIPNeedsToRecalculate)
                    {
                        StepForceResponseCalculate();

                        double xtPar = StepForceResponse[i];

                        ParticularSolution.Add(xtPar);
                    }


                    else if (HarmonicIPNeedsToRecalculate)
                    {
                        double xtPar = X * Math.Cos(ExcitationFrequencyRad * TimeIntervals[i] - PhyHarmonic);

                        ParticularSolution.Add(xtPar);
                    }
                }
            }
        }

        private void TotalResponseCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if(TotalResponse == null)
                {
                    TotalResponse = new List<double>();
                }

                TotalResponse.Clear();

                HomogeneousSolutionCalculation();

                ParticularSolutionCalculate();

                for(int i = 0; i<TimeIntervals.Count; i++)
                {
                    if (StepIPNeedsToRecalculate)
                    {
                        double xtT = HomogeneousSolution[i] + ParticularSolution[i];

                        TotalResponse.Add(xtT);
                    }

                    else if (HarmonicIPNeedsToRecalculate)
                    {
                        double xtT = HomogeneousSolution[i] + ParticularSolution[i];

                        TotalResponse.Add(xtT);
                    }
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

                for(int i = 0; i< TimeIntervals.Count; i++)
                {
                    if (StepIPNeedsToRecalculate)
                    {
                        double a = ForceVector[i] * UnitStepFunctionVelocity[i];

                        BodyVelocity.Add(a);
                    }

                    else if (HarmonicIPNeedsToRecalculate)
                    {
                        if (DampingRatio < 1.0)
                        {
                            double a = InitialDisplacement - (X * Math.Cos(PhyHarmonic));
                            double A = Math.Pow(a, 2);

                            double _b = DampingRatio * NaturalFrequencyRad;
                            double b1 = _b * InitialDisplacement;
                            double b2 = _b * X * Math.Cos(PhyHarmonic);
                            double b3 = ExcitationFrequencyRad * X * Math.Sin(PhyHarmonic);
                            double b = (b1 + InitialDisplacement - b2 - b3) / DampedNaturalFrequency;
                            double B = Math.Pow(b, 2);

                            double X0 = Math.Sqrt(A + B);

                            double phy0 = Math.Atan(b / a);

                            double exp = Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]);

                            double A_1 = DampedNaturalFrequency * Math.Sin(DampedNaturalFrequency * TimeIntervals[i] - phy0);
                            double A_2 = DampingRatio * NaturalFrequencyRad * Math.Cos(DampedNaturalFrequency * TimeIntervals[i] - phy0);

                            double A1 = X0 * exp * (A_1 + A_2);

                            double B1 = X * ExcitationFrequencyRad * Math.Sin(ExcitationFrequencyRad * TimeIntervals[i] - phy0);

                            double xDotT = -(A1 + B1);

                            BodyVelocity.Add(xDotT);

                        }

                        else if (DampingRatio > 1.0)
                        {
                            double a = Math.Sqrt(Math.Pow(DampingRatio, 2) - 1);

                            double D = 2.0 * NaturalFrequencyRad * a;

                            double _c1 = 3.0 * a - DampingRatio;
                            double c11 = NaturalFrequencyRad * InitialDisplacement * _c1;
                            double c12 = ExcitationFrequencyRad * X * Math.Sin(PhyHarmonic);


                            double C1 = (c11 + c12 - InitialVelocity) / D;

                            double c2 = (-DampingRatio + a) * NaturalFrequencyRad * InitialDisplacement;

                            double C2 = (InitialVelocity - c2 - c12) / D;

                            double exp1 = -DampingRatio + a;
                            double exp2 = -DampingRatio - a;

                            double A = exp1 * NaturalFrequencyRad * C1 * Math.Exp(exp1 * NaturalFrequencyRad * TimeIntervals[i]);

                            double B = exp2 * NaturalFrequencyRad * C2 * Math.Exp(exp2 * NaturalFrequencyRad * TimeIntervals[i]);

                            double C = ExcitationFrequencyRad * X * Math.Sin(ExcitationFrequencyRad * TimeIntervals[i] - PhyHarmonic);

                            double xDotT = A + B - C;

                            BodyVelocity.Add(xDotT);


                        }
                    }
                   

                }
            }
        }


        private void BodyAccelnCalculate()
        {
            if (ResponseNeedsToRecalculate)
            {
                if (BodyAcceln == null)
                {
                    BodyAcceln = new List<double>();
                }

                BodyAcceln.Clear();

                for (int i = 0; i < TimeIntervals.Count; i++)
                {
                    if (StepIPNeedsToRecalculate)
                    {
                        double a = ForceVector[i] * UnitStepFunctionAcceleration[i] / g;

                        BodyAcceln.Add(a);
                    }

                    else if (HarmonicIPNeedsToRecalculate)
                    {
                        if (DampingRatio < 1.0)
                        {
                            double a = InitialDisplacement - (X * Math.Cos(PhyHarmonic));
                            double A = Math.Pow(a, 2);

                            double _b = DampingRatio * NaturalFrequencyRad;
                            double b1 = _b * InitialDisplacement;
                            double b2 = _b * X * Math.Cos(PhyHarmonic);
                            double b3 = ExcitationFrequencyRad * X * Math.Sin(PhyHarmonic);
                            double b = (b1 + InitialDisplacement - b2 - b3) / DampedNaturalFrequency;
                            double B = Math.Pow(b, 2);

                            double X0 = Math.Sqrt(A + B);

                            double phy0 = Math.Atan(b / a);

                            double exp = Math.Exp(-DampingRatio * NaturalFrequencyRad * TimeIntervals[i]);

                            double A1_1 = 2.0 * DampingRatio * NaturalFrequencyRad * DampedNaturalFrequency * Math.Sin(DampedNaturalFrequency * TimeIntervals[i] - phy0);
                            double A1_2 = (Math.Pow(DampedNaturalFrequency, 2) - Math.Pow(DampingRatio * NaturalFrequencyRad, 2)) * Math.Cos(DampedNaturalFrequency * TimeIntervals[i] - phy0);

                            double A1 = X0 * exp * (A1_1 - A1_2);

                            double B1 = X * Math.Pow(ExcitationFrequencyRad, 2) * Math.Cos(ExcitationFrequencyRad * TimeIntervals[i] - PhyHarmonic);

                            double v = A1 - B1;

                            double xDDotT = v / g;

                            BodyAcceln.Add(xDDotT);
                        }


                        else if (DampingRatio > 1.0)
                        {
                            double a = Math.Sqrt(Math.Pow(DampingRatio, 2) - 1);

                            double D = 2.0 * NaturalFrequencyRad * a;

                            double _c1 = 3.0 * a - DampingRatio;
                            double c11 = NaturalFrequencyRad * InitialDisplacement * _c1;
                            double c12 = ExcitationFrequencyRad * X * Math.Sin(PhyHarmonic);


                            double C1 = (c11 + c12 - InitialVelocity) / D;

                            double c2 = (-DampingRatio + a) * NaturalFrequencyRad * InitialDisplacement;

                            double C2 = (InitialVelocity - c2 - c12) / D;

                            double exp1 = -DampingRatio + a;
                            double exp2 = -DampingRatio - a;

                            double A = Math.Pow(exp1 * NaturalFrequencyRad, 2) * C1 * Math.Exp(exp1 * NaturalFrequencyRad * TimeIntervals[i]);

                            double B = Math.Pow(exp2 * NaturalFrequencyRad, 2) * C2 * Math.Exp(exp2 * NaturalFrequencyRad * TimeIntervals[i]);

                            double C = Math.Pow(ExcitationFrequencyRad,2) * X * Math.Cos(ExcitationFrequencyRad * TimeIntervals[i] - PhyHarmonic);

                            double v = A + B - C;

                            double xDDotT = v / g;

                            BodyAcceln.Add(xDDotT);
                        }
                    }
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

                for (int i = 0; i < TotalResponse.Count; i++)
                {
                    double Fs = SpringStiffness * TotalResponse[i];

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


        #region Input Data Calculation
        public void InputDataCalculate()
        {
            if (TimeNeedsToRecalculate)
            {
                TimeCalculate();

                if (ResponseNeedsToRecalculate)
                {
                    if (StepIPNeedsToRecalculate)
                    {
                        ForceVectorCalculate();
                        ResponseToUnitStepFunctionCalculate();

                    }

                    else if (HarmonicIPNeedsToRecalculate)
                    {
                        HarmonicInputCalcualte();
                    }
                }
            }

            TimeNeedsToRecalculate = false;


        }
        #endregion

        #region Output Data Calculation
        public void OutputDataCalculate()
        {

            if (ResponseNeedsToRecalculate)
            {

                HomogeneousSolutionCalculation();
                ParticularSolutionCalculate();
                TotalResponseCalculate();

                UnitStepVelocityCalculate();
                UnitStepAccelerationCalculate();


                BodyVelocityCalculate();
                BodyAccelnCalculate();

                SpringForceCalculate();
                DamperForceCalculate();



                //ModelResponse();

                //BodyAccelnCalculate();



                //BodyForceCalculate();
            }

            ResponseCalculationComplete = true;
            HarmonicIPNeedsToRecalculate = false;
            StepIPNeedsToRecalculate = false;
            ResponseNeedsToRecalculate = false;

        }
        #endregion






        public void SaveCar(string carFileName)
        {
            Car.Save(carFileName);
        }


        public void SaveSimData(string simFileName)
        {
            Sim.Save(simFileName);
        }

        public void LoadCar(string carFileName)
        {
            Car.Load(carFileName);
        }

        public void LoadSimData(string simFileName)
        {
            Sim.Load(simFileName);
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