using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace conversorDivisas.Config
{
    public class Config
    {
        private string WS_LOGIN { get; set; }
        public static async Task<String> getWebService(String valor)
        {
            String ruta = "http://18.216.125.131/" + valor;
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(10);
            var response = await client.GetStringAsync(ruta);

            if (response != null){
                return response;
            }
            else{
                return null;
            }
        }
    }
}
