using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using conversorDivisas.Models;
using conversorDivisas.Services;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json.Linq;
using conversorDivisas.Models;

namespace conversorDivisas.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DialogService dialogService;
        private string _lblMonto = "Ingresar Monto:";
        private string _txtMontoIngresado = "";
        private string _lblOrigen = "Divisa Origen:";
        private string _pckTitulo = "Seleccionar";
        private string _lblDestino = "Divisa Destino:";
        private string _btnConvertir = "Convertir";
        private string _btnLimpiar = "Limpiar";
        private string _btnSalir = "Salir";
        private string _lblResultado = "Resultado:";
        private string _lblResultadoMonto = "500";
        private List<ModeloPicker> _pckOrigen;
        private List<ModeloPicker> _pckDestino;
        private ModeloPicker _pckOrigenSeleccionado;
        private ModeloPicker _pckDestinoSeleccionado;
        //public ObservableCollection<ModeloPicker> _pckElementos { get; set; }
        ConnectionDB conexionDB;

        public MainViewModel()
        {
            dialogService = new DialogService();
            _pckOrigen = new List<ModeloPicker>();
            _pckDestino = new List<ModeloPicker>();
            conexionDB = new ConnectionDB();

            conexionDB.CreateTables();
            //_pckElementos = new ObservableCollection<ModeloPicker>();

            for (int i = 0; i <= 10; i++)
            {
                _pckOrigen.Add(new ModeloPicker
                {
                    id = i.ToString(),
                    nombreDivisa = "Dolar 1"
                });
            }
            
            for (int i = 0; i <= 5; i++)
            {
                _pckDestino.Add(new ModeloPicker
                {
                    id = i.ToString(),
                    nombreDivisa = "Dolar 1"
                });
            }

            try
            {
                for (int i = 0; i <= 10; i++)
                {
                    TableRecord historial = new TableRecord();
                    historial.Dolar = "uva " + i;
                    historial.Euro = "pera " + i;
                    int e = conexionDB.Save(historial);
                }

                string query = "SELECT * FROM historial";
                ObservableCollection<TableRecord> listaHistorial = conexionDB.GetHistorial(query);
                // var list3 = getData.GetMatchPasseante(query3);
                if (listaHistorial.Count != 0)
                {
                    foreach (TableRecord h in listaHistorial)
                    {
                        Console.WriteLine("----------------------------- id: " + h.Id + " " + h.Dolar + " " + h.Euro);
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        public async void LlenarPicker()
        {
            String __id = "";
            var __nombre = "Desconocido";
            string enviar = "";
            JObject json = null;
            String ruta = "";
            String respuesta = "";

            enviar = await Config.Config.getWebService("/mod_clima/clima_servicio.php?nombre_pueblo=" + ruta);
            json = JObject.Parse(enviar);
            respuesta = (String)json.GetValue("success");

            if (respuesta.Equals("True"))
            {
                int conteoForeach = 0;
                var resultado = json.Value<JObject>("resultado");
                foreach (JProperty property in resultado.Properties())
                {

                    _pckOrigen.Add(new ModeloPicker
                    {
                        id = __id,
                        nombreDivisa = __nombre,
                    });
                    //RaisePropertyChanged();
                }
            }
        }

        public ICommand ConvertirCommand { get { return new RelayCommand(Convertir); } }

        private async void Convertir()
        {
            Regex expresion = new Regex(@"^\$?(\d{1,3},?(\d{3},?)*\d{3}(.\d{0,3})?|\d{1,3}(.\d{2})?)$");

            try
            {
                if (!expresion.IsMatch(txtMontoIngresado))
                {
                    await dialogService.ShowMessage("Advertencia", "Favor de ingresar un formato de moneda valido, ejemplo: 25.00");
                }
                else
                {
                    if (Double.Parse(txtMontoIngresado) < 1)
                    {
                        await dialogService.ShowMessage("Advertencia", "Favor de ingresar un valor diferente de 0");
                    }
                    else
                    {
                        await dialogService.ShowMessage("ok", "Si cumple el formato de moneda");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public ICommand LimpiarCommand { get { return new RelayCommand(Limpiar); } }

        private async void Limpiar()
        {
            txtMontoIngresado = "";
            lblResultadoMonto = "0.0";
            pckOrigenSeleccionado = null;
            pckDestinoSeleccionado = null;
        }

        public ICommand SalirCommand { get { return new RelayCommand(Salir); } }

        private async void Salir()
        {
            System.Environment.Exit(0);
        }

        public string lblMonto
        {
            set { SetProperty(ref _lblMonto, value); }
            get { return _lblMonto; }
        }

        public string lblOrigen
        {
            set { SetProperty(ref _lblOrigen, value); }
            get { return _lblOrigen; }
        }
        
        public string pckTitulo
        {
            set { SetProperty(ref _pckTitulo, value); }
            get { return _pckTitulo; }
        }

        public string lblDestino
        {
            set { SetProperty(ref _lblDestino, value); }
            get { return _lblDestino; }
        }

        public string btnConvertir
        {
            set { SetProperty(ref _btnConvertir, value); }
            get { return _btnConvertir; }
        }

        public string btnLimpiar
        {
            set { SetProperty(ref _btnLimpiar, value); }
            get { return _btnLimpiar; }
        }

        public string btnSalir
        {
            set { SetProperty(ref _btnSalir, value); }
            get { return _btnSalir; }
        }

        public string lblResultado
        {
            set { SetProperty(ref _lblResultado, value); }
            get { return _lblResultado; }
        }

        public string lblResultadoMonto
        {
            set { SetProperty(ref _lblResultadoMonto, value); }
            get { return _lblResultadoMonto; }
        }

        public string txtMontoIngresado
        {
            set { SetProperty(ref _txtMontoIngresado, value); }
            get { return _txtMontoIngresado; }
        }

        public List<ModeloPicker> pckOrigen
        {
            get { return _pckOrigen; }
            private set{
                _pckOrigen = value;
            }
        }

        public ModeloPicker pckOrigenSeleccionado
        {
            get { return _pckOrigenSeleccionado; }
            set
            {
                SetProperty(ref _pckOrigenSeleccionado, value);
                if (_pckOrigenSeleccionado != null){
                    //idTransporte = Converter.ConverterValueInt(_pckSeleccionarTransporte.Id);
                }
                else{
                    _pckOrigenSeleccionado = null;
                }
            }
        }

        public List<ModeloPicker> pckDestino
        {
            get { return _pckDestino; }
            private set {
                _pckDestino = value;
            }
        }

        public ModeloPicker pckDestinoSeleccionado
        {
            get { return _pckDestinoSeleccionado; }
            set
            {
                SetProperty(ref _pckDestinoSeleccionado, value);
                if (_pckDestinoSeleccionado != null) {
                    //idTransporte = Converter.ConverterValueInt(_pckSeleccionarTransporte.Id);
                }
                else{
                    _pckDestinoSeleccionado = null;
                }
            }
        }


        /*public decimal Dollars
        {
            set
            {
                if (dollars != value)
                {
                    dollars = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Dollars"));
                }
            }
            get
            {
                return dollars;
            }
        }*/

        bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /*public void SaveProfile(int id, int responseIdUser, string responseName, string responseLastName)
    {
        if (id == 0)
        {
            TableProfile savePro = new TableProfile();
            savePro.IdUser = responseIdUser;
            savePro.Name = responseName;
            savePro.LastName = responseLastName;
            savePro.BirthDate = responseBirthDate.Date.ToString("yyy-MM-dd");
            int e = setLogin.Save(savePro);
        }
        else
        {
            TableProfile savePro = new TableProfile();
            savePro.Id = id;
            savePro.IdUser = responseIdUser;
            savePro.Name = responseName;
            savePro.LastName = responseLastName;
            savePro.BirthDate = responseBirthDate.Date.ToString("yyy-MM-dd");
            int e = setLogin.Save(savePro);
        }
    }*/
}
