using static Patientportal.Model.Holidays;

namespace Patientportal.Model
{
    public class Holidays
    {
            public long? Id {  get; set; }
            public DateTimeOffset? StartDate { get; set; }
            public DateTimeOffset? EndDate { get; set; }
            public string? Description { get; set; }
            public long? Year { get; set; }
    }
}
