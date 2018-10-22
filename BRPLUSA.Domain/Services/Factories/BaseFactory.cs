namespace BRPLUSA.Domain.Services.Factories
{
    public abstract class BaseFactory<T> : IFactory<T>
    {
        public abstract T Create();

        public abstract T Create(object obj);
    }
}
