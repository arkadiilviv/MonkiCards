using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monki.API.Models;
using Monki.DAL.Interfaces;
using Monki.DAL.Models;

namespace Monki.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DecksController : ControllerBase
	{
		private readonly IDeckService _deckService;
		private readonly IUserService _userService;
		public DecksController(IDeckService deckService, IUserService userService)
		{
			_deckService = deckService;
			_userService = userService;
		}
		/// <summary>
		/// Returns decks for current user
		/// </summary>
		/// <returns></returns>
		[HttpGet("GetDeck")]
		[Authorize]
		public async Task<IActionResult> GetDeck()
		{
			try
			{
				var user = await _userService.GetUserAsync(User);
				if (!user.Success)
					return Unauthorized(user);
				string userId = (user.Data as MonkiUser).Id;
				var decks = _deckService.GetAll()
					.Where(x => x.UserId == userId)
					.Select(x => new DeckDTOResponse(x));

				return Ok(ServiceResult.SuccessResult(data: decks));
			} catch (Exception ex)
			{
				return BadRequest(ServiceResult.FailureResult(data: ex));
			}
		}

		[HttpGet("GetDecksByUser")]
		[Authorize]
		public async Task<IActionResult> GetDecksByUser(string userId)
		{
			try
			{
				var decks = _deckService.GetAll()
					.Where(x => x.IsPrivate == false && x.UserId == userId)
					.Select(x => new DeckDTOResponse(x));

				return Ok(ServiceResult.SuccessResult(data: decks));
			} catch (Exception ex)
			{
				return BadRequest(ServiceResult.FailureResult(data: ex));
			}
		}

		[HttpGet("GetDeckPublic")]
		public IActionResult GetDeckPublic()
		{
			try
			{
				var decks = _deckService.GetAll()
					.Where(x => x.IsPrivate == false)
					.Select(x => new DeckDTOResponse(x));

				return Ok(ServiceResult.SuccessResult(data: decks));
			} catch (Exception ex)
			{
				return BadRequest(ServiceResult.FailureResult(data: ex));
			}
		}
		

		[HttpPost("CreateDeck")]
		[Authorize]
		public async Task<IActionResult> CreateDeck([FromBody] DeckDTO deck)
		{
			try
			{
				var user = await _userService.GetUserAsync(User);
				if (!user.Success)
					return Unauthorized(user);
				var res = await _deckService.AddAsync(new MonkiDeck
				{
					Name = deck.Name,
					Description = deck.Description,
					IsPrivate = deck.IsPrivate,
					UserId = (user.Data as MonkiUser)!.Id
				});

				if (res == null)
					return BadRequest(ServiceResult.FailureResult("Failed to create deck."));

				return Ok(ServiceResult.SuccessResult(data: res));
			} catch (Exception ex)
			{
				return BadRequest(ServiceResult.FailureResult(data: ex));
			}
		}

		[HttpDelete("DeleteDeck")]
		[Authorize]
		public async Task<IActionResult> DeleteDeck(int DeckId)
		{
			try
			{
				var user = await _userService.GetUserAsync(User);
				if (!user.Success)
					return Unauthorized(user);

				var deck = _deckService.GetAll().FirstOrDefault(x => x.Id == DeckId && x.UserId == (user.Data as MonkiUser)!.Id);
				if (deck == null)
					return NotFound(ServiceResult.FailureResult("Deck not found."));

				await _deckService.Delete(deck);

				return Ok(ServiceResult.SuccessResult());
			} catch (Exception ex)
			{
				return BadRequest(ServiceResult.FailureResult(data: ex));
			}
		}

		[HttpPut("UpdateDeck")]
		[Authorize]
		public async Task<IActionResult> UpdateDeck([FromBody] DeckDTO deckDto)
		{
			try
			{
				var user = await _userService.GetUserAsync(User);
				if (!user.Success)
					return Unauthorized(user);

				var deck = _deckService.GetById(deckDto.Id);
				if (deck == null)
					return NotFound(ServiceResult.FailureResult("Deck not found."));

				if (deck.UserId != (user.Data as MonkiUser)!.Id)
					return BadRequest(ServiceResult.FailureResult("You are not authorized to update this deck."));

				deck.Name = deckDto.Name;
				deck.Description = deckDto.Description;
				deck.UpdatedAt = DateTime.UtcNow;

				await _deckService.UpdateModelAsync(deck);

				return Ok(ServiceResult.SuccessResult("Deck updated successfully.", deck));
			}
			catch (Exception ex)
			{
				return BadRequest(ServiceResult.FailureResult(ex.Message, ex));
			}
		}
	}
}
