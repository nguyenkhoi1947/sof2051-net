using System.Data;
using DAL_QLBanHang;
using DTO_QLBanHang;

namespace BUS_QLBanHang
{
    public class BUS_NhanVien
    {
        DAL_NhanVien dalNhanVien = new DAL_NhanVien();

        public bool NhanVienDangNhap(DTO_NhanVien nv)
        {
            return dalNhanVien.NhanVienDangNhap(nv);
        }

        public DataTable GetNhanVien()
        {
            return dalNhanVien.getNhanVien();
        }
        public bool InsertNhanVien(DTO_NhanVien Nv)
        {
            return dalNhanVien.insertNhanVien(Nv);
        }
        public bool UpdateNhanVien(DTO_NhanVien Nv)
        {
            return dalNhanVien.UpdateNhanVien(Nv);
        }
        public bool DeleteNhanVien(string tenDangNhap)
        {
            return dalNhanVien.DeleteNnhanVien(tenDangNhap);
        }
        public DataTable SearchNhanVien(string tenNhanvien)
        {
            return dalNhanVien.SearchNhanVien(tenNhanvien);
        }
        public DataTable VaiTroNhanVien(string email)
        {
            return dalNhanVien.VaiTroNhanVien(email);
        }
        public bool UpdateMatKhau(string email, string matKhauCu, string matKhauMoi)
        {
            return dalNhanVien.UpdateMatKhau(email, matKhauCu, matKhauMoi);
        }
        public bool NhanVienQuenMatKhau(string email)
        {
            return dalNhanVien.NhanVienQuenMatKhau(email);
        }
        public bool TaoMatKhau(string email, string matKhauMoi)
        {
            return dalNhanVien.TaoMatKhau(email, matKhauMoi);
        }
    }
}
