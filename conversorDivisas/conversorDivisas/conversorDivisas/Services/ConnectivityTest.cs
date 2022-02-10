using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace conversorDivisas.Services
{
    public class ConnectivityTest
    {
        private DialogService dialogService;
        public ConnectivityTest()
        {
            dialogService = new DialogService();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;
            var profiles = e.ConnectionProfiles;
            string respuesta = "No se pudo comprobar la conexión a internet";
            if (access != NetworkAccess.Internet)
            {
                respuesta = "Sin conexión a internet";
            }
            else
            {
                if (profiles.Contains(ConnectionProfile.WiFi))
                {
                    respuesta = "Conexión Wi-Fi";
                }
                else if (profiles.Contains(ConnectionProfile.Cellular))
                {
                    respuesta ="Conexión datos móviles";
                }
            }
            dialogService.ShowMessage("Advertencia", respuesta);
        }
    }
}
