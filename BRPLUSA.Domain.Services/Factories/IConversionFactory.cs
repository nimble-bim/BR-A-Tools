namespace BRPLUSA.Domain.Services.Factories
{
    public interface IConversionFactory<T, T1> : IFactory<T>
    {
        T Create(T1 obj);
    }
}