﻿using ActivityScheduler.Core.Appilcation;
using ActivityScheduler.Data.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ActivityScheduler.Core.Settings
{
    public partial class Settings : Window
    {
        private SettingsManager _settingsManager;
        private ActivitySchedulerApp _app;
        private WorkerServiceManager _workerMgr;
        public Settings(SettingsManager settingsManager, ActivitySchedulerApp app, WorkerServiceManager workerMgr)
        {
            _settingsManager = settingsManager;
            _app = app;
            _workerMgr = workerMgr;
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var settings = new SettingsData();

            settings.DefaultScriptPath = SelectScriptPathForBatchTb.Text;

            if ((bool)FillTestDataOnLaunch.IsChecked) { settings.FillTestDataOnLaunch = true; } else { settings.FillTestDataOnLaunch = false; }
            
            _settingsManager.SaveSettings(settings);

            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var settings = _settingsManager.GetSettings();
            SelectScriptPathForBatchTb.Text = settings.DefaultScriptPath;
            FillTestDataOnLaunch.IsChecked = settings.FillTestDataOnLaunch;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GetStateBtn_Click(object sender, RoutedEventArgs e)
        {
            StateLbl.Text = _workerMgr.GetServiceState().ToString();
        }

        private void InstallBtn_Click(object sender, RoutedEventArgs e)
        {
            _workerMgr.InstallService();
        }

        private void UninstallBtn_Click(object sender, RoutedEventArgs e)
        {
            _workerMgr.UninstallService();
        }

        private void RunBtn_Click(object sender, RoutedEventArgs e)
        {
            _workerMgr.StartService();
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            _workerMgr.StopService();
        }

        private void SelectScriptrFileForBatchReset_Click(object sender, RoutedEventArgs e)
        {
            SelectScriptPathForBatchTb.Text = string.Empty;
        }

        private void SelectScriptFileForBatch_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".ps1";
            dlg.Filter = "PowershellScript (.ps1)|*.ps1";

            //dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            // Display OpenFileDialog by calling ShowDialog method

            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                SelectScriptPathForBatchTb.Text = filename;
            }
        }
    }
}
