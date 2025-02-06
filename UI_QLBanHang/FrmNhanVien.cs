using System;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using BUS_QLBanHang;
using DTO_QLBanHang;

namespace UI_QLBanHang
{
    public partial class FrmNhanVien : Form
    {
        private readonly BUS_NhanVien busNhanVien = new BUS_NhanVien();

        public FrmNhanVien()
        {
            InitializeComponent();
        }

        private void FrmNhanVien_Load(object sender, EventArgs e)
        {
            ResetValues();
            LoadGridview_NhanVien();
        }

        private void BtnBoqua_Click(object sender, EventArgs e)
        {
            ResetValues();
            LoadGridview_NhanVien();
        }

        private void LoadGridview_NhanVien()
        {
            dgvNhanvien.DataSource = busNhanVien.GetNhanVien();
            dgvNhanvien.Columns[0].HeaderText = "Email";
            dgvNhanvien.Columns[1].HeaderText = "Tên Nhân Viên";
            dgvNhanvien.Columns[2].HeaderText = "Địa chỉ";
            dgvNhanvien.Columns[3].HeaderText = "Vai Trò";
            dgvNhanvien.Columns[4].HeaderText = "Tình Trạng";
        }

        private void ResetValues()
        {
            txttimKiem.Text = "Nhập tên nhân viên";
            txtEmail.Text = null;
            txtTennv.Text = null;
            txtDiachi.Text = null;
            txtEmail.Enabled = false;
            txtTennv.Enabled = false;
            txtDiachi.Enabled = false;
            rbNhanvien.Enabled = false;
            rbQuantri.Enabled = false;
            rbHoatDong.Enabled = false;
            rbNgung.Enabled = false;
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnDong.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            txtEmail.Text = null;
            txtTennv.Text = null;
            txtDiachi.Text = null;
            txtTennv.Enabled = true;
            txtEmail.Enabled = true;
            txtDiachi.Enabled = true;
            rbNhanvien.Enabled = true;
            rbQuantri.Enabled = true;
            rbNgung.Enabled = true;
            rbHoatDong.Enabled = true;
            btnLuu.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            rbNhanvien.Checked = false;
            rbNgung.Checked = false;
            rbQuantri.Checked = false;
            rbHoatDong.Checked = false;
            txtEmail.Focus();
        }

        private void BtnDong_Click(object sender, EventArgs e)
        {
            Close();
        }

        public bool IsValid(string emailAddress)
        {
            try
            {
                new MailAddress(emailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public void SendMail(string email)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 25)
                {
                    Credentials = new NetworkCredential("duoichon1@gmail.com", "chonduoi"),
                    EnableSsl = true
                };

                MailMessage msg = new MailMessage
                {
                    From = new MailAddress("duoichon1@gmail.com"),
                    Subject = "Chao mừng thành viên mới",
                    Body = "Chào anh/chị. Mật khẩu truy cập phần mềm là abc123, anh/chị vui lòng đăng nhập vào phần mềm và đổi mật khẩu."
                };
                msg.To.Add(email);

                client.Send(msg);
                MessageBox.Show("Email đã được gửi thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Bạn phải nhập email.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmail.Focus();
                return;
            }

            if (!IsValid(txtEmail.Text))
            {
                MessageBox.Show("Bạn phải nhập đúng định dạng email.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTennv.Text))
            {
                MessageBox.Show("Bạn phải nhập tên nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTennv.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDiachi.Text))
            {
                MessageBox.Show("Bạn phải nhập địa chỉ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDiachi.Focus();
                return;
            }

            if (!rbQuantri.Checked && !rbNhanvien.Checked)
            {
                MessageBox.Show("Bạn phải chọn vai trò nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!rbHoatDong.Checked && !rbNgung.Checked)
            {
                MessageBox.Show("Bạn phải chọn tình trạng nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int role = rbQuantri.Checked ? 1 : 0;
            int status = rbHoatDong.Checked ? 1 : 0;

            DTO_NhanVien nv = new DTO_NhanVien(txtEmail.Text, txtTennv.Text, txtDiachi.Text, role, status);

            if (busNhanVien.InsertNhanVien(nv))
            {
                MessageBox.Show("Thêm thành công.");
                ResetValues();
                LoadGridview_NhanVien();
                SendMail(nv.EmailNV);
            }
            else
            {
                MessageBox.Show("Thêm không thành công.");
            }
        }

        private void DgvNhanvien_Click(object sender, EventArgs e)
        {
            if (dgvNhanvien.Rows.Count > 1)
            {
                btnLuu.Enabled = false;
                txtTennv.Enabled = true;
                txtDiachi.Enabled = true;
                rbQuantri.Enabled = true;
                rbNhanvien.Enabled = true;
                rbHoatDong.Enabled = true;
                rbNgung.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;

                txtEmail.Text = dgvNhanvien.CurrentRow.Cells["email"].Value.ToString();
                txtTennv.Text = dgvNhanvien.CurrentRow.Cells["TenNv"].Value.ToString();
                txtDiachi.Text = dgvNhanvien.CurrentRow.Cells["diaChi"].Value.ToString();
                rbQuantri.Checked = int.Parse(dgvNhanvien.CurrentRow.Cells["vaiTro"].Value.ToString()) == 1;
                rbNhanvien.Checked = !rbQuantri.Checked;
                rbHoatDong.Checked = int.Parse(dgvNhanvien.CurrentRow.Cells["tinhTrang"].Value.ToString()) == 1;
                rbNgung.Checked = !rbHoatDong.Checked;
            }
            else
            {
                MessageBox.Show("Bảng không tồn tại dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTennv.Text) || string.IsNullOrWhiteSpace(txtDiachi.Text))
            {
                MessageBox.Show("Bạn phải nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int role = rbQuantri.Checked ? 1 : 0;
            int status = rbHoatDong.Checked ? 1 : 0;

            DTO_NhanVien nv = new DTO_NhanVien(txtEmail.Text, txtTennv.Text, txtDiachi.Text, role, status);

            if (MessageBox.Show("Bạn có chắc muốn chỉnh sửa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (busNhanVien.UpdateNhanVien(nv))
                {
                    MessageBox.Show("Sửa thành công.");
                    ResetValues();
                    LoadGridview_NhanVien();
                }
                else
                {
                    MessageBox.Show("Sửa không thành công.");
                }
            }
            else
            {
                ResetValues();
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;

            if (MessageBox.Show("Bạn có chắc muốn xóa dữ liệu?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (busNhanVien.DeleteNhanVien(email))
                {
                    MessageBox.Show("Xóa dữ liệu thành công.");
                    ResetValues();
                    LoadGridview_NhanVien();
                }
                else
                {
                    MessageBox.Show("Xóa không thành công.");
                }
            }
        }

        private void BtnTimkiem_Click(object sender, EventArgs e)
        {
            string tenNhanvien = txttimKiem.Text;
            DataTable ds = busNhanVien.SearchNhanVien(tenNhanvien);

            if (ds.Rows.Count > 0)
            {
                dgvNhanvien.DataSource = ds;
                dgvNhanvien.Columns[0].HeaderText = "Email";
                dgvNhanvien.Columns[1].HeaderText = "Tên Nhân Viên";
                dgvNhanvien.Columns[2].HeaderText = "Địa chỉ";
                dgvNhanvien.Columns[3].HeaderText = "Vai Trò";
                dgvNhanvien.Columns[4].HeaderText = "Tình Trạng";
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            txttimKiem.Text = "Nhập tên nhân viên";
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
            LoadGridview_NhanVien();
        }
    }
}