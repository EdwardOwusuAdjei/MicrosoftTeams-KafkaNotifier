using System.Threading.Tasks;

namespace AutoLagNotifier.Interfaces
{
    public interface INotifier
    {
        void ReportLag(string message);
    }
}