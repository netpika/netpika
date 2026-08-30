using BTCPayServer.Abstractions.Contracts;
using BTCPayServer.Abstractions.Models;
using BTCPayServer.Payments;
using Microsoft.Extensions.DependencyInjection;

namespace NetPika.AddressOnlyQR;

public sealed class Plugin : BaseBTCPayServerPlugin
{
    public override string Identifier => "NetPika.AddressOnlyQR";
    public override string Name => "netPika Address-Only QR";
    public override string Description => "Uses the raw BTC/LTC destination address as the checkout QR payload while preserving the full payment URI for Pay in Wallet.";

    public override IBTCPayServerPlugin.PluginDependency[] Dependencies { get; } =
    {
        new IBTCPayServerPlugin.PluginDependency
        {
            Identifier = nameof(BTCPayServer),
            Condition = ">=2.4.3 <2.5.0"
        }
    };

    public override void Execute(IServiceCollection services)
    {
        services.AddSingleton<IGlobalCheckoutModelExtension, AddressOnlyQrCheckoutExtension>();
    }
}

public sealed class AddressOnlyQrCheckoutExtension : IGlobalCheckoutModelExtension
{
    public void ModifyCheckoutModel(CheckoutModelContext context)
    {
        var model = context.Model;

        if (model.PaymentMethodId is not ("BTC-CHAIN" or "LTC-CHAIN"))
            return;

        if (string.IsNullOrWhiteSpace(model.Address))
            return;

        model.InvoiceBitcoinUrlQR = model.Address;
    }
}
