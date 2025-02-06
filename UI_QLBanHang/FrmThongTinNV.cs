using System;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BUS_QLBanHang;

namespace UI_QLBanHang
{
    public partial class FrmThongTinNV : Form
    {
        private Thread th;
        private string stremail;
        private readonly BUS_NhanVien busNhanVien = new BUS_NhanVien();

        public FrmThongTinNV(string email)
        {
            InitializeComponent();
            stremail = email;
            txtemail.Text = stremail;
            txtemail.Enabled = false;
        }

        private void Btnthoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private string Encryption(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] encrypt = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder encryptData = new StringBuilder();
                foreach (byte b in encrypt)
                {
                    encryptData.Append(b.ToString("x2"));
                }
                return encryptData.ToString();
            }
        }

        private void Btndoimatkhau_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtmatkhaucu.Text))
            {
                MessageBox.Show("Bạn phải nhập mật khẩu cũ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtmatkhaucu.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtmatkhaumoi.Text))
            {
                MessageBox.Show("Bạn phải nhập mật khẩu mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtmatkhaumoi.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtmatkhaumoi2.Text))
            {
                MessageBox.Show("Bạn phải nhập lại mật khẩu mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtmatkhaumoi2.Focus();
                return;
            }
            if (txtmatkhaumoi2.Text.Trim() != txtmatkhaumoi.Text.Trim())
            {
                MessageBox.Show("Mật khẩu mới và xác nhận mật khẩu không khớp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtmatkhaumoi.Focus();
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn cập nhật mật khẩu?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string newPassword = Encryption(txtmatkhaumoi.Text);
                string currentPassword = Encryption(txtmatkhaucu.Text);

                if (busNhanVien.UpdateMatKhau(txtemail.Text, currentPassword, newPassword))
                {
                    FrmMain.profile = 1;
                    FrmMain.session = 0;
                    SendMail(stremail, txtmatkhaumoi2.Text);
                    MessageBox.Show("Cập nhật mật khẩu thành công, bạn cần đăng nhập lại.");
                    Close();
                }
                else
                {
                    MessageBox.Show("Mật khẩu cũ không đúng. Cập nhật mật khẩu không thành công.");
                    ClearPasswordFields();
                }
            }
            else
            {
                ClearPasswordFields();
            }
        }

        private void ClearPasswordFields()
        {
            txtmatkhaucu.Clear();
            txtmatkhaumoi.Clear();
            txtmatkhaumoi2.Clear();
        }

        private void SendMail(string email, string password)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 25)
                {
                    Credentials = new NetworkCredential("sender@gmail.com", "password"),
                    EnableSsl = true
                };

                MailMessage message = new MailMessage
                {
                    From = new MailAddress("sender@gmail.com"),
                    Subject = "Bạn đã thay đổi mật khẩu",
                    Body = "Chào bạn, mật khẩu truy cập phần mềm của bạn là " + password
                };
                message.To.Add(email);

                client.Send(message);
                MessageBox.Show("Email cập nhật mật khẩu đã được gửi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenNewForm()
        {
            Application.Run(new FrmDangNhap());
        }
    }
}