﻿using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BUS_QLBanHang;
using DTO_QLBanHang;

namespace UI_QLBanHang
{
    public partial class FrmHang : Form
    {
        BUS_Hang busHang = new BUS_Hang();
        string stremail = FrmMain.mail;
        string checkUrlImage;
        string fileName;
        string fileSavePath;
        string fileAddress;

        public FrmHang()
        {
            InitializeComponent();
        }

        private void FrmHang_Load(object sender, EventArgs e)
        {
            ResetValues();
            LoadGridview_Hang();
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnBoqua.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            btnMo.Enabled = true;
            txtTenhang.Enabled = true;
            txtSoluong.Enabled = true;
            txtDongianhap.Enabled = true;
            txtDongiaban.Enabled = true;
            txtGhichu.Enabled = true;
            txtMahang.Text = txtTenhang.Text = txtSoluong.Text = txtDongianhap.Text = txtDongiaban.Text = txtHinh.Text = txtGhichu.Text = null;
            pbHinh.Image = null;
            txtTenhang.Focus();
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            int maHang = int.Parse(txtMahang.Text);
            if (MessageBox.Show("Bạn có chắc muốn xóa dữ liệu", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (busHang.DeleteHang(maHang))
                {
                    MessageBox.Show("Xóa dữ liệu thành công");
                    ResetValues();
                    LoadGridview_Hang();
                }
                else
                {
                    MessageBox.Show("Xóa không thành công");
                }
            }
            else
            {
                ResetValues();
            }
        }

        private void BtnDong_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnBoqua_Click(object sender, EventArgs e)
        {
            ResetValues();
        }

        private void LoadGridview_Hang()
        {
            dgvhang.DataSource = busHang.GetHang();
            dgvhang.Columns[0].HeaderText = "Mã Sản Phẩm";
            dgvhang.Columns[1].HeaderText = "Tên Sản Phẩm";
            dgvhang.Columns[2].HeaderText = "Số Lượng";
            dgvhang.Columns[3].HeaderText = "Đơn Giá Nhập";
            dgvhang.Columns[4].HeaderText = "Đơn Giá Bán";
            dgvhang.Columns[5].HeaderText = "Hình Ảnh";
            dgvhang.Columns[7].Visible = false;
        }

        private void ResetValues()
        {
            txtMahang.Text = txtTenhang.Text = txtSoluong.Text = txtDongianhap.Text = txtDongiaban.Text = txtHinh.Text = txtGhichu.Text = null;
            pbHinh.Image = null;

            txtTenhang.Enabled = txtSoluong.Enabled = txtDongianhap.Enabled = txtDongiaban.Enabled = txtHinh.Enabled = txtGhichu.Enabled = false;
            btnMo.Enabled = false;
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnDong.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtSoluong.Text, out int intSoLuong) || intSoLuong < 0 ||
                !float.TryParse(txtDongianhap.Text, out float floatDonGiaNhap) || floatDonGiaNhap < 0 ||
                !float.TryParse(txtDongiaban.Text, out float floatDonGiaBan) || floatDonGiaBan < 0 ||
                string.IsNullOrWhiteSpace(txtTenhang.Text) ||
                string.IsNullOrWhiteSpace(txtHinh.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ và đúng dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DTO_Hang h = new DTO_Hang(txtTenhang.Text, intSoLuong, floatDonGiaNhap, floatDonGiaBan, "\\Images\\" + fileName, txtGhichu.Text, stremail);
            if (busHang.InsertHang(h))
            {
                MessageBox.Show("Thêm sản phẩm thành công");
                File.Copy(fileAddress, fileSavePath, true);
                ResetValues();
                LoadGridview_Hang();
            }
            else
            {
                MessageBox.Show("Thêm sản phẩm không thành công");
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            int intSoLuong;
            bool isInt = int.TryParse(txtSoluong.Text.Trim(), out intSoLuong);
            float floatDonGiaNhap;
            bool isFloatNhap = float.TryParse(txtDongianhap.Text.Trim(), out floatDonGiaNhap);
            float floatDonGiaBan;
            bool isFloatBan = float.TryParse(txtDongiaban.Text.Trim(), out floatDonGiaBan);

            if (txtTenhang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenhang.Focus();
                return;
            }
            if (!isInt || intSoLuong < 0)
            {
                MessageBox.Show("Bạn phải nhập số lượng sản phẩm > 0", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoluong.Focus();
                return;
            }
            if (!isFloatNhap || floatDonGiaNhap < 0)
            {
                MessageBox.Show("Bạn phải nhập đơn giá nhập > 0", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDongianhap.Focus();
                return;
            }
            if (!isFloatBan || floatDonGiaBan < 0)
            {
                MessageBox.Show("Bạn phải nhập đơn giá bán > 0", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDongiaban.Focus();
                return;
            }
            if (txtHinh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải upload hình", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnMo.Focus();
                return;
            }

            DTO_Hang h = new DTO_Hang(
                int.Parse(txtMahang.Text), txtTenhang.Text, intSoLuong,
                floatDonGiaNhap, floatDonGiaBan, txtHinh.Text, txtGhichu.Text
            );

            if (MessageBox.Show("Bạn có chắc muốn chỉnh sửa", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (busHang.UpdateHang(h))
                {
                    if (txtHinh.Text != checkUrlImage)
                        File.Copy(fileAddress, fileSavePath, true);

                    MessageBox.Show("Sửa thành công");
                    ResetValues();
                    LoadGridview_Hang();
                }
                else
                {
                    MessageBox.Show("Sửa không thành công");
                }
            }
            else
            {
                ResetValues();
            }
        }

        private void BtnMo_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog
            {
                Filter = "Bitmap(*.bmp)|*.bmp|JPEG(*.jpg)|*.jpg|GIF(*.gif)|*.gif|All files(*.*)|*.*",
                FilterIndex = 2,
                Title = "Chọn ảnh minh hoạ cho sản phẩm"
            };

            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                fileAddress = dlgOpen.FileName;
                pbHinh.Image = Image.FromFile(fileAddress);
                fileName = Path.GetFileName(dlgOpen.FileName);
                string saveDirectory = Application.StartupPath.Substring(0, Application.StartupPath.Length - 10);
                fileSavePath = saveDirectory + "\\Images\\" + fileName;
                txtHinh.Text = "\\Images\\" + fileName;
            }
        }

        private void BtnTimkiem_Click(object sender, EventArgs e)
        {
            DataTable ds = busHang.SearchHang(txttimKiem.Text);
            if (ds.Rows.Count > 0)
            {
                dgvhang.DataSource = ds;
            }
            else
            {
                MessageBox.Show("Không tìm thấy sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            txttimKiem.Text = "Nhập tên sản phẩm";
            txttimKiem.BackColor = Color.LightGray;
            ResetValues();
        }

        private void TxttimKiem_Click(object sender, EventArgs e)
        {
            txttimKiem.Text = null;
            txttimKiem.BackColor = Color.White;
        }

        private void BtnDanhsach_Click(object sender, EventArgs e)
        {
            ResetValues();
            LoadGridview_Hang();
        }
        private void Dgvhang_Click(object sender, EventArgs e)
        {
            string saveDirectory = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10));
            if (dgvhang.Rows.Count > 1)
            {
                btnMo.Enabled = true;
                btnLuu.Enabled = false;
                txtTenhang.Enabled = true;
                txtSoluong.Enabled = true;
                txtDongianhap.Enabled = true;
                txtDongiaban.Enabled = true;
                txtGhichu.Enabled = true;
                txtTenhang.Focus();
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                txtMahang.Text = dgvhang.CurrentRow.Cells["MaHang"].Value.ToString();
                txtTenhang.Text = dgvhang.CurrentRow.Cells["TenHang"].Value.ToString();
                txtSoluong.Text = dgvhang.CurrentRow.Cells["SoLuong"].Value.ToString();
                txtDongianhap.Text = dgvhang.CurrentRow.Cells["DonGiaNhap"].Value.ToString();
                txtDongiaban.Text = dgvhang.CurrentRow.Cells["DonGiaBan"].Value.ToString();
                txtHinh.Text = dgvhang.CurrentRow.Cells["HinhAnh"].Value.ToString();
                checkUrlImage = txtHinh.Text;
                pbHinh.Image = Image.FromFile(saveDirectory + dgvhang.CurrentRow.Cells["HinhAnh"].Value.ToString());
                txtGhichu.Text = dgvhang.CurrentRow.Cells["GhiChu"].Value.ToString();
            }
            else
            {
                MessageBox.Show("Bảng không tồn tại dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
