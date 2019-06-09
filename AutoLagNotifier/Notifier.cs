using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AutoLagNotifier.Interfaces
{
    public class Notifier:INotifier
    {
        private readonly ILogger<Notifier> _notifier;
        public Notifier(ILogger<Notifier> notify)
        {
            _notifier = notify;
        }
        public void ReportLag(string message)
        {
           _notifier.LogError(message);
        }
    }
}