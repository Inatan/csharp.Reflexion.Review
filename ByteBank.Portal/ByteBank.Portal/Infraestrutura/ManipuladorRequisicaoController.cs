using ByteBank.Portal.Infraestrutura.Binding;
using ByteBank.Portal.Infraestrutura.Filtros;
using System;
using System.Net;
using System.Text;

namespace ByteBank.Portal.Infraestrutura
{
    public class ManipuladorRequisicaoController
    {
        private readonly ActionBinder _actionBinder = new ActionBinder();
        private readonly FilterResolver _filterResolver = new FilterResolver();

        public void Manipular(HttpListenerResponse resposta, string path)
        {
            var partes = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var controllerNome = partes[0];
            var actionNome = partes[1];
            var controllerNomeCompleto = $"ByteBank.Portal.Controller.{controllerNome}Controller"; //.{actionNome}
            var controllerWrapper = Activator.CreateInstance("ByteBank.Portal", controllerNomeCompleto, new object[0]);
            var controller = controllerWrapper.Unwrap();

            var actionBindInfo = _actionBinder.ObterActionBindInfo(controller,path);//controller.GetType().GetMethod(actionNome);

            var filterResult = _filterResolver.VerificarFiltros(actionBindInfo);

            if (filterResult.PodeContinuar)
            {
                var resultadoAction = (string)actionBindInfo.Invoke(controller);

                var bufferArquivo = Encoding.UTF8.GetBytes(resultadoAction);
                resposta.StatusCode = HttpStatusCode.OK.GetHashCode();
                resposta.ContentType = "text/html; charset=utf-8";
                resposta.ContentLength64 = bufferArquivo.Length;

                resposta.OutputStream.Write(bufferArquivo, 0, bufferArquivo.Length);
            }
            else
            {
                resposta.StatusCode = HttpStatusCode.TemporaryRedirect.GetHashCode();
                resposta.RedirectLocation = "/Erro/Inesperado";
            }
            resposta.OutputStream.Close();

        }
    }
}
