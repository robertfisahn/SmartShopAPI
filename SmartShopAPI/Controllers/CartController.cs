using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShopAPI.Entities;
using SmartShopAPI.Interfaces;
using SmartShopAPI.Models.Dtos.CartItem;

namespace SmartShopAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService) {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<CartItemDto>> GetAll([FromRoute]int userId) {
           var cartItems = _cartService.GetById(userId);
            return Ok(cartItems);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult Create([FromQuery]int productId, [FromBody]CreateCartDto item)
        {
            var cartItemId = _cartService.Create(productId, item);
            return Created($"api/cart/{cartItemId}", null);
        }

        [HttpDelete("{cartItemId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult Delete([FromRoute]int cartItemId) {
            _cartService.Delete(cartItemId);
            return NoContent();
        }

        [HttpPut("{cartItemId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult Update([FromRoute]int cartItemId, CreateCartDto dto)
        {
            _cartService.Update(cartItemId, dto);
            return Ok();
        }
    }
}
