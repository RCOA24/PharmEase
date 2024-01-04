using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PharmacyManagementSystem.PharmacistUC
{
    public partial class UC_P_SellMedicine : UserControl
    {
        DataTable dt;
        function fn = new function();
        String query;
        DataSet ds;
        public UC_P_SellMedicine()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void UC_P_SellMedicine_Load(object sender, EventArgs e)
        {
            dt = new DataTable();
            dt.Columns.Add("Medicine ID", typeof(string));
            dt.Columns.Add("Medicine Name", typeof(string));
            dt.Columns.Add("Expiry Date", typeof(string));
            dt.Columns.Add("Price Per Unit", typeof(string));
            dt.Columns.Add("No. of Units", typeof(string));
            dt.Columns.Add("Total Price", typeof(string));
            guna2DataGridView1.DataSource = dt;
            listBoxMedicines.Items.Clear();
            query = "select mname from medic where eDate >= getdate() and quantity >'0'";
            ds = fn.getData(query);

            for (int i=0;i<ds.Tables[0].Rows.Count;i++)
            {
                listBoxMedicines.Items.Add(ds.Tables[0].Rows[i][0].ToString());
            }
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            UC_P_SellMedicine_Load(this, null);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            listBoxMedicines.Items.Clear();
            query = "select mname from medic where mname like '"+txtSearch.Text+"%' and eDate >= getdate() and quantity > '0'";
            ds = fn.getData(query);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                listBoxMedicines.Items.Add(ds.Tables[0].Rows[i][0].ToString());
            }
        }

        private void listBoxMedicines_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtNoOfUnits.Clear();
            String name = listBoxMedicines.GetItemText(listBoxMedicines.SelectedItem);

            txtMediName.Text = name;
            query = "select mid, eDate, perUnit from medic where mname = '"+name+"'";
            ds = fn.getData(query);
            txtMediId.Text = ds.Tables[0].Rows[0][0].ToString();
            txtExpireDate.Text = ds.Tables[0].Rows[0][1].ToString();
            txtPricePerUnit.Text = ds.Tables[0].Rows[0][2].ToString();
        }

        private void txtNoOfUnits_TextChanged(object sender, EventArgs e)
        {
            if (txtNoOfUnits.Text != "")
            {
                Int64 unitPrice = Int64.Parse(txtPricePerUnit.Text);
                Int64 noOfUnit = Int64.Parse(txtNoOfUnits.Text);
                Int64 totalamount = unitPrice * noOfUnit;
                txtTotalPrice.Text = totalamount.ToString();
            }
            else
            {
                txtTotalPrice.Clear();
            }
        }


  
        
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        protected int n, totalamount = 0;
        protected Int64 quantity, newQuantity;

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (txtMediId.Text != "")
            {
                query = "select quantity from medic where mid ='"+txtMediId.Text+"'";
                ds = fn.getData(query);

                quantity = Int64.Parse(ds.Tables[0].Rows[0][0].ToString()); //kunwari 50 ang quantity
                newQuantity = quantity - Int64.Parse(txtNoOfUnits.Text);    //50 - 5 = 45


                if (newQuantity >= 0)
                {
                    DataRow row = dt.NewRow();
                    //n = guna2DataGridView1.Rows.Add();
                    row["Medicine ID"] = txtMediId.Text;
                    row["Medicine Name"] = txtMediName.Text;
                    row["Expiry Date"] = txtExpireDate.Text;
                    row["Price Per Unit"] = txtPricePerUnit.Text;
                    row["No. of Units"] = txtNoOfUnits.Text;
                    row["Total Price"] = txtTotalPrice.Text;

                   
                    dt.Rows.Add(row);
                    totalamount = totalamount + int.Parse(txtTotalPrice.Text);
                    totalLabel.Text = "PH. " + totalamount.ToString();
               

                    query = "update medic set quantity = '"+newQuantity+"' where mid = '"+txtMediId.Text+"'";
                    fn.setData(query, "Medicine Added.");
                }
                else
                {
                    MessageBox.Show("Medicine is Out of Stock.\n Only "+quantity+" Left","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
               
                //UC_P_SellMedicine_Load(this, null);
                //clearAll();
            }
            else
            {
                MessageBox.Show("Select Medicine First.", "Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }      
        }

        int valueAmount;
        String valueId;
        protected Int64 noOfunit;

       

        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                valueAmount = int.Parse(guna2DataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                valueId = guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                noOfunit = Int64.Parse(guna2DataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
            }catch (Exception)
            { }    
        }

        

        private void btnRemove_Click(object sender, EventArgs e)
        {
          if(valueId != null)
            {
                try
                {
                    guna2DataGridView1.Rows.RemoveAt(this.guna2DataGridView1.SelectedRows[0].Index);
                }
                catch
                {

                }
                finally
                {
                    query = "select quantity from medic where mid = '"+valueId+"'";
                    ds = fn.getData(query);
                    quantity = Int64.Parse(ds.Tables[0].Rows[0][0].ToString());
                    newQuantity = quantity + noOfunit;

                    query = "update medic set quantity = '" + newQuantity + "' where mid  = '" + valueId + "'";
                    fn.setData(query,"Medicine Removed from the Cart.");
                    totalamount = totalamount - valueAmount;
                    totalLabel.Text = "Ph. "+ totalamount.ToString();
                }
                UC_P_SellMedicine_Load(this, null);
            }
        }

        private void btnPurchasePrint_Click(object sender, EventArgs e)
        {
            DGVPrinter print = new DGVPrinter();
            print.Title = "Medicine Bill";
            print.SubTitle = String.Format("Date: - {0}",DateTime.Now.Date);
            print.SubTitle = String.Format("Time: {0}",DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt"));
            print.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            print.PageNumbers = true;
            print.PageNumberInHeader = false;
            print.PorportionalColumns = true;
            print.HeaderCellAlignment = StringAlignment.Near;
            print.Footer = "Total Payable Amount : "+totalLabel.Text;
            print.FooterSpacing = 15;
            print.PrintDataGridView(guna2DataGridView1);

            totalamount = 0;
            totalLabel.Text = "Ph. 00";
            guna2DataGridView1.DataSource = 0;
            clearAll();
            UC_P_SellMedicine_Load(this, null);
            
        }
     

        private void clearAll()
        {
            txtMediId.Clear();
            txtMediName.Clear();
            txtExpireDate.ResetText();
            txtPricePerUnit.Clear();
            txtNoOfUnits.Clear();

        }
    }
}
