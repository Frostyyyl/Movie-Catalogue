using GrobelnyKasprzak.MovieCatalogue.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrobelnyKasprzak.MovieCatalogue.MVC.Controllers
{
    public class DirectorsController : Controller
    {
        private readonly ILogger<DirectorsController> _logger;
        private readonly DirectorService _service = new();

        public DirectorsController(ILogger<DirectorsController> logger)
        {
            _logger = logger;
        }

        // GET: DirectorController
        public ActionResult Index()
        {
            return View();
        }

        // GET: DirectorController/Details/5
        public ActionResult Details(int id)
        {
            return View(_service.GetDirectorById(id));
        }

        // GET: DirectorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DirectorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DirectorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DirectorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DirectorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DirectorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
