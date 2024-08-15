using System;
using Raylib_cs;
using static Fluids.Settings;

namespace Fluids;

public class Game
{
    public List<Particle> particles;

    public Game()
    {
        particles = new();
        particles.Add(new(new(100, 100), new(0, 0), 10));
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

    public void Update()
    {
        foreach (Particle particle in particles)
        {
            particle.Update();
            particle.Draw();
        }
    }
}