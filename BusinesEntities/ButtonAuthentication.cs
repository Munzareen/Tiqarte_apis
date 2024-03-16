using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    class ButtonAuthentication
    {
        [Key]
        public int ButtonId { get; set; }
        public string RoleName { get; set; }
        public string Page { get; set; }
        public string BtnName { get; set; }


    }
}
