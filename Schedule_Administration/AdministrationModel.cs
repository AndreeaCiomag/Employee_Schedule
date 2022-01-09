using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Schedule_Administration
{
    public partial class AdministrationModel : DbContext
    {
        public AdministrationModel()
            : base("name=AdministrationModel")
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Timetable> Timetables { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Schedules)
                .WithOptional(e => e.Employee)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Timetable>()
                .HasMany(e => e.Schedules)
                .WithOptional(e => e.Timetable)
                .HasForeignKey(e => e.TimetableID)
                .WillCascadeOnDelete();
        }
    }
}
