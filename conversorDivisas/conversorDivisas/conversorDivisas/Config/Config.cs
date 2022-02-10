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
            string urlprincipal = "http://api.exchangeratesapi.io/v1/";
            string password = "latest?access_key=bc7dba21855a8412e47921a997992103" + valor;
            string urlFinal = urlprincipal + password + valor;
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(10);
            var response = await client.GetStringAsync(urlFinal);

            if (response != null){
                return response;
            }
            else{
                return null;
            }
        }
    }
}
