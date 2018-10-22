namespace BRPLUSA.Domain.Services.Factories
{
    public interface IFactory<out T>
    {
        T Create();
        T Create(object obj);
    }
}