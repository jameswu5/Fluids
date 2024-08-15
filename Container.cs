using System;
using System.Numerics;
using Raylib_cs;
using static Fluids.Settings;

namespace Fluids;

public class Container
{
    public readonly Rectangle container;
    public List<Particle> particles;

    public const float RestDensity = 1f;
    public const float GasConstant = 100;

    public Container(int particleCount)
    {
        container = new Rectangle(ContainerPadding, ContainerPadding, ContainerWidth, ContainerHeight);
        particles = CreateParticles(particleCount);
    }

    private static List<Particle> CreateParticles(int count, int spacing = 20)
    {
        List<Particle> res = new();

        // Create a square grid of particles
        int rows = (int)Math.Sqrt(count);
        int cols = (count - 1) / rows + 1;

        int x = ScreenWidth / 2 - (cols * ParticleRadius/ 2) - (cols - 1) * spacing / 2;
        int y = ScreenHeight / 2 - (rows * ParticleRadius / 2) - (rows - 1) * spacing / 2;

        for (int i = 0; i < count; i++)
        {
            int row = i / cols;
            int col = i % cols;
            Vector2 position = new(x + col * (ParticleRadius + spacing), y + row * (ParticleRadius + spacing));
            Vector2 velocity = new(0, 0);
            Particle particle = new(position, velocity);
            res.Add(particle);
        }
        return res;
    }

    public void Update()
    {
        UpdateParticleVelocities();

        foreach (Particle particle in particles)
        {
            particle.Update();
        }

        DisplayContainer();
    }

    private void DisplayContainer()
    {
        Raylib.DrawRectangleLinesEx(container, 5, White);
    }

    private void UpdateParticleVelocities()
    {
        Parallel.For(0, particles.Count, i =>
        {
            Particle particle = particles[i];
            Vector2 acceleration = CalculateAcceleration(i);
            particle.velocity += acceleration * FrameTime;
        });
    }

    private Vector2 CalculateAcceleration(int particleIndex)
    {
        throw new NotImplementedException();   
    }

    private float CalculateDensity(int particleIndex)
    {
        throw new NotImplementedException();
    }

    private float CalculatePressure(int particleIndex)
    {
        // compute p = k * (ρ - ρ0)
        throw new NotImplementedException();
    }

    private float CalculateViscosity(int particleIndex)
    {
        throw new NotImplementedException();
    }
}