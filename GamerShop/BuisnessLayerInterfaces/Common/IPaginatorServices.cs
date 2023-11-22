using BusinessLayerInterfaces.BusinessModels;
using System.Linq.Expressions;
using DALInterfaces.Models;

namespace BusinessLayerInterfaces.Common
{
    public interface IPaginatorServices<BlmTemplate, DbModel> where DbModel : BaseModel
    {
        PaginatorBlm<BlmTemplate> GetPaginatorBlm(int page, int perPage);

        PaginatorBlm<BlmTemplate> GetPaginatorBlmWithFilter(
            Expression<Func<DbModel, bool>> filter,
            string sortingCriteria,
            int page,
            int perPage,
            bool isAscending) => GetPaginatorBlm(page, perPage);
    }
}
