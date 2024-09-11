using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
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

        private async void addItemButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridControl.FocusedView is ColumnView view)
            {
                var createResult = EditForm.CreateItem();
                if (createResult.changesSaved)
                {
                    await DataServiceClient.CreateOrderItemAsync(createResult.item!);
                    view.RefreshData();
                }
            }
        }

        private async void deleteItemButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridControl.FocusedView is ColumnView view &&
                view.GetFocusedRow() is OrderItem orderItem)
            {
                await DataServiceClient.DeleteOrderItemAsync(orderItem.Id);
                view.RefreshData();
            }
        }
    }
}
