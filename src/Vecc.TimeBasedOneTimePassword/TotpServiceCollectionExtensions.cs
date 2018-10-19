using Vecc.TimeBasedOneTimePassword;
using Vecc.TimeBasedOneTimePassword.Internal;

namespace Microsoft.Extensions.DependencyInjection
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
