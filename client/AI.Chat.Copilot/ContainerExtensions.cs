using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace AI.Chat.Copilot
{
    //public static class ContainerExtensions
    //{
    //    public static Disposable<T> Resolve<T>(this IServiceScope scope)
    //    {
    //        try
    //        {
    //           return new Disposable<T>(scope, (T)scope.ServiceProvider.GetRequiredService(typeof(T)));
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //        finally
    //        {
    //            scope.Dispose();
    //        }
    //    }

    //    public static Disposable<T> ResolveKeyed<T>(this IServiceScope scope,object serviceKey)
    //    {
    //        try
    //        {
    //            return new Disposable<T>(scope, (T)scope.ServiceProvider.GetKeyedService<T>(serviceKey));
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //        finally
    //        {
    //            scope.Dispose();
    //        }
    //    }
    //}
    //public struct Disposable<T> : IDisposable
    //{
    //    private IServiceScope ServiceScope { get; }
    //    public Disposable(IServiceScope serviceScope,T t)
    //    {
    //        ServiceScope = serviceScope ?? throw new ArgumentNullException();
    //        Value = t;
    //    }
    //    public T Value { get; private set; }
    //    public void Dispose()
    //    {
    //       ServiceScope?.Dispose();
    //    }
    //}

    public struct DisposableScope : IDisposable
    {
        private IServiceScope ServiceScope { get; }
        public DisposableScope(IServiceScope serviceScope)
        {
            ServiceScope = serviceScope ?? throw new ArgumentNullException();
        }
        public T Resolve<T>()
        {
           return  (T)ServiceScope.ServiceProvider.GetRequiredService(typeof(T));
        }

        public T ResolveKeyed<T>(object serviceKey)
        {
            return (T)ServiceScope.ServiceProvider.GetKeyedService<T>(serviceKey);
        }

        public void Dispose()
        {
            ServiceScope?.Dispose();
        }
    }
}
