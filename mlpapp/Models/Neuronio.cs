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
        private double[] entradas_in;
        private double[] pesos_entradas;
        private int num_entradas;
        private double NET, erro;
        private int tipo_saida;

        private bool modoTeste;
        
        //dados de saida
        public double i, derivada;

        /*
         * TIPOS DE SAIDA
         * 1 - LINEAR
         * 2 - LOGÍSTICA
         * 3 - HIPERBÓLICA
         */
        public Neuronio(int tipo_de_saida)
        {
            this.tipo_saida = tipo_de_saida;
            this.i = 0;
            this.derivada = 0;
            this.erro = 0;
            this.entradas_in = null;
            this.pesos_entradas = null;
            this.num_entradas = 0;
            this.NET = 0;
            this.modoTeste = false;
        }

        public void isTeste(bool s)
        {
            this.modoTeste = s;
        }

        public void reiniciarNeuronioParaTeste()
        {
            this.derivada = 0;
            this.i = 0;
            this.entradas_in = null;
            this.erro = 0;
            this.num_entradas = 0;
            this.NET = 0;
        }

        public void setEntradas(double[] entradas)
        {
            num_entradas = entradas.Length;
            Random randNum = new Random();

            if (modoTeste == false)
            {
                entradas_in = new double[num_entradas];
                pesos_entradas = new double[num_entradas];
                for (int i = 0; i < num_entradas; i++)
                {
                    entradas_in[i] = entradas[i];
                    pesos_entradas[i] = randNum.Next(-2, 2);
                }
            }
            else
            {
                entradas_in = new double[num_entradas];
                for (int i = 0; i < num_entradas; i++)
                {
                    entradas_in[i] = entradas[i];
                }
            }

            //faz os calculos
            funcao_ativacao();
            funcao_propagacao();
            calcularDerivada();
        }

        private void funcao_ativacao() 
        {
            this.NET = 0;
            for (int i = 0; i < num_entradas; i++)
            {
                this.NET += this.entradas_in[i] * this.pesos_entradas[i];
            }
        }

        private void funcao_propagacao()
        {
            switch (this.tipo_saida)
            { 
                case 1://linerar
                    this.i = this.NET / 10; 
                break;

                case 2://logistica
                    this.i = 1 / (1 + Math.Pow(Math.E, -this.NET));
                break;

                case 3://hiperbolica
                    this.i = (1 - Math.Pow(Math.E, (-2) * this.NET)) / (1 + Math.Pow(Math.E, (-2) * this.NET));
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
        private void calcularDerivada() 
        {
            switch (this.tipo_saida)
            {
                case 1://linerar
                    this.derivada = 0.1;
                break;

                case 2://logistica
                    //this.derivada = this.i * (1 - this.i);
                    this.derivada = this.NET * (1 - this.NET);
                break;

                case 3://hiperbolica
                    //this.derivada = 1 - Math.Pow(this.i, 2);
                    this.derivada = 1 - Math.Pow(this.NET, 2);
                break;
            }
        }

        public void setErro(double erro)
        {
            this.erro = erro;
        }

        public double getErro()
        {
            return this.erro;
        }

        public int getNumEntradas()
        {
            return this.num_entradas;
        }

        public double getEntrada(int n)
        {
            return this.entradas_in[n];
        }

        public double getPeso(int n)
        {
            return this.pesos_entradas[n];
        }

        public void setPeso(int n, double novoPeso)
        {
            this.pesos_entradas[n] = novoPeso;
        }
    }
}
