using System;
using Raylib_cs;
using static Fluids.Settings;

namespace Fluids;

public class Game
{
    public void Run()
    {
        Raylib.InitWindow(ScreenWidth, ScreenHeight, Name);
        Raylib.SetTargetFPS(FrameRate);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Black);
            Update();
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    public void Update()
    {

    }
}