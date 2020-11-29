using consoleApp.Model;
using consoleApp.View;
namespace consoleApp.Controller
{
    class Controler
    {
        public ModelS model { get; set; }
        public InterFace view { get; set; }

        public Controler(ModelS model, InterFace view)
        {
            this.model = model;
            this.view = view;
        }

        public void Start()
        {
            view.Start();
            ShowMenu();
        }

        private void LaunchSave()
        {
            view.TerminalMessage("launch");
            int saveProcedureIndex = view.SelectSaveProcedure(model.WorkList);

            if (saveProcedureIndex != 9)
            {
                if (view.Confirm())
                {
                    //To Implement (sauvegarde en cours blablabla)

                    view.SaveInProgressMessage(model.WorkList[saveProcedureIndex - 1]);
                    model.DoSave(saveProcedureIndex);
                    view.SaveIsDoneMessage(model.WorkList[saveProcedureIndex - 1]);
                    //fonction vue pour retour user
                    ShowMenu();
                    return;
                }
                else
                {
                    ShowMenu();
                    return;
                }
            }
            else
            {
                ShowMenu();
                return;
            }
        }

        private void CreateSave()
        {
            string[] saveProcedure = view.CreateSaveProcedure();

            SaveWorkType type = SaveWorkType.unset;
            if (saveProcedure[3] == "1")
            {
                type = SaveWorkType.complete;
            }
            else if (saveProcedure[3] == "2")
            {
                type = SaveWorkType.differencial;
            }
            model.CreateWork(int.Parse(saveProcedure[4]), saveProcedure[0], saveProcedure[1], saveProcedure[2], type);


            ShowMenu();
            return;
        }

        private void ModifySave()
        {
            view.TerminalMessage("modify");
            int saveProcedureIndex = view.SelectSaveProcedure(model.WorkList);
            if (saveProcedureIndex != 9)
            {
                SaveWork saveProcedure = view.ModifySaveProcedure(model.WorkList[saveProcedureIndex - 1]);
                if (saveProcedure != null)
                {
                    model.ChangeWork(saveProcedureIndex, saveProcedure.name, saveProcedure.sourcePath, saveProcedure.destinationPath, saveProcedure.type);
                    view.OperationDoneMessage();
                }
            }
            ShowMenu();
            return;
        }

        private void DeleteSave()
        {
            view.TerminalMessage("delete");
            int saveProcedureIndex = view.SelectSaveProcedure(model.WorkList);
            if (saveProcedureIndex != 9)
            {
                if (view.Confirm())
                {
                    model.DeleteWork(saveProcedureIndex);
                    //TODO: Afficher done delete
                    view.OperationDoneMessage();
                    ShowMenu();
                    return;
                }
                else
                {
                    ShowMenu();
                    return;
                }
            }
            ShowMenu();
            return;

        }

        private void LaunchAllSavesSequentially()
        {
            if (view.Confirm())
            {
                for (int i = 1; i < model.WorkList.Length + 1; i++)
                {
                    view.SaveInProgressMessage(model.WorkList[i - 1]);
                    model.DoSave(i);
                    view.SaveIsDoneMessage(model.WorkList[i - 1]);
                }
                ShowMenu();
                return;
            }
            else
            {
                ShowMenu();
                return;
            }
        }

        private void ShowMenu()
        {
            switch (view.ShowMainMenu())
            {
                case "1":
                    LaunchSave();
                    break;
                case "2":
                    CreateSave();
                    break;
                case "3":
                    ModifySave();
                    break;
                case "4":
                    DeleteSave();
                    break;
                case "5":
                    LaunchAllSavesSequentially();
                    break;
                default:
                    break;
            }

        }
    }
}
