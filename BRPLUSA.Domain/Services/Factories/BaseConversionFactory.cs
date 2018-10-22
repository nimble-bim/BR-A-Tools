namespace BRPLUSA.Domain.Services.Factories
{
    public abstract class BaseConversionFactory<T, T1> : IConversionFactory<T, T1>
    {
        public abstract T Create(T1 obj);
        public abstract T Create();
        public abstract T Create(object obj);
    }
}