using ByteBank.Service;
using ByteBank.Service.Cambio;
using ByteBank.Portal.Filtros;

namespace ByteBank.Portal.Controller
{
    public class CambioController : ControllerBase
    {
        private ICambioService _cambioService;

        public CambioController()
        { 
            _cambioService = new CambioTesteService();
        }


        [ApenasHorarioComercial]
        public string MXN()
        {
            var valorFinal = _cambioService.Calcular("MXN", "BRL", 1);
            return View(new
            {
                Valor = valorFinal
            });
        }

        [ApenasHorarioComercial]
        public string USD()
        {
            var valorFinal = _cambioService.Calcular("USD", "BRL", 1);
            return View(new
                {
                    Valor = valorFinal
                });
        }

        [ApenasHorarioComercial]
        public string Calculo(string moedaDestino) =>
            Calculo("BRL", moedaDestino, 1);

        [ApenasHorarioComercial]
        public string Calculo(string moedaDestino, decimal valor) =>
            Calculo("BRL", moedaDestino, valor);

        [ApenasHorarioComercial]
        public string Calculo(string moedaOrigem, string moedaDestino, decimal valor)
        {
            

            var valorFinal = _cambioService.Calcular(moedaOrigem, moedaDestino, valor);

            var modelo = new
            {
                MoedaDestino = moedaDestino,
                MoedaOrigem = moedaOrigem,
                ValorOrigem = valor,
                ValorDestino = valorFinal
            };

            return View(modelo);
        }
    }
}
