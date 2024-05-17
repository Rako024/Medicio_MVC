using Business.Exceptions.Doctor;
using Business.Services.Abstracts;
using Core.Models;
using Core.RepositoryAbstracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concretes;

public class DoctorService : IDoctorService
{
    IDoctorRepository _doctorRepository;
    IWebHostEnvironment _webHostEnvironment;
    public DoctorService(IDoctorRepository doctorRepository, IWebHostEnvironment webHostEnvironment)
    {
        _doctorRepository = doctorRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    public void CreateDoctor(Doctor doctor)
    {
        if(doctor == null)
        {
            throw new NotFoundDoctorException("", "Doctor is null!");
        }
        if(doctor.PhotoFile == null)
        {
            throw new NotFoundPhotoFileException("PhotoFile", "Photo File is null!");
        }
        if (!doctor.PhotoFile.ContentType.Contains("image/"))
        {
            throw new PhotoFileContentException("PhotoFile", "Photo file content is not valid!");
        }

        string path = _webHostEnvironment.WebRootPath + @"\upload\doctor\" + doctor.PhotoFile.FileName;
        using(FileStream file = new FileStream(path, FileMode.Create))
        {
            doctor.PhotoFile.CopyTo(file);
        }
        doctor.ImgUrl = doctor.PhotoFile.FileName;
        _doctorRepository.Add(doctor);
        _doctorRepository.Commit();

    }

    public void DeleteDoctor(int id)
    {
        Doctor doctor = _doctorRepository.Get(x=>x.Id == id);
        if(doctor == null)
        {
            throw new NotFoundDoctorException("", id + " - Doctor is not find!");
        }
        string path = _webHostEnvironment.WebRootPath + @"\upload\doctor\" + doctor.ImgUrl;
        FileInfo fileInfo = new FileInfo(path);
        fileInfo.Delete();
        _doctorRepository.Delete(doctor);
        _doctorRepository.Commit();
    }

    public List<Doctor> GetAllDoctors(Func<Doctor, bool>? func = null)
    {
        return _doctorRepository.GetAll(func);
    }

    public Doctor GetDoctor(Func<Doctor, bool>? func = null)
    {
        return _doctorRepository.Get(func);
    }

    public void UpdateDoctor(int id, Doctor newDoctor)
    {
        Doctor oldDoctor = _doctorRepository.Get(x => x.Id == id);
        if (oldDoctor == null)
        {
            throw new NotFoundDoctorException("", id + " - Doctor is not find!");
        }
        if(newDoctor == null)
        {
            throw new NotFoundDoctorException("", "Doctor is null!");
        }
        if(newDoctor.PhotoFile != null)
        {
            if (!newDoctor.PhotoFile.ContentType.Contains("image/"))
            {
                throw new PhotoFileContentException("PhotoFile", "Photo file content is not valid!");
            }

            string oldPath = _webHostEnvironment.WebRootPath + @"\upload\doctor\" + oldDoctor.ImgUrl;
            FileInfo fileInfo = new FileInfo(oldPath);
            fileInfo.Delete();

            string path = _webHostEnvironment.WebRootPath + @"\upload\doctor\" + newDoctor.PhotoFile.FileName;
            using (FileStream file = new FileStream(path, FileMode.Create))
            {
                newDoctor.PhotoFile.CopyTo(file);
            }
            oldDoctor.ImgUrl = newDoctor.PhotoFile.FileName;
        }

        oldDoctor.Name = newDoctor.Name;
        oldDoctor.Surname = newDoctor.Surname;
        oldDoctor.Position = newDoctor.Position;
        _doctorRepository.Commit();
    }
}
