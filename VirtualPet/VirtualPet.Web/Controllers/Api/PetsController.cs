using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VirtualPet.Application.DTOs;
using VirtualPet.Application.Services;
using VirtualPet.Infrastructure.Identity;
using VirtualPet.Web.Models.Api.Pets;
using VirtualPet.Web.Options;

namespace VirtualPet.Web.Controllers.Api
{
    [ApiController]
    [Route("api/pets")]
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public sealed class PetsController : ControllerBase
    {
        private readonly PetApplicationService _petApplicationService;
        private readonly UserApplicationService _userApplicationService;
        private readonly PetGameOptions _petGameOptions;
        private readonly UserManager<ApplicationUser> _userManager;

        public PetsController(PetApplicationService petApplicationService, UserApplicationService userApplicationService,
            IOptions<PetGameOptions> petGameOptions, UserManager<ApplicationUser> userManager)
        {
            _petApplicationService = petApplicationService ?? throw new ArgumentNullException(nameof(petApplicationService));
            _userApplicationService = userApplicationService ?? throw new ArgumentNullException(nameof(userApplicationService));

            ArgumentNullException.ThrowIfNull(petGameOptions);
            _petGameOptions = petGameOptions.Value;

            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }


        private async Task<UserDto> GetCurrentUserAsync(CancellationToken cancellationToken)
        {
            var account = await _userManager.GetUserAsync(User);

            if (account is null)
            {
                throw new UnauthorizedAccessException("Current account was not found.");
            }

            var user = await _userApplicationService.GetUserByAccountIdAsync(account.Id, cancellationToken);

            if (user is null)
            {
                throw new InvalidOperationException("Domain user for the current account was not found.");
            }

            return user;
        }

        private async Task<PetDto> GetOwnedPetAsync(Guid petId, CancellationToken cancellationToken)
        {
            var currentUser = await GetCurrentUserAsync(cancellationToken);

            var pet = await _petApplicationService.GetPetAsync(petId, cancellationToken);

            if (pet is null)
            {
                throw new KeyNotFoundException($"Pet with id '{petId}' was not found.");
            }

            if (pet.UserId != currentUser.Id)
            {
                throw new UnauthorizedAccessException("You are not allowed to access this pet.");
            }

            return pet;
        }

        private static PetActionResponse CreateActionResponse(PetActionResultDto result, string message)
        {
            return new PetActionResponse
            {
                Pet = result.Pet,
                Evolved = result.Evolved,
                Message = message
            };
        }

        /// <summary>
        /// 取得目前登入玩家擁有的所有 Pet
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PetDto>>> GetAll(CancellationToken cancellationToken)
        {
            var currentUser = await GetCurrentUserAsync(cancellationToken);

            var pets = await _petApplicationService.GetPetsByUserAsync(currentUser.Id, cancellationToken);

            return Ok(pets);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PetDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var pet = await GetOwnedPetAsync(id, cancellationToken);

            return Ok(pet);
        }


        [HttpGet("{id:guid}/histories")]
        public async Task<ActionResult<IReadOnlyList<PetHistoryDto>>> GetHistories(Guid id, CancellationToken cancellationToken)
        {
            await GetOwnedPetAsync(id, cancellationToken);

            var histories = await _petApplicationService.GetPetHistoriesAsync(id, cancellationToken);

            return Ok(histories);
        }

        [HttpPost]
        public async Task<ActionResult<PetDto>> Create(CreatePetRequest request, CancellationToken cancellationToken)
        {
            var currentUser = await GetCurrentUserAsync(cancellationToken);

            var pet = await _petApplicationService.CreatePetAsync(currentUser.Id, request.Name, request.Species!.Value, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
        }

        [HttpPost("{id:guid}/feed")]
        public async Task<ActionResult<PetActionResponse>> Feed(Guid id, CancellationToken cancellationToken)
        {
            await GetOwnedPetAsync(id, cancellationToken);

            var result = await _petApplicationService.FeedPetAsync(id, cancellationToken);

            return Ok(CreateActionResponse(result, $"{result.Pet.Name} 已完成餵食。"));
        }

        [HttpPost("{id:guid}/play")]
        public async Task<ActionResult<PetActionResponse>> Play(Guid id, CancellationToken cancellationToken)
        {
            await GetOwnedPetAsync(id, cancellationToken);

            var result = await _petApplicationService.PlayWithPetAsync(id, cancellationToken);

            return Ok(CreateActionResponse(result, $"{result.Pet.Name} 玩得很開心。"));
        }

        [HttpPost("{id:guid}/rest")]
        public async Task<ActionResult<PetActionResponse>> Rest(Guid id, CancellationToken cancellationToken)
        {
            await GetOwnedPetAsync(id, cancellationToken);

            var result = await _petApplicationService.RestPetAsync(id, cancellationToken);

            return Ok(CreateActionResponse(result, $"{result.Pet.Name} 已完成休息。"));
        }

        [HttpPost("{id:guid}/gain-experience")]
        public async Task<ActionResult<PetActionResponse>> GainExperience(Guid id, CancellationToken cancellationToken)
        {
            await GetOwnedPetAsync(id, cancellationToken);

            var result = await _petApplicationService.GainExperienceAsync(id, _petGameOptions.TrainingExperience, cancellationToken);

            return Ok(CreateActionResponse(result, $"{result.Pet.Name} 獲得 {_petGameOptions.TrainingExperience} EXP。"));
        }
    }
}
