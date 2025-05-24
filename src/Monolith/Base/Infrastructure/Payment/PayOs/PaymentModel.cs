using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Payment.PayOs;

/// <summary>
/// Represent the payment data input model.
/// </summary>
public class PaymentModel
{
    public long OrderCode { get; set; }
    public int Amount { get; set; }
    public string Description { get; set; }
    public List<ItemData> Items { get; set; }
    public string CancelUrl { get; set; }
    public string ReturnUrl { get; set; }

    public sealed class ItemData
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
