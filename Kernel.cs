using System;

namespace Fluids;

public abstract class Kernel
{
    public enum Type { Polynomial, Spiky, Viscous }

    public static Kernel Create(Type type)
    {
        return type switch
        {
            Type.Polynomial => new Polynomial(),
            Type.Spiky => new Spiky(),
            Type.Viscous => new Viscous(),
            _ => throw new NotImplementedException()
        };
    }

    public static bool Validate(float distance, float radius)
    {
        return distance >= 0 && distance <= radius;
    }

    public abstract float Evaluate(float distance, float radius);
    public abstract float Derivative(float distance, float radius);
    public abstract float Laplacian(float distance, float radius);
}

public class Polynomial : Kernel
{
    public override float Evaluate(float distance, float radius)
    {
        if (!Validate(distance, radius)) return 0;

        return 315f / (64f * MathF.PI * MathF.Pow(radius, 9)) * MathF.Pow(radius * radius - distance * distance, 3);
    }

    public override float Derivative(float distance, float radius)
    {
        if (!Validate(distance, radius)) return 0;

        return -945f / (32f * MathF.PI * MathF.Pow(radius, 9)) * distance * MathF.Pow(radius * radius - distance * distance, 2);
    }

    public override float Laplacian(float distance, float radius)
    {
        if (!Validate(distance, radius)) return 0;

        return 945f / (8f * MathF.PI * MathF.Pow(radius, 9)) * (radius * radius - distance * distance);
    }
}

public class Spiky : Kernel
{
    public override float Evaluate(float distance, float radius)
    {
        if (!Validate(distance, radius)) return 0;

        float scale = 15 / (2 * MathF.PI * MathF.Pow(radius, 5));
        float v = radius - distance;
        return v * v * scale;

    }

    public override float Derivative(float distance, float radius)
    {
        if (!Validate(distance, radius)) return 0;

        float scale = 15 / (MathF.Pow(radius, 5) * MathF.PI);
		float v = radius - distance;
		return -v * scale;
    }

    public override float Laplacian(float distance, float radius)
    {
        throw new NotImplementedException();
    }
}

public class Viscous : Kernel
{
    public override float Evaluate(float distance, float radius)
    {
        throw new NotImplementedException();
    }

    public override float Derivative(float distance, float radius)
    {
        throw new NotImplementedException();
    }

    public override float Laplacian(float distance, float radius)
    {
        if (!Validate(distance, radius)) return 0;

        return 45f / (MathF.PI * MathF.Pow(radius, 6)) * (radius - distance);
    }
}