using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;

namespace WinForms.Client
{
    public partial class MainForm : XtraForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        class CustomOverlayImagePainter : OverlayImagePainter
        {
            public CustomOverlayImagePainter(Image image, Action clickAction) : base(image, clickAction: clickAction) { }

            protected override Rectangle CalcImageBounds(OverlayLayeredWindowObjectInfoArgs drawArgs)
            {
                return Image.Size.AlignWith(drawArgs.Bounds).WithY(550);
            }
        }

        class CustomOverlayTextPainter : OverlayTextPainter
        {
            public CustomOverlayTextPainter(string text) : base(text) { }
            protected override Rectangle CalcTextBounds(OverlayLayeredWindowObjectInfoArgs drawArgs)
            {
                Size textSz = CalcTextSize(drawArgs);
                return textSz.AlignWith(drawArgs.Bounds).WithY(700);
            }
        }

        IOverlaySplashScreenHandle? overlayHandle;

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            overlayHandle = SplashScreenManager.ShowOverlayForm(this,
                animationType: WaitAnimationType.Line,
                customPainter: new OverlayWindowCompositePainter(
                    new CustomOverlayTextPainter("Click the lock to log in"),
                    new CustomOverlayImagePainter(svgImageCollection.GetImage(0), LogIn)
            ));
        }

        private void LogIn()
        {
            var loginForm = new LoginForm();
            loginForm.ShowDialog();
            if (DataServiceClient.LoggedIn)
            {
                if (overlayHandle is not null)
                    SplashScreenManager.CloseOverlayForm(overlayHandle);
                Invoke(new Action(() => { gridControl.DataSource = virtualServerModeSource; }));
            }
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
