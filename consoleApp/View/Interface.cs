﻿using System;
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



        /* Simple Void Console Write methods */


        public void Start()
        {
            Console.WriteLine("Welcome to EasySave !\nEasySave v.1.0");
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

        public void OperationDoneMessage()
        {
            Console.WriteLine("\nDone.");
        }

        public void SaveIsDoneMessage(SaveWork _save) //Done method is different for the launch option as we don't want to show unset save procedures.
        {
            
                Console.WriteLine("\nDone.");
            
        }

        //Shows a different message depending on selection.
        public void TerminalMessage(string _type)
        {
            Console.WriteLine("\nSelect a save procedure to " + _type + " or return to the main menu :\n");
        }
    }
}
