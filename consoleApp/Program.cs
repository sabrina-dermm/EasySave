using System;
using consoleApp.Controller;
using consoleApp;
using consoleApp.Model;
using consoleApp.View;

namespace consoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            

            ModelS model = new ModelS();
            InterFace view = new InterFace(model);
            Controler ctrl = new Controler(model, view);
            ctrl.Start();
        }
    }
}
