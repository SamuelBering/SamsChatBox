using AutoMapper;
using DotNetGigs.Models;
using DotNetGigs.ViewModels;

namespace DotNetGigs.ViewModels.Mappings
{
    public class ModelToViewModelProfile : Profile
    {
        public ModelToViewModelProfile()
        {
            CreateMap<Place, PlaceViewModel>().ForMember(vm => vm.Name, map => map.MapFrom(m => m.name));
        }
    }
}