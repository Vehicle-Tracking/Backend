using System;
using System.Threading;
using System.Threading.Tasks;
using Avt.Web.Backend.DTO;
using Avt.Web.Backend.Helper;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace Avt.Web.Backend.Service
{
    /// <summary>
    /// we have couple of other options for listening to the queue, may be setting up the listener in a web app is not 
    /// a good idea and we could have a listener in a separate service and as soon as receiving any new message notify 
    /// the web app vi a ** Webhook call ** which was rather a real-time notification, but that was the matter of time 
    /// which i had to manage :)
    /// </summary>
    public class LongRunningWorkerService : IHostedService, IDisposable
    {
        private Task _runningTask;
        private CancellationTokenSource _cts;

        private readonly PingService _pingService;
        private readonly OverviewService _overviewService;

        public LongRunningWorkerService(PingService pingService, OverviewService overviewService, IHubContext<StatusOverviewHub> hub)
        {
            _pingService = pingService;
            _overviewService = overviewService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _runningTask = DoWorkAsync(_cts.Token);

            return _runningTask.IsCompleted ? _runningTask : Task.CompletedTask;
        }

        private async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // since we set a timer to wait for a new message, we might call it long-polling approach
                    await AwsSqsReceiver.OnReceive(async (msgBody) =>
                    {
                        var dto = JsonHelper.ReadObject<AwsSnsMessageDto>(msgBody);
                        var status = JsonHelper.ReadObject<VehicleStatusDto>(dto.Message);

                        // concurrent task execution
                        // because they are doing different jobs in separate context, they can do their job in parallel

                        // NOTICE: i really like to make this part two different application, since one is just receiving and echoing the message
                        // and the other create aggregate report of the input message, so we can make it a front-door application which is reponsible
                        // for real-time comminication with the clients and a receiver app which receives the incomming messages and if it needs no extra
                        // process they send it via a ** Webhook call ** to the front-door app then the front-doot app will trigger and event as receiving
                        // Webhook request and update the client, if the incomming messsage needs extra process so after finishing extra proccess like the same
                        // which is doing here to create aggregate data, then the same process for notifying front-door app will be done
                        await Task.WhenAll(
                            _pingService.SendVehicleStatusAsync(status),
                            _overviewService.SendVehicleOverviewStatusAsync(status)
                        );
                    });
                }
                catch (Exception exception)
                {
                    // ToDo: log the error
                    // something went wrong, do you care? maybe! but let's give it one more try in the next run
                }

                await Task.Delay(TimeSpan.FromMilliseconds(3000), cancellationToken);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_runningTask == null)
            {
                return;
            }


            _cts.Cancel();
            await Task.WhenAny(_runningTask, Task.Delay(-1, cancellationToken));
            cancellationToken.ThrowIfCancellationRequested();
        }

        public void Dispose()
        {
        }
    }
}
