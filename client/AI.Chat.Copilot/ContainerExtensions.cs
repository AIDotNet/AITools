using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace AI.Chat.Copilot
{
    public static class ContainerExtensions
    {
        public static Disposable<T> Resolve<T>(this IServiceScope scope)
        {
            try
            {
               return new Disposable<T>(scope, (T)scope.ServiceProvider.GetRequiredService(typeof(T)));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                scope.Dispose();
            }
        }
    }
    public struct Disposable<T> : IDisposable
    {
        private IServiceScope ServiceScope { get; }
        public Disposable(IServiceScope serviceScope,T t)
        {
            ServiceScope = serviceScope ?? throw new ArgumentNullException();
            Value = t;
        }
        public T Value { get; private set; }
        public void Dispose()
        {
           ServiceScope?.Dispose();
        }
    }
}
