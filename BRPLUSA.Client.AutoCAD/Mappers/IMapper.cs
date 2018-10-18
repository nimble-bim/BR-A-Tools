namespace BRPLUSA.AutoCAD.Mappers
{
    public interface IMapper<in T, out T1>
    {
        T1 Map(T item);
    }
}