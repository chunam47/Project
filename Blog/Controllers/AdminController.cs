using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;

using System.IO;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
       dbBlogDataContext data = new dbBlogDataContext();
        
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Tin(string id)
        {
           
            return View(data.TINs.ToList());
            
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if(String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                ADMIN ad = data.ADMINs.SingleOrDefault(n =>n.TENDANGNHAP == tendn && n.MATKHAU == matkhau);
                if (ad != null)
                {
                    ViewBag.Thongbao = "Đăng nhập thành công";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Tin", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không chính xác";
             }
            return View();                     
        }
        [HttpGet]
        public ActionResult Themmoi()
        {

            ViewBag.MaTD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TENCHUDE), "MACHUDE", "TENCHUDE");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoi(TIN tin, HttpPostedFileBase fileupload)
        {
            ViewBag.MaTD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TENCHUDE), "MACHUDE", "TENCHUDE");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/HinhAnh/"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    tin.MAANH= fileName;
                    data.TINs.InsertOnSubmit(tin);
                    data.SubmitChanges();
                }
                return RedirectToAction("Tin");
            }

        }
        //public ActionResult Chitietsanpham(int id)
        //{
        //    SANPHAM sanpham = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
        //    ViewBag.MaSP = sanpham.MaSP;
        //    if(sanpham==null)
        //    {
        //        Response.StatusCode = 404;
        //        return null;
        //    }
        //    return View(sanpham);
        //}
        [HttpGet]
        public ActionResult Xoa(string id)
        {
            TIN sanpham = data.TINs.SingleOrDefault(n => n.MATIN == id);
            ViewBag.MaSP = sanpham.MATIN;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }
        [HttpPost, ActionName("Xoa")]
        public ActionResult Xacnhanxoa(string id)
        {
            TIN sanpham = data.TINs.SingleOrDefault(n => n.MATIN == id);
            ViewBag.MaSP = sanpham.MATIN;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.TINs.DeleteOnSubmit(sanpham);
            data.SubmitChanges();
            return RedirectToAction("Tin");
        }
        //[HttpGet]
        //public ActionResult Suasanpham(int id)
        //{
        //    SANPHAM sanpham = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
        //    if(sanpham==null)
        //    {
        //        Response.StatusCode = 404;
        //        return null;
        //    }
        //    ViewBag.MaTD = new SelectList(data.THUCDONs.ToList().OrderBy(n => n.TenThucDon), "MaTD", "TenThucDon", sanpham.MaTD);
        //    return View(sanpham);
        //}
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Suasanpham(SANPHAM sanpham, HttpPostedFileBase fileupload)
        //{
        //    ViewBag.MaTD = new SelectList(data.THUCDONs.ToList().OrderBy(n => n.TenThucDon), "MaTD", "TenThucDon");

        //    if(fileupload == null)
        //    {
        //        ViewBag.Thongbao = "Vui lòng chọn ảnh";
        //        return View();
        //    }
        //    else
        //    {
        //        if(ModelState.IsValid)
        //        {
        //            var fileName = Path.GetFileName(fileupload.FileName);
        //            var path = Path.Combine(Server.MapPath("~/fastfood/"), fileName);
        //            if (System.IO.File.Exists(path))
        //                ViewBag.Thongbao = "File đã tồn tại";
        //            else
        //               {
        //                fileupload.SaveAs(path);
        //               }
        //            sanpham.Anhbia = fileName;
        //            UpdateModel(sanpham);
        //            data.SubmitChanges();
        //        }
        //        return RedirectToAction("Sanpham");
        //    }

        //}

    }
}