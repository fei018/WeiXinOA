using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeiXinOA.Models.Account
{
    public class LoginUser
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="用户名不能为空")]
        [MaxLength(10)]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [MaxLength(10)]      
        public string Password { get; set; }

        [MaxLength(20)]
        public string RoleName { get; set; }
    }
}
