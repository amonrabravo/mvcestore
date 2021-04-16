using Microsoft.EntityFrameworkCore;

namespace MVCEStoreData.Infrastructure
{
    public interface IBaseEntity
    {
        void Build(ModelBuilder builder);
    }
}