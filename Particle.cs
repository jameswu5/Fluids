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
        ResolveCollisions();
        Draw();
    }

    private void Draw()
    {
        Raylib.DrawCircleV(position, radius, Settings.White);
    }

    private void ResolveCollisions()
    {
        if (position.X - radius < Settings.ContainerBounds[0])
        {
            position.X = Settings.ContainerBounds[0] + radius;
            velocity.X *= -1;
        }
        else if (position.X + radius > Settings.ContainerBounds[1])
        {
            position.X = Settings.ContainerBounds[1] - radius;
            velocity.X *= -1;
        }

        if (position.Y - radius < Settings.ContainerBounds[2])
        {
            position.Y = Settings.ContainerBounds[2] + radius;
            velocity.Y *= -1;
        }
        else if (position.Y + radius > Settings.ContainerBounds[3])
        {
            position.Y = Settings.ContainerBounds[3] - radius;
            velocity.Y *= -1;
        }
    }
}