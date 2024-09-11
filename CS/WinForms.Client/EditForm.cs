﻿namespace WinForms.Client
{
    public partial class EditForm : DevExpress.XtraEditors.XtraForm
    {
        public EditForm()
        {
            InitializeComponent();
        }

        public static (bool changesSaved, OrderItem item) EditItem(OrderItem orderItem)
        {
            using var form = new EditForm();
            var clonedItem = new OrderItem
            {
                Id = orderItem.Id,
                ProductName = orderItem.ProductName,
                UnitPrice = orderItem.UnitPrice,
                Quantity = orderItem.Quantity,
                Discount = orderItem.Discount,
                OrderDate = orderItem.OrderDate
            };
            form.orderItemBindingSource.DataSource = clonedItem;
            if (form.ShowDialog() == DialogResult.OK)
            {
                return (true, clonedItem);
            }
            return (false, orderItem);
        }
    }
}