namespace TestHospitalAPI.Models
{
    public class patient
    {
        public int Id { get; set; }
        public string? firstName{get;set;}
        public string? lastName { get; set; }
        public DateTime DOB { get; set; }

        public int bedNo { get; set; }
        public bool isDischarged { get; set; }
        public DateTime admittedDate { get; set; }
    }
}
