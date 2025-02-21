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
    // [Route("[controller]")]
    public class AssetsApiController : Controller
    {
        private readonly IAssetInterface _assets;

        public AssetsApiController(IAssetInterface assets)
        {
            _assets = assets;
        }

        // [HttpGet]
        // public async Task<List<t_Assets>> GetAll(){
        //     List<t_Assets> assets_list = await _assets.GetAll();

        // }

        [HttpPost]
        [Route("AddAsset")]
        public async Task<IActionResult> AddAsset([FromForm] t_Assets asset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (asset.c_assetPicture != null && asset.c_assetPicture.Length > 0)
            {
                var filename = asset.c_assetsId + Path.GetExtension(asset.c_assetPicture.FileName);
                var filepath = Path.Combine("../MVC/wwwroot/asset_images", filename);
                asset.c_assetImage = filename;
                System.IO.File.Delete(filepath);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    asset.c_assetPicture.CopyTo(stream);
                }
            }
            var status = await _assets.Create(asset);

            if (status == 1)
            {
                return Ok(new { success = true, message = "Asset Created Successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Error in creating asset in controller" });
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromForm] t_Assets asset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (asset.c_assetPicture != null && asset.c_assetPicture.Length > 0)
            {
                var filename = asset.c_assetsId + Path.GetExtension(asset.c_assetPicture.FileName);
                var filepath = Path.Combine("../MVC/wwwroot/asset_images", filename);
                asset.c_assetImage = filename;
                System.IO.File.Delete(filepath);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    asset.c_assetPicture.CopyTo(stream);
                }
            }
            var status = await _assets.Update(asset);

            if (status == 1)
            {
                return Ok(new { success = true, message = "Asset Updated Successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Error in Updating in controller" });
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id){
            int status = await _assets.Delete(id);
            if(status == 1){
                return Ok(new {success = true , message= "Asset removed successfully"});
            }else{
                return BadRequest(new{success = false, message="Error While Removing the asset"});
            }
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetAssetById(string id){
            t_Assets status = await _assets.GetAssetsById(id);

            if(status != null){
                return Ok(new {success = true, message = "Asset is Read from the database", data = status});
            }else{
                return BadRequest(new {success = false, message= "Erro in getiing specific asset in controller"});
            }
        }


        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllAsset(){
            // string user_id = "1";
            string user_id = HttpContext.Request.Query["userid"].ToString();
            List<t_Assets> asset_list = null;

            if(user_id != ""){
                asset_list = await _assets.GetAssetsByUser(user_id);
            }else{
                asset_list = await _assets.GetAll();
            }
            return Ok(new {success = true, message = "asset retrieve from database",data = asset_list });
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}