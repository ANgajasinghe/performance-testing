using AutoMapper;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using performance_testing_api.Data;
using performance_testing_api.Domain;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace performance_testing_api.Controllers
{
    [ApiController]
    [Route("api/student")]
    public class StudentController : Controller
    {
        private readonly IMapper mapper;

        public StudentController(AppDbContext appDbContext, IMapper mapper)
        {
            AppDbContext = appDbContext;
            this.mapper = mapper;
        }

        public AppDbContext AppDbContext { get; }


        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            var res = await AppDbContext.Students
                .OrderBy(x => x.Id)
                .Take(9999)
                .Cacheable()
                .ToListAsync(cancellationToken);

            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
            return Ok(res);


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            var res = await AppDbContext.Students.Cacheable().FirstOrDefaultAsync(x => x.Id == id);

            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
            return Ok(res);


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            AppDbContext.Students.Remove(new Student { Id = id });
            await AppDbContext.SaveChangesAsync();

            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
            return Ok(new { Messsage = "Student Deleted" });


        }

        [HttpPut]
        public async Task<IActionResult> Put(Student student, CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            // var res = await AppDbContext.Students.FirstOrDefaultAsync(x => x.Id == student.Id);

            AppDbContext.Students.Update(student);
            await AppDbContext.SaveChangesAsync();

            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
            return Ok(student);


        }




    }



}
