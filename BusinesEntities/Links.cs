using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public partial class Links
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string InviteFriendsLink { get; set; }

    }
}
