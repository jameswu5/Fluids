using System;
using System.Numerics;
using Raylib_cs;
using static Fluids.Settings;

namespace Fluids;

public class Container
{
    public readonly Rectangle container;
    public List<Particle> particles;

    public Random random;

    public const float SmoothingRadius = 100;
    public const float RestDensity = 1f;
    public const float GasConstant = 100;
    public const float Mass = 1f;

    public static readonly IKernel kernel = new Polynomial();

    public Container(int particleCount)
    {
        random = new();
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

    private Vector2 CalculatePressureForce(int particleIndex)
    {
        // this is the pressure component -∇(p) of the Navier-Stokes equation

        Vector2 pressureForce = Vector2.Zero;
        for (int i = 0; i < particles.Count; i++)
        {
            if (particleIndex == i) continue;

            Vector2 diff = particles[i].position - particles[particleIndex].position;
            float distance = diff.Length();
            Vector2 direction = distance == 0 ? GetRandomDirection() : diff / distance; // create random vector instead
            Vector2 gradient = kernel.Derivative(distance, SmoothingRadius) * direction;
            float pressure = (CalculatePressureFromDensity(particles[i].density) + CalculatePressureFromDensity(particles[particleIndex].density)) / 2;
            pressureForce += pressure * gradient * Mass / particles[i].density;

        }
        return pressureForce;
    }

    private Vector2 GetRandomDirection()
    {
        Vector2 dir = new((float)random.NextDouble(), (float)random.NextDouble());
        return Vector2.Normalize(dir);
    }

    private static float CalculatePressureFromDensity(float density)
    {
        return GasConstant * (density - RestDensity);
    }
}