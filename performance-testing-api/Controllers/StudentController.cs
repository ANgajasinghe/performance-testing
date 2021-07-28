using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using performance_testing_api.Data;
using performance_testing_api.Domain;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace performance_testing_api.Controllers
{
    [ApiController]
    [Route("api/student")]
    public class StudentController : ControllerBase
    {
        private readonly IMapper mapper;

        public StudentController(AppDbContext appDbContext, IMapper mapper)
        {
            AppDbContext = appDbContext;
            this.mapper = mapper;
        }

        public AppDbContext AppDbContext { get; }



        // //[HttpGet]
        // [HttpGet]
        // [EnableQuery(PageSize = 20)]
        // public IActionResult Get()
        // {
        //     return Ok(AppDbContext.Students.Cacheable());
        // }
        //
        // [HttpGet("{key}")]
        // [EnableQuery]
        // public IActionResult Get(int key)
        // {
        //     return Ok(AppDbContext.Students.FirstOrDefault(c => c.Id == key));
        //
        // }



        [HttpGet]
        [EnableQuery(PageSize = 20)]
        public IActionResult Get(CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            var res = AppDbContext.Students.AsQueryable();

            // var res = await AppDbContext.Students
            //     .OrderBy(x => x.Id)
            //     .Take(20)
            //     .Cacheable()
            //     .ToListAsync(cancellationToken);

            // var res = await AppDbContext.Students
            //     .Cacheable()
            //     .ToListAsync();

            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
            return Ok(res);


        }

        //
        //  [HttpGet("{id}")]
        //  public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        //  {
        //      Stopwatch stopwatch = new Stopwatch();
        //      stopwatch.Start();
        //
        //
        //      var res = await AppDbContext.Students.Cacheable().FirstOrDefaultAsync(x => x.Id == id);
        //
        //      Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
        //      return Ok(res);
        //
        //
        //  }

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
