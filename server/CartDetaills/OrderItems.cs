using Models;

namespace Cart {
    public class OrderClass {
        public OrderItem? Orders { get; set; }
        public DateTime OrderDate { get; set; }
        public int DiscountID { get; set; }
        public required User Users { get; set; }

        public Discount? Discounts { get; set; }

        public int OrderID { get; set; }
        public Product? Products { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}