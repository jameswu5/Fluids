using System;
using System.Numerics;
using Raylib_cs;
using static Fluids.Settings;

namespace Fluids;

public class Container
{
    public readonly Rectangle container;
    public List<Particle> particles;
    public const int ParticleCount = 49;
    public const int SimulationsPerFrame = 5;
    public Random random;

    public const float SmoothingRadius = 2f;
    public const float RestDensity = 1f;
    public const float GasConstant = 2f;
    public const float Mass = 1f;
    public static readonly Vector2 Gravity = new(0, Settings.Gravity);

    public static readonly IKernel kernel = new Polynomial();

    public Container()
    {
        random = new();
        container = new Rectangle(ContainerPadding, ContainerPadding, ContainerWidth * Scale, ContainerHeight * Scale);
        particles = CreateParticles(ParticleCount);
    }

    private static List<Particle> CreateParticles(int count, float spacing = 0.2f)
    {
        List<Particle> res = new();

        // Create a square grid of particles
        int rows = (int)Math.Sqrt(count);
        int cols = (count - 1) / rows + 1;

        float x = ContainerWidth / 2 - (cols * ParticleRadius / 2) - (cols - 1) * spacing / 2;
        float y = ContainerHeight / 2 - (rows * ParticleRadius / 2) - (rows - 1) * spacing / 2;

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
        for (int i = 0; i < SimulationsPerFrame; i++)
        {
            Step();
        }
        Display();
    }

    private void Display()
    {
        // Display container
        Raylib.DrawRectangleLinesEx(container, 5, White);

        // Draw particles
        foreach (Particle particle in particles)
        {
            particle.Draw();
        }
    }

    private void Step()
    {
        // Predict positions
        Parallel.For(0, ParticleCount, i =>
        {
            particles[i].predictedPosition = particles[i].position + particles[i].velocity * FrameTime;
        });

        // Precompute particle densities
        Parallel.For(0, ParticleCount, i =>
        {
            particles[i].density = CalculateDensity(particles[i].predictedPosition);
        });

        // Update particle forces
        Parallel.For(0, ParticleCount, i =>
        {
            Vector2 force = CalculateResultantForce(i);
            particles[i].velocity += force * FrameTime / particles[i].density;
        });

        // Update particle positions
        Parallel.For(0, ParticleCount, i =>
        {
            particles[i].position += particles[i].velocity * FrameTime;
            particles[i].ResolveCollisions();
        });
    }

    private float CalculateDensity(Vector2 point)
    {
        float density = 0f;
        for (int i = 0; i < particles.Count; i++)
        {
            Vector2 diff = particles[i].position - point;
            float distance = diff.Length();
            density += Mass * kernel.Evaluate(distance, SmoothingRadius);
        }
        return density;
    }

    private Vector2 CalculateResultantForce(int particleIndex)
    {
        Vector2 force = Vector2.Zero;

        // Pressure
        force += CalculatePressureForce(particleIndex);
        // Gravity
        // force -= particles[particleIndex].density * Gravity;

        return force;
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
            Vector2 direction = distance == 0 ? GetRandomDirection() : Vector2.Normalize(diff); // create random vector instead
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