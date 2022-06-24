using System;

namespace DbReflector.Test.Entities
{
    public class Claim
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
