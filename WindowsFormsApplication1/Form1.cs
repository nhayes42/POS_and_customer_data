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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        double valueOfProduct_A = 20.0;
        double valueOfProduct_B = 15.5 ;

        int Product_A = 0;
        int Product_B = 0;

        public void InitializeValues()
        {
            //read from inventory storage file 
            FileStream inventory = new FileStream(@"c:\codetemp\inventory.txt", FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
            using (StreamReader sr = new StreamReader(inventory))
            {
                string line = sr.ReadLine();
                if (line != null) { Product_A = int.Parse(line);}
                
                line = sr.ReadLine();
                if (line != null) { Product_B = int.Parse(line);}
            }
            inventory.Close();
        }

        public int parseCustomers()
        {
            int numberOfCustomers = 0;
            StringBuilder customerList = new StringBuilder();

            using (FileStream salesTable = new FileStream(@"c:\codetemp\SalesTable.csv", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(salesTable))
                {
                    sr.ReadLine(); //Move past the first line with headers 
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        int x = line.IndexOf(",") + 1;
                        for (int k = 1; k < 2; k++) //sets name to the the number of fields that k goes up to (includes commas)
                        {
                            x = line.IndexOf(",", x + 1);
                        }
                        string name = line.Remove(x+1);
                        //Console.Out.WriteLine(name);
                        
                        if (customerList.ToString().Contains(name) != true)
                        {
                            numberOfCustomers++;
                            customerList.Append(name);
                        }
                    }
                }
            }
            return numberOfCustomers;
        }

        public void FormUpdate()
        {
            //write to customer and sales .csv file

            //write to value storage file
            FileStream inventory = new FileStream(@"c:\codetemp\inventory.txt", FileMode.Truncate, FileAccess.ReadWrite, FileShare.Read);
            using (StreamWriter sw = new StreamWriter(inventory))
            {
                sw.WriteLine(Product_A.ToString());
                sw.WriteLine(Product_B.ToString());
            }
            inventory.Close();
            //Update GUI
            label4.Text = Product_A.ToString();
            label5.Text = Product_B.ToString();

            int numberOfCustomers = parseCustomers();
            label8.Text = numberOfCustomers.ToString();
        } 

        public struct Customer
        {
            public string first;
            public string last;         
            public string phone;
            public string email;

            public Customer(string first, string last, string phone, string email)
            {
                this.first = first;
                this.last = last;
                this.phone = phone;
                this.email = email;
            }

        }

        private void writeCustomerData(Customer info, int product_A_sold, int product_B_sold) // Customer struct is first, last, phone, email
        {
            using (FileStream salesTable = new FileStream(@"c:\codetemp\SalesTable.csv", FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                using (StreamWriter sw = new StreamWriter(salesTable))
                {
                    double valueOfsale = valueOfProduct_A * product_A_sold + valueOfProduct_B * product_B_sold;
                    string line = info.first + ", " + info.last + ", " + info.phone + ", " + info.email + ", "
                                + product_A_sold.ToString() + ", " + product_B_sold.ToString() + ", " + valueOfsale.ToString();
                    sw.WriteLine(line);                    
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
            InitializeValues();
            FormUpdate();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Product_A = Product_A + (int)numericUpDown1.Value;
            Product_B = Product_B + (int)numericUpDown2.Value;
            FormUpdate();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((0 < (int)numericUpDown1.Value) || (0 < (int)numericUpDown2.Value))
            {
                if ((Product_A >= (int)numericUpDown1.Value) && (Product_B >= (int)numericUpDown2.Value))
                {
                    if ((textBox1.Text != "") && (textBox2.Text != "") && (textBox3.Text != "") && (textBox4.Text != ""))
                    {
                        Product_A = Product_A - (int)numericUpDown1.Value;
                        Product_B = Product_B - (int)numericUpDown2.Value;

                        Customer customerInfo = new Customer(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                        writeCustomerData(customerInfo, (int)numericUpDown1.Value, (int)numericUpDown2.Value);

                        FormUpdate();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Customer info cannot be blank");
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Insufficient stock, sale canceled.");
                }
            } 
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
