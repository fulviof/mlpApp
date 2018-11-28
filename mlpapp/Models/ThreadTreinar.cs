using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Windows.Forms.DataVisualization.Charting;

namespace mlpapp.Models
{
    /*public class ThreadTreinar
    {
        private Chart cGrafico;
        private Neuronio[] neuronios_camada_oculta, neuronios_camada_saida;
        private DataTable dados;
        private Label infoIteracoes, infoHera, podeTestar;
        private Button btnTreinar;

        private double taxaAprendisagem;
        private double minimoErroRede;
        private List<String> listaClasses;
        private int COLUNA_CLASSE, maximoIteracoes;

        public ThreadTreinar(Chart grafico, Label infoIteracoes, Label infoHera, Button treinar, Label podeTestar, Neuronio[] camada_oculta, Neuronio[] camada_saida, DataTable dados, double TaxaAprendisagem, int MaximoIteracoes, double minimoErroRede, List<String> listaClasses) 
        {
            this.cGrafico = grafico;
            this.neuronios_camada_oculta = camada_oculta;
            this.neuronios_camada_saida = camada_saida;
            this.dados = dados;
            this.taxaAprendisagem = TaxaAprendisagem;
            this.maximoIteracoes = MaximoIteracoes;
            this.minimoErroRede = minimoErroRede;
            this.listaClasses = listaClasses;
            this.COLUNA_CLASSE = dados.Columns.Count - 1;
            this.infoIteracoes = infoIteracoes;
            this.infoHera = infoHera;
            this.btnTreinar = treinar;
            this.podeTestar = podeTestar;
        }

        //fazer os calculos usando o datatable
        public void execute()
        {
            double erroDaRede = 0, saidaObitda, erroNeuronio, derivada, novoPeso, mediaErrosRede = 0;
            int it = 1, NEo, NEs, cDADOS, saidaDesejada, entrada;

            double[] vetEntradas, vetEntradasSaidas;

            infoIteracoes.Invoke(new MethodInvoker(delegate
            {
                infoIteracoes.Text = "0";
            }));

            infoHera.Invoke(new MethodInvoker(delegate
            {
                infoHera.Text = "0";
            }));

            while (it < maximoIteracoes)// se erroDaRede for maior do que o erro especificado ou as iterações excederem o limite especificado
            {
                //começamos a parecorrer os dados
                cDADOS = 0;
                while (cDADOS < dados.Rows.Count)
                {
                    erroDaRede = 0;
                    vetEntradasSaidas = null;
                    vetEntradas = null;

                    //1 - calcular o net dos neuronios da camada oculta
                    // e gera uma vetor de double com os valores de cada coluna da linha, que é requerido como entrada do neuronio
                    vetEntradas = gerarEntradas(dados.Rows[cDADOS]);

                    //percorrer o vetor de neuronios da camada oculta
                    NEo = 0;
                    while(NEo < neuronios_camada_oculta.Length)
                    {
                        //insere as entradas no neuronio
                        neuronios_camada_oculta[NEo].setEntradas(vetEntradas);
                        NEo++;
                    }

                    //2 - calcular o net dos neuronios da camada saida
                    //gera as entradas dos neuronios de saida a partir de cada saida dos neuronios da camada oculta
                    vetEntradasSaidas = new double[neuronios_camada_oculta.Length];
                    NEo = 0;
                    while(NEo < neuronios_camada_oculta.Length)
                    {
                        vetEntradasSaidas[NEo] = neuronios_camada_oculta[NEo].i;
                        NEo++;
                    }
                    //percorrer o vetor de neuronios da camada de saida
                    NEs = 0;
                    while(NEs < neuronios_camada_saida.Length)
                    {
                        //insere as entradas no neuronio
                        neuronios_camada_saida[NEs].setEntradas(vetEntradasSaidas);
                        NEs++;
                    }

                    //3 passo calcular o erro do neuronio de saida
                    NEs = 0;
                    while(NEs < neuronios_camada_saida.Length)
                    {
                        saidaObitda = neuronios_camada_saida[NEs].i;
                        saidaDesejada = int.Parse(dados.Rows[cDADOS][COLUNA_CLASSE].ToString());
                        derivada = neuronios_camada_saida[NEs].derivada;

                        erroNeuronio = (saidaDesejada - saidaObitda) * derivada;
                        neuronios_camada_saida[NEs].setErro(erroNeuronio);
                        NEs++;
                    }

                    //4 calcular o erro da rede
                    NEs = 0;
                    while(NEs < neuronios_camada_saida.Length)
                    {
                        erroDaRede += Math.Pow(neuronios_camada_saida[NEs].getErro(), 2);
                        NEs++;
                    }
                    erroDaRede *= 0.5;

                    //somar os erros para tirar a media
                    mediaErrosRede += erroDaRede;

                    //5 calcular o erro da camada oculta
                    //percorro os neuronios da camada oculta para pegar sua derivada e seu indice para pegar os pesos
                    //no vetor de pesos da classe neuronio
                    NEo = 0;
                    while(NEo < neuronios_camada_oculta.Length)
                    {
                        erroNeuronio = 0;//zero o erro para cada neuornio

                        NEs = 0;
                        while (NEs < neuronios_camada_saida.Length)
                        {
                            //faço a somatória do erro da camada de saida * o peso
                            erroNeuronio += neuronios_camada_saida[NEs].getErro() * neuronios_camada_saida[NEs].getPeso(NEo);
                            NEs++;
                        }
                        
                        //multiplico a somatoria com a derivada do neuronio da camada oculta
                        erroNeuronio = neuronios_camada_oculta[NEo].derivada * erroNeuronio;

                        //guardo seu erro
                        neuronios_camada_oculta[NEo].setErro(erroNeuronio);
                        NEo++;
                    }

                    //6 atualizar os pesos da camada de saida
                    NEs = 0;
                    while(NEs < neuronios_camada_saida.Length)
                    {
                        NEo = 0;
                        while(NEo < neuronios_camada_saida[NEs].getNumEntradas())
                        {
                            novoPeso = neuronios_camada_saida[NEs].getPeso(NEo) + (taxaAprendisagem * neuronios_camada_saida[NEs].getErro() * neuronios_camada_saida[NEs].getEntrada(NEo));
                            neuronios_camada_saida[NEs].setPeso(NEo, novoPeso);
                            NEo++;
                        }

                        NEs++;
                    }

                    //7 atualizar os pesos da camada oculta
                    NEo = 0;
                    while(NEo < neuronios_camada_oculta.Length)
                    {
                        entrada = 0;
                        while(entrada < neuronios_camada_oculta[NEo].getNumEntradas())
                        {
                            novoPeso = neuronios_camada_oculta[NEo].getPeso(entrada) + (taxaAprendisagem * neuronios_camada_oculta[NEo].getErro() * neuronios_camada_oculta[NEo].getEntrada(entrada));
                            neuronios_camada_oculta[NEo].setPeso(entrada, novoPeso);
                            entrada++;
                        }

                        NEo++;
                    }

                    if (cDADOS == 0)
                    {
                        //mudo o tipo de de acao para nao deixar criar novos pesos para as entradas
                        NEo = 0;
                        while (NEo < neuronios_camada_oculta.Length)
                        {
                            neuronios_camada_oculta[NEo].isTeste(true);
                            NEo++;
                        }

                        NEs = 0;
                        while (NEs < neuronios_camada_saida.Length)
                        {
                            neuronios_camada_saida[NEs].isTeste(true);
                            NEs++;
                        }
                    }

                    cDADOS++;

                    infoHera.Invoke(new MethodInvoker(delegate
                    {
                        infoHera.Text = cDADOS.ToString();
                    }));
                }

                mediaErrosRede = mediaErrosRede / dados.Rows.Count;

                //mostra os erros da camada de saida em um grafico
                mostraNoGrafico(mediaErrosRede);

                if (mediaErrosRede <= minimoErroRede)
                    break;
                else
                {
                    it++;
                    infoIteracoes.Invoke(new MethodInvoker(delegate
                    {
                        infoIteracoes.Text = it.ToString();
                    }));
                }
            }

            MessageBox.Show("Rede Treinada", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnTreinar.Invoke(new MethodInvoker(delegate
            {
                btnTreinar.Enabled = true;
            }));

            podeTestar.Invoke(new MethodInvoker(delegate
            {
                podeTestar.Text = "1";
            }));
        }

        private double[] gerarEntradas(DataRow row)
        {
            int cls = row.Table.Columns.Count - 1;

            double[] vet = new double[cls];

            for (int a = 0; a < cls; a++)
            {
                vet[a] = double.Parse(row[a].ToString());
            }

            return vet;
        }


        private void mostraNoGrafico(double erro)
        {
            //Random r = new Random();               
            cGrafico.Invoke(new MethodInvoker(delegate {
                if (cGrafico.Series[0].Points.Count > neuronios_camada_saida.Length)
                {
                    cGrafico.Series[0].Points.RemoveAt(0);
                    cGrafico.Update();
                }

                cGrafico.Series[0].Points.AddXY("", erro); 
            }));
            Thread.Sleep(80);
        }
    }*/
}
