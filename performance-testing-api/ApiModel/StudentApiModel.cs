using System.ComponentModel.DataAnnotations;

namespace performance_testing_api.ApiModel
{
    public class StudentApiModel
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string FirstName { get; set; }
    }
}
