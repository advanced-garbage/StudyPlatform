﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPlatform.Data.Models
{
    public class Teacher
    {
        [Key]
        //[Required]
        public Guid Id { get; set; }
    }
}
