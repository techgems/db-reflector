using System;

namespace DbReflector.Test.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PreferredName { get; set; }

        public string SecurityStamp { get; set; }

        public DateTime? LastLogin { get; set; }

        public DateTime CreateDate { get; set; }

        public Guid? InviteGuid { get; set; }

        public int RoleId { get; set; }
    }
}
