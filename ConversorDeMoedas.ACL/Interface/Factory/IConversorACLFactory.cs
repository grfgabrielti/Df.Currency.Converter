using System;
using System.Collections.Generic;
using System.Text;

namespace ConversorDeMoedas.ACL.Interface.Factory
{
    public interface IConversorACLFactory
    {
        IConversorACL Create();
    }
}
