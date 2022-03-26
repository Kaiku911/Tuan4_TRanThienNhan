using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuan4_TRanThienNhan.Models;

namespace Tuan4_TRanThienNhan.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        MyDataDataContext data = new MyDataDataContext();


        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstGiohang = Session["Giohang"] as List<GioHang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<GioHang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;

        }

        public ActionResult ThemGiohang(int id, string strURL)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.Find(n => n.masach == id);
            if (sanpham == null)
            {
                sanpham = new GioHang(id);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        public int TongSoluong()
        {
            int tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Sum(n => n.iSoluong);

            }
            return tsl;
        }

        public int TongSoluongSanPham()
        {
            int tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }

        public double Tong()
        {
            double tt = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tt = lstGiohang.Sum(n => n.dThanhtien);
            }
            return tt;
        }

        public ActionResult GioHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = Tong();
            ViewBag.Tongsoluongsanpham = TongSoluongSanPham();
            ViewBag.Message = Session["Message"];
            ViewBag.AlertStatus = Session["AlertStatus"];
            Session.Remove("Message");
            Session.Remove("AlertStatus");
            return View(lstGiohang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = Tong();
            ViewBag.Tongsoluongsanpham = TongSoluongSanPham();
            return PartialView();
        }

        public ActionResult XoaGiohang(int id)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.masach == id);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.masach == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatGiohang(int id, FormCollection collection)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.masach == id);
            if (sanpham != null)
            {
                Sach sach = data.Saches.FirstOrDefault(m => m.masach == id);
                int soluong = int.Parse(collection["txtSoLg"].ToString());
                if (soluong > sach.soluongton)
                {
                    Session["Message"] = "Không đủ số lượng sách";
                    Session["AlertStatus"] = "danger";
                    return RedirectToAction("GioHang");
                }
                sanpham.iSoluong = soluong;
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("GioHang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
        
            if (Session["TaiKhoan"] == null || Session ["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["Giohang"]==null)
            {
                return RedirectToAction("Index", "Sach");
            }
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.TongTien = Tong();
            ViewBag.Tongsoluongsanpham = TongSoluongSanPham();
            return View(lstGiohang);
        }

        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Sach s = new Sach();

            List < GioHang > gh =  Laygiohang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);

            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;

            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();

            foreach(var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.masach = item.masach;
                ctdh.soluong = item.masach;
                ctdh.gia = (decimal)item.giaban;
                s = data.Saches.Single(n => n.masach == item.masach);
                s.soluongton -= ctdh.soluong;
                data.SubmitChanges();
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "GioHang");
        }



        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }

}