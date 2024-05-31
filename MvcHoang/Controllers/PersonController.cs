using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcHoang.Data;
using MvcHoang.Models;
using MvcHoang.Models.Process;
using OfficeOpenXml;
using X.PagedList;

namespace MvcHoang.Controllers
{
    public class PersonController : Controller
    {
        private readonly ApplicationDbContext _context;

         private ExcelProcess _excelProcess = new ExcelProcess();
        public PersonController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index (int? page, int? PageSize)
        {
            ViewBag.PageSize= new List<SelectListItem>()
            {
        
            new SelectListItem() { Value="3", Text="3" }, 
            new SelectListItem() { Value="5", Text="5" }, 
            new SelectListItem() { Value="10",Text="10" }, 
            new SelectListItem() { Value="15", Text="15" }, 
            new SelectListItem() { Value="25", Text="25" },
            new SelectListItem() { Value="50" ,Text="50" },
            };
            int pagesize = (PageSize ?? 3);
            ViewBag.psize = pagesize;
            var model = _context.Person.ToList().ToPagedList (page ?? 1, pagesize); 
            return View(model);
        } 
        [HttpPost]
        public async Task<IActionResult> Index( string searchTen)
        {
            
            return View(await _context.Person.Where(m => m.FullName.Contains(searchTen)).ToListAsync());
        
        }

        // GET: Person/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: Person/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Person/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,FullName,Address")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: Person/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PersonId,FullName,Address")] Person person)
        {
            if (id != person.PersonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.PersonId))
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
            return View(person);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var person = await _context.Person.FindAsync(id);
            if (person != null)
            {
                _context.Person.Remove(person);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(string id)
        {
            return _context.Person.Any(e => e.PersonId == id);
        }
  // uploads
    public async Task<IActionResult> Upload()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Upload (IFormFile file)
    {
        if (file!=null)
        {
            string fileExtension = Path.GetExtension (file.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")

            {
                ModelState.AddModelError("", "Please choose excel file to upload!");

            }
             else
                {
        
                var fileName = DateTime.Now.ToShortTimeString() + fileExtension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads/Excels", fileName); 
                var fileLocation = new FileInfo(filePath).ToString();
                using (var stream = new FileStream(filePath, FileMode.Create))


                {await file.CopyToAsync(stream);
                
                var dt = _excelProcess.ExcelToDataTable(fileLocation);
                    //using for loop to read data from dt
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                    //create new Person object
                    var ps = new Person();
                    //set value to attributes
                    ps.PersonId = dt.Rows[i][0].ToString(); 
                    ps.FullName = dt.Rows[i][1].ToString(); 
                    ps.Address = dt.Rows[i][2].ToString(); 
                    //add object to context 
                    _context.Add(ps);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));}
                }
        }
    
        
        return View();
    }

    // Download   
        public IActionResult Download()
        {
        //Name the file when downloading
        var fileName = "YourFileName" + ".xlsx";
        using (ExcelPackage excelPackage = new ExcelPackage())
        {
        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
        //add some text to cell A1
        worksheet.Cells["A1"].Value = "StudentID"; 
        worksheet.Cells["B1"].Value = "FullName";
        worksheet.Cells["C1"].Value = "Age"; 
        //get all Person
        var studentList = _context.Student.ToList(); 
        //fill data to worksheet
        worksheet.Cells["A2"].LoadFromCollection (studentList);
        var stream = new MemoryStream (excelPackage.GetAsByteArray()); //download file
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    
    }
}

}