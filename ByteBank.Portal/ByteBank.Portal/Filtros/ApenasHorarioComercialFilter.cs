using System;

namespace ByteBank.Portal.Filtros
{
    public class ApenasHorarioComercialFilterAttribute : FilterAttribute
    {

        public override bool PodeContinuar()
        {
            var datacCorrente = DateTime.Now;
            var hora = datacCorrente.Hour;

            return (hora < 16 && hora >= 9);
        }
    }
}
