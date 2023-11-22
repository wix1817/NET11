namespace BusinessLayerInterfaces.BusinessModels.PCBuildModels;

public class IndexBuildBlm 
{
    public int Count { get; set; }
    public int Page { get; set; }
    public int PerPage { get; set; }
    public IEnumerable<ShortBuildBlm> Items { get; set; }
}