using System;
using System.Numerics;
using Raylib_cs;
using static Fluids.Settings;

namespace Fluids;

public class Container
{
    public readonly Rectangle container;
    public List<Particle> particles;
    public const int ParticleCount = 400;
    public const int SimulationsPerFrame = 3;
    public Random random;

    public const float SmoothingRadius = 0.4f;
    public const float RestDensity = 10f;
    public const float GasConstant = 2f;
    public const float Viscosity = 0.1f;
    public const float Mass = 1f;
    public static readonly Vector2 Gravity = new(0, Settings.Gravity);

    public static readonly Kernel DensityKernel = Kernel.Create(Kernel.Type.Polynomial);
    public static readonly Kernel PressureKernel = Kernel.Create(Kernel.Type.Spiky);
    public static readonly Kernel ViscosityKernel = Kernel.Create(Kernel.Type.Viscous);

    public const float MouseForce = 5 * 9.81f;
    public const float MouseRadius = 1f;
    public Vector2 mouseLocation;
    public int mouseForceActive; // -1 for negative, 0 for none, 1 for positive

    public Container()
    {
        random = new();
        container = new Rectangle(ContainerPadding, ContainerPadding, ContainerWidth * Scale, ContainerHeight * Scale);
        particles = CreateParticles(ParticleCount);

        mouseLocation = Vector2.Zero;
        mouseForceActive = 0;
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
            particle.Draw(coloured: true);
        }

        // Draw mouse force
        if (mouseForceActive != 0)
        {
            Console.WriteLine(mouseLocation);
            Raylib.DrawCircleLines(ContainerPadding + (int)(mouseLocation.X * Scale), ContainerPadding + (int)((ContainerHeight - mouseLocation.Y) * Scale), (int)(MouseRadius * Scale), White);
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
            particles[i].velocity += force * FrameTime;
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
            density += Mass * DensityKernel.Evaluate(distance, SmoothingRadius);
        }
        return density;
    }

    private Vector2 CalculateResultantForce(int particleIndex)
    {
        Vector2 force = Vector2.Zero;

        // Pressure
        force += CalculatePressureForce(particleIndex);
        // External forces
        force += CalculateExternalForce(particleIndex, gravity: true);
        // Viscosity
        force += CalculateViscosity(particleIndex);

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
            Vector2 gradient = PressureKernel.Derivative(distance, SmoothingRadius) * direction;
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

    private Vector2 CalculateExternalForce(int particleIndex, bool gravity = true)
    {
        // Gravity
        Vector2 externalForce = gravity ? Gravity : Vector2.Zero;
        // Mouse force
        if (mouseForceActive != 0)
        {
            Vector2 diff = mouseLocation - particles[particleIndex].position;
            float distance = diff.Length();
            if (distance <= MouseRadius)
            {
                externalForce += MouseForce * diff * mouseForceActive / distance;
            }
        }

        return externalForce;
    }

    private Vector2 CalculateViscosity(int particleIndex)
    {
        Vector2 viscosityForce = Vector2.Zero;

        for (int i = 0; i < particles.Count; i++)
        {
            Vector2 diff = particles[i].position - particles[particleIndex].position;
            float distance = diff.Length();
            Vector2 direction = distance == 0 ? GetRandomDirection() : Vector2.Normalize(diff); // create random vector instead
            Vector2 laplace = ViscosityKernel.Laplacian(distance, SmoothingRadius) * direction;
            Vector2 velocityDifference = particles[i].velocity - particles[particleIndex].velocity;
            viscosityForce += Viscosity * velocityDifference * laplace * Mass / particles[i].density;
        }

        return viscosityForce;
    }
}