using Lab06.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab06
{
    public partial class frm_QuanLySach : Form
    {
        QuanLySachDB context = new QuanLySachDB();
        public frm_QuanLySach()
        {
            InitializeComponent();
            this.dgvSach.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dgvSach_CellContentClick);
        }

        private void setNull()
        {
            txtMaSach.Text = "";
            txtTenSach.Text = "";
            txtNamXB.Text = "";
            cmbTheLoai.SelectedItem = cmbTheLoai.FindStringExact("Khoa Học");
        }
        private void frm_QuanLySach_Load(object sender, EventArgs e)
        {
            setNull();
            //setGridViewStyle(dgvSach);
            List<Sach> listSach = context.Saches.ToList();
            List<LoaiSach> listLoai = context.LoaiSaches.ToList();  
            BindGrid(listSach);
            fillTheLoai(listLoai);
        }

        private void BindGrid(List<Sach> listSach)
        {
            dgvSach.Rows.Clear();
            
            foreach (var item in listSach)
            {
                int index = dgvSach.Rows.Add();
                dgvSach.Rows[index].Cells[0].Value = item.MaSach;
                dgvSach.Rows[index].Cells[1].Value = item.TenSach;
                dgvSach.Rows[index].Cells[2].Value = item.NamXB;
                dgvSach.Rows[index].Cells[3].Value = item.LoaiSach.TenLoai;
            }
        }
        //public void setGridViewStyle(DataGridView dgview)
        //{
        //    dgview.BorderStyle = BorderStyle.None;
        //    dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
        //    dgview.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        //    dgview.BackgroundColor = Color.White;
        //    dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        //}

        private void dgvSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && e.RowIndex < dgvSach.Rows.Count - 1)
            {
                DataGridViewRow rows = dgvSach.Rows[e.RowIndex];
                txtMaSach.Text = rows.Cells[0].Value.ToString();
                txtTenSach.Text = rows.Cells[1].Value.ToString();
                txtNamXB.Text = rows.Cells[2].Value.ToString();
                cmbTheLoai.Text = rows.Cells[3].Value.ToString();
            }
            else
            {
                MessageBox.Show("Đối tượng chọn không hợp lệ!!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void fillTheLoai(List<LoaiSach> listLoaiSach)
        {
            this.cmbTheLoai.DataSource = listLoaiSach;
            this.cmbTheLoai.ValueMember = "MaLoai";
            this.cmbTheLoai.DisplayMember = "TenLoai";
        }
        private void InsertUpdate(Sach s)
        {
            context.Saches.AddOrUpdate(s);
            context.SaveChanges();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaSach.Text == "" || txtTenSach.Text == "" || txtNamXB.Text == "")
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin!");
                }else if(txtMaSach.Text.Length != 6)
                {
                    MessageBox.Show("Mã sách phải có 6 kí tự!");
                }
                else
                {
                    Sach s = context.Saches.FirstOrDefault(f => f.MaSach == txtMaSach.Text);
                    if (s == null)
                    {
                        s = new Sach()
                        {
                            MaSach = txtMaSach.Text,
                            TenSach = txtTenSach.Text,
                            NamXB = int.Parse(txtNamXB.Text),
                            MaLoai = context.LoaiSaches.Where(f => f.TenLoai == cmbTheLoai.Text).Select(f => f.MaLoai).FirstOrDefault(),
                        };
                        InsertUpdate(s);
                        MessageBox.Show("Thêm thành công !", "Thông Báo", MessageBoxButtons.OK);
                        frm_QuanLySach_Load(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Thông tin đã có!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try { 
                Sach s = context.Saches.FirstOrDefault(f => f.MaSach == txtMaSach.Text);
                if (s != null)
                {
                    s.TenSach = txtTenSach.Text;
                    s.NamXB = int.Parse(txtNamXB.Text);
                    s.MaLoai = context.LoaiSaches.Where(f => f.TenLoai == cmbTheLoai.Text).Select(f => f.MaLoai).FirstOrDefault();
                    InsertUpdate(s);
                    MessageBox.Show("Sửa thành công !", "Thông Báo", MessageBoxButtons.OK);
                    frm_QuanLySach_Load(sender, e);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin sinh viên cần sửa!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Sach s = context.Saches.FirstOrDefault(f => f.MaSach == txtMaSach.Text);
                if (s != null)
                {
                    DialogResult dlg = MessageBox.Show("Bạn có muốn xoá thông tin này không?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(dlg == DialogResult.Yes)
                    {
                        context.Saches.Remove(s);
                        context.SaveChanges();
                        MessageBox.Show("Xoá thành công !", "Thông Báo", MessageBoxButtons.OK);
                        frm_QuanLySach_Load(sender, e);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin cần sửa!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtTimKiem.Text.ToLower(); // chuyển đổi về chữ thường
                foreach (DataGridViewRow row in dgvSach.Rows)
                {
                    // Kiểm tra nếu không phải là dòng mới (NewRow)
                    if (!row.IsNewRow)
                    {
                        bool isVisible = false; // Đánh dấu dòng có chứa nội dung tìm kiếm

                        // Duyệt qua tất cả các ô trong hàng
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText))
                            {
                                isVisible = true; // Nếu tìm thấy giá trị khớp, đặt là true
                                break;
                            }
                        }
                        // Đặt hiển thị dòng dựa trên kết quả tìm kiếm
                        row.Visible = isVisible;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
