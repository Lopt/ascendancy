using System;
using System.ComponentModel;
using System.Collections.Generic;
using Xamarin.Forms;

namespace client.Common.Models
{
    // This Code is from https://github.com/zumero/Xamarin.Forms-demo/blob/master/demo.Shared/Models/BaseModel.cs
    public class ViewBaseModel : INotifyPropertyChanged
    {
        public bool NotifyIfPropertiesChange = true;

        #region INotifyPropertyChanging implementation

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        public void OnPropertyChanging (string propertyName)
        {
            if (NotifyIfPropertiesChange == false || PropertyChanging == null)
                return;

            PropertyChanging (this, new PropertyChangingEventArgs (propertyName));
        }


        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void OnPropertyChanged (string propertyName)
        {
            if (NotifyIfPropertiesChange == false || PropertyChanged == null)
                return;

            PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
        }

        protected void SetProperty<U> (
            ref U backingStore, U value,
            string propertyName,
            Action onChanged = null,
            Action<U> onChanging = null)
        {


            if (EqualityComparer<U>.Default.Equals (backingStore, value))
                return;

            if (onChanging != null)
                onChanging (value);

            OnPropertyChanging (propertyName);

            backingStore = value;

            if (onChanged != null)
                onChanged ();

            OnPropertyChanged (propertyName);
        }

        protected void SetProperty<U> (
            U backingStore, U value,
            Action<U> performChange,
            string propertyName,
            Action onChanged = null,
            Action<U> onChanging = null)
        {


            if (EqualityComparer<U>.Default.Equals (backingStore, value))
                return;

            if (onChanging != null)
                onChanging (value);

            OnPropertyChanging (propertyName);

            if (performChange != null)
                performChange (value);

            if (onChanged != null)
                onChanged ();

            OnPropertyChanged (propertyName);
        }
    }
}

