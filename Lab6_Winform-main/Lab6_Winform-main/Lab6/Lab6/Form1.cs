using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Lab6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string connectionString = "server=PC829; database = RestaurantManagement; Integrated Security = true; "; // Tạo đối tượng kết nối
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            // Tạo đối tượng thực thi lệnh
            SqlCommand sqlCommand = sqlConnection.CreateCommand();

            // Thiết lập lệnh truy vấn cho đối tượng Command 
            string query = "SELECT ID, Name, Type FROM Category";
            sqlCommand.CommandText = query;
            // Mở kết nối tới cơ sở dữ liệu

            sqlConnection.Open();
            // Thực thi lệnh bằng phương thức ExcuteReader 

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            // Gọi hàm hiển thị dữ liệu lên màn hình
            this.DisplayCategory(sqlDataReader);

            // Đóng kết nối
            sqlConnection.Close();
        }

        private void DisplayCategory(SqlDataReader reader)
        { // Xóa tất cả các dòng hiện tại
            lvCategory.Items.Clear();
            // Đọc một dòng dữ liệu
            while (reader.Read())
            {
                // Tạo một dòng mới trong ListView
                ListViewItem item = new ListViewItem(reader["ID"].ToString());
                // Thêm dòng mới vào ListView
                lvCategory.Items.Add(item);
                // Bổ sung các thông tin khác cho ListViewItem
                item.SubItems.Add(reader["Name"].ToString());
                item.SubItems.Add(reader["Type"].ToString());
            }
        }


        private void lvCategory_Click(object sender, EventArgs e)
        {
            // Lấy dòng được chọn trong Listview ListViewItem item lvCategory.SelectedItems[0];
            // Hiển thị dữ liệu lên Textbox
            ListViewItem item = lvCategory.SelectedItems[0];
            txtID.Text = item.Text;
            txtName.Text = item.SubItems[1].Text;
            txtType.Text = item.SubItems[2].Text == "0" ? "Thức uống" : "Đồ ân";
            // Hiển thị nút cập nhật và xóa
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string connectionString = "server=PC829; database = RestaurantManagement; Integrated Security = true; ";
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            //Tạo đối tượng thực thi lệnh
            SqlCommand sqlCommand= sqlConnection.CreateCommand();
            //Thiết lập lệnh truy vấn cho đối tượng command
            sqlCommand.CommandText = "INSERT INTO Category(Name, [Type])" +
                "VALUES (N'" + txtName.Text + "'," + txtType.Text + ")";

            //Mở kết nối tới CSDL
            sqlConnection.Open();

            //thực thi lệnh bằng ExcuteReader
            int NumOfRowsEffected = sqlCommand.ExecuteNonQuery();

            //Dóng kết nối
            sqlConnection.Close();

            if(NumOfRowsEffected == 1)
            {
                MessageBox.Show("Thêm nhóm món ăn thành công");

                //Tải lại dữ liệu
                btnLoad.PerformClick();

                //Xóa các ô nhập
                txtName.Text = "";
                txtType.Text = "";
            }
            else
            {
                MessageBox.Show("Đã có lỗi xảy ra.Vui lòng thử lại");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Tạo đối tượng kết nối
            string connectionString = "server=PC829; database = RestaurantManagement; Integrated Security = true; ";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Tạo đối tượng thực thi lệnh
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            
            //Thiết lập lệnh truy vấn cho đối tượng Command
            sqlCommand.CommandText = "UPDATE Category SET Name = N'" + txtName.Text +
                                                        "', [Type] = " + txtType.Text +
                                                        " WHERE ID = " + txtID.Text ;

            //Mở kết nối tới CSDL
            sqlConnection.Open();

            //thực thi lệnh bằng ExcuteReader
            int NumOfRowsEffected = sqlCommand.ExecuteNonQuery();

            //Dóng kết nối
            sqlConnection.Close();

            if (NumOfRowsEffected == 1)
            {
                //Cập nhật lại dữ liệu trên listView
                ListViewItem item = lvCategory.SelectedItems[0];
                item.SubItems[1].Text = txtName.Text;
                item.SubItems[2].Text = txtType.Text;

                //Xóa các ô nhập
                txtID.Text = "";
                txtName.Text = "";
                txtType.Text = "";

                //Disable các nút xóa và cập nhật
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

                MessageBox.Show("Cập nhật nhóm món ăn thành công");
            }
            else
            {
                MessageBox.Show("Đã có lỗi xảy ra vui lòng thử lại");
            }    
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Tạo đối tượng kết nối
            string connectionString = "server=PC829; database = RestaurantManagement; Integrated Security = true; ";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Tạo đối tượng thực thi lệnh
            SqlCommand sqlCommand = sqlConnection.CreateCommand();

            //Thiết lập lệnh truy vấn
            sqlCommand.CommandText = "DELETE FROM Category " +
                "WHERE ID = " + txtID.Text;
            //Mở kết nối tới CSDL
            sqlConnection.Open();

            //thực thi lệnh bằng ExcuteReader
            int NumOfRowsEffected = sqlCommand.ExecuteNonQuery();

            //Dóng kết nối
            sqlConnection.Close();

            if (NumOfRowsEffected == 1)
            {
                //Cập nhật lại dữ liệu trên listView
                ListViewItem item = lvCategory.SelectedItems[0];
                lvCategory.Items.Remove(item);
                //Xóa các ô nhập
                txtID.Text = "";
                txtName.Text = "";
                txtType.Text = "";

                //Disable các nút xóa và cập nhật
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

                MessageBox.Show("Xóa nhóm món ăn thành công");
            }
            else
            {
                MessageBox.Show("Đã có lỗi xảy ra vui lòng thử lại");
            }
        }

        private void tsmDelete_Click(object sender, EventArgs e)
        {
           if(lvCategory.SelectedItems.Count > 0)
                btnDelete.PerformClick();
        }

        private void tsmViewFood_Click(object sender, EventArgs e)
        {

        }
    }
}
