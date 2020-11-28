using consoleApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace consoleApp.View
{
    class Interface
    {
        
            public ModelS model { get; set; }

            public Interface(ModelS model)
            {
                this.model = model;
            }






            // A methode to welcome the user 
            public void welcome()
            {
                Console.WriteLine("Welcome to EasySave ! ");
                Console.WriteLine("                       ");
                Console.WriteLine("                       ");
                Console.WriteLine("                       ");
                // return checkIdOperationIsValid(showAllOperations());
            }
            // A methode to show all the operations 
            public String showAllOperations()
            {
                Console.WriteLine("\n\nPlease select an option :\n" +
                    "1. Diplay a backup environment list.\n" +
                    "2. Create a save procedure.\n" +
                    "3. Modify a save procedure.\n" +
                    "4. Delete a save procedure.\n" +
                    "5. Launch all save procedures sequentially.\n" +
                    "6. Close application.\n");
                return Console.ReadLine();

            }

            //A methode to check if the operation is valid 
            public String checkIdOperationIsValid(String choice)
            {

                while (!Regex.IsMatch(choice, @"^[1-9]$"))
                {
                    Console.WriteLine("Please enter a valid operation");
                    choice = showAllOperations();

                }
                return choice;
            }


            //A methode to get inputs from the user
            public string[] GetDataSaveProcedure()
            {
                String[] data = new string[5];
                String nameF = null;
                String sourcePath = null;
                String destinationPath = null;
                String typeSave = null;
                //Ask for name of the save procedure
                Console.WriteLine("Choose the name of your save procedure: ");
                nameF = Console.ReadLine();
                while (!Regex.IsMatch(nameF, @"^[a-zA-Z0-9]+$"))
                {
                    Console.WriteLine("Please enter a valid name");
                    nameF = Console.ReadLine();
                }

                data[0] = nameF;
                //Ask for souce path
                Console.WriteLine("Choose a souce path to save :");
                sourcePath = Console.ReadLine();
                while (!Regex.IsMatch(sourcePath, @"^[a-zA-Z]:(?:\/[a-zA-Z0-9 _]+)*$"))
                {
                    Console.WriteLine("Please enter a valide souce path");
                    sourcePath = Console.ReadLine();
                }
                data[1] = sourcePath;
                //ask for destination path
                Console.WriteLine("Choose a destination path to save :");
                destinationPath = Console.ReadLine();
                while (!Regex.IsMatch(destinationPath, @"^[a-zA-Z]:(?:\/[a-zA-Z0-9 _]+)*$"))
                {
                    Console.WriteLine("Please enter a valide destination path");
                    destinationPath = Console.ReadLine();
                }
                data[2] = destinationPath;

                //ask for type of the backup
                Console.WriteLine("Choose the save type:\n" +
                    "1. Complete save\n" +
                    "2. Defferential save\n");
                typeSave = Console.ReadLine();
                while (typeSave != "1" && typeSave != "2")
                {
                    Console.WriteLine("Please enter a correct value to proceed.");
                    typeSave = Console.ReadLine();
                }
                data[3] = typeSave;


                


                return data;

            }
            //show the success message of save procedure
            public void creationSuccess()
            {
                Console.WriteLine("\n \n \n A new backup environment has been created\n ");
            }
            public void displaySaveEnvirements(SaveConfiguration[] saveConfigurationList)
            {
                int saveId = 0;
                if (saveConfigurationList == null)
                {
                    Console.WriteLine("\nNo save procedure created yet");
                    return;
                }
                else
                {
                    foreach (SaveConfiguration saveConfiguration in saveConfigurationList)
                    {
                        saveId++;
                        Console.WriteLine(saveId + saveConfiguration.name);
                    }
                }
            }
        }
}
