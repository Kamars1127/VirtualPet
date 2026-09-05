using Microsoft.AspNetCore.Mvc;
using VirtualPet.Application.Services;
using VirtualPet.Web.Models.Pets;

namespace VirtualPet.Web.Controllers
{
    public class PetController : Controller
    {
        private readonly PetApplicationService _petApplicationService;

        public PetController(PetApplicationService petApplicationService)
        {
            _petApplicationService = petApplicationService ?? throw new ArgumentNullException(nameof(petApplicationService));
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var pets = await _petApplicationService.GetPetsAsync(cancellationToken);

            return View(pets);
        }

        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
        {
            var pet = await _petApplicationService.GetPetAsync(id, cancellationToken);

            if (pet is null) return NotFound();

            var histories = await _petApplicationService.GetPetHistoriesAsync(id, cancellationToken);

            var viewModel = new PetDetailsViewModel
            {
                Pet = pet,
                Histories = histories
            };

            return View(viewModel);
        }

    }
}
