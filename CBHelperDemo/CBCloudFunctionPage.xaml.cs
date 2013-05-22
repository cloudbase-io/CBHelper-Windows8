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
    public sealed partial class CBCloudFunctionPage : Page
    {
        public CBCloudFunctionPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void ExecuteFunctionButton_Click(object sender, RoutedEventArgs e)
        {
            string fcode = this.FunctionCodeBox.Text;

            App.helper.ExecuteCloudFunction(fcode, null, delegate(CBResponseInfo resp)
            {
                this.OutputText.Text = resp.OutputString;
                return true;
            });
        }

        private void ExecuteAppletButton_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> appletParams = new Dictionary<string, string>();
            appletParams.Add("lat", "52.981723");
            appletParams.Add("lng", "-2.153320");
            App.helper.ExecuteApplet("cb_get_address_from_coordinates", appletParams, delegate(CBResponseInfo resp)
            {
                this.OutputText.Text = "Applet output: " + resp.OutputString;
                return true;
            });
        }

        private void ExecuteAppletButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> Params = new Dictionary<string,string>();
            Params.Add("shared_message", "Hello from the Windows 8 demo application");

            App.helper.ExecuteSharedApi("f_demo-application_demo-shared-api", "cb_demo,1!", Params, delegate(CBResponseInfo resp)
            {
                this.OutputText.Text = "Applet output: " + resp.OutputString;
                return true;
            });
        }
    }
}
