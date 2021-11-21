using System;
using System.Diagnostics;
using System.IO;
using Avalonia;
using MultiRPC.Logging;
using MultiRPC.Setting;
using MultiRPC.Setting.Settings;
using MultiRPC.UI;
using MultiRPC.Utils;
using TinyUpdate.Core;
using TinyUpdate.Core.Logging;
using TinyUpdate.Core.Logging.Loggers;

//TODO: Make this be used for the versioning
[assembly: SemanticVersion("7.0.0-testing")]
namespace MultiRPC
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            LoggingCreator.GlobalLevel = SettingManager<GeneralSettings>.Setting.LogLevel;

            // Set the current directory to the app install location to get assets.
#if WINSTORE
            Directory.SetCurrentDirectory(AppContext.BaseDirectory + "/");
#else
            // This seems to break windows apps even though it *can* write to the log folder.
            var logFolder = Path.Combine(Constants.SettingsFolder, "Logging");
            Directory.CreateDirectory(logFolder);
            LoggingCreator.AddLogBuilder(new FileLoggerBuilder(logFolder));
#endif
            LoggingCreator.AddLogBuilder(new LoggingPageBuilder());
            
            //We do the check here so if we do throw, it'll be logged wherever it needs to be logged
            if (!DebugUtil.IsDebugBuild && Process.GetProcessesByName("MultiRPC").Length > 1)
            {
                //TODO: Make it show the first instance
                throw new Exception("Multiple instances are not allowed!");
            }

            var builder = BuildAvaloniaApp();
            builder.StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .With(new Win32PlatformOptions
                {
                    EnableMultitouch = true
                })
                .LogToTrace();
    }
}