using BusinessLayerInterfaces.BusinessModels.PCBuildModels;
using BusinessLayerInterfaces.PcBuilderServices;
using GamerShop.Services;
using GamerShop.Models.PcBuild;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using DALInterfaces.Models.Movies;
using DALInterfaces.Models.PcBuild;

namespace GamerShop.Controllers.PcBuild;

public class PcBuildController : Controller
{
    private readonly IBuildServices _buildServices;
    private readonly IAuthService _authService;
    private readonly IPaginatorService _paginatorService;

    public PcBuildController(IBuildServices buildServices, IAuthService authService, IPaginatorService paginatorService)
    {
        _buildServices = buildServices;
        _authService = authService;
        _paginatorService = paginatorService;
    }

    public IActionResult Index(int page = 1, int perPage = 3, string sortingCriteria = "Date", bool isAscending = true)
    {
        var filter = (Expression<Func<Build, bool>>)(x => x.IsPrivate == false);

        var paginatorViewModel = _paginatorService.GetPaginatorViewModelWithFilter(
            _buildServices,
            MapBlmToViewModel,
            filter,
            sortingCriteria,
            page,
            perPage,
            isAscending);

        return View(paginatorViewModel);
    }

    private BuildsIndexViewModel MapBlmToViewModel(ShortBuildBlm shortBuildBlm)
    {
        return new BuildsIndexViewModel
        {
            Id = shortBuildBlm.Id.ToString(),
            UserName = shortBuildBlm.CreatorName,
            Price = shortBuildBlm.Price,
            Rating = shortBuildBlm.Rating.ToString(),
            BuildName = shortBuildBlm.Label,
            BuildPhotoPath = shortBuildBlm.PhotoPath,
            Processor = shortBuildBlm.ProcessorName,
            GPU = shortBuildBlm.GpuName ?? "",
            RedPrice = shortBuildBlm.RedPrice
        };
    }

    [Authorize]
    [HttpGet]
    public IActionResult CreateBuild()
    {
        var allComponents = _buildServices.GetAllComponents();
        var viewModel = new CreateBuildViewModel();
        viewModel.Cases = allComponents
            .Cases
            .Select(x => new SelectListItem()
            {
                Text = x.Name + " " + x.Price + " тугриков",
                Value = x.Id.ToString(),
            })
            .ToList();
        viewModel.Processors = allComponents
            .Processors
            .Select(x => new SelectListItem()
            {
                Text = x.Name + " " + x.Price + " тугриков",
                Value = x.Id.ToString(),
            })
            .ToList();
        viewModel.Motherboards = allComponents
            .Motherboards
            .Select(x => new SelectListItem()
            {
                Text = x.Name + " " + x.Price + " тугриков",
                Value = x.Id.ToString(),
            })
            .ToList();
        viewModel.Ssds = allComponents
            .Ssds
            .Select(x => new SelectListItem()
            {
                Text = x.Name + " " + x.Price + " тугриков",
                Value = x.Id.ToString(),
            })
            .ToList();
        viewModel.Gpus = allComponents
            .Gpus
            .Select(x => new SelectListItem()
            {
                Text = x.Name + " " + x.Price + " тугриков",
                Value = x.Id.ToString(),
            })
            .ToList();
        viewModel.Hdds = allComponents
            .Hdds
            .Select(x => new SelectListItem()
            {
                Text = x.Name + " " + x.Price + " тугриков",
                Value = x.Id.ToString(),
            })
            .ToList();
        viewModel.Coolers = allComponents
            .Coolers
            .Select(x => new SelectListItem()
            {
                Text = x.Name + " " + x.Price + " тугриков",
                Value = x.Id.ToString(),
            })
            .ToList();
        viewModel.Rams = allComponents
            .Rams
            .Select(x => new SelectListItem()
            {
                Text = x.Name + " " + x.Price + " тугриков",
                Value = x.Id.ToString(),
            })
            .ToList();
        viewModel.Psus = allComponents
            .Psus
            .Select(x => new SelectListItem()
            {
                Text = x.Name + " " + x.Price + " тугриков",
                Value = x.Id.ToString(),
            })
            .ToList();
        return View(viewModel);
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult CreateBuild(CreateBuildAnswerViewModel viewModel)
    {
        var currentUserId = _authService.GetCurrentUser().Id;
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index", "PcBuild");
        }

        var newBuild = new NewBuildBlm()
        {
            CreatorId = currentUserId,
            ProcessorId = viewModel.ProcessorId,
            MotherboardId = viewModel.MotherboardId,
            GpuId = viewModel.GpuId,
            GpuCount = viewModel.GpuCount,
            CurrentCaseId = viewModel.CaseId,
            CoolerId = viewModel.CoolerId,
            HddId = viewModel.HddId,
            HddCount = viewModel.HddCount,
            SsdId = viewModel.SsdId,
            SsdCount = viewModel.SsdCount,
            RamId = viewModel.RamId,
            RamCount = viewModel.RamCount,
            PsuId = viewModel.PsuId,
            Title = viewModel.Title,
            Description = viewModel.Description
        };
        _buildServices.CreateNewBuild(newBuild);
        return RedirectToAction("Index", "PcBuild");
    }

    public IActionResult Remove(int id) //TODO
    {
        return RedirectToAction("Index", "PcBuild");
    }

    [HttpGet]
    public IActionResult Build(int id)
    {
        var build = _buildServices.GetBuildById(id);
        var viewModel = new BuildViewModel()
        {
            Id = build.Id,
            CreatorName = build.CreatorName,
            CreatorAvatarPath = build.CreatorAvatarPath,
            DatePublished = build.DateOfCreate,
            Cpu = build.CpuName,
            CpuColler = build.CpuCollerName,
            Case = build.CaseName,
            CasePrice = build.CasePrice,
            CollerPrice = build.CollerPrice,
            CpuPrice = build.CpuPrice,
            CpuRate = "1",
            Comments = new List<string> { "1" },
            CommentsCount = "1",
            CpuTempIdle = "1",
            CpuTempLoad = "1",
            DateBuild = "1",
            Description = build.Description,
            Gpu = build.GpuName,
            GpuCount = build.GpuCount,
            GpuPrice = build.GpuPrice,
            GpuTempIdle = "1",
            GpuTempLoad = "1",
            Label = build.Label,
            Link = "1",
            Motherboard = build.MotherboardName,
            MotherboardPrice = build.MotherboardPrice,
            Psu = build.PsuName,
            PsuPrice = build.PsuPrice,
            Ram = build.RamName,
            RamPrice = build.RamPrice,
            Rating = build.Rating.ToString(),
            Storage = build.HddName,
            StorageCount = build.HddCount,
            StoragePrice = build.HddPrice
        };
        return View(viewModel);
    }

    [Authorize]
    public IActionResult Like(int buildId)
    {
        var userId = _authService.GetCurrentUser().Id;
        _buildServices.LikeBuild(userId, buildId);
        return RedirectToAction("Index");
    }

    [Authorize]
    public IActionResult Unlike(int buildId)
    {
        var userId = _authService.GetCurrentUser().Id;
        _buildServices.UnlikeBuild(userId, buildId);
        return RedirectToAction("Index");
    }
}