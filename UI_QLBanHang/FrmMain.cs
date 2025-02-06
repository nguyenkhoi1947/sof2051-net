using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace UI_QLBanHang
{
    public partial class FrmMain : Form
    {
        public static int session = 0;
        public static int profile = 0;
        public static string mail;
        Thread th;
        FrmDangNhap dn = new FrmDangNhap();

        public FrmMain()
        {
            InitializeComponent();
        }

        public void FrmMain_Load(object sender, EventArgs e)
        {
            Resetvalue();
            if (profile == 1)
            {
                thongtinnvToolStripMenuItem.Text = null;
                profile = 0;
            }
        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckExistForm("FrmKhach"))
            {
                FrmKhach nv = new FrmKhach();
                nv.MdiParent = this;
                nv.Show();
            }
            else
                ActiveChildForm("FrmKhach");
        }

        private void SảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckExistForm("frmHang"))
            {
                th = new Thread(OpenNewForm);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
                ActiveChildForm("frmHang");
        }

        private void NhanVienToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckExistForm("frmNhanVien"))
            {
                FrmNhanVien nv = new FrmNhanVien();
                nv.MdiParent = this;
                nv.Show();
            }
            else
                ActiveChildForm("frmNhanVien");
        }

        public void ĐăngNhậpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckExistForm("FrmDangNhap"))
            {
                dn.MdiParent = this;
                dn.Show();
                dn.FormClosed += new FormClosedEventHandler(FrmDangNhap_FormClosed);
            }
            else
                ActiveChildForm("FrmDangNhap");
        }

        private void ProfileNvToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmThongTinNV profilenv = new FrmThongTinNV(FrmMain.mail);

            if (!CheckExistForm("frmThongTinNV"))
            {
                profilenv.MdiParent = this;
                profilenv.FormClosed += new FormClosedEventHandler(FrmThongTinNV_FormClosed);
                profilenv.Show();
            }
            else
                ActiveChildForm("frmThongTinNV");
        }

        private void ThongKeSPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckExistForm("FrmThongKe"))
            {
                FrmThongKe nv = new FrmThongKe();
                nv.MdiParent = this;
                nv.Show();
            }
            else
                ActiveChildForm("FrmThongKe");
        }

        private void HuongDanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Tai lieu huong dan su dung phan mem.pdf");
                System.Diagnostics.Process.Start(path);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("The file is not found in the specified location");
            }
        }

        private bool CheckExistForm(string name)
        {
            bool check = false;
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Name == name)
                {
                    check = true;
                    break;
                }
            }
            return check;
        }

        private void ActiveChildForm(string name)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Name == name)
                {
                    frm.Activate();
                    break;
                }
            }
        }

        private void VaiTroNV()
        {
            NhanVienToolStripMenuItem.Visible = false;
            thongkeToolStripMenuItem.Visible = false;
        }

        private void Resetvalue()
        {
            if (session == 1)
            {
                thongtinnvToolStripMenuItem.Text = "Chào " + FrmMain.mail;
                NhanVienToolStripMenuItem.Visible = true;
                danhMụcToolStripMenuItem.Visible = true;
                LoOutToolStripMenuItem1.Enabled = true;
                thongkeToolStripMenuItem.Visible = true;
                ThongKeSPToolStripMenuItem.Visible = true;
                ProfileNvToolStripMenuItem.Visible = true;
                đăngNhậpToolStripMenuItem.Enabled = false;
                if (int.Parse(dn.VaiTro) == 0)
                {
                    VaiTroNV();
                }
            }
            else
            {
                NhanVienToolStripMenuItem.Visible = false;
                danhMụcToolStripMenuItem.Visible = false;
                LoOutToolStripMenuItem1.Enabled = false;
                ProfileNvToolStripMenuItem.Visible = false;
                ThongKeSPToolStripMenuItem.Visible = false;
                thongkeToolStripMenuItem.Visible = false;
                đăngNhậpToolStripMenuItem.Enabled = true;
            }
        }

        void FrmDangNhap_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Refresh();
            FrmMain_Load(sender, e);
        }

        void FrmThongTinNV_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Refresh();
            FrmMain_Load(sender, e);
        }

        private void LoOutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            thongtinnvToolStripMenuItem.Text = null;
            session = 0;
            Resetvalue();
        }

        private void OpenNewForm()
        {
            Application.Run(new FrmHang());
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void hướngDẫnToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
    }
}
