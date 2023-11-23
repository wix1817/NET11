using System.Linq.Expressions;
using BusinessLayerInterfaces.BusinessModels;
using BusinessLayerInterfaces.BusinessModels.PCBuildModels;
using BusinessLayerInterfaces.PcBuilderServices;
using DALInterfaces.DataModels.PcBuild;
using DALInterfaces.Models;
using DALInterfaces.Models.PcBuild;
using DALInterfaces.Repositories;
using DALInterfaces.Repositories.Movies;
using DALInterfaces.Repositories.PCBuild;

namespace BusinessLayer.PcBuildServices
{
    public class PcBuildServices : IBuildServices
    {
        private IBuildRepository _buildRepository;
        private IProcessorRepository _processorRepository;
        private IMotherboardRepository _motherboardRepository;
        private IGpuRepository _gpuRepository;
        private IRamRepository _ramRepository;
        private ISsdRepository _ssdRepository;
        private IHddRepository _hddRepository;
        private ICaseRepository _caseRepository;
        private ICoolerRepository _coolerRepository;
        private IPsuRepository _psuRepository;
        private IUserRepository _userRepository;

        public PcBuildServices(IBuildRepository buildRepository, IProcessorRepository processorRepository, 
            IMotherboardRepository motherboardRepository, IRamRepository ramRepository, IGpuRepository gpuRepository, 
            ISsdRepository ssdRepository, IHddRepository hddRepository, ICaseRepository caseRepository, 
            ICoolerRepository coolerRepository, IPsuRepository psuRepository, IUserRepository userRepository, IRatingRepository ratingRepository)
        {
            _buildRepository = buildRepository;
            _processorRepository = processorRepository;
            _motherboardRepository = motherboardRepository;
            _ramRepository = ramRepository;
            _gpuRepository = gpuRepository;
            _ssdRepository = ssdRepository;
            _hddRepository = hddRepository;
            _caseRepository = caseRepository;
            _coolerRepository = coolerRepository;
            _psuRepository = psuRepository;
            _userRepository = userRepository;
        }

        public PaginatorBlm<ShortBuildBlm> GetPaginatorBlm(int page, int perPage)
        {
            var data = _buildRepository.GetPaginatorDataModel(Map, page, perPage);
            return new PaginatorBlm<ShortBuildBlm>()
            {
                Count = data.Count,
                Page = data.Page,
                PerPage = data.PerPage,
                Items = data.Items
                    .Select(model => new ShortBuildBlm
                    {
                        Id = model.Id,
                        Label = model.Label,
                        Price = model.Price.ToString(),
                        Rating = model.Rating,
                        CreatorId = model.CreatorId,
                        CreatorName = model.CreatorName,
                        DateOfCreate = model.DateOfCreate.ToShortDateString(),
                        ProcessorName = model.ProcessorName,
                        GpuName = model.GpuName
                    }).ToList()
            };
        }

        private BuildDataModel Map(Build build)
        {
            return new BuildDataModel
            {
                Id = build.Id,
                Label = build.Label,
                Price = build.Price,
                Rating = build.Rating,
                CreatorId = build.Creator.Id,
                CreatorName = build.Creator.Name,
                DateOfCreate = build.DateOfCreate,
                ProcessorName = build.Processor.FullName,
                GpuName = build.Gpu.FullName
            };
        }

        public PaginatorBlm<ShortBuildBlm> GetPaginatorBlmWithFilter(
            Expression<Func<Build, bool>> filter,
            string sortingCriteria,
            int page,
            int perPage,
            bool isAscending)
        {
            Func<Build, IComparable> funcSortingCriteria;

            switch (sortingCriteria)
            {
                case "Date":
                    funcSortingCriteria = collection =>
                        collection
                            .DateOfCreate;
                    break;

                case "Price":
                    funcSortingCriteria = collection =>
                        collection
                            .Price;
                    break;
                case "Rating":
                    funcSortingCriteria = collection =>
                        collection
                            .Rating;
                    break;

                default:
                    throw new ArgumentException("Неподдерживаемый критерий сортировки", nameof(sortingCriteria));
            }
            var movieCollectionPaginatorDataModel = _buildRepository
                .GetPaginatorDataModelWithFilter(Map, filter, page, perPage, funcSortingCriteria, isAscending);

            return new PaginatorBlm<ShortBuildBlm>()
            {
                Page = movieCollectionPaginatorDataModel.Page,
                PerPage = movieCollectionPaginatorDataModel.PerPage,
                Count = movieCollectionPaginatorDataModel.Count,
                Items = movieCollectionPaginatorDataModel
                    .Items
                    .Select(model => new ShortBuildBlm
                    {
                        Id = model.Id,
                        Label = model.Label,
                        Price = model.Price.ToString(),
                        Rating = model.Rating,
                        CreatorId = model.CreatorId,
                        CreatorName = model.CreatorName,
                        DateOfCreate = model.DateOfCreate.ToShortDateString(),
                        ProcessorName = model.ProcessorName,
                        GpuName = model.GpuName
                    })
                    .ToList()
            };
        }

