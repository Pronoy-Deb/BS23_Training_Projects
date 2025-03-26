using Microsoft.AspNetCore.Mvc;
using OA.Data.Model;
using OA.Repo.Repository;
using OA.Service.Service;

namespace OA.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IStudentRepository _studentRepository;

        public CoursesController(IRepository<Course> courseRepository,
            IEnrollmentRepository enrollmentRepository,
            IStudentRepository studentRepository)
        {
            _courseRepository = courseRepository;
            _enrollmentRepository = enrollmentRepository;
            _studentRepository = studentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _courseRepository.GetAllAsync();

            return View(courses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            if (!ModelState.IsValid)
                return View(course);

            await _courseRepository.AddAsync(course);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);

            if (course == null)
                return NotFound($"Course with {id} not found");

            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Course course)
        {
            if (!ModelState.IsValid)
                return View(course);

            await _courseRepository.UpdateAsync(course);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Enrollments(int id)
        {
            var enrollments = await _enrollmentRepository.GetCourseEnrollmentsAsync(id);

            ViewBag.CourseId = id;
            ViewBag.Students = await _studentRepository.GetAllAsync();

            return View(enrollments);
        }

        [HttpPost]
        public async Task<IActionResult> AddEnrollment(Enrollment enrollment)
        {
            try
            {
                await _enrollmentRepository.AddAsync(enrollment);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(Enrollments), new { id = enrollment.CourseId });
        }
    }
}
