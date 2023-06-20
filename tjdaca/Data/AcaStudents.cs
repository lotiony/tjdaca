﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using tjdaca.Class;

namespace tjdaca.Data
{
    public partial class AcaStudents
    {
        [Display(Name = "학생번호")]
        public int StuIdx { get; set; }

        [Display(Name = "학생이름")]
        [Required(ErrorMessage="학생이름은 필수입니다."), MinLength(2)]
        public string StuName { get; set; } = string.Empty;

        [Display(Name = "학생 연락처")]
        [Required(ErrorMessage = "학생 연락처는 필수입니다."), MinLength(10)]
        public string StudentPhone { get; set; } = string.Empty;

        [Display(Name = "학부모이름")]
        [Required(ErrorMessage = "학부모 이름은 필수입니다."), MinLength(2)]
        public string Parent { get; set; } = string.Empty;
        
        [Display(Name = "학부모 연락처")]
        [Required(ErrorMessage = "학부모 연락처는 필수입니다."), MinLength(10)]
        public string ParentPhone { get; set; } = string.Empty;

        [Display(Name = "우편번호")]
        public string Zipcode { get; set; }
        [Display(Name = "도로명주소")]
        public string Address1 { get; set; }
        [Display(Name = "상세주소")]
        public string Address2 { get; set; }
        [Display(Name = "학교")]
        public string School { get; set; }

        [Display(Name = "학교구분")]
        /// <summary>
        /// 초등/중등/고등
        /// </summary>
        public string SchType { get; set; }

        [Display(Name = "학년")]
        /// <summary>
        /// 학년
        /// </summary>
        public string SchGrade { get; set; }

        [Display(Name = "희망고등학교")]
        public string HopeHighschool { get; set; }

        [Display(Name = "희망대학교")]
        public string HopeUniversity { get; set; }

        [Display(Name = "수강희망과목")]
        public string Subject { get; set; }

        [Display(Name = "과정선택")]
        public string SubjectOption { get; set; }

        [Display(Name = "현재수학진도현황")]
        public string MathProgress { get; set; }

        [Display(Name = "학원인지경로")]
        [Required(ErrorMessage = "학원 인지경로는 필수입니다."), MinLength(2)]
        public string CognitivePathway { get; set; }

        [IgnoreExcelAttribute]
        public string VisitPurpose { get; set; }

        [Required(ErrorMessage = "개인정보 동의에 체크해 주세요."), IgnoreExcelAttribute]
        public bool? Agree { get; set; }

        [Display(Name = "학원등록일")]
        public DateTime? AcaRegdate { get; set; }

        [Display(Name = "학원퇴원일")]
        public DateTime? AcaOutdate { get; set; }

        [Display(Name = "담당선생님")]
        public string Teacher { get; set; }

        [Display(Name = "테스트날짜")]
        public DateTime? TestDate { get; set; }

        [Display(Name = "테스트범위")]
        public string TestArea { get; set; }

        [Display(Name = "테스트점수")]
        public string TestScore { get; set; }

        [Display(Name = "테스트결과")]
        public string TestResult { get; set; }

        [IgnoreExcelAttribute]
        public string Class { get; set; }
        [IgnoreExcelAttribute]
        public string Etc1 { get; set; }
        [IgnoreExcelAttribute]
        public string Etc2 { get; set; }
        [IgnoreExcelAttribute]
        public string Etc3 { get; set; }
        [IgnoreExcelAttribute]
        public string Etc4 { get; set; }
        [IgnoreExcelAttribute]
        public string Etc5 { get; set; }
        [IgnoreExcelAttribute]
        public string Etc6 { get; set; }
        [IgnoreExcelAttribute]
        public string Etc7 { get; set; }
        [IgnoreExcelAttribute]
        public string Etc8 { get; set; }
        [IgnoreExcelAttribute]
        public string Etc9 { get; set; }

        [Display(Name = "등록일")]
        public DateTime Regdate { get; set; }

        [IgnoreExcelAttribute]
        public DateTime Lastdate { get; set; } = DateTime.Now;
    }
}