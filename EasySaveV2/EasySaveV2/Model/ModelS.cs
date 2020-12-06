using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EasySaveV2.Model
{
    public class ModelS
    {
        private static List<SaveWork> saveWorkList;

        //Create the constructer
        public ModelS()
        {
            saveWorkList = new List<SaveWork>()
            
              {
                new SaveWork
                {
                    NameSave="save 1", SrcPath="src 1", DestPath="dest 1"
                    , Type="unset"
                }
            };
             

        }


        //methode to get all the saves

        public List<SaveWork> getAll()
        {
            return saveWorkList;
        }

        // methode to add the employee in the list and return true if it worked
        public bool addSaveWork(SaveWork save)
        {
            if ((!Regex.IsMatch(save.NameSave, @"^[a-zA-Z0-9 _]+$")))
            {
                throw new ArgumentException("Please only make use of alphanumeric characters, spaces or underscores.");
            }

            
             
            if (!Regex.IsMatch(save.SrcPath, @"^[a-zA-Z]:(?:\/[a-zA-Z0-9 _]+)*$"))
            {
                throw new ArgumentException("Please enter a valid absolute path.");

            }
            if (!Regex.IsMatch(save.DestPath, @"^[a-zA-Z]:(?:\/[a-zA-Z0-9 _]+)*$"))
            {
                throw new ArgumentException("Please enter a valid absolute path.");
            }
              if (save.Type != "complete" && save.Type != "differential")
            {
                throw new ArgumentException("You can only choose between complete and differential");
            }
            saveWorkList.Add(save);
            return true;
        }


        // methode to modift the list 
        //i need to modify this -------------------------------------------------------
        public bool updateList(SaveWork em)
        {
            bool isUpdated = false;
            for (int i = 0; i < saveWorkList.Count; i++)
            {
                if (saveWorkList[i].NameSave == em.NameSave)
                {
                    saveWorkList[i] = em;
                    isUpdated = true;
                    break;
                }
            }
            return isUpdated;
        }

    }
}
