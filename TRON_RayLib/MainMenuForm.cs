using System.Drawing.Text;

namespace TRON_RayLib
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
            //
            // Loading custom Font
            //
            PrivateFontCollection privateFont = new();
            privateFont.AddFontFile("TRON.TTF");
            Font customFont = new(privateFont.Families[0], 30);
            TITLE.Font = customFont;
        }

        private void EXIT_BTN_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void VS_AI_BTN_Click(object sender, EventArgs e)
        {
            Hide();
            Launcher.LaunchVS_AI();
        }

        private void VS_LOCAL_BTN_Click(object sender, EventArgs e)
        {
            Hide();
            Launcher.LaunchVS_Local();
        }

        private void VS_ONLINE_BTN_Click(object sender, EventArgs e)
        {
            Hide();
            string ip = PromptForIP();
            Launcher.LaunchVS_Online(ip);
        }

        private static string PromptForIP()
        {
            string ip = Microsoft.VisualBasic.Interaction.InputBox("Enter Server IP:", "Online Mode", "127.0.0.1");
            return ip;
        }

        [STAThread]
        static void Main()
        {
            Application.Run(new MainMenuForm());
        }
    }
}
