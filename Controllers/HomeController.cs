using System;
using System.Reflection.Metadata;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using hospital.Data;
using hospital.Models;
using hospital.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using hospital.Entity;
using Microsoft.EntityFrameworkCore;

namespace Queue.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly RowDbContext _dbcontext;
    private readonly UserManager<QueueUser> _userManager;
    private readonly SignInManager<QueueUser> _signinManager;

    public HomeController(ILogger<HomeController> logger, RowDbContext context, UserManager<QueueUser> userM, SignInManager<QueueUser> siginInM)
    {
        _logger = logger;
        _dbcontext=context;
        _userManager = userM;
        _signinManager = siginInM;
    }

    public IActionResult Index()
    {
        var rows = _dbcontext.rows.ToList();
        return View(rows);
    }

    [Authorize(Roles = "superadmin")]
    [HttpGet("[action]")]
    public IActionResult AdminPage()
    {
        var queues =_dbcontext.rows.Where(q => q.IsActive == true).ToList();

        return View(queues);
    }

    // [Authorize(Roles = "superadmin")]   
    // [HttpGet("Home/{id}")]
    // public IActionResult UserInactive(Guid id)
    // {
    //     var user = _dbcontext.rows.FirstOrDefault(u => u.Id == id);
    //     if(user is not null)
    //     {
    //     user.IsActive = false;
    //     _dbcontext.rows.Update(user);
    //     _dbcontext.SaveChanges();

    //     }
    //     return View(user);

    // }

    // [HttpPost]
    // [Authorize(Roles = "superadmin")]
    // public IActionResult AdminPage(Guid id)
    // {
    //     var queues=_dbcontext.rows.FirstOrDefault (p=> p.Id == id);
    //     _dbcontext.rows.Remove(queues);
    //     _dbcontext.SaveChanges();
    //     return RedirectToAction("AdminPage");
    // }

    [HttpGet]
    public IActionResult TakeRow()
    {
        return View();
    }
    [HttpPost]
    public IActionResult TakeRow([FromForm] RowViewModel model)
    {
        var user = new RowViewModel();

        user.Id = model.Id;
        user.Fullname = model.Fullname;
        user.CreatedAt = model.CreatedAt = DateTimeOffset.UtcNow.ToLocalTime();
        user.Phone = model.Phone;
        user.IsActive = true;

        try
        {
            _dbcontext.rows.Add(user);
            _dbcontext.SaveChanges();
        }

        catch(ArgumentNullException)
        {
            System.Console.WriteLine("Rejected");
        }
        return RedirectToAction("ShowRow",user);
    }
    
    
    public IActionResult ShowRow([FromRoute]RowViewModel model)
    {
        var client =_dbcontext.rows.FirstOrDefault(u => u.Id == model.Id);
        return View(client);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize(Roles = "superadmin")]
    [HttpPost("{id}")]
    public async Task<IActionResult> Inactive([FromForm]Guid id)
    {
        var qu           eue = await _dbcontext.rows.FirstOrDefaultAsync(q => q.Id == id);
        if(queue is not null)
        {
            queue.IsActive = false;
            _dbcontext.rows.UpdateRange(queue);
            await _dbcontext.SaveChangesAsync();
        }
        return RedirectToAction("AdminPage");
    }

    [HttpGet("[action]")]
    public IActionResult Login(string returnUrl) 
        => View(new SigninViewModel() { ReturnUrl = returnUrl ?? string.Empty });



    [HttpPost("[action]")]
    public async Task<IActionResult> Login(SigninViewModel model)
    {    
        if(!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if(user is  null)
        {
            ModelState.AddModelError("Password", "Email yoki parol noto'g'ri kiritilgan.");
            return View(model);
        }
            var result = await _signinManager.PasswordSignInAsync(user, model.Password, false, false);
        if(result.Succeeded)
        {
            return RedirectToAction("AdminPage");
        }

        return BadRequest(result.IsNotAllowed);
    }
}