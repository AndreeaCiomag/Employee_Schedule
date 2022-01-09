namespace Schedule_Administration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Schedule")]
    public partial class Schedule
    {
        [Key]
        public int IdSch { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public int? EmployeeID { get; set; }

        public int? TimetableID { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Timetable Timetable { get; set; }
    }
}
