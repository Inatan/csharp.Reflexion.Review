using ByteBank.Portal.Infraestrutura;

namespace ByteBank.Portal
{
    class Program
    {
        static void Main(string[] args)
        {
            var prefixo = new string[] { "http://localhost:5341/" };
            var webApplication = new WebApplication(prefixo);
            webApplication.Iniciar();
        }
    }
}
