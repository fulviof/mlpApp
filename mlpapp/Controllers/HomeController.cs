using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using mlpapp.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace mlpapp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
        
        [HttpPost]
        public IActionResult Normalizar()
        {
            try
            {
                var file = HttpContext.Request.Form.Files[0];        
                var result = string.Empty;
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    result = reader.ReadToEnd();  
                }

                var list = result.Split("\r\n");
                
                var colunas = list[0].Split(",");
                
                var vetorResultados = new int[(colunas.Length - 1)*3];

                var classes = new int[list.Length - 1];

                for (var i = 1; i < list.Length-1; i++)
                {
                    var linha = list[i].Split(",");
                    classes.SetValue(Convert.ToInt32(linha[linha.Length-1]), i-1);
                }
                
                for (var i = 0; i<colunas.Length - 1; i++)
                {              
                    var maior = -999999;
                    var menor = 999999;   
                    for (var j = 1; j < list.Length - 1; j++)
                    {
                        var linha = list[j].Split(",");
                        
                        if(Convert.ToInt32(linha[i]) > maior)
                            maior = Convert.ToInt32(linha[i]);
                        
                        if(Convert.ToInt32(linha[i]) < menor)
                            menor = Convert.ToInt32(linha[i]);
                    }

                    vetorResultados.SetValue(maior, i*3);
                    vetorResultados.SetValue(menor, i*3+1);
                    vetorResultados.SetValue(maior-menor, i*3+2);
                }

                return Json(new {dados = list, resultados = vetorResultados, clas = classes, qtde = colunas.Length - 1});
            }
            catch (Exception e)
            {
                return Json(new {erro = e.Message});
            }
        }

        [HttpPost]
        public IActionResult Treinar(int entrada, int saida, int oculta, int interacao, double erro, double aprendizagem, int funcao, string normalizacao, string[] classes)
        {
            try
            {
                double erroRede = 0;
                double saidaObtida = 0;
                double erroNeuronio = 0;
                double derivada= 0;
                double novoPeso = 0; 
                double mediaErros = 0;
                int saidaDesejada = 0;
                double aux = 0;
                bool primeira = true;
                
                var neuroniosSaida = new Neuronio[saida];
                var neuroniosOculta = new Neuronio[oculta];
                
                var qtde = new List<string>();
            
                for (int i = 0; i < classes.Length-1; i++)
                {
                    if (!qtde.Contains(classes[i]))
                    {
                        qtde.Add(classes[i]);
                    }
                }

                int[,] matrix = new int[saida, qtde.Count];
                var errosPorEpoca = new List<double>();

                var listaNormalizada = JsonConvert.DeserializeObject<List<string[]>>(normalizacao);

                for (int i = 0; i < neuroniosOculta.Length; i++)
                    neuroniosOculta[i] = new Neuronio(funcao);

                for (int i = 0; i < neuroniosSaida.Length; i++)
                    neuroniosSaida[i] = new Neuronio(funcao);

                int epoch = 0;

                for (int i = 0; i < saida; i++)
                {
                    for (int j = 0; j < saida; j++)
                    {
                        if (i == j)
                            matrix[i, j] = 1;
                        else
                        {
                            matrix[i, j] = 0;
                        }
                    }
                }
                
                do
                {
                    mediaErros = 0;
                    for (int j = 0; j < listaNormalizada.Count; j++)
                    {
                        erroRede = 0;
                        var entradasOculta = listaNormalizada[j];
                        
                        //insere as entradas na camada oculta com seus pesos
                        for(int k = 0; k < neuroniosOculta.Length; k++)
                        {
                            //insere as entradas no neuronio
                            neuroniosOculta[k].setEntradas(entradasOculta, primeira);
                        }

                        var entradasSaida = new string[neuroniosOculta.Length];
                        
                        //cria as entradas da camada de saida a partir da camada oculta
                        for(int k = 0; k < neuroniosOculta.Length; k++)
                        {
                            entradasSaida[k] = Convert.ToString(neuroniosOculta[k].i);
                        }
                        
                        //insere as entradas na camada de saida com seus pesos
                        for(int k = 0; k < neuroniosSaida.Length; k++)
                        {
                            //insere as entradas no neuronio
                            neuroniosSaida[k].setEntradas(entradasSaida, primeira);
                        }
                        
                        //erro do neuronio de saida
                        for(int k = 0; k < neuroniosSaida.Length; k++)
                        {
                            saidaObtida = neuroniosSaida[k].i;
                            saidaDesejada = matrix[Convert.ToInt32(classes[j]) - 1, k];
                            derivada = neuroniosSaida[k].derivada;

                            erroNeuronio = (saidaDesejada - saidaObtida) * derivada;
                            neuroniosSaida[k].erro = erroNeuronio;
                        }
                        
                        //erro da rede
                        for (int k = 0; k < neuroniosSaida.Length; k++)
                             erroRede += Math.Pow(neuroniosSaida[k].erro, 2);

                        mediaErros += erroRede * 0.5;

                        //erro camada oculta
                        for (int k = 0; k < neuroniosOculta.Length; k++)
                        {
                            erroNeuronio = 0;

                            for (int x = 0; x < neuroniosSaida.Length; x++)
                                erroNeuronio += neuroniosSaida[x].erro * neuroniosSaida[x].pesosEntradas[k];

                            erroNeuronio = erroNeuronio * neuroniosOculta[k].derivada;

                            neuroniosOculta[k].erro = erroNeuronio;
                        }

                        //atualiza pesos da camada de saida
                        for (int k = 0; k < neuroniosSaida.Length; k++)
                        {
                            for (int x = 0; x < neuroniosSaida[k].entradas.Length; x++)
                            {
                                novoPeso = (aprendizagem * neuroniosSaida[k].erro * neuroniosSaida[k].entradas[x]) + neuroniosSaida[k].pesosEntradas[x];
                                neuroniosSaida[k].pesosEntradas.SetValue(novoPeso, x);
                            }
                        }
                        
                        //atualiza pesos da camada oculta
                        for (int k = 0; k < neuroniosOculta.Length; k++)
                        {
                            for (int x = 0; x < neuroniosOculta[k].entradas.Length; x++)
                            {
                                novoPeso = (aprendizagem * neuroniosOculta[k].erro * neuroniosOculta[k].entradas[x]) + neuroniosOculta[k].pesosEntradas[x];
                                neuroniosOculta[k].pesosEntradas.SetValue(novoPeso, x);
                            }
                        }

                        primeira = false;
                    }

                    aux = mediaErros / listaNormalizada.Count;
                    errosPorEpoca.Add(aux);
                    epoch++;
                }
                while (epoch < interacao && aux > erro) ;
                
                return Json(new {errosEpoca = errosPorEpoca, neuroniosSaida = neuroniosSaida, neuroniosOculta = neuroniosOculta});
            }
            catch (Exception e)
            {
                return Json(new {erro = e.Message});
            }
        }

        [HttpPost]
        public IActionResult CarregaArquivoTeste()
        {
            var file = HttpContext.Request.Form.Files[0];        
            var result = string.Empty;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                result = reader.ReadToEnd();  
            }

            var list = result.Split("\r\n");
            
            var colunas = list[0].Split(",");
                
            var vetorResultados = new int[(colunas.Length - 1)*3];

            var classes = new int[list.Length - 1];

            for (var i = 1; i < list.Length - 1; i++)
            {
                var linha = list[i].Split(",");
                classes.SetValue(Convert.ToInt32(linha[linha.Length-1]), i-1);
            }
                
            for (var i = 0; i<colunas.Length - 1; i++)
            {              
                var maior = -999999;
                var menor = 999999;   
                for (var j = 1; j < list.Length - 1; j++)
                {
                    var linha = list[j].Split(",");
                        
                    if(Convert.ToInt32(linha[i]) > maior)
                        maior = Convert.ToInt32(linha[i]);
                        
                    if(Convert.ToInt32(linha[i]) < menor)
                        menor = Convert.ToInt32(linha[i]);
                }

                vetorResultados.SetValue(maior, i*3);
                vetorResultados.SetValue(menor, i*3+1);
                vetorResultados.SetValue(maior-menor, i*3+2);
            }
            
            return Json(new {dados = list, resultados = vetorResultados, clas = classes, qtde = colunas.Length - 1});
        }
        
        [HttpPost]
        public IActionResult Testar(string normalizacao, string classes, string neuroniosSaida, string neuroniosOculta, int saida)
        {     
            var dados = JsonConvert.DeserializeObject<List<string[]>>(normalizacao);
            var clas = JsonConvert.DeserializeObject<int[]>(classes);
            var pesosSaida = JsonConvert.DeserializeObject<List<double[]>>(neuroniosSaida);
            var pesosOculta = JsonConvert.DeserializeObject<List<double[]>>(neuroniosOculta);
            
            var qtde = new List<int>();
            
            for (int i = 0; i < clas.Length-1; i++)
            {
                if (!qtde.Contains(clas[i]))
                {
                    qtde.Add(clas[i]);
                }
            }

            foreach (string[] a in dados)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i] == "N")
                    {
                        a[i] = "0";
                    }
                }
            }

            int[,] matrix = new int[qtde.Count, pesosSaida.Count];

            var neuroOculta = new Neuronio[pesosOculta.Count];
            var neuroSaida = new Neuronio[pesosSaida.Count];
            
            for (int i = 0; i < dados.Count - 1; i++)
            {
                var entradasSaida = new string[neuroOculta.Length];   
                var entradasOculta = dados[i];

                //entradas oculta
                for (int j = 0; j < neuroOculta.Length; j++)
                {
                    neuroOculta[j] = new Neuronio(saida);
                    neuroOculta[j].setEntradasPesos(entradasOculta, pesosOculta.ElementAt(j)); 
                }

                for (int j = 0; j < neuroOculta.Length; j++)
                {
                    entradasSaida[j] = Convert.ToString(neuroOculta[j].i);
                }
                   
                //insere as entradas na camada de saida com seus pesos
                for(int j = 0; j < neuroSaida.Length; j++)
                {
                    neuroSaida[j] = new Neuronio(saida);
                    neuroSaida[j].setEntradasPesos(entradasSaida, pesosSaida.ElementAt(j));
                }

                int maior = 0;
                
                for (int j = 1; j < neuroSaida.Length; j++)
                {
                    if (neuroSaida[j].i > neuroSaida[maior].i)
                    {
                        maior = j;
                    }
                }

                matrix[clas[i]-1, maior] += 1;
            }

            return Json(new {matrix = matrix, linha = qtde.Count, coluna = pesosSaida.Count});
        }
    }
}