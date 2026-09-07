using Microsoft.AspNetCore.Mvc;
using VirtualPet.Application.Services;
using VirtualPet.Web.Models.Pets;

namespace VirtualPet.Web.Controllers
{
    public sealed class PetController : Controller
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Feed(Guid id, CancellationToken cancellationToken)
        {
            await _petApplicationService.FeedPetAsync(id, cancellationToken);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Play(Guid id, CancellationToken cancellationToken)
        {
            await _petApplicationService.PlayWithPetAsync(id, cancellationToken);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rest(Guid id, CancellationToken cancellationToken)
        {
            await _petApplicationService.RestPetAsync(id, cancellationToken);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GainExperience(Guid id, CancellationToken cancellationToken)
        {
            await _petApplicationService.GainExperienceAsync(id, 25, cancellationToken);

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
