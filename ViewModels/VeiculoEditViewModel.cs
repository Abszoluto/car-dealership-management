using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using CarDealerApp.Models;
using System.Globalization;
using System.Windows;
using Dapper;

namespace CarDealerApp.ViewModels
{
    public class VeiculoEditViewModel : ObservableObject
    {
        public Veiculo Veiculo { get; set; }


        public ObservableCollection<Filial> Filiais { get; set; } = new();
        public ObservableCollection<string> StatusOptions { get; set; } = new();


        private string _valorTexto;
        public string ValorTexto
        {
            get => _valorTexto;
            set => SetProperty(ref _valorTexto, value);
        }

        public VeiculoEditViewModel(Veiculo veiculo, ObservableCollection<Filial> filiais)
        {
            this.Veiculo = veiculo;


            foreach (var filial in filiais)
            {
                Filiais.Add(filial);
            }

            StatusOptions.Add("estoque");
            StatusOptions.Add("reservado");
            StatusOptions.Add("vendido");
            StatusOptions.Add("baixado");

            ValorTexto = veiculo.Valor.ToString("F2", CultureInfo.CurrentCulture);
        }

        public bool UpdateValorFromTexto()
        {
            if (double.TryParse(ValorTexto, NumberStyles.Any, CultureInfo.CurrentCulture, out double valor) ||
                double.TryParse(ValorTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out valor))
            {
                Veiculo.Valor = valor;
                return true;
            }
            return false;
        }
    }
}