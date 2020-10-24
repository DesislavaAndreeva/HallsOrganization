using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EfDbHallsOrg
{
    enum TeacherFields
    {
        NAME = 0,
        DEGREE_TITLE = 1,
        FACULTY = 2,
        DEPARTMENT = 3,
        EMAIL = 4,
        EGN = 5
    }

    [Table("Teacher")]
    public class Teacher
    {
        public Teacher() { }

        [Key]
        public int TeacherId { get; set; }
        public string Egn { get; set; }
        public string Full_Name { get; set; }
        public string Faculty { get; set; }
        public string Degree_Title { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }

        public static int FindTeacherIdByEGN(string egn)
        {
            using (var db = new DbHallContext())
            {
                var teacher = (from d in db.Teachers where d.Egn == egn select d.TeacherId).Single();
                return teacher;
            }
        }

        public static bool CheckIfTeacherExists(string egn)
        {
            using (var db = new DbHallContext())
            {
                var teacher = (from d in db.Teachers where d.Egn == egn select d).SingleOrDefault();

                if (teacher != null)
                    return true;
            }

            return false;
        }

        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static void ChangeTeacherInfoByEgn(string egn, int field, string new_data)
        {
            using (var context = new DbHallContext())
            {
                var teacher = (from d in context.Teachers where d.Egn == egn select d).SingleOrDefault();

                if (teacher != null)
                {
                    switch (field)
                    {
                        case (int)TeacherFields.NAME:
                            teacher.Full_Name = new_data;
                            break;

                        case (int)TeacherFields.DEGREE_TITLE:
                            teacher.Degree_Title = new_data;
                            break;

                        case (int)TeacherFields.DEPARTMENT:
                            teacher.Department = new_data;
                            break;

                        case (int)TeacherFields.FACULTY:
                            teacher.Faculty = new_data;
                            break;

                        case (int)TeacherFields.EMAIL:
                            teacher.Email = new_data;
                            break;

                        /*case (int)TeacherFields.EGN:
                            teacher.Egn = new_data;
                            break;*/
                  
                    }

                    context.SaveChanges();
                }
            }
        }

    }
}
