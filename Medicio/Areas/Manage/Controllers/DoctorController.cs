using Business.Exceptions.Doctor;
using Business.Services.Abstracts;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medicio.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="Admin")]
    public class DoctorController : Controller
    {
        IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        public IActionResult Index()
        {
            List<Doctor> doctors = _doctorService.GetAllDoctors();
            return View(doctors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
           
            try
            {
                _doctorService.CreateDoctor(doctor);
            }catch(NotFoundDoctorException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (PhotoFileContentException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (NotFoundPhotoFileException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _doctorService.DeleteDoctor(id);
            }catch(NotFoundDoctorException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            Doctor doctor = _doctorService.GetDoctor(x => x.Id == id);
            if(doctor == null)
            {
                ModelState.AddModelError("", "Doctor is null!");
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }
        [HttpPost]
        public IActionResult Update(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _doctorService.UpdateDoctor(doctor.Id, doctor);
            }
            catch (PhotoFileContentException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (NotFoundPhotoFileException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