        public void CreateNewBuild(NewBuildBlm newBuildBlm)
        {
            var processor = _processorRepository.Get(newBuildBlm.ProcessorId);
            var motherboard = _motherboardRepository.Get(newBuildBlm.MotherboardId);
            var gpu = newBuildBlm.GpuId == null ? null : _gpuRepository.Get(newBuildBlm.GpuId);
            var ssd = newBuildBlm.SsdId == null ? null : _ssdRepository.Get(newBuildBlm.SsdId);
            var hdd = newBuildBlm.HddId == null ? null : _hddRepository.Get(newBuildBlm.HddId);
            var ram = _ramRepository.Get(newBuildBlm.RamId);
            var psu = _psuRepository.Get(newBuildBlm.PsuId);
            var currentCase = _caseRepository.Get(newBuildBlm.CurrentCaseId);
            var cooler = _coolerRepository.Get(newBuildBlm.CoolerId);
            var price = CalculatePrice(processor, motherboard, gpu, newBuildBlm.GpuCount, ssd, 
                newBuildBlm.SsdCount, hdd, newBuildBlm.HddCount, ram, newBuildBlm.RamCount, psu,
                currentCase, cooler);
            var buildDb = new Build()
            {
                Creator = _userRepository.Get(newBuildBlm.CreatorId),
                Processor = processor,
                Motherboard = motherboard,
                Gpu = gpu,
                Ssd = ssd,
                Hdd = hdd,
                Ram = ram,
                Psu = psu,
                Case = currentCase,
                Cooler = cooler,
                isVirtual = false, //TODO
                DateOfCreate = DateTime.Now,
                Label = newBuildBlm.Title,
                IsPrivate = false, //TODO
                GpusCount = newBuildBlm.GpuCount,
                SsdCount = newBuildBlm.SsdCount,
                HddCount = newBuildBlm.HddCount,
                RamCount = newBuildBlm.RamCount,
                Price = price
            };
            _buildRepository.Save(buildDb);
        }

        public BuildBlm GetBuildById(int id)
        {
            var build = _buildRepository.Get(id);
            return new BuildBlm()
            {
                Id = build.Id,
                Label = build.Label,
                Description = build.Description,
                isVirtual = build.isVirtual,
                PhotosPath = build.PhotosPath,
                Rating = build.Rating,
                Price = build.Price.ToString(),
                IsPrivate = build.IsPrivate,
                CreatorId = build.Creator.Id,
                CreatorAvatarPath = build.Creator.AvatarPath,
                CreatorName = build.Creator.Name,
                //Link = $"dsgsdgsdgsdgsgddsg/{build.Id}", //TODO
                CpuName = build.Processor.FullName,
                CpuPrice = build.Processor.Price.ToString(),
                CpuCollerName = build.Cooler.FullName,
                CollerPrice = build.Cooler.Price.ToString(),
                MotherboardName = build.Motherboard.FullName,
                MotherboardPrice = build.Motherboard.Price.ToString(),
                RamName = build.Ram.FullName,
                RamPrice = build.Ram.Price.ToString(),
                RamCount = build.RamCount.ToString(),
                SsdName = build.Ssd.FullName,
                SsdCount = build.SsdCount.ToString(),
                SsdPrice = build.Ssd.Price.ToString(),
                HddName = build.Hdd.FullName,
                HddPrice = build.Hdd.Price.ToString(),
                HddCount = build.HddCount.ToString(),
                GpuName = build.Gpu.FullName,
                GpuPrice = build.Gpu.Price.ToString(),
                GpuCount = build.GpusCount.ToString(),
                CaseName = build.Case.FullName,
                CasePrice = build.Case.Price.ToString(),
                PsuName = build.Psu.FullName,
                PsuPrice = build.Psu.Price.ToString(),
                DateOfCreate = build.DateOfCreate.ToString()
            };
        }

