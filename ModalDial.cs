using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOLStartUp
{
    public partial class ModalDialog : Form
    {
        public ModalDialog()
        {
            InitializeComponent();
        }

        private void ModalDial_Load(object sender, EventArgs e)
        {
            Okbutton.DialogResult = DialogResult.OK;
            Cancelbutton.DialogResult = DialogResult.Cancel;
            this.AcceptButton = Okbutton;
            this.CancelButton = Cancelbutton;
            this.CenterToScreen();
            ComboTimerInterval.Items.Clear();
            for (int i = 0; i < 2000; i++)
            {
                string[] numbers = { i.ToString()};
                ComboTimerInterval.Items.AddRange(numbers);
            }
            for (int i = 5; i < 400; i++)
            {
                string[] numbers = { i.ToString()};
                ComboUniverseHeight.Items.AddRange(numbers);
                ComboUniverseWidth.Items.AddRange(numbers);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ComboUniverseWidth_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Okbutton_Click(object sender, EventArgs e)
        {

        }
    }
}
