#pragma checksum "/Users/fulviofanelli/RiderProjects/mlp/mlpapp/Views/Home/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "69a9622e9eea539477bc815f64edf46839a113a5"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(AspNetCore.Views_Home_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "/Users/fulviofanelli/RiderProjects/mlp/mlpapp/Views/_ViewImports.cshtml"
using mlpapp;

#line default
#line hidden
#line 2 "/Users/fulviofanelli/RiderProjects/mlp/mlpapp/Views/_ViewImports.cshtml"
using mlpapp.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"69a9622e9eea539477bc815f64edf46839a113a5", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"970868c51c4de1bb906cbd2df4abca6c62899166", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "/Users/fulviofanelli/RiderProjects/mlp/mlpapp/Views/Home/Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
            BeginContext(42, 9443, true);
            WriteLiteral(@"<nav>
    <div class=""nav nav-tabs"" id=""nav-tab"" role=""tablist"">
        <a class=""nav-item nav-link active"" id=""nav-normalizar-tab"" data-toggle=""tab"" href=""#nav-normalizar"" role=""tab"" aria-controls=""nav-normalizar"" aria-selected=""true"">Normalizar</a>
        <a class=""nav-item nav-link disabled"" id=""nav-treinar-tab"" data-toggle=""tab"" href=""#nav-treinar"" role=""tab""  aria-controls=""nav-treinar"" aria-selected=""false"">Treinar</a>
        <a class=""nav-item nav-link disabled"" id=""nav-testar-tab"" data-toggle=""tab"" href=""#nav-testar"" role=""tab"" aria-controls=""nav-testar"" aria-selected=""false"">Testar</a>
    </div>
</nav>
<div class=""tab-content"" id=""nav-tabContent"">
    <div class=""tab-pane fade show active"" id=""nav-normalizar"" role=""tabpanel"" aria-labelledby=""nav-normalizar-tab"">
        <div class=""modal-dialog"">
            <div class=""modal-content"">
                <div class=""modal-header"">
                    <h5>Upload do arquivo</h5>
                </div>
                <div class=""form-group form-label-");
            WriteLiteral(@"left"" enctype=""multipart/form-data"">
                    <div class=""modal-body"">
                        <div class=""form-group"">
                            <div class=""input-group mb-3"">
                                <input type=""text"" class=""form-control"" readonly="""" id=""fileInput"">
                                <div class=""input-group-append"">
                                    <span class=""btn btn-outline-primary"">
                                        <label class=""input-group-btn"" style=""margin: 0 auto"">                                    
                                            Procurar… <input type=""file"" style=""display: none;"" name=""file"" id=""fileUpload"">                                   
                                        </label>
                                    </span>
                                </div>
                            </div>                   
                        </div>
                    </div>
                </div>`
            </div>
        </div>
 ");
            WriteLiteral(@"       <div class=""row justify-content-around"">
            <div class=""col-sm-9"">
                <table id=""tabela-csv"" class=""table table-responsive table-hover"">
            
                </table>
            </div>
        </div>
    </div>
    <div class=""tab-pane fade"" id=""nav-treinar"" role=""tabpanel"" aria-labelledby=""nav-treinar-tab"">
        <div class=""form-group"" style=""margin-top: 3%"">
            <div class=""row justify-content-around"">
                <div class=""card"" style=""width: 16rem;"">
                    <div class=""card-body"">
                        <h5 class=""card-title"">Número de neurônios</h5>
                        <label class=""control-label col-sm-12"" for=""input-entrada"">Camada de entrada:
                            <input id=""input-entrada"" type=""text"" class=""form-control"" disabled=""disabled""></input>
                        </label>
                        
                        <label class=""control-label col-sm-12"" for=""input-saida"">Camada de saída:
                    ");
            WriteLiteral(@"        <input id=""input-saida"" type=""text"" class=""form-control"" disabled=""disabled""></input>
                        </label>
                        
                        <label class=""control-label col-sm-12"" for=""input-oculta"">Camada oculta:
                            <input id=""input-oculta"" type=""text"" class=""form-control""></input>
                        </label>
                    </div>
                </div>
                <div class=""card"" style=""width: 16rem;"">
                    <div class=""card-body"">
                        <h5 class=""card-title"">Critério de parada</h5>
                        <label class=""control-label col-sm-12"" for=""input-intercao"">Iterações:
                            <input type=""text"" id=""input-interacao"" class=""form-control"">
                        </label>
                        
                        <label class=""control-label col-sm-12"" for=""input-erro"">Erro:
                            <input type=""text"" id=""input-erro"" class=""form-control"">
           ");
            WriteLiteral(@"             </label>     
                    </div>
                </div>
                
                <div class=""card"" style=""width: 16rem;"">
                    <div class=""card-body"">
                        <h5 class=""card-title"">Taxa de aprendizagem</h5>
                        <label class=""control-label col-sm-12"" for=""input-interacao"">N(%):
                            <input id=""input-aprendizagem"" type=""text"" class=""form-control"" ></input>
                        </label>
                    </div>
                </div>
        
                <div class=""card"" style=""width: 16rem;"">
                    <div class=""card-body"">
                        <h5 class=""card-title"">Função de transferência</h5>
                        <div class=""form-check"">
                            <input class=""form-check-input"" type=""radio"" name=""funcao-transf"" value=""1"" class=""rd-funcao""></input>
                            <label class=""form-check-label"" for=""rd-linear"">
                                Linea");
            WriteLiteral(@"r
                            </label>
                        </div>
                        <div class=""form-check"">
                            <input class=""form-check-input"" type=""radio"" name=""funcao-transf"" value=""2"" class=""rd-funcao""></input>
                            <label class=""form-check-label"" for=""rd-log"">
                                Logística
                            </label>
                        </div>
                        <div class=""form-check"">
                            <input class=""form-check-input"" type=""radio"" name=""funcao-transf"" value=""3"" class=""rd-funcao""></input>
                            <label class=""form-check-label"" for=""rd-hiper"">
                                Hiperbólica
                            </label>
                        </div>  
                    </div>
                </div>
            </div>
            <div class=""row justify-content-center"" style=""margin-top: 3%"">
                <div class=""col-sm-6"">
                    <label class=""co");
            WriteLiteral(@"ntrol-label col-sm-12"" for=""input-arq-treino"">Arquivo: 
                        <input class=""form-control"" type=""text"" id=""input-arq-treino"" disabled=""disabled""></input>
                    </label>      
                </div>
                <div class=""col-sm-2"" style=""margin-top: 1.5em"">
                    <button class=""btn btn-outline-primary col-sm-12"" id=""btn-treinar"">Treinar</button>
                </div>
            </div>
            <!--<div class=""row justify-content-center"">
                <div class=""col-sm-3"">
                    <label class=""control-label col-sm-12"" for=""qtde-interacao"">Iterações: 
                        <input class=""form-control"" type=""text"" id=""qtde-interacao"" disabled=""disabled""></input>
                    </label>
                    
                </div>
                <div class=""col-sm-3"">
                    <label class=""control-label col-sm-12"" for=""qtde-registro"">Registros:
                        <input class=""form-control"" type=""text"" id=""qtde-registro");
            WriteLiteral(@""" disabled=""disabled""></input>
                    </label>
                    
                </div>
            </div>-->
        </div>
        
        <div class=""chart-container"" style=""position: relative; height:40vh; width: 80vw; margin-top: 5%; display:none"">
            <canvas id=""grafico-treinamento"">
            
            </canvas>
        </div>
    </div>
    <div class=""tab-pane fade"" id=""nav-testar"" role=""tabpanel"" aria-labelledby=""nav-testar-tab"">...</div>
</div>

<div class=""wrapper"" id=""animacao-treinamento"" style=""display:none"">
    <svg  xmlns=""http://www.w3.org/2000/svg"" x=""0px"" y=""0px"" viewBox=""0 0 288 288"" style=""width: 10%"">
        <linearGradient id=""PSgrad_0"" x1=""70.711%"" x2=""0%"" y1=""70.711%"" y2=""0%"">
            <stop offset=""0%"" stop-color=""rgb(95,70,255)"" stop-opacity=""1"" />
            <stop offset=""100%"" stop-color=""rgb(50,255,255)"" stop-opacity=""1"" />
        </linearGradient>
        <path fill=""url(#PSgrad_0)"">

            <animate  repeatCount=""indefinite"" attribute");
            WriteLiteral(@"Name=""d"" dur=""5s""
	
                      values=""M37.5,186c-12.1-10.5-11.8-32.3-7.2-46.7c4.8-15,13.1-17.8,30.1-36.7C91,68.8,83.5,56.7,103.4,45
	c22.2-13.1,51.1-9.5,69.6-1.6c18.1,7.8,15.7,15.3,43.3,33.2c28.8,18.8,37.2,14.3,46.7,27.9c15.6,22.3,6.4,53.3,4.4,60.2
	c-3.3,11.2-7.1,23.9-18.5,32c-16.3,11.5-29.5,0.7-48.6,11c-16.2,8.7-12.6,19.7-28.2,33.2c-22.7,19.7-63.8,25.7-79.9,9.7
	c-15.2-15.1,0.3-41.7-16.6-54.9C63,186,49.7,196.7,37.5,186z;
	
	
	M51,171.3c-6.1-17.7-15.3-17.2-20.7-32c-8-21.9,0.7-54.6,20.7-67.1c19.5-12.3,32.8,5.5,67.7-3.4C145.2,62,145,49.9,173,43.4
	c12-2.8,41.4-9.6,60.2,6.6c19,16.4,16.7,47.5,16,57.7c-1.7,22.8-10.3,25.5-9.4,46.4c1,22.5,11.2,25.8,9.1,42.6
	c-2.2,17.6-16.3,37.5-33.5,40.8c-22,4.1-29.4-22.4-54.9-22.6c-31-0.2-40.8,39-68.3,35.7c-17.3-2-32.2-19.8-37.3-34.8
	C48.9,198.6,57.8,191,51,171.3z;
	
	M37.5,186c-12.1-10.5-11.8-32.3-7.2-46.7c4.8-15,13.1-17.8,30.1-36.7C91,68.8,83.5,56.7,103.4,45
	c22.2-13.1,51.1-9.5,69.6-1.6c18.1,7.8,15.7,15.3,43.3,33.2c28.8,18.8,37.2,14.3,46.7,27.9c15.6,22.3,6.4,53.3,");
            WriteLiteral("4.4,60.2\n\tc-3.3,11.2-7.1,23.9-18.5,32c-16.3,11.5-29.5,0.7-48.6,11c-16.2,8.7-12.6,19.7-28.2,33.2c-22.7,19.7-63.8,25.7-79.9,9.7\n\tc-15.2-15.1,0.3-41.7-16.6-54.9C63,186,49.7,196.7,37.5,186z\t\"/>\n\n        </path>\n\n    </svg>\n</div>\n\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
