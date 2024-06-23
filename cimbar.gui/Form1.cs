using cimbar.lib;

namespace cimbar.gui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            Encoder enc = new Encoder();
            var res = enc.encode(ofd.FileName, "test");

        }
    }
}