using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using SMIUCarpool.Data;
using SMIUCarpool.Forms;

namespace SMIUCarpool;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        // Bind distinct event handlers to avoid compiler ambiguity
        Application.ThreadException += HandleThreadException;
        AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;

        try
        {
            ApplicationConfiguration.Initialize();
            DatabaseHelper.InitializeDatabase();
            DatabaseSeeder.SeedData();
            Application.Run(new LoginForm());
        }
        catch (Exception ex)
        {
            LogAndShowError(ex);
        }
    }

    // Explicit handler for Windows Forms UI thread exceptions
    private static void HandleThreadException(object sender, ThreadExceptionEventArgs e)
    {
        LogAndShowError(e.Exception);
    }

    // Explicit handler for non-UI/background thread exceptions
    private static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            LogAndShowError(ex);
        }
    }

    // Unified helper to log the stack trace and notify the user safely
    private static void LogAndShowError(Exception ex)
    {
        try
        {
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "startup-error.log");
            File.WriteAllText(logPath, ex.ToString());
        }
        catch
        {
            // Fail silently if the app lacks disk write permissions
        }

        MessageBox.Show(
            "The application encountered a critical error and could not start. A startup error log was written next to the executable.",
            "Startup Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);

        // Terminate the process since the application state is unstable
        Environment.Exit(1);
    }
}