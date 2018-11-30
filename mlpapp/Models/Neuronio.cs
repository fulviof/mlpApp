using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlpapp.Models
{
    /*
     * Cada neuronio conhece a sua entrada e o peso de sua entrada
     *      A         
     *                      
     *              N1          
     * 
     *      B               NS1
     *      
     *              N2          
     *              
     *      C
     *      
     * N1: A, PA; B, PB, C, PC
     * N2: A, PA; B, PB, C, PC
     * NS1 N1, PN1; N2, PN2
     */
    public class Neuronio
    {
        public double[] entradas { get; set; }
        public double[] pesosEntradas { get; set; }
        public int numEntradas { get; set; }
        public double net { get; set; }
        public double erro { get; set; }
        public int tipoSaida { get; set; }    
        public double i { get; set; }
        public double derivada { get; set; }

        /*
         * TIPOS DE SAIDA
         * 1 - LINEAR
         * 2 - LOGÍSTICA
         * 3 - HIPERBÓLICA
         */
        public Neuronio(int tipoSaida)
        {
            this.tipoSaida = tipoSaida;
            this.i = 0;
            this.derivada = 0;
            this.erro = 0;
            this.entradas = null;
            this.pesosEntradas = null;
            this.numEntradas = 0;
            this.net = 0;
        }

        public void reiniciarNeuronioParaTeste()
        {
            this.derivada = 0;
            this.i = 0;
            this.entradas = null;
            this.erro = 0;
            this.numEntradas = 0;
            this.net = 0;
        }

        public void setEntradas(string[] entradas, bool primeiraVez)
        {
            numEntradas = entradas.Length;
            Random randNum = new Random();

            this.entradas = new double[numEntradas];

            if (primeiraVez)
            {
                pesosEntradas = new double[numEntradas];
                for (int i = 0; i < numEntradas; i++)
                {
                    this.entradas[i] = Convert.ToDouble(entradas[i]);
                    pesosEntradas[i] = randNum.Next(-2, 2);
                }
            }
            else
            {
                for (int i = 0; i < numEntradas; i++)
                {
                    this.entradas[i] = Convert.ToDouble(entradas[i]);
                }
            }


            //faz os calculos
            FuncaoAtivacao();
            FuncaoPropagacao();
            CalcularDerivada();
        }

        private void FuncaoAtivacao() 
        {
            this.net = 0;
            for (int i = 0; i < numEntradas; i++)
            {
                this.net += this.entradas[i] * this.pesosEntradas[i];
            }
        }

        private void FuncaoPropagacao()
        {
            switch (this.tipoSaida)
            { 
                case 1://linear
                    this.i = this.net / 10; 
                break;

                case 2://logistica
                    this.i = 1 / (1 + Math.Pow(Math.E, (this.net*(-1))));
                break;

                case 3://hiperbolica
                    this.i = (1 - Math.Pow(Math.E, (-2) * this.net)) / (1 + Math.Pow(Math.E, (-2) * this.net));
                break;
            }
        }

        //calcula da derivada do net
        /*
         * PARA CALCULAR O ERRO
         * 1 - calcular o erro dos neuronios de saida. exemplo duas saidas
         * Ex: S1 - (Desajado - Obitido) * f'(net)
         * 
         * 2 - calcular o erro da rede, multiplica 0.5 pela soma dos erros de cada neuronio ao quadrado
         * Ex: 0.5 * ((NS1: erro^2) + (NS2: erro^2))
         * 
         * 3 - calcula os erros dos neuronios da cama oculta
         * Ex: neuronio1 = (erro do neuronio de saida1 * peso do neuronio1 oculto e o neuronio de saida1) +
         *     (erro do neuronio de saida2 * peso do neuronio1 oculto e o neuronio de saida2) ...
         */
        private void CalcularDerivada() 
        {
            switch (this.tipoSaida)
            {
                case 1://linear
                    this.derivada = 0.1;
                break;

                case 2://logistica
                    //this.derivada = this.i * (1 - this.i);
                    this.derivada = 1 / (1 + Math.Pow(Math.E, (this.net*(-1)))) * (1 - 1 / (1 + Math.Pow(Math.E, (this.net*(-1)))));
                break;

                case 3://hiperbolica
                    //this.derivada = 1 - Math.Pow(this.i, 2);
                    this.derivada = 1 - Math.Pow((1 - Math.Pow(Math.E, (-2) * this.net)) / (1 + Math.Pow(Math.E, (-2) * this.net)), 2);
                break;
            }
        }
        
        public double GetEntrada(int x)
        {
            return this.entradas[x];
        }

        public double GetPeso(int x)
        { 
            return this.pesosEntradas[x];
        }

        public void SetPeso(int x, double novoPeso)
        {
            this.pesosEntradas[x] = novoPeso;
        }
    }
}
