using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace Otus.Teaching.Pcf.Administration.WebHost.BackgroundService;

public class GivingPromoCodeToCustomerListener : IHostedService
{
    private IBusControl _busControl;

    public GivingPromoCodeToCustomerListener(IBusControl busControl)
    {
        _busControl = busControl;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _busControl.StartAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _busControl.StartAsync(cancellationToken);
    }
}