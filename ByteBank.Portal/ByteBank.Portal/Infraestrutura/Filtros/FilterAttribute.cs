using System;

namespace ByteBank.Portal.Filtros
{
    public abstract class FilterAttribute : Attribute
    {
        public abstract bool PodeContinuar();
    }
}