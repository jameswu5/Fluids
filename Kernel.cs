

namespace Fluids;

public interface IKernel
{
    float Evaluate(float distance, float radius);
    float Derivative(float distance, float radius);
}

public class Polynomial : IKernel
{
    public float Evaluate(float distance, float radius)
    {
        if (distance < 0 || distance > radius)
        {
            return 0;
        }

        return 315f / (64f * MathF.PI * MathF.Pow(radius, 9)) * MathF.Pow(radius * radius - distance * distance, 3);
    }

    public float Derivative(float distance, float radius)
    {
        if (distance < 0 || distance > radius)
        {
            return 0;
        }

        return -945f / (32f * MathF.PI * MathF.Pow(radius, 9)) * distance * MathF.Pow(radius * radius - distance * distance, 2);
    }
}

public class Spiky : IKernel
{
    public float Evaluate(float distance, float radius)
    {
        return 15f / (MathF.PI * MathF.Pow(radius, 6)) * MathF.Pow(radius - distance, 3);
    }

    public float Derivative(float distance, float radius)
    {
        return -45f / (MathF.PI * MathF.Pow(radius, 6)) * MathF.Pow(radius - distance, 2);
    }
}