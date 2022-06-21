using System.ComponentModel.DataAnnotations;

namespace HospitalWebApplication.Models
{
    public class patientDetails
    {
        [Key]
        public int patientid { get; set; }
        public string? URN { get; set; }
        public int bedID { get; set; }
        public string? PatientName { get; set; }
        public DateTime DOB { get; set; }
        public bool isAdmitted{ get; set; }
        public DateTime? admittedTime{ get; set; }
        public bool isDischarged{ get; set; }
        public DateTime? dischargedTime { get; set; }
        public string? presentingIssue { get; set; }
        public string? nurse { get; set; }
        public string? comments { get; set; }
    }
}
