using BusinessLayerInterfaces.BusinessModels.PCBuildModels;
using BusinessLayerInterfaces.Common;
using DALInterfaces.Models.PcBuild;

namespace BusinessLayerInterfaces.PcBuilderServices;

public interface IBuildServices : IPaginatorServices<ShortBuildBlm, Build>
{
    IEnumerable<BaseBuildBlm> GetAllBuilds();
    void Remove(int id);
    void Save(BaseBuildBlm buildBlm);
    public AllComponentsForAddingBlm GetAllComponents();
    void CreateNewBuild(NewBuildBlm newBuild);
    BuildBlm GetBuildById(int id);
    void LikeBuild(int userId, int buildId);
    void UnlikeBuild(int userId, int buildId);
}