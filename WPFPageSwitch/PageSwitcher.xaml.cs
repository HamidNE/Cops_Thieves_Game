﻿using System;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using WPFPageSwitch.Menu;

namespace WPFPageSwitch
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    
    public partial class PageSwitcher : Window
    {
        
        public PageSwitcher()
        {
            
            InitializeComponent();
            Switcher.pageSwitcher = this;
            Switcher.Switch(new StartTheGame());
                       
        }

        public void Navigate(UserControl nextPage)
        {
            this.Content = nextPage;
        }

        public void Navigate(UserControl nextPage, object state)
        {
            this.Content = nextPage;
            ISwitchable s = nextPage as ISwitchable;

            if (s != null)
                s.UtilizeState(state);
            else
                throw new ArgumentException("NextPage is not ISwitchable! "
                  + nextPage.Name.ToString());
        }
    }
}
