using System.Linq.Expressions;
using BusinessLayerInterfaces.Common;
using DALInterfaces.Models;
using GamerShop.Models;

namespace GamerShop.Services
{
    public interface IPaginatorService
    {
        PaginatorViewModel<ViewModelTemplate> GetPaginatorViewModel<ViewModelTemplate, BlmTemplate, DbModel>(
            IPaginatorServices<BlmTemplate, DbModel> services,
            Func<BlmTemplate, ViewModelTemplate> mapViewModelFromBlm, 
            int page,
            int perPage) where DbModel : BaseModel;

        PaginatorViewModel<ViewModelTemplate> GetPaginatorViewModelWithFilter<ViewModelTemplate, BlmTemplate, DbModel>(
            IPaginatorServices<BlmTemplate, DbModel> services,
            Func<BlmTemplate, ViewModelTemplate> mapViewModelFromBlm,
            Expression<Func<DbModel, bool>> filter,
            string sortingCriteria,
            int page,
            int perPage,
            bool isAscending
        ) where DbModel : BaseModel;
    }
}