using System.Net;
using System.Reflection;

namespace ByteBank.Portal.Infraestrutura
{
    public class ManipulaporRequisicaoArquivo
    {
        public void Manipular(HttpListenerResponse resposta, string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var nomeResource = Utilidades.ConverterPathParaNomeAssembly(path);
            var resourceStream = assembly.GetManifestResourceStream(nomeResource);
           

            if (resourceStream == null)
            {
                resposta.StatusCode = HttpStatusCode.BadRequest.GetHashCode();
            }
            else
            {
                using (resourceStream)
                {
                    var bytesResource = new byte[resourceStream.Length];

                    resourceStream.Read(bytesResource, 0, bytesResource.Length);

                    resposta.ContentType = Utilidades.ObterTipoDeConteudo(path);
                    resposta.StatusCode = HttpStatusCode.OK.GetHashCode();
                    resposta.ContentLength64 = resourceStream.Length;

                    resposta.OutputStream.Write(bytesResource, 0, bytesResource.Length);

                    resposta.OutputStream.Close();
                }

            }
        }
    }
}
