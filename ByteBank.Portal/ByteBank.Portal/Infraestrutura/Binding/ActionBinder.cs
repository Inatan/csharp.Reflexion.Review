using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ByteBank.Portal.Infraestrutura.Binding
{
    public class ActionBinder
    {
        public object ObterMethodInfo(object controller, string path)
        {
            var idxInterrogacao = path.IndexOf('?');
            var existeQueryString = idxInterrogacao > 0;
            if (!existeQueryString)
            {
                var nomeAction = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1];

                return controller.GetType().GetMethod(nomeAction);
            }
            else
            {
                var nomeControllerComAction = path.Substring(0,idxInterrogacao);
                var nomeAction = nomeControllerComAction.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                var queryString = path.Substring(idxInterrogacao+1);

                var tuplasNomeValor = ObterArgumentoNomeValores(queryString);
                var bindingFlags = 
                    BindingFlags.Instance 
                    | BindingFlags.Static 
                    | BindingFlags.Public 
                    | BindingFlags.DeclaredOnly;

                return controller.GetType().GetMethods(bindingFlags);
            }
        }

        private IEnumerable<ArgumentoNomeValor> ObterArgumentoNomeValores(string queryString)
        {
            var tuplasNomeValor = queryString.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var tupla in tuplasNomeValor)
            {
                var partesTuplas = tupla.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                yield return new ArgumentoNomeValor(partesTuplas[0], partesTuplas[1]);
            }
        }
    }
}
