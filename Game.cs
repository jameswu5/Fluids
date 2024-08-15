using System;
using Raylib_cs;
using static Fluids.Settings;

namespace Fluids;

public class Game
{
    public Container container;

    public Game()
    {
        container = new(25);
    }

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

    private void Update()
    {
        container.Update();
    }
}