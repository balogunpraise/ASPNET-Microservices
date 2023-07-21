namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public ShoppingCart()
        {
            
        }
        public ShoppingCart(string userNamer)
        {
            UserName = userNamer;
        }

        public decimal TotalPrice 
        {
            get
            {
                decimal totalPrice = 0;
                return totalPrice = (decimal)Items?.Select(x => (x.Price * x.Quantity)).Sum();
            }
        }
    }
}
