using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;

namespace tjdaca.Models
{
    public class StuCliniq
    {
        [Display(Name = "학생번호")]
        public int StuIdx { get; set; }

        [Display(Name = "학생이름")]
        public string StuName { get; set; } = string.Empty;


        [Display(Name = "학교")]
        public string School { get; set; } = string.Empty;

        [Display(Name = "학교구분")]
        /// <summary>
        /// 초등/중등/고등
        /// </summary>
        public string SchType { get; set; } = string.Empty;

        [Display(Name = "학년")]
        /// <summary>
        /// 학년
        /// </summary>
        public string SchGrade { get; set; } = string.Empty;

        [Display(Name = "담임명")]
        public string Teacher { get; set; } = string.Empty;


        [Display(Name = "결과값")]
        public decimal? AverageScore { get; set; }

        [Display(Name = "70점미만단원")]
        public string CliniqSubject { get; set; }
    }
}
