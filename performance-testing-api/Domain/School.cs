using System;
using System.ComponentModel.DataAnnotations;

namespace performance_testing_api.Domain
{
    public class School
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime ModifiedTime { get; set; }

    }
}
