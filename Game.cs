using System;
using System.Numerics;
using Raylib_cs;
using static Fluids.Settings;

namespace Fluids;

public class Game
{
    public Container container;

    public Game()
    {
        container = new();
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
        HandleInput();
        container.Update();
    }

    private void HandleInput()
    {
        container.mouseLocation = ConvertMousePosition(Raylib.GetMousePosition());

        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            // Set positive force
            container.mouseForceActive = 1;
        }

        if (Raylib.IsMouseButtonDown(MouseButton.Right))
        {
            // Set negative force
            container.mouseForceActive = -1;
        }

        if (Raylib.IsMouseButtonReleased(MouseButton.Left) || Raylib.IsMouseButtonReleased(MouseButton.Right))
        {
            // Remove mouse force
            container.mouseForceActive = 0;
        }
    }

    private static Vector2 ConvertMousePosition(Vector2 mousePosition)
    {
        return new((mousePosition.X - ContainerPadding) / Scale, ContainerHeight - (mousePosition.Y - ContainerPadding) / Scale);
    }
}