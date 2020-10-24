using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfDbHallsOrg
{
    public enum HallFields
    {
        CAPACITY = 0,
        INTERNET = 1,
        PROJECTOR = 2,
        COMPUTERS = 3,
        BOARD_COLOR = 4,
        HALL_NAME = 5
    }

    [Table("Hall")]
    public class Hall
    {
        public Hall() { }

        [Key]
        public int HallId { get; set; }
        public string Hall_Name { get; set; }
        public int Capacity { get; set; }
        public bool Internet { get; set; }
        public bool Projector { get; set; }
        public int Comp_Count { get; set; }
        public string Board_Color { get; set; }

        public static bool CheckIfHallNameExists(string name)
        {
            using (var db = new DbHallContext())
            {
                var hall = (from d in db.Halls where d.Hall_Name == name select d).SingleOrDefault();

                if (hall != null)
                    return true;
            }

            return false;
        }

        public static void ChangeHallByName(string name, int field, string new_stdata, bool new_bdata, int new_idata)
        {
            using (var context = new DbHallContext())
            {
                var hall = (from d in context.Halls where d.Hall_Name == name select d).SingleOrDefault();

                if (hall != null)
                {
                    switch (field)
                    {
                        /*case (int)HallFields.HALL_NAME:
                            var hall_name = (from d in context.Halls where d.Hall_Name == new_stdata select d).SingleOrDefault();

                            if(hall_name == null)
                                hall.Hall_Name = new_stdata;
                            break;*/

                        case (int)HallFields.CAPACITY:
                            hall.Capacity = new_idata;
                            break;

                        case (int)HallFields.INTERNET:
                            hall.Internet = new_bdata;
                            break;

                        case (int)HallFields.PROJECTOR:
                            hall.Projector = new_bdata;
                            break;

                        case (int)HallFields.COMPUTERS:
                            hall.Comp_Count = new_idata;
                            break;

                        case (int)HallFields.BOARD_COLOR:
                            hall.Board_Color = new_stdata;
                            break;
                    }
                    context.SaveChanges();
                }  
            }
        }
    }
}
