﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HTQLGPLX
{
    public partial class formEditHoSo : Form
    {
        public formEditHoSo()
        {
            InitializeComponent();
        }

        formHoSoGPLX obj = (formHoSoGPLX)Application.OpenForms["formHoSoGPLX"];
        SqlConnection conn = new SqlConnection(@"Data Source=35.222.184.247;Initial Catalog=QLGPLX;User ID=sqlserver;Password=admin");

        private void loadComboBoxMaHang()
        {
            try
            {
                String querry = "SELECT MaHang FROM HangGPLX";
                SqlDataAdapter sda = new SqlDataAdapter(querry, conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                comboBoxMaHang.DisplayMember = "MaHang";
                comboBoxMaHang.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void loadComboBoxTTSatHach()
        {
            try
            {
                String querry = "SELECT TenTT FROM TrungTamSatHach";
                SqlDataAdapter sda = new SqlDataAdapter(querry, conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                comboBoxTTSatHach.DisplayMember = "TenTT";
                comboBoxTTSatHach.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void formEditHoSo_Load(object sender, EventArgs e)
        {
            string maGPLX = obj.dataGridViewHSGPLX.CurrentRow.Cells["MaGPLX"].Value.ToString();
            try
            {
                String querry = "select l.SoCCCD, l.HoLot + l.Ten [HoTen], hs.MaGPLX, hs.MaHang, hs.NgayCapGPLX, hs.NgayHetHanGPLX, hs.DiemLT, hs.DiemTH, tt.TenTT from HoSoGPLX hs, TrungTamSatHach tt, LyLich l where hs.SoCCCD = l.SoCCCD and hs.MaTT = tt.MaTT and hs.MaGPLX = '" + maGPLX + "'";
                SqlDataAdapter sda = new SqlDataAdapter(querry, conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                labelSOCCCD.Text = dt.Rows[0]["SoCCCD"].ToString();
                labelHoTen.Text = dt.Rows[0]["HoTen"].ToString();
                textBoxMaGPLX.Text = dt.Rows[0]["MaGPLX"].ToString();
                comboBoxMaHang.Text = dt.Rows[0]["MaHang"].ToString();
                dateTimePickerNgayCap.Text = dt.Rows[0]["NgayCapGPLX"].ToString();
                dateTimePickerNgayHetHan.Text = dt.Rows[0]["NgayHetHanGPLX"].ToString();
                textBoxDiemLT.Text = dt.Rows[0]["DiemLT"].ToString();
                textBoxDiemTH.Text = dt.Rows[0]["DiemTH"].ToString();
                comboBoxTTSatHach.Text = dt.Rows[0]["TenTT"].ToString();
                loadComboBoxMaHang();
                loadComboBoxTTSatHach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string maGPLX = obj.dataGridViewHSGPLX.CurrentRow.Cells["MaGPLX"].Value.ToString();
                string maHang = comboBoxMaHang.Text;
                string ngayCapGPLX = dateTimePickerNgayCap.Value.ToString("yyyy-MM-dd");
                string ngayHetHanGPLX = dateTimePickerNgayHetHan.Value.ToString("yyyy-MM-dd");
                string diemLT = textBoxDiemLT.Text;
                string diemTH = textBoxDiemTH.Text;

                String querry1 = "SELECT MaTT FROM TrungTamSatHach WHERE TenTT LIKE N'" + comboBoxTTSatHach.Text + "'";
                SqlDataAdapter sda1 = new SqlDataAdapter(querry1, conn);
                DataTable dt1 = new DataTable();
                sda1.Fill(dt1);
                string maTT = dt1.Rows[0][0].ToString();

                string querry = "update HoSoGPLX set MaHang = '" + maHang + "', NgayCapGPLX = '" + ngayCapGPLX + "', NgayHetHanGPLX = '" + ngayHetHanGPLX + "', DiemLT = '" + diemLT + "', DiemTH = '" + diemTH + "', MaTT = '" + maTT + "' where MaGPLX = '" + maGPLX + "'";
                SqlCommand cmd = new SqlCommand(querry, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Cập nhật thông tin thành công", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                obj.loadDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}

