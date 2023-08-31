using System.ComponentModel.DataAnnotations;

namespace tjdaca.Models
{
    public class StuStatistics
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

        [Display(Name = "1학기_중간")]
        public float? Score1_1 { get; set; }

        [Display(Name = "1학기_기말")]
        public float? Score1_2 { get; set; }
        [Display(Name = "1학기_종합")]
        public float? Score1_3 { get; set; }
        [Display(Name = "2학기_중간")]
        public float? Score2_1 { get; set; }

        [Display(Name = "2학기_기말")]
        public float? Score2_2 { get; set; }
        [Display(Name = "2학기_종합")]
        public float? Score2_3 { get; set; }

        [Display(Name = "수업일수기준 10건")]
        public decimal? DailyScore10 { get; set; }

        [Display(Name = "수업일수기준 20건")]
        public decimal? DailyScore20 { get; set; }
    }
}
