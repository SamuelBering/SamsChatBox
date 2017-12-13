using FluentValidation.Attributes;
using DotNetGigs.ViewModels.Validations;

namespace DotNetGigs.ViewModels
{
    [Validator(typeof(RoomViewModelValidator))]
    public class RoomViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public override bool Equals(object obj)
        {
            RoomViewModel roomViewModel = obj as RoomViewModel;
            return roomViewModel != null && roomViewModel.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}