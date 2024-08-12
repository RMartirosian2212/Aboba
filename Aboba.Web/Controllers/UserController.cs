using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Aboba.Controllers;

[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<IActionResult> Index()
    {
        var users = await _userManager.GetUsersInRoleAsync("User");
        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return PartialView("_DeleteUserPartial", user);
    }
    
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeletePost(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        
        await _userManager.DeleteAsync(user);

        return RedirectToAction(nameof(Index));
    }
}