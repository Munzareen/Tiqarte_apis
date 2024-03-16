using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public class UserPermission
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Route{ get; set; }
        public bool IsActive { get; set; }
        public string Page { get; set; }
        public string Permission { get; set; }
    }

}
