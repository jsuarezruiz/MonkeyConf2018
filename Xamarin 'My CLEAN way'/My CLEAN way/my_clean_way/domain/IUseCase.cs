using System;
using System.Threading.Tasks;

namespace my_clean_way.domain
{
    public interface IUseCase<T, V>
    {
        Task<Transaction<T>> execute(V parameter);
    }
}
