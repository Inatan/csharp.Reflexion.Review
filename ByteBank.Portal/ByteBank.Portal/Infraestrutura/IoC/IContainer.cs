using System;

namespace ByteBank.Portal.Infraestrutura.IoC
{
    public interface IContainer
    {
        void Registrar(Type tipoOrigem, Type tipoDestino);

        object Recuperar(Type tipoOrigem);
    }
}
