using System;
using Raylib_cs;

namespace Fluids;

public class Settings
{
    public const string Name = "Fluids";
    public const int ScreenWidth = 1080;
    public const int ScreenHeight = 780;
    public const int FrameRate = 120;
    public const float FrameTime = 1f / FrameRate;

    public static readonly Color Black = new(30, 30, 30, 255);
    public static readonly Color White = new(235, 235, 235, 255);

    public const float Gravity = -9.81f;

    public const int Scale = 100;
    public const float ParticleRadius = 0.1f;
    public const int ParticleUIRadius = (int)(ParticleRadius * Scale);

    public const float ContainerWidth = 10;
    public const float ContainerHeight = 7;
    public const int ContainerPaddingX = (ScreenWidth - (int)ContainerWidth * Scale) / 2;
    public const int ContainerPaddingY = (ScreenHeight - (int)ContainerHeight * Scale) / 2;

    public static readonly float[] ContainerBounds = { 0, ContainerWidth, 0, ContainerHeight };
}