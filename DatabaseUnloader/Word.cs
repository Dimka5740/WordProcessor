using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLoader
{
    class Word
    {
        [Key]
        [MaxLength(20)]
        public string WordText { get; set; }
        public int Count { get; set; }
    }
}
