using System;
using System.Numerics;
using Raylib_cs;
using static Fluids.Settings;

namespace Fluids;

public class Particle
{
    public Vector2 position;
    public Vector2 predictedPosition;
    public Vector2 velocity;
    public float density;

    public Particle(Vector2 position, Vector2 velocity)
    {
        this.position = position;
        predictedPosition = Vector2.Zero; // need to repopulate in runtime
        this.velocity = velocity;
        density = 0; // need to repopulate in runtime
    }

    public void Draw()
    {
        int x = (int)(position.X * Scale) + ContainerPadding;
        int y = (int)((ContainerHeight - position.Y) * Scale) + ContainerPadding;

        Raylib.DrawCircleV(new Vector2(x, y), ParticleUIRadius, White);
    }

    public void ResolveCollisions()
    {
        if (position.X - ParticleRadius < ContainerBounds[0])
        {
            position.X = ContainerBounds[0] + ParticleRadius;
            velocity.X *= -Dampening;
        }
        else if (position.X + ParticleRadius > ContainerBounds[1])
        {
            position.X = ContainerBounds[1] - ParticleRadius;
            velocity.X *= -Dampening;
        }

        if (position.Y - ParticleRadius < ContainerBounds[2])
        {
            position.Y = ContainerBounds[2] + ParticleRadius;
            velocity.Y *= -Dampening;
        }
        else if (position.Y + ParticleRadius > ContainerBounds[3])
        {
            position.Y = ContainerBounds[3] - ParticleRadius;
            velocity.Y *= -Dampening;
        }
    }
}