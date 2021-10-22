using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MultiRPC.Rpc;
using MultiRPC.Rpc.Page;
using MultiRPC.UI.Pages;
using MultiRPC.UI.Pages.Rpc;
using MultiRPC.UI.Pages.Rpc.Custom;
using TinyUpdate.Binary;
using TinyUpdate.Core.Extensions;
using TinyUpdate.Core.Update;
using TinyUpdate.Github;

namespace MultiRPC.UI
{
    public class App : Application
    {
        public static readonly RpcClient RpcClient = new RpcClient();

        //TODO: Put this somewhere else, for now this works
        private UpdateClient? Updater;
        
        public override void Initialize()
        {
#if !WINSTORE && !DEBUG
            Updater = new GithubUpdateClient(new BinaryApplier(), "FluxpointDev", "MultiRPC");
#endif
            AvaloniaXamlLoader.Load(this);

            PageManager.AddRpcPage(new MultiRpcPage());
            PageManager.AddRpcPage(new CustomPage());
            PageManager.AddPage(new SettingsPage());
            PageManager.AddPage(new LoggingPage());
            PageManager.AddPage(new CreditsPage());
            PageManager.AddPage(new ThemeEditorPage());
            PageManager.AddPage(new MissingPage());
            RpcPageManager.GiveRpcClient(RpcClient);

#if !DEBUG
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                _= Updater?.UpdateApp(null);
            }
#endif
        }

        public IClassicDesktopStyleApplicationLifetime? DesktopLifetime;
        
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                DesktopLifetime = desktop;
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
