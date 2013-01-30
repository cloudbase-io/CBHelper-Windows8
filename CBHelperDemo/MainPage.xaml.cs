/* Copyright (C) 2012 cloudbase.io
 
 This program is free software; you can redistribute it and/or modify it under
 the terms of the GNU General Public License, version 2, as published by
 the Free Software Foundation.
 
 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 for more details.
 
 You should have received a copy of the GNU General Public License
 along with this program; see the file COPYING.  If not, write to the Free
 Software Foundation, 59 Temple Place - Suite 330, Boston, MA
 02111-1307, USA.
 */
using Cloudbase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CBHelperDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (localSettings.Values["CBAppCode"] != null)
            {
                AppCodeBox.Text = Convert.ToString(localSettings.Values["CBAppCode"]);
                AppUniqBox.Text = Convert.ToString(localSettings.Values["CBAppUniq"]);
                AppPwdBox.Text = Convert.ToString(localSettings.Values["CBAppPwd"]);
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string appCode = AppCodeBox.Text;
            string appUniq = AppUniqBox.Text;
            string appPwd = MD5Core.GetHashString(AppPwdBox.Text);

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
   
            // Persisting simple Application Settings.
            localSettings.Values["CBAppCode"] = appCode;
            localSettings.Values["CBAppUniq"] = appUniq;
            localSettings.Values["CBAppPwd"] = AppPwdBox.Text;
    
            App.helper = new CBHelper(appCode, appUniq);
            App.helper.Dispatcher = this.Dispatcher;
            App.helper.SetPassword(appPwd);
            App.helper.DebugMode = true;

            this.Frame.Navigate(typeof(APIMainPage));
        }
    }
}
