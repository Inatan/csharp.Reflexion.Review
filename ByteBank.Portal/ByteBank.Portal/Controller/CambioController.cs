using ByteBank.Service;
using ByteBank.Portal.Filtros;

namespace ByteBank.Portal.Controller
{
    public class CambioController : ControllerBase
    {
        private ICambioService _cambioService;
        private ICartaoService _cartaoService;

        public CambioController(ICambioService cambioService, ICartaoService cartaoService)
        { 
            _cambioService = cambioService;
            _cartaoService = cartaoService;
        }


        [ApenasHorarioComercialFilter]
        public string MXN()
        {
            var valorFinal = _cambioService.Calcular("MXN", "BRL", 1);
            return View(new
            {
                Valor = valorFinal
            });
        }

        [ApenasHorarioComercialFilter]
        public string USD()
        {
            var valorFinal = _cambioService.Calcular("USD", "BRL", 1);
            return View(new
                {
                    Valor = valorFinal
                });
        }

        [ApenasHorarioComercialFilter]
        public string Calculo(string moedaDestino) =>
            Calculo("BRL", moedaDestino, 1);

        [ApenasHorarioComercialFilter]
        public string Calculo(string moedaDestino, decimal valor) =>
            Calculo("BRL", moedaDestino, valor);

        [ApenasHorarioComercialFilter]
        public string Calculo(string moedaOrigem, string moedaDestino, decimal valor)
        {
            var valorFinal = _cambioService.Calcular(moedaOrigem, moedaDestino, valor);
            var cartaoPromocao = _cartaoService.ObterCartaoDeCreditoDeDestaque();
            var modelo = new
            {
                MoedaDestino = moedaDestino,
                MoedaOrigem = moedaOrigem,
                ValorOrigem = valor,
                ValorDestino = valorFinal,
                CartaoPromocao = cartaoPromocao
            };

            return View(modelo);
        }
    }
}
