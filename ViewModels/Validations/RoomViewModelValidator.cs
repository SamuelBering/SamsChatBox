using FluentValidation;

namespace DotNetGigs.ViewModels.Validations
{
    public class RoomViewModelValidator : AbstractValidator<RoomViewModel>
    {
        public RoomViewModelValidator()
        {
            RuleFor(vm => vm.Title).NotEmpty().WithMessage("Title cannot be empty");
        }
    }
}