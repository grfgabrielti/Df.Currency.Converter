using System;
namespace ConversorDeMoedas.Infrastructure.Interface.Factory
{
    public interface IConfigurationHelperFactory
    {
        IConfigurationHelper Create();
    }
}
