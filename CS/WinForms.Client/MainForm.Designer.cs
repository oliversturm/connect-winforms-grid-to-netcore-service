namespace WinForms.Client {
    partial class MainForm {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            gridControl1 = new DevExpress.XtraGrid.GridControl();
            virtualServerModeSource = new DevExpress.Data.VirtualServerModeSource(components);
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            colId = new DevExpress.XtraGrid.Columns.GridColumn();
            colProductName = new DevExpress.XtraGrid.Columns.GridColumn();
            colUnitPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            colQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            colDiscount = new DevExpress.XtraGrid.Columns.GridColumn();
            colOrderDate = new DevExpress.XtraGrid.Columns.GridColumn();
            MainFormlayoutControl1ConvertedLayout = new DevExpress.XtraLayout.LayoutControl();
            layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            gridControl1item = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)virtualServerModeSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MainFormlayoutControl1ConvertedLayout).BeginInit();
            MainFormlayoutControl1ConvertedLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridControl1item).BeginInit();
            SuspendLayout();
            // 
            // gridControl1
            // 
            gridControl1.DataSource = virtualServerModeSource;
            gridControl1.Location = new Point(12, 12);
            gridControl1.MainView = gridView1;
            gridControl1.Name = "gridControl1";
            gridControl1.Size = new Size(776, 426);
            gridControl1.TabIndex = 0;
            gridControl1.UseEmbeddedNavigator = true;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView1 });
            // 
            // virtualServerModeSource
            // 
            virtualServerModeSource.RowType = typeof(OrderItem);
            virtualServerModeSource.ConfigurationChanged += VirtualServerModeSource_ConfigurationChanged;
            virtualServerModeSource.MoreRows += VirtualServerModeSource_MoreRows;
            virtualServerModeSource.AcquireInnerList += virtualServerModeSource_AcquireInnerList;
            // 
            // gridView1
            // 
            gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colId, colProductName, colUnitPrice, colQuantity, colDiscount, colOrderDate });
            gridView1.GridControl = gridControl1;
            gridView1.Name = "gridView1";
            gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.InitNewRow += gridView1_InitNewRow;
            gridView1.RowDeleting += gridView1_RowDeleting;
            gridView1.RowUpdated += gridView1_RowUpdated;
            gridView1.RowEditCanceled += gridView1_RowEditCanceled;
            // 
            // colId
            // 
            colId.FieldName = "Id";
            colId.Name = "colId";
            colId.Visible = true;
            colId.VisibleIndex = 0;
            // 
            // colProductName
            // 
            colProductName.FieldName = "ProductName";
            colProductName.Name = "colProductName";
            colProductName.Visible = true;
            colProductName.VisibleIndex = 1;
            // 
            // colUnitPrice
            // 
            colUnitPrice.FieldName = "UnitPrice";
            colUnitPrice.Name = "colUnitPrice";
            colUnitPrice.Visible = true;
            colUnitPrice.VisibleIndex = 2;
            // 
            // colQuantity
            // 
            colQuantity.FieldName = "Quantity";
            colQuantity.Name = "colQuantity";
            colQuantity.Visible = true;
            colQuantity.VisibleIndex = 3;
            // 
            // colDiscount
            // 
            colDiscount.FieldName = "Discount";
            colDiscount.Name = "colDiscount";
            colDiscount.Visible = true;
            colDiscount.VisibleIndex = 4;
            // 
            // colOrderDate
            // 
            colOrderDate.FieldName = "OrderDate";
            colOrderDate.Name = "colOrderDate";
            colOrderDate.Visible = true;
            colOrderDate.VisibleIndex = 5;
            // 
            // MainFormlayoutControl1ConvertedLayout
            // 
            MainFormlayoutControl1ConvertedLayout.Controls.Add(gridControl1);
            MainFormlayoutControl1ConvertedLayout.Dock = DockStyle.Fill;
            MainFormlayoutControl1ConvertedLayout.Location = new Point(0, 0);
            MainFormlayoutControl1ConvertedLayout.Name = "MainFormlayoutControl1ConvertedLayout";
            MainFormlayoutControl1ConvertedLayout.Root = layoutControlGroup1;
            MainFormlayoutControl1ConvertedLayout.Size = new Size(800, 450);
            MainFormlayoutControl1ConvertedLayout.TabIndex = 1;
            // 
            // layoutControlGroup1
            // 
            layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            layoutControlGroup1.GroupBordersVisible = false;
            layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { gridControl1item });
            layoutControlGroup1.Name = "layoutControlGroup1";
            layoutControlGroup1.Size = new Size(800, 450);
            layoutControlGroup1.TextVisible = false;
            // 
            // gridControl1item
            // 
            gridControl1item.Control = gridControl1;
            gridControl1item.Location = new Point(0, 0);
            gridControl1item.Name = "gridControl1item";
            gridControl1item.Size = new Size(780, 430);
            gridControl1item.TextSize = new Size(0, 0);
            gridControl1item.TextVisible = false;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(MainFormlayoutControl1ConvertedLayout);
            Name = "MainForm";
            Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)virtualServerModeSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)MainFormlayoutControl1ConvertedLayout).EndInit();
            MainFormlayoutControl1ConvertedLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup1).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridControl1item).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraLayout.LayoutControl MainFormlayoutControl1ConvertedLayout;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem gridControl1item;
        private DevExpress.Data.VirtualServerModeSource virtualServerModeSource;
        private DevExpress.XtraGrid.Columns.GridColumn colId;
        private DevExpress.XtraGrid.Columns.GridColumn colProductName;
        private DevExpress.XtraGrid.Columns.GridColumn colUnitPrice;
        private DevExpress.XtraGrid.Columns.GridColumn colQuantity;
        private DevExpress.XtraGrid.Columns.GridColumn colDiscount;
        private DevExpress.XtraGrid.Columns.GridColumn colOrderDate;
    }
}
