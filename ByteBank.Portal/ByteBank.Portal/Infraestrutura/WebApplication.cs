using ByteBank.Portal.Controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace ByteBank.Portal.Infraestrutura
{
    public class WebApplication
    {
        // http://bytebank.com/ ?
        // http://localhost:80/ ?


        private readonly string[] _prefixos;

        public WebApplication(string[] prefixos)
        {
            if(prefixos == null)
                throw new ArgumentNullException(nameof(prefixos));
            _prefixos = prefixos;
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

            var path = requisicao.Url.AbsolutePath;
            if(Utilidades.EhArquivo(path))
            {
                var manipulador = new ManipulaporRequisicaoArquivo();
                manipulador.Manipular(resposta, path);
            }
            else
            {
                var manipuladorController = new ManipuladorRequisicaoController();
                manipuladorController.Manipular(resposta, path);
            }


            httpListener.Stop();
        }
    }
}