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

namespace datagridview_app2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=emyo;Integrated Security=True");
        private void vericek(string komut)
        {
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(komut, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            //DataTable dt = new DataTable();
            //da.Fill(dt);
            //dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
            conn.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //dataGridView1.ColumnCount = 6;
            //dataGridView1.Columns[0].Name = "Id";
            //dataGridView1.Columns[1].Name = "Ad";
            //dataGridView1.Columns[2].Name = "Soyad";
            //dataGridView1.Columns[3].Name = "Doğum Tarihi";
            //dataGridView1.Columns[4].Name = "İl";
            //dataGridView1.Columns[5].Name = "İlçe";
            conn.Open();
            SqlCommand c1 = new SqlCommand("select * from iller", conn);
            SqlDataReader dr = c1.ExecuteReader();
            while (dr.Read())
            {
                cmbIller.Items.Add(dr["il_adi"].ToString());
            }
            conn.Close();
            vericek("select * from Kisiler");
            cmbIller.SelectionChangeCommitted += CmbIller_SelectionChangeCommitted;
        }

        private void CmbIller_SelectionChangeCommitted(object sender, EventArgs e)
        {
            conn.Open();
            cmbIlceler.Items.Clear();
            SqlCommand c2 = new SqlCommand("select a.ilce_adi from ilceler a inner join iller b on a.ilId = b.ilId where b.il_adi='"+(sender as ComboBox).SelectedItem as string+"'", conn);
            SqlDataReader dr2 = c2.ExecuteReader();
            while (dr2.Read())
            {
                cmbIlceler.Items.Add(dr2["ilce_adi"].ToString());
            }
            conn.Close();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            //int i = dataGridView1.Rows.Add();
            //i = 0;
            //dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString();
            //dataGridView1.Rows[i].Cells[1].Value = txtAd.Text;
            //dataGridView1.Rows[i].Cells[2].Value = txtSoyad.Text;
            //dataGridView1.Rows[i].Cells[3].Value = dtpDogumTarihi.Value;
            //dataGridView1.Rows[i].Cells[4].Value = cmbIller.GetItemText(cmbIller.SelectedItem);
            //dataGridView1.Rows[i].Cells[5].Value = cmbIlceler.GetItemText(cmbIlceler.SelectedItem);
            conn.Open();
            SqlCommand c3 = new SqlCommand("insert into Kisiler (ad,soyad,dogum_tarihi,il,ilce) values (@ad,@soyad,@dogum_tarihi,@il,@ilce)", conn);
            c3.Parameters.AddWithValue("@ad", txtAd.Text);
            c3.Parameters.AddWithValue("@soyad",txtSoyad.Text);
            c3.Parameters.AddWithValue("@dogum_tarihi",dtpDogumTarihi.Value);
            c3.Parameters.AddWithValue("@il",cmbIller.GetItemText(cmbIller.SelectedItem));
            c3.Parameters.AddWithValue("@ilce",cmbIlceler.GetItemText(cmbIlceler.SelectedItem));
            c3.ExecuteNonQuery();
            conn.Close();
            vericek("select * from Kisiler");
        }
    }
}
