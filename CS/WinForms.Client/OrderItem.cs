﻿namespace WinForms.Client
{
    public class OrderItem
    {
        public int Id { get; set; }

        public string ProductName { get; set; } = null!;
        private decimal _unitPrice = 0;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public float Discount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
