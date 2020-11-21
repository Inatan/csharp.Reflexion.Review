using ByteBank.Portal.Infraestrutura.IoC;
using ByteBank.Service;
using ByteBank.Service.Cambio;
using ByteBank.Service.Cartao;
using System;
using System.Net;

namespace ByteBank.Portal.Infraestrutura
{
    public class CartaoServiceTesteContainer
    {
        public string ObterCartaoDeCreditoDeDestaque() =>
            "Cartão de Crédito do Teste de Container";
        public string ObterCartaoDeDebitoDeDestaque() =>
            "Cartão de Débito do Teste de Container";
    }

    public class WebApplication
    {
        private readonly string[] _prefixos;
        private readonly IContainer _container = new ContainerSimples();

        public WebApplication(string[] prefixos)
        {
            if(prefixos == null)
                throw new ArgumentNullException(nameof(prefixos));
            _prefixos = prefixos;

            Configurar();
        }

        private void Configurar()
        {
            _container.Registrar<ICambioService,CambioTesteService>();
            _container.Registrar<ICartaoService,CartaoServiceTeste>();
        }
        
        public void Iniciar()
        {
            while(true)
                ManipularRequisicao();

        }

        private void ManipularRequisicao()
        {
            var httpListener = new HttpListener();
            foreach (var prefixo in _prefixos)
            {
                httpListener.Prefixes.Add(prefixo);
            }

            httpListener.Start();

            var contexto = httpListener.GetContext();
            var requisicao = contexto.Request;
            var resposta = contexto.Response;

            var path = requisicao.Url.PathAndQuery;
            if(Utilidades.EhArquivo(path))
            {
                var manipulador = new ManipulaporRequisicaoArquivo();
                manipulador.Manipular(resposta, path);
            }
            else
            {
                var manipuladorController = new ManipuladorRequisicaoController(_container);
                manipuladorController.Manipular(resposta, path);
            }


            httpListener.Stop();
        }
    }
}