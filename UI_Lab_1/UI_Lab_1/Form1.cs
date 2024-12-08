namespace UI_Lab_1
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer;
        private float opacityStep = 0.05f; // Швидкість зниження прозорості
        private int timeElapsed = 0; // Час, що пройшов у мілісекундах

        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load);

            this.BackgroundImage = Image.FromFile(@"C:\Users\gavel\Downloads\Logo_1.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch; 

            //Таймер
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; 
            timer.Tick += Timer_Tick;
            timer.Start();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            timeElapsed += timer.Interval;

            if (timeElapsed <= 20000) // 20 секунд
            {
                // Зменшуємо прозорість
                if (this.Opacity > 0)
                {
                    this.Opacity -= opacityStep;
                }
            }
            else
            {
                timer.Stop();
                this.Opacity = 0; 
            }
        }
    }
}
