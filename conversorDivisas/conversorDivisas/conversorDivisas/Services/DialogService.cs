﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace conversorDivisas.Services
{
    public class DialogService
    {
        public async Task ShowMessage(string title, string message)
        {
            await App.Current.MainPage.DisplayAlert(title, message, "Aceptar");
        }

        public async Task<bool> ShowConfirm(string title, string message)
        {
            return await App.Current.MainPage.DisplayAlert(title, message, "Sí", "No");
        }

    }
}
