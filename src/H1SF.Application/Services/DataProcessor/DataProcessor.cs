using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1SF.Application.Services
{

    public class DataProcessor
    {
        public void TrocaPontoCorpo(Dictionary<string, string> data)
        {
            Console.WriteLine("770-00-TROCA-PONTO-CORPO");

            var keys = new List<string>
        {
            "WM01-PRECO-TOTAL-M",
            "WM01-VL-AJUSTE-PRECO-TOTAL-M",
            "WM01-VL-FRETE",
            "WM01-VL-OUTRAS-DESPESAS",
            "WM01-VL-SEGURO",
            "WM01-VL-MANAUS-A",
            "WM01-VL-MANAUS-B",
            "WM01-VL-MANAUS-C",
            "WM01-VL-MANAUS-D",
            "WM01-VL-TOTAL-BASE-ICMS",
            "WM01-VL-TOTAL-BASE-STF",
            "WM01-VL-TOTAL-CONTABIL",
            "WM01-VL-TOTAL-ICMS",
            "WM01-VL-TOTAL-IPI",
            "WM01-VL-TOTAL-STF",
            "WM01-PESO-BRUTO-KG",
            "WM01-PESO-LIQUIDO-KG"
        };

            foreach (var key in keys)
            {
                if (data.ContainsKey(key))
                {
                    data[key] = data[key].Replace('.', '!').Replace(',', '.').Replace('!', ',');
                }
            }
        }
    }
}
