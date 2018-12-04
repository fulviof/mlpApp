// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var neuroniosSaida = [];

var neuroniosOculta = [];

var dadosNormalizadosTreinamento = [];

var dadosNormalizadosTeste = [];

var classes = [];

var grafico;

$(function () {
    
    $("#btn-treinar").click(function(){
        
        var entrada = $("#input-entrada").val(); 
        
        var saida = $("#input-saida").val();
        
        var oculta = $("#input-oculta").val();
        
        var interacao = $("#input-interacao").val();
        
        var erro = parseFloat($("#input-erro").val());
        
        var aprendizagem = $("#input-aprendizagem").val()/100;
        
        var funcao = $("input[name='funcao-transf']:checked").val();
        
        if(oculta === undefined || oculta === "0" || oculta === "") {
            alert("Preencha a camada oculta");
            return;
        }

        if(interacao === undefined || interacao === "0" || interacao === "") {
            alert("Preencha a quantidade de interacoes");
            return;
        }

        if(erro === undefined || erro === "0" || erro === "") {
            alert("Preencha o campo erro");
            return;
        }

        if(aprendizagem === undefined || aprendizagem === 0) {
            alert("Preencha o campo taxa de aprendizagem");
            return;
        }

        if(funcao === undefined || funcao === "0" || funcao === "") {
            alert("Preencha a funcao de transferencia");
            return;
        }
        if(interacao > 21000) {
            alert("O máximo de iteracoes é 5000");
            return;
        }

        neuroniosSaida = [];

        neuroniosOculta = [];
        
        loadingState();
        $.ajax({
            url: "/Home/Treinar",
            type: "POST",
            data: { entrada: entrada, saida: saida, oculta: oculta, interacao: interacao, erro: erro, aprendizagem: aprendizagem, funcao: funcao,
                    normalizacao: JSON.stringify(dadosNormalizadosTreinamento), classes: classes}
        })
        .done(function(data) {
            if(!data.hasOwnProperty("erro")) {
                removeLoadingState();
                $("#nav-testar-tab").removeClass("disabled");
                $("#input-arq-treino-teste").val($("#fileInput").val());
                $(".chart-container").show();
                plot(data.errosEpoca);
                
                
                neuroniosOculta = data.neuroniosOculta;
                neuroniosSaida = data.neuroniosSaida;
            }
            else {
                removeLoadingState();
                alert(data.erro);
            }
        })
        .fail(function(data){
            removeLoadingState();
            alert("Erro ao processar solicitacao");
        })
        
    });

    var form;
    $('#fileUpload').change(function (event) {
        form = new FormData();
        var file = document.getElementById("fileUpload").files[0];
        $("#fileInput").val(file.name);
        $()
        form.append('fileUpload', file); // para apenas 1 arquivo
        //var name = event.target.files[0].content.name; // para capturar o nome do arquivo com sua extenção
        $.ajax({
            url: '/Home/Normalizar', // Url do lado server que vai receber o arquivo
            data: form,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (data) {
                if(!data.hasOwnProperty("erro")) {
                    dadosNormalizadosTreinamento = [];
                    classes = data.clas;
                    tabelaCsv(data);
                    
                    var qtde = quantidadeClasses(data.clas); 
                    
                    $("#input-arq-treino").val($("#fileInput").val());

                    $("#input-entrada").val(data.qtde);

                    $("#input-saida").val(qtde);
                    
                    $("#input-oculta").val(parseInt((data.qtde+qtde)/2));

                    $("#nav-treinar-tab").removeClass("disabled");
                }
                else {
                    alert(data.erro);
                }
            }
        });
    });
    
    var form2;
    $('#fileUpload-teste').change(function (event) {
        form2 = new FormData();
        var file = document.getElementById("fileUpload-teste").files[0];
        $("#fileInput-teste").val(file.name);
        form2.append('fileUpload-teste', file); // para apenas 1 arquivo
        //var name = event.target.files[0].content.name; // para capturar o nome do arquivo com sua extenção
        
    });
    
    $("#btn-testar").click(function(){
        $.ajax({
            url: '/Home/CarregaArquivoTeste', // Url do lado server que vai receber o arquivo
            data: form2,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function (data) {
                if(!data.hasOwnProperty("erro")) {

                    dadosNormalizadosTeste = [];
                    var resultados = data.resultados;
                    var dados = data.dados;
                    var classes = data.clas;
                    var pesosO = [];
                    var pesosS = [];
                    var saida;

                    for (var j = 1; j < dados.length; j++){

                        var valores = dados[j].split(",");
                        var vet = [];
                        var valor;

                        for (var k = 0; k < valores.length; k++) {
                            
                            if(k !== valores.length -1) {
                                valor = (valores[k] - resultados[k * 3 + 1]) / resultados[k * 3 + 2];
                                vet.push(valor);
                            }
                        }
                        dadosNormalizadosTeste.push(vet);
                    }
                    
                    for (var i = 0; i < neuroniosSaida.length; i++) {
                        pesosS.push(neuroniosSaida[i].pesosEntradas);
                    }

                    for (var i = 0; i < neuroniosOculta.length; i++) {
                        pesosO.push(neuroniosOculta[i].pesosEntradas);
                    }
                    
                    var saida = neuroniosSaida[0].tipoSaida;
                    
                    $.ajax({
                        url: "/Home/Testar",
                        type: "POST",
                        data: {normalizacao: JSON.stringify(dadosNormalizadosTeste), classes: JSON.stringify(classes), neuroniosSaida: JSON.stringify(pesosS), neuroniosOculta: JSON.stringify(pesosO), saida: saida }
                    })
                    .done(function(data) {
                        if(!data.hasOwnProperty("erro")) {
                            matrizConfusao(data.matrix, data.linha, data.coluna);
                        }
                        else {
                            alert(data.erro);
                        }
                    })
                    .fail(function(data){
                        alert("Erro ao processar solicitacao");
                    })
                }
                else {
                    alert(data.erro);
                }
            }
        }); 
    });
});

