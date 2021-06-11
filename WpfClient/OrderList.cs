using System;
using System.Collections.Generic;
using System.Text;

namespace WpfClient
{
    public class OrderList
    {
        public string Name { get; set; }
        public float Price { get; set; }

        public OrderList Copy()
        {
            var result = new OrderList();
            result.Name = this.Name;
            result.Price = this.Price;
            return result;
        }
    }
}
