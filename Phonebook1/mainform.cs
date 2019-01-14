using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Phonebook1
{ 
    public partial class frmphonebook : Form
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd1 = new SqlCommand();
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();
        CurrencyManager cr;
       
        public frmphonebook()
        {
            InitializeComponent();
        }

        private void frmphonebook_Load(object sender, EventArgs e)
        {
            conn.ConnectionString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\tellbook.mdf;Integrated Security=True;User Instance=True";
            conn.Open();
            fillgrid("");

        }

        void fillgrid(string s){
            if(s=="")
                s="select * from tbl_tell";

            cmd1.CommandText = s;
            cmd1.Connection = conn;
            da.SelectCommand = cmd1;
            ds.Clear();
            da.Fill(ds, "t1");
            dgv1.DataBindings.Clear();
            dgv1.DataBindings.Add("datasource", ds, "t1");

            txtname.DataBindings.Clear();
            txtname.DataBindings.Add("text", ds, "t1.Firstname");
            txtfamily.DataBindings.Clear();
            txtfamily.DataBindings.Add("text", ds, "t1.lastname");
            txttell.DataBindings.Clear();
            txttell.DataBindings.Add("text", ds, "t1.phone");
            txtaddress.DataBindings.Clear();
            txtaddress.DataBindings.Add("text", ds, "t1.address");
            cr = (CurrencyManager) this.BindingContext[ds, "t1"];

        }

        private void btnnext_Click(object sender, EventArgs e)
        {
            cr.Position++;
            dgv1.CurrentCell = dgv1.Rows[cr.Position].Cells[0];
        }

        private void btnpre_Click(object sender, EventArgs e)
        {
            cr.Position--;
        }

        private void btnfirst_Click(object sender, EventArgs e)
        {
            cr.Position = 0;
        }

        private void btnlast_Click(object sender, EventArgs e)
        {
            cr.Position = cr.Count;
        }

        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(e.RowIndex.ToString() + " : " + e.ColumnIndex.ToString());
            cr.Position = e.RowIndex;
        }

        private void btnnew_Click(object sender, EventArgs e)
        {
            txtname.Text = "";
            txtfamily.Text = "";
            txttell.Text = "";
            txtaddress.Text = "";
            txtname.Focus();
            btnnew.Enabled = false;
            btnsave.Enabled = true;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            SqlCommand c1 = new SqlCommand();
            c1.CommandText="insert into tbl_tell values(@p1,@p2,@p3,@p4)";
            c1.Parameters.AddWithValue("p1", txtname.Text);
            c1.Parameters.AddWithValue("p2", txtfamily.Text);
            c1.Parameters.AddWithValue("p3", txttell.Text);
            c1.Parameters.AddWithValue("p4", txtaddress.Text);
            c1.Connection = conn;
            c1.ExecuteNonQuery();
            btnsave.Enabled = false;
            btnnew.Enabled = true;
            fillgrid("");
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            SqlCommand c1 = new SqlCommand();
            c1.CommandText = "delete from tbl_tell where phone=@p1";
            c1.Parameters.AddWithValue("p1", txttell.Text);
            c1.Connection = conn;
            c1.ExecuteNonQuery();
                  fillgrid("");
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            string s;
            s="select * from tbl_tell where " + cmbfield.Text + " like '" + txtfind.Text + "%'";
          //  MessageBox.Show(s);
            fillgrid(s);
        }

        private void txtfind_TextChanged(object sender, EventArgs e)
        {
            btnsearch_Click(null, null);
        }

    }
}
