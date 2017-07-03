namespace HBD.EntityFramework.Base
{
    public interface IPagable
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalItems { get; }
    }
}