using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vorder.Application.DTOs.Shop;
using Vorder.Application.Interfaces.Repositories;
using Vorder.Application.Mapper;
using Vorder.Infrastructure.Data;

namespace Vorder.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class ShopController(IShopRepository shopRepository, UserManager<ApplicationUser> _userManager) : ControllerBase
    {

        [HttpGet(Name = "GetShops")]
        public async Task<ActionResult> GetShops()
        {
            var shops = await shopRepository.GetAllAsync();
            List<ReturnShopDTO> shopsList = new List<ReturnShopDTO>();
            foreach (var shop in shops)
            {
                string username = await SubdomainHelper.GetUsernameByID(shop.OwnerId, _userManager);
                shopsList.Add(shop.ToShop(username));
            }
            return Ok(shopsList);
        }

        [HttpGet(Name = "GetShopByID")]
        public async Task<ActionResult> GetShopByID(Guid shopID)
        {
            var shop = await shopRepository.GetByIdAsync(shopID);
            if (shop is null)
                return NotFound();


            string username = await SubdomainHelper.GetUsernameByID(shop.OwnerId, _userManager);
            return Ok(shop.ToShop(username));
        }


        [HttpPost(Name = "CreateShop")]
        public async Task<ActionResult> CreateShop(CreateShopDTO shop)
        {
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userName = User.FindFirstValue(ClaimTypes.Name);
            Guid userId = Guid.Parse(userIdString);

            var shopCreated = await shopRepository.AddShopAsync(shop, userId);

            return CreatedAtAction(nameof(CreateShop), shopCreated.ToShop(userName));
        }
    }
}
