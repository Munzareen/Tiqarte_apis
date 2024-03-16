using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinesEntities
{
    public partial class Organizer
    {
        [Key]
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public string ImageURL { get; set; }
        public int Followers { get; set; }
        [NotMapped]
        public string[] Collections { get; set; }
    }

    public partial class OrganizerCollections
    {
        [Key]
        public int Id { get; set; }
        public int OrganizerId { get; set; }
        public string ImageURL { get; set; }
    }

    public partial class CustomerOrganizerFollow
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrganizerId { get; set; }
    }
}
