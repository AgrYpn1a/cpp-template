#ProjectName and SolutionName varibales
$pName = Read-Host -Prompt 'Please enter Project Name'
$sName = Read-Host -Prompt 'Please enter Solution Name'

if($pName -and $sName) {
    Write-Host "Project Name and Solution Name are successfuly added! :)"
} else {
    Write-Warning -Message "No names added."
}

#This part of script does Creating path, Planting application.cpp in root folder, Changing #ProjectName and #SolutionName in main file
# 

#=========================================================================================================================================

#Use the path from your default project locaton by Visual Studio
New-Item -ItemType "directory" -Path ".\source\$pname\application"

#=========================================================================================================================================

#Files will be copied to this directory
Copy-Item .\templates\cpp.template -Destination .\source\$pname\application -Recurse


#=========================================================================================================================================

#This will rename cpp.template file
Rename-Item -Path ".\source\$pname\application\cpp.template" -NewName "application.cpp"


#=========================================================================================================================================

#This will change strings in tools\Sharpmake\Sharpmake.Build\main.sharpmake.cs
(Get-Content ".\tools\Sharpmake\Sharpmake.Build\main.sharpmake.cs") | ForEach-Object{ $_ -replace "#SolutionName", "$sname" } | Set-Content ".\tools\Sharpmake\Sharpmake.Build\main.sharpmake.cs" -Force

(Get-Content ".\tools\Sharpmake\Sharpmake.Build\main.sharpmake.cs") | ForEach-Object { $_ -replace "#ProjectName", "$pname" } | Set-Content ".\tools\Sharpmake\Sharpmake.Build\main.sharpmake.cs" -Force


#=========================================================================================================================================

#This will remove files like .git .gitignore and \templates directory
Get-ChildItem -Path "." * -Include *.gitignore -Recurse | Remove-Item
Remove-Item '.\templates' -Recurse
Remove-Item '.git' -Recurse


#=========================================================================================================================================

#DEVELOPER NOTES FOR LATER:

#Get-Content ispitati regex pristup? UPDATE: Throws in some sort of pipeline mode...SOLUTION?
#ForEach-Object {  $_ -replace "#ProjectName", "$pname"}


#Kako push na Git i o Git-u :D