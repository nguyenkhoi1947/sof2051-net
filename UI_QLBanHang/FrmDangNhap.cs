using System;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using BUS_QLBanHang;
using DTO_QLBanHang;

namespace UI_QLBanHang
{
    public partial class FrmDangNhap : Form
    {
        private readonly BUS_NhanVien busNhanVien = new BUS_NhanVien();
        private static readonly Random RandomGenerator = new Random();

        public string Email { get; set; }
        public string MatKhau { get; set; }
        public string VaiTro { get; set; }

        public FrmDangNhap()
        {
            InitializeComponent();
        }

        private void FrmDangNhap_Load(object sender, EventArgs e)
        {
            FrmMain.session = 0;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Btndangnhap_Click(object sender, EventArgs e)
        {
            var nv = new DTO_NhanVien
            {
                EmailNV = txtemail.Text,
                MatKhau = encryption(txtmatkhau.Text)
            };

            if (busNhanVien.NhanVienDangNhap(nv))
            {
                FrmMain.mail = nv.EmailNV;
                DataTable dt = busNhanVien.VaiTroNhanVien(nv.EmailNV);
                VaiTro = dt.Rows[0][0].ToString();
                MessageBox.Show("Đăng nhập thành công");
                FrmMain.session = 1;
                Close();
            }
            else
            {
                MessageBox.Show("Đăng nhập không thành công, kiểm tra lại email hoặc mật khẩu");
                txtemail.Clear();
                txtmatkhau.Clear();
                txtemail.Focus();
            }
        }

        public string encryption(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            encrypt = md5.ComputeHash(encode.GetBytes(password));
            StringBuilder encryptdata = new StringBuilder();
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());
            }
            return encryptdata.ToString();
        }

        private void BtnQuenmk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtemail.Text))
            {
                if (busNhanVien.NhanVienQuenMatKhau(txtemail.Text))
                {
                    string newPassword = GenerateRandomPassword();
                    string encryptedPassword = encryption(newPassword);

                    busNhanVien.TaoMatKhau(txtemail.Text, encryptedPassword);
                    SendMail(txtemail.Text, newPassword);
                }
                else
                {
                    MessageBox.Show("Email không tồn tại, vui lòng nhập lại email!");
                }
            }
            else
            {
                MessageBox.Show("Bạn cần nhập email để nhận thông tin phục hồi mật khẩu!");
                txtemail.Focus();
            }
        }

        private string GenerateRandomPassword()
        {
            var builder = new StringBuilder();
            builder.Append(GenerateRandomString(4, true));
            builder.Append(RandomGenerator.Next(1000, 9999));
            builder.Append(GenerateRandomString(2, false));
            return builder.ToString();
        }

        private string GenerateRandomString(int size, bool lowerCase)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(RandomGenerator.Next(65, 91));
                builder.Append(ch);
            }
            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        private void SendMail(string email, string newPassword)
        {
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 25)
                {
                    Credentials = new NetworkCredential("sender@gmail.com", "password"),
                    EnableSsl = true
                };

                var message = new MailMessage
                {
                    From = new MailAddress("sender@gmail.com"),
                    Subject = "Bạn đã sử dụng tính năng quên mật khẩu",
                    Body = $"Chào anh/chị. Mật khẩu mới truy cập phần mềm là {newPassword}"
                };

                message.To.Add(email);
                client.Send(message);

                MessageBox.Show("Một email phục hồi mật khẩu đã được gửi tới bạn!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi email: {ex.Message}");
            }
        }
    }
}
