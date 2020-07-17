using CryptoMonitor.Enums;

namespace CryptoMonitor.Domain.Services.Interfaces
{
    public interface IPriceCalculator
    {
        decimal CalculateMedianForNewCoinValue(decimal amount, Exchanges exchange);
    }
}