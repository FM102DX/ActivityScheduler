﻿using ActivityScheduler.Core;
using ActivityScheduler.Data.Contracts;
using ActivityScheduler.Data.Managers;
using ActivityScheduler.Data.Models;
using ActivityScheduler.Shared.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ActivityScheduler.Gui.EditWindow
{
    public class EditWindowViewModel : INotifyPropertyChanged
    {
        private Serilog.ILogger _logger;
        private IAsyncRepositoryT<Activity> _repo;
        private BatchManager _batchManager;
        private ActivityManager _activityManager;
        public SelectionMode SelectionModeVar { get; set; } = SelectionMode.None;
        private Batch _currentBatch;
        public ICommand SaveActivityCmd { get; private set; }
        public ICommand SaveBatchCmd { get; private set; }
        
        public Batch CurrentBatch { 
            get
            {
                return _currentBatch;
            }
            set
            {
                _currentBatch = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentBatch"));
            }
        }

        private List<Activity> _activitiesList = new List<Activity>();

        public List<Activity> AcivityListItemSource 
        { 
            get
            {
                return (List<Activity>) _activitiesList;    
            }
            
            set
            {
                _activitiesList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AcivityListItemSource"));
            }
        
        }

        private Activity _selectedItem;

        public Activity SelectedItemDisplayed { get; set; }

        public Activity SelectedItem 
        {
            get 
            { 
                return _selectedItem; 
            }
            set
            {
                _selectedItem = value;
                
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedItemDisplayed"));

                SelectedItemDisplayed = _selectedItem.Clone();
                
                if (_selectedItem == null)
                {
                    //SelectionModeVar = SelectionMode.None;
                    return;
                }

                UpdateSelectionMode();
            }
        }

        private TimeSpan _actStartTime { get; set; }
        private bool _saveActivityStopMarker = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string BufferIn { get; set; }

        public EditWindowViewModel (BatchManager batchManager, ActivityManager activityManager, Batch currentBatch, Serilog.ILogger logger)
        {
            _logger = logger;
            
            _batchManager = batchManager;
            
            CurrentBatch = _batchManager.Clone(currentBatch);
            
            _activityManager = activityManager;

            if(CurrentBatch.IsGroup) 
            {
                SelectionModeVar = SelectionMode.GroupMode;
            }
            else
            {
                SelectionModeVar = SelectionMode.ActivityModeNoSelection;
            }

            //_batchManager._checker.BindControlToCheck("UpdateNumber", CurrentBatch.Number).BindControlToCheck("UpdateName", CurrentBatch.Name);

            //_activityManager._checker.BindActionToCheck("UpdateTransactionId", () => { TransactionIdTb.Focus(); TransactionIdTb.SelectAll(); });

            SaveActivityCmd = new ActionCommand(() =>
            {

            });
            SaveBatchCmd = new ActionCommand(() =>
            {
                //CommonOperationResult chkRez;

                //_currentBatch.Number = BatchNumberTb.Text;

                //_currentBatch.Name = BatchNameTb.Text;

                //var btcAddRez = _batchManager.ModifyBatch(_currentBatch).Result;

                //if (!btcAddRez.Success)
                //{
                //    System.Windows.Forms.MessageBox.Show(btcAddRez.Message);
                //    return;
                //}

                //Close();
            });

            LoadActivities();
        }

        private void LoadActivities()
        {
            _activitiesList = _activityManager.GetAll(CurrentBatch.Id).Result.ToList();
        }

        private void UpdateSelectionMode()
        {
            //if (CurrentBatch == null) return;

            //if (CurrentBatch.IsGroup)
            //{
            //    SelectionModeVar = SelectionMode.Group;
            //}
            //else
            //{
            //    if (IsBatchRunning(CurrentBatch.Number))
            //    {
            //        SelectionModeVar = SelectionMode.RealBatchRunning;
            //    }
            //    else
            //    {
            //        SelectionModeVar = SelectionMode.RealBatchStopped;
            //    }
            //}

            //if (SelectionModeChanged != null)
            //{
            //    SelectionModeChanged(SelectionModeVar);
            //}
        }

        

        public enum SelectionMode
        {
            None=0,
            GroupMode = 1,
            ActivityModeNoSelection = 2,
            ActivityModeRegularSelection = 3
        }



    }


}
