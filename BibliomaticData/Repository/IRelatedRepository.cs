using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliomaticData.Repository
{
    public interface IRelatedRepository<REntity> where REntity : class
    {
        Task<IEnumerable<REntity>> GetAllRelated(Guid id);
        Task<REntity> GetRelatedById(Guid id, Guid relatedId);
        Task CreateRelated(REntity entity);     
        Task UpdateRelated(REntity entity);
        Task DeleteRelated(REntity entity);
        Task SaveChanges();
    }
}
