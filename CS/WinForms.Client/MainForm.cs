using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using System.Drawing.Text;

namespace WinForms.Client
{
    public partial class MainForm : XtraForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        class CustomOverlayTextPainter(string text) : OverlayWindowPainterBase
        {
            static readonly Font font = new("Tahoma", 16, FontStyle.Bold);

            protected override void Draw(OverlayWindowCustomDrawContext context)
            {
                var cache = context.DrawArgs.Cache;
                cache.TextRenderingHint = TextRenderingHint.AntiAlias;

                context.DrawBackground();

                var bounds = context.DrawArgs.Bounds;
                var midX = bounds.Left + bounds.Width / 2;
                var midY = bounds.Top + bounds.Height / 2;

                SizeF textSize = cache.CalcTextSize(text, font);
                cache.DrawString(text, font, Brushes.Black, new PointF(midX - textSize.Width / 2, midY + 30));

                context.Handled = true;
            }
        }

        class CustomOverlayImagePainter : OverlayImagePainter
        {
            public CustomOverlayImagePainter(Image image, Action clickAction) : base(image, clickAction: clickAction) { }

            protected override Rectangle CalcImageBounds(OverlayLayeredWindowObjectInfoArgs drawArgs)
            {
                return Image.Size.AlignWith(drawArgs.Bounds).WithY(300);
            }
        }

        IOverlaySplashScreenHandle? overlayHandle;

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            ShowOverlay();
        }

        private void ShowOverlay()
        {
            overlayHandle = SplashScreenManager.ShowOverlayForm(this,
                opacity: 200,
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
                logOutItem.Caption = $"Log out {DataServiceClient.Name}";
                if (overlayHandle is not null)
                    SplashScreenManager.CloseOverlayForm(overlayHandle);
                Invoke(new Action(() => { gridControl.DataSource = virtualServerModeSource; }));
            }
        }

        private void logOutItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (DataServiceClient.LoggedIn)
            {
                gridControl.DataSource = null;
                logOutItem.Caption = "Log Out";
                DataServiceClient.LogOut();
                ShowOverlay();
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
