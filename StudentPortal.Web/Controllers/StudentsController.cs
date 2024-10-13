using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models;
using StudentPortal.Web.Models.Entities;

namespace StudentPortal.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel studentModel)
        {
            var student = new Student
            {
                Name = studentModel.Name,
                Email = studentModel.Email,
                Phone = studentModel.Phone,
                Subscribed = studentModel.Subscribed,
            };

            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            return View();
        }

        [HttpGet]
        public IActionResult List()
        {
            var students = _context.Students.ToList();

            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await _context.Students.FindAsync(id);

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student student)
        {
            var stud = await _context.Students.FindAsync(student.Id);
            if (stud is not null)
            {
                stud.Name = student.Name;
                stud.Email = student.Email;
                stud.Phone = student.Phone;
                stud.Subscribed = student.Subscribed;

                _context.Update(stud);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("List", "Students");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Student student)
        {
            var stud = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == student.Id);

            if (stud is not null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("List", "Students");
        }
    }
}
