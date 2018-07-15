using System;
using System.Collections.Generic;
using System.Text;

namespace ConversorDeMoedas.Services.Interface.Factory
{
    public interface IConversorServiceFactory
    {
        IConversorService Create();

    }
}
