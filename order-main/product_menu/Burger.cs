namespace OrderForm.product_menu
{
    public partial class Burger : Form
    {
        private Main_form main_form;
        public Burger(Main_form main_form)
        {
            InitializeComponent();
            this.main_form = main_form;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            main_form.Load_Products_calum(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            main_form.Load_Products_calum(2);
        }
    }
}
