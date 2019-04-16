# KenticoBoilerplate_v12
Kentico Boilerplate MVC Site which is geared to provide a clean starting point with various Kentico features enabled and examples provided without added fluff.

## Setup Instructions

1. Download the Repo (obviously)
2. Unzip OriginalDatabase-RestoreMeThenRunCI-Restore.zip somewhere, and restore that .bak into a SQL Server (SQL 2014+)
3. Create 2 local IIS sites, one for Kentico (Point to the CMS Folder), one for the MVC Site (Point to Boilerplate folder)
4. Set the bindings for the Kentico site to both dev.kenticoboilerplate.hbs.net and admin.kenticoboilerplate.hbs.net
5. Set the bindings for the MVC site to kenticoboilerplate.hbs.net
6. Adjust your Host file to point those 3 domains to your localhost (127.0.0.1)
7. Unzip CI-Restore-UnzipAndModifyMe.zip's CI-Restore.bat file in the root of the solution (next to CI-Restore.ps1) 
8. Edit the CI-Restore.bat and modify the Path and AppPoolName to match that of your local instance (the App Pool Name should be the one for your Kentico Instance)
9. Go to the CMS folder, and unzip ConfigFiles_UnzipMe.zip, place the 3 files in the CMS folder
10. Edit the config files as desired, minimum the ConnectionStrings.config needs to be configured to point to your database you restored in step 2
11. Run the CI-Restore.ps1 in the root of this solution, this will update your database with the Continuous Integration changes

## Contribution Rules
If you wish to contribute to the boilerplate, please follow the following rules

1. Keep Example Controllers, Models, Views, with a Prefix of Example, this will make it easier for users who are using the Boilerplate to remove all example material.
2. Please do not install other NuGet packages (Except the RelationshipsExtended, RelationshipsExtendedMVCHelper, and CSVImport) without asking me first.
3. Please do not install Kentico Hotfixes, I wish to keep this boilerplate as close to the base 12 as possible.  You should of course hotfix your own sites you may build, even ones starting from the boilerplate, but only hotfix if you have no plans on contributing to the project.
4. Comment well!
5. Please feel free to include useful generic widgets and inline editors, just make sure they are indeed generic and useable in multiple situations.

## More information

For more information, please see me at www.devtrev.com

Sincerely,
   Trevor Fayas - Kentico MVP
