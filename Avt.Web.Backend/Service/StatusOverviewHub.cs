using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Avt.Web.Backend.Service
{
    public class StatusOverviewHub : Hub
    {
        public static int InstanceCount = 0;
        public static int ClientCount = 0;
        public static readonly object locker = new object();
        public StatusOverviewHub()
        {
            InstanceCount++;
        }

        public override Task OnConnectedAsync()
        {
            lock (locker)
            {
                ClientCount++;
            }
            System.Diagnostics.Debug.WriteLine("**************OnConnect***************");
            System.Diagnostics.Debug.WriteLine("ConnectionId: {0}, UserId: {1}, UserIdentity: {2}, ClientCount: {3}", this.Context.ConnectionId, this.Context.UserIdentifier, this.Context.User.Identity, ClientCount);
            System.Diagnostics.Debug.WriteLine("**************************************");
            return base.OnConnectedAsync();
        }

        /// <summary>Called when a connection with the hub is terminated.</summary>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous disconnect.</returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            lock (locker)
            {
                ClientCount--;
            }
            System.Diagnostics.Debug.WriteLine("**************OnDisconnect***************");
            System.Diagnostics.Debug.WriteLine("ConnectionId: {0}, UserId: {1}, UserIdentity: {2}, ClientCount: {3}, Error:{4}", this.Context.ConnectionId, this.Context.UserIdentifier, this.Context.User.Identity, ClientCount, exception);
            System.Diagnostics.Debug.WriteLine("**************************************");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
