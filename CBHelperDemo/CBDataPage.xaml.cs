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
using Cloudbase.DataCommands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CBHelperDemo
{
    public class TestObject
    {
        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CBDataPage : Page
    {
        public CBDataPage()
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
            TestObject newObject = new TestObject();
            newObject.FirstName = "Cloud";
            newObject.LastName = "Base";
            newObject.Title = ".io";
            
            App.helper.InsertDocument("test_windows8", newObject, delegate(CBResponseInfo resp)
            {
                this.OutputText.Text = "OUTPUT: " + resp.OutputString;
                return true;
            });
        }

        private async void InsertFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".Jpeg");
            openPicker.FileTypeFilter.Add(".Jpg");
            StorageFile file = await openPicker.PickSingleFileAsync();

            var stream = await file.OpenReadAsync();

            using (var dataReader = new DataReader(stream))
            {
                var bytes = new byte[stream.Size];
                await dataReader.LoadAsync((uint)stream.Size);
                dataReader.ReadBytes(bytes);
                //var stream2 = new MemoryStream(bytes);

                CBHelperAttachment attachment = new CBHelperAttachment();
                attachment.FileName = file.Name;
                attachment.FileData = bytes;

                TestObject newObject = new TestObject();
                newObject.FirstName = "Cloud";
                newObject.LastName = "Base";
                newObject.Title = ".io";

                List<CBHelperAttachment> attList = new List<CBHelperAttachment>();
                attList.Add(attachment);

                App.helper.InsertDocument("test_windows8", newObject, attList, delegate(CBResponseInfo resp)
                {
                    this.OutputText.Text = "OUTPUT: " + resp.OutputString;
                    return true;
                });
            }
        }

        private void SearchObjectButton_Click(object sender, RoutedEventArgs e)
        {
            CBHelperSearchCondition cond = new CBHelperSearchCondition("FirstName", CBConditionOperator.CBOperatorEqual, "Cloud");

            App.helper.SearchDocument("test_windows8", cond, delegate(CBResponseInfo resp)
            {
                this.OutputText.Text = "OUTPUT: " + resp.OutputString;
                return true;
            });
        }
    }
}
