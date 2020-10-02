using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeiXinOA.Models.ApplyForm
{
    public class VolunteerFamily
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*姓名")]
        [MaxLength(5)]
        public string Name { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*性别")]
        [MaxLength(2)]
        public string Sex { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*年龄")]
        [MaxLength(2)]
        public string Age { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*关系")]
        [MaxLength(5)]
        public string Relationship { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*手机")]
        [MaxLength(15)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*常住地址")]
        [MaxLength(40)]
        public string HomeAddress { get; set; }

        //义工身份证Id
        public string VolunteerIdNumber { get; set; }
    }
}
