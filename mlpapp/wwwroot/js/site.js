﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    
    $("#btn-treinar").click(function(){
        
        var entrada = $("#input-entrada").val(); 
        
        var saida = $("#input-saida").val();
        
        var oculta = $("#input-oculta").val();
        
        var interacao = $("#input-interacao").val();
        
        var erro = $("#input-erro").val();
        
        var aprendizagem = $("#input-aprendizagem").val()/100;
        
        var funcao = $("input[name='funcao-transf']:checked").val();
        
        if(oculta === undefined || oculta === null || oculta === "") {
            alert("Preencha a camada oculta");
            return;
        }

        if(interacao === undefined || interacao === null || interacao === "") {
            alert("Preencha a quantidade de interacoes");
            return;
        }

        if(erro === undefined || erro === null || erro === "") {
            alert("Preencha o campo erro");
            return;
        }

        if(aprendizagem === undefined || aprendizagem === null || aprendizagem === "" || aprendizagem === 0) {
            alert("Preencha o campo taxa de aprendizagem");
            return;
        }

        if(funcao === undefined || funcao === null || funcao === "") {
            alert("Preencha a funcao de transferencia");
            return;
        }
        
        $.ajax({
            url: "/Home/Treinar",
            type: "POST",
            data: { entrada: entrada, saida: saida, oculta: oculta, interacao: interacao, erro: erro, aprendizagem: aprendizagem, funcao: funcao}
        })
        .done(function(data) {
            if(!data.hasOwnProperty("erro")) {
                
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
        
        for (var k = 0; k < valores.length; k++) {
            
            if (k === valores.length - 1) {
                html += "<td>"+ classes[j] +"</td>"
            }
            else {
                html += "<td>"+ trunc((valores[k] - resultados[k*3+1])/resultados[k*3+2]) +"</td>"
            }
        } 
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