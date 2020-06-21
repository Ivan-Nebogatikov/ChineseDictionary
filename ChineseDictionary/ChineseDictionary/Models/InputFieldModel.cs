using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Models
{
    public class InputFieldModel
    {
        [Required]
        [StringLength(255, ErrorMessage = "Incorrect length.", MinimumLength = 1)]
        public string SearchString { get; set; }
    }
}
