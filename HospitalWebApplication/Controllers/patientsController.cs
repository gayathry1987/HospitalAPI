using HospitalWebApplication.Data;
using HospitalWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalWebApplication.Controllers
{
    public class patientsController : Controller
    {
        private readonly HospitalWebApplicationContext _context;

        public patientsController(HospitalWebApplicationContext context)
        {
            _context = context;
        }


        // GET: patients
        public async Task<IActionResult> Index()
        {
            int BedInUse = 0, BedsAvailable = 0, TotalPatients = 0;
           if (_context.patient !=null)
            {
                BedInUse = _context.patient.Where(t => t.PatientID != 0).Count();
                BedsAvailable = _context.patient.Where(t => t.PatientID == 0).Count();
                if (_context.patientDetails != null)
                    TotalPatients = _context.patientDetails.Where(t =>t.admittedTime>= DateTime.Today && t.admittedTime <DateTime.Today.AddDays(1)).Count();
            }
            @ViewBag.BedInUse = BedInUse;
            @ViewBag.BedsAvailable = BedsAvailable;
            @ViewBag.TotalPatients = TotalPatients;
            return _context.patient != null ? 
                          View(await _context.patient.ToListAsync()) :
                          Problem("Entity set 'HospitalWebApplicationContext.patients'  is null.");
        }

        public async Task<IActionResult> SelectPatient(int id)
        {
            // int? bedNo = id;
            if(_context.patientDetails !=null)
            {
                List<patientDetails> details = _context.patientDetails.ToList();
                foreach(var patientDetails in details)
                {
                    patientDetails.bedID = id;
                }
               return View(await _context.patientDetails.ToListAsync());
            }
            else
            {
                return Problem("Entity set 'HospitalWebApplicationContext.patientDetails'  is null.");
            }
          
        }

        public async Task<IActionResult> Comments(int? id)
        {
            return _context.patient != null ?
                        View(await _context.patientComments.Where(t=>t.patientID == id).ToListAsync()) :
                        Problem("Entity set 'HospitalWebApplicationContext.patients'  is null.");
        }
        public async Task<IActionResult> Add(int? id)
        {
            if (id == null || _context.patientComments == null)
            {
                return NotFound();
            }

            var patients = await _context.patientDetails
                .FirstOrDefaultAsync(m => m.patientid == id);
            patientComments comments = new patientComments();
            comments.patientID = patients.patientid;
            comments.URN = patients.URN;
            comments.PatientName = patients.PatientName;
            return View(comments);
        }
        // GET: patients/Details/5
        // GET: patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.patientDetails == null)
            {
                return NotFound();
            }

            var patients = await _context.patientDetails
                .FirstOrDefaultAsync(m => m.patientid == id);
            if (patients == null)
            {
                return NotFound();
            }

            return View(patients);
        }

        // GET: patients/Create
        public IActionResult Create()
        {
            return  View();
        }
       
        // POST: patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("URN,bedID,PatientName,DOB,presentingIssue")] patientDetails patientDetails)
        {
            if (ModelState.IsValid)
            {
                patientDetails.admittedTime = null;
                patientDetails.dischargedTime = null;
                patientDetails.isAdmitted = false;
                patientDetails.isDischarged = false;
                _context.Add(patientDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patientDetails);
        }

        // GET: patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.patientDetails == null)
            {
                return NotFound();
            }

            var patients = await _context.patientDetails.FindAsync(id);
          
            if (patients == null)
            {
                return NotFound();
            }
            return View(patients);
        }
        public async Task<IActionResult> Update(int URN,int bedNo)
        {
            var patientDetail = await _context.patientDetails.FindAsync(URN);
            if (patientDetail == null)
            {
                return NotFound();
            }
            patientDetail.isAdmitted = true;
            patientDetail.admittedTime = DateTime.Now;
            patientDetail.bedID = bedNo;
            _context.Update(patientDetail);
            await _context.SaveChangesAsync();

            var bedStatus = await _context.patient.FindAsync(bedNo);
            if (bedStatus == null)
            {
                return NotFound();
            }
            bedStatus.URN = patientDetail.URN;
            bedStatus.PatientID = patientDetail.patientid;
            bedStatus.PatientName = patientDetail.PatientName;
            bedStatus.BedNo = bedStatus.BedNo;
            bedStatus.Status = "In Use";
            bedStatus.LastUpdate = DateTime.Now;
            bedStatus.presentingIssue = patientDetail.presentingIssue;
            _context.Update(bedStatus);

            patientComments comments = new patientComments();
            comments.Nurse = patientDetail.nurse;
            comments.Comments = "Admitted";
            comments.updatedTime = DateTime.Now;
            comments.PatientName = patientDetail.PatientName;
            comments.URN = patientDetail.URN;
            comments.patientID = patientDetail.patientid;
            _context.Add(comments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); 
        }

        // POST: patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("patientid,URN,bedID,PatientName,DOB,isAdmitted,admittedTime,isDischarged,dischargedTime,presentingIssue,nurse,comments")] patientDetails patients)
        {
            if (id != patients.patientid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    patientComments comments = new patientComments();
                    comments.Nurse = patients.nurse;
                    comments.Comments = patients.comments;
                    comments.updatedTime = DateTime.Now;
                    comments.PatientName = patients.PatientName;
                    comments.URN = patients.URN;
                    comments.patientID = patients.patientid;
                    _context.Add(comments);

                    patient bedStatus = await _context.patient.FindAsync(patients.bedID);
                    if(bedStatus != null)
                    {
                        bedStatus.LastUpdate = DateTime.Now;
                        bedStatus.LastComment = patients.comments;
                        _context.Update(bedStatus);
                    }
                    _context.Update(patients);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!patientsExists(patients.patientid))
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
            return View(patients);
        }

        // GET: patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.patientDetails == null)
            {
                return NotFound();
            }

            var patients = await _context.patientDetails
                .FirstOrDefaultAsync(m => m.patientid == id);
            if (patients == null)
            {
                return NotFound();
            }

            return View(patients);
        }

        // POST: patients/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.patientDetails == null)
            {
                return Problem("Entity set 'HospitalWebApplicationContext.patients'  is null.");
            }
            patientDetails patients = await _context.patientDetails.FindAsync(id);
            if (patients != null)
            {
                int bedID = patients.bedID;
                patients.isDischarged = true;
                patients.dischargedTime = DateTime.Now;
                patients.bedID = 0;
                _context.Update(patients);

                patient bedStatus = await _context.patient.FindAsync(bedID);
                bedStatus.PatientID = 0;
                bedStatus.PatientName = "";
                bedStatus.presentingIssue = "";
                bedStatus.URN = "";
                bedStatus.LastComment = "";
                //bedStatus.LastUpdate = DateTime.MinValue;
                bedStatus.Status = "Unoccupied";
                _context.Update(bedStatus);
                //_context.patientDetails.Remove(patients);
                patientComments comments = new patientComments();
                comments.Nurse = patients.nurse;
                comments.Comments = "Discharged";
                comments.updatedTime = DateTime.Now;
                comments.PatientName = patients.PatientName;
                comments.URN = patients.URN;
                comments.patientID = patients.patientid;
                _context.Add(comments);
                _context.Update(patients);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost, ActionName("Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("patientID,Nurse,Comments")] patientComments pcomments)
        {
            int patientid = pcomments.patientID;
            patientDetails patientsd = await _context.patientDetails.FindAsync(patientid);
            pcomments.updatedTime = DateTime.Now;
            pcomments.PatientName = patientsd.PatientName;
            pcomments.URN = patientsd.URN;
            pcomments.patientID = patientsd.patientid;
            _context.Add(pcomments);
            patient bedStatus = await _context.patient.FindAsync(patientsd.bedID);
            if (bedStatus != null)
            {
                bedStatus.LastUpdate = DateTime.Now;
                bedStatus.LastComment = pcomments.Comments;
                _context.Update(bedStatus);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
            private bool patientsExists(int id)
        {
          return (_context.patient?.Any(e => e.PatientID == id)).GetValueOrDefault();
        }
    }
}
