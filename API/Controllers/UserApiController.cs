using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repositories.Interfaces;
using Repositories.models;

namespace API.Controllers
{
    // [Route("/UserApi/")]
    // [Route("api/[controller]")]
    // [ApiController]
    public class UserApiController : Controller
    {
        private readonly IUserInterface _user;
        private readonly IAssetInterface _asset;
        private readonly ILogger<UserApiController> _logger;

        public UserApiController(IUserInterface userInterface,IAssetInterface assetInterface)
        {
            _user = userInterface;
            _asset = assetInterface;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] t_User user)
        {
            if (user.ProfilePicture != null && user.ProfilePicture.Length > 0)
            {
                var fileName = user.c_Email + Path.GetExtension(user.ProfilePicture.FileName);
                var filePath = Path.Combine("../MVC/wwwroot/profile_images", fileName);
                user.c_Image = fileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    user.ProfilePicture.CopyTo(stream);
                }
            }
            Console.WriteLine("Filename : " + user.c_UserName);
            var status = await _user.Register(user);
            if (status == 1)
            {
                return Ok(new { success = true, message = "User Registered" });
                // return Redirect("http://localhost:5003/");
            }
            else if (status == 0)
            {
                return Ok(new { success = false, message = "User Already Exist" });
            }
            else
            {
                return BadRequest(new
                {
                    success = false,
                    message = "There was some error while Registration"
                });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromForm] Login user)
        {
            var rooms = await _asset.GetALLRomms();
            ViewBag.rooms = rooms;

            var cupboards = await _asset.GetALLCupboards();
            ViewBag.cupboards = cupboards;
            
            t_User UserData = await _user.Login(user);
            if (UserData.c_UserId != 0)
            {
                return Ok(new
                {
                    success = true,
                    message = "Login Success",
                    UserData = UserData
                });
            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "Invalid email or password",
                    UserData = UserData
                });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
        

        // public IActionResult Logout()
        // {
        //     Console.WriteLine("Logout");
        //     return View();
        // }
    }
}