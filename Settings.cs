using System;
using System.Numerics;
using Raylib_cs;

namespace Fluids;

public static class Settings
{
    public const string Name = "Fluids";
    public const int ScreenWidth = 1080;
    public const int ScreenHeight = 720;
    public const int FrameRate = 120;
    public const float FrameTime = 1f / FrameRate;

    public static readonly Color Black = new(30, 30, 30, 255);
    public static readonly Color White = new(235, 235, 235, 255);

    public static readonly Color PastelRed = new(253, 138, 138, 255);
    public static readonly Color PastelYellow = new(241, 247, 181, 255);
    public static readonly Color PastelBlue = new(158, 161, 212, 255);

    public static readonly Color Teal = new(55, 183, 195, 255);
    public static readonly Color Blue = new(8, 131, 149, 255);

    public static readonly Color Gradient1 = Blue;
    public static readonly Color Gradient2 = Teal;
    public static readonly Color Gradient3 = White;

    public const float GradientMiddleThreshold = 4.0f;
    public const float GradientUpperThreshold = 6.0f;

    public static Color GetGradientColour(float val)
    {
        if (val < 0)
        {
            return Gradient1;
        }
        else if (val < GradientMiddleThreshold)
        {
            return InterpolateColour(Gradient1, Gradient2, val / GradientMiddleThreshold);
        }
        else if (val < GradientUpperThreshold)
        {
            return InterpolateColour(Gradient2, Gradient3, (val - GradientMiddleThreshold) / (GradientUpperThreshold - GradientMiddleThreshold));
        }
        else
        {
            return Gradient3;
        }
    }

    private static Color InterpolateColour(Color startColour, Color endColour, float t)
    {
        // t has to be between 0 and 1
        Vector4 start = new(startColour.R, startColour.G, startColour.B, startColour.A);
        Vector4 end = new(endColour.R, endColour.G, endColour.B, endColour.A);
        Vector4 result = Vector4.Lerp(start, end, t);
        return new Color((byte)result.X, (byte)result.Y, (byte)result.Z, (byte)result.W);
    }

    public const float Gravity = -9.81f;
    public const float Dampening = 0.4f;

    public const int Scale = 100;
    public const float ParticleRadius = 0.1f;
    public const int ParticleUIRadius = (int)(ParticleRadius * Scale);

    public const int ContainerPadding = 40;

    public const float ContainerWidth = (float)(ScreenWidth - 2 * ContainerPadding) / Scale;
    public const float ContainerHeight = (float)(ScreenHeight - 2 * ContainerPadding) / Scale;

    public static readonly float[] ContainerBounds = { 0, ContainerWidth, 0, ContainerHeight };
}