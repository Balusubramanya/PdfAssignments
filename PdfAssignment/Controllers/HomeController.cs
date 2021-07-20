using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using PdfAssignment.Models;
using PdfAssignment.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PdfAssignment.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// This is is just i have written for an example logic.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //this view also can be triggered after click event also now for example i have taken direct
            return View();
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase files)
        {
            try
            {
                if (files != null)
                {
                    ViewBag.FileStatus = string.Empty;
                    String FileExt = System.IO.Path.GetExtension(files.FileName).ToUpper();
                    var path = Request.PhysicalApplicationPath + @"UploadedFiles";
                    Directory.CreateDirectory(path);//if folder not exist it will create
                    if (files.ContentLength > 0 && FileExt == ".PDF")
                    {
                        string _FileName = System.IO.Path.GetFileName(files.FileName);
                        string _path = System.IO.Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                        files.SaveAs(_path);
                        DataS objDatas = new DataS();
                        var reader = path + "\\" + files.FileName;
                        if (System.IO.File.Exists(reader))
                        {
                            PdfReader inputPdf = new PdfReader(reader);
                            for (var i = 1; i <= inputPdf.NumberOfPages; i++)//used to extract page by page
                            {
                                var extract = PdfTextExtractor.GetTextFromPage(inputPdf, i);
                                var extractor = extract.Split('\n');
                                //Below properties is used to get only first text as taken below
                                objDatas.MyProperty1 = String.IsNullOrEmpty(objDatas.MyProperty1) ? extractor.Where(p => p.ToString().Contains("Policy no.:")).FirstOrDefault() : objDatas.MyProperty1;
                                objDatas.MyProperty2 = String.IsNullOrEmpty(objDatas.MyProperty2) ? extractor.Where(p => p.ToString().Contains("Telephone:")).FirstOrDefault() : objDatas.MyProperty2;
                                objDatas.MyProperty3 = String.IsNullOrEmpty(objDatas.MyProperty3) ? extractor.Where(p => p.ToString().Contains("Email id:")).FirstOrDefault() : objDatas.MyProperty3;
                            }
                            return View(objDatas);
                        }
                    }
                    else
                    {
                        ViewBag.FileStatus = "File upload failed!!";
                    }
                }
                else
                {
                    ViewBag.FileStatus = "Choose Atleast one file of PDF only";
                }
                return View();
            }
            catch
            {
                ViewBag.FileStatus = "File upload failed or check the file once!!";
                return View();
            }

        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}