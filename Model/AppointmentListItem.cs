using System.ComponentModel.DataAnnotations.Schema;

namespace Patientportal.Model
{
    public class AppointmentListItem
    {
        //public string? Location { get; set; }
        public int? DoctoreId { get; set; }
        public int? SourceId { get; set; }
        public DateTime? AppointmentStartTime { get; set; }
        public string? FormofAppointment { get; set; }
        public DateTime? AppointmentEndDateTime { get; set; }

        public int? StatusId { get; set; }
        public string? AppoinmentType { get; set; }
        //[NotMapped]
        public long? LeadId { get; set; }
        //[NotMapped]
        //public string? Name { get; set; }
        //[NotMapped]
        public string? Mobile { get; set; }
        public int? Age { get; set; }
        public string? Location { get; set; }
        //[NotMapped]
        public string? Gender { get; set; }

        //[NotMapped]
        public string? Email { get; set; }
        //[NotMapped]
        public long? PatientId { get; set; }
        public string? PatientName { get; set; }

        public string? ProfileImage { get; set; }

        //[NotMapped]
        public string? AppointmentNo { get; set; }

        public short? Year { get; set; }
        public string? StatusName { get; set; }

        public string? AppointmentForm { get; set; }
        public string? Comment { get; set; }
        public string? SourceName { get; set; }
        public string? DoctorName { get; set; }


        //[NotMapped]
        public string? ConcernGroups { get; set; }
        [NotMapped]
        public List<string>? NotAllowedAction { get; set; }
        public long Id { get; set; }
        public long? CreatedBy { get; set; }
        //public string? CreatedByUserName { get; set; }
        public long? ModifiedBy { get; set; }
        //public string? ModifiedByUserName { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
    }
}
