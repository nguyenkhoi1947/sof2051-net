﻿using System.Data;
using DAL_QLBanHang;
using DTO_QLBanHang;

namespace BUS_QLBanHang
{
    public class BUS_Khach
    {
        DAL_Khach dalKhach = new DAL_Khach();
        public DataTable getKhach()
        {
            return dalKhach.getKhach();
        }
        public bool InsertKhach(DTO_Khach khach)
        {
            return dalKhach.insertKhach(khach);
        }
        public bool UpdateKhach(DTO_Khach khach)
        {
            return dalKhach.UpdateKhach(khach);
        }
        public bool DeleteKhach(string soDT)
        {
            return dalKhach.DeleteKhach(soDT);
        }
        public DataTable SearchKhach(string soDT)
        {
            return dalKhach.SearchKhach(soDT);
        }
    }
}
