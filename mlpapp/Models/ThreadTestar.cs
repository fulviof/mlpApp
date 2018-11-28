using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace mlpapp.Models
{
    /*public class ThreadTestar
    {
        private DataGridView matrizConfusao;
        private Neuronio[] neuronios_camada_oculta, neuronios_camada_saida;
        private DataTable dados;
        private Label infoIteracoes, infoHera;
        private Button btnTreinar;

        private List<String> listaClasses;
        private int COLUNA_CLASSE;

        public ThreadTestar(DataGridView matrizConfusao, Label infoIteracoes, Label infoHera, Button treinar, Neuronio[] camada_oculta, Neuronio[] camada_saida, DataTable dados, List<String> listaClasses)
        {
            this.matrizConfusao = matrizConfusao;
            this.neuronios_camada_oculta = camada_oculta;
            this.neuronios_camada_saida = camada_saida;
            this.dados = dados;
            this.COLUNA_CLASSE = dados.Columns.Count - 1;
            this.infoIteracoes = infoIteracoes;
            this.infoHera = infoHera;
            this.btnTreinar = treinar;
            this.listaClasses = listaClasses;
        }

        //fazer os calculos usando o datatable
        public void execute()
        {
            int it = 1, NEo, NEs, cDADOS, neuronioVencedor;

            double[] vetEntradas, vetEntradasSaidas;


            infoHera.Invoke(new MethodInvoker(delegate
            {
                infoHera.Text = "0";
            }));

            gerarMatrizConfusao();

            //começamos a parecorrer os dados
            cDADOS = 0;
            while (cDADOS < dados.Rows.Count)
            {
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

                neuronioVencedor = selecionaVencedor(neuronios_camada_saida);
                mostraMatrizConfusao(neuronioVencedor, dados.Rows[cDADOS][COLUNA_CLASSE].ToString());

                cDADOS++;

                infoHera.Invoke(new MethodInvoker(delegate
                {
                    infoHera.Text = cDADOS.ToString();
                }));

                /*
                NEo = 0;
                while (NEo < neuronios_camada_oculta.Length)
                {
                    neuronios_camada_oculta[NEo].reiniciarNeuronioParaTeste();
                    NEo++;
                }

                NEs = 0;
                while (NEs < neuronios_camada_saida.Length)
                {
                    neuronios_camada_saida[NEs].reiniciarNeuronioParaTeste();
                    NEs++;
                }#1#
            }

            MessageBox.Show("Rede Testada", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnTreinar.Invoke(new MethodInvoker(delegate
            {
                btnTreinar.Enabled = true;
            }));
        }

        private int selecionaVencedor(Neuronio[] ns)
        {
            int vencedor = 0;
            double maior = ns[0].i;
            for (int a = 1; a < ns.Length; a++)
                if (ns[a].i > maior)
                {
                    maior = ns[a].i;
                    vencedor = a;
                }

            return vencedor;
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

        private void gerarMatrizConfusao() 
        {
            int classe = dados.Columns.Count - 1;

            matrizConfusao.Invoke(new MethodInvoker(delegate
            {
                matrizConfusao.Columns.Clear();
                matrizConfusao.Rows.Clear();
            }));

            matrizConfusao.Invoke(new MethodInvoker(delegate
            {
                matrizConfusao.Columns.Add("saidas", "Saidas");
                matrizConfusao.Columns["saidas"].Width = 80;
                matrizConfusao.Columns["saidas"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                matrizConfusao.Columns["saidas"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }));

            for(int c = 0; c < neuronios_camada_saida.Length; c++)
            {
                matrizConfusao.Invoke(new MethodInvoker(delegate
                {
                    matrizConfusao.Columns.Add("ns"+c, "NS " + (c + 1));
                    matrizConfusao.Columns["ns" + c].Width = 60;
                    matrizConfusao.Columns["ns" + c].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    matrizConfusao.Columns["ns" + c].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }));
            }

            for (int l = 0; l < listaClasses.Count; l++)
            {
                matrizConfusao.Invoke(new MethodInvoker(delegate
                {
                    matrizConfusao.Rows.Add((l + 1).ToString());
                }));
            }

            matrizConfusao.Invoke(new MethodInvoker(delegate
            {
                for(int c = 1; c < matrizConfusao.Columns.Count; c++)
                    for (int l = 0; l < matrizConfusao.Rows.Count; l++)
                    {
                        matrizConfusao.Rows[l].Cells[c].Value = 0;
                    }
            }));
        }

        /*
         * verifica qual a saida desejada ex: saida 2
         * pega qual foi o neurionio vencedor: neuronio 3
         * busca o indice na lista de saidas pela saida 2
         * depois incrementa o contador na posicao da DataGridView row[indice_lista].Cells["ns"+neuronio]
         * 
         * listaClasses = { 
         *      Posicao => Valor
         *      0 => 1, 
         *      1 => 2, 
         *      2 => 3, 
         *      3 => 4, 
         *      4 => 5 
         *  }
         * 
         * 
         *      NS 1    NS 2    NS 3    NS 4    NS 5
         *  S1    0       0       0       0      0   
         *  S2    0       0       0       0      0
         *  S3    0       0       0       0      0
         *  S4    0       0       0       0      0
         *  S5    0       0       0       0      0
         * 
         #1#
        private void mostraMatrizConfusao(int neuronio, string desejado)
        {
            int ult, indice = buscarPosicaoSaidas(desejado);        
            matrizConfusao.Invoke(new MethodInvoker(delegate
            {
                ult = int.Parse(matrizConfusao.Rows[indice].Cells["ns" + neuronio].Value.ToString());
                matrizConfusao.Rows[indice].Cells["ns"+neuronio].Value = ult + 1;
            }));
            Thread.Sleep(80);
        }

        private int buscarPosicaoSaidas(String saida)
        {
            int indice = 0;
            while (indice < listaClasses.Count && listaClasses[indice] != saida)
                indice++;

            if (indice < listaClasses.Count)
                return indice;
            else
                return -1;
        }
    }*/
}
