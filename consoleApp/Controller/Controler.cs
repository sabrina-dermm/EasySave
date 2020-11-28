using consoleApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using consoleApp.View;
namespace consoleApp.Controller
{
    class Controler
    {

        public ModelS model { get; set; }
        public Interface interFace { get; set; }
        public Controler(ModelS model, Interface interFace)
        {
            this.model = model;
            this.interFace = interFace;

        }
        public void start()
        {
            interFace.welcome();
            chooseTheOperation();
        }
        private void createSave()
        {
            String[] data = interFace.GetDataSaveProcedure();
            SaveType type = SaveType.notSet;
            //get the type of the backup

            if (data[3] == "1")
            {
                type = SaveType.complete;
            }
            else
            {
                type = SaveType.differencial;
            }
            model.createSave(data[0], data[1], data[2], type);
            return;
        }
        private void displaySaveEnvirements()
        {
            interFace.displaySaveEnvirements(model.saveConfigurationList);
        }
        // the methode that will treet the choice of the operation 
        public void chooseTheOperation()
        {
            String choice = interFace.checkIdOperationIsValid(interFace.showAllOperations());

            switch (choice)
            {
                case "1":
                    displaySaveEnvirements();
                    break;
                case "2":
                    // UpdateUserInput();
                    createSave();
                    interFace.creationSuccess();
                    break;
                case "3":
                    //call a methode
                    break;
                case "4":
                    //call a methode
                    break;
                case "5":
                    //call a methode
                    break;
                case "6":
                    //call a methode
                    break;

            }
        }

    }
}
