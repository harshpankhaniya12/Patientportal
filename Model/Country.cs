namespace Patientportal.Model
{
    public class Country
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
     
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
