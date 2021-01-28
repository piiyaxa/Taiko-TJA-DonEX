using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TJAPlayer3;
using FDK;

namespace TJAPlayer3
{
    class Easing
    {
        public static int EaseIn(CCounter counter, int StartPoint, int EndPoint, CalcType type)
        {
            int Sa = EndPoint - StartPoint;
            int TimeMs = counter.n終了値;
            double CounterValue = counter.n現在の値;

            double Value;

            switch (type)
            {
                case CalcType.Quadratic: //Quadratic
                    CounterValue /= TimeMs;
                    Value = Sa * CounterValue * CounterValue + StartPoint;
                    break;
                case CalcType.Cubic: //Cubic
                    CounterValue /= TimeMs;
                    Value = Sa * CounterValue * CounterValue * CounterValue + StartPoint;
                    break;
                case CalcType.Quartic: //Quartic
                    CounterValue /= TimeMs;
                    Value = Sa * CounterValue * CounterValue * CounterValue * CounterValue + StartPoint;
                    break;
                case CalcType.Quintic: //Quintic
                    CounterValue /= TimeMs;
                    Value = Sa * CounterValue * CounterValue * CounterValue * CounterValue * CounterValue + StartPoint;
                    break;
                case CalcType.Sinusoidal: //Sinusoidal
                    Value = -Sa * Math.Cos(CounterValue / TimeMs * (Math.PI / 2)) + Sa + StartPoint;
                    break;
                case CalcType.Exponential: //Exponential
                    Value = Sa * Math.Pow(2, 10 * (CounterValue / TimeMs - 1)) + StartPoint;
                    break;
                default: //Circular
                    CounterValue /= TimeMs;
                    Value = -Sa * (Math.Sqrt(1 - CounterValue * CounterValue) - 1) + StartPoint;
                    break;
            }

            return (int)Value;
        }
        public static int EaseOut(CCounter counter, int StartPoint, int EndPoint, CalcType type)
        {
            int Sa = EndPoint - StartPoint;
            int TimeMs = counter.n終了値;
            double CounterValue = counter.n現在の値;

            double Value;

            switch (type)
            {
                case CalcType.Quadratic: //Quadratic
                    CounterValue /= TimeMs;
                    Value = -Sa * CounterValue * (CounterValue - 2) + StartPoint;
                    break;
                case CalcType.Cubic: //Cubic
                    CounterValue /= TimeMs;
                    CounterValue--;
                    Value = Sa * (CounterValue * CounterValue * CounterValue + 1) + StartPoint;
                    break;
                case CalcType.Quartic: //Quartic
                    CounterValue /= TimeMs;
                    CounterValue--;
                    Value = -Sa * (CounterValue * CounterValue * CounterValue * CounterValue - 1) + StartPoint;
                    break;
                case CalcType.Quintic: //Quintic
                    CounterValue /= TimeMs;
                    CounterValue--;
                    Value = Sa * (CounterValue * CounterValue * CounterValue * CounterValue * CounterValue + 1) + StartPoint;
                    break;
                case CalcType.Sinusoidal: //Sinusoidal
                    Value = Sa * Math.Sin(CounterValue / TimeMs * (Math.PI / 2)) + StartPoint;
                    break;
                case CalcType.Exponential: //Exponential
                    Value = Sa * (-Math.Pow(2, -10 * CounterValue / TimeMs) + 1) + StartPoint;
                    break;
                default: //Circular
                    CounterValue /= TimeMs;
                    CounterValue--;
                    Value = Sa * Math.Sqrt(1 - CounterValue * CounterValue) + StartPoint;
                    break;
            }

            return (int)Value;
        }

        public enum CalcType
        {
            Quadratic,
            Cubic,
            Quartic,
            Quintic,
            Sinusoidal,
            Exponential,
            Circular
        }
    }
}