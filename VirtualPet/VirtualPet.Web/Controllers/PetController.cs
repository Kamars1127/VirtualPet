using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VirtualPet.Application.DTOs;
using VirtualPet.Application.Services;
using VirtualPet.Infrastructure.Identity;
using VirtualPet.Web.Models.Pets;
using VirtualPet.Web.Options;

namespace VirtualPet.Web.Controllers
{
    [Authorize]
    public sealed class PetController : Controller
    {
        private readonly PetApplicationService _petApplicationService;
        private readonly UserApplicationService _userApplicationService;
        private readonly PetGameOptions _petGameOptions;
        private readonly ILogger<PetController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public PetController(PetApplicationService petApplicationService, UserApplicationService userApplicationService,
            IOptions<PetGameOptions> petGameOptions, ILogger<PetController> logger, UserManager<ApplicationUser> userManager)
        {
            _petApplicationService = petApplicationService ?? throw new ArgumentNullException(nameof(petApplicationService));
            _userApplicationService = userApplicationService ?? throw new ArgumentNullException(nameof(userApplicationService));

            ArgumentNullException.ThrowIfNull(petGameOptions);
            _petGameOptions = petGameOptions.Value;

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var currentUser = await GetCurrentUserAsync(cancellationToken);

            var pets = await _petApplicationService.GetPetsByUserAsync(currentUser.Id, cancellationToken);

            return View(pets);
        }

        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
        {
            var pet = await GetOwnedPetAsync(id, cancellationToken); 

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
            await GetOwnedPetAsync(id, cancellationToken);
            await _petApplicationService.FeedPetAsync(id, cancellationToken);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Play(Guid id, CancellationToken cancellationToken)
        {
            await GetOwnedPetAsync(id, cancellationToken);
            await _petApplicationService.PlayWithPetAsync(id, cancellationToken);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rest(Guid id, CancellationToken cancellationToken)
        {
            await GetOwnedPetAsync(id, cancellationToken);
            await _petApplicationService.RestPetAsync(id, cancellationToken);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GainExperience(Guid id, CancellationToken cancellationToken)
        {
            await GetOwnedPetAsync(id, cancellationToken);
            _logger.LogInformation("Training pet {PetId}.", id);

            await _petApplicationService.GainExperienceAsync(id, _petGameOptions.TrainingExperience, cancellationToken);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePetViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUser = await GetCurrentUserAsync(cancellationToken);

            var pet = await _petApplicationService.CreatePetAsync(currentUser.Id, model.Name, model.Species!.Value, cancellationToken);

            _logger.LogInformation("Pet {PetId} ({PetName}) was create for user {UserId}.",pet.Id, pet.Name, currentUser.Id);

            return RedirectToAction(nameof(Details), new { id = pet.Id });
        }

        private async Task<UserDto> GetCurrentUserAsync(CancellationToken cancellationToken)
        {
            var account = await _userManager.GetUserAsync(User);

            if(account is null)
            {
                throw new UnauthorizedAccessException("Current account was not found.");
            }

            var user = await _userApplicationService.GetUserByAccountIdAsync(account.Id, cancellationToken);

            if(user is null)
            {
                throw new InvalidOperationException("Domain user for the current account was not found.");
            }

            return user;
        }

        private async Task<PetDto> GetOwnedPetAsync(Guid petId, CancellationToken cancellationToken)
        {
            var currentUser = await GetCurrentUserAsync (cancellationToken);

            var pet = await _petApplicationService.GetPetAsync(petId, cancellationToken);

            if(pet is null)
            {
                throw new KeyNotFoundException($"Pet with id '{petId}' was not found.");
            }

            if(pet.UserId != currentUser.Id)
            {
                throw new UnauthorizedAccessException("You are not allowed to access this pet.");
            }

            return pet;
        }
    }
}
