namespace GamerShop.Models.PcBuild;

public class BuildViewModel
{
    public int Id { get; set; }
    public string Label { get; set; }
    public string CreatorAvatarPath { get; set; }
    public string CreatorName { get; set; }
    public string Rating { get; set; }
    public string CommentsCount { get; set; }
    public string Link { get; set; }
    public string Cpu { get; set; }
    public string CpuPrice { get; set; }
    public string CpuColler { get; set; }
    public string CollerPrice { get; set; }
    public string Motherboard { get; set; }
    public string MotherboardPrice { get; set; }
    public string Ram { get; set; }
    public string RamPrice { get; set; }
    public string Storage { get; set; }
    public string StoragePrice { get; set; }
    public string StorageCount { get; set; }
    public string Gpu { get; set; }
    public string GpuPrice { get; set; }
    public string GpuCount { get; set; }
    public string Case { get; set; }
    public string CasePrice { get; set; }
    public string Psu { get; set; }
    public string PsuPrice { get; set; }
    public string DatePublished { get; set; }
    public string DateBuild { get; set; }
    public string CpuRate { get; set; }
    public string CpuTempIdle { get; set; }
    public string CpuTempLoad { get; set; }
    public string GpuTempIdle { get; set; }
    public string GpuTempLoad { get; set; }
    public string Description { get; set; }
    public List<string> Comments { get; set; }
}