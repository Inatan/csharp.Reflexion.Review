﻿using System;

namespace ByteBank.Service.Cartao
{
    public class CartaoServiceTeste : ICartaoService
    {
        public string ObterCartaoDeCreditoDeDestaque() =>
            "ByteBank Gold Platinum Extra Premium Special";

        public string ObterCartaoDeDebitoDeDestaque() =>
            "ByteBank Estudante Sem Taxas de Manutenção";
        
    }
}
