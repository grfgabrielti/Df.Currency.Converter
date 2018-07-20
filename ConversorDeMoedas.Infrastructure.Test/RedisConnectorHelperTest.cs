using ConversorDeMoedas.Domain;
using ConversorDeMoedas.Domain.Interface;
using ConversorDeMoedas.Infrastructure.Factory;
using ConversorDeMoedas.Infrastructure.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace ConversorDeMoedas.Infrastructure.Test
{
    public class RedisConnectorHelperTest
    {
        [Fact]
        public void GetComRetornoInformandoOTipoDoObjeto()
        {
            IMoeda moeda = new Moeda("BRL", 3.85M);
            Mock<IDistributedCache> mckcache = new Mock<IDistributedCache>();
            mckcache.Setup(x => x.Get("GetObjetoDeserializadoDo")).Returns(Serialize(moeda));
            IRedisConnectorHelper redisHelper = new RedisConnectorHelper(mckcache.Object);
            var result = redisHelper.Get<IMoeda>("GetObjetoDeserializadoDo");
            Assert.True(moeda.Equals(result));

        }
        [Fact]
        public void GetComRetornoSemInformarOTipo()
        {
            IMoeda moeda = new Moeda("BRL", 3.85M);
            Mock<IDistributedCache> mckcache = new Mock<IDistributedCache>();
            mckcache.Setup(x => x.Get("GetObjetoDeserializado")).Returns(Serialize(moeda));
            IRedisConnectorHelper redisHelper = new RedisConnectorHelper(mckcache.Object);
            var result = redisHelper.Get("GetObjetoDeserializado");
            var resultDescerialize = Deserialize<IMoeda>(result);
            Assert.True(moeda.Equals(resultDescerialize));

        }
        public void TestGetComRetornDoTipoString()
        {

            //Teste deve analisar o retorno do metodo GetString 
            //Não estou conseguindo definir uma variavel string para retorno mock do redis
            //Mas funciona.. 
            //Precisa analisar melhor esse teste com calma...
        }
        [Fact]
        public void TestStandoValoresNulosOuEmBranco()
        {
            IMoeda MoedaNull = null;
            IMoeda Moeda = new Moeda("USD", 1);
            Mock<IDistributedCache> mckcache = new Mock<IDistributedCache>();
            IRedisConnectorHelper redisHelper = new RedisConnectorHelper(mckcache.Object);

            Assert.Throws<Exception>(() => redisHelper.Set("Teste", MoedaNull));
            Assert.Throws<Exception>(() => redisHelper.Set("", Moeda));
            Assert.Throws<Exception>(() => redisHelper.Set("", MoedaNull));
        }
        [Fact]
        public void TestStandoValoresNulosOuEmBrancoComTempoDeAmarzenamento()
        {
            IMoeda MoedaNull = null;
            IMoeda Moeda = new Moeda("USD", 1);
            Mock<IDistributedCache> mckcache = new Mock<IDistributedCache>();
            IRedisConnectorHelper redisHelper = new RedisConnectorHelper(mckcache.Object);

            Assert.Throws<Exception>(() => redisHelper.Set("Teste", MoedaNull, 1));
            Assert.Throws<Exception>(() => redisHelper.Set("", Moeda, 1));
            Assert.Throws<Exception>(() => redisHelper.Set("", MoedaNull, 1));
        }
        [Fact]
        public void TestStandoValorStringNulosOuEmBranco()
        {
            Mock<IDistributedCache> mckcache = new Mock<IDistributedCache>();
            IRedisConnectorHelper redisHelper = new RedisConnectorHelper(mckcache.Object);

            Assert.Throws<Exception>(() => redisHelper.SetString("Teste", ""));
            Assert.Throws<Exception>(() => redisHelper.SetString("", "Teste"));
            Assert.Throws<Exception>(() => redisHelper.SetString("", ""));
        }
        [Fact]
        public void TestStandoValorStringNulosOuEmBrancoComTempoDeAmarzenamento()
        {
            Mock<IDistributedCache> mckcache = new Mock<IDistributedCache>();
            IRedisConnectorHelper redisHelper = new RedisConnectorHelper(mckcache.Object);

            Assert.Throws<Exception>(() => redisHelper.SetString("Teste", "", 1));
            Assert.Throws<Exception>(() => redisHelper.SetString("", "Teste", 1));
            Assert.Throws<Exception>(() => redisHelper.SetString("", "", 1));
        }

        private static byte[] Serialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            BinaryFormatter objBinaryFormatter = new BinaryFormatter();
            using (MemoryStream objMemoryStream = new MemoryStream())
            {
                objBinaryFormatter.Serialize(objMemoryStream, obj);
                byte[] objDataAsByte = objMemoryStream.ToArray();
                return objDataAsByte;
            }
        }
        public static T Deserialize<T>(byte[] bytes)
        {
            BinaryFormatter objBinaryFormatter = new BinaryFormatter();
            if (bytes == null)
                return default(T);

            using (MemoryStream objMemoryStream = new MemoryStream(bytes))
            {
                T result = (T)objBinaryFormatter.Deserialize(objMemoryStream);
                return result;
            }
        }


    }
}
