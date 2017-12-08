using FluentValidation.Attributes;
using DotNetGigs.ViewModels.Validations;

namespace DotNetGigs.ViewModels
{
    [Validator(typeof(RoomViewModelValidator))]
    public class RoomViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}