namespace BusinessLayerInterfaces.BusinessModels.PCBuildModels;

public class BuildBlm : BaseBuildBlm
{
    public string Description { get; set; }
    public bool isVirtual { get; set; }
    public string? PhotosPath { get; set; }
    public bool IsPrivate { get; set; }
    public string CreatorAvatarPath { get; set; }
    public string Link { get; set; }
    public string CpuName { get; set; }
    public string CpuPrice { get; set; }
    public string CpuCollerName { get; set; }
    public string CollerPrice { get; set; }
    public string MotherboardName { get; set; }
    public string MotherboardPrice { get; set; }
    public string RamName { get; set; }
    public string RamPrice { get; set; }
    public string RamCount { get; set; }
    public string? SsdName { get; set; }
    public string? SsdPrice { get; set; }
    public string? SsdCount { get; set; }
    public string? HddName { get; set; }
    public string? HddPrice { get; set; }
    public string? HddCount { get; set; }
    public string? GpuName { get; set; }
    public string? GpuPrice { get; set; }
    public string? GpuCount { get; set; }
    public string? CaseName { get; set; }
    public string? CasePrice { get; set; }
    public string PsuName { get; set; }
    public string PsuPrice { get; set; }
}