        public void LikeBuild(int userId, int buildId)
        {
            var user = _userRepository.Get(userId);
            var build = _buildRepository.Get(buildId);
            if (!build.UsersWhoLikeIt.Contains(user))
            {
                build.Rating += 1;
                build.UsersWhoLikeIt.Add(user);
                _buildRepository.Update(build);
            }
        }

        public void UnlikeBuild(int userId, int buildId)
        {
            var user = _userRepository.Get(userId);
            var build = _buildRepository.Get(buildId);
            if (build.UsersWhoLikeIt.Contains(user))
            {
                build.Rating -= 1;
                build.UsersWhoLikeIt.Remove(user);
                _buildRepository.Update(build);
            }
        }

        private decimal CalculatePrice(Processor processor, Motherboard motherboard, Gpu? gpu, int? gpuCount, Ssd? ssd,
            int? ssdCount, Hdd? hdd, int? hddCount, Ram ram, int? ramCount, Psu psu, Case currentCase, Cooler cooler)
        {
            var gpusSum = gpu?.Price * gpuCount ?? 0;
            var ssdSum = ssd?.Price * ssdCount ?? 0;
            var hddSum = hdd?.Price * hddCount ?? 0;
            var ramSum = ram?.Price * ramCount ?? 0;
            return processor.Price + motherboard.Price + gpusSum + ssdSum + hddSum + ramSum + psu.Price +
                   currentCase.Price + cooler.Price;
        }

        private int CalculateRating(Build build)
        {
            return build.UsersWhoLikeIt.Count;
        }

        private IEnumerable<ComponentBlm> GetAllProcessors()
        {
            return _processorRepository.GetAll().Select(c => new ComponentBlm()
            {
                Id = c.Id,
                Name = c.FullName,
                Price = c.Price
            });
        }

        private IEnumerable<ComponentBlm> GetAllCases()
        {
            return _caseRepository.GetAll().Select(c => new ComponentBlm()
            {
                Id = c.Id,
                Name = c.FullName,
                Price = c.Price
            }); 
        }

        private IEnumerable<ComponentBlm> GetAllMotherboards()
        {
            return _motherboardRepository.GetAll().Select(c => new ComponentBlm()
            {
                Id = c.Id,
                Name = c.FullName,
                Price = c.Price
            }); 
        }

        private IEnumerable<ComponentBlm> GetAllRams()
        {
            return _ramRepository.GetAll().Select(c => new ComponentBlm()
            {
                Id = c.Id,
                Name = c.FullName,
                Price = c.Price
            }); 
        }

        private IEnumerable<ComponentBlm> GetAllPsus()
        {
            return _psuRepository.GetAll().Select(c => new ComponentBlm()
            {
                Id = c.Id,
                Name = c.FullName,
                Price = c.Price
            }); 
        }

        private IEnumerable<ComponentBlm> GetAllCoolers()
        {
            return _coolerRepository.GetAll().Select(c => new ComponentBlm()
            {
                Id = c.Id,
                Name = c.FullName,
                Price = c.Price
            }); 
        }

        private IEnumerable<ComponentBlm> GetAllGpus()
        {
            return _gpuRepository.GetAll().Select(c => new ComponentBlm()
            {
                Id = c.Id,
                Name = c.FullName,
                Price = c.Price
            }); 
        }

        private IEnumerable<ComponentBlm> GetAllSsd()
        {
            return _ssdRepository.GetAll().Select(c => new ComponentBlm()
            {
                Id = c.Id,
                Name = c.FullName,
                Price = c.Price
            }); 
        }

        private IEnumerable<ComponentBlm> GetAllHdd()
        {
            return _hddRepository.GetAll().Select(c => new ComponentBlm()
            {
                Id = c.Id,
                Name = c.FullName,
                Price = c.Price
            }); 
        }

        public AllComponentsForAddingBlm GetAllComponents()
        {
            return new AllComponentsForAddingBlm()
            {
                Processors = GetAllProcessors(),
                Cases = GetAllCases(),
                Motherboards = GetAllMotherboards(),
                Rams = GetAllRams(),
                Ssds = GetAllSsd(),
                Hdds = GetAllHdd(),
                Coolers = GetAllCoolers(),
                Gpus = GetAllGpus(),
                Psus = GetAllPsus()
            };
        }

        public IEnumerable<BaseBuildBlm> GetAllBuilds()
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Save(BaseBuildBlm buildBlm)
        {
            throw new NotImplementedException();
        }
    }
}
