using System;
using ConversorDeMoedas.Infrastructure.Interface;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace ConversorDeMoedas.Infrastructure.Test
{
    public class ConfigurationHelperTest
    {
        [Fact]
        public void GetSectionComRetorno()
        {
            Mock<IConfiguration> mckConfiguration= new Mock<IConfiguration>();
            mckConfiguration.Setup(x => x.GetSection("KeyValue").Value).Returns("Retorno Do Valor Desejado");

            IConfigurationHelper configurationHelper = new ConfigurationHelper(mckConfiguration.Object);
            String result = configurationHelper.GetSection("KeyValue");

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal("Retorno Do Valor Desejado", result);

        }
    
    }
}
