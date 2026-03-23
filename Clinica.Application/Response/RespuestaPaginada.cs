using System;
using System.Collections.Generic;
using System.Text;

namespace Clinica.Application.Response
{
    public class RespuestaPaginada<T>
    {
        public IEnumerable<T> Elementos { get; set; } = Enumerable.Empty<T>();
        public int TotalPaginas { get; set; }
        public int NumeroPagina { get; set; }
        public int TotalElementos { get; set; }
        public int TamanioPagina { get; set; }


        public RespuestaPaginada(IEnumerable<T>  elementos, int totalElementos, int NumeroPagina, int tamanioPagina)
        {
            Elementos = elementos;
            TotalElementos = totalElementos;
            NumeroPagina = NumeroPagina<1 ? 1 : NumeroPagina;
        TamanioPagina = tamanioPagina< 1 ? 10 : tamanioPagina;
        TotalPaginas = (int) Math.Ceiling(totalElementos / (double) tamanioPagina);
    }
    }
}
