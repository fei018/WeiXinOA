
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WeiXinOA.Models.ApplyForm
{
    public class ElderFamily
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
        [Display(Name = "*手机")]
        [MaxLength(15)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*关系")]
        [MaxLength(5)]
        public string Relationship { get; set; }      

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*职业")]
        [MaxLength(6)]
        public string Profession { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*学历")]
        [MaxLength(5)]
        public string Education { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*身份证")]
        [MaxLength(18)]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*常住地址")]
        [MaxLength(40)]
        public string HomeAddress { get; set; }

        //老人身份证Id
        [MaxLength(18)]
        public string ElderIdNumber { get; set; }

        #region NotMapped

        [NotMapped]
        [Display(Name ="年龄")]
        public string Age {
            get 
            {
                try
                {
                    var birth = IdNumber.Substring(6, 8);
                    var birthdate = DateTime.Parse($"{birth.Substring(0, 4)}-{birth.Substring(4, 2)}-{birth.Substring(6, 2)}");

                    DateTime now = DateTime.Now;
                    int age = now.Year - birthdate.Year;
                    if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
                    {
                        age--;
                    }
                    age = age < 0 ? 0 : age;
                    return age.ToString();
                }
                catch (Exception)
                {
                    return "0";
                }

            } 
        }
        #endregion
    }
}
