using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalWebApplication.Models
{
    public class patient
    {
        [Key]
        public int BedID { get; set; }
        public int BedNo { get; set; }
        public string? Status { get; set; }
        public string? URN { get; set; }
        public int PatientID { get; set; }
        public string? PatientName { get; set; }
        public string? presentingIssue { get; set; }
        public string? LastComment { get; set; }        
        public DateTime LastUpdate { get; set; }
       

        /* public patient(int id, int uRN, string firstName, string lastName, DateOnly dOB, int bedNo, DateTime admittedTime, bool isDischarged, DateTime? dischargedTime, string presentedIssues)
         {
             patientID = id;
             URN = uRN;
             this.FirstName = firstName;
             this.LastName = lastName;
             DOB = dOB;
             this.bedID = bedNo;
             this.admittedTime = admittedTime;
             this.isDischarged = isDischarged;
             this.dischargedTime = dischargedTime;
             this.presentingIssue = presentedIssues;
         }

         public patient()
         {
         }*/
    }
}
