using System;
using Raylib_cs;

namespace Fluids;

public class Settings
{
    public const string Name = "Fluids";
    public const int ScreenWidth = 1080;
    public const int ScreenHeight = 720;
    public const int FrameRate = 120;
    public const float FrameTime = 1f / FrameRate;

    public static readonly Color Black = new(30, 30, 30, 255);
    public static readonly Color White = new(235, 235, 235, 255);

    public const float Gravity = -9.81f;

    public const int Scale = 100;
    public const float ParticleRadius = 0.1f;
    public const int ParticleUIRadius = (int)(ParticleRadius * Scale);

    public const int ContainerPadding = 40;

    public const float ContainerWidth = (float)(ScreenWidth - 2 * ContainerPadding) / Scale;
    public const float ContainerHeight = (float)(ScreenHeight - 2 * ContainerPadding) / Scale;

    public static readonly float[] ContainerBounds = { 0, ContainerWidth, 0, ContainerHeight };
}