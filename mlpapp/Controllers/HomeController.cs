﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using mlpapp.Models;
using Newtonsoft.Json;

namespace mlpapp.Controllers
{
    public class HomeController : Controller
    {
        public List<string[]> normalizados;//tirar o calculo do js e deixar no metodo Normalizar para facilitar
        
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
        public IActionResult Treinar(int entrada, int saida, int oculta, int interacao, float erro, float aprendizagem, int funcao, string normalizacao, string[] classes)
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

                var errosPorEpoca = new List<double>();

                var listaNormalizada = JsonConvert.DeserializeObject<List<string[]>>(normalizacao);

                for (int i = 0; i < neuroniosOculta.Length; i++)
                    neuroniosOculta[i] = new Neuronio(funcao);

                for (int i = 0; i < neuroniosSaida.Length; i++)
                    neuroniosSaida[i] = new Neuronio(funcao);

                int epoch = 0;
                
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
                            saidaDesejada = Convert.ToInt32(classes[j]);
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

                return Json(new {errosEpoca = errosPorEpoca});
            }
            catch (Exception e)
            {
                return Json(new {erro = e.Message});
            }
        }
    }
}