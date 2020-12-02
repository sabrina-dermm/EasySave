Logiciel de Sauvegarde EasySave 


Votre équipe vient d’intégrer l’éditeur de logiciels ProSoft.   

Sous la responsabilité du DSI, vous aurez la responsabilité de gérer le projet “EasySave” qui consiste à développer un logiciel de sauvegarde .  

Comme tout logiciel de la Suite ProSoft, le logiciel s’intégrera à la politique tarifaire. 

- Prix unitaire : 200 €HT 
- Contrat de maintenance annuel 5/7 8-17h (mises à jour incluses):  12% prix d’achat (Contrat annuel à tacite reconduction avec revalorisation basée sur l’indice SYNTEC) 

Lors de ce projet, votre équipe devra assurer le développement, la gestion des versions majeures et mineures, mais aussi les documentations (utilisateur et support client).  

 Pour garantir une reprise de votre travail par d’autres équipes, la direction vous impose de travailler dans le respect des contraintes suivantes :  

Outils et méthodes  

- Visual Studio 2019  16.3 ou supérieure 
- GIT Azur DevOps.   
- Tous vos documents et l’ensemble des codes doivent être gérés dans cet outil.  
- Votre responsable (tuteur ou pilote) doit être invité sur votre GIT pour pouvoir suivre vos développements .
- Editeur UML :  Nous préconisons l’utilisation d' ArgoUML ,  Visual Paradigm  ou  Visio.

 Langage, FrameWork  
- Langage C#  
- Bibliothèque Net.Core 3.X   
 Autres :  
- L’ensemble des documents, lignes de codes et commentaires doivent être exploitables par les filiales anglophones.  
- La documentation utilisateur doit tenir en une seule page 
- Release note obligatoire  

 Vous devez conduire ce projet de manière à réduire les coûts de développement des futures versions et surtout d’être capable de réagir rapidement à la remontée éventuelle d’un dysfonctionnement. 

Le logiciel devant être distribué chez les clients,  il est impératif de soigner les IHM. 
Livrables 0 et 1
Livrable 0 :  Environnement de travail et conduite du projet 

Votre équipe doit installer un environnement de travail respectant les contraintes imposées par ProSoft.   

Le bon usage de l’environnement de travail et des contraintes imposées par la direction seront évaluées tout au long du projet. 

Une vigilance particulière sera portée sur la gestion de GIT (versioning) et sur les diagrammes UML. 

 Livrable 1 :  EasySave 1.0 

Le cahier des charges de la première version du logiciel est le suivant :  

Le logiciel est une application Console utilisant .Net Core. 

Le logiciel doit permettre de créer jusqu’à 5 travaux de sauvegarde 

Un travail de sauvegarde est défini par  :
Une appellation
Un répertoire source 
Un répertoire cible 
Un type (complet ou différentiel) 
Le logiciel doit être utilisable par des utilisateurs anglophones. 
L’utilisateur peut demander l’exécution d’un des travaux de sauvegarde ou l’exécution séquentielle  des travaux. 

Les répertoires (sources et cibles) pourront être sur des disques locaux , Externes ou des lecteurs réseaux.

Tous les éléments du répertoire source sont concernés par la sauvegarde. 

Fichier Log journalier : 

Le logiciel doit écrire en temps réel dans un fichier log journalier l’historique des actions des travaux de sauvegarde. Les informations minimales attendues sont : 

- Horodatage   
- Appellation du travail de sauvegarde 
- Adresse complète du fichier Source (format UNC) 
- Adresse complète du fichier de destination (format UNC) 
- Taille du fichier  
- Temps de transfert du fichier en ms (négatif si erreur)   
  

Fichier Etat 
Le logiciel doit enregistrer en temps réel, dans un fichier unique, l’état d’avancement des travaux de sauvegarde.  Les informations minimales attendues sont :   

- Horodatage  
- Appellation du travail de sauvegarde 
- Etat du travail de Sauvegarde (ex : Actif, Non Actif...) 
Si le travail est actif 
     - Le nombre total de fichiers éligibles 
     - La taille des fichiers à transférer  
     - La progression          
     - Nombre de fichiers restants   
     - Taille des fichiers restants   
     - Adresse complète du fichier Source en cours de sauvegarde 
     - Adresse complète du fichier de destination 
 

Les emplacements des deux fichiers décrits ci-dessus (log journalier et état) devront être étudiés pour fonctionner sur les serveurs des clients. De ce fait, les emplacements du type « c:\temp\ » sont à proscrire. 

Les fichiers (log journalier et état) et les éventuels fichiers de configuration seront au format XML ou JSON.  Pour permettre une lecture rapide via Notepad, il est nécessaire de mettre des retours à la ligne entre les éléments XML (ou JSON). Une pagination serait un plus. 

 Remarque importante : si le logiciel donne satisfaction, la direction vous demandera de développer une version 2.0 utilisant une interface graphique (abandon du mode console) 
 Livrable 2 (EasySave 2.0)

EasySave 1.0 a été distribuée chez de nombreux clients.  

Suite à une enquête client, la direction a décidé de créer une version 2.0 dont les améliorations sont les suivantes :  

1) Interface Graphique 

Abandon du mode Console. L’application doit désormais être développée en WPF sous .Net Core 

 2) Nombre de travaux illimités 

Le nombre de travaux de sauvegarde est désormais illimité.  

 3) Crypage via le logiciel CryptoSoft 

Le logiciel devra être capable de crypter les fichiers en utilisant le logiciel CryptoSoft (réalisé durant le prosit 5).  Seuls les fichiers dont les extensions ont été définies par l’utilisateur dans les paramètres généraux devront être cryptés. 

 4) Evolution du fichier Log Journalier 

Le fichier Log journalier doit contenir une information supplémentaire   :  Temps nécessaire au cryptage du fichier (en ms)   

- 0 : pas de cryptage  
- >0 : temps de cryptage (en ms)  
- <0 : code erreur  
 

5) Logiciel Métier 

Si la présence d’un logiciel métier est détectée, le logiciel doit interdire le lancement d’un travail de sauvegarde. Dans le cas de travaux séquentiels, le logiciel doit terminer le travail en cours et s’arrêter avant de lancer le prochain travail. L’utilisateur pourra définir le logiciel métier dans les paramètres généraux du logiciel. (Remarque : l’application calculatrice peut substituer le logiciel métier lors des démonstrations)  

 Remarque : Des clients souhaitent avoir, pour chaque travail de Sauvegarde, une interface permettant d’agir sur celui-ci via trois fonctions (Play, Pause, Stop). Le service commercial a demandé à ce que cette fonction ne soit pas prises en compte dans la version 2.0.  Cependant cette fonction sera dans le cahier des charges de la version 3.0. 
