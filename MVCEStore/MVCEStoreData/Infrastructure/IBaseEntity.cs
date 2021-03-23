using Microsoft.EntityFrameworkCore;

namespace MvcEStoreData.Infrastructure
{
    public interface IBaseEntity
    {
        void Build(ModelBuilder builder);
    }
}