using DALInterfaces.DataModels;
using DALInterfaces.Models.PcBuild;
using DALInterfaces.Repositories.PCBuild;

namespace DALEfDB.Repositories.PCBuild
{
    public class FollowersRepository : BaseRepository<Followers>, IFollowersRepository
    {
        public FollowersRepository(WebContext context) : base(context)
        {
        }
    }
}
