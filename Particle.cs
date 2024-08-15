using System;
using System.Numerics;
using Raylib_cs;
using static Fluids.Settings;

namespace Fluids;

public class Particle
{
    public Vector2 position;
    public Vector2 velocity;

    public Particle(Vector2 position, Vector2 velocity)
    {
        this.position = position;
        this.velocity = velocity;
    }

    public void Update()
    {
        velocity -= Vector2.UnitY * Gravity;
        position += velocity * FrameTime;
        ResolveCollisions();
        Draw();
    }

    private void Draw()
    {
        Raylib.DrawCircleV(position, ParticleRadius, White);
    }

    private void ResolveCollisions()
    {
        if (position.X - ParticleRadius < ContainerBounds[0])
        {
            position.X = ContainerBounds[0] + ParticleRadius;
            velocity.X *= -1;
        }
        else if (position.X + ParticleRadius > ContainerBounds[1])
        {
            position.X = ContainerBounds[1] - ParticleRadius;
            velocity.X *= -1;
        }

        if (position.Y - ParticleRadius < ContainerBounds[2])
        {
            position.Y = ContainerBounds[2] + ParticleRadius;
            velocity.Y *= -1;
        }
        else if (position.Y + ParticleRadius > ContainerBounds[3])
        {
            position.Y = ContainerBounds[3] - ParticleRadius;
            velocity.Y *= -1;
        }
    }
}