using BTCPayServer.Abstractions.Contracts;
using BTCPayServer.Abstractions.Models;
using Microsoft.Extensions.DependencyInjection;

namespace NetPika.ReturnOnProcessing;

public sealed class Plugin : BaseBTCPayServerPlugin
{
    public override string Identifier => "NetPika.ReturnOnProcessing";
    public override string Name => "netPika Return On Processing";
    public override string Description => "Returns netPika customers to their WHMCS invoice as soon as BTCPay detects an on-chain payment.";

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
        services.AddUIExtension(
            "checkout-end",
            "/Plugins/NetPika.ReturnOnProcessing/Views/Shared/ReturnOnProcessing.cshtml");
        base.Execute(services);
    }
}
