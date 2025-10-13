using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using Monki.API.Models;
using Monki.DAL.Interfaces;
using Monki.DAL.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Monki.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CardsController : ControllerBase
	{
		private readonly ICardService _cardService;
		private readonly IUserService _userService;
		private readonly IDeckService _deckService;
		public CardsController(ICardService cardService, IUserService userService, IDeckService deckService)
		{
			_cardService = cardService;
			_userService = userService;
			_deckService = deckService;
		}
		// GET: api/<CardsController>
		[HttpGet("GetByDeckId")]
		public async Task<IActionResult> GetByDeckId(int deckId)
		{
			try
			{
				var deck = _deckService.GetById(deckId);
				if (deck.IsPrivate)
				{
					var user = await _userService.GetUserAsync(User);
					if (!user.Success)
						return Unauthorized(ServiceResult.FailureResult("Unauthorized", data: user));
					if (!deck.UserId.Equals((user.Data as MonkiUser)!.Id))
						return BadRequest(ServiceResult.FailureResult("You are not authorized to access this private deck."));
				}
				var cards = _cardService.GetCardsByDeckId(deckId);
				return Ok(ServiceResult.SuccessResult(data: cards));
			} catch (Exception ex)
			{
				return BadRequest(ServiceResult.FailureResult(ex.Message, data: ex));
			}

		}

		// POST api/<CardsController>
		[HttpPost("AddCard")]
		[Authorize]
		public async Task<IActionResult> AddCard([FromBody] CardDTO card)
		{
			try
			{
				var user = await _userService.GetUserAsync(User);
				if (!user.Success)
					return Unauthorized(user);
				var deck = _deckService.GetById(card.DeckId);
				if (deck.UserId != (user.Data as MonkiUser)!.Id)
					return BadRequest(ServiceResult.FailureResult("You are not authorized to add cards to this deck."));

				var res = await _cardService.AddAsync(new MonkiCard()
				{
					SideA = card.SideA,
					SideB = card.SideB,
					ExampleA = card.ExampleA,
					ExampleB = card.ExampleB,
					DeckId = card.DeckId
				});
				if (res == null)
					return BadRequest(ServiceResult.FailureResult("Failed to add card."));

				return Ok(ServiceResult.SuccessResult(data: res));
			} catch (Exception ex)
			{
				return BadRequest(ServiceResult.FailureResult(ex.Message, data: ex));
			}
		}

		// DELETE api/<CardsController>/5
		[HttpDelete("DeleteCard")]
		[Authorize]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var card = _cardService.GetById(id);
				if (card == null)
					return NotFound(ServiceResult.FailureResult("Card not found."));
				await _cardService.Delete(card);
				return Ok(ServiceResult.SuccessResult(data: id));
			} catch (Exception ex)
			{
				return BadRequest(ServiceResult.FailureResult(ex.Message, data: ex));
			}
		}

		// PUT api/<CardsController>/5
		[HttpPut("UpdateCard")]
		[Authorize]
		public async Task<IActionResult> UpdateCard([FromBody] CardDTO cardDto)
		{
			try
			{
				var user = await _userService.GetUserAsync(User);
				if (!user.Success)
					return Unauthorized(user);

				var card = _cardService.GetById(cardDto.Id);
				if (card == null)
					return NotFound(ServiceResult.FailureResult("Card not found."));

				var deck = _deckService.GetById(card.DeckId);
				if (deck.UserId != (user.Data as MonkiUser)!.Id)
					return BadRequest(ServiceResult.FailureResult("You are not authorized to update cards in this deck."));

				// Update fields
				card.SideA = cardDto.SideA;
				card.SideB = cardDto.SideB;
				card.ExampleA = cardDto.ExampleA;
				card.ExampleB = cardDto.ExampleB;
				card.UpdatedAt = DateTime.UtcNow;

				await _cardService.UpdateModelAsync(card);

				return Ok(ServiceResult.SuccessResult("Card updated successfully.", card));
			} catch (Exception ex)
			{
				return BadRequest(ServiceResult.FailureResult(ex.Message, ex));
			}
		}
	}
}