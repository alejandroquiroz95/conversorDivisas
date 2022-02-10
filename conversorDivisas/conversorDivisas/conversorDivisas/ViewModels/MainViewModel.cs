using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using conversorDivisas.Models;
using conversorDivisas.Services;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Essentials;

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
        private string _lblResultadoMonto = "0";
        string r_usd = "0";
        string r_eur = "0";
        string r_mxn = "0";
        string r_gbp = "0";
        string r_cad = "0";
        string r_aud = "0";
        string idSOrigen = "0";
        string idSDestino = "0";
        private List<ModelPicker> _pckOrigen;
        private List<ModelPicker> _pckDestino;
        private ModelPicker _pckOrigenSeleccionado;
        private ModelPicker _pckDestinoSeleccionado;
        public ObservableCollection<ModelRecord> listaDivisas { get; set; }
        ConnectionDB conexionDB;
        private ConnectivityTest conexionInternet;

        public MainViewModel()
        {
            dialogService = new DialogService();
            _pckOrigen = new List<ModelPicker>();
            _pckDestino = new List<ModelPicker>();
            listaDivisas = new ObservableCollection<ModelRecord>();
            conexionDB = new ConnectionDB();
            conexionInternet = new ConnectivityTest();
            //string query2 = "DROP TABLE historial";
            //ObservableCollection<ModelTableRecord> listaHistorial2 = conexionDB.GetHistorial(query2);
            conexionDB.CreateTables();
            GuardarHistorial();
            LlenarPicker();
            ConsultarHistorial();  
        }

        public void LlenarPicker()
        {
            var listDivisas = new List<String> {"USD", "EUR", "MXN", "GBP", "CAD", "AUD"};
            int i = 1;
            foreach (string elemento in listDivisas)
            {

                _pckOrigen.Add(new ModelPicker
                {
                    id = i.ToString(),
                    nombreDivisa = elemento
                });

                _pckDestino.Add(new ModelPicker
                {
                    id = i.ToString(),
                    nombreDivisa = elemento
                });
                i++;
            }
        }

        public async void ConsultarHistorial()
        {
            try
            {
                string query = "SELECT * FROM historial";
                ObservableCollection<ModelTableRecord> listaHistorial = conexionDB.GetHistorial(query);
                if (listaHistorial.Count != 0)
                {
                    listaDivisas.Add(new ModelRecord
                    {
                        usd = "USD  |" + "  EUR  |" +"  MXN  |" + "  GBP  |" + "  CAD  |" + "  AUD"

                    });
                    foreach (ModelTableRecord h in listaHistorial)
                    {
                        listaDivisas.Add(new ModelRecord
                        {
                            usd = h.USD + "  |  "+ h.EUR +"  |  "+ h.MXN + "  |  "+ h.GBP + "  |  "+ h.CAD + "  |"+ h.AUD
                        });

                    }
                }
            }
            catch(Exception ex)
            {
            } 
        }

        public async void GuardarHistorial()
        {
            string enviar = "";
            string respuesta = "";
            string fecha = "";
            JObject json = null;

            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    enviar = await Config.Config.getWebService("&base=EUR&simbols=USD,AUD,CAD,PLN,MXN");
                    json = JObject.Parse(enviar);
                    respuesta = (String)json.GetValue("success");
                    fecha = (String)json.GetValue("date");

                    if (respuesta == "True")
                    {
                        string usd2 = "0";
                        string eur2 = "0";
                        string mxn2 = "0";
                        string gbp2 = "0";
                        string cad2 = "0";
                        string aud2 = "0";

                        var resultado = json.Value<JObject>("rates");
                        foreach (JProperty property in resultado.Properties())
                        {
                            usd2 = resultado.Value<String>("USD");
                            eur2 = resultado.Value<String>("EUR");
                            mxn2 = resultado.Value<String>("MXN");
                            gbp2 = resultado.Value<String>("GBP");
                            cad2 = resultado.Value<String>("CAD");
                            aud2 = resultado.Value<String>("AUD");
                        }

                        r_usd = ConvertirADolar(usd2, usd2);
                        r_eur = ConvertirADolar(usd2, eur2);
                        r_mxn = ConvertirADolar(usd2, mxn2);
                        r_gbp = ConvertirADolar(usd2, gbp2);
                        r_cad = ConvertirADolar(usd2, cad2);
                        r_aud = ConvertirADolar(usd2, aud2);

                        string query = "SELECT * FROM historial WHERE Fecha = '" + fecha + "'";
                        ObservableCollection<ModelTableRecord> listaFecha = conexionDB.GetHistorial(query);
                        if (listaFecha.Count == 0)
                        {
                            ModelTableRecord historial = new ModelTableRecord();
                            historial.USD = r_usd;
                            historial.EUR = r_eur;
                            historial.MXN = r_mxn;
                            historial.GBP = r_gbp;
                            historial.CAD = r_cad;
                            historial.AUD = r_aud;
                            historial.Fecha = fecha;
                            int e = conexionDB.Save(historial);
                        }
                    }
                }
                else
                {
                     dialogService.ShowMessage("Advertencia", "Sin conexión a internet");
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        public string FormulaConvertir(string vo, string vd, string monto)
        {
            string resultado = "0";
            float operacion = 0;
            //vd * m/ vo 
            if (vo != null || vo != "" && vd != null || vd != "" && monto != null || monto != "")
            {
                operacion = float.Parse(vd) * float.Parse(monto) / float.Parse(vo);
                resultado = operacion.ToString();
            }

            return resultado;
        }

        public string ConvertirADolar(string dolar, string valor)
        {
            string resultado = "0";
            float operacion = 0;
            if (valor != null || valor != "")
            {
                operacion = float.Parse(valor) / float.Parse(dolar);
                resultado = operacion.ToString();
            }
            return resultado;
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
                        var current = Connectivity.NetworkAccess;
                        if (current == NetworkAccess.Internet)
                        {
                            string vOrigen = valorSeleccion(idSOrigen);
                            string vDestino = valorSeleccion(idSDestino);
                            string resultadoC = FormulaConvertir(vOrigen, vDestino, txtMontoIngresado);
                            lblResultadoMonto = resultadoC;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public string valorSeleccion(string valorSeleccion)
        {
            string resultado = "0";
            switch (valorSeleccion)
            {
                case "1":
                    resultado = r_usd;
                    break;
                case "2":
                    resultado = r_eur;
                    break;
                case "3":
                    resultado = r_mxn;
                    break;
                case "4":
                    resultado = r_gbp;
                    break;
                case "5":
                    resultado = r_cad;
                    break;
                case "6":
                    resultado = r_aud;
                    break;
                default:
                    resultado = "0";
                    break;
            }
            return resultado;
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

        public List<ModelPicker> pckOrigen
        {
            get { return _pckOrigen; }
            private set{
                _pckOrigen = value;
            }
        }

        public ModelPicker pckOrigenSeleccionado
        {
            get { return _pckOrigenSeleccionado; }
            set
            {
                SetProperty(ref _pckOrigenSeleccionado, value);
                if (_pckOrigenSeleccionado != null){

                    idSOrigen = _pckOrigenSeleccionado.id;
                }
                else{
                    _pckOrigenSeleccionado = null;
                }
            }
        }

        public List<ModelPicker> pckDestino
        {
            get { return _pckDestino; }
            private set {
                _pckDestino = value;
            }
        }

        public ModelPicker pckDestinoSeleccionado
        {
            get { return _pckDestinoSeleccionado; }
            set
            {
                SetProperty(ref _pckDestinoSeleccionado, value);
                if (_pckDestinoSeleccionado != null) {
                    idSDestino = _pckDestinoSeleccionado.id;
                }
                else
                {
                    _pckDestinoSeleccionado = null;
                }
            }
        }

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
}
