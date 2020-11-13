﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Context.Data;
using Context.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Context.Controllers
{
    [Authorize]
    public class UserBooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public UserBooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        // GET: UserBooks
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();

            var applicationDbContext = _context.UserBooks.Include(u => u.Book).Include(u => u.User);
            return View(await applicationDbContext.Where(u => u.UserId == user.Id).ToListAsync());
        }

        // GET: UserBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBook = await _context.UserBooks
                .Include(u => u.Book)
                .FirstOrDefaultAsync(m => m.UserBookId == id);
            if (userBook == null)
            {
                return NotFound();
            }

            return View(userBook);
        }

        //Get: UserBooks/ViewCountryInfo/5
        public async Task<IActionResult> ViewCountryInfo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBook = await _context.UserBooks
                .Include(u => u.Book)
                .FirstOrDefaultAsync(m => m.UserBookId == id);
            if (userBook == null)
            {
                return NotFound();
            }

            return View(userBook);
        }


        // GET: UserBooks/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "AuthorFirstName");
            return View();
        }

        // POST: UserBooks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserBookId,BookId,UserId")] UserBook userBook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "AuthorFirstName", userBook.BookId);
            return View(userBook);
        }

        // GET: UserBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }



            var userBook = await _context.UserBooks.Include(ub => ub.Book).FirstOrDefaultAsync(ub => ub.UserBookId == id);
            if (userBook == null)
            {
                return NotFound();
            }
            //ViewData["BookId"] = new SelectList(_context.Books, "BookId", "AuthorFirstName", userBook.BookId);
            return View(userBook);
        }

        // POST: UserBooks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserBookId,BookId,UserId,Comments")] UserBook userBook)
        {


            
            if (id != userBook.UserBookId)
                {
                    return NotFound();
                }
                           


            ModelState.Remove("User");
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
            var user = await GetCurrentUserAsync();
                userBook.UserId = user.Id;
               var ub = await _context.UserBooks.FindAsync(id);
                userBook.BookId = ub.BookId;


                try
                {
                    _context.Entry(ub).State = EntityState.Detached;
                    _context.Update(userBook);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserBookExists(userBook.UserBookId)
)
                        {
                        return NotFound();
                         }
                        else
                        {
                        throw;
                    }
                }
                return RedirectToAction("Details", "userBooks", new {id});
                // return RedirectToAction(nameof(Index/));
            }
            //ViewData["BookId"] = new SelectList(_context.Books, "BookId", "AuthorFirstName", userBook.BookId);
            return View(userBook);
        }

        // GET: UserBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBook = await _context.UserBooks
                .Include(u => u.Book)
                .FirstOrDefaultAsync(m => m.UserBookId == id);
            if (userBook == null)
            {
                return NotFound();
            }

            return View(userBook);
        }

        // POST: UserBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userBook = await _context.UserBooks.FindAsync(id);
            _context.UserBooks.Remove(userBook);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserBookExists(int id)
        {
            return _context.UserBooks.Any(e => e.UserBookId == id);
        }
    }
}
