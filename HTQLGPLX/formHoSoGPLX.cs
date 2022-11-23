﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace HTQLGPLX
{
    public partial class formHoSoGPLX : Form
    {
        public formHoSoGPLX()
        {
            InitializeComponent();
        }

        private void dataGridViewHSGPLX_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex == this.dataGridViewHSGPLX.NewRowIndex)
            {
                return;
            }
            if (e.ColumnIndex == this.dataGridViewHSGPLX.Columns["STT"].Index)
            {
                e.Value = e.RowIndex + 1;
            }
        }

        private void textBoxSearch_Enter(object sender, EventArgs e)
        {
            if (textBoxSearch.Text == "Search")
            {
                textBoxSearch.Text = null;
                textBoxSearch.ForeColor = Color.Black;
            }
        }

        private void textBoxSearch_Leave(object sender, EventArgs e)
        {
            if (textBoxSearch.Text == "")
            {
                textBoxSearch.Text = "Search";
                textBoxSearch.ForeColor = Color.DarkGray;
            }
            if (textBoxSearch.Text == "Search")
            {
                formHoSo_Load(sender, e);
            }
        }

        MySqlConnection Conn = new MySqlConnection("Server = quanlygplx.mysql.database.azure.com; Database = qlgplx; UId = phuc; Pwd = @hutech123; Pooling=false;Character Set=utf8");
        internal void loadDataGridView()
        {
            try
            {
                //MySqlConnection Conn = new MySqlConnection(connstring);
                Conn.Open();
                string querry = "CALL proc_HoSoGPLX()";
                MySqlDataAdapter adap = new MySqlDataAdapter(querry, Conn);
                MySqlCommandBuilder cmd = new MySqlCommandBuilder(adap);
                DataTable dtbl = new DataTable();
                adap.Fill(dtbl);
                dataGridViewHSGPLX.DataSource = dtbl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Conn.Close();
            }
        }
        
        private void formHoSo_Load(object sender, EventArgs e)
        {
            loadDataGridView();
            ToolTip toolTipTxtBoxUsername = new ToolTip();
            toolTipTxtBoxUsername.SetToolTip(textBoxSearch, "Tìm kiếm theo mã giấy phép lái xe");
            ToolTip toolTipButtonSearch = new ToolTip();
            toolTipButtonSearch.SetToolTip(buttonSearch, "Tìm kiếm");
            ToolTip toolTipButtonAdd = new ToolTip();
            toolTipButtonAdd.SetToolTip(buttonAdd, "Thêm mới");
            ToolTip toolTipButtonEdit = new ToolTip();
            toolTipButtonEdit.SetToolTip(buttonEdit, "Sửa");
            ToolTip toolTipButtonDelete = new ToolTip();
            toolTipButtonDelete.SetToolTip(buttonDelete, "Xóa");
            ToolTip toolTipButtonLogout = new ToolTip();
            toolTipButtonLogout.SetToolTip(buttonLogout, "Đăng xuất");
            ToolTip toolStripMenuHeThong = new ToolTip();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Conn.Open();
                string querry = "CALL proc_TimHoSoGPLX('" + textBoxSearch.Text + "')";
                MySqlDataAdapter adap = new MySqlDataAdapter(querry, Conn);
                MySqlCommandBuilder cmd = new MySqlCommandBuilder(adap);
                DataTable dtbl = new DataTable();
                adap.Fill(dtbl);
                if (textBoxSearch.Text == "Search")
                {
                    MessageBox.Show("Vui lòng điền thông tin cần tìm kiếm", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (dtbl.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy hồ sơ GPLX có mã " + textBoxSearch.Text, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    formHoSo_Load(sender, e);
                }
                else
                {
                    dataGridViewHSGPLX.DataSource = dtbl;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Conn.Close();
            }
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất tài khoản?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
                formLogin formLogin = new formLogin();
                formLogin.Show();
            }
            else if (dialogResult == DialogResult.No)
            {
                this.Show();
            }
        }

        private void buttonThemHoSo_Click(object sender, EventArgs e)
        {
            formThemHSGPLX formThemTT = new formThemHSGPLX();
            formThemTT.Show();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
           string maGPLX = dataGridViewHSGPLX.CurrentRow.Cells["MaGPLX"].Value.ToString();
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xoá hồ sơ GPLX có mã " + maGPLX + " không?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (maGPLX == "")
            {
                MessageBox.Show("Vui lòng chọn hồ sơ GPLX cần xoá", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    Conn.Open();
                    string querry = "CALL proc_XoaHoSoGPLX('"+maGPLX+"')";
                    MySqlDataAdapter adap = new MySqlDataAdapter(querry, Conn);
                    MySqlCommandBuilder cmd = new MySqlCommandBuilder(adap);
                    DataTable dtbl = new DataTable();
                    adap.Fill(dtbl);
                    MessageBox.Show("Xoá hồ sơ GPLX có mã " + maGPLX + " thành công", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    formHoSo_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Conn.Close();
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                this.Show();
            }
        }

        private void toolStripMenuLogout_Click(object sender, EventArgs e)
        {
            buttonLogout_Click(sender, e);
        }

        private void toolStripMenuExit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (dialogResult == DialogResult.No)
            {
                this.Show();
            }
        }

        private void toolStripMenuQLViPham_Click(object sender, EventArgs e)
        {
            formQLyViPham formQLyViPham = new formQLyViPham();
            formQLyViPham.Show();
            this.Close();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            string maGPLX = dataGridViewHSGPLX.CurrentRow.Cells["MaGPLX"].Value.ToString();
            if (maGPLX == "")
            {
                MessageBox.Show("Vui lòng chọn hồ sơ GPLX cần sửa", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                formEditHoSo formEditHoSo = new formEditHoSo();
                formEditHoSo.Show();
            }
        }

        private void toolStripMenuThem_Click(object sender, EventArgs e)
        {
            formThemHSGPLX formThemTT = new formThemHSGPLX();
            formThemTT.Show();
        }

        private void toolStripMenuSua_Click(object sender, EventArgs e)
        {
            string maGPLX = dataGridViewHSGPLX.CurrentRow.Cells["MaGPLX"].Value.ToString();
            if (maGPLX == "")
            {
                MessageBox.Show("Vui lòng chọn hồ sơ GPLX cần sửa", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                formEditHoSo formEditHoSo = new formEditHoSo();
                formEditHoSo.Show();
            }
        }
    }
}
