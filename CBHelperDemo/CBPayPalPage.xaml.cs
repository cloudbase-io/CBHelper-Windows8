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
    public sealed partial class CBPayPalPage : Page
    {
        public CBPayPalPage()
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CBPayPalBill bill = new CBPayPalBill();
            bill.Currency = "USD";
            bill.Description = "Test transaction for $9.99";
            bill.InvoiceNumber = "TST-INVOICE-001";
            bill.Name = "Test transaction";

            CBPayPalBillItem billItem = new CBPayPalBillItem();
            billItem.Amount = 9.99;
            billItem.Description = "Test item for $9.99";
            billItem.Name = "Test item";
            billItem.Quantity = 1;
            billItem.Tax = 0.0;

            bill.AddNewItem(billItem);

            App.helper.PreparePayPalPurchase(bill, true, delegate(CBResponseInfo resp)
            {

                this.PayPalWebView.Visibility = Windows.UI.Xaml.Visibility.Visible;

                string url = Convert.ToString(((Dictionary<string, object>)resp.Data)["checkout_url"]);
                System.Diagnostics.Debug.WriteLine("received response : " + resp.OutputString);


                this.PayPalWebView.Navigate(new Uri(url));

                this.PayPalWebView.LoadCompleted += delegate(object webViewSender, NavigationEventArgs webViewE)
                {
                    //System.Diagnostics.Debug.WriteLine("started navigating to " + e.Uri.AbsoluteUri);
                    if (App.helper.IsPayPayPaymentComplete(webViewE.Uri, delegate(CBResponseInfo finalResp)
                    {
                        // you can read the payment details
                        this.PayPalWebView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        return true;
                    }));
                };

                return true;
            });
        }
    }
}
