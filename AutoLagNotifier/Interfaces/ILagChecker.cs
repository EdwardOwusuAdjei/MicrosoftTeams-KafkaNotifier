using System.Threading.Tasks;
using AutoLagNotifier.Models.Response;

namespace AutoLagNotifier.Interfaces
{
    public interface ILagChecker
    {
        Task CheckLags();
    }
}