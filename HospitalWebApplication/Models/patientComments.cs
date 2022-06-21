using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HospitalWebApplication.Models
{
    public class patientComments
    {
        [Key]
        public int patientCommentID { get; set; }
        public string? URN { get; set; }
        public int patientID { get; set; }
        
        public string? PatientName { get; set; }
        public string? Comments { get; set; }
        public string? Nurse { get; set; }
        public DateTime updatedTime { get; set; }
    }
}
