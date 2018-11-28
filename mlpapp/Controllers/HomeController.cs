using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using mlpapp.Models;

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
                    classes.SetValue(Convert.ToInt32(linha[linha.Length-1]), i);
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
        public IActionResult Treinar(int entrada, int saida, int oculta, int interacao, int erro, float aprendizagem, int funcao)
        {
            return Ok();
        }
    }
}