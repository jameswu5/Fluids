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

    public const int ParticleRadius = 10;

    public const int ContainerPadding = 20;
    public const int ContainerWidth = ScreenWidth - 2 * ContainerPadding;
    public const int ContainerHeight = ScreenHeight - 2 * ContainerPadding;
    public static readonly int[] ContainerBounds = { ContainerPadding, ContainerWidth + ContainerPadding, ContainerPadding, ContainerHeight + ContainerPadding };
}