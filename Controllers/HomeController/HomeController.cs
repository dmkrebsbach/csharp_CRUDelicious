using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CRUDelicious.Models;
using System.Linq;
using Microsoft.AspNetCore.Http; // FOR USE OF SESSIONS
using Microsoft.AspNetCore.Mvc;

// Other using statements
namespace CRUDelicious.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
    
        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
    
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            List<Dish> AllDishes = dbContext.Dishes.OrderByDescending(d => d.CreatedAt).ToList();  //a list of all dishes that will be added to the dbContext list w/ property of Dishes
            
            return View("Index", AllDishes);
        }

        [HttpGet("New")]
        public IActionResult NewDish(){
            return View("New");
        }

        [HttpPost("createdish")]
        public IActionResult CreateDish(Dish newDish)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(newDish);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View("NewDish");
            }
        }

        [HttpGet("{id}")]
        public IActionResult DisplayDish(int id){
            Dish dish = dbContext.Dishes.FirstOrDefault(d => d.DishId == id);
            return View("Dish", dish);
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id){
            Dish dish = dbContext.Dishes.FirstOrDefault(d => d.DishId == id);
            return View("Edit", dish);
        }

        [HttpPost("EditDish")]
        public IActionResult UpdateDish(Dish updateDish)
        {
            if(ModelState.IsValid)
            {
                Dish dish = dbContext.Dishes.FirstOrDefault(d => d.DishId == updateDish.DishId);
                dish.Chef = updateDish.Chef;
                dish.Name = updateDish.Name;
                dish.Calories = updateDish.Calories;
                dish.Description = updateDish.Description;
                dish.Tastiness = updateDish.Tastiness;
                dish.UpdatedAt = DateTime.Now;
                dbContext.SaveChanges();
                return RedirectToAction("DisplayDish", new {id = dish.DishId});
            }
            else
            {
                return View("Edit");
            }
        }

        [HttpGet("destroy/{id}")]
        public IActionResult Destroy(int id){
            Dish dish = dbContext.Dishes.FirstOrDefault(d => d.DishId == id);
            dbContext.Dishes.Remove(dish);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}