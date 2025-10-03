using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vorder.Application.DTOs.Shop;
using Vorder.Application.Interfaces.Repositories;
using Vorder.Application.Mapper;
using Vorder.Application.ResultPattern;
using Vorder.Domain.Models;
using Vorder.Infrastructure.Data;

namespace Vorder.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class ShopController(IShopRepository shopRepository, UserManager<ApplicationUser> _userManager) : ControllerBase
    {

        [HttpGet(Name = "GetPagenatedShops")]
        public async Task<ApplicationResult<List<ReturnShopDTO>>> GetPagenatedShops(Pagination pagination)
        {
            var shops = await shopRepository.GetPagedAsync(pagination.PageNumber, pagination.PageSize);
            List<ReturnShopDTO> shopsList = new List<ReturnShopDTO>();
            foreach (var shop in shops)
            {
                string username = await SubdomainHelper.GetUsernameByID(shop.OwnerId, _userManager);
                shopsList.Add(shop.ToShop(username));
            }
            return shopsList;
        }

        [HttpGet(Name = "GetShops")]
        public async Task<ApplicationResult<List<ReturnShopDTO>>> GetShops()
        {
            var shops = await shopRepository.GetAllAsync();
            List<ReturnShopDTO> shopsList = new List<ReturnShopDTO>();
            foreach (var shop in shops)
            {
                string username = await SubdomainHelper.GetUsernameByID(shop.OwnerId, _userManager);
                shopsList.Add(shop.ToShop(username));
            }
            return shopsList;
        }

        [HttpGet(Name = "GetShopByID")]
        public async Task<ApplicationResult<ReturnShopDTO>> GetShopByID(Guid shopID)
        {
            var shop = await shopRepository.GetByIdAsync(shopID);
            if (shop is null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return Errors.NotFound(ErrorConstants.SHOPNOTFOUND, ErrorConstants.SHOPNOTFOUNDCODE);
            }

            string username = await SubdomainHelper.GetUsernameByID(shop.OwnerId, _userManager);
            return shop.ToShop(username);
        }


        [HttpPost(Name = "CreateShop")]
        public async Task<ApplicationResult<ReturnShopDTO>> CreateShop(CreateShopDTO shop)
        {
            string? userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string? userName = User.FindFirstValue(ClaimTypes.Name);
            if (userIdString is null || userName is null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return Errors.NotFound(ErrorConstants.USERNOTFOUND, ErrorConstants.USERNOTFOUNDCODE);
            }

            Guid userId = Guid.Parse(userIdString);
            var shopExists = await shopRepository.GetShopByNameAsync(shop.ShopName);
            if (shopExists is not null)
                return Errors.Exists(ErrorConstants.SHOPNAMEEXISTS, ErrorConstants.SHOPNAMEEXISTSCODE);

            var shopCreated = await shopRepository.AddShopAsync(shop, userId);
            Response.StatusCode = StatusCodes.Status201Created;
            return shopCreated.ToShop(userName);
        }
    }
}
