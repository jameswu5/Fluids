using System;
using System.Numerics;
using Raylib_cs;

namespace Fluids;

public class Particle
{
    public Vector2 position;
    public Vector2 velocity;
    public int radius;

    public Particle(Vector2 position, Vector2 velocity, int radius)
    {
        this.position = position;
        this.velocity = velocity;
        this.radius = radius;
    }

    public void Update()
    {
        velocity -= Vector2.UnitY * Settings.Gravity;
        position += velocity * Raylib.GetFrameTime();
    }

    public void Draw()
    {
        Raylib.DrawCircleV(position, radius, Settings.White);
    }
}