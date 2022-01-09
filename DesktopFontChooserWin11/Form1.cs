using Microsoft.Win32;
using System.Security.Principal;

namespace DesktopFontChooserWin11
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            if (!IsAdministrator())
            {
                MessageBox.Show("You will need to be an Administrator to set the desktop FONT");
            }
        }

        

    public static bool IsAdministrator()
    {
        using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
        {
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }

    private void button1_Click(object sender, EventArgs e)
        {
            if (FD1.ShowDialog() == DialogResult.OK)
            {
                // here we can hydrate the interface with the selected sample

                txtSample.Font = FD1.Font;
            }


        }

        private void MakeSureKeysExist()
        {
            try
            {

                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts");

                if (key == null)
                {
                    // we need to make it here
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts");
                }


                RegistryKey key2 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes");

                if (key2 == null)
                {
                    Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + System.Environment.NewLine + "You may need to be administrator for this to work.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                if (true) // used to have a check here but Font selection for current users does not work apparently
                {
                    MakeSureKeysExist();

                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts", true))
                    {
                        //"Segoe UI (TrueType)" = ""
                        //"Segoe UI Bold (TrueType)" = ""
                        //"Segoe UI Bold Italic (TrueType)" = ""
                        //"Segoe UI Italic (TrueType)" = ""
                        //"Segoe UI Light (TrueType)" = ""
                        //"Segoe UI Semibold (TrueType)" = ""
                        //"Segoe UI Symbol (TrueType)" = ""

                        // Make sure we have such a thing
                        if (key != null)
                        {

                            key.SetValue("Segoe UI (TrueType)", "");
                            key.SetValue("Segoe UI Bold (TrueType)", "");
                            key.SetValue("Segoe UI Bold Italic (TrueType)", "");
                            key.SetValue("Segoe UI Italic (TrueType)", "");
                            key.SetValue("Segoe UI Light (TrueType)", "");
                            key.SetValue("Segoe UI Semibold (TrueType)", "");
                            key.SetValue("Segoe UI Symbol (TrueType)", "");

                            // now to set the substitute

                            using (RegistryKey key2 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontSubstitutes", true))
                            {
                                if (key2 != null)
                                {
                                    key2.SetValue("Segoe UI", txtSample.Font.Name);

                                    MessageBox.Show("Logout and Back in to see your new font applied");
                                }
                                else
                                {
                                    // big triuble in little china
                                    MessageBox.Show(" big trouble in little china");
                                }
                            }

                        }
                        else
                        {
                            // Big trouble in little china
                            MessageBox.Show(" big trouble in little china");
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + System.Environment.NewLine + "You may need to be administrator for this to work.");

            }
            
        }
    }
}