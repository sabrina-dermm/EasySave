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
            Interface interFace = new Interface(model);
            Controler con = new Controler(model, interFace);
            con.start();
        }
    }
}
