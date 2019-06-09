using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoLagNotifier.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AutoLagNotifier
{
    public class CheckService:ICheckService,IHostedService
    {
        private readonly ILogger<CheckService> _logger;
        private readonly ILagChecker _lagChecker;
        public CheckService(ILogger<CheckService> logger,ILagChecker lagChecker)
        {
            _logger = logger;
            _lagChecker = lagChecker;
        }
        public async Task Start()
        {
            await _lagChecker.CheckLags();
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Home now");
            await Start();
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

      
    }
}