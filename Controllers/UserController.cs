using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Databasteknik.Models;

namespace Databasteknik.Controllers;

public class UserController : Controller
{
    private static string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");

    // Load users from JSON file
    private IList<UserModel> LoadUsers()
    {
        try
        {
            if (!System.IO.File.Exists(_filePath))
            {
                Console.WriteLine($"Warning: {_filePath} not found.");
                return new List<UserModel>();
            }

            var json = System.IO.File.ReadAllText(_filePath);
            var users = JsonSerializer.Deserialize<List<UserModel>>(json);
            return users ?? new List<UserModel>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading users: {ex.Message}");
            return new List<UserModel>();
        }
    }

    // Save users to JSON file
    private void SaveUsers(IList<UserModel> users)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(users, options);
            System.IO.File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving users: {ex.Message}");
        }
    }

    public IActionResult Index()
    {
        var userList = LoadUsers();
        
        // SESSION EXAMPLES
        var visitCount = HttpContext.Session.GetInt32("VisitCount") ?? 0;
        visitCount++;
        HttpContext.Session.SetInt32("VisitCount", visitCount);
        ViewBag.VisitCount = visitCount;
        
        var lastVisit = HttpContext.Session.GetString("LastVisit");
        HttpContext.Session.SetString("LastVisit", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        ViewBag.LastVisit = lastVisit ?? "First visit";
        
        // COOKIE EXAMPLES
        var lastViewedUserId = Request.Cookies["LastViewedUser"];
        if (!string.IsNullOrEmpty(lastViewedUserId))
        {
            var lastUser = userList.FirstOrDefault(u => u.UserId.ToString() == lastViewedUserId);
            ViewBag.LastViewedUser = lastUser?.FirstName + " " + lastUser?.LastName;
        }
        
        ViewBag.TotalUsers = userList.Count;
        ViewBag.LastUpdated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
        return View(userList);
    }

    //GET: /User/Create
    public IActionResult Create()
    {
        ViewData["PageInfo"] = "Fill out the form to create a new user";
        
        var usersCreated = HttpContext.Session.GetInt32("UsersCreated") ?? 0;
        if (usersCreated > 0)
        {
            ViewBag.CreationMessage = $"You've created {usersCreated} user(s) this session!";
        }
        
        return View();
    }

    //POST: /User/Create
    [HttpPost]
    public IActionResult Create(UserModel newUser)
    {
        try
        {
            var userList = LoadUsers().ToList();
            
            newUser.UserId = userList.Any() ? userList.Max(u => u.UserId) + 1 : 1;
            newUser.DateOfCreation = DateTime.Now;
            newUser.PasswordHash = $"hash{Guid.NewGuid().ToString().Substring(0, 8)}";
            newUser.IsActive = true;
            
            userList.Add(newUser);
            SaveUsers(userList);
            
            var usersCreated = HttpContext.Session.GetInt32("UsersCreated") ?? 0;
            usersCreated++;
            HttpContext.Session.SetInt32("UsersCreated", usersCreated);
            
            Response.Cookies.Append("LastCreatedUser", $"{newUser.FirstName} {newUser.LastName}", new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(30),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
            
            Response.Cookies.Append("LastViewedUser", newUser.UserId.ToString(), new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7)
            });
            
            TempData["SuccessMessage"] = $"User {newUser.FirstName} {newUser.LastName} created successfully!";
            
            return RedirectToAction("");
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Error: {ex.Message}";
            return View(newUser);
        }
    }
    
    //GET: /User/ViewUser/5
    public IActionResult ViewUser(int id)
    {
        var userList = LoadUsers();
        var user = userList.FirstOrDefault(u => u.UserId == id);
        
        if (user == null)
        {
            TempData["ErrorMessage"] = "User not found!";
            return RedirectToAction("");
        }
        
        // Store last viewed user in cookie
        Response.Cookies.Append("LastViewedUser", id.ToString(), new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddDays(7),
            HttpOnly = true
        });
        
        // Add to recently viewed list in session
        var recentlyViewed = HttpContext.Session.GetString("RecentlyViewed") ?? "";
        var viewedList = recentlyViewed.Split(',').Where(x => !string.IsNullOrEmpty(x)).ToList();
        
        viewedList.Remove(id.ToString());
        viewedList.Insert(0, id.ToString());
        
        if (viewedList.Count > 5)
        {
            viewedList = viewedList.Take(5).ToList();
        }
        
        HttpContext.Session.SetString("RecentlyViewed", string.Join(",", viewedList));
        
        return View(user);
    }
    
    //POST: /User/Delete/5
    [HttpPost]
    public IActionResult Delete(int id)
    {
        try
        {
            var userList = LoadUsers().ToList();
            var userToDelete = userList.FirstOrDefault(u => u.UserId == id);
            
            if (userToDelete == null)
            {
                TempData["ErrorMessage"] = "User not found!";
                return RedirectToAction("");
            }
            
            // Store user name before deleting
            var userName = $"{userToDelete.FirstName} {userToDelete.LastName}";
            
            // Remove user from list
            userList.Remove(userToDelete);
            
            // Save updated list to JSON
            SaveUsers(userList);
            
            // Clear the "LastViewedUser" cookie if it was this user
            var lastViewedUserId = Request.Cookies["LastViewedUser"];
            if (lastViewedUserId == id.ToString())
            {
                Response.Cookies.Delete("LastViewedUser");
            }
            
            // Update session - increment deleted counter
            var usersDeleted = HttpContext.Session.GetInt32("UsersDeleted") ?? 0;
            usersDeleted++;
            HttpContext.Session.SetInt32("UsersDeleted", usersDeleted);
            
            TempData["SuccessMessage"] = $"User {userName} (ID: {id}) has been deleted successfully!";
            
            return RedirectToAction("");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error deleting user: {ex.Message}";
            return RedirectToAction("");
        }
    }
    
    //GET: /User/SetPreference
    public IActionResult SetPreference(string view)
    {
        Response.Cookies.Append("PreferredView", view, new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddMonths(6),
            HttpOnly = true
        });
        
        TempData["SuccessMessage"] = $"Preference saved: {view} view";
        return RedirectToAction("");
    }
    
    //GET: /User/ClearSession
    public IActionResult ClearSession()
    {
        HttpContext.Session.Clear();
        
        TempData["SuccessMessage"] = "Session cleared successfully!";
        return RedirectToAction("");
    }
    
    //GET: /User/ClearCookies
    public IActionResult ClearCookies()
    {
        Response.Cookies.Delete("LastViewedUser");
        Response.Cookies.Delete("LastCreatedUser");
        Response.Cookies.Delete("PreferredView");
        
        TempData["SuccessMessage"] = "Cookies cleared successfully!";
        return RedirectToAction("");
    }
}