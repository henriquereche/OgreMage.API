using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OgreMage.API.HostedServices.Base
{
    /// <summary>
    /// Timed hosted service.
    /// </summary>
    public abstract class TimedHostedService : IHostedService, IDisposable
    {
        private readonly TimeSpan Delay;
        private readonly TimeSpan interval;

        protected TimedHostedService(TimeSpan delay, TimeSpan interval)
        {
            this.Delay = delay;
            this.interval = interval;
        }

        private Timer Timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.Timer = new Timer(
                this.DoWork,
                null,
                this.Delay,
                this.interval
            );

            return Task.CompletedTask;
        }

        /// <summary>
        /// Hosted service work.
        /// </summary>
        /// <param name="state"></param>
        public abstract void DoWork(object state);

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.Timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this.Timer?.Dispose();
        }
    }
}
