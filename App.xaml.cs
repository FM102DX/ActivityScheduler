﻿using ActivityScheduler.Core.Appilcation;
using ActivityScheduler.Service;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;





namespace ActivityScheduler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private ServiceProvider serviceProvider;
        Serilog.ILogger Logger;
        System.Windows.Forms.NotifyIcon notifyIcon1;
        MainWindow mainWindow;
        ActivityScheduler.Core.TrayContextMenu trayContextMenu;
        ServiceProvider _serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }
        private void ConfigureServices(ServiceCollection services)
        {

            services.AddSingleton<MainWindow>();

            services.AddSingleton(typeof(ActivitySchedulerApp), (x) => new ActivitySchedulerApp());

            var _serviceProvider = services.BuildServiceProvider();

            var app= _serviceProvider.GetService<ActivitySchedulerApp>();

            Functions.LeaveLastNFilesOrFoldersInDirectory(app.LogsDirectory, 20);

            string logFilePath= System.IO.Path.Combine(app.LogsDirectory, Functions.GetNextFreeFileName(app.LogsDirectory, "ActivitySchedulerLogs","txt")); 


            Serilog.ILogger _logger = new LoggerConfiguration()
                  .WriteTo.File(logFilePath)
                  .CreateLogger();


            services.AddSingleton(typeof(Serilog.ILogger), (x) => _logger);

        }
        private void OnStartup(object sender, StartupEventArgs e)
        {

            var app = _serviceProvider.GetService<ActivitySchedulerApp>();

            string iconFileFullPath = Path.Combine(app.IconsDirectory, "app.ico");

            notifyIcon1 = new NotifyIcon();

            notifyIcon1.Icon = new System.Drawing.Icon(iconFileFullPath);

            notifyIcon1.Text = "Activity Scheduler";

            notifyIcon1.Visible = true;

            notifyIcon1.MouseDown += NotifyIcon1_MouseDown;
            notifyIcon1.MouseDoubleClick += NotifyIcon1_MouseDoubleClick;
            

            mainWindow = _serviceProvider.GetService<MainWindow>();

            var _logger = _serviceProvider.GetService<Serilog.ILogger>();

            Logger = _logger;

            Logger.Information("Starting ActivityScheduler app");

            var icon = new Icon(SystemIcons.Exclamation, 40, 40);

            trayContextMenu = new ActivityScheduler.Core.TrayContextMenu(this);

            mainWindow.Show();
           
        }

        private void NotifyIcon1_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            mainWindow.WindowState = WindowState.Normal;
            mainWindow.Show();
            mainWindow.Activate();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon1.Visible = false;
            base.OnExit(e);
        }

        private void NotifyIcon1_MouseDown(object? sender, MouseEventArgs e)
        {
            
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ContextMenu menu = (ContextMenu)trayContextMenu.FindResource("NotifierContextMenu");

                menu.IsOpen = true;

                Logger.Information("Context menu opened by r-click");
            }
        }

        public void ShowMainWindow()
        {
            mainWindow = new ActivityScheduler.MainWindow();
            mainWindow.Show();
            
        }
        public void HideMainWindow()
        {
            mainWindow.Hide();
        }
    }
}