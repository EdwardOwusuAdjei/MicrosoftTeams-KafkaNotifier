using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using AutoLagNotifier.Interfaces;
using AutoLagNotifier.Models.Response;
using AutoLagNotifier.Setting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AutoLagNotifier
{
    public class LagChecker : ILagChecker
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<BurrowEndpoints> _burrowConfig;
        private readonly ILogger<LagChecker> _logger;
        private readonly INotifier _notifier;
        private readonly IOptions<Lag> _lagConfig;
        private readonly IOptions<FilterName> _names;
        private readonly int _refreshSeconds;

        public LagChecker(HttpClient httpClient, IOptions<Config> config, ILogger<LagChecker> logger,
            IOptions<BurrowEndpoints> burrowConfig, INotifier notifier, IOptions<Lag> lagConfig,IOptions<FilterName> names)
        {
            _httpClient = httpClient;
            _burrowConfig = burrowConfig;
            _notifier = notifier;
            _names = names;
            _logger = logger;
            _lagConfig = lagConfig;
            _refreshSeconds = config.Value.Delay;
            var timer = new Timer
            {
                Interval = config.Value.Delay * 1000
            };
            timer.Elapsed += Check;
            timer.Enabled = true;
        }

        private void Check(object sender, ElapsedEventArgs e)
        {
            Task.Run(CheckLags);
        }

        private void ReportLag(LagResponseModel lagResponseModel)
        {
            _notifier.ReportLag($"The Consumers with GroupId as {lagResponseModel.Status.Group} with total lag {lagResponseModel.Status.Totallag.ToString()}" +
                                $" which is equal to or above the set limit  {_lagConfig.Value.MaxLagAlert.ToString()} checking in the next {_refreshSeconds.ToString()} secs"+
                                $"\n\n\n\nFull response is \n\n " +
                                $"{JsonConvert.SerializeObject(lagResponseModel)}" +
                                $"");
        }


        public async Task CheckLags()
        {
            try
            {
                var response = await _httpClient.GetAsync(_burrowConfig.Value.ListConsumers);
                var consumerResponse =
                    JsonConvert.DeserializeObject<ConsumerListModel>(await response.Content.ReadAsStringAsync());
                
                if (consumerResponse.Error is false)
                {
                    consumerResponse.Consumers = consumerResponse.Consumers
                        .Where(x => x.Contains(_names.Value.IgnoreCaseFilterNames,StringComparison.OrdinalIgnoreCase))
                        .Select(x => x);
                    _logger.LogInformation($"{JsonConvert.SerializeObject(consumerResponse.Consumers)}");
                    Parallel.ForEach(consumerResponse.Consumers, async (consumer) =>
                    {
                        async Task<HttpResponseMessage> Function()
                        {
                            var result = await _httpClient.GetAsync(_burrowConfig.Value.LagStatus
                                .Replace("(cluster)", "local").ToString().Replace("(group)", consumer));
                            return result;
                        }
                        
                        var lagResponse = await Function();
                        var lagResponseModel =
                            JsonConvert.DeserializeObject<LagResponseModel>(
                                await lagResponse.Content.ReadAsStringAsync());
                        
                        if (lagResponseModel.Status.Totallag >= _lagConfig.Value.MaxLagAlert)
                        {
                            ReportLag(lagResponseModel);
                        }
                    });
                }
                else
                {
                    _logger.LogError($"{JsonConvert.SerializeObject(consumerResponse)}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

    }
}