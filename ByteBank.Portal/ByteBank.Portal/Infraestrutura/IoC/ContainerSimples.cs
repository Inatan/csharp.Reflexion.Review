using System;
using System.Collections.Generic;
using System.Linq;

namespace ByteBank.Portal.Infraestrutura.IoC
{
    public class ContainerSimples : IContainer
    {
        private readonly Dictionary<Type, Type> _mapaDeTipos = new Dictionary<Type, Type>();

        public object Recuperar(Type tipoOrigem)
        {
            var tipoOrigemFoiMapeado = _mapaDeTipos.ContainsKey(tipoOrigem);
            if(tipoOrigemFoiMapeado)
            {
                var tipoDestino = _mapaDeTipos[tipoOrigem];
                return Recuperar(tipoDestino);
            }

            var construtores = tipoOrigem.GetConstructors();
            var construtorSemParametro = construtores.FirstOrDefault(c => c.GetParameters().Any() == false);

            if(construtorSemParametro != null)
            {
                var instanciaDeSemParametros = construtorSemParametro.Invoke(new object[0]);
                return instanciaDeSemParametros;
            }


            var construtorUso = construtores[0];
            var parametrosDoConstrutor = construtorUso.GetParameters();
            var countParametros = parametrosDoConstrutor.Count();
            var valoresDeParametros = new object[countParametros];

            for (int i = 0; i < countParametros; i++)
            {
                var parametro = parametrosDoConstrutor[i];
                var tipoParametro = parametro.ParameterType;
                valoresDeParametros[i] = Recuperar(tipoParametro);
            }

            var instancia = construtorUso.Invoke(valoresDeParametros);
            return instancia;
        }

        private void VerificarHierarquiaOuLancarExcecao(Type tipoOrigem, Type tipoDestino)
        {
            if (tipoOrigem.IsInterface)
            {
                var tipoDestinoImplementaInterface = 
                    tipoDestino
                        .GetInterfaces()
                        .Any(td => td == tipoOrigem);
                if (!tipoDestinoImplementaInterface)
                    throw new InvalidOperationException("O tipo destino não implementa a interface");

            }
            else
            {
               var tipoDestinoHerdaTipoOrigem = tipoDestino.IsSubclassOf(tipoOrigem);
               if(!tipoDestinoHerdaTipoOrigem)
                    throw new InvalidOperationException("O tipo destino não herda o tipo origem");
            }
        }
        public void Registrar<TOrigem, TDestino>() where TDestino : TOrigem
        {
            if (_mapaDeTipos.ContainsKey(typeof(TOrigem)))
                throw new InvalidOperationException("Tipo já mapeado!");

            VerificarHierarquiaOuLancarExcecao(typeof(TOrigem), typeof(TDestino));

            _mapaDeTipos.Add(typeof(TOrigem), typeof(TDestino));
        }

        public void Registrar(Type tipoOrigem, Type tipoDestino)
        {
            if (_mapaDeTipos.ContainsKey(tipoOrigem))
                throw new InvalidOperationException("Tipo já mapeado!");

            VerificarHierarquiaOuLancarExcecao(tipoOrigem, tipoDestino);

            _mapaDeTipos.Add(tipoOrigem, tipoDestino);
        }

    }
}
