using System;
using System.Windows.Forms;

namespace Bookstore
{
    public partial class DeleteForm : Form
    {
        public bool Confirmed { get; private set; }

        public DeleteForm()
        {
            InitializeComponent();
            radioButton2.Checked = true;
            Confirmed = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Confirmed = radioButton1.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void DeleteForm_Load(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}