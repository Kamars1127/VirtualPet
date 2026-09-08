using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VirtualPet.Application.Services;
using VirtualPet.Web.Models.Pets;
using VirtualPet.Web.Options;

namespace VirtualPet.Web.Controllers
{
    public sealed class PetController : Controller
    {
        private readonly PetApplicationService _petApplicationService;
        private readonly UserApplicationService _userApplicationService;
        private readonly PetGameOptions _petGameOptions;
        private readonly ILogger<PetController> _logger;


        public PetController(PetApplicationService petApplicationService, UserApplicationService userApplicationService,
            IOptions<PetGameOptions> petGameOptions, ILogger<PetController> logger)
        {
            _petApplicationService = petApplicationService ?? throw new ArgumentNullException(nameof(petApplicationService));
            _userApplicationService = userApplicationService ?? throw new ArgumentNullException(nameof(userApplicationService));

            ArgumentNullException.ThrowIfNull(petGameOptions);
            _petGameOptions = petGameOptions.Value;

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            _logger.LogInformation("Training pet {PetId}.", id);

            await _petApplicationService.GainExperienceAsync(id, _petGameOptions.TrainingExperience, cancellationToken);

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

            _logger.LogInformation("Pet {PetId} ({PetName}) was create for user {UserId}.",pet.Id, pet.Name, model.UserId.Value);

            return RedirectToAction(nameof(Details), new { id = pet.Id });
        }
    }
}
