using MVCEStoreData;
using System.Collections.Generic;

namespace MVCEStoreWeb.Areas.Admin.Models
{
    public class DashboardDataViewModel
    {
        public int Users { get; set; }
        public IEnumerable<Order> NewOrders { get; set; } = new List<Order>();
        public decimal SalesTotal { get; set; }
        public decimal SalesThisMonth { get; set; }
        public IEnumerable<SalesDataViewModel> SalesData { get; set; } = new List<SalesDataViewModel>();
        public IEnumerable<User> LastUsers { get; set; } = new List<User>();
        public IEnumerable<ProductSalesViewModel> MostProductSales { get; set; } = new List<ProductSalesViewModel>();
        public IEnumerable<Product> MostProductReviews { get; set; } = new List<Product>();

    }
}
