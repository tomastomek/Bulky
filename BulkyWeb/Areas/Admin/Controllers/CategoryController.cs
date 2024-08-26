using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            //// Custom validation
            //if (category.Name == category.DisplayOrder.ToString())
            //{
            //    // first parameter is key, second is message
            //    ModelState.AddModelError("name", "The DisplayOrder cannot match the name.");
            //}

            // check model if all requirements are satisfied
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // find works on primary keys
            Category? catFromDb = _unitOfWork.Category.Get(x => x.Id == id);

            // Find out if there's record matching the condition in bracket otherwise
            // return default value - here null
            //Category? catFromDb2 = _db.Categories.FirstOrDefault(x => x.Id == id);

            //Category? catFromDb3 = _db.Categories.Where(x => x.Id == id).FirstOrDefault();

            if (catFromDb == null)
            {
                return NotFound();
            }

            return View(catFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {

            // check model if all requirements are satisfied
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // find works on primary keys
            Category? catFromDb = _unitOfWork.Category.Get(x => x.Id == id);

            if (catFromDb == null)
            {
                return NotFound();
            }

            return View(catFromDb);
        }

        // We say if we call Delete action, this method should be invoked
        // we rename method name, because parameters are same for GET and POST method
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? catToDelete = _unitOfWork.Category.Get(x => x.Id == id);
            if (catToDelete == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(catToDelete);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
