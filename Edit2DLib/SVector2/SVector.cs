using System;

namespace Edit2DLib
{
    // A simple 2d vector class that can port to Javascript

    public partial class SVector2
    {
        
        public float X { get; set; }
        public float Y { get; set; }

        public SVector2(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public float Magnitude
        {
            get
            {
                return (float)Math.Sqrt(Y * Y + X * X);
            }
        }

        public static SVector2 Add(SVector2 v1, SVector2 v2)
        {
            return new SVector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static SVector2 Subtract(SVector2 v1, SVector2 v2)
        {
            return new SVector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public SVector2 Negate(SVector2 v1)
        {
            return new SVector2(-v1.X, -v1.Y);
        }

        public static SVector2 Scale(SVector2 v2, float scale)
        {
            return new SVector2(v2.X * scale, v2.Y * scale);
        }

        public static SVector2 Normalize(SVector2 v1)
        {
            float inverse = 1 / v1.Magnitude;

            return new SVector2(v1.X * inverse, v1.Y * inverse);
        }

        public static double Distance(SVector2 v1, SVector2 v2)
        {
            return
               Math.Sqrt
               (
                   (v1.X - v2.X) * (v1.X - v2.X) +
                   (v1.Y - v2.Y) * (v1.Y - v2.Y) 
               );
        }

        public static SVector2 Interpolate(
            SVector2 v1,
            SVector2 v2,
            float control)
        {
            return new SVector2(
                v1.X * (1 - control) + v2.X * control,
                v1.Y * (1 - control) + v2.Y * control
                );
        }
    }
}
