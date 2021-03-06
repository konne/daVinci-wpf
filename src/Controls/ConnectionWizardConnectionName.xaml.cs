﻿namespace daVinci.Controls
{
    #region Usings
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Controls;
    #endregion

    /// <summary>
    /// Interaktionslogik für ConnectionWizardConnectionName.xaml
    /// </summary>
    public partial class ConnectionWizardConnectionName : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
        #endregion

        public ConnectionWizardConnectionName()
        {
            InitializeComponent();
        }

        #region Properties & Variables
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (text != value)
                {
                    text = value;
                    RaisePropertyChanged();
                }
            }
        }
        private string hint;
        public string Hint
        {
            get
            {
                return hint;
            }
            set
            {
                if (hint != value)
                {
                    hint = value;
                    RaisePropertyChanged();
                }
            }
        }
        private string inputText;
        public string InputText
        {
            get
            {
                return inputText;
            }
            set
            {
                if (inputText != value)
                {
                    inputText = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion
    }
}
