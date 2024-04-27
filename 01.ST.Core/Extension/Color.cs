using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core
{
    public static partial class Extensions
    {
        /// <summary>
        /// 현재 색상에서 밝기 배율(brightnessRevision 최대 1)을 더한 새로운 색상을 반환합니다.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="brightnessRevision"></param>
        /// <returns></returns>
        public static Color GetColor(this Color color, float brightnessRevision)
        {
            Hsl hsl = color.ToHls();
            hsl.L += brightnessRevision;
            Color rs = hsl.ToColor();
            return rs;
        }

        /// <summary>
        /// condition 값이 Ture일 때 현재 색상에서 밝기 배율(brightnessRevision 최대 1)을 더한 새로운 색상을 반환합니다. condition 값이 False이면 현재 색상을 반환합니다.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="condition"></param>
        /// <param name="brightnessRevision"></param>
        /// <returns></returns>
        public static Color GetColor(this Color color, bool condition, float brightnessRevision)
        {
            Color rs = color;
            if (condition)
            {
                Hsl hsl = color.ToHls();
                hsl.L += brightnessRevision;
                rs = hsl.ToColor();
                return rs;
            }
            return rs;
        }

        // Original Code
        // https://son10001.blogspot.com/2014/04/c-hsl-to-rgb.html
        /// <summary>
        /// HSL 구조체를 RGB Color로 변환 후 반환합니다.
        /// </summary>
        /// <param name="hsl"></param>
        /// <returns></returns>
        public static Color ToColor(this Hsl hsl)
        {
            double r, g, b, temp1, temp2;

            if (hsl.L == 0)
            {
                r = g = b = 0;
            }
            else
            {
                if (hsl.S == 0)
                {
                    r = g = b = hsl.L;
                }
                else
                {
                    temp2 = ((hsl.L <= 0.5) ? hsl.L * (1.0 + hsl.S) : hsl.L + hsl.S - (hsl.L * hsl.S));
                    temp1 = 2.0 * hsl.L - temp2;

                    double[] t3 = new double[] { hsl.H + 1.0 / 3.0, hsl.H, hsl.H - 1.0 / 3.0 };
                    double[] clr = new double[] { 0, 0, 0 };

                    for (int i = 0; i < 3; i++)
                    {
                        if (t3[i] < 0)
                        {
                            t3[i] += 1.0;
                        }

                        if (t3[i] > 1)
                        {
                            t3[i] -= 1.0;
                        }

                        if (6.0 * t3[i] < 1.0)
                        {
                            clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0;
                        }
                        else if (2.0 * t3[i] < 1.0)
                        {
                            clr[i] = temp2;
                        }
                        else if (3.0 * t3[i] < 2.0)
                        {
                            clr[i] = (temp1 + (temp2 - temp1) * ((2.0 / 3.0) - t3[i]) * 6.0);
                        }
                        else
                        {
                            clr[i] = temp1;
                        }
                    }

                    r = clr[0];
                    g = clr[1];
                    b = clr[2];
                }
            }

            Color rs = Color.FromArgb(
                  Math.Max(Math.Min((int)(255 * r), 255), 0)
                , Math.Max(Math.Min((int)(255 * g), 255), 0)
                , Math.Max(Math.Min((int)(255 * b), 255), 0)
            );
            return rs;
        }

        /// <summary>
        /// RGB Color을 HSL 구조체려 변환 후 반환합니다.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Hsl ToHls(this Color color)
        {
            Hsl rsHsl = new Hsl(
                  color.GetHue() / 360.0
                , color.GetSaturation()
                , color.GetBrightness()
            );
            return rsHsl;
        }
    }

    /// <summary>
    /// HSL 클래스입니다.
    /// </summary>
    public class Hsl
    {
        public double H { get; set; } = 0;

        public double S { get; set; } = 0;

        public double L { get; set; } = 0;

        public Hsl() { }

        public Hsl(double h, double s, double l)
        {
            SetHSL(h, s, l);
        }

        public void SetHSL(double h, double s, double l)
        {
            H = h;
            S = s;
            L = l;
        }
    }
}
