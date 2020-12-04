using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using consoleApp.Model;

namespace consoleApp.View
{
    class InterFace
    {
        public ModelS model { get; set; }

        public InterFace(ModelS _model)
        {
            this.model = _model;
        }



        //Returns the user choice as an string to the controller.
        public string ShowMainMenu()
        {
            Console.WriteLine("\n\nPlease select an option :\n" +
                "1. Launch a save procedure.\n" +
                "2. Create a save procedure.\n" +
                "3. Modify a save procedure.\n" +
                "4. Delete a save procedure.\n" +
                "5. Launch all save procedures sequentially.\n" +
                "9. Close application.\n");

            return Console.ReadLine();
        }
        //traduction of showMainMenu methode
        public string AfficherMainMenu()
        {
            Console.WriteLine("\n\nVeuillez selectionner une option :\n" +
                "1. Lancer une procédure de sauvegarde.\n" +
                "2. Crée une procédure de sauvegarde.\n" +
                "3. Modifier une procédure de sauvegarde.\n" +
                "4. Supprimer une procédure de sauvegarde.\n" +
                "5. Lancer toutes les procédures de sauvegarde séquentiellement.\n" +
                "9. Fermer l'application.\n");

            return Console.ReadLine();
        }

        //Allows the user to create a new save procedure by giving it's name, source path, destination path and type.
        public string[] CreateSaveProcedure()
        {
            string[] choice = new string[5];

            
                // Ask for name.
                Console.WriteLine("\nChoose a name for your save procedure:");
                string enteredName = Console.ReadLine();
                while (!Regex.IsMatch(enteredName, @"^[a-zA-Z0-9 _]+$"))  //Regex only allowing alphanumeric chars, spaces or underscores.
                {
                    Console.WriteLine("\nPlease only make use of alphanumeric characters, spaces or underscores.\n");
                    enteredName = Console.ReadLine();
                }
                choice[0] = enteredName;
                

                // Ask for source path.
                Console.WriteLine("\nChoose a source path to save :");
                string enteredSource = Console.ReadLine();
                while (!Regex.IsMatch(enteredSource, @"^[a-zA-Z]:(?:\/[a-zA-Z0-9 _]+)*$"))  //Regex for valid windows folder path.
                {
                    Console.WriteLine("\nPlease enter a valid absolute path.\n");
                    enteredSource = Console.ReadLine();
                }
                choice[1] = enteredSource;
                

                // Ask for destination path.
                Console.WriteLine("\nChoose a destination path to export the save :");
                string enteredDestination = Console.ReadLine();
                while (!Regex.IsMatch(enteredDestination, @"^[a-zA-Z]:(?:\/[a-zA-Z0-9 _]+)*$"))  //Regex for valid windows folder path.
                {
                    Console.WriteLine("\nPlease enter a valid absolute path.\n");
                    enteredDestination = Console.ReadLine();
                }
                choice[2] = enteredDestination;
                


                // Ask for backup type.
                Console.WriteLine("\nChoose a save type :\n" +
                    "1. Complete save\n" +
                    "2. Differential save");

                string saveTypeChoice = Console.ReadLine();

                // Check if input is correct.
                while (saveTypeChoice != "1" && saveTypeChoice != "2")
                {
                    Console.WriteLine("\nPlease enter a correct value to proceed.");
                    saveTypeChoice = Console.ReadLine();
                }
                choice[3] = saveTypeChoice;
                
             return choice;
        }
        //traduction of CreateSaveProcedure methode
        public string[] CreeUneProcedureDeSauvegarde()
        {
            string[] choice = new string[5];


            // Ask for name.
            Console.WriteLine("\nChoisissez un nom pour votre procédure de sauvegarde: ");
            string enteredName = Console.ReadLine();
            while (!Regex.IsMatch(enteredName, @"^[a-zA-Z0-9 _]+$"))  //Regex only allowing alphanumeric chars, spaces or underscores.
            {
                Console.WriteLine("\nVeuillez n'utiliser que des caractères alphanumériques, des espaces ou des traits de soulignement.\n");
                enteredName = Console.ReadLine();
            }
            choice[0] = enteredName;


            // Ask for source path.
            Console.WriteLine("\nChoisissez un chemin source à enregistrer: ");
            string enteredSource = Console.ReadLine();
            while (!Regex.IsMatch(enteredSource, @"^[a-zA-Z]:(?:\/[a-zA-Z0-9 _]+)*$"))  //Regex for valid windows folder path.
            {
                Console.WriteLine("\nVeuillez entrer un chemin absolu valide\n");
                enteredSource = Console.ReadLine();
            }
            choice[1] = enteredSource;


            // Ask for destination path.
            Console.WriteLine("\nChoisissez un chemin de destination pour exporter la sauvegarde:");
            string enteredDestination = Console.ReadLine();
            while (!Regex.IsMatch(enteredDestination, @"^[a-zA-Z]:(?:\/[a-zA-Z0-9 _]+)*$"))  //Regex for valid windows folder path.
            {
                Console.WriteLine("\nVeuillez entrer un chemin absolu valide.\n");
                enteredDestination = Console.ReadLine();
            }
            choice[2] = enteredDestination;



            // Ask for backup type.
            Console.WriteLine("\nChoisissez un type de sauvegarde:\n" +
                "1. Sauvegarde complète\n" +
                "2. Sauvegarde différentielle");

            string saveTypeChoice = Console.ReadLine();

            // Check if input is correct.
            while (saveTypeChoice != "1" && saveTypeChoice != "2")
            {
                Console.WriteLine("\nVeuillez saisir une valeur correcte pour continuer.");
                saveTypeChoice = Console.ReadLine();
            }
            choice[3] = saveTypeChoice;

            return choice;
        }
        //Allows the user to modifiy an existing save procedure name, source path, destination path and/or type.
        public SaveWork ModifySaveProcedure(SaveWork _save)
        {
            
                SaveWork modifiedSave = _save;
                string choice = "";

                while (choice != "5" && choice != "9") //While loop to allow the user to modify multiple values.
                {
                    Console.WriteLine("\n\nPlease select a parameter to modify :\n" +
                        "1. Name : " + modifiedSave.name +
                        "\n2. Source Path : " + modifiedSave.sourcePath +
                        "\n3. Destination Path : " + modifiedSave.destinationPath +
                        "\n4. Save Type : " + modifiedSave.type +
                        "\n5. Confirm" +
                        "\n9. Cancel.\n");

                    choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("\nPlease enter a new name\n");
                            string enteredName = Console.ReadLine();
                            while (!Regex.IsMatch(enteredName, @"^[a-zA-Z0-9 _]+$"))  //Regex only allowing alphanumeric chars, spaces or underscores.
                            {
                                Console.WriteLine("\nPlease only make use of alphanumeric characters, spaces or underscores.\n");
                                enteredName = Console.ReadLine();
                            }
                            modifiedSave.name = enteredName;
                            break;

                        case "2":
                            Console.WriteLine("Please enter a new source path to save (absolute) :\n");
                            string enteredSource = Console.ReadLine();
                            while (!Regex.IsMatch(enteredSource, @"^[a-zA-Z]:(?:\/[a-zA-Z0-9 _]+)*$"))  //Regex for valid windows folder path.
                            {
                                Console.WriteLine("\nPlease enter a valid absolute path.\n");
                                enteredSource = Console.ReadLine();
                            }
                            modifiedSave.sourcePath = enteredSource;
                            break;

                        case "3":
                            Console.WriteLine("Please enter a new destination path to export the save (absolute) :\n");
                            string enteredDestination = Console.ReadLine();
                            while (!Regex.IsMatch(enteredDestination, @"^[a-zA-Z]:(?:\/[a-zA-Z0-9 _]+)*$"))  //Regex for valid windows folder path.
                            {
                                Console.WriteLine("\nPlease enter a valid absolute path.\n");
                                enteredDestination = Console.ReadLine();
                            }
                            modifiedSave.sourcePath = enteredDestination;
                            break;

                        case "4":
                            Console.WriteLine("Please choose a new save type :\n" +
                                "1. Complete\n" +
                                "2. Differencial\n");

                            string enteredValue = Console.ReadLine();

                            //Check for valid value entered by the user (1 or 2).
                            while (enteredValue != "1" && enteredValue != "2")
                            {
                                Console.WriteLine("\nPlease enter a correct value to proceed.\n");
                                enteredValue = Console.ReadLine();
                            }

                            modifiedSave.type = enteredValue == "1" ? SaveWorkType.complete : SaveWorkType.differencial;
                            break;

                        case "5":
                            break;

                        case "9":
                            break;
                        default:
                            Console.WriteLine("\nPlease enter a correct value to proceed.\n");
                            break;
                    }

                }
                return choice == "5" ? modifiedSave : null;
           
        }
        //traduction of modify methode
        public SaveWork ModifierUneProcedureDeSauvegarde(SaveWork _save)
        {

            SaveWork modifiedSave = _save;
            string choice = "";

            while (choice != "5" && choice != "9") //While loop to allow the user to modify multiple values.
            {
                Console.WriteLine("\n\nVeuillez sélectionner un paramètre à modifier:\n" +
                    "1. Nom : " + modifiedSave.name +
                    "\n2. Chemin Source : " + modifiedSave.sourcePath +
                    "\n3. Chemin de destination : " + modifiedSave.destinationPath +
                    "\n4. Type de sauvegarde : " + modifiedSave.type +
                    "\n5. Confirmer" +
                    "\n9. Annuler.\n");

                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("\nVeuillez saisir un nouveau nom\n");
                        string enteredName = Console.ReadLine();
                        while (!Regex.IsMatch(enteredName, @"^[a-zA-Z0-9 _]+$"))  //Regex only allowing alphanumeric chars, spaces or underscores.
                        {
                            Console.WriteLine("\nVeuillez n'utiliser que des caractères alphanumériques, des espaces ou des traits de soulignement.\n");
                            enteredName = Console.ReadLine();
                        }
                        modifiedSave.name = enteredName;
                        break;

                    case "2":
                        Console.WriteLine("Veuillez saisir un nouveau chemin source à enregistrer (absolu):\n");
                        string enteredSource = Console.ReadLine();
                        while (!Regex.IsMatch(enteredSource, @"^[a-zA-Z]:(?:\/[a-zA-Z0-9 _]+)*$"))  //Regex for valid windows folder path.
                        {
                            Console.WriteLine("\nVeuillez entrer un chemin absolu valide.\n");
                            enteredSource = Console.ReadLine();
                        }
                        modifiedSave.sourcePath = enteredSource;
                        break;

                    case "3":
                        Console.WriteLine("Veuillez entrer un nouveau chemin de destination pour exporter la sauvegarde(absolue):\n");
                        string enteredDestination = Console.ReadLine();
                        while (!Regex.IsMatch(enteredDestination, @"^[a-zA-Z]:(?:\/[a-zA-Z0-9 _]+)*$"))  //Regex for valid windows folder path.
                        {
                            Console.WriteLine("\nVeuillez entrer un chemin absolu valide.\n");
                            enteredDestination = Console.ReadLine();
                        }
                        modifiedSave.sourcePath = enteredDestination;
                        break;

                    case "4":
                        Console.WriteLine("Veuillez choisir un nouveau type de sauvegarde:\n" +
                            "1. Complète\n" +
                            "2. Différentielle\n");

                        string enteredValue = Console.ReadLine();

                        //Check for valid value entered by the user (1 or 2).
                        while (enteredValue != "1" && enteredValue != "2")
                        {
                            Console.WriteLine("\nVeuillez saisir une valeur correcte pour continuer.\n");
                            enteredValue = Console.ReadLine();
                        }

                        modifiedSave.type = enteredValue == "1" ? SaveWorkType.complete : SaveWorkType.differencial;
                        break;

                    case "5":
                        break;

                    case "9":
                        break;
                    default:
                        Console.WriteLine("\nVeuillez saisir une valeur correcte pour continuer.\n");
                        break;
                }

            }
            return choice == "5" ? modifiedSave : null;

        }
        //Shows the menu from which you can select a save procedure to delete. It receives all the procedures as a parameter.
        public int SelectSaveProcedure(List<SaveWork> _saveList)
        {
            if (_saveList == null)
            {
                Console.WriteLine("\nNo save procedures created yet.");
                return 0;
            }

            int increment = 0;
            //Later, we'll check if the value entered by the user is in this regex string, meaning it corresponds to a save procedure or the cancel option. Can be considered as an int list.
            string regexNumbers = "0";

            //Write the name of every save procedure in the terminal as a list and add the procedure index in the string regexNumbers.
            foreach (SaveWork saveWork in _saveList)
            {
                increment++;
               
                    regexNumbers += increment;
                    Console.WriteLine(increment + ". " + saveWork.name + "\n");
                
            }
            Console.WriteLine("0. Cancel\n");


            string enteredValue = Console.ReadLine();


            //Check for valid value entered by the user.
            while (!Regex.IsMatch(enteredValue, @"^[" + regexNumbers + "]$"))
            {
                Console.WriteLine("\nPlease enter a correct value to proceed.\n");
                enteredValue = Console.ReadLine();
            }

            //Will return the index of the save procedure or 9 if "9" is the value entered.
            /*
            if(enteredValue == "0")
            {
                return 0;
            }
            else
            {
                return int.Parse(enteredValue);
            }*/
            return enteredValue != "0" ? int.Parse(enteredValue) : 0;
        }


        //traduction of the SelectSaveProcedure
        public int SelectionProcedureDeSauvegarde(List<SaveWork> _saveList)
        {
            if (_saveList == null)
            {
                Console.WriteLine("\nAucune procédure de sauvegarde n'a encore été crée.");
                return 0;
            }

            int increment = 0;
            //Later, we'll check if the value entered by the user is in this regex string, meaning it corresponds to a save procedure or the cancel option. Can be considered as an int list.
            string regexNumbers = "0";

            //Write the name of every save procedure in the terminal as a list and add the procedure index in the string regexNumbers.
            foreach (SaveWork saveWork in _saveList)
            {
                increment++;

                regexNumbers += increment;
                Console.WriteLine(increment + ". " + saveWork.name + "\n");

            }
            Console.WriteLine("0. Annuler\n");


            string enteredValue = Console.ReadLine();


            //Check for valid value entered by the user.
            while (!Regex.IsMatch(enteredValue, @"^[" + regexNumbers + "]$"))
            {
                Console.WriteLine("\nVeuillez saisir une valeur correcte pour continuer.\n");
                enteredValue = Console.ReadLine();
            }

           
            return enteredValue != "0" ? int.Parse(enteredValue) : 0;
        }

        //The user has to confirm critical interactions.
        public bool Confirm()
        {
            Console.WriteLine("\nAre you sure you want to do this ? y/n");

            string choice = Console.ReadLine();

            while (choice != "y" && choice != "n")
            {
                Console.WriteLine("\nPlease enter a correct value to proceed.\n");
                choice = Console.ReadLine();
            }

            if (choice == "y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //traduction of confirm methode

        public bool Confirmer()
        {
            Console.WriteLine("\nEs-tu sûr de vouloir faire ça ? o/n");

            string choice = Console.ReadLine();

            while (choice != "o" && choice != "n")
            {
                Console.WriteLine("\nPlease enter a correct value to proceed.\n");
                choice = Console.ReadLine();
            }

            if (choice == "o")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /* Simple Void Console Write methods */


        public int Start()
        {
            
            String choiceLanguage = null;
            Console.WriteLine("Welcome to EasySave !\nEasySave v.1.0");
            Console.WriteLine("Please choose a language that you want to prceed with");
            Console.WriteLine("1. English\n2. French ");
            choiceLanguage= Console.ReadLine();
            while(choiceLanguage != "1" && choiceLanguage != "2")
            {
                Console.WriteLine("Please enter a valid value !");
                choiceLanguage = Console.ReadLine();
            }
            if(choiceLanguage == "1")
            {
                return 1;
            }
            else
            {
                return 2;
            }

        }

        //Shows a different message depending on save type.
        public void SaveInProgressMessage(SaveWork _save)
        {
            if (_save.type == SaveWorkType.differencial)
            {
                Console.WriteLine("\nDifferential save " + _save.name + " in progress...");
            }
            else if (_save.type == SaveWorkType.complete)
            {
                Console.WriteLine("\nComplete save " + _save.name + " in progress...");
            }
        }
        //traduction of SaveInProgressMessage method
        public void EnregistrerLeMessageEnCours(SaveWork _save)
        {
            if (_save.type == SaveWorkType.differencial)
            {
                Console.WriteLine("\nSauvegarde différentiel " + _save.name + " en cours...");
            }
            else if (_save.type == SaveWorkType.complete)
            {
                Console.WriteLine("\nSauvegarde complète " + _save.name + " en cours...");
            }
        }

        public void OperationDoneMessage()
        {
            Console.WriteLine("\nDone.");
        }
        // traduction of OperationDoneMessage methode
        public void MessageDoperationTerminee()
        {
            Console.WriteLine("\nTerminé.");
        }

        public void SaveIsDoneMessage(SaveWork _save) //Done method is different for the launch option as we don't want to show unset save procedures.
        {
            
                Console.WriteLine("\nDone.");
            
        }
        //traduction of done methode
        public void EnregistrerLeMessageTermine(SaveWork _save) //Done method is different for the launch option as we don't want to show unset save procedures.
        {

            Console.WriteLine("\nTerminé.");

        }

        //Shows a different message depending on selection.
        public void TerminalMessage(string _type)
        {
            Console.WriteLine("\nSelect a save procedure to " + _type + " or return to the main menu :\n");
        }
        //traduction of terminaMessage methode
        public void AfficherMessage(string _type)
        {
            Console.WriteLine("\nSelectionnez une procedure de sauvegarde " + _type + " ou bien revener au menu :\n");
        }
    }
}
