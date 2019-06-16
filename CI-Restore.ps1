# Parameter that specifies the path of your Kentico web project folder (i.e. the CMS subfolder)
param (
    [Parameter(Mandatory=$true)]
    [string]$Path,
    [Parameter(Mandatory=$true)]
    [string]$AppPoolName
)

import-module WebAdministration

Stop-WebAppPool $AppPoolName

# Creates an 'App_Offline.htm' file to stop the website
"<html><head></head><body>Continuous integration restore in progress...</body></html>" > "$Path\App_Offline.htm"

# Runs the continuous integration restore utility
& "$Path\bin\ContinuousIntegration.exe" -r

# Removes the 'App_Offline.htm' file to bring the site back online
Remove-Item "$Path\App_Offline.htm"

Start-WebAppPool $AppPoolName