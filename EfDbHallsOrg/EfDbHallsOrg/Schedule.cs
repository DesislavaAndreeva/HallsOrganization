using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfDbHallsOrg
{
    enum ScheduleFields
    {
        DATE = 0,
        HOUR = 1,
        TEACHER_ID = 2,
        HALL_ID = 3
    }

    [Table("Schedule")]
    public class Schedule
    {
        public Schedule() { }

        [Key]
        public int ScheduleId { get; set; }
        public DateTime Date_of_occupation { get; set; }
        public string Day_of_the_week { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Hour { get; set; }

        public int? HallId { get; set; }
        [ForeignKey("HallId")]
        public Hall Hall { get; set; }
        public int? TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; }

        private static void ChangeScheduleById(int id, int field, DateTime date, string new_data, int new_idata)
        {
            using (var context = new DbHallContext())
            {
                var schedule = (from d in context.Schedules where d.ScheduleId == id select d).SingleOrDefault();

                if (schedule != null)
                {
                    switch (field)
                    {
                        case (int)ScheduleFields.DATE:
                            schedule.Date_of_occupation = date;
                            schedule.Day_of_the_week = date.DayOfWeek.ToString();
                            schedule.Month = date.Month.ToString();
                            schedule.Year = date.Year.ToString();
                            break;

                        case (int)ScheduleFields.HOUR:
                            schedule.Hour = new_data;
                            break;

                        case (int)ScheduleFields.TEACHER_ID:
                            schedule.TeacherId = new_idata;
                            break;

                        case (int)ScheduleFields.HALL_ID:
                            schedule.HallId = new_idata;
                            break;

                    }

                    context.SaveChanges();
                }
            }
        }
    }
}
