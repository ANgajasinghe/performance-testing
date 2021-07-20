using System;
using System.ComponentModel.DataAnnotations;

namespace performance_testing_api.Domain
{
    public class Teacher
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string FirstName { get; set; }

        [MaxLength(200)]
        public string LastName { get; set; }

        public DateTime Created { get; set; }

    }
}
