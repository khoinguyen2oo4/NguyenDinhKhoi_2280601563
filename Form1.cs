using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace frmQuanLySinhVien
{
    public partial class Form1 : Form
    {
        private Model1 db = new Model1();
        public Form1()
        {
            InitializeComponent();
        }
        public class Student
        {
            public string StudentID { get; set; }
            public string FullName { get; set; }
            public float AverageScore { get; set; }
            public int FacultyID { get; set; }

            public virtual Faculty Faculty { get; set; }
        }
        public class Faculty
        {
            public int FacultyID { get; set; }
            public string FacultyName { get; set; }
            public virtual ICollection<Student> Students { get; set; }
        }
        public class Model1 : DbContext
        {
            public Model1() : base("name=QuanLySinhVienDB") { }

            public DbSet<Student> Students { get; set; }
            public DbSet<Faculty> Faculties { get; set; }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadKhoa();
            LoadSinhVien();
        }
      
        private void LoadKhoa()
        {
            var khoaList = db.Faculties.ToList();
            cboKhoa.DataSource = khoaList;
            cboKhoa.DisplayMember = "FacultyName";
            cboKhoa.ValueMember = "FacultyID";
        }

        private void LoadSinhVien()
        {
            var query = db.Students.Select(sv => new
            {
                sv.StudentID,
                sv.FullName,
                FacultyName = sv.Faculty.FacultyName,
                sv.AverageScore
            }).ToList();

            dgvQuanLyThongTinSinhVien.DataSource = query;
            dgvQuanLyThongTinSinhVien.Columns["StudentID"].HeaderText = "Mã Số SV";
            dgvQuanLyThongTinSinhVien.Columns["FullName"].HeaderText = "Họ Tên";
            dgvQuanLyThongTinSinhVien.Columns["FacultyName"].HeaderText = "Tên Khoa";
            dgvQuanLyThongTinSinhVien.Columns["AverageScore"].HeaderText = "Điểm TB";
        }
        private void txtMaSoSV_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtHoTen_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtDiemTB_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var student = new Student
            {
                StudentID = txtMaSoSV.Text,
                FullName = txtHoTen.Text,
                AverageScore = float.Parse(txtDiemTB.Text),
                FacultyID = (int)cboKhoa.SelectedValue
            };

            db.Students.Add(student);
            db.SaveChanges();
            LoadSinhVien();
            MessageBox.Show("Thêm sinh viên thành công!");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            var student = db.Students.Find(txtMaSoSV.Text);
            if (student != null)
            {
                student.FullName = txtHoTen.Text;
                student.AverageScore = float.Parse(txtDiemTB.Text);
                student.FacultyID = (int)cboKhoa.SelectedValue;

                db.SaveChanges();
                LoadSinhVien();
                MessageBox.Show("Sửa thông tin thành công!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var student = db.Students.Find(txtMaSoSV.Text);
            if (student != null)
            {
                db.Students.Remove(student);
                db.SaveChanges();
                LoadSinhVien();
                MessageBox.Show("Xóa sinh viên thành công!");
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {          
                this.Close();
            
        }

        private void dgvQuanLyThongTinSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvQuanLyThongTinSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvQuanLyThongTinSinhVien.Rows[e.RowIndex];
                txtMaSoSV.Text = row.Cells["StudentID"].Value.ToString();
                txtHoTen.Text = row.Cells["FullName"].Value.ToString();
                cboKhoa.Text = row.Cells["FacultyName"].Value.ToString();
                txtDiemTB.Text = row.Cells["AverageScore"].Value.ToString();
            }
        }
    }
}
