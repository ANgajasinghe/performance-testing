using AutoMapper;
using performance_testing_api.ApiModel;
using performance_testing_api.Domain;

namespace performance_testing_api
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Student, StudentApiModel>();
        }
    }
}
