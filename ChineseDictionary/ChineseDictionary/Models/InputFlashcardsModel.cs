using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseDictionary.Models
{
    public class InputFlashcardsModel
    {
        [Required]
        [Range(1, 20, ErrorMessage = "Words count should be in [1; 20]")]
        public int WordsCount { get; set; }
    }
}
