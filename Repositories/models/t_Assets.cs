using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Repositories.models
{
    public class t_Assets
    {
        public int c_assetsId { get; set; }

        public string c_assetsName { get; set; }

        public string c_Description { get; set; }

        public string c_assetImage { get; set; }

        public IFormFile c_assetPicture { get; set; }

        public int c_userID { get; set; }

        public int c_cupboardID { get; set; }

        public string c_tags { get; set; }

    }
}