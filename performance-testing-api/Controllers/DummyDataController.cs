using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using performance_testing_api.Data;
using performance_testing_api.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace performance_testing_api.Controllers
{

    [Route("api/dummy-data")]
    public class DummyDataController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly Test test;

        public DummyDataController(AppDbContext appDbContext, Test test)
        {
            this._appDbContext = appDbContext;
            this.test = test;
        }


        [HttpPost("create-2309809-students")]
        public async Task<IActionResult> Create50000000Students()
        {

            Thread thread = new Thread(test.StudentAddThread);
            thread.Start();
            return Ok();

        }





        [HttpPost("create-2309809-school")]
        public async Task<IActionResult> Create50000000School()
        {
            Thread thread = new Thread(test.SchoolAddThread);
            thread.Start();
            return Ok();
        }

        [HttpPost("create-2309809-teacher")]
        public async Task<IActionResult> Create50000000Teacher()
        {
            Thread thread = new Thread(test.TeacherAddThread);
            thread.Start();
            return Ok();
        }

    }

    public class Test
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public Test(IServiceProvider serviceProvider, IServiceScopeFactory serviceScope)
        {
            ServiceProvider = serviceProvider;
            _scopeFactory = serviceScope;
        }

        public IServiceProvider ServiceProvider { get; }


        public void StudentAddThread()
        {
            Console.WriteLine("===== Student theread is started ====== ");

            using var scope = _scopeFactory.CreateScope();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var dbcontext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var list = new List<Student>();

            for (int i = 0; i < 2309809; i++)
            {
                list.Add(new Student()
                {

                    FirstName = $"First Name - {i}",
                    LastName = $"Last Name - {i}",
                    StudentName = $"Student Name - {i}"
                });
            }

            dbcontext.Students.AddRange(list);
            dbcontext.SaveChanges();
            Console.WriteLine("Student theread is compleated");

            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);

        }

        public void SchoolAddThread()
        {
            Console.WriteLine("===== School theread is started ====== ");

            using var scope = _scopeFactory.CreateScope();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var list = new List<School>();

            for (int i = 0; i < 2309809; i++)
            {
                list.Add(new School()
                {
                    Name = $"Shool - {i}",
                    Description = $"{Guid.NewGuid()}"
                });
            }

            using var dbcontext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbcontext.Schools.AddRange(list);
            dbcontext.SaveChanges();
            Console.WriteLine("School theread is compleated");

            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);

        }

        public void TeacherAddThread()
        {

            using (var scope = _scopeFactory.CreateScope())
            {
                Console.WriteLine(" ======= TeacherAddThread theread is stated ======== ");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();



                var list = new List<Teacher>();

                for (int i = 0; i < 2309809; i++)
                {
                    list.Add(new Teacher()
                    {
                        FirstName = $"Shool - {i}",
                        LastName = $"{Guid.NewGuid()}"
                    });
                }


                using var dbcontext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbcontext.Teachers.AddRange(list);
                dbcontext.SaveChanges();
                Console.WriteLine("TeacherAddThread theread is compleated");

                Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
            }

        }
    }
}
