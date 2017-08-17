using Microsoft.Extensions.DependencyInjection;
using Vecc.TimeBasedOneTimePassword.Internal;

namespace Vecc.TimeBasedOneTimePassword
{
    public static class TotpServiceCollectionExtensions
    {
        public static IServiceCollection AddTimeBasedOneTimePassword(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            serviceCollection.AddSingleton<ITotp, Totp>();

            return serviceCollection;
        }
    }
}
