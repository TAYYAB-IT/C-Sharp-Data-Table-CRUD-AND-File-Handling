using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace App9
{
    public partial class Form1 : Form
    {
        private DataTable table = new DataTable();
        private int count = 0;

        public Form1()
        {
            InitializeComponent();
            table.Columns.Add("Sr#");
            table.Columns.Add("Roll#");
            table.Columns.Add("Name");
            table.Columns.Add("Age");
            records_view.DataSource = table;
            this.readfile();
            records_view.Refresh();
        }
        //After Editing
        public Form1(DataTable table)
        {
            InitializeComponent();
            this.table = table;
            DataRow last_row = this.table.Rows[table.Rows.Count - 1];
            this.count = Int32.Parse(last_row["Sr#"].ToString());
            records_view.DataSource = this.table;
            this.writefile();
            records_view.Refresh();
        }
        //Insertion
        private void add_record_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(Roll_No.Text) && !String.IsNullOrWhiteSpace(Name.Text) && !String.IsNullOrWhiteSpace(Age.Text))
            {
                if (!chk_roll_exist(Roll_No.Text))
                {
                    
                    DataRow record = table.NewRow();
                    record["Sr#"] = ++count;
                    record["Roll#"] = Roll_No.Text;
                    record["Name"] = Name.Text;
                    record["Age"] = Age.Text;
                    this.table.Rows.Add(record);
                    this.writefile();
                    Roll_No.Text = Name.Text = Age.Text = "";
                    records_view.Refresh();
                }
                else
                {
                    MessageBox.Show($"This Roll# {Roll_No.Text} is already Taken");
                }
            }
            else
            {
                MessageBox.Show("Please Fill Out All Requirements!");
            }
        }
        //Check Either a Roll# Exist
        public bool chk_roll_exist(string roll)
        {

            foreach (DataRow row in table.Rows)
            {
                if (row["Roll#"].ToString() == roll)
                {

                    return true;
                }
            }
            return false;
        }
        //Edit an Record
        private void edit_record_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(Roll_No.Text))
            {
                if (chk_roll_exist(Roll_No.Text))
                {
                    Form2 f2 = new Form2(this.table, Roll_No.Text);
                    this.Hide();
                    f2.Show();
                }
                else
                {
                    MessageBox.Show($"Record No Exist to Edit with Roll# {Roll_No.Text} !");
                }


            }
            else
            {
                MessageBox.Show("Enter Roll No to Edit !");
            }
        }
        //Delete Record
        private void delete_record_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(Roll_No.Text))
            {
                if (chk_roll_exist(Roll_No.Text))
                {
                    int sr = 0;
                    foreach (DataRow row in this.table.Rows)
                    {
                        if (row["Roll#"].ToString() == Roll_No.Text)
                        {
                            sr = Int32.Parse(row["Sr#"].ToString());
                            MessageBox.Show($"Deleted!\nSr#{row["Sr#"]}\n Roll#{row["Roll#"]} \nName::{row["Name"]}\nAge::{row["Age"]}");
                            this.table.Rows.Remove(row);
                            this.count--;
                            break;
                        }
                    }

                    foreach (DataRow row in this.table.Rows)
                    {
                        if (Int32.Parse(row["Sr#"].ToString()) == sr + 1)
                        {
                            row["Sr#"] = sr;
                            sr++;
                        }
                    }
                    this.writefile();
                    records_view.Refresh();
                    Roll_No.Text = "";

                }
                else
                {
                    MessageBox.Show($" No Such Record Exist with Roll# {Roll_No.Text} !");
                }
            }
            else
            {
                MessageBox.Show("Kindly, Enter Roll# To Delete Record!");
            }
        }
        //Write Data in file
        public void writefile(){
            string fpath = Directory.GetCurrentDirectory() + @"\DataFile.txt";
            if (File.Exists(fpath))
            {
                File.Delete(fpath);
            }
            
                Stream stream =File.Open(fpath, FileMode.Create);

                using (StreamWriter sw = new StreamWriter(stream))
                {
                    foreach (DataRow row in this.table.Rows)
                    {
                        sw.WriteLine(row["Sr#"].ToString());
                        sw.WriteLine(row["Roll#"].ToString());
                        sw.WriteLine(row["Name"].ToString());
                        sw.WriteLine(row["Age"].ToString());
                    }
                }
                
        }
        //Read Data From File
        public void readfile()
        {
            string fpath = Directory.GetCurrentDirectory() + @"\DataFile.txt";
            if (File.Exists(fpath))
            {
                Stream stream = File.Open(fpath,FileMode.Open);
                using (StreamReader sr = new StreamReader(stream))
                {
                    while (!sr.EndOfStream)
                    {
                        DataRow record = this.table.NewRow();
                        record["Sr#"]=sr.ReadLine();
                        record["Roll#"]=sr.ReadLine();
                        record["Name"] = sr.ReadLine();
                        record["Age"] = sr.ReadLine();
                        this.table.Rows.Add(record);
                        this.count++;
                    }
                   
                }
            }
        }
    }
}