function quantidadeClasses(classes) {
    
    var valores = [];
    
    for (var i = 0; i< classes.length; i++){
        
        if(valores.includes(classes[i]) == false) {
            valores.push(classes[i]);
        }
    } 
    
    return valores.length - 1;
}

function matrizConfusao(matrix, linha, coluna){
    
    var html = "<thead class=\"thead-dark\"><tr><th scope=\"col\">#</th>";
    var aux;
    for (var i = 0; i< coluna; i++) {
        aux = i+1;
        html += "<th scope=\"col\">N"+ aux +"</th>"
        
    }
    html += "</tr></thead><tbody>";
    for (var i = 0; i < linha; i++) {
        aux = i+1;
        html += "<tr>\n" +
            "      <th scope=\"row\">Classe "+ aux +"</th>";
        
        for (var j = 0; j < coluna; j++) {
            html += "<td>"+matrix[i][j]+"</td>"
        } 
        
        html += "<\tr>";
    }

    html += "</tbody>";

    $("#matrix").html(html);
    
}

function loadingState(length) {
    
    $('#animacao').show('fast');
    $('#btn-treinar').addClass('disabled');
    $('#btn-treinar')[0].innerText = 'Treinando...';
}

function removeLoadingState() {
    
    $('#animacao').hide('fast');
    $('#btn-treinar').removeClass('disabled');
    $('#btn-treinar')[0].innerText = 'Treinar';
}


function tabelaCsv(data) {
    
    var resultados = data.resultados;
    var dados = data.dados;
    var classes = data.clas;
    var html = "<thead class=\"thead-dark\"><tr>";
    
    var cabecalho = dados[0].split(",")
    
    for (var i = 0; i < cabecalho.length; i++) {

        html += "<th scope=\"col\">"+ cabecalho[i] +"</th>"
    }

    html += "</tr></thead><tbody>";
    for (var j = 1; j < dados.length - 1; j++){
        
        html += "<tr>";
        var valores = dados[j].split(",");
        var vet = [];
        var valor;
        for (var k = 0; k < valores.length; k++) {
            
            if (k === valores.length - 1) {
                html += "<td>"+ classes[j] +"</td>"
            }
            else {
                valor = trunc((valores[k] - resultados[k*3+1])/resultados[k*3+2]);
                html += "<td>"+ valor +"</td>";
                vet.push(valor);
            }
        }
        dadosNormalizadosTreinamento.push(vet);
        html += "</tr>"
    } 
    
    html += "</tbody>";
    
    $("#tabela-csv").html(html);
}

function trunc(n) {
    var t=n.toString();
    var regex=/(\d*.\d{0,3})/;
    return t.match(regex)[0];
}


function plot(data) {
    
    if(grafico !== undefined || grafico != null)
        grafico.destroy();
    
    var label = [];
    var cores = [];
    var resize = [];
    if(data.length > 1000) {

        if(data.length > 1000 && data.length <= 2000) {
            for (var i = 0; i < data.length; i++) {
                if(i%2 == 0) {
                    resize.push(data[i]);
                    label.push("Época "+ i);
                    cores.push('rgba(54, 150, 255, 0.5)');
                }

            }
        }

        if(data.length > 2000 && data.length <= 3000) {
            for (var i = 0; i < data.length; i++) {
                if(i%3 == 0) {
                    resize.push(data[i]);
                    label.push("Época "+ i);
                    cores.push('rgba(54, 150, 255, 0.5)');
                }

            }
        }

        if(data.length > 3000 && data.length <= 4000) {
            for (var i = 0; i < data.length; i++) {
                if(i%4 == 0) {
                    resize.push(data[i]);
                    label.push("Época "+ i);
                    cores.push('rgba(54, 150, 255, 0.5)');
                }

            }
        }

        if(data.length > 4000 && data.length <= 5000) {
            for (var i = 0; i < data.length; i++) {
                if(i%5 == 0) {
                    resize.push(data[i]);
                    label.push("Época "+ i);
                    cores.push('rgba(54, 150, 255, 0.5)');
                }

            }
        }

        if(data.length > 5000 && data.length <= 20000) {
            for (var i = 0; i < data.length; i++) {
                if(i%20 == 0) {
                    resize.push(data[i]);
                    label.push("Época "+ i);
                    cores.push('rgba(54, 150, 255, 0.5)');
                }

            }
        }
        
        var config = {
            type: 'bar',
            data: {
                datasets: [{
                    label: "Erro médio",
                    data: resize,
                    backgroundColor: cores,
                    borderColor: cores,
                    borderWidth: 1
                }],
                labels: label
            },
            options: {
                resposive: true,
                legend: {
                    display: true
                },
                tooltips: {
                    enabled: true
                }
            }
        };
    }
    else {
        for (var i = 0; i < data.length; i++) {
            label.push("Época "+ i);
            cores.push('rgba(54, 150, 255, 0.5)');
        }

        var config = {
            type: 'bar',
            data: {
                datasets: [{
                    label: "Erro médio",
                    data: data,
                    backgroundColor: cores,
                    borderColor: cores,
                    borderWidth: 1
                }],
                labels: label
            },
            options: {
                resposive: true,
                legend: {
                    display: true
                },
                tooltips: {
                    enabled: true
                }
            }
        };
    }
    
    var g = document.getElementById("grafico-treinamento");
    grafico = new Chart(g, config);
    
}

