using DevExpress.XtraEditors;

namespace WinForms.Client
{
    public partial class MainForm : XtraForm
    {
        public MainForm()
        {
            InitializeComponent();

            virtualServerModeSource.RowType = typeof(OrderItem);
            virtualServerModeSource.ConfigurationChanged += VirtualServerModeSource_ConfigurationChanged;
            virtualServerModeSource.MoreRows += VirtualServerModeSource_MoreRows;

            gridControl1.DataSource = virtualServerModeSource;
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
    }
}
