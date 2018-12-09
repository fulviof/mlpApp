using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mlpapp.Models
{
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
                    pesosEntradas[i] = randNum.Next(-1, 1) + randNum.NextDouble();
                }
            }
            else
            {
                for (int i = 0; i < numEntradas; i++)
                {
                    this.entradas[i] = Convert.ToDouble(entradas[i]);
                }
            }

            FuncaoAtivacao();
            FuncaoPropagacao();
            CalcularDerivada();
        }
        
        public void setEntradasPesos(string[] entradas, double[] pesos)
        {
            numEntradas = entradas.Length;
            Random randNum = new Random();

            this.entradas = new double[numEntradas];
            this.pesosEntradas = new double[pesos.Length];
            
            for (int i = 0; i < numEntradas; i++)
            {
                this.entradas[i] = Convert.ToDouble(entradas[i]);
                pesosEntradas[i] = pesos[i];
            }
            
            FuncaoAtivacao();
            FuncaoPropagacao();
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
                case 1:
                    this.i = this.net / 10; 
                break;

                case 2:
                    this.i = 1 / (1 + Math.Pow(Math.E, (this.net*(-1))));
                break;

                case 3:
                    this.i = Math.Tanh(this.net);
                break;
            }
        }

        private void CalcularDerivada() 
        {
            switch (this.tipoSaida)
            {
                case 1:
                    this.derivada = 0.1;
                break;

                case 2:
                    this.derivada = 1 / (1 + Math.Pow(Math.E, (this.net*(-1)))) * (1 - 1 / (1 + Math.Pow(Math.E, (this.net*(-1)))));
                break;

                case 3:
                    this.derivada = 1 - (Math.Pow(Math.Tanh(net), 2));
                break;
            }
        }
    }
}
