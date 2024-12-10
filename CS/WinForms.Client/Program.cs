using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using Microsoft.Win32;
using System.IO;
using System.IO.Pipes;

namespace WinForms.Client
{
    internal static class Program
    {
        private const string pipeName = "WinAppDemoInstancePipe";

        [STAThread]
        static int Main(string[] args)
        {

            if (args.Length > 1)
            {
                Console.Error.WriteLine("Invalid number of arguments.");
                return -1;
            }
            else if (args.Length == 1)
            {
                // Assuming that protocol messages should normally come in when another 
                // instance of the app is already running, try first to send the message
                // to the existing instance.
                try
                {
                    using (var client = new NamedPipeClientStream(".", pipeName, PipeDirection.Out))
                    {
                        client.Connect(500);
                        using (var writer = new StreamWriter(client) { AutoFlush = true })
                        {
                            writer.Write(args[0]);

                            // Nothing else to do after sending, so quit.
                            return 0;
                        }
                    }
                }
                catch (TimeoutException)
                {
                    // No existing instance found, so we need to start up and then handle the 
                    // message ourselves.
                }
            }

            WindowsFormsSettings.SetPerMonitorDpiAware();
            WindowsFormsSettings.DefaultLookAndFeel.SetSkinStyle(SkinStyle.WXI);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RegisterProtocol();

            Application.Run(new MainForm());

            return 0;
        }

        static void RegisterProtocol()
        {
            string customProtocol = "winappdemo";
            string applicationPath = Application.ExecutablePath;
            var keyPath = $@"Software\Classes\{customProtocol}";
            using (var key = Registry.CurrentUser.CreateSubKey(keyPath, true))
            {
                if (key == null)
                {
                    throw new Exception($"Registry key can't be written: {keyPath}");
                }
                key.SetValue(string.Empty, "URL:" + customProtocol);
                key.SetValue("URL Protocol", string.Empty);
                using (var commandKey = key.CreateSubKey(@"shell\open\command"))
                {
                    commandKey.SetValue(string.Empty, applicationPath + " " + "%1");
                }
            }
        }

        static void HandleProtocolMessage(string msg)
        {

        }
    }
}