# KenticoBoilerplate_v12 (Hotfixed to Kentico 12.0.29 Service Pack)
Kentico Boilerplate MVC Site which is geared to provide a clean starting point with various Kentico features enabled and examples provided without added fluff.

## Setup Instructions

1. Install Kentico using normal instructions
   1. You can either create a blank MVC Site, or...
   1. You can use the `KenticoBoilerplate_SiteImport_Base.zip` to import the default example pages and page types.
1. Replace the MVC site with the boilerplate site.
1. Go to the CMS folder, and unzip `ConfigFiles_UnzipMe.zip`, place the 3 files in the CMS folder
1. Edit the config files as desired, MINIMUM the `ConnectionStrings.config` needs to be configured to point to your database you restored
1. Edit the `CMSHashStringSalt` in the `AppSettings.config` needs a unique GUID added, do this in both the CMS and Boilerplate folder's AppSettings.config

## If starting your own site from the Boilerplate
1. Replace the .gitignore with the unzipped `EntierCMS-GitIgnore.zip` as this will track all your CMS folders and files if you wish to track your entire repo
	1. _*NOTE: You should not upload the CMS/Lib folder into any public repository, but you can use it for private repos such as Azure DevOps
1. Unzip the `ConfigFilesForCMS_UnzipMe.zip` into your CMS Folder and configure your settings
1. Ensure that your `CMSHashStringSalt` in the App Settings matches for the MVC site and the 'Mother' (CMS)
1. If you wish, you can leverage the given `CI-Restore-UnzipAndModifyMe.zip` to use Continuous Integration
   1. Unzip `CI-Restore-UnzipAndModifyMe.zip`'s `CI-Restore.bat` file in the root of the solution (next to `CI-Restore.ps1`) 
   1. Edit the `CI-Restore.bat` and modify the Path and AppPoolName to match that of your local instance (the App Pool Name should be the one for your Kentico Instance)
   1. Create a backup of your database as the root (other developers will need to start from this point)
   1. You can now run the `CI-Restore.ps1` in the root of this solution to incorporate any CI changes in your repo.

_*NOTE: Sometimes, depending on your system configuration, powershell scripts can be blocked if they originated from the internet, in which case you can simply copy the content of the powershell, delete the file, then create a new CI-Restore.ps1 and paste in the content, this way it originated from yourself._

## Contribution Rules
If you wish to contribute to the boilerplate, please follow the following rules

1. Keep Example `Controllers`, `Models`, and `Views` with a Prefix of Example, this will make it easier for users who are using the Boilerplate to remove all example material.
1. Please do not install other `NuGet` packages (Except the `RelationshipsExtended`, `RelationshipsExtendedMVCHelper`, and `CSVImport`) without asking me first.
1. Please do not install Kentico Hotfixes, I wish to keep this boilerplate as close to the base (K12 SP1) as possible.  You should of course hotfix your own sites you may build, even ones starting from the boilerplate, but only hotfix if you have no plans on contributing to the project.
1. Comment well!
1. Please feel free to include useful generic widgets and inline editors, just make sure they are indeed generic and useable in multiple situations.

## Removal of Examples/Generic Widget Page
The Boilerplate, while trying to be as minimalistic as possible, does contain some examples to help demonstrate the various methods or ways of doing things for those just starting out in MVC.  If you want to remove these things, just do the following:

1. Delete `Boilerplate/KenticoBoilerplate`
1. Delete `Boilerplate/Controllers/Examples`
1. Delete `Boilerplate/Controllers/PageTypes/KMVCHelper_GenericWidgetPageController.cs`
1. Delete `Boilerplate/Models/Examples`
1. Delete `Boilerplate/Models/CMSClasses`
1. Delete `Boilerplate/Views/Examples`
1. Delete `Boilerplate/Views/KMVCHelper_GenericWidgetPage`

If you wish you can also uninstall the NuGet packages `RelationshipsExtendedMVCHelper` if you are not using that module, and update the Kentico packages to whatever hotfix you are currently using (Boilerplate is on version 18).

## More information

For more information, please see me at www.devtrev.com

Sincerely,
   Trevor Fayas - Kentico MVP
