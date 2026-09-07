using Microsoft.AspNetCore.Mvc;
using VirtualPet.Application.Services;
using VirtualPet.Web.Models.Pets;

namespace VirtualPet.Web.Controllers
{
    public sealed class PetController : Controller
    {
        private readonly PetApplicationService _petApplicationService;
        private readonly UserApplicationService _userApplicationService;

        public PetController(PetApplicationService petApplicationService, UserApplicationService userApplicationService)
        {
            _petApplicationService = petApplicationService ?? throw new ArgumentNullException(nameof(petApplicationService));
            _userApplicationService = userApplicationService ?? throw new ArgumentNullException(nameof(userApplicationService));
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

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var users = await _userApplicationService.GetUsersAsync(cancellationToken);

            var viewModel = new CreatePetViewModel { Users = users };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePetViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                model.Users = await _userApplicationService.GetUsersAsync(cancellationToken);

                return View(model);
            }

            var pet = await _petApplicationService.CreatePetAsync(model.UserId!.Value, model.Name, model.Species!.Value, cancellationToken);

            return RedirectToAction(nameof(Details), new { id = pet.Id });
        }
    }
}
