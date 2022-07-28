using Microsoft.Extensions.Logging;
using Prism.Ioc;
using Prism.Modularity;
using Rep.Controls.CustomContorls;
using Rep.Controls.View;
using Rep.MES.Views;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Prism.MES
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        ILogger _logger;
        //重写CreateShell方法，在App.xaml文件中，不再设置StartupUri
        protected override Window CreateShell()
        {
            //UI线程未捕获异常处理事件
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
            //多线程异常
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            //使用容器创建主窗体
            return Container.Resolve<MainWindows>();
        }
        protected override IModuleCatalog CreateModuleCatalog()
        {
            //指定模块加载方式为从文件夹中以反射发现并加载module(推荐用法)
            return new DirectoryModuleCatalog() { ModulePath = @".\" };
        }
        //给容器中注册对象
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var factory = new NLog.Extensions.Logging.NLogLoggerFactory();
            _logger = factory.CreateLogger("NLog");
            //注入到Prism DI容器中
            containerRegistry.RegisterInstance(_logger);
            containerRegistry.RegisterDialog<MessageDialogView, MessageDialogControl>();
            containerRegistry.RegisterDialog<AddItemDialogView, AddItemDialogControl>();
            containerRegistry.RegisterDialog<TextBoxDialogView, TextBoxDialogControl>();
            containerRegistry.RegisterForNavigation<MainView>("HomeView");
        }

        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //通常全局异常捕捉的都是致命信息
            _logger.LogError($"{ e.Exception.StackTrace },{ e.Exception.Message }");
        }

        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            _logger.LogError($"{ e.Exception.StackTrace },{ e.Exception.Message }");
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            _logger.LogError($"{ ex.StackTrace },{ ex.Message }");
        }
    }
}
