using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payments.Core.Domain.RepositoryInterfaces
{
    public interface IBundleRepository
    {
        Bundle Get(long id);
        List<Bundle> GetByAuthor(long authorId);
        List<Bundle> GetAllPublished();
        Bundle Create(Bundle bundle);
        Bundle Update(Bundle bundle);
        void Delete(long id);
    }
}
