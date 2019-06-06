# KenticoBoilerplate_v12
Kentico Boilerplate MVC Site which is geared to provide a clean starting point with various Kentico features enabled and examples provided without added fluff.

## Setup Instructions

1. Download the Repo (obviously)
1. Unzip `OriginalDatabase-RestoreMeThenRunCI-Restore.zip` somewhere, and restore that `.bak` into a SQL Server (SQL 2014+)
1. Create 2 local IIS sites:
   1. `dev.kenticoboilerplate.hbs.net` for Kentico (Pointing to the CMS Folder)
   1. `kenticoboilerplate.hbs.net` one for the MVC Site (Pointing to Boilerplate folder)
1. Add the following host entry to your `hosts` file:
   1. `127.0.0.1 dev.kenticoboilerplate.hbs.net kenticoboilerplate.hbs.net` 
1. Unzip `CI-Restore-UnzipAndModifyMe.zip`'s `CI-Restore.bat` file in the root of the solution (next to `CI-Restore.ps1`) 
1. Edit the `CI-Restore.bat` and modify the Path and AppPoolName to match that of your local instance (the App Pool Name should be the one for your Kentico Instance)
1. Go to the CMS folder, and unzip `ConfigFiles_UnzipMe.zip`, place the 3 files in the CMS folder
1. Edit the config files as desired, MINIMUM the `ConnectionStrings.config` needs to be configured to point to your database you restored in step 2, and the `CMSHashStringSalt` in the `AppSettings.config` needs a unique GUID added
1. Run the `CI-Restore.ps1` in the root of this solution, this will update your database with the Continuous Integration changes.  

_*NOTE: Sometimes, depending on your system configuration, powershell scripts can be blocked if they originated from the internet, in which case you can simply copy the content of the powershell, delete the file, then create a new CI-Restore.ps1 and paste in the content, this way it originated from yourself._

## Contribution Rules
If you wish to contribute to the boilerplate, please follow the following rules

1. Keep Example `Controllers`, `Models`, and `Views` with a Prefix of Example, this will make it easier for users who are using the Boilerplate to remove all example material.
1. Please do not install other `NuGet` packages (Except the `RelationshipsExtended`, `RelationshipsExtendedMVCHelper`, and `CSVImport`) without asking me first.
1. Please do not install Kentico Hotfixes, I wish to keep this boilerplate as close to the base 12 as possible.  You should of course hotfix your own sites you may build, even ones starting from the boilerplate, but only hotfix if you have no plans on contributing to the project.
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
