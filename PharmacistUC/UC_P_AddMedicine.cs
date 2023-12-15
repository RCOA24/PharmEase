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
    public partial class UC_P_AddMedicine : UserControl
    {
        function fn = new function();
        String query;
     
        public UC_P_AddMedicine()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if  (txtMediId.Text!="" && txtMediName.Text!="" && txtMediNumber.Text!="" && txtQuantity.Text!="" && txtPricePerUnit.Text!="")
            {
               
                String mid = txtMediId.Text;
                String mname = txtMediName.Text;
                String mnumber = txtMediNumber.Text;
                String mdate = txtManufacturingDate.Text;
                String edate = txtExpireDate.Text;
                Int64 quantity = Int64.Parse(txtQuantity.Text);
                Int64 perunit = Int64.Parse(txtPricePerUnit.Text);

                query = "insert into medic (mid,mname,mnumber,mDate,eDate,quantity,perUnit) values ('" + mid + "', '" + mname + "','" + mnumber + "','" + mdate + "','" + edate + "','" + quantity + "','" + perunit + "')";
                fn.setData(query, "Medicine Added to Database.");

            }
            else
            {
                MessageBox.Show("Enter all data.", "Information", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clearall();
        }
        public void clearall()
        {
            txtMediId.Clear();
            txtMediName.Clear();
            txtQuantity.Clear();
            txtMediNumber.Clear();
            txtPricePerUnit.Clear();
            txtManufacturingDate.ResetText();
            txtExpireDate.ResetText();
        }

        private void UC_P_AddMedicine_Load(object sender, EventArgs e)
        {

        }

        private void txtMediId_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
