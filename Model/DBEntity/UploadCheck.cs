﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RecruitmentPortalBE.Model.DBEntity
{
    public class UploadCheck
    {
        [NotMapped]
        public IFormFile Files { get; set; }
     
    }
}
