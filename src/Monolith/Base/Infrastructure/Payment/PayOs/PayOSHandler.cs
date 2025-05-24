using System.Linq;
using System.Threading.Tasks;
using BaseApiReference.Abstractions.Payments;
using Net.payOS;
using Net.payOS.Types;

namespace Infrastructure.Payment.PayOs;

/// <summary>
/// Implementation of payment handler interface.
/// </summary>
public class PayOSHandler : IPaymentHandler
{
    private readonly PayOS _payOS;

    public PayOSHandler(PayOS payOS)
    {
        _payOS = payOS;
    }

    public async Task<string> CreatePaymentLink(dynamic paymentModel)
    {
        var paymentModelType = (PaymentModel)paymentModel;
        PaymentData paymentData = new PaymentData(
            orderCode: paymentModelType.OrderCode,
            amount: paymentModelType.Amount,
            description: paymentModelType.Description,
            items: paymentModelType
                .Items.Select(x => new ItemData(x.Name, x.Price, x.Quantity))
                .ToList(),
            cancelUrl: paymentModelType.CancelUrl,
            returnUrl: paymentModelType.ReturnUrl
        );

        CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

        if (string.IsNullOrEmpty(createPayment.checkoutUrl))
        {
            return default;
        }

        return createPayment.checkoutUrl;
    }

    public bool VerifyWebhookData(dynamic webhookType)
    {
        var webhookTypeInstance = new Net.payOS.Types.WebhookType(
            webhookType.Code,
            webhookType.Desc,
            new WebhookData(
                webhookType.Data.AccountNumber,
                webhookType.Data.Amount,
                webhookType.Data.Description,
                webhookType.Data.Reference,
                webhookType.Data.TransactionDateTime,
                webhookType.Data.VirtualAccountNumber,
                webhookType.Data.CounterAccountBankId,
                webhookType.Data.CounterAccountBankName,
                webhookType.Data.CounterAccountName,
                webhookType.Data.CounterAccountNumber,
                webhookType.Data.VirtualAccountName,
                webhookType.Data.OrderCode,
                webhookType.Data.Currency,
                webhookType.Data.PaymentLinkId,
                webhookType.Data.Code,
                webhookType.Data.Desc
            ),
            webhookType.Signature
        );

        WebhookData webhookData;

        webhookData = _payOS.verifyPaymentWebhookData(webhookTypeInstance);

        if (webhookData != null)
        {
            return true;
        }

        return false;
    }
}
