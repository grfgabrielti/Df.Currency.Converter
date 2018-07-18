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
            var result  = redisHelper.Get<IMoeda>("GetObjetoDeserializadoDo");
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
        [Fact]
        public void GetQueDeveMeRetornarUmaString()
        {
            String StringDeRetorno = "Eu não sei o que escrever aqui mas eu sei que esse metodo deve me retornar uma string por isso estou aqui.";
            Mock<IDistributedCache> mckcache = new Mock<IDistributedCache>();
            mckcache.Setup(x => x.GetString("TestandoGet")).Returns("oi");

            IRedisConnectorHelper redisHelper = new RedisConnectorHelper(mckcache.Object);
            var result = redisHelper.GetString("TestandoGet");
            Assert.Equal(StringDeRetorno, result);

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
