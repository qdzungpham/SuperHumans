using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using SuperHumans.Helpers;
using SuperHumans.Models;
using SuperHumans.Services;

namespace SuperHumans.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IParseAccess ParseAccess => ServiceLocator.Instance.Get<IParseAccess>() ?? new ParseAccess();
        public IRestService RestService => ServiceLocator.Instance.Get<IRestService>() ?? new RestService();


        public User CurrentUser
        {
            get
            {
                var u = ParseAccess.CurrentUser();
                if (u == null) return null;
                return new User
                {
                    Username = u.Username,
                    Email = u.Email
                };
            }
        }

        public string CurrentUIMode
        {
            get
            {
                return Settings.UIMode;
            }
        }

        bool isAsyncFinished = false;
        public bool IsAsyncFinished
        {
            get { return isAsyncFinished; }
            set { SetProperty(ref isAsyncFinished, value); }
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
