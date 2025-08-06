using Microsoft.AspNetCore.Mvc;
using MultiShop.Images.WebUI.DAL.Entities;
using MultiShop.Images.WebUI.Models;
using MultiShop.Images.WebUI.Services;


namespace MultiShop.Images.WebUI.Controllers
{
    public class HomeController : Controller
    {

        private readonly ICloudStorageService _cloudStorageService;

        

        public HomeController( ICloudStorageService cloudStorageService)
        {
       
            _cloudStorageService = cloudStorageService;
        }

   


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(ImageDrive imageDrive)
        {
            if (ModelState.IsValid)
            {
                if (imageDrive.Photo != null)
                {
                    imageDrive.SavedFileName = GenerateFileNameToSave(imageDrive.Photo.FileName);
                    imageDrive.SavedUrl = await _cloudStorageService.UploadFileAsync(imageDrive.Photo, imageDrive.SavedFileName);
                }
                //_context.Add(imageDrive);
               // await _context.SaveChangesAsync();
                return RedirectToAction("Create","Home");
            }
            return View(imageDrive);
        }

        private string? GenerateFileNameToSave(string incomingFileName)
        {
            var fileName = Path.GetFileNameWithoutExtension(incomingFileName);
            var extension = Path.GetExtension(incomingFileName);
            return $"{fileName}-{DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmss")}{extension}";
        }


        private async Task GenerateSignedUrl(ImageDrive imageDrive)
        {
            // Get Signed URL only when Saved File Name is available.
            if (!string.IsNullOrWhiteSpace(imageDrive.SavedFileName))
            {
                imageDrive.SignedUrl = await _cloudStorageService.GetSignedUrlAsync(imageDrive.SavedFileName);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            /* if (id == null || _context.ImageDrives == null)
             {
                 return NotFound();
             }

             var imageDrive = await _context.ImageDrives.FindAsync(id);
             if (imageDrive == null)
             {
                 return NotFound();
             }
             await GenerateSignedUrl(imageDrive);
             return View(imageDrive);*/
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ImageDrive imageDrive)
        {
            /* if (id != imageDrive.Id)
             {
                 return NotFound();
             }

             if (ModelState.IsValid)
             {
                 try
                 {
                     await ReplacePhoto(imageDrive);
                     _context.Update(imageDrive);
                     await _context.SaveChangesAsync();
                 }
                 catch (DbUpdateConcurrencyException)
                 {
                     if (!imageDriveExists(imageDrive.Id))
                     {
                         return NotFound();
                     }
                     else
                     {
                         throw;
                     }
                 }
                 return RedirectToAction("Home");
             }
             return View(imageDrive);*/
            return View();
        }

        private async Task ReplacePhoto(ImageDrive imageDrive)
        {
            if (imageDrive.Photo != null)
            {
                //replace the file by deleting imageDrive.SavedFileName file and then uploading new imageDrive.Photo
                if (!string.IsNullOrEmpty(imageDrive.SavedFileName))
                {
                    await _cloudStorageService.DeleteFileAsync(imageDrive.SavedFileName);
                }
                imageDrive.SavedFileName = GenerateFileNameToSave(imageDrive.Photo.FileName);
                imageDrive.SavedUrl = await _cloudStorageService.UploadFileAsync(imageDrive.Photo, imageDrive.SavedFileName);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
           /* if (_context.ImageDrives == null)
            {
                return Problem("Entity set is null.");
            }
            var imageDrive = await _context.ImageDrives.FindAsync(id);
            if (imageDrive != null)
            {
                if (!string.IsNullOrWhiteSpace(imageDrive.SavedFileName))
                {
                    await _cloudStorageService.DeleteFileAsync(imageDrive.SavedFileName);
                    imageDrive.SavedFileName = String.Empty;
                    imageDrive.SavedUrl = String.Empty;
                }
                _context.ImageDrives.Remove(imageDrive);
            }

            await _context.SaveChangesAsync();*/
            return RedirectToAction("Home");
        }

        private bool imageDriveExists(int id)
        {
            // return _context.ImageDrives.Any(e => e.Id == id);
            return false;
        }
    }


}

