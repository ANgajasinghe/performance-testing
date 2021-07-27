using System;
using System.ComponentModel.DataAnnotations;

namespace performance_testing_api.Domain
{
    public class Student
    {
        public Student()
        {
            StudentId = Guid.NewGuid().ToString();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public string FirstName { get; set; }

        [MaxLength(200)]
        public string LastName { get; set; }

        [MaxLength(80)]
        public string StudentId { get; set; }

        [MaxLength(200)]
        public string StudentName { get; set; }

        // public  Type { get; set; }


    }
}
