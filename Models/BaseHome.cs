using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentValidation.Models
{
    public class BaseHome
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsConfirmed { get; set; }
        public BaseHome()
        {
            IsConfirmed = true;
            CreatedDate = DateTime.Now.ToLocalTime();
        }
    }
}
