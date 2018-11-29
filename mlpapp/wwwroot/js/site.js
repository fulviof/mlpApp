// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var dadosNormalizados = [];

var classes = [];

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
        
        $.ajax({
            url: "/Home/Treinar",
            type: "POST",
            data: { entrada: entrada, saida: saida, oculta: oculta, interacao: interacao, erro: erro, aprendizagem: aprendizagem, funcao: funcao,
                    normalizacao: JSON.stringify(dadosNormalizados), classes: classes}
        })
        .done(function(data) {
            if(!data.hasOwnProperty("erro")) {
                plot(data.errosEpoca);
                alert("Rede treinada!");
            }
            else {
                alert(data.erro);
            }
        })
            .fail(function(data){
                alert("Erro ao processar solicitacao");
            })
        
    });

    var form;
    $('#fileUpload').change(function (event) {
        form = new FormData();
        var file = document.getElementById("fileUpload").files[0];
        $("#fileInput").val(file.name);
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
                    dadosNormalizados = [];
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

    $('#btnEnviar').click(function (event) {
        
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
        dadosNormalizados.push(vet);
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
    var label = [];
    var ctx = document.getElementById("grafico-treinamento");
    for (var i = 0; i < data.length; i++) {
        label.push("Época "+ i);
    } 
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: label,
            datasets: [{
                label: 'Erro médio',
                data: data,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255,99,132,1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero:true
                    }
                }]
            }
        }
    });
}