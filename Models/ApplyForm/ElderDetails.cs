﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WeiXinOA.Models.ApplyForm
{
    public class ElderDetails
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
        [Display(Name = "*身份证")]
        [MaxLength(18)]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*婚姻状况")]
        [MaxLength(5)]
        public string Marital { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*学历")]
        [MaxLength(5)]
        public string Education { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*手机")]
        [MaxLength(15)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*民族")]
        [MaxLength(5)]
        public string MinZu { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*籍贯")]
        [MaxLength(10)]
        public string JiGuan { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*户籍地址")]
        [MaxLength(40)]
        public string HuJiAddress { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*常住地址")]
        [MaxLength(40)]
        public string HomeAddress { get; set; }

        [Display(Name = "工作单位")]
        [MaxLength(10)]
        public string WorkUnit { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*收入")]
        [MaxLength(6)]
        public string InCome { get; set; }

        [Display(Name = "退休时间")]
        [MaxLength(11)]
        public string RetirementDate { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*健康状况")]
        [MaxLength(10)]
        public string HealthState { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*精神状况")]
        [MaxLength(10)]
        public string MentalState { get; set; }

        [Display(Name = "常备药物")]
        [MaxLength(20)]
        public string ChangBeiYaoWu { get; set; }

        [Display(Name = "爱好特长")]
        [MaxLength(10)]
        public string AiHao { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*饮食习惯")]
        [MaxLength(10)]
        public string DietaryHabit { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*信仰")]
        [MaxLength(5)]
        public string Faith { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "*信仰时长")]
        [MaxLength(4)]
        public string FaithTime { get; set; }

        [Display(Name = "修学法门")]
        [MaxLength(10)]
        public string XiuXingFaMen { get; set; }

        [Display(Name = "功课")]
        [MaxLength(10)]
        public string HomeWork { get; set; }

        [Display(Name = "备注")]
        [MaxLength(50)]
        public string Remark { get; set; }

        [Display(Name = "填表时间")]
        public string AddTime { get; set; }

        public string IdPhotoPath { get; set; }

        #region NotMapped

        [NotMapped]
        [Display(Name = "*出生日期")]
        public string BirthDate
        {
            get
            {
                try
                {
                    var birth = IdNumber.Substring(6, 8);
                    return $"{birth.Substring(0, 4)}-{birth.Substring(4, 2)}-{birth.Substring(6, 2)}";
                }
                catch (Exception)
                {
                    return "错误身份证";
                }
            }
        }

        [NotMapped]
        [Display(Name = "*年龄")]
        public string Age
        {
            get
            {
                try
                {
                    var birthdate = DateTime.Parse(BirthDate);

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