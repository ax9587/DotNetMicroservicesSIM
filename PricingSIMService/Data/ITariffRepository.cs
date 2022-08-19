using PricingSIMService.Model;
using System.Threading.Tasks;

namespace PricingSIMService.Data
{
    public interface ITariffRepository
    {
        Task<Tariff> WithCode(string code);
        
        Task<Tariff> this[string code] { get; }

        void Add(Tariff tariff);
        
        Task<bool> Exists(string code);
    }
}
