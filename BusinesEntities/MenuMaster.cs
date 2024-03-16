using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public class MenuMaster
    {
        [Key]
        public int MenuIdentity { get; set; }
        public string MenuID { get; set; }
        public string PageName { get; set; }
        public string Parent_MenuID { get; set; }
        public string User_Roll { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string USE_YN { get; set; }
        public bool IsActive { get; set; }
        public string icion { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
