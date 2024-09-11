using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace WinForms.Client
{
    public partial class MainForm : XtraForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private VirtualServerModeDataLoader? loader;

        private void VirtualServerModeSource_ConfigurationChanged(object? sender, DevExpress.Data.VirtualServerModeRowsEventArgs e)
        {
            loader = new VirtualServerModeDataLoader(e.ConfigurationInfo);
            e.RowsTask = loader.GetRowsAsync(e);
        }

        private void VirtualServerModeSource_MoreRows(object? sender, DevExpress.Data.VirtualServerModeRowsEventArgs e)
        {
            if (loader is not null)
            {
                e.RowsTask = loader.GetRowsAsync(e);
            }
        }

        private async void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (sender is GridView view)
            {
                if (view.FocusedRowObject is OrderItem oi)
                {
                    var editResult = EditForm.EditItem(oi);
                    if (editResult.changesSaved)
                    {
                        await DataServiceClient.UpdateOrderItemAsync(editResult.item);
                        view.RefreshData();
                    }
                }
            }
        }

        //private BindingList<OrderItem> loadedOrderItems = new();
        //private void virtualServerModeSource_AcquireInnerList(object sender, DevExpress.Data.VirtualServerModeAcquireInnerListEventArgs e)
        //{
        //    e.InnerList = loadedOrderItems;
        //}

        //private async void gridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        //{
        //    if (sender is GridView view)
        //    {
        //        if (e.RowHandle == GridControl.NewItemRowHandle)
        //        {
        //            if (e.Row is OrderItem o)
        //            {
        //                // The persisted item includes any server-generated values such as 
        //                // primary keys
        //                var persistedItem = await DataServiceClient.CreateOrderItemAsync(o);
        //                if (persistedItem != null)
        //                {
        //                    // You can update the local row with that information, but note
        //                    // that the row may not appear in the intended location because
        //                    // the client does not apply sorting to the local data.
        //                    o.Id = persistedItem.Id;
        //                }

        //                // Refresh the grid to reflect the changes -- this invokes a server roundtrip
        //                view.RefreshData();
        //            }
        //        }
        //        else
        //        {
        //            await DataServiceClient.UpdateOrderItemAsync((OrderItem)e.Row);
        //        }
        //    }
        //}

        //private void gridView1_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        //{
        //    if (sender is GridView view)
        //    {
        //        view.SetRowCellValue(e.RowHandle, view.Columns["OrderDate"], DateTime.Now);
        //    }
        //}

        //private void gridView1_RowEditCanceled(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        //{
        //    Debug.WriteLine("RowEditCanceled");
        //    //if (e.RowHandle == GridControl.NewItemRowHandle)
        //    //    loadedOrderItems.Remove((OrderItem)e.Row);
        //}

        //private async void gridView1_RowDeleting(object sender, DevExpress.Data.RowDeletingEventArgs e)
        //{
        //    var result = await DataServiceClient.DeleteOrderItemAsync(((OrderItem)e.Row).Id);
        //    if (result)
        //    {
        //        loadedOrderItems.Remove((OrderItem)e.Row);
        //    }
        //    else
        //    {
        //        e.Cancel = true;
        //    }
        //}
    }
}
