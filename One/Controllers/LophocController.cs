using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using One.Models;
using One.Models.Process;

namespace One.Controllers
{
    public class LophocController : Controller
    {
        private readonly MvcMovieContext _context;
        private StringProcess strPro = new StringProcess();
        private ExcelProcess _excelProcess = new ExcelProcess();

        public LophocController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Lophoc
        public async Task<IActionResult> Index()
        {
              return _context.Lophoc != null ? 
                          View(await _context.Lophoc.ToListAsync()) :
                          Problem("Entity set 'MvcMovieContext.Lophoc'  is null.");
        }

        // GET: Lophoc/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Lophoc == null)
            {
                return NotFound();
            }

            var lophoc = await _context.Lophoc
                .FirstOrDefaultAsync(m => m.Malop == id);
            if (lophoc == null)
            {
                return NotFound();
            }

            return View(lophoc);
        }

       // GET: Sinhvien/Create
        //Sinh mã tự động 
        // public IActionResult Create()
        // {
        //     return View();
        // }
        public IActionResult Create()
        {
            var IDdautien = "L1";
            var countAnh = _context.Lophoc.Count();
            if (countAnh > 0)
            {
                var Malop = _context.Lophoc.OrderByDescending(m => m.Malop).First().Malop;
                IDdautien = strPro.AutoGenerateCode(Malop);
            }
            ViewBag.newID = IDdautien;
            return View();
        }
        // POST: Lophoc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Malop,Hoten")] Lophoc lophoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lophoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lophoc);
        }

        // GET: Lophoc/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Lophoc == null)
            {
                return NotFound();
            }

            var lophoc = await _context.Lophoc.FindAsync(id);
            if (lophoc == null)
            {
                return NotFound();
            }
            return View(lophoc);
        }

        // POST: Lophoc/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Malop,Hoten")] Lophoc lophoc)
        {
            if (id != lophoc.Malop)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lophoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LophocExists(lophoc.Malop))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lophoc);
        }

        // GET: Lophoc/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Lophoc == null)
            {
                return NotFound();
            }

            var lophoc = await _context.Lophoc
                .FirstOrDefaultAsync(m => m.Malop == id);
            if (lophoc == null)
            {
                return NotFound();
            }

            return View(lophoc);
        }

        // POST: Lophoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Lophoc == null)
            {
                return Problem("Entity set 'MvcMovieContext.Lophoc'  is null.");
            }
            var lophoc = await _context.Lophoc.FindAsync(id);
            if (lophoc != null)
            {
                _context.Lophoc.Remove(lophoc);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LophocExists(string id)
        {
          return (_context.Lophoc?.Any(e => e.Malop == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> Upload()
        {
            return View();
        }
    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    ModelState.AddModelError("", "Please choose excel file to upload!");
                }
                else
                {
                    var FileName = DateTime.Now.ToShortTimeString() + fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads/Excels", FileName);
                    var fileLocation = new FileInfo(filePath).ToString();
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        //save file to sever
                        await file.CopyToAsync(stream);
                        var dt = _excelProcess.ExcelToDataTable(fileLocation);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var sv = new Lophoc();

                            sv.Malop = dt.Rows[i][0].ToString();
                            sv.Hoten = dt.Rows[i][1].ToString();
                        
                         _context.Lophoc.Add(sv);
                        }
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View();
    }



    }
}